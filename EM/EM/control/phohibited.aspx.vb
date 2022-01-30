Imports nsEngine

Public Class phohibited
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
            Case "group" : JSONResponse.setItems(Of GroupItem)(Me.Group())
            Case "area" : JSONResponse.setItems(Of AllAreaItem)(Me.GetArea())
            Case "save_area" : JSONResponse.setItems(Of DataItem)(Me.SaveArea())
            Case "save_group" : JSONResponse.setItems(Of DataItem)(Me.SaveGroup())
            Case "list_area" : JSONResponse.setItems(Of List(Of AreaItem))(Me.ListArea("Y"))
            Case "list_group" : JSONResponse.setItems(Of List(Of GroupItem))(Me.ListGroup("Y"))
            Case "get_group" : JSONResponse.setItems(Of List(Of GroupItem))(Me.ListGroup("N"))
            Case "status_area" : JSONResponse.setItems(Of DataItem)(Me.ChangeStatus("Restricted_Area"))
            Case "status_group" : JSONResponse.setItems(Of DataItem)(Me.ChangeStatus("Area_Group"))
            Case Else
                Dim exMsg As String = IIf(String.IsNullOrEmpty(_REQUEST("action")), "", _REQUEST("action"))
                Throw New Exception("Not found action " & exMsg & ".", New Exception("Please check your action name"))
        End Select
        Return JSONResponse.ToJSON()
    End Function

    Private Function GetArea() As AllAreaItem
        Dim data As New AllAreaItem
        data.area = Me.Area()
        data.area_details = Me.Area_Detail()
        Return data
    End Function

    Private Function Area_Detail() As List(Of AreaDetailItem)
        Dim list_detail As New List(Of AreaDetailItem)
        Dim detail As AreaDetailItem

        stmt = "select restricted_area_id, restrict_day, time_start, time_end 
                from Restricted_AreaDetails with(nolock)
                where restricted_area_id = @id"

        param = New SQLCollection
        param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("area_id"))))
        Dim item As DataTable = conn.QueryTable(stmt, param)
        If item.Rows.Count > 0 Then
            For Each data As DataRow In item.Rows()
                detail = New AreaDetailItem
                detail.data_area = data("restricted_area_id").ToString()
                detail.data_day = data("restrict_day").ToString()
                detail.data_start = data("time_start").ToString()
                detail.data_end = data("time_end").ToString()
                list_detail.Add(detail)
            Next
        End If
        Return list_detail
    End Function

    Private Function Area() As AreaItem
        Dim item As New AreaItem
        stmt = "select  id, 
                        restricted_name, 
                        area_group_id, 
                        location, 
                        month1 +','+ month2 +','+ month3 +','+ month4 +','+ month5 +','+ month6 +','+ month7 +','+ month8 +','+ month9 +','+ month10 +','+ month11  +','+ month12 AS month, 
                        status
                from Restricted_Area with(nolock) 
                where id = @id
                order by status"

        param = New SQLCollection
        param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("area_id"))))
        Dim data As DataTable = conn.QueryTable(stmt, param)
        If data.Rows.Count > 0 Then
            item.area_name = data.Rows(0)("restricted_name").ToString()
            item.area_type = data.Rows(0)("area_group_id").ToString()
            item.area_month = data.Rows(0)("month").ToString()
            item.area_location = data.Rows(0)("location").ToString()
            item.area_status = data.Rows(0)("status").ToString()
        End If
        Return item
    End Function

    Private Function ListArea(ByVal encode As String) As List(Of AreaItem)
        Dim ListData As New List(Of AreaItem)
        Dim ItemArea As AreaItem

        stmt = "select ra.id, ra.restricted_name, a.area_group, convert(varchar, ra.system_date, 111) AS system_date, ra.status 
                from Restricted_Area ra with(nolock)
				inner join Area_Group a on a.id = ra.area_group_id
                order by status"

        Dim rowPhohibited_area As DataTable = conn.QueryTable(stmt)
        If rowPhohibited_area.Rows.Count > 0 Then
            For Each phohi As DataRow In rowPhohibited_area.Rows()
                ItemArea = New AreaItem
                If encode = "Y" Then
                    ItemArea.area_id = HttpContext.Current.Server.UrlEncode(nsEngine.nsEngine.Encrypt(phohi("id").ToString()))
                Else
                    ItemArea.area_id = phohi("id").ToString()
                End If
                ItemArea.area_name = phohi("restricted_name").ToString()
                ItemArea.area_type = phohi("area_group").ToString()
                ItemArea.area_date = phohi("system_date").ToString()
                ItemArea.area_status = phohi("status").ToString()
                ListData.Add(ItemArea)
            Next
        End If
        Return ListData
    End Function

    Private Function Group() As GroupItem
        Dim Item As New GroupItem

        param = New SQLCollection
        param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("group_id"))))
        Dim data As DataTable = conn.QueryTable("select id, area_group, status from Area_Group with(nolock) where id = @id", param)

        If data.Rows.Count > 0 Then
            Item.group_name = data.Rows(0)("area_group")
            Item.group_status = data.Rows(0)("status")
        End If
        Return Item
    End Function

    Private Function ListGroup(ByVal encode As String) As List(Of GroupItem)
        Dim ListData As New List(Of GroupItem)
        Dim ItemGroup As GroupItem

        stmt = "select id, area_group,status, convert(varchar, system_date, 111) AS system_date 
                from Area_Group with(nolock)
                order by status"

        Dim rowProhi As DataTable = conn.QueryTable(stmt)
        If rowProhi.Rows.Count > 0 Then
            For Each pho As DataRow In rowProhi.Rows()
                ItemGroup = New GroupItem
                If encode = "Y" Then
                    ItemGroup.group_id = HttpContext.Current.Server.UrlEncode(nsEngine.nsEngine.Encrypt(pho("id").ToString()))
                Else
                    ItemGroup.group_id = pho("id").ToString()
                End If
                ItemGroup.group_name = pho("area_group").ToString()
                ItemGroup.group_date = pho("system_date").ToString()
                ItemGroup.group_status = pho("status").ToString()
                ListData.Add(ItemGroup)
            Next
        End If
        Return ListData
    End Function

    Private Function SaveGroup() As DataItem
        Dim data As New DataItem
        Dim group_id As String

        param = New SQLCollection
        param.Add("@area_group", DbType.String, _REQUEST("area_group_name"))
        group_id = conn.QueryField("select id from Area_Group with(nolock) where area_group = @area_group", param)

        If Not String.IsNullOrEmpty(group_id) Then
            data.status = "fail"
            data.txtAlert = "ไม่สามารถบันทึกข้อมูลนี้ได้ เนื่องจากมีข้อมูลกลุ่มพื้นที่หวงห้ามนี้อยู่แล้ว"

        Else
            group_id = _REQUEST("area_group_id").ToString()
            param.Add("@staff", DbType.String, "0")

            If Not String.IsNullOrEmpty(group_id) Then
                stmt = "update Area_Group SET 
                            update_staff = @staff, 
                            update_date = GETDATE(), 
                            area_group = @area_group,
                            status = @status
                   	    where id = @id"

                param.Add("@status", DbType.String, _REQUEST("area_group_status"))
                param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(group_id)))
                conn.Execute(stmt, param)

                data.status = "success"
                data.txtAlert = "แก้ไขข้อมูลสำเร็จ"

            Else
                stmt = "INSERT INTO Area_Group(system_staff, system_date, area_group, status, location) VALUES (@staff, GETDATE(), @area_group, 'Active', NULL)"
                conn.Execute(stmt, param)

                data.status = "success"
                data.txtAlert = "บันทึกข้อมูลสำเร็จ"
            End If
        End If
        Return data
    End Function

    Private Function SaveArea() As DataItem
        Dim data As New DataItem
        Dim DayItem As List(Of GetDay) = JSON.Deserialize(Of List(Of GetDay))(_REQUEST("day"))
        Dim MonthItem As List(Of GetMonth) = JSON.Deserialize(Of List(Of GetMonth))(_REQUEST("month"))
        Dim area_id As String = _REQUEST("area_id").ToString()

        param = New SQLCollection
        param.Add("@staff", DbType.String, "0")
        param.Add("@area_name", DbType.String, _REQUEST("area_name"))
        param.Add("@group_id", DbType.String, _REQUEST("group_area"))
        param.Add("@location", DbType.String, _REQUEST("location"))
        param.Add("@month1", DbType.String, MonthItem(0).status)
        param.Add("@month2", DbType.String, MonthItem(1).status)
        param.Add("@month3", DbType.String, MonthItem(2).status)
        param.Add("@month4", DbType.String, MonthItem(3).status)
        param.Add("@month5", DbType.String, MonthItem(4).status)
        param.Add("@month6", DbType.String, MonthItem(5).status)
        param.Add("@month7", DbType.String, MonthItem(6).status)
        param.Add("@month8", DbType.String, MonthItem(7).status)
        param.Add("@month9", DbType.String, MonthItem(8).status)
        param.Add("@month10", DbType.String, MonthItem(9).status)
        param.Add("@month11", DbType.String, MonthItem(10).status)
        param.Add("@month12", DbType.String, MonthItem(11).status)

        If Not String.IsNullOrEmpty(area_id) Then
            area_id = nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(area_id))

            stmt = "update Restricted_Area SET 
                        update_staff = @staff, 
                        update_date = GETDATE(), 
                        restricted_name = @area_name,
                        area_group_id = @group_id,
                        month1 = @month1,
                        month2 = @month2, 
                        month3 = @month3, 
                        month4 = @month4, 
                        month5 = @month5,
                        month6 = @month6,
                        month7 = @month7,
                        month8 = @month8, 
                        month9 = @month9,
                        month10 = @month10,
                        month11 = @month11,
                        month12 = @month12,
                        location = @location, 
                        status = @status    
                    where id = @id"

            param.Add("@status", DbType.String, _REQUEST("area_status"))
            param.Add("@id", DbType.Int64, area_id)
            conn.Execute(stmt, param)

            'delete Restricted_AreaDetails
            conn.Execute("delete from Restricted_AreaDetails where restricted_area_id = @id", param)

            'insert Restricted_AreaDetails
            stmt = "insert into Restricted_AreaDetails(restricted_area_id, restrict_day, time_start, time_end) 
                    values (@restricted_area_id, @restrict_day, @time_start, @time_end)"

            For Each item In DayItem
                param = New SQLCollection
                param.Add("@restricted_area_id", DbType.Int64, area_id)
                param.Add("@restrict_day", DbType.String, item.day)
                param.Add("@time_start", DbType.Time, item.time_start)
                param.Add("@time_end", DbType.Time, item.time_end)
                conn.Execute(stmt, param)
            Next

            data.status = "success"
            data.txtAlert = "แก้ไขข้อมูลสำเร็จ"

        Else
            stmt = "insert into Restricted_Area(system_staff, system_date, restricted_name, area_group_id,
                    month1, month2, month3, month4, month5, month6, month7, month8, month9, month10, month11, month12, location, status) 
                    values (@staff, GETDATE(), @area_name, @group_id,
                    @month1, @month2, @month3, @month4, @month5, @month6, @month7, @month8, @month9, @month10, @month11, @month12, @location, 'Active')"
            area_id = conn.Execute(stmt, param)

            stmt = "insert into Restricted_AreaDetails(restricted_area_id, restrict_day, time_start, time_end) 
                    values (@restricted_area_id, @restrict_day, @time_start, @time_end)"

            For Each item In DayItem
                param = New SQLCollection
                param.Add("@restricted_area_id", DbType.Int64, area_id)
                param.Add("@restrict_day", DbType.String, item.day)
                param.Add("@time_start", DbType.Time, item.time_start)
                param.Add("@time_end", DbType.Time, item.time_end)
                conn.Execute(stmt, param)
            Next

            data.status = "success"
            data.txtAlert = "บันทึกข้อมูลสำเร็จ"
        End If
        Return data
    End Function

    Private Function ChangeStatus(ByVal table As String) As DataItem
        Dim data As New DataItem
        Dim result As String()
        Dim id_split As String()
        Dim id As String = _REQUEST("data")

        stmt = "update " & table & " SET update_staff = @staff, update_date = GETDATE(), status = @status where id = @id"

        If _REQUEST("list") = "Y" Then
            id = id.Remove(id.Length - 1)
            id_split = id.Split(New Char() {","c})

            For Each row In id_split
                param = New SQLCollection
                result = row.Split(New Char() {"|"c})

                If result(1) = "Active" Then
                    param.Add("@status", DbType.String, "Inactive")
                Else
                    param.Add("@status", DbType.String, "Active")
                End If

                param.Add("@staff", DbType.String, "0")
                param.Add("@id", DbType.String, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(result(0))))
                conn.Execute(stmt, param)
            Next

        Else
            param = New SQLCollection
            result = id.Split(New Char() {"|"c})

            If result(1) = "Active" Then
                param.Add("@status", DbType.String, "Inactive")
            Else
                param.Add("@status", DbType.String, "Active")
            End If

            param.Add("@staff", DbType.String, "0")
            param.Add("@id", DbType.String, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(result(0))))
            conn.Execute(stmt, param)
        End If

        data.status = "success"
        data.txtAlert = "แก้ไขข้อมูลสำเร็จ"
        Return data
    End Function

    Public Structure GetMonth
        Public status As String
    End Structure

    Public Structure DataItem
        Public status As String
        Public txtAlert As String
    End Structure

    Public Structure GetDay
        Public day As String
        Public time_start As String
        Public time_end As String
    End Structure

    Public Structure AreaDetailItem
        Public data_area As String
        Public data_day As String
        Public data_start As String
        Public data_end As String
    End Structure

    Public Structure GroupItem
        Public group_id As String
        Public group_name As String
        Public group_date As String
        Public group_status As String
    End Structure

    Public Structure AreaItem
        Public area_id As String
        Public area_name As String
        Public area_type As String
        Public area_date As String
        Public area_month As String
        Public area_location As String
        Public area_status As String
    End Structure

    Public Structure AllAreaItem
        Public area As AreaItem
        Public area_details As List(Of AreaDetailItem)
    End Structure
End Class