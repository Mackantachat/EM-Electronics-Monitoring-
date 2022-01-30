Imports nsEngine

Public Class Location
    Inherits UI.Page
    Dim stmt As String
    Dim conn As New DBQuery
    Dim param As New SQLCollection()
    Dim JSONResponse As New CallbackException()

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
            Case "district" : JSONResponse.setItems(Of DataItem)(Me.District())
            Case "province" : JSONResponse.setItems(Of DataItem)(Me.Province())
            Case "subdistrict" : JSONResponse.setItems(Of DataItem)(Me.Subdistrict())
            Case "save_district" : JSONResponse.setItems(Of Item)(Me.SaveDistrict())
            Case "save_province" : JSONResponse.setItems(Of Item)(Me.SaveProvince())
            Case "save_subdistrict" : JSONResponse.setItems(Of Item)(Me.SaveSubdDistrict())
            Case "status_district" : JSONResponse.setItems(Of Item)(Me.ChangeStatus("District"))
            Case "status_province" : JSONResponse.setItems(Of Item)(Me.ChangeStatus("Province"))
            Case "status_subdistrict" : JSONResponse.setItems(Of Item)(Me.ChangeStatus("Sub_District"))
            Case "get_district" : JSONResponse.setItems(Of List(Of DataItem))(Me.GetList("N", "District"))
            Case "get_province" : JSONResponse.setItems(Of List(Of DataItem))(Me.GetList("N", "Province"))
            Case "get_subdistrict" : JSONResponse.setItems(Of List(Of DataItem))(Me.GetList("N", "Sub_District"))
            Case "list_address" : JSONResponse.setItems(Of AddressItem)(Me.GetAddress())
            Case "list_district" : JSONResponse.setItems(Of List(Of DataItem))(Me.GetList("Y", "District"))
            Case "list_province" : JSONResponse.setItems(Of List(Of DataItem))(Me.GetList("Y", "Province"))
            Case "list_subdistrict" : JSONResponse.setItems(Of List(Of DataItem))(Me.GetList("Y", "Sub_District"))
            Case Else
                Dim exMsg As String = IIf(String.IsNullOrEmpty(_REQUEST("action")), "", _REQUEST("action"))
                Throw New Exception("Not found action " & exMsg & ".", New Exception("Please check your action name"))
        End Select
        Return JSONResponse.ToJSON()
    End Function

    Private Function GetList(ByVal encode As String, ByVal table As String) As List(Of DataItem)
        Dim list_data As New List(Of DataItem)
        Dim item_data As DataItem
        stmt = "select id, code, name_th, status, convert(varchar, system_date, 111) AS system_date"

        If table = "Province" Then
            stmt &= ", null as data_key"

        ElseIf table = "District" Then
            stmt &= ", province_id as data_key"

        ElseIf table = "Sub_District" Then
            stmt &= ", district_id as data_key"
        End If

        stmt &= " from " & table & " with(nolock) order by status, CAST(code AS int)"

        Dim item As DataTable = conn.QueryTable(stmt)
        If item.Rows.Count > 0 Then
            For Each data As DataRow In item.Rows()
                item_data = New DataItem
                If encode = "Y" Then
                    item_data.data_id = HttpContext.Current.Server.UrlEncode(nsEngine.nsEngine.Encrypt(data("id").ToString()))
                Else
                    item_data.data_id = data("id").ToString()
                End If
                item_data.data_code = data("code").ToString()
                item_data.data_name = data("name_th").ToString()
                item_data.data_date = data("system_date").ToString()
                item_data.data_key = data("data_key").ToString()
                item_data.data_status = data("status").ToString()
                list_data.Add(item_data)
            Next
        End If
        Return list_data
    End Function

    Private Function Province() As DataItem
        Dim item As DataItem
        item = New DataItem

        param = New SQLCollection
        param.Add("@pid", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("pid"))))
        Dim data As DataTable = conn.QueryTable("select code, name_th, status from Province with(nolock) where id = @pid", param)

        If data.Rows.Count > 0 Then
            item.data_code = data.Rows(0)("code")
            item.data_name = data.Rows(0)("name_th")
            item.data_status = data.Rows(0)("status")
        End If
        Return item
    End Function

    Private Function District() As DataItem
        Dim item As DataItem
        item = New DataItem

        param = New SQLCollection
        param.Add("@did", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("did"))))
        Dim data As DataTable = conn.QueryTable("select code, name_th, province_id, status from district with(nolock) where id = @did", param)

        If data.Rows.Count > 0 Then
            item.data_key = data.Rows(0)("province_id")
            item.data_code = data.Rows(0)("code")
            item.data_name = data.Rows(0)("name_th")
            item.data_status = data.Rows(0)("status")
        End If
        Return item
    End Function

    Private Function Subdistrict() As DataItem
        Dim item As DataItem
        item = New DataItem

        stmt = "select sd.code, 
                    sd.name_th,
                    CONVERT(varchar(10), d.province_id) +','+  CONVERT(varchar(10), sd.district_id) as address, 
                    sd.status
                from Sub_district sd with(nolock)
                inner join district d on sd.district_id = d.id
                where sd.id = @sdid"

        param = New SQLCollection
        param.Add("@sdid", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("sdid"))))
        Dim data As DataTable = conn.QueryTable(stmt, param)

        If data.Rows.Count > 0 Then
            item.data_key = data.Rows(0)("address").ToString()
            item.data_code = data.Rows(0)("code").ToString()
            item.data_name = data.Rows(0)("name_th").ToString()
            item.data_status = data.Rows(0)("status").ToString()
        End If
        Return item
    End Function

    Private Function SaveProvince() As Item
        Dim data As New Item
        Dim province As DataItem = JSON.Deserialize(Of DataItem)(_REQUEST("da"))
        If Not String.IsNullOrEmpty(province.data_id) Then
            stmt = "update Province SET 
                        update_staff = @staff, 
                        update_date = GETDATE(), 
                        name_th = @name_th,
                        status = @status
                    where id = @id"

            param = New SQLCollection
            param.Add("@staff", DbType.String, "0")
            param.Add("@name_th", DbType.String, province.data_name)
            param.Add("@status", DbType.String, province.data_status)
            param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(province.data_id)))
            conn.Execute(stmt, param)

            data.status = "success"
            data.txtAlert = "แก้ไขข้อมูลจังหวัดสำเร็จ"

        Else
            param = New SQLCollection
            param.Add("@code", DbType.String, province.data_code)
            param.Add("@name_th", DbType.String, province.data_name)
            province.data_id = conn.QueryField("select id from Province with(nolock) where code = @code or name_th = @name_th", param)

            If Not String.IsNullOrEmpty(province.data_id) Then
                data.status = "fail"
                data.txtAlert = "ไม่สามารถบันทึกข้อมูลจังหวัดนี้ได้ เนื่องจากมีข้อมูลจังหวัดนี้อยู่แล้ว"

            Else
                param.Add("@staff", DbType.String, "0")
                param.Add("@status", DbType.String, province.data_status)

                stmt = "insert into Province(system_staff, system_date, code, name_th, status) values(@staff, GETDATE(), @code, @name_th, @status)"
                conn.Execute(stmt, param)

                data.status = "success"
                data.txtAlert = "บันทึกข้อมูลจังหวัดสำเร็จ"
            End If
        End If
        Return data
    End Function

    Private Function SaveDistrict() As Item
        Dim data As New Item
        Dim district As DataItem = JSON.Deserialize(Of DataItem)(_REQUEST("da"))
        If Not String.IsNullOrEmpty(district.data_id) Then
            stmt = "update District SET 
                        update_staff = @staff, 
                        update_date = GETDATE(), 
                        name_th = @name_th,
                        status = @status
                    where id = @id"

            param = New SQLCollection
            param.Add("@staff", DbType.String, "0")
            param.Add("@name_th", DbType.String, district.data_name)
            param.Add("@status", DbType.String, district.data_status)
            param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(district.data_id)))
            conn.Execute(stmt, param)

            data.status = "success"
            data.txtAlert = "แก้ไขข้อมูลอำเภอสำเร็จ"

        Else
            param = New SQLCollection
            param.Add("@code", DbType.String, district.data_code)
            param.Add("@name_th", DbType.String, district.data_name)
            param.Add("@province_id", DbType.String, district.data_key)

            stmt = "select Top 1 id from District with(nolock) where (province_id = @province_id and name_th = @name_th) or code = @code"
            district.data_id = conn.QueryField(stmt, param)

            If Not String.IsNullOrEmpty(district.data_id) Then
                data.status = "fail"
                data.txtAlert = "ไม่สามารถบันทึกข้อมูลอำเภอนี้ได้ เนื่องจากมีข้อมูลอำเภอนี้อยู่แล้ว"

            Else
                param.Add("@staff", DbType.String, "0")
                param.Add("@status", DbType.String, district.data_status)
                stmt = "insert into District(system_staff, system_date, code, name_th, province_id, status) values(@staff, GETDATE(), @code, @name_th, @province_id, @status)"
                conn.Execute(stmt, param)

                data.status = "success"
                data.txtAlert = "บันทึกข้อมูลอำเภอสำเร็จ"
            End If
        End If
        Return data
    End Function

    Private Function SaveSubdDistrict() As Item
        Dim data As New Item
        Dim sub_district As DataItem = JSON.Deserialize(Of DataItem)(_REQUEST("da"))
        If Not String.IsNullOrEmpty(sub_district.data_id) Then
            stmt = "update Sub_District SET 
                        update_staff = @staff, 
                        update_date = GETDATE(), 
                        name_th = @name_th,
                        status = @status
                    where id = @id"

            param = New SQLCollection
            param.Add("@staff", DbType.String, "0")
            param.Add("@name_th", DbType.String, sub_district.data_name)
            param.Add("@status", DbType.String, sub_district.data_status)
            param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(sub_district.data_id)))
            conn.Execute(stmt, param)

            data.status = "success"
            data.txtAlert = "แก้ไขข้อมูลตำบลสำเร็จ"

        Else
            param = New SQLCollection
            param.Add("@code", DbType.String, sub_district.data_code)
            param.Add("@name_th", DbType.String, sub_district.data_name)
            param.Add("@district_id", DbType.String, sub_district.data_key)

            stmt = "select Top 1 id from Sub_District with(nolock) where (district_id = @district_id and name_th = @name_th) or code = @code"
            sub_district.data_id = conn.QueryField(stmt, param)

            If Not String.IsNullOrEmpty(sub_district.data_id) Then
                data.status = "fail"
                data.txtAlert = "ไม่สามารถบันทึกข้อมูลตำบลนี้ได้ เนื่องจากมีข้อมูลตำบลนี้อยู่แล้ว"

            Else
                param.Add("@staff", DbType.String, "0")
                param.Add("@status", DbType.String, sub_district.data_status)
                stmt = "insert into Sub_District(system_staff, system_date, code, name_th, district_id, status) values(@staff, GETDATE(), @code, @name_th, @district_id, @status)"
                conn.Execute(stmt, param)

                data.status = "success"
                data.txtAlert = "บันทึกข้อมูลตำบลสำเร็จ"
            End If
        End If
        Return data
    End Function

    Private Function ChangeStatus(ByVal table As String) As Item
        Dim data As New Item
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
        data.txtAlert = "เปลี่ยนสถานะข้อมูลสำเร็จ"
        Return data
    End Function

    Public Structure Item
        Public status As String
        Public txtAlert As String
    End Structure

    Public Structure DataItem
        Public data_id As String
        Public data_date As String
        Public data_code As String
        Public data_name As String
        Public data_key As String
        Public data_status As String
    End Structure

    Public Structure AddressItem
        Public province As List(Of DataItem)
        Public district As List(Of DataItem)
        Public subdistrict As List(Of DataItem)
    End Structure

    Private Function GetAddress() As AddressItem
        Dim data As New AddressItem
        data.province = Me.GetList("N", "Province")
        data.district = Me.GetList("N", "District")
        data.subdistrict = Me.GetList("N", "Sub_District")
        Return data
    End Function
End Class