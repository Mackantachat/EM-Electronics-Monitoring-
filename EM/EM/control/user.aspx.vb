Imports nsEngine
Imports nsEngine.nsEngine
Imports System.Threading
Imports System.Globalization

Public Class user
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
            Case "role" : JSONResponse.setItems(Of RoleItem)(Me.Role())
            Case "group" : JSONResponse.setItems(Of GroupItem)(Me.Group())
            Case "department" : JSONResponse.setItems(Of DepartmentItem)(Me.Department())
            Case "user_account" : JSONResponse.setItems(Of UserItem)(Me.UserAccount())
            Case "save_role" : JSONResponse.setItems(Of DataItem)(Me.SaveUserRole())
            Case "save_group" : JSONResponse.setItems(Of DataItem)(Me.SaveUserGroup())
            Case "save_department" : JSONResponse.setItems(Of DataItem)(Me.SaveDepartment())
            Case "save_account" : JSONResponse.setItems(Of DataItem)(Me.SaveUserAccount())
            Case "get_role" : JSONResponse.setItems(Of List(Of ListItem))(Me.ListSelect("role"))
            Case "get_group" : JSONResponse.setItems(Of List(Of ListItem))(Me.ListSelect("group"))
            Case "get_department" : JSONResponse.setItems(Of List(Of ListItem))(Me.ListSelect("department"))
            Case "get_account" : JSONResponse.setItems(Of List(Of AccountItem))(Me.ListAccount("account"))
            Case "list_role" : JSONResponse.setItems(Of List(Of RoleItem))(Me.ListRole())
            Case "list_group" : JSONResponse.setItems(Of List(Of GroupItem))(Me.ListGroup())
            Case "list_department" : JSONResponse.setItems(Of List(Of DepartmentItem))(Me.ListDepartment())
            Case "list_account" : JSONResponse.setItems(Of List(Of AccountItem))(Me.ListAccount("Y"))
            Case "status_role" : JSONResponse.setItems(Of DataItem)(Me.ChangeStatus("User_Role"))
            Case "status_group" : JSONResponse.setItems(Of DataItem)(Me.ChangeStatus("User_Group"))
            Case "status_department" : JSONResponse.setItems(Of DataItem)(Me.ChangeStatus("Department"))
            Case "status_account" : JSONResponse.setItems(Of DataItem)(Me.ChangeStatus("User_Account"))
            Case "changepassword" : JSONResponse.setItems(Of DataItem)(Me.ChangePassword())
            Case Else
                Dim exMsg As String = IIf(String.IsNullOrEmpty(_REQUEST("action")), "", _REQUEST("action"))
                Throw New Exception("Not found action " & exMsg & ".", New Exception("Please check your action name"))
        End Select
        Return JSONResponse.ToJSON()
    End Function

    Private Function ListSelect(ByVal table As String) As List(Of ListItem)
        Dim ListData As New List(Of ListItem)
        Dim ItemData As ListItem

        If table = "role" Then
            stmt = "select id, role_name_th name_th, role_name_eng name_eng from User_Role with(nolock)"

        ElseIf table = "group" Then
            stmt = "select id, group_name_th name_th, group_name_eng name_eng from User_Group with(nolock)"

        ElseIf table = "department" Then
            stmt = "select id, department_name_th name_th, department_name_eng name_eng from Department with(nolock)"

        ElseIf table = "account" Then
            stmt = "select id, name + ' ' + last_name name_th, '' name_eng from User_Account with(nolock)"

        End If

        Dim item As DataTable = conn.QueryTable(stmt)
        If item.Rows.Count > 0 Then
            For Each data As DataRow In item.Rows()
                ItemData = New ListItem
                ItemData.id = data("id").ToString()
                ItemData.name = data("name_th").ToString()
                ItemData.name_en = data("name_eng").ToString()
                ListData.Add(ItemData)
            Next
        End If
        Return ListData

    End Function

    Private Function ListRole() As List(Of RoleItem)
        Dim ListData As New List(Of RoleItem)
        Dim ItemRole As RoleItem
        stmt = "select id, role_name_th, status, convert(varchar, system_date, 111) AS system_date  
                from User_Role with(nolock)
                order by status, role_name_th"

        Dim rowRole As DataTable = conn.QueryTable(stmt)
        If rowRole.Rows.Count > 0 Then
            For Each Rol As DataRow In rowRole.Rows()
                ItemRole = New RoleItem
                ItemRole.role_id = HttpContext.Current.Server.UrlEncode(nsEngine.nsEngine.Encrypt(Rol("id").ToString()))
                ItemRole.role_name = Rol("role_name_th").ToString()
                ItemRole.role_date = Rol("system_date").ToString()
                ItemRole.role_status = Rol("status").ToString()
                ListData.Add(ItemRole)
            Next
        End If
        Return ListData
    End Function

    Private Function ListGroup() As List(Of GroupItem)
        Dim ListData As New List(Of GroupItem)
        Dim ItemGroup As GroupItem
        stmt = "select id, group_name_th, convert(varchar, system_date, 111) AS system_date, status
                from User_Group with(nolock) 
                order by status, group_name_th"

        Dim item As DataTable = conn.QueryTable(stmt)
        If item.Rows.Count > 0 Then
            For Each data As DataRow In item.Rows()
                ItemGroup = New GroupItem
                ItemGroup.group_id = HttpContext.Current.Server.UrlEncode(nsEngine.nsEngine.Encrypt(data("id").ToString()))
                ItemGroup.group_name = data("group_name_th").ToString()
                ItemGroup.group_date = data("system_date").ToString()
                ItemGroup.group_status = data("status").ToString()
                ListData.Add(ItemGroup)
            Next
        End If
        Return ListData
    End Function

    Private Function ListDepartment() As List(Of DepartmentItem)
        Dim ListData As New List(Of DepartmentItem)
        Dim ItemDepartment As DepartmentItem
        stmt = "select d.id, d.code, d.department_name_th, d.telephone, d.address_number, d.moo, d.road, d.lane, d.status, Convert(varchar, d.system_date, 111) As system_date, 
                       p.name_th province_name, dis.name_th district_name, sd.name_th subdistrict_name
                from Department d with(nolock) 
				inner join Province p on d.province_id = p.id  
			    inner join District dis on d.district_id = dis.id
				inner join Sub_District sd on d.sub_district_id = sd.id
                order by d.status"

        Dim item As DataTable = conn.QueryTable(stmt)
        If item.Rows.Count > 0 Then
            For Each dep As DataRow In item.Rows()
                ItemDepartment = New DepartmentItem
                ItemDepartment.department_id = HttpContext.Current.Server.UrlEncode(nsEngine.nsEngine.Encrypt(dep("id").ToString()))
                ItemDepartment.department_code = dep("code").ToString()
                ItemDepartment.department_name = dep("department_name_th").ToString()
                ItemDepartment.department_telephone = dep("telephone").ToString()
                ItemDepartment.department_number = dep("address_number").ToString()
                ItemDepartment.department_moo = dep("moo").ToString()
                ItemDepartment.department_road = dep("road").ToString()
                ItemDepartment.department_lane = dep("lane").ToString()
                ItemDepartment.department_province = dep("province_name").ToString()
                ItemDepartment.department_district = dep("district_name").ToString()
                ItemDepartment.department_sub_district = dep("subdistrict_name").ToString()
                ItemDepartment.department_Date = dep("system_date").ToString()
                ItemDepartment.department_status = dep("status").ToString()
                ListData.Add(ItemDepartment)
            Next
        End If
        Return ListData
    End Function

    Private Function ListAccount(ByVal encode As String) As List(Of AccountItem)
        Dim ListData As New List(Of AccountItem)
        Dim ItemAccount As AccountItem
        stmt = "select account.id, user_name, name+'   '+last_name As fullname, department_name_th, account.telephone, email, role.role_name_th, dep.department_name_th, account.status
                from User_Account account with(nolock)
				left join Department dep ON account.department_id = dep.id
				left join User_Role role On role.id = account.user_role_id
                order by account.status, fullname"

        Dim item As DataTable = conn.QueryTable(stmt)
        If item.Rows.Count > 0 Then
            For Each data As DataRow In item.Rows()
                ItemAccount = New AccountItem
                If encode = "Y" Then
                    ItemAccount.account_id = HttpContext.Current.Server.UrlEncode(nsEngine.nsEngine.Encrypt(data("id").ToString()))
                Else
                    ItemAccount.account_id = data("id").ToString()
                End If
                ItemAccount.account_user_name = data("user_name").ToString()
                ItemAccount.account_name = data("fullname").ToString()
                ItemAccount.account_department_name = data("department_name_th").ToString()
                ItemAccount.account_telephone = data("telephone").ToString()
                ItemAccount.account_email = data("email").ToString()
                ItemAccount.account_user_role_name = data("role_name_th").ToString()
                ItemAccount.account_status = data("status").ToString()
                ListData.Add(ItemAccount)
            Next
        End If
        Return ListData
    End Function

    Private Function Role() As RoleItem
        Dim ItemRole As New RoleItem
        param = New SQLCollection
        param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("role_id"))))
        Dim data As DataTable = conn.QueryTable("select role_name_th, status from User_Role with(nolock) where id = @id", param)

        If data.Rows.Count > 0 Then
            ItemRole.role_name = data.Rows(0)("role_name_th")
            ItemRole.role_status = data.Rows(0)("status")
        End If
        Return ItemRole
    End Function

    Private Function Group() As GroupItem
        Dim ItemGroup As New GroupItem
        stmt = "select group_name_th, status from User_Group with(nolock) where id = @id"
        param = New SQLCollection
        param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("user_group_id"))))

        Dim data As DataTable = conn.QueryTable(stmt, param)
        If data.Rows.Count > 0 Then
            ItemGroup.group_name = data.Rows(0)("group_name_th")
            ItemGroup.group_status = data.Rows(0)("status")
        End If
        Return ItemGroup
    End Function

    Private Function Department() As DepartmentItem
        Dim depart As New DepartmentItem
        stmt = "select department_name_th, code, telephone_mobile, telephone, address_number, moo, road, lane, province_id, district_id, sub_district_id, status 
                from Department with(nolock) 
                where id = @id"

        param = New SQLCollection
        param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("department_id"))))

        Dim data As DataTable = conn.QueryTable(stmt, param)
        If data.Rows.Count > 0 Then
            depart.department_code = data.Rows(0)("code").ToString()
            depart.department_name = data.Rows(0)("department_name_th").ToString()
            depart.department_mobile = data.Rows(0)("telephone_mobile").ToString()
            depart.department_telephone = data.Rows(0)("telephone").ToString()
            depart.department_number = data.Rows(0)("address_number").ToString()
            depart.department_moo = data.Rows(0)("moo").ToString()
            depart.department_road = data.Rows(0)("road").ToString()
            depart.department_lane = data.Rows(0)("lane").ToString()
            depart.department_province = data.Rows(0)("province_id").ToString()
            depart.department_district = data.Rows(0)("district_id").ToString()
            depart.department_sub_district = data.Rows(0)("sub_district_id").ToString()
            depart.department_status = data.Rows(0)("status").ToString()
        End If
        Return depart
    End Function

    Private Function UserAccount() As UserItem
        Dim account As New UserItem
        stmt = "select user_name, password, name, last_name, PID, email, telephone, Convert(varchar, birthdate, 103) As birthday, department_id, user_role_id, user_group_id, status
                from User_Account with(nolock) 
                where id = @id"

        param = New SQLCollection
        param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("account_id"))))

        Dim data As DataTable = conn.QueryTable(stmt, param)
        If data.Rows.Count > 0 Then
            account.username = data.Rows(0)("user_name")
            account.password = data.Rows(0)("password")
            account.name = data.Rows(0)("name")
            account.lastname = data.Rows(0)("last_name")
            account.PID = data.Rows(0)("PID")
            account.email = data.Rows(0)("email")
            account.tel = data.Rows(0)("telephone")
            account.birthdate = data.Rows(0)("birthday")
            account.department = data.Rows(0)("department_id")
            account.user_role = data.Rows(0)("user_role_id")
            account.user_group = data.Rows(0)("user_group_id")
            account.status = data.Rows(0)("status")
        End If
        Return account
    End Function

    Private Function SaveUserRole() As DataItem
        Dim role_id As String
        Dim data As New DataItem

        param = New SQLCollection
        param.Add("@role_name", DbType.String, _REQUEST("role_name"))
        role_id = conn.QueryField("select id from User_Role with(nolock) where role_name_th = @role_name", param)

        If Not String.IsNullOrEmpty(role_id) Then
            data.status = "fail"
            data.txtAlert = "ไม่สามารถบันทึกข้อมูลนี้ได้ เนื่องจากมีข้อมูลสิทธิ์ผู้ใช้งานนี้อยู่แล้ว"

        Else
            role_id = _REQUEST("role_id").ToString()
            param.Add("@staff", DbType.String, "0")

            If Not String.IsNullOrEmpty(role_id) Then
                stmt = "update User_Role SET 
                            update_staff = @staff, 
                            update_date = GETDATE(), 
                            role_name_th = @role_name,
                            status = @status
                    where id = @id"

                param.Add("@status", DbType.String, _REQUEST("role_status"))
                param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(role_id)))
                conn.Execute(stmt, param)

                data.status = "success"
                data.txtAlert = "แก้ไขข้อมูลสำเร็จ"

            Else
                stmt = "insert into User_Role(system_staff, system_date, role_name_th, role_code, status)
                        values (@staff , GETDATE(), @role_name, 'admin1', 'Active')"
                conn.Execute(stmt, param)

                data.status = "success"
                data.txtAlert = "บันทึกข้อมูลสำเร็จ"
            End If
        End If
        Return data
    End Function

    Private Function SaveUserGroup() As DataItem
        Dim group_id As String
        Dim data As New DataItem

        param = New SQLCollection
        param.Add("@group_name", DbType.String, _REQUEST("group_name"))
        group_id = conn.QueryField("select id from User_group with(nolock) where group_name_th = @group_name", param)

        If Not String.IsNullOrEmpty(group_id.ToString()) Then
            data.status = "fail"
            data.txtAlert = "ไม่สามารถบันทึกข้อมูลกลุ่มนี้ได้ เนื่องจากมีข้อมูลกลุ่มนี้อยู่แล้ว"

        Else
            group_id = _REQUEST("group_id").ToString()
            param.Add("@staff", DbType.String, "0")

            If Not String.IsNullOrEmpty(group_id) Then
                stmt = "update  User_Group SET 
                                update_staff = @staff, 
                                update_date = GETDATE(), 
                                group_name_th = @group_name,
                                status = @status
                        where id = @id"

                param.Add("@status", DbType.String, _REQUEST("group_status"))
                param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("group_id"))))
                conn.Execute(stmt, param)

                data.status = "success"
                data.txtAlert = "แก้ไขข้อมูลสำเร็จ"

            Else
                stmt = "insert into User_Group(system_staff, system_date, group_name_th, status)
                        values (@staff , GETDATE(), @group_name, 'Active')"

                conn.Execute(stmt, param)

                data.status = "success"
                data.txtAlert = "บันทึกข้อมูลสำเร็จ"
            End If
        End If
        Return data
    End Function

    Private Function SaveDepartment() As DataItem
        Dim department As DepartmentItem = JSON.Deserialize(Of DepartmentItem)(_REQUEST("da"))
        Dim department_id As String = department.department_id
        Dim data As New DataItem

        param = New SQLCollection
        param.Add("@staff", DbType.String, "0")
        param.Add("@code", DbType.String, department.department_code)
        param.Add("@department_name", DbType.String, department.department_name)
        param.Add("@mobile", DbType.String, department.department_mobile.ToString())
        param.Add("@telephone", DbType.String, department.department_telephone.ToString())
        param.Add("@address_number", DbType.String, department.department_number.ToString())
        param.Add("@moo", DbType.String, department.department_moo.ToString())
        param.Add("@road", DbType.String, department.department_road.ToString())
        param.Add("@lane", DbType.String, department.department_lane.ToString())
        param.Add("@province", DbType.String, department.department_province.ToString())
        param.Add("@district", DbType.String, department.department_district.ToString())
        param.Add("@sub_district", DbType.String, department.department_sub_district.ToString())

        If Not String.IsNullOrEmpty(department_id) Then
            stmt = "update Department SET 
                        update_staff = @staff, 
                        update_date = GETDATE(), 
                        department_name_th = @department_name_th,
                        telephone_mobile = @mobile,
                        telephone = @telephone,
                        address_number = @address_number,
                        moo = @moo,
                        road = @road,
                        lane = @lane,
                        province_id = @province,
                        district_id = @district,
                        sub_district_id = @sub_district,
                        status = @status
                    where id = @id"

            param.Add("@status", DbType.String, department.department_status)
            param.Add("@id", DbType.String, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(department.department_id)))
            conn.Execute(stmt, param)

            data.status = "success"
            data.txtAlert = "แก้ไขข้อมูลสำเร็จ"

        Else
            department_id = conn.QueryField("select id from Department with(nolock) where code = @code or department_name_th = @department_name", param)
            If Not String.IsNullOrEmpty(department_id.ToString()) Then
                data.status = "fail"
                data.txtAlert = "ไม่สามารถแก้ไขข้อมูลนี้ได้ เนื่องจากมีข้อมูลชื่อหน่วยงานหรือรหัสหน่วยงานนี้อยู่แล้ว"

            Else
                stmt = "insert into Department(system_staff, system_date, department_name_th, code, telephone_mobile, telephone, 
                               address_number, moo, road, lane, province_id, district_id, sub_district_id, status)
                        values (@staff, GETDATE(), @department_name, @code, @mobile, @telephone, 
                                @address_number, @moo, @road, @lane, @province, @district, @sub_district, 'Active')"

                conn.Execute(stmt, param)

                data.status = "success"
                data.txtAlert = "บันทึกข้อมูลสำเร็จ"
            End If
        End If
        Return data
    End Function

    Private Function SaveUserAccount() As DataItem
        Dim account_id As String
        Dim data As New DataItem
        Dim THInfo As New Globalization.CultureInfo("th-TH")

        param = New SQLCollection
        param.Add("@user_name", DbType.String, _REQUEST("username"))
        account_id = conn.QueryField("select id from User_Account with(nolock) where user_name = @user_name", param)

        If Not String.IsNullOrEmpty(account_id.ToString()) Then
            data.status = "fail"
            data.txtAlert = "ไม่สามารถบันทึกข้อมูลผู้ใช้งานนี้ได้ เนื่องจากมีข้อมูลผู้ใช้งานนี้อยู่แล้ว"

        Else
            account_id = _REQUEST("id").ToString()
            param.Add("@staff", DbType.String, "0")
            param.Add("@password", DbType.String, _REQUEST("password"))
            param.Add("@name", DbType.String, _REQUEST("name"))
            param.Add("@last_name", DbType.String, _REQUEST("lastname"))
            param.Add("@PID", DbType.String, _REQUEST("PID"))
            param.Add("@email", DbType.String, _REQUEST("email"))
            param.Add("@telephone", DbType.String, _REQUEST("tel"))
            param.Add("@birthdate", DbType.DateTime, System.Convert.ToDateTime(_REQUEST("birthdate"), THInfo).ToString("dd-MM-yyyy"))
            param.Add("@department", DbType.String, _REQUEST("department"))
            param.Add("@user_role", DbType.String, _REQUEST("user_role"))
            param.Add("@user_group", DbType.String, _REQUEST("user_group"))

            If Not String.IsNullOrEmpty(account_id) Then
                stmt = "update User_Account SET 
                            update_staff = @staff, 
                            update_date = GETDATE(), 
                            user_name = @user_name,
                            password = @password,
                            name = @name, 
                            last_name = @last_name, 
                            PID = @PID,
                            telephone = @telephone,  
                            birthdate = @birthdate, 
                            department_id = @department,
                            user_role_id = @user_role,
                            user_group_id = @user_group
                        where id = @id"

                param.Add("@status", DbType.String, _REQUEST("status"))
                param.Add("@id", DbType.String, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(account_id)))
                conn.Execute(stmt, param)

                data.status = "success"
                data.txtAlert = "แก้ไขข้อมูลสำเร็จ"

            Else
                stmt = "insert into User_Account (system_staff, system_date, user_name, password,name, last_name, PID, email, telephone, birthdate, department_id, user_role_id, user_group_id, status)
                        values (@staff, GETDATE(), @user_name, @password, @name, @last_name, @PID, @email, @telephone, @birthdate, @department, @user_role, @user_group, 'Active') "

                conn.Execute(stmt, param)

                data.status = "success"
                data.txtAlert = "บันทึกข้อมูลสำเร็จ"
            End If
        End If
        Return data
    End Function

    Private Function ChangePassword() As DataItem
        Dim data As New DataItem
        param = New SQLCollection

        If Not String.IsNullOrEmpty(_REQUEST("account_id").ToString()) Then
            param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("account_id"))))
            param.Add("@password", DbType.String, _REQUEST("password"))

            conn.Execute("update User_Account set password = @password where id =  @id", param)

            data.status = "success"
            data.txtAlert = "แก้ไขรหัสผ่านสำเร็จ"
        Else
            data.status = "fail"
            data.txtAlert = "save fail"
        End If
        Return Data
    End Function

    Private Function ChangeStatus(ByVal table As String) As DataItem
        Dim data As New DataItem
        Dim id As String = _REQUEST("data")
        Dim id_split As String()
        Dim result As String()

        If _REQUEST("list") = "Y" Then
            id = id.Remove(id.Length - 1)
            id_split = id.Split(New Char() {","c})
            Dim row As String
            For Each row In id_split
                result = row.Split(New Char() {"|"c})
                param = New SQLCollection
                param.Add("@staff", DbType.String, "0")
                param.Add("@id", DbType.String, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(result(0))))

                If result(1) = "Active" Then
                    param.Add("@status", DbType.String, "Inactive")
                Else
                    param.Add("@status", DbType.String, "Active")
                End If
                conn.Execute("update " & table & " SET update_staff = @staff, update_date = GETDATE(), status = @status where id = @id", param)
            Next
        Else
            result = id.Split(New Char() {"|"c})
            param = New SQLCollection
            param.Add("@staff", DbType.String, "0")
            param.Add("@id", DbType.String, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(result(0))))
            If result(1) = "Active" Then
                param.Add("@status", DbType.String, "Inactive")
            Else
                param.Add("@status", DbType.String, "Active")
            End If
            conn.Execute("update " & table & " SET update_staff = @staff, update_date = GETDATE(), status = @status where id = @id", param)
        End If

        data.status = "success"
        data.txtAlert = "เปลี่ยนสถานะข้อมูลสำเร็จ"
        Return data
    End Function

    Public Structure DataItem
        Public status As String
        Public txtAlert As String
    End Structure

    Public Structure ListItem
        Public id As String
        Public name As String
        Public name_en As String
    End Structure

    Public Structure UserItem
        Public id As String
        Public username As String
        Public password As String
        Public name As String
        Public lastname As String
        Public PID As String
        Public email As String
        Public tel As String
        Public birthdate As String
        Public department As String
        Public user_role As String
        Public user_group As String
        Public status As String
    End Structure

    Public Structure AccountItem
        Public account_id As String
        Public account_user_name As String
        Public account_name As String
        Public account_department_name As String
        Public account_telephone As String
        Public account_email As String
        Public account_user_role_name As String
        Public account_status As String
    End Structure

    Public Structure DepartmentItem
        Public department_id As String
        Public department_code As String
        Public department_name As String
        Public department_mobile As String
        Public department_telephone As String
        Public department_number As String
        Public department_road As String
        Public department_moo As String
        Public department_lane As String
        Public department_province As String
        Public department_district As String
        Public department_sub_district As String
        Public department_Date As String
        Public department_status As String
    End Structure

    Public Structure GroupItem
        Public group_id As String
        Public group_name As String
        Public group_status As String
        Public group_date As String
    End Structure

    Public Structure RoleItem
        Public role_id As String
        Public role_date As String
        Public role_name As String
        Public role_status As String
    End Structure
End Class