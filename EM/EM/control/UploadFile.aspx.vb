Imports nsEngine
Imports System.IO
Imports System.Data
Imports System.Data.OleDb
Imports System.Drawing
Imports System.Drawing.Imaging

Public Class UploadFile
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call UploadFilepicture(Request.Files("FileUpload"))
    End Sub

    Public Structure JSONObject
        Public Param As Object
        Public exMessage As String
    End Structure

    Private Sub UploadFilepicture(ByVal _File As HttpPostedFile)
        Dim ddMMyyyyHHmmss As String = DateTime.Now.ToString("ddMMyyyyHHmmss")
        Dim subFloder As String = "EM\resources_constant\FileUpload\"

        Dim UploadFile As HttpPostedFile = _File
        Dim UploadDirectory As String = Request.PhysicalApplicationPath & subFloder

        If IsNothing(UploadFile) Then
            Response.Write(JSON.Serialize(Of String)("ไฟล์ที่ไม่ได้อัพโหลด"))
        End If

        Dim file_name As String = Path.GetFileNameWithoutExtension(UploadFile.FileName())
        Dim file_type As String = Path.GetExtension(UploadFile.FileName())

        Dim name_len As Integer = file_name.Length
        IIf(name_len > 100, name_len = 100, name_len = name_len)
        file_name = file_name.Substring(0, name_len)
        Dim FileName As String = file_name & "_" & ddMMyyyyHHmmss & file_type

        If (Not Directory.Exists(UploadDirectory)) Then Directory.CreateDirectory(UploadDirectory)
        Dim FilePath As String = UploadDirectory & FileName
        UploadFile.SaveAs(FilePath)
        Response.Write(JSON.Serialize(Of String)("..\" & subFloder & FileName))
    End Sub
End Class