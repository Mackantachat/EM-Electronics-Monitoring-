Imports MySql.Data.MySqlClient
Imports System.Web.UI.WebControls

Public Class myDBQuery
    Private Const _DB_USER As String = "travoxmos"
    Private Const _DB_PASS As String = "systrav"
    Private Const _DB_TIMEOUT As Integer = 30

    Public _MysqlServer As String = "10.0.1.191"
    Public _MysqlDBname As String = "test_scs"
    Public _MysqlUsername As String = "ss"
    Public _MysqlPasswd As String = "ss2015"

    Public Enum Schema
        [GLOBAL]
        [MBOS]
        [HOTEL]
        [TRANSFER]
    End Enum
    Public ReadOnly Property Connected() As Boolean
        Get
            Return Not (connection Is Nothing OrElse connection.State = ConnectionState.Closed)
        End Get
    End Property

    Public connection As MySqlConnection
    Public transection As MySqlTransaction
    Public cmMy As MySqlCommand
    Public drMy As MySqlDataReader
    Public daMy As MySqlDataAdapter
    Public cbMy As MySqlCommandBuilder
    Private EnableExecute As Boolean = False

    Private StoreOutput As String
    Private DataCompany As DataRow

    Private Enum QueryCase
        [INSERT]
        [UPDATE]
        [DELETE]
        [SELECT]
    End Enum

    ReadOnly Property ConnectionString() As String
        Get
            Return String.Format(My.Resources.ConnectionMysql, _MysqlServer, _MysqlUsername, _MysqlPasswd, _MysqlDBname)
        End Get
    End Property

    Public Sub New(Optional ByVal enable As Boolean = True)
        EnableExecute = enable
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
            connection = New MySqlConnection(Me.ConnectionString())

            Do While connection Is Nothing OrElse connection.State = ConnectionState.Closed
                Try
                    connection.Open()
                Catch

                    Threading.Thread.Sleep(5000)
                End Try
            Loop
            transection = connection.BeginTransaction()
        End If
        If (Me.Connected) Then
            Dim command As MySqlCommand = BuildCommands(query, connection, param)
            command.Transaction = transection
            AfterInsertID = command.ExecuteScalar()
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
        Return QueryTable("H2G_TableQuery", query, New ParameterCollection(), sync)
    End Function
    Public Function QueryTable(ByVal query As String, ByVal param As ParameterCollection, Optional ByVal sync As Boolean = False) As DataTable
        Return QueryTable("H2G_TableQuery", query, param, sync)
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
        Dim trans As MySqlTransaction, conn As MySqlConnection
        If Not sync Then
            conn = New MySqlConnection(Me.ConnectionString())
            conn.Open()
            'Do While conn Is Nothing OrElse conn.State = ConnectionState.Closed
            '    Try
            '        conn.Open()
            '    Catch

            '        Threading.Thread.Sleep(5000)
            '    End Try
            'Loop
            trans = conn.BeginTransaction()
        Else
            If (connection Is Nothing OrElse connection.State = ConnectionState.Closed) Then
                connection = New MySqlConnection(Me.ConnectionString())
                transection = connection.BeginTransaction(IsolationLevel.ReadUncommitted)
            End If
            trans = transection
            conn = connection

        End If
        Dim mbosCommand As MySqlCommand = BuildCommands(query, conn, param)
        mbosCommand.Transaction = trans
        mbosCommand.CommandTimeout = 600 '120
        Dim adapter As New MySqlDataAdapter(mbosCommand)
        Try
            adapter.Fill(result)
            If Not sync Then trans.Commit()
        Catch ex As Exception
            trans.Rollback()
            Throw ex
        End Try
        If Not sync Then conn.Close()
        result.TableName = table_name
        Return result
    End Function

    Private Function BuildCommands(ByVal query As String, ByVal mbosConn As MySqlConnection, ByVal param As ParameterCollection) As MySqlCommand
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

        Dim mbosCommand As New MySqlCommand(query, mbosConn)
        mbosCommand.CommandTimeout = _DB_TIMEOUT
        For Each para As Parameter In param
            If (para.Name.Contains("@")) Then
                If (para.Direction <> ParameterDirection.Input) Then
                    mbosCommand.Parameters.Add(para.Name, SqlDbType.NVarChar, 4000)
                Else
                    Dim paramSize As Integer = 0
                    If (para.DefaultValue IsNot Nothing) Then paramSize = para.DefaultValue.Length
                    mbosCommand.Parameters.Add(para.Name, para.DbType, paramSize)
                    mbosCommand.Parameters.Item(para.Name).Size = paramSize
                    mbosCommand.Parameters.Item(para.Name).DbType = para.DbType
                    mbosCommand.Parameters.Item(para.Name).Value = DBNull.Value
                End If
                mbosCommand.Parameters.Item(para.Name).Direction = para.Direction
                Select Case qCase
                    Case QueryCase.SELECT
                        If para.DbType = DbType.Date Or para.DbType = DbType.DateTime Or para.DbType = DbType.DateTime2 Then
                            mbosCommand.Parameters.Item(para.Name).DbType = DbType.String
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
                                If (para.DefaultValue IsNot Nothing) Then
                                    mbosCommand.Parameters.Item(para.Name).Value = nsEngine.Dec(para.DefaultValue)
                                End If
                            Case DbType.Date, DbType.DateTime, DbType.DateTime2
                                mbosCommand.Parameters.Item(para.Name).Size = 18
                                mbosCommand.Parameters.Item(para.Name).DbType = DbType.String
                                If (Not String.IsNullOrEmpty(para.DefaultValue)) Then
                                    mbosCommand.Parameters.Item(para.Name).Value = nsEngine.Convert(Of DateTime)(para.DefaultValue).ToString("yyyy-MM-dd HH:mm:ss")
                                End If
                                query = query.Replace(para.Name, "CONVERT(DATETIME," & para.Name & ", 120)")
                            Case Else
                                If (Not String.IsNullOrEmpty(para.DefaultValue)) Then
                                    mbosCommand.Parameters.Item(para.Name).Value = para.DefaultValue
                                End If
                        End Select
                End Select
            ElseIf (query.Contains(para.Name)) Then
                query = query.Replace("/*" & para.Name & "*/", " " & para.DefaultValue & " ")
            End If
        Next
        If (query.Contains("/*[")) Then query = ChangeSchemaDatabase(query)
        mbosCommand.CommandText = query
        Return mbosCommand
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

    ', Optional ByVal sync As Boolean = False

End Class

Public Class DB
    Public TableName As String
    Public SQL As String
    Public Sub New(ByVal table_name As String)
        TableName = table_name
    End Sub
    Public Sub New(ByVal table_name As String, ByVal sql_query As String)
        'If (sql_query.Contains("SQL::")) Then sql_query = H2G.FileRead("!SQLStore\" & sql_query.Replace("SQL::", "") & ".sql")
        'TableName = table_name
        'SQL = sql_query
    End Sub
End Class