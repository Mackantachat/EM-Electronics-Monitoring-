<DataContract()> _
Public Class CallbackException
    'Inherits Exception
    <DataMember()> _
    Public onError As Boolean = True
    <DataMember()> _
    Public exTitle As String = ""
    <DataMember()> _
    Public exMessage As String = ""
    <DataMember()>
    Private getItems As String = "[]"

    Public Sub New()
        exTitle = "Successful"
        exMessage = ""
    End Sub
    Public Sub New(ByVal ex As Exception)
        exMessage = JSON.FixString(ex.Message)
        If ex.InnerException Is Nothing Then
            exTitle = "CODING ERROR"
        Else
            exTitle = exMessage
            exMessage = JSON.FixString(ex.InnerException.Message)
        End If
    End Sub
    Public Sub New(ByVal message As String)
        onError = False
        exTitle = "Successful"
        exMessage = message
    End Sub
    Public Sub New(ByVal title As String, ByVal message As String)
        exTitle = title
        exMessage = message
    End Sub

    Public Sub setItems(Of T)(ByVal v As T)
        onError = False
        getItems = JSON.Serialize(Of T)(v)
    End Sub

    Public Sub setItems(ByVal value As String)
        onError = False
        getItems = value
    End Sub

    Public Function ToJSON() As String
        Return JSON.Serialize(Of CallbackException)(Me)
    End Function

    Public Overrides Function ToString() As String
        Return exTitle & " >> " & exMessage & vbCrLf & getItems
    End Function

End Class