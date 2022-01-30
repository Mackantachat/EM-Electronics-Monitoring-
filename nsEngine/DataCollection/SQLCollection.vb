Imports System.Data.SqlClient
Public Class SQLCollection
    Inherits ParameterCollection

    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal name As String, ByVal value As Object)
        MyBase.New()
        Me.Add(name, value)
    End Sub
    Public Sub New(ByVal name As String, ByVal dbType As Data.DbType, ByVal value As Object)
        MyBase.New()
        Me.Add(name, dbType, value)
    End Sub

    Public Sub ANDBetween(ByVal name As String, ByVal column As String, ByVal value_from As String, ByVal value_to As String)
        Me.Between("AND", name, column, value_from, value_to, False)
    End Sub
    Public Sub ANDBetween(ByVal name As String, ByVal column As String, ByVal value_from As String, ByVal value_to As String, ByVal type As DbType)
        Me.Between("AND", name, column, value_from, value_to, type)
    End Sub

    Private Sub Between(ByVal condition As String, ByVal name As String, ByVal column As String, ByVal value_from As String, ByVal value_to As String, ByVal ignore_null As Boolean)
        Dim sQuery As String = " ISNULL(NULL," & column & ") "
        Dim NullValue As Boolean = False
        If (Not String.IsNullOrEmpty(value_from) Or Not String.IsNullOrEmpty(value_to)) Then
            If (Not String.IsNullOrEmpty(value_from)) Then value_from = "'" & value_from & "'" Else value_from = sQuery
            If (Not String.IsNullOrEmpty(value_to)) Then value_to = "'" & value_to & "'" Else value_to = sQuery
            sQuery = String.Format(" {0} BETWEEN {1} AND {2}", column, value_from, value_to)
        Else
            NullValue = True
            sQuery = String.Format(" {0} IS NOT NULL", column)
        End If
        If (Not NullValue Or Not ignore_null) Then Me.Add(name, DbType.String, condition & sQuery)
    End Sub

    Private Sub Between(ByVal condition As String, ByVal name As String, ByVal column As String, ByVal value_from As String, ByVal value_to As String, ByVal type As DbType)
        Select Case type
            Case DbType.Date, DbType.DateTime, DbType.DateTime2
                column = "CONVERT(CHAR(10)," & column & ",121)"
                If (Not String.IsNullOrEmpty(value_from)) Then value_from = Format(nsEngine.Convert(Of DateTime)(value_from), "yyyy-MM-dd")
                If (Not String.IsNullOrEmpty(value_to)) Then value_to = Format(nsEngine.Convert(Of DateTime)(value_to), "yyyy-MM-dd")
            Case DbType.String

        End Select
        Me.Between(condition, name, column, value_from, value_to, True)
    End Sub

    Public Overloads Sub Add(ByVal Parameter As String, ByVal Dbtype As DbType, ByVal value As Object)
        Me.Add(Parameter, Dbtype, -1, value)
    End Sub

    Public Overloads Sub Add(ByVal Parameter As String, ByVal Dbtype As DbType, ByVal size As Integer, ByVal value As Object)
        Dim strValue As String = ""
        Select Case True
            Case TypeOf value Is DateTime
                strValue = value.ToString("yyyy-MM-dd")
            Case value IsNot Nothing
                strValue = value.ToString
            Case Else

        End Select

        If value IsNot Nothing AndAlso size > 0 AndAlso size < strValue.Length Then
            strValue = strValue.Substring(0, size)
        End If

        Me.Add(Parameter, Dbtype, strValue)
    End Sub
    Public Sub AddIIfElse(ByVal Parameter As String, ByVal Format As String, ByVal CaseTrue As String, ByVal CaseFalse As String)
        Me.Add(Parameter, String.Format(Format, IIf(SQLCollection.IsNotALL(CaseTrue), CaseTrue, CaseFalse)))
    End Sub

    Public Sub AddIIf(ByVal Parameter As String, ByVal Format As String, ByVal CaseTrue As String)
        If SQLCollection.IsNotALL(CaseTrue) Then Me.Add(Parameter, String.Format(Format, CaseTrue))
    End Sub

    Public Shared Function IsNotALL(ByVal parameter As String) As Boolean
        Return Not (String.IsNullOrEmpty(parameter) OrElse parameter = "ALL")
    End Function
End Class