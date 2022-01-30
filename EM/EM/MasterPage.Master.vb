Public Class MasterPage
    Inherits System.Web.UI.MasterPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Console.WriteLine("Session")
        'If Session("staff_id") IsNot Nothing Then
        '    Response.Redirect("/home/allhome.aspx")
        'Else
        '    Response.Redirect("/default.aspx")
        'End If
    End Sub
End Class