Imports System.IO

Namespace FileManager
    Public Class Template
        Public Shared Function Mail(ByVal name As String)
            Dim existsTemplate As String = String.Format("Resources\TemplateMail\{0}\{1}.html", nsEngine.CompanyCode, name)
            If (Not File.Exists(existsTemplate)) Then existsTemplate = String.Format("Resources\TemplateMail\MOS\{0}.html", name)
            Return nsEngine.FileRead(existsTemplate)
        End Function
        Public Shared Function Others(ByVal name As String)
            Dim existsTemplate As String = String.Format("Resources\TemplateOthers\{0}.txt", name)
            Return nsEngine.FileRead(existsTemplate)
        End Function
        Public Shared Function Voucher(ByVal name As String)
            Return Template.GetPathTemplate("Voucher", name)
        End Function
        Public Shared Function Confirmation(ByVal name As String)
            Return Template.GetPathTemplate("Confirmation", name)
        End Function
        Public Shared Function History(ByVal name As String)
            Return Template.GetPathTemplate("History", name)
        End Function
        Public Shared Function Refund(ByVal name As String)
            Return Template.GetPathTemplate("Refund", name)
        End Function

        Private Shared Function GetPathTemplate(ByVal folder As String, ByVal name As String) As String
            Dim existsTemplate As String = String.Format("Resources\TemplateStored\{0}\{1}\{2}.procedures", folder, nsEngine.CompanyCode, name)
            If (Not File.Exists(nsEngine.Path & existsTemplate)) Then existsTemplate = existsTemplate.Replace("\" & nsEngine.CompanyCode & "\", "\MOS\")
            Return nsEngine.FileRead(existsTemplate)
        End Function

        Public Shared Function Voucher_OldOnReservation(ByVal name As String) As String
            Dim existsTemplate As String = String.Format("Resources\TemplateStored\Voucher\{0}\{1}.procedures", nsEngine.CompanyCode, name)
            If (Not File.Exists(nsEngine.Path & existsTemplate)) Then existsTemplate = existsTemplate.Replace("\" & nsEngine.CompanyCode & "\", "\MOS\")
            Return nsEngine.FileRead(existsTemplate)
        End Function
    End Class
End Namespace