Imports nsEngine

Public Class probationer
    Inherits UI.Page
    Dim JSONResponse As New CallbackException()
    Dim param As New SQLCollection()
    Dim conn As New DBQuery
    Dim stmt As String
    Dim THInfo As New Globalization.CultureInfo("th-TH")

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
            Case "probationer" : JSONResponse.setItems(Of AllprobationerItem)(Me.GetProbationer())
            Case "save_probationer" : JSONResponse.setItems(Of Item)(Me.SaveProbationer())
            Case "get_data" : JSONResponse.setItems(Of AllItem)(Me.GetData())
            Case "get_title" : JSONResponse.setItems(Of List(Of TitleItem))(Me.ListTitle())
            Case "list_probationer" : JSONResponse.setItems(Of List(Of ProbationerListItem))(Me.ListProbationer())
            Case "list_offence" : JSONResponse.setItems(Of List(Of OffenceItem))(Me.GetListOffence())
            Case "list_subOffence" : JSONResponse.setItems(Of List(Of SubOffenceItem))(Me.GetListSuboffence())
            Case "list_Education" : JSONResponse.setItems(Of List(Of EducationItem))(Me.GetListEducation())
            Case "list_criminal" : JSONResponse.setItems(Of List(Of CriminalItem))(Me.GetListCriminal())
            Case "status_probationer" : JSONResponse.setItems(Of Item)(Me.ChangeStatus())
            Case Else
                Dim exMsg As String = IIf(String.IsNullOrEmpty(_REQUEST("action")), "", _REQUEST("action"))
                Throw New Exception("Not found action " & exMsg & ".", New Exception("Please check your action name"))
        End Select
        Return JSONResponse.ToJSON()
    End Function

    Private Function GetData() As AllItem
        Dim data As New AllItem
        data.offence = Me.GetListOffence()
        data.sub_offence = Me.GetListSuboffence()
        data.condition = Me.GetListCriminal()
        Return data
    End Function

    Private Function GetListOffence() As List(Of OffenceItem)
        Dim list_data As New List(Of OffenceItem)
        Dim item_data As OffenceItem
        stmt = "select id, code, name_th ,offence_sequence
                from Offence with(nolock) 
                order by offence_sequence "

        Dim item As DataTable = conn.QueryTable(stmt)
        If item.Rows.Count > 0 Then
            For Each data As DataRow In item.Rows()
                item_data = New OffenceItem
                item_data.offence_id = data("id").ToString()
                item_data.offence_code = data("code").ToString()
                item_data.offence_name = data("name_th").ToString()
                list_data.Add(item_data)
            Next
        End If
        Return list_data
    End Function

    Private Function GetListCriminal() As List(Of CriminalItem)
        Dim list_data As New List(Of CriminalItem)
        Dim item_data As CriminalItem
        stmt = "Select id, code, name_th, sequence
                From Criminal with(nolock)
                where status = 'Active'
                Order by sequence"

        Dim item As DataTable = conn.QueryTable(stmt)
        If item.Rows.Count > 0 Then
            For Each data As DataRow In item.Rows()
                item_data = New CriminalItem
                item_data.criminal_id = data("id").ToString()
                item_data.criminal_code = data("code").ToString()
                item_data.criminal_name = data("name_th").ToString()
                list_data.Add(item_data)
            Next
        End If
        Return list_data
    End Function

    Private Function GetListSuboffence() As List(Of SubOffenceItem)
        Dim list_data As New List(Of SubOffenceItem)
        Dim item_data As SubOffenceItem
        stmt = "select id, code, name_th, offence_id, sub_sequence
                from Sub_Offence with(nolock) 
                order by sub_sequence"

        Dim item As DataTable = conn.QueryTable(stmt)
        If item.Rows.Count > 0 Then
            For Each data As DataRow In item.Rows()
                item_data = New SubOffenceItem
                item_data.suboffence_id = data("id").ToString()
                item_data.suboffence_code = data("code").ToString()
                item_data.suboffence_name = data("name_th").ToString()
                item_data.offence_id = data("offence_id").ToString()
                list_data.Add(item_data)
            Next
        End If
        Return list_data
    End Function

    Private Function GetListEducation() As List(Of EducationItem)
        Dim list_data As New List(Of EducationItem)
        Dim item_data As EducationItem
        stmt = "select id , Name_Education_TH , Name_Education_EN
                from Education_level"

        Dim item As DataTable = conn.QueryTable(stmt)
        If item.Rows.Count > 0 Then
            For Each data As DataRow In item.Rows()
                item_data = New EducationItem
                item_data.Education_id = data("id").ToString()
                item_data.Education_name_th = data("Name_Education_TH").ToString()
                item_data.Education_name_en = data("Name_Education_EN").ToString()
                list_data.Add(item_data)
            Next
        End If
        Return list_data
    End Function

    Private Function ListTitle() As List(Of TitleItem)
        Dim ItemTitle As TitleItem
        Dim ListData As New List(Of TitleItem)
        Dim rowTitle As DataTable = conn.QueryTable("select id, title_name_th from title where status = 'Active' order by status, code")

        If rowTitle.Rows.Count > 0 Then
            For Each title As DataRow In rowTitle.Rows()
                ItemTitle = New TitleItem
                ItemTitle.id = title("id").ToString()
                ItemTitle.name = title("title_name_th").ToString()
                ListData.Add(ItemTitle)
            Next
        End If
        Return ListData
    End Function

    Private Function Probationer() As ProbationerItem
        Dim ItemProbationer As ProbationerItem
        ItemProbationer = New ProbationerItem
        stmt = "select id, title_id, name, last_name, gender_id, age, PID, status, 
                address_number, moo, road, lane, province_id, district_id, sub_district_id, telephone_mobile, telephone,
                case_black_number, case_red_number, caseID, department_id, remark, EM_devices_id, activate_homebase, convert(varchar, IMEI_start_date, 103) AS IMEI_start_date,
                convert(varchar, judge_end_date, 103) AS judge_end_date, convert(varchar, IMEI_end_date, 103) AS IMEI_end_date, date_total, other ,  restricted_area_id
                Civil_Registration_Address, Education_Level, Career, Probation_Officer_Name, Probation_Officer_Telephone,Probation_Officer_LastName,Current_Address_Moo,
                Current_Address_Road, Current_Address_Lane,Current_Address_Subdistrict, Current_Address_District,Current_Address_Provice,Current_Address_Postcode , notification_by_email
                ,notification_by_sms , Current_Address_Number
                from Probationer
                where id = @id"

        param = New SQLCollection
        param.Add("@id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("probationer_id"))))
        Dim data As DataTable = conn.QueryTable(stmt, param)

        If data.Rows.Count > 0 Then
            ItemProbationer.probationer_title = data.Rows(0)("title_id").ToString()
            ItemProbationer.probationer_name = data.Rows(0)("name").ToString()
            ItemProbationer.probationer_last_name = data.Rows(0)("last_name").ToString()
            ItemProbationer.probationer_gender = data.Rows(0)("gender_id").ToString()
            ItemProbationer.probationer_age = data.Rows(0)("age").ToString()
            ItemProbationer.probationer_cardid = data.Rows(0)("PID").ToString()
            ItemProbationer.probationer_status = data.Rows(0)("status").ToString()
            ItemProbationer.probationer_address_number = data.Rows(0)("address_number").ToString()
            ItemProbationer.probationer_moo = data.Rows(0)("moo").ToString()
            ItemProbationer.probationer_road = data.Rows(0)("road").ToString()
            ItemProbationer.probationer_lane = data.Rows(0)("lane").ToString()
            ItemProbationer.probationer_province = data.Rows(0)("province_id").ToString()
            ItemProbationer.probationer_district = data.Rows(0)("district_id").ToString()
            ItemProbationer.probationer_subdistrict = data.Rows(0)("sub_district_id").ToString()
            ItemProbationer.probationer_mobile = data.Rows(0)("telephone_mobile").ToString()
            ItemProbationer.probationer_telephone = data.Rows(0)("telephone").ToString()
            ItemProbationer.probationer_case_black_number = data.Rows(0)("case_black_number").ToString()
            ItemProbationer.probationer_case_red_number = data.Rows(0)("case_red_number").ToString()
            ItemProbationer.probationer_caseID = data.Rows(0)("caseID").ToString()
            ItemProbationer.probationer_department = data.Rows(0)("department_id").ToString()
            ItemProbationer.probationer_remark = data.Rows(0)("remark").ToString()
            ItemProbationer.probationer_EM_devices = data.Rows(0)("EM_devices_id").ToString()
            ItemProbationer.probationer_activate_homebase = data.Rows(0)("activate_homebase").ToString()
            ItemProbationer.probationer_IMEI_start_date = data.Rows(0)("IMEI_start_date").ToString()
            ItemProbationer.probationer_judge_end_date = data.Rows(0)("judge_end_date").ToString()
            ItemProbationer.probationer_IMEI_end_date = data.Rows(0)("IMEI_end_date").ToString()
            ItemProbationer.probationer_date_total = data.Rows(0)("date_total").ToString()
            ItemProbationer.probationer_other = data.Rows(0)("other").ToString()
            ItemProbationer.probationer_education = data.Rows(0)("Education_Level").ToString()
            'ItemProbationer.restricted_area_id = data.Rows(0)("restricted_area_id").ToString()
            ItemProbationer.probationer_career = data.Rows(0)("Career").ToString()
            ItemProbationer.probation_Officer_Name = data.Rows(0)("Probation_Officer_Name").ToString()
            ItemProbationer.probation_Officer_Telephone = data.Rows(0)("Probation_Officer_Telephone").ToString()
            ItemProbationer.probation_Officer_LastName = data.Rows(0)("Probation_Officer_LastName").ToString()
            ItemProbationer.current_moo = data.Rows(0)("Current_Address_Moo").ToString()
            ItemProbationer.current_road = data.Rows(0)("Current_Address_Road").ToString()
            ItemProbationer.current_lane = data.Rows(0)("Current_Address_Lane").ToString()
            ItemProbationer.current_subdistrict = data.Rows(0)("Current_Address_Subdistrict").ToString()
            ItemProbationer.current_district = data.Rows(0)("Current_Address_District").ToString()
            ItemProbationer.current_province = data.Rows(0)("Current_Address_Provice").ToString()
            ItemProbationer.current_postcode = data.Rows(0)("Current_Address_Postcode").ToString()
            ItemProbationer.current_address_number = data.Rows(0)("Current_Address_Number").ToString()
            ItemProbationer.notification_by_sms = data.Rows(0)("notification_by_sms").ToString()
            ItemProbationer.notification_by_email = data.Rows(0)("notification_by_email").ToString()

        End If
        Return ItemProbationer
    End Function
    Private Function GetProbationer() As AllprobationerItem
        Dim data As New AllprobationerItem
        data.Probationer = Me.Probationer()
        data.ProbationerReference = Me.ProbationerReference()
        data.ProbationerOffence = Me.ProbationerOffence()
        data.probationerCriminal = Me.ProbationerCriminal()
        Return data
    End Function

    Public Structure AllprobationerItem
        Public Probationer As ProbationerItem
        Public ProbationerReference As List(Of RefProbaItem)
        Public ProbationerOffence As List(Of SubOffenceItem)
        Public probationerCriminal As List(Of GetProbationerCriminal)
    End Structure

    Private Function ProbationerCriminal() As List(Of GetProbationerCriminal)
        Dim list_data As New List(Of GetProbationerCriminal)
        Dim criminalproba As GetProbationerCriminal
        stmt = "SELECT id,probationer_id,criminal_id,criminal_text ,CONVERT(varchar(5), start_time, 108) AS start_time,CONVERT(varchar(5), end_time, 108) AS end_time
                FROM Probationer_Criminal
                Where probationer_id = @probationer_id "

        param = New SQLCollection
        param.Add("@probationer_id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("probationer_id"))))
        Dim item As DataTable = conn.QueryTable(stmt, param)
        If item.Rows.Count > 0 Then
            For Each data As DataRow In item.Rows()
                criminalproba = New GetProbationerCriminal
                criminalproba.criminal_id = data("criminal_id").ToString()
                criminalproba.criminal_text = data("criminal_text").ToString()
                criminalproba.start_time = data("start_time").ToString()
                criminalproba.end_time = data("end_time").ToString()
                list_data.Add(criminalproba)
            Next
        End If
        Return list_data
    End Function

    Private Function ProbationerOffence() As List(Of SubOffenceItem)
        Dim list_probationeroffence As New List(Of SubOffenceItem)
        Dim offenceproba As SubOffenceItem

        stmt = "SELECT id ,system_staff ,system_date ,update_staff,update_date,offence_id,sub_offence_id,probationer_id,sub_offence_text,status
FROM Probationer_Offence
where probationer_id = @probationer_id"
        param = New SQLCollection
        param.Add("@probationer_id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("probationer_id"))))
        Dim Item As DataTable = conn.QueryTable(stmt, param)

        If Item.Rows.Count > 0 Then
            For Each data As DataRow In Item.Rows()
                offenceproba = New SubOffenceItem
                offenceproba.offence_id = data("offence_id").ToString()
                offenceproba.suboffence_id = data("sub_offence_id").ToString()
                offenceproba.suboffence_text = data("sub_offence_text").ToString()
                offenceproba.suboffence_status = data("status").ToString()
                list_probationeroffence.Add(offenceproba)
            Next
        End If
        Return list_probationeroffence

    End Function

    Private Function ProbationerReference() As List(Of RefProbaItem)
        Dim list_probareference As New List(Of RefProbaItem)
        Dim personreference As RefProbaItem
        stmt = "select Person_Reference_Name  ,Person_Reference_LastName ,Person_Reference_IdCard ,Person_Reference_Telephone ,Civil_Registration_Address_Moo ,Civil_Registration_Address_Road
      ,Civil_Registration_Address_Lane ,Civil_Registration_Address_Subdistrict ,Civil_Registration_Address_District ,Civil_Registration_Address_Provice
      ,Civil_Registration_Address_Postcode ,Current_Address_Moo ,Current_Address_Road,Current_Address_Lane,Current_Address_Subdistrict ,Current_Address_District ,Current_Address_Provice
      ,Current_Address_Postcode ,  Current_Address_Number ,Civil_Registration_Address_Number,Person_Reference_TitleName ,Person_Reference_Age,Person_Reference_Gender
        from Person_Reference
                where probationer_id = @probationer_id"

        param = New SQLCollection
        param.Add("@probationer_id", DbType.Int64, nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(_REQUEST("probationer_id"))))
        Dim Item As DataTable = conn.QueryTable(stmt, param)

        If Item.Rows.Count > 0 Then
            For Each data As DataRow In Item.Rows()
                personreference = New RefProbaItem
                personreference.reference_name = data("Person_Reference_Name").ToString()
                personreference.reference_last_name = data("Person_Reference_LastName").ToString()
                personreference.reference_title = data("Person_Reference_TitleName").ToString()
                personreference.reference_gender = data("Person_Reference_Gender").ToString()
                personreference.reference_age = data("Person_Reference_Age").ToString()
                personreference.reference_cardid = data("Person_Reference_IdCard").ToString()
                personreference.reference_mobile = data("Person_Reference_Telephone").ToString()
                personreference.reference_address_number = data("Civil_Registration_Address_Number").ToString()
                personreference.reference_moo = data("Civil_Registration_Address_Moo").ToString()
                personreference.reference_road = data("Civil_Registration_Address_Road").ToString()
                personreference.reference_lane = data("Civil_Registration_Address_Lane").ToString()
                personreference.reference_subdistrict = data("Civil_Registration_Address_Subdistrict").ToString()
                personreference.reference_district = data("Civil_Registration_Address_District").ToString()
                personreference.reference_province = data("Civil_Registration_Address_Provice").ToString()
                'personreference.reference_postcode = data("Civil_Registration_Address_Postcode").ToString()
                personreference.reference_current_address_number = data("Current_Address_Number").ToString()
                personreference.reference_current_moo = data("Current_Address_Moo").ToString()
                personreference.reference_current_road = data("Current_Address_Road").ToString()
                personreference.reference_current_lane = data("Current_Address_Lane").ToString()
                personreference.reference_current_subdistrict = data("Current_Address_Subdistrict").ToString()
                personreference.reference_current_district = data("Current_Address_District").ToString()
                personreference.reference_current_province = data("Current_Address_Provice").ToString()
                'personreference.reference_current_postcode = data("Current_Address_Postcode").ToString()
                list_probareference.Add(personreference)
            Next
        End If
        Return list_probareference
    End Function

    Private Function ListProbationer() As List(Of ProbationerListItem)
        Dim ListData As New List(Of ProbationerListItem)
        Dim ItemProbationer As ProbationerListItem
        stmt = "select p.id, (name +'  '+ last_name) AS fullname, PID, p.telephone_mobile, caseID, department_name_th, EM_Devices.IMEI, 
                convert(varchar, IMEI_start_date, 111) AS IMEI_start_date, p.status
                from Probationer p
                left join Department on p.department_id = Department.id
                left join EM_Devices on p.EM_devices_id = EM_Devices.id
                order by p.status, fullname"

        Dim rowProbationer As DataTable = conn.QueryTable(stmt)
        If rowProbationer.Rows.Count > 0 Then
            For Each proba As DataRow In rowProbationer.Rows()
                ItemProbationer = New ProbationerListItem
                ItemProbationer.pid = HttpContext.Current.Server.UrlEncode(nsEngine.nsEngine.Encrypt(proba("id").ToString()))
                ItemProbationer.name = proba("fullname").ToString()
                ItemProbationer.personal_ID = proba("PID").ToString()
                ItemProbationer.caseID = proba("caseID").ToString()
                ItemProbationer.department = proba("department_name_th").ToString()
                ItemProbationer.mobile = proba("telephone_mobile").ToString()
                ItemProbationer.device = proba("IMEI").ToString()
                ItemProbationer.IMEI_start_date = proba("IMEI_start_date").ToString()
                ItemProbationer.status = proba("status").ToString()
                ListData.Add(ItemProbationer)
            Next
        End If
        Return ListData
    End Function

    Private Function SaveProbationer() As Item
        Dim data As New Item
        Dim probationer As ProbationerItem = JSON.Deserialize(Of ProbationerItem)(_REQUEST("probationer"))
        Dim refprobaItem As List(Of RefProbaItem) = JSON.Deserialize(Of List(Of RefProbaItem))(_REQUEST("refprobaItem"))
        Dim offenceItem As List(Of SubOffenceItem) = JSON.Deserialize(Of List(Of SubOffenceItem))(_REQUEST("offenceItem"))
        Dim criminalItem As List(Of GetProbationerCriminal) = JSON.Deserialize(Of List(Of GetProbationerCriminal))(_REQUEST("criminalItem"))
        Dim pid As String

        param = New SQLCollection

        param.Add("@PID", DbType.String, probationer.probationer_cardid)
        param.Add("@staff", DbType.String, "0")
        param.Add("@title_id", DbType.String, probationer.probationer_title)
        param.Add("@name", DbType.String, probationer.probationer_name)
        param.Add("@last_name", DbType.String, probationer.probationer_last_name)
        param.Add("@gender_id", DbType.String, probationer.probationer_gender)
        param.Add("@age", DbType.String, probationer.probationer_age)
        param.Add("@status", DbType.String, probationer.probationer_status)
        param.Add("@address_number", DbType.String, probationer.probationer_address_number)
        param.Add("@moo", DbType.String, probationer.probationer_moo)
        param.Add("@road", DbType.String, probationer.probationer_road)
        param.Add("@lane", DbType.String, probationer.probationer_lane)
        param.Add("@province_id", DbType.String, probationer.probationer_province)
        param.Add("@district_id", DbType.String, probationer.probationer_district)
        param.Add("@sub_district_id", DbType.String, probationer.probationer_subdistrict)
        param.Add("@telephone_mobile", DbType.String, probationer.probationer_mobile)
        param.Add("@telephone", DbType.String, probationer.probationer_telephone)
        param.Add("@case_black_number", DbType.String, probationer.probationer_case_black_number)
        param.Add("@case_red_number", DbType.String, probationer.probationer_case_red_number)
        param.Add("@caseID", DbType.String, probationer.probationer_caseID)
        param.Add("@department_id", DbType.String, probationer.probationer_department)
        param.Add("@IMEI_start_date", DbType.Date, Convert.ToDateTime(probationer.probationer_IMEI_start_date, THInfo).ToString("dd-MM-yyyy"))
        param.Add("@IMEI_end_date", DbType.Date, Convert.ToDateTime(probationer.probationer_IMEI_end_date, THInfo).ToString("dd-MM-yyyy"))
        param.Add("@judge_end_date", DbType.Date, Convert.ToDateTime(probationer.probationer_judge_end_date, THInfo).ToString("dd-MM-yyyy"))
        param.Add("@date_total", DbType.String, probationer.probationer_date_total)
        param.Add("@remark", DbType.String, probationer.probationer_remark)
        param.Add("@EM_devices_id", DbType.String, probationer.probationer_EM_devices)
        'param.Add("@activate_homebase", DbType.String, probationer.probationer_activate_homebase)
        param.Add("@activate_homebase", DbType.String, "Y")
        param.Add("@other", DbType.String, probationer.probationer_other)
        param.Add("@restricted_area_id", DbType.String, probationer.restricted_area_id)
        param.Add("@Civil_Registration_Address", DbType.String, probationer.civil_registration_address)
        param.Add("@Education_Level", DbType.String, probationer.probationer_education)
        param.Add("@Career", DbType.String, probationer.probationer_career)
        param.Add("@Probation_Officer_Name", DbType.String, probationer.probation_Officer_Name)
        param.Add("@Probation_Officer_Telephone", DbType.String, probationer.probation_Officer_Telephone)
        param.Add("@Probation_Officer_LastName", DbType.String, probationer.probation_Officer_LastName)
        param.Add("@Current_Address_Moo", DbType.String, probationer.current_moo)
        param.Add("@Current_Address_Road", DbType.String, probationer.current_road)
        param.Add("@Current_Address_Lane", DbType.String, probationer.current_lane)
        param.Add("@Current_Address_Subdistrict", DbType.String, probationer.current_subdistrict)
        param.Add("@Current_Address_District", DbType.String, probationer.current_district)
        param.Add("@Current_Address_Provice", DbType.String, probationer.current_province)
        param.Add("@Current_Address_Postcode", DbType.String, probationer.current_postcode)
        param.Add("@notification_by_email", DbType.String, probationer.notification_by_email)
        param.Add("@notification_by_sms", DbType.String, probationer.notification_by_sms)
        param.Add("@Current_Address_Number", DbType.String, probationer.current_address_number)


        If Not String.IsNullOrEmpty(probationer.pid) Then
            probationer.pid = nsEngine.nsEngine.Decrypt(HttpUtility.UrlDecode(probationer.pid))

            stmt = "update Probationer set 
                        update_staff = 0, 
                        update_date = GETDATE(), 
                        title_id = @title_id,
                        name =  @name,
                        last_name = @last_name,
                        gender_id = @gender_id,
                        age = @age,
                        PID = @PID,
                        status = @status,
                        address_number = @address_number,
                        moo = @moo,
                        road = @road,
                        lane = @lane,
                        province_id = @province_id,
                        district_id = @district_id,
                        sub_district_id = @sub_district_id,
                        telephone_mobile = @telephone_mobile,
                        telephone = @telephone,
                        case_black_number = @case_black_number,
                        case_red_number = @case_red_number,
                        caseID = @caseID,
                        department_id = @department_id,
                        IMEI_start_date = GETDATE(),
                        judge_end_date = GETDATE(),
                        IMEI_end_date = GETDATE(),
                        date_total    = @date_total ,
                        remark   =   @remark,
                        EM_devices_id = @EM_devices_id,
                        activate_homebase = @activate_homebase,
                        other = @other,
                        notification_by_website = 'www.test.com',
                        notification_by_email = @notification_by_email,
                        notification_by_sms = @notification_by_sms,
                        officer_monitoring_id = 77,
                        restricted_area_id = @restricted_area_id,
                        Civil_Registration_Address = @Civil_Registration_Address,
                        Education_Level = @Education_Level,
                        Career = @Career,
                        Probation_Officer_Name = @Probation_Officer_Name,
                        Probation_Officer_Telephone = @Probation_Officer_Telephone,
                        Probation_Officer_LastName = @Probation_Officer_LastName,
                        Current_Address_Moo = @Current_Address_Moo,
                        Current_Address_Road = @Current_Address_Road,
                        Current_Address_Lane = @Current_Address_Lane,
                        Current_Address_Subdistrict = @Current_Address_Subdistrict,
                        Current_Address_District = @Current_Address_District,
                        Current_Address_Provice = @Current_Address_Provice,
                        Current_Address_Postcode = @Current_Address_Postcode,
                        Current_Address_Number = @Current_Address_Number

                    where id = @id"
            param.Add("@id", DbType.Int64, probationer.pid)
            conn.Execute(stmt, param)

            'delete Person_Reference
            conn.Execute("delete from Person_Reference where Probationer_id = @id", param)

            'insert Person_Reference
            stmt = "insert into Person_Reference(Probationer_id, Person_Reference_Name, Person_Reference_LastName, Person_Reference_IdCard,Person_Reference_Telephone
,Civil_Registration_Address_Moo,Civil_Registration_Address_Road,Civil_Registration_Address_Lane,Civil_Registration_Address_Subdistrict,Civil_Registration_Address_District
,Civil_Registration_Address_Provice,Civil_Registration_Address_Postcode,Current_Address_Moo,Current_Address_Road,Current_Address_Lane
,Current_Address_Subdistrict,Current_Address_District,Current_Address_Provice,Current_Address_Postcode ,Current_Address_Number
                    ,Civil_Registration_Address_Number  ,Person_Reference_TitleName ,Person_Reference_Age ,Person_Reference_Gender) 

                    values (@Probationer_id, @Person_Reference_Name, @Person_Reference_LastName, @Person_Reference_IdCard,@Person_Reference_Telephone
,@Civil_Registration_Address_Moo,@Civil_Registration_Address_Road,@Civil_Registration_Address_Lane,@Civil_Registration_Address_Subdistrict,@Civil_Registration_Address_District
,@Civil_Registration_Address_Provice,@Civil_Registration_Address_Postcode,@Current_Address_Moo,@Current_Address_Road,@Current_Address_Lane
,@Current_Address_Subdistrict,@Current_Address_District,@Current_Address_Provice,@Current_Address_Postcode  , @Current_Address_Number , @Civil_Registration_Address_Number
                    , @Person_Reference_TitleName,@Person_Reference_Age , @Person_Reference_Gender)"

            For Each item In refprobaItem
                param = New SQLCollection
                param.Add("@Person_Reference_Name", DbType.String, item.reference_name)
                param.Add("@Person_Reference_LastName", DbType.String, item.reference_last_name)
                param.Add("@Person_Reference_IdCard", DbType.String, item.reference_cardid)
                param.Add("@Person_Reference_Telephone", DbType.String, item.reference_mobile)
                param.Add("@Civil_Registration_Address_Moo", DbType.String, item.reference_moo)
                param.Add("@Civil_Registration_Address_Road", DbType.String, item.reference_road)
                param.Add("@Civil_Registration_Address_Lane", DbType.String, item.reference_lane)
                param.Add("@Civil_Registration_Address_Subdistrict", DbType.String, item.reference_subdistrict)
                param.Add("@Civil_Registration_Address_District", DbType.String, item.reference_district)
                param.Add("@Civil_Registration_Address_Provice", DbType.String, item.reference_province)
                param.Add("@Civil_Registration_Address_Postcode", DbType.String, item.reference_postcode)
                param.Add("@Current_Address_Moo", DbType.String, item.reference_current_moo)
                param.Add("@Current_Address_Road", DbType.String, item.reference_current_road)
                param.Add("@Current_Address_Lane", DbType.String, item.reference_current_lane)
                param.Add("@Current_Address_Subdistrict", DbType.String, item.reference_current_subdistrict)
                param.Add("@Current_Address_District", DbType.String, item.reference_current_district)
                param.Add("@Current_Address_Provice", DbType.String, item.reference_current_province)
                param.Add("@Current_Address_Postcode", DbType.String, item.reference_current_postcode)
                param.Add("@Current_Address_Number", DbType.String, item.reference_current_address_number)
                param.Add("@Civil_Registration_Address_Number", DbType.String, item.reference_address_number)
                param.Add("@Person_Reference_TitleName", DbType.String, item.reference_title)
                param.Add("@Person_Reference_Age", DbType.String, item.reference_age)
                param.Add("@Person_Reference_Gender", DbType.String, item.reference_gender)
                param.Add("@Probationer_id", DbType.Int64, probationer.pid)

                conn.Execute(stmt, param)
            Next

            'delete Probationer_Offence
            conn.Execute("delete from Probationer_Offence where probationer_id = @probationer_id", param)

            'insert Probationer_Offence
            stmt = "insert into Probationer_Offence(system_staff,system_date ,offence_id,sub_offence_id,probationer_id,status) 

                    values (0 ,GETDATE(), @offence_id , @sub_offence_id ,@probationer_id,'Active')"

            For Each offence In offenceItem
                param = New SQLCollection
                param.Add("@offence_id", DbType.Int64, offence.offence_id)
                param.Add("@sub_offence_id", DbType.Int64, offence.suboffence_id)
                'param.Add("@sub_offence_text", DbType.String, offence.suboffence_text)
                param.Add("@probationer_id", DbType.Int64, probationer.pid)

                conn.Execute(stmt, param)
            Next

            'delete Probationer_Criminal
            conn.Execute("delete from Probationer_Criminal where probationer_id = @probationer_id", param)

            'insert Probationer_Criminal
            stmt = "insert into Probationer_Criminal(probationer_id ,criminal_id ,criminal_text ,start_time,end_time)
                    values ( @probationer_id ,@criminal_id ,@criminal_text ,@start_time ,@end_time)"

            For Each criminal In criminalItem
                param = New SQLCollection
                param.Add("@criminal_id", DbType.String, criminal.criminal_id)
                param.Add("@criminal_text", DbType.String, criminal.criminal_text)
                param.Add("@start_time", DbType.String, criminal.start_time)
                param.Add("@end_time", DbType.String, criminal.end_time)
                param.Add("@Probationer_id", DbType.Int64, probationer.pid)

                conn.Execute(stmt, param)
            Next

            data.status = "success"
            data.txtAlert = "แก้ไขข้อมูลสำเร็จ"

        Else
            Dim person_id As String = conn.QueryField("select id from Probationer with(nolock) where PID = @PID", param)
            If Not String.IsNullOrEmpty(person_id) Then
                data.status = "fail"
                data.txtAlert = "ไม่สามารถบันทึกข้อมูลผู้ถูกคุมความประพฤตินี้ได้ เนื่องจากมีข้อมูลรหัสบัตรประชาชนผู้ถูกคุมความประพฤตินี้แล้วนี้อยู่แล้ว"
            Else
                stmt = "insert into Probationer 
                        (system_staff, system_date, title_id, name, last_name, gender_id, age, PID, status, 
                        address_number, moo, road, lane, province_id, district_id, sub_district_id, telephone_mobile, telephone,
                        judge_name, case_black_number, case_red_number, caseID, department_id, remark, 
                        EM_devices_id, activate_homebase, IMEI_start_date, judge_end_date, IMEI_end_date, date_total, 
                        other, officer_monitoring_id, notification_by_website, notification_by_email, notification_by_sms,
                        restricted_area_id ,Civil_Registration_Address  ,Education_Level ,Career  ,Probation_Officer_Name  ,Probation_Officer_Telephone
                        ,Probation_Officer_LastName,Current_Address_Moo,Current_Address_Road ,Current_Address_Lane ,Current_Address_Subdistrict
                        ,Current_Address_District ,Current_Address_Provice ,Current_Address_Postcode)

                            values (@staff, GETDATE(), @title_id, @name, @last_name, @gender_id, @age, @PID, @status, 
                        @address_number, @moo, @road, @lane, @province_id, @district_id, @sub_district_id, @telephone_mobile, @telephone,
                        @judge_name, @case_black_number, @case_red_number, @caseID, @department_id, @remark, 
                        @EM_devices_id, @activate_homebase, @IMEI_start_date, @judge_end_date, @IMEI_end_date, @date_total, 
                        @other, 77, 'www.test.com', @notification_by_email, @notification_by_email
                        ,@restricted_area_id ,@Civil_Registration_Address,@Education_Level ,@Career ,@Probation_Officer_Name
                       ,@Probation_Officer_Telephone ,@Probation_Officer_LastName  ,@Current_Address_Moo ,@Current_Address_Road  ,@Current_Address_Lane
                       ,@Current_Address_Subdistrict ,@Current_Address_District,@Current_Address_Provice ,@Current_Address_Postcode)"

                pid = conn.Execute(stmt, param)

                stmt = "insert into Person_Reference(Probationer_id, Person_Reference_Name, Person_Reference_LastName, Person_Reference_IdCard,Person_Reference_Telephone
,Civil_Registration_Address_Moo,Civil_Registration_Address_Road,Civil_Registration_Address_Lane,Civil_Registration_Address_Subdistrict,Civil_Registration_Address_District
,Civil_Registration_Address_Provice,Civil_Registration_Address_Postcode,Current_Address_Moo,Current_Address_Road,Current_Address_Lane
,Current_Address_Subdistrict,Current_Address_District,Current_Address_Provice,Current_Address_Postcode ,Current_Address_Number
                    ,Civil_Registration_Address_Number  ,Person_Reference_TitleName ,Person_Reference_Age ,Person_Reference_Gender) 

                    values (@Probationer_id, @Person_Reference_Name, @Person_Reference_LastName, @Person_Reference_IdCard,@Person_Reference_Telephone
,@Civil_Registration_Address_Moo,@Civil_Registration_Address_Road,@Civil_Registration_Address_Lane,@Civil_Registration_Address_Subdistrict,@Civil_Registration_Address_District
,@Civil_Registration_Address_Provice,@Civil_Registration_Address_Postcode,@Current_Address_Moo,@Current_Address_Road,@Current_Address_Lane
,@Current_Address_Subdistrict,@Current_Address_District,@Current_Address_Provice,@Current_Address_Postcode  , @Current_Address_Number , @Civil_Registration_Address_Number
                    , @Person_Reference_TitleName,@Person_Reference_Age , @Person_Reference_Gender)"

                For Each item In refprobaItem
                    param = New SQLCollection
                    param.Add("@Probationer_id", DbType.Int64, pid)
                    param.Add("@Person_Reference_Name", DbType.String, item.reference_name)
                    param.Add("@Person_Reference_LastName", DbType.String, item.reference_last_name)
                    param.Add("@Person_Reference_IdCard", DbType.String, item.reference_cardid)
                    param.Add("@Person_Reference_Telephone", DbType.String, item.reference_mobile)
                    param.Add("@Civil_Registration_Address_Moo", DbType.String, item.reference_moo)
                    param.Add("@Civil_Registration_Address_Road", DbType.String, item.reference_road)
                    param.Add("@Civil_Registration_Address_Lane", DbType.String, item.reference_lane)
                    param.Add("@Civil_Registration_Address_Subdistrict", DbType.String, item.reference_subdistrict)
                    param.Add("@Civil_Registration_Address_District", DbType.String, item.reference_district)
                    param.Add("@Civil_Registration_Address_Provice", DbType.String, item.reference_province)
                    param.Add("@Civil_Registration_Address_Postcode", DbType.String, item.reference_postcode)
                    param.Add("@Current_Address_Moo", DbType.String, item.reference_current_moo)
                    param.Add("@Current_Address_Road", DbType.String, item.reference_current_road)
                    param.Add("@Current_Address_Lane", DbType.String, item.reference_current_lane)
                    param.Add("@Current_Address_Subdistrict", DbType.String, item.reference_current_subdistrict)
                    param.Add("@Current_Address_District", DbType.String, item.reference_current_district)
                    param.Add("@Current_Address_Provice", DbType.String, item.reference_current_province)
                    param.Add("@Current_Address_Postcode", DbType.String, item.reference_current_postcode)
                    param.Add("@Current_Address_Number", DbType.String, item.reference_current_address_number)
                    param.Add("@Civil_Registration_Address_Number", DbType.String, item.reference_address_number)
                    param.Add("@Person_Reference_TitleName", DbType.String, item.reference_title)
                    param.Add("@Person_Reference_Age", DbType.String, item.reference_age)
                    param.Add("@Person_Reference_Gender", DbType.String, item.reference_gender)
                    conn.Execute(stmt, param)
                Next

                pid = conn.Execute(stmt, param)

                stmt = "insert into Probationer_Offence(system_staff,system_date,offence_id,sub_offence_id,sub_offence_text,status)
                    values (@system_staff, GETDATE(),@offence_id,@sub_offence_id,@sub_offence_text,@status)"

                For Each item In offenceItem
                    param = New SQLCollection
                    param.Add("@Probationer_id", DbType.Int64, pid)
                    param.Add("@offence_id", DbType.String, item.offence_id)
                    param.Add("@sub_offence_id", DbType.String, item.suboffence_id)
                    param.Add("@sub_offence_text", DbType.String, item.suboffence_text)
                    conn.Execute(stmt, param)
                Next

                pid = conn.Execute(stmt, param)

                stmt = "insert into Probationer_Criminal(probationer_id ,criminal_id ,criminal_text ,start_time,end_time)
                    values ( @probationer_id ,@criminal_id ,@criminal_text ,@start_time ,@end_time)"

                For Each criminal In criminalItem
                    param = New SQLCollection
                    param.Add("@criminal_id", DbType.String, criminal.criminal_id)
                    param.Add("@criminal_text", DbType.String, criminal.criminal_text)
                    param.Add("@start_time", DbType.String, criminal.start_time)
                    param.Add("@end_time", DbType.String, criminal.end_time)
                    param.Add("@Probationer_id", DbType.Int64, pid)

                    conn.Execute(stmt, param)
                Next

                data.status = "success"
                data.txtAlert = "บันทึกข้อมูลสำเร็จ"
            End If
        End If
        Return data
    End Function

    Private Function ChangeStatus() As Item
        Dim id As String = _REQUEST("data")
        Dim id_split As String()
        Dim result As String()
        Dim data As New Item

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
                conn.Execute("update Probationer SET update_staff = @staff, update_date = GETDATE(), status = @status where id = @id", param)
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
            conn.Execute("update Probationer SET update_staff = @staff, update_date = GETDATE(), status = @status where id = @id", param)
        End If

        data.status = "success"
        data.txtAlert = "เปลี่ยนสถานะข้อมูลสำเร็จ"
        Return data
    End Function

    Public Structure Item
        Public status As String
        Public txtAlert As String
    End Structure

    Public Structure TitleItem
        Public id As String
        Public name As String
    End Structure

    Public Structure GetProbationerCriminal
        Public probationer_id As String
        Public criminal_id As String
        Public criminal_text As String
        Public start_time As String
        Public end_time As String
    End Structure

    Public Structure GetProbationerOffence
        Public offence_id As String
        Public sub_offence_id As String
        Public sub_offence_text As String
    End Structure

    Public Structure DataItem
        Public data_id As String
        Public data_code As String
        Public data_name As String
    End Structure

    Public Structure OffenceItem
        Public offence_id As String
        Public offence_name As String
        Public offence_code As String
    End Structure

    Public Structure SubOffenceItem
        Public offence_id As String
        Public suboffence_id As String
        Public suboffence_name As String
        Public suboffence_code As String
        Public suboffence_text As String
        Public suboffence_status As String
    End Structure
    Public Structure EducationItem
        Public Education_id As String
        Public Education_name_th As String
        Public Education_name_en As String
    End Structure

    Public Structure CriminalItem
        Public criminal_id As String
        Public criminal_code As String
        Public criminal_name As String
    End Structure

    Public Structure AllItem
        Public offence As List(Of OffenceItem)
        Public sub_offence As List(Of SubOffenceItem)
        Public condition As List(Of CriminalItem)
    End Structure

    Public Structure ProbationerListItem
        Public pid As String
        Public personal_ID As String
        Public name As String
        Public caseID As String
        Public department As String
        Public mobile As String
        Public device As String
        Public IMEI_start_date As String
        Public status As String
    End Structure

    Public Structure ProbationerItem
        Public pid As String
        Public probationer_status As String
        Public probationer_title As String
        Public probationer_name As String
        Public probationer_last_name As String
        Public probationer_gender As String
        Public probationer_age As String
        Public probationer_cardid As String
        Public probationer_education As String
        Public probationer_career As String
        Public probationer_mobile As String
        Public probationer_telephone As String
        Public probationer_address_number As String
        Public probationer_moo As String
        Public probationer_road As String
        Public probationer_lane As String
        Public probationer_province As String
        Public probationer_district As String
        Public probationer_subdistrict As String
        Public current_address_number As String
        Public current_moo As String
        Public current_road As String
        Public current_lane As String
        Public current_province As String
        Public current_district As String
        Public current_subdistrict As String
        Public current_postcode As String
        Public notification_by_email As String
        Public notification_by_sms As String
        Public probationer_judge_name As String
        Public probationer_case_black_number As String
        Public probationer_case_red_number As String
        Public probationer_caseID As String
        Public probationer_department As String
        Public probationer_remark As String
        Public probationer_EM_devices As String
        Public probationer_activate_homebase As String
        Public probationer_IMEI As String
        Public probationer_IMEI_start_date As String
        Public probationer_judge_end_date As String
        Public probationer_IMEI_end_date As String
        Public probationer_date_total As String
        Public probationer_other As String
        Public restricted_area_id As String
        Public civil_registration_address As String
        Public probation_Officer_Name As String
        Public probation_Officer_Telephone As String
        Public probation_Officer_LastName As String

    End Structure
    Public Structure RefProbaItem
        Public prorefid As String
        Public reference_title As String
        Public reference_id As String
        Public reference_name As String
        Public reference_last_name As String
        Public reference_age As String
        Public reference_gender As String
        Public reference_cardid As String
        Public reference_mobile As String
        Public reference_address_number As String
        Public reference_moo As String
        Public reference_road As String
        Public reference_lane As String
        Public reference_subdistrict As String
        Public reference_district As String
        Public reference_province As String
        Public reference_postcode As String
        Public reference_current_address_number As String
        Public reference_current_moo As String
        Public reference_current_road As String
        Public reference_current_lane As String
        Public reference_current_subdistrict As String
        Public reference_current_district As String
        Public reference_current_province As String
        Public reference_current_postcode As String
        Public reference_probationer_id As String
    End Structure

End Class





