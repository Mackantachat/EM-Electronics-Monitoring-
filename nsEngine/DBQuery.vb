Imports System.Data.SqlClient
Imports System.Collections.Generic

Public Class DBQuery
    Private Const _DB_SERVER As String = "10.0.1.93"
    Private Const _DB_NAME As String = "nippon_em"
    Private Const _DB_USER As String = "em"
    Private Const _DB_PASS As String = "N!pem"
    Private Const _DB_TIMEOUT As Integer = 40
    Private Const _DB_TIMEOUT_APP As Integer = 30

    Private Const _DB_Test As Boolean = False

    Public Enum Schema
        [OMIC]
    End Enum
    Public ReadOnly Property Connected() As Boolean
        Get
            Return Not (connection Is Nothing OrElse connection.State = ConnectionState.Closed)
        End Get
    End Property

    Private StoreOutput As String
    Private DataCompany As DataRow
    Protected transection As SqlTransaction
    Protected connection As SqlConnection

    ReadOnly Property getDatabaseName() As String
        Get
            Return _DB_NAME
        End Get
    End Property
    Private Enum QueryCase
        [INSERT]
        [UPDATE]
        [DELETE]
        [SELECT]
    End Enum
    ReadOnly Property ConnectionString() As String
        Get
            Return String.Format(My.Resources.Connection, _DB_SERVER, _DB_NAME, _DB_USER, _DB_PASS)
        End Get
    End Property
    Public Sub New()

    End Sub
    Protected Overrides Sub Finalize()
        Try
            connection.Close()
        Catch
        End Try
        MyBase.Finalize()
    End Sub
    Public Function Execute(ByVal query As String) As String
        Return Execute(query, New ParameterCollection())
    End Function
    Public Function Execute(ByVal query As String, ByVal param As ParameterCollection) As String
        Dim AfterInsertID As String = ""
        If (connection Is Nothing OrElse connection.State = ConnectionState.Closed) Then
            connection = New SqlConnection(Me.ConnectionString())
            connection.Open()
            transection = connection.BeginTransaction()
        End If
        If (Me.Connected) Then
            Dim command As SqlCommand = BuildCommands(query, connection, param)
            command.Transaction = transection
            Try
                AfterInsertID = command.ExecuteScalar()
            Catch ex As Exception
                transection.Rollback()
                If IsApplication() AndAlso ex.Message.Contains("Timeout Expired.") Then
                    Throw New Exception("Execution Timeout Expired. -=Query=- " & command.CommandText, ex)
                Else
                    Throw ex
                End If
            End Try
        End If
        Return AfterInsertID
    End Function
    Public Function Apply() As Boolean
        Dim result As Boolean = False
        Try
            If (Me.Connected) Then
                transection.Commit()
                connection.Close()
                result = True
            End If
        Catch ex As Exception
            transection.Rollback()
            connection.Close()
            Throw New Exception(ex.Message())
        End Try
        Return result
    End Function
    Public Sub Rollback()
        Try
            transection.Rollback()
            connection.Close()
        Catch
        End Try
    End Sub
    Public Function QueryField(ByVal query As String, Optional ByVal sync As Boolean = False) As String
        Return QueryField(query, New ParameterCollection(), "", sync)
    End Function
    Public Function QueryField(ByVal query As String, ByVal _default As String, Optional ByVal sync As Boolean = False) As String
        Return QueryField(query, New ParameterCollection(), _default, sync)
    End Function
    Public Function QueryField(ByVal query As String, ByVal param As ParameterCollection, Optional ByVal sync As Boolean = False) As String
        Return QueryField(query, param, "", sync)
    End Function
    Public Function QueryField(ByVal query As String, ByVal param As ParameterCollection, ByVal _default As String, Optional ByVal sync As Boolean = False) As String
        Dim dtQuery As DataTable = QueryTable(query, param, sync)
        Dim result As String = _default
        If (dtQuery.Columns.Count >= 1 And dtQuery.Rows.Count >= 1) Then
            result = dtQuery.Rows(0)(0).ToString()
        End If
        Return result
    End Function

    Public Function QueryTable(ByVal query As String, Optional ByVal sync As Boolean = False) As DataTable
        Return QueryTable("TableQuery", query, New ParameterCollection(), sync)
    End Function
    Public Function QueryTable(ByVal query As String, ByVal param As ParameterCollection, Optional ByVal sync As Boolean = False) As DataTable
        Return QueryTable("TableQuery", query, param, sync)
    End Function
    Public Function QueryTable(ByVal db As DB, Optional ByVal sync As Boolean = False) As DataTable
        Return QueryTable(db.TableName, db.SQL, New ParameterCollection(), sync)
    End Function
    Public Function QueryTable(ByVal db As DB, ByVal param As ParameterCollection, Optional ByVal sync As Boolean = False) As DataTable
        Return QueryTable(db.TableName, db.SQL, param, sync)
    End Function
    Public Function QueryTable(ByVal table_name As String, ByVal query As String, Optional ByVal sync As Boolean = False) As DataTable
        Return QueryTable(table_name, query, New ParameterCollection(), sync)
    End Function
    Public Function QueryTable(ByVal table_name As String, ByVal query As String, ByVal param As ParameterCollection, Optional ByVal sync As Boolean = False) As DataTable
        Dim result As New DataTable
        Dim trans As SqlTransaction, conn As SqlConnection
        If Not sync Then
            'not sync
            conn = New SqlConnection(Me.ConnectionString())
            If Not IsApplication() Then
                conn.Open()
            Else
                Do While conn Is Nothing OrElse conn.State = ConnectionState.Closed
                    Try
                        conn.Open()
                    Catch
                        Console.ForegroundColor = ConsoleColor.White
                        Console.Write("[" & Now.ToString("HH:mm:ss") & "]   ")
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.Write("Database cannot connect, Id {0} Waiting...")
                        Threading.Thread.Sleep(5000)
                    End Try
                Loop
            End If
            trans = conn.BeginTransaction(IsolationLevel.ReadUncommitted) '
        Else
            'sync
            If (connection Is Nothing OrElse connection.State = ConnectionState.Closed) Then
                connection = New SqlConnection(Me.ConnectionString())
                If Not IsApplication() Then
                    connection.Open()
                Else
                    Do While connection Is Nothing OrElse connection.State = ConnectionState.Closed
                        Try
                            connection.Open()
                        Catch
                            Console.ForegroundColor = ConsoleColor.White
                            Console.Write("[" & Now.ToString("HH:mm:ss") & "]   ")
                            Console.ForegroundColor = ConsoleColor.Red
                            Console.Write("Database cannot connect, Id {0} Waiting...")
                            Threading.Thread.Sleep(5000)
                        End Try
                    Loop
                End If
                transection = connection.BeginTransaction(IsolationLevel.ReadUncommitted) '
            End If
            trans = transection
            conn = connection
        End If
        Dim command As SqlCommand = BuildCommands(query, conn, param)
        command.Transaction = trans
        Dim adapter As New SqlDataAdapter(command)
        Try
            adapter.Fill(result)
            If Not sync Then trans.Commit()
        Catch ex As Exception
            If IsApplication() AndAlso ex.Message.Contains("Timeout Expired.") Then
                Throw New Exception("Execution Timeout Expired. -=Query=- " & command.CommandText, ex)
            Else
                Throw ex
            End If
        End Try
        If Not sync Then
            If (conn.State = ConnectionState.Open) Then conn.Close()
        End If
        result.TableName = table_name
        Return result
    End Function
    Public Function StoredParamOne(ByVal store_name As String, ByVal schema_name As Schema, ByVal param As ParameterCollection) As String
        param = StoredParam(store_name, schema_name, param)
        Dim result As String = StoreOutput
        StoreOutput = Nothing
        If (Not String.IsNullOrEmpty(result)) Then result = param.Item(result).DefaultValue.ToString()
        Return result
    End Function
    Public Function StoredParam(ByVal store_name As String, ByVal schema_type As Schema, ByVal param As ParameterCollection) As ParameterCollection
        Dim NowConnection As Boolean = False
        Dim schema_name As String = "/*[MBOS]*/"
        Dim mbosCommand As New SqlCommand
        Select Case schema_type
            Case Schema.OMIC : schema_name = "/*[OMIC]*/"
        End Select

        If (Not Me.Connected) Then
            connection = New SqlConnection(Me.ConnectionString())
            connection.Open()
            NowConnection = True
        End If
        mbosCommand = BuildCommands(schema_name & "[" & store_name.Trim() & "]", connection, param)
        Dim adapter As New SqlDataAdapter(mbosCommand)
        If (Not NowConnection) Then mbosCommand.Transaction = transection
        mbosCommand.CommandType = CommandType.StoredProcedure
        mbosCommand.Connection = connection
        mbosCommand.ExecuteNonQuery()
        If (NowConnection) Then connection.Close()

        For Each para As SqlParameter In mbosCommand.Parameters
            If (para.Direction <> ParameterDirection.Input) Then
                'If (para.Value Is DBNull.Value) Then Throw New Exception("System Stored Procedures Value Return is NULL")
                param.Item(para.ParameterName).DefaultValue = IIf(para.Value Is DBNull.Value, "", para.Value)
                param.Item(para.ParameterName).DbType = para.DbType
                StoreOutput = para.ParameterName
            End If
        Next

        Return param
    End Function
    Private Function BuildCommands(ByVal query As String, ByVal mbosConn As SqlConnection, ByVal param As ParameterCollection) As SqlCommand
        Dim qCase As QueryCase = QueryCase.SELECT
        Try
            If (query.Contains("SQL::")) Then query = nsEngine.FileRead("!SQLStore\" & query.Replace("SQL::", "") & ".sql")
            If (query.ToLower().Contains("insert into") And query.ToLower().Contains("values")) Then
                qCase = QueryCase.INSERT
                query &= " SELECT @@IDENTITY"
            ElseIf (query.ToLower().Contains("update") And query.ToLower().Contains("set")) Then
                qCase = QueryCase.UPDATE
            ElseIf (query.ToLower().Contains("delete") And query.ToLower().Contains("from")) Then
                qCase = QueryCase.DELETE
            End If
        Catch
            Throw New Exception("SQL Query file", New Exception("SQL query file path is not exists."))
        End Try

        Dim command As New SqlCommand(query, mbosConn)
        command.CommandTimeout = _DB_TIMEOUT
        If IsApplication() Then command.CommandTimeout = _DB_TIMEOUT_APP
        For Each para As Parameter In param
            If (para.Name.Contains("@")) Then
                If (para.Direction <> ParameterDirection.Input) Then
                    command.Parameters.Add(para.Name, SqlDbType.NVarChar, 4000)
                Else
                    Dim paramSize As Integer = 0
                    If (para.DefaultValue IsNot Nothing) Then paramSize = para.DefaultValue.Length
                    command.Parameters.Add(para.Name, para.DbType, paramSize)
                    command.Parameters.Item(para.Name).Size = paramSize
                    command.Parameters.Item(para.Name).DbType = para.DbType
                    command.Parameters.Item(para.Name).Value = DBNull.Value
                End If
                command.Parameters.Item(para.Name).Direction = para.Direction
                Select Case qCase
                    Case QueryCase.SELECT
                        If para.DbType = DbType.Date Or para.DbType = DbType.DateTime Or para.DbType = DbType.DateTime2 Then
                            command.Parameters.Item(para.Name).DbType = DbType.String
                            command.Parameters.Item(para.Name).Value = nsEngine.Convert(Of DateTime)(para.DefaultValue).ToString("yyyy-MM-dd HH:mm:ss")
                        ElseIf ((para.DbType = DbType.String And para.DefaultValue IsNot Nothing) Or Not String.IsNullOrEmpty(para.DefaultValue)) Then
                            command.Parameters.Item(para.Name).Value = para.DefaultValue
                        End If
                    Case Else
                        Select Case para.DbType
                            Case DbType.String
                                If (para.DefaultValue IsNot Nothing) Then
                                    command.Parameters.Item(para.Name).Value = para.DefaultValue
                                Else
                                    command.Parameters.Item(para.Name).DbType = DbType.String
                                    command.Parameters.Item(para.Name).Value = DBNull.Value
                                End If
                            Case DbType.Decimal
                                If (Not String.IsNullOrEmpty(para.DefaultValue)) Then
                                    command.Parameters.Item(para.Name).Value = nsEngine.Dec(para.DefaultValue)
                                Else
                                    command.Parameters.Item(para.Name).DbType = DbType.Decimal
                                    command.Parameters.Item(para.Name).Value = DBNull.Value
                                End If
                            Case DbType.Date, DbType.DateTime, DbType.DateTime2
                                command.Parameters.Item(para.Name).Size = 18
                                command.Parameters.Item(para.Name).DbType = DbType.String
                                If (Not String.IsNullOrEmpty(para.DefaultValue)) Then
                                    command.Parameters.Item(para.Name).Value = nsEngine.Convert(Of DateTime)(para.DefaultValue).ToString("yyyy-MM-dd HH:mm:ss")
                                    query = query.Replace(para.Name, "CONVERT(DATETIME," & para.Name & ", 120)")
                                Else
                                    query = query.Replace(para.Name, "null") ' HH24MISS
                                End If
                            Case DbType.Int16, DbType.Int32, DbType.Int64
                                If (Not String.IsNullOrEmpty(para.DefaultValue)) Then
                                    command.Parameters.Item(para.Name).Value = nsEngine.Int(para.DefaultValue)
                                Else
                                    command.Parameters.Item(para.Name).DbType = para.DbType
                                    command.Parameters.Item(para.Name).Value = DBNull.Value
                                End If

                            Case Else
                                If (Not String.IsNullOrEmpty(para.DefaultValue)) Then
                                    command.Parameters.Item(para.Name).Value = para.DefaultValue
                                End If
                        End Select
                End Select
            ElseIf (query.Contains(para.Name)) Then
                query = query.Replace("/*" & para.Name & "*/", " " & para.DefaultValue & " ")
            End If
        Next
        'If (query.Contains("/*[")) Then query = ChangeSchemaDatabase(query)
        command.CommandText = query
        Return command
    End Function
    Private Function ChangeSchemaDatabase(ByVal query As String) As String
        Dim schema As String = ".travoxmos."
        Dim arrSchema() As String = {"GLOBAL", "MBOS", "HOTEL", "TRANSFER"}
        For Each name As String In arrSchema
            If (query.Contains("/*[" & name & "]*/")) Then
                Select Case name
                    Case "GLOBAL" : schema = "travox_global" & schema
                    Case "MBOS" : schema = DataCompany("database_name").ToString & schema
                    Case Else : schema = DataCompany("database_" & name.ToLower).ToString & schema
                End Select
                query = query.Replace("/*[" & name & "]*/", " " & schema).Trim()
            End If
        Next
        Return query
    End Function
    Private Function IsApplication() As Boolean
        If HttpContext.Current Is Nothing Then
            Return True
        Else
            Return False
        End If
    End Function
End Class