Imports EM
Imports nsEngine
Imports System.IO
Imports System.Net
Imports System.Text


Public Class LineNotifications
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
            Case "LineNoti" : JSONResponse.setItems(Of LineItem)(Me.BtnTestSend_Click())
            Case Else
                Dim exMsg As String = IIf(String.IsNullOrEmpty(_REQUEST("action")), "", _REQUEST("action"))
                Throw New Exception("Not found action " & exMsg & ".", New Exception("Please check your action name"))
        End Select
        Return JSONResponse.ToJSON()
    End Function

    Private Function BtnTestSend_Click() As LineItem
        Throw New NotImplementedException()
    End Function

    Private Sub BtnTestSend_Click(sender As Object, e As System.EventArgs) Handles Me.Load
        System.Net.ServicePointManager.Expect100Continue = False
        Dim request = DirectCast(WebRequest.Create("https://notify-api.line.me/api/notify"), HttpWebRequest)
        Dim postData = String.Format("message={0}", _REQUEST("line_text"))
        Dim data = Encoding.UTF8.GetBytes(postData)
        request.Method = "POST"
        request.ContentType = "application/x-www-form-urlencoded"
        request.ContentLength = data.Length
        request.Headers.Add("Authorization", "Bearer buzmg5IdEuptgDA3JN7WRvREoz9wqi0cu5SxorPRaZu")
        request.AllowWriteStreamBuffering = True
        request.KeepAlive = False
        request.Credentials = CredentialCache.DefaultCredentials
        Using stream = request.GetRequestStream()
            stream.Write(data, 0, data.Length)
        End Using

        Dim response = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim responseString = New StreamReader(response.GetResponseStream()).ReadToEnd()
    End Sub

    Public Structure LineItem
        Public line_text As String
    End Structure

End Class