'Imports System.Data.SqlClient
'Imports Oracle.ManagedDataAccess.Client
'Imports System.Data.OleDb
Imports Oracle.ManagedDataAccess.Client
Imports System.Collections.Generic

Public Class oraDBQuery
    Private Const _DB_USER As String = "claret"
    Private Const _DB_PASS As String = "g8iupf" '"nbc1234" '
    Private Const _DB_SERVER As String = "10.0.2.172" '"10.0.2.172" '
    Private Const _DB_CATALOG As String = "claret"
    Private Const _DB_PORT As String = "1521"
    Private Const _DB_SID As String = "CLARET" '"HIIG" '
    Private Const _DB_TIMEOUT As Integer = 30

    Public ReadOnly Property Connected() As Boolean
        Get
            Return Not (connection Is Nothing OrElse connection.State = ConnectionState.Closed)
        End Get
    End Property

    'Private DataCompany As DataRow
    Private StoreOutput As String
    Protected transection As OracleTransaction
    Protected connection As OracleConnection
    Private EnabledExecute As Boolean = True

    Private Enum QueryCase
        [INSERT]
        [UPDATE]
        [DELETE]
        [SELECT]
    End Enum
    ReadOnly Property ConnectionString() As String
        Get
            Return String.Format(My.Resources.ConnectionOracle, _DB_SERVER, _DB_PORT, _DB_USER, _DB_PASS, _DB_SID)
        End Get
    End Property
    Public Sub New(Optional ByVal enable As Boolean = True)
        EnabledExecute = enable
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
        'If EnabledExecute Then
        If (connection Is Nothing OrElse connection.State = ConnectionState.Closed) Then
            connection = New OracleConnection(Me.ConnectionString())
            connection.Open()
            transection = connection.BeginTransaction()
        End If
        If (Me.Connected) Then
            Dim command As OracleCommand = BuildCommands(query, connection, param)
            command.Transaction = transection
            AfterInsertID = command.ExecuteScalar()
        End If
        'End If

        Return AfterInsertID
    End Function
    Public Function Apply() As Boolean
        Dim result As Boolean = False
        If EnabledExecute Then
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
        Else
            result = True
        End If

        Return result
    End Function
    Public Sub Rollback()
        If EnabledExecute Then
            Try
                transection.Rollback()
                connection.Close()
            Catch
            End Try
        End If
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
        Dim result As String = _default
        If EnabledExecute Then
            Dim dtQuery As DataTable = QueryTable(query, param, sync)
            If (dtQuery.Columns.Count >= 1 And dtQuery.Rows.Count >= 1) Then
                result = dtQuery.Rows(0)(0).ToString()
            End If
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
        'If EnabledExecute Then
        Dim trans As OracleTransaction, conn As OracleConnection '(Me.ConnectionString())
        If Not sync Then
            conn = New OracleConnection(Me.ConnectionString())
            conn.Open()
            trans = conn.BeginTransaction()
        Else
            If (connection Is Nothing OrElse connection.State = ConnectionState.Closed) Then
                connection = New OracleConnection(Me.ConnectionString())
                connection.Open()
                transection = connection.BeginTransaction()
            End If
            trans = transection
            conn = connection

        End If
        Dim mbosCommand As OracleCommand = BuildCommands(query, conn, param)

        mbosCommand.Transaction = trans
        mbosCommand.CommandTimeout = 120
        Dim adapter As New OracleDataAdapter(mbosCommand)
        Try
            adapter.Fill(result)
            If Not sync Then trans.Commit()
        Catch ex As Exception
            trans.Rollback()
            Throw ex
        End Try
        If Not sync Then
            If (conn.State = ConnectionState.Open) Then conn.Close()
        End If
        result.TableName = table_name
        'End If

        Return result
    End Function
    Public Function StoredParamOne(ByVal store_name As String, ByVal param As ParameterCollection) As String
        Dim result As String = StoreOutput

        If EnabledExecute Then
            param = StoredParam(store_name, param)
            StoreOutput = Nothing
            If (Not String.IsNullOrEmpty(result)) Then result = param.Item(result).DefaultValue.ToString()
        End If

        Return result
    End Function
    Public Function StoredParam(ByVal store_name As String, ByVal param As ParameterCollection) As ParameterCollection
        If EnabledExecute Then
            Dim NowConnection As Boolean = False
            Dim mbosCommand As New OracleCommand

            If (Not Me.Connected) Then
                connection = New OracleConnection(Me.ConnectionString())
                connection.Open()
                NowConnection = True
            End If
            mbosCommand = BuildCommands("[" & store_name.Trim() & "]", connection, param)
            Dim adapter As New OracleDataAdapter(mbosCommand)
            If (Not NowConnection) Then mbosCommand.Transaction = transection
            mbosCommand.CommandType = CommandType.StoredProcedure
            mbosCommand.Connection = connection
            mbosCommand.ExecuteNonQuery()
            If (NowConnection) Then connection.Close()

            For Each para As OracleParameter In mbosCommand.Parameters
                If (para.Direction <> ParameterDirection.Input) Then
                    param.Item(para.ParameterName).DefaultValue = IIf(para.Value Is DBNull.Value, "", para.Value)
                    param.Item(para.ParameterName).DbType = para.DbType
                    StoreOutput = para.ParameterName
                End If
            Next
        End If

        Return param
    End Function
    Private Function BuildCommands(ByVal query As String, ByVal mbosConn As OracleConnection, ByVal param As ParameterCollection) As OracleCommand
        Dim qCase As QueryCase = QueryCase.SELECT
        Try
            If (query.Contains("SQL::")) Then query = nsEngine.FileRead("!SQLStore\" & query.Replace("SQL::", "") & ".sql")
            If (query.ToLower().Contains("insert into") And query.ToLower().Contains("values")) Then
                qCase = QueryCase.INSERT
                'query &= " SELECT @@IDENTITY"
            ElseIf (query.ToLower().Contains("update") And query.ToLower().Contains("set")) Then
                qCase = QueryCase.UPDATE
            ElseIf (query.ToLower().Contains("delete") And query.ToLower().Contains("from")) Then
                qCase = QueryCase.DELETE
            End If
        Catch
            Throw New Exception("SQL Query file", New Exception("SQL query file path is not exists."))
        End Try

        Dim mbosCommand As New OracleCommand(query, mbosConn)
        mbosCommand.CommandTimeout = _DB_TIMEOUT
        For Each para As Match In Regex.Matches(query, "\:\w+", RegexOptions.IgnoreCase)
            Dim paramSize As Integer = 0
            mbosCommand.Parameters.Add(para.Value, OracleDbType.NChar, paramSize)
            mbosCommand.Parameters.Item(para.Value).Size = paramSize
            mbosCommand.Parameters.Item(para.Value).Value = DBNull.Value
            mbosCommand.Parameters.Item(para.Value).Direction = ParameterDirection.Input
        Next


        For Each para As Parameter In param
            Dim foundParam As Boolean = False
            For Each gen As OracleParameter In mbosCommand.Parameters
                If gen.ParameterName = para.Name Then
                    foundParam = True
                    Exit For
                End If
            Next
            If (Not para.Name.Contains("#") And foundParam) Then
                Select Case qCase
                    Case QueryCase.SELECT
                        If para.DbType = DbType.Date Or para.DbType = DbType.DateTime Or para.DbType = DbType.DateTime2 Then
                            mbosCommand.Parameters.Item(para.Name).OracleDbType = OracleDbType.Char
                            mbosCommand.Parameters.Item(para.Name).Value = nsEngine.Convert(Of DateTime)(para.DefaultValue).ToString("yyyy-MM-dd HH:mm:ss")
                        ElseIf ((para.DbType = DbType.String And para.DefaultValue IsNot Nothing) Or Not String.IsNullOrEmpty(para.DefaultValue)) Then
                            mbosCommand.Parameters.Item(para.Name).Value = para.DefaultValue
                        End If
                    Case Else
                        Select Case para.DbType
                            Case DbType.String
                                If (para.DefaultValue IsNot Nothing) Then
                                    mbosCommand.Parameters.Item(para.Name).Value = para.DefaultValue
                                End If
                            Case DbType.Decimal
                                If (Not String.IsNullOrEmpty(para.DefaultValue)) Then
                                    mbosCommand.Parameters.Item(para.Name).OracleDbType = OracleDbType.Decimal
                                    mbosCommand.Parameters.Item(para.Name).Value = nsEngine.Dec(para.DefaultValue)
                                Else
                                    mbosCommand.Parameters.Item(para.Name).OracleDbType = OracleDbType.Decimal
                                    mbosCommand.Parameters.Item(para.Name).Value = Nothing
                                End If
                            Case DbType.Int16, DbType.Int32, DbType.Int64, DbType.VarNumeric
                                If (Not String.IsNullOrEmpty(para.DefaultValue)) Then
                                    mbosCommand.Parameters.Item(para.Name).OracleDbType = OracleDbType.Int64
                                    mbosCommand.Parameters.Item(para.Name).Value = nsEngine.Int(para.DefaultValue)
                                Else
                                    mbosCommand.Parameters.Item(para.Name).OracleDbType = OracleDbType.Int64
                                    mbosCommand.Parameters.Item(para.Name).Value = Nothing
                                End If
                            Case DbType.Date, DbType.DateTime, DbType.DateTime2
                                If (Not String.IsNullOrEmpty(para.DefaultValue)) Then
                                    query = query.Replace(para.Name, "to_date('" & nsEngine.Convert(Of DateTime)(para.DefaultValue).ToString("ddMMyyyy HHmmss") & "','DDMMYYYY HH24MISS')") ' HH24MISS
                                Else
                                    query = query.Replace(para.Name, "null") ' HH24MISS
                                End If
                                mbosCommand.Parameters.RemoveAt(para.Name)

                            Case Else
                                If (Not String.IsNullOrEmpty(para.DefaultValue)) Then
                                    mbosCommand.Parameters.Item(para.Name).Value = para.DefaultValue
                                End If
                        End Select '
                End Select
            ElseIf (query.Contains(para.Name)) Then
                query = query.Replace("/*" & para.Name & "*/", " " & para.DefaultValue & " ")
            End If
        Next
        'If (query.Contains("/*[")) Then query = ChangeSchemaDatabase(query)
        mbosCommand.CommandText = query
        Return mbosCommand
    End Function
    Private Function ChangeSchemaDatabase(ByVal query As String) As String
        Dim schema As String = ".dbo."
        Dim arrSchema() As String = {"NEW", "OLD"}
        For Each name As String In arrSchema
            If (query.Contains("/*[" & name & "]*/")) Then
                schema = ".dbo."
                Select Case name
                    Case "NEW" : schema = "claret" & schema
                    Case "OLD" : schema = "hematos" & schema
                    Case Else : schema = "claret" & schema
                End Select
                query = query.Replace("/*[" & name & "]*/", " " & schema).Trim()
            End If
        Next
        Return query
    End Function

End Class
