Imports nsEngine
Imports nsEngine.nsEngine
Imports System.Threading
Imports System.Globalization

Public Class EMDevice
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
            Case "device" : JSONResponse.setItems(Of EMDeviceItem)(Me.Device())
            Case "list_device" : JSONResponse.setItems(Of List(Of EMDeviceItem))(Me.GetList())
            Case "get_device" : JSONResponse.setItems(Of List(Of DeviceItem))(Me.ListSelect())
            Case "save_device" : JSONResponse.setItems(Of Item)(Me.SaveDevice())
            Case "status_devices" : JSONResponse.setItems(Of Item)(Me.ChangeStatus())
            Case Else
                Dim exMsg As String = IIf(String.IsNullOrEmpty(_REQUEST("action")), "", _REQUEST("action"))
                Throw New Exception("Not found action " & exMsg & ".", New Exception("Please check your action name"))
        End Select
        Return JSONResponse.ToJSON()
    End Function

    Private Function Device() As EMDeviceItem
        Dim Item As New EMDeviceItem

        param = New SQLCollection
        param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("device_id"))))
        Dim data As DataTable = conn.QueryTable("select serial_no, IMEI, Mac_address, department_id, status from EM_Devices with(nolock) where id = @id", param)

        If data.Rows.Count > 0 Then
            Item.device_SN = data.Rows(0)("serial_no")
            Item.device_IMEI = data.Rows(0)("IMEI")
            Item.device_Mac_address = data.Rows(0)("Mac_address")
            Item.device_department = data.Rows(0)("department_id")
            Item.device_status = data.Rows(0)("status")
        End If
        Return Item
    End Function

    Private Function ListSelect() As List(Of DeviceItem)
        Dim ListData As New List(Of DeviceItem)
        Dim ItemData As DeviceItem
        Dim item As DataTable = conn.QueryTable("select em.id, IMEI from EM_Devices em with(nolock) order by em.status")

        If item.Rows.Count > 0 Then
            For Each emd As DataRow In item.Rows()
                ItemData = New DeviceItem
                ItemData.device_id = emd("id").ToString()
                ItemData.device_IMEI = emd("IMEI").ToString()
                ListData.Add(ItemData)
            Next
        End If
        Return ListData
    End Function

    Private Function GetList() As List(Of EMDeviceItem)
        Dim ListData As New List(Of EMDeviceItem)
        Dim ItemEMDevice As EMDeviceItem

        stmt = "select em.id, serial_no, IMEI, Mac_address, department_name_th, em.status, convert(varchar, em.system_date, 111) AS system_date
                from EM_Devices em with(nolock)
                left join Department d on em.department_id = d.id
		        order by em.status "

        Dim rowEMDevice As DataTable = conn.QueryTable(stmt)
        If rowEMDevice.Rows.Count > 0 Then
            For Each emd As DataRow In rowEMDevice.Rows()
                ItemEMDevice = New EMDeviceItem
                ItemEMDevice.device_id = HttpContext.Current.Server.UrlEncode(nsEngine.nsEngine.Encrypt(emd("id").ToString()))
                ItemEMDevice.device_SN = emd("serial_no").ToString()
                ItemEMDevice.device_IMEI = emd("IMEI").ToString()
                ItemEMDevice.device_Mac_address = emd("Mac_address").ToString()
                ItemEMDevice.device_department_name = emd("department_name_th").ToString()
                ItemEMDevice.device_status = emd("status").ToString()
                ItemEMDevice.device_date = emd("system_date").ToString()
                ListData.Add(ItemEMDevice)
            Next
        End If
        Return ListData
    End Function

    Private Function SaveDevice() As Item
        Dim data As New Item
        Dim device_id As String = _REQUEST("device_id").ToString()
        param = New SQLCollection
        param.Add("@staff", DbType.String, "0")

        If Not String.IsNullOrEmpty(device_id) Then
            stmt = "update EM_Devices SET 
                            update_staff = @staff, 
                            update_date = GETDATE(), 
                            department_id = @department_id,
                            status = @status
                        where id = @id"

            param.Add("@department_id", DbType.String, _REQUEST("department_id"))
            param.Add("@status", DbType.String, _REQUEST("status"))
            param.Add("@id", DbType.String, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(device_id)))
            conn.Execute(stmt, param)

            data.status = "success"
            data.txtAlert = "แก้ไขข้อมูลสำเร็จ"

        Else
            param.Add("@imei", DbType.String, _REQUEST("imei"))
            param.Add("@serial_no", DbType.String, _REQUEST("serial_no"))
            param.Add("@mac_address", DbType.String, _REQUEST("mac_address"))
            device_id = conn.QueryField("select id from EM_Devices with(nolock) where serial_no = @serial_no or IMEI = @imei or Mac_address = @mac_address", param)

            If Not String.IsNullOrEmpty(device_id.ToString()) Then
                data.status = "fail"
                data.txtAlert = "ไม่สามารถบันทึกข้อมูลอุปกรณ์นี้ได้ เนื่องจากมีข้อมูลอุปกรณ์นี้อยู่แล้ว"

            Else
                stmt = "insert into EM_Devices(system_staff, system_date, serial_no, IMEI, Mac_address, department_id, status)
                        values (@staff, GETDATE(), @serial_no, @IMEI, @Mac_address, @department_id , 'Active')"

                param.Add("@department_id", DbType.String, _REQUEST("department_id"))
                conn.Execute(stmt, param)

                data.status = "success"
                data.txtAlert = "บันทึกข้อมูลสำเร็จ"
            End If
        End If
        Return data
    End Function

    Private Function ChangeStatus() As Item
        Dim data As New Item
        Dim result As String()
        Dim id_split As String()
        Dim id As String = _REQUEST("data")

        stmt = "update EM_Devices SET update_staff = @staff, update_date = GETDATE(), status = @status where id = @id"

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

    Public Structure Item
        Public status As String
        Public txtAlert As String
    End Structure

    Public Structure DeviceItem
        Public device_id As String
        Public device_IMEI As String
    End Structure

    Public Structure EMDeviceItem
        Public device_id As String
        Public device_SN As String
        Public device_IMEI As String
        Public device_Mac_address As String
        Public device_department As String
        Public device_department_name As String
        Public device_status As String
        Public device_date As String
    End Structure
End Class