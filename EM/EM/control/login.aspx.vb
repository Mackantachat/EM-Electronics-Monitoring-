Imports nsEngine

Public Class login
    Inherits UI.Page
    Dim JSONResponse As New CallbackException()
    Dim param As New SQLCollection()
    Dim conn As New DBQuery
    Dim stmt As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Response.Write(Action())
        Catch ex As Exception
            conn.Rollback()
            JSONResponse = New CallbackException(ex)
            Response.Write(JSONResponse.ToJSON())
        Finally
            conn.Apply()
        End Try
    End Sub

    Private Function Action() As String
        Select Case _REQUEST("action")
            Case "login" : JSONResponse.setItems(Of DataItem)(Me.login())
            Case Else
                Dim exMsg As String = IIf(String.IsNullOrEmpty(_REQUEST("action")), "", _REQUEST("action"))
                Throw New Exception("Not found action [" & exMsg & "].", New Exception("Please check your action name"))
        End Select
        Return JSONResponse.ToJSON()
    End Function

    Private Function login() As DataItem
        Dim StaffItem As New DataItem
        stmt = "select Top 1 id, name 
                        from User_Account with(nolock)
                        where user_name = @username COLLATE SQL_Latin1_General_CP1_CS_AS
                        and password = @password COLLATE SQL_Latin1_General_CP1_CS_AS
                        and status = 'Active'"

        param = New SQLCollection
        param.Add("@username", DbType.String, _REQUEST("username"))
        param.Add("@password", DbType.String, _REQUEST("password"))

        Dim dt As DataTable = conn.QueryTable(stmt, param)
        If dt.Rows.Count > 0 Then
            StaffItem.status = True
            StaffItem.id = dt(0)("id").ToString()
            StaffItem.name = dt(0)("name").ToString()
        Else
            StaffItem.status = False
            StaffItem.text_alert = "กรุณาตรวจสอบชื่อบัญชีผู้ใช้และรหัสผ่าน"
        End If
        Return StaffItem
    End Function

    Public Structure DataItem
        Public id As Integer
        Public name As String
        Public status As Boolean
        Public text_alert As String
    End Structure
End Class