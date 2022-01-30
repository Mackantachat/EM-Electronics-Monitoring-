<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="add_user_account.aspx.vb" Inherits="EM.add_user_account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/resources_constant/AdminLTE2/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css">
    <link rel="stylesheet" href="/resources_constant/css/bootstrap-switch/bootstrap-switch.css">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2.min.css">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2-bootstrap.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box">
            <div class="row text-header datacontent">
                <div class="col-md-6">
                    <h3 class="text-light-blue"><span id="subject">เพิ่มข้อมูลบัญชีผู้ใช้งาน</span></h3>
                </div>
                <div class="clearfix"></div>
            </div>
            <form>
                <div class="datacontent">
                    <div class="row">
                        <div class="col-md-12 text-right hidden" id="btnstatus">
                            <h5><b>สถานะ<b class="text-red">*</b></b></h5>
                            <input type="checkbox" id="status" name="status" checked data-on-color="success" data-on-text="ใช้งาน" data-off-color="danger" data-off-text="ไม่ใช้งาน">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <h5><b>บัญชีผู้ใช้งาน</b><b class="text-red">*</b></h5>
                            <input type="text" class="form-control required" id="username" placeholder="บัญชีผู้ใช้งาน">
                            <h5 class="text-aqua">บัญชีผู้ใช้งานควรจะมีความยาวอย่างน้อย 6 - 12 ตัวอักษร โดยใช้อักษรภาษาอังกฤษหรือตัวเลขในการกำหนด</h5>
                        </div>
                        <div class="col-md-4">
                            <h5><b>รหัสผ่าน</b><b class="text-red">*</b></h5>
                            <input type="password" class="form-control required" id="password" placeholder="รหัสผ่าน">
                            <h5 class="text-aqua">รหัสผ่านควรจะมีความยาวอย่างน้อย 6 - 12 ตัวอักษร โดยใช้อักษรภาษาอังกฤษหรือตัวเลขในการกำหนด</h5>
                        </div>
                        <div class="col-md-4">
                            <h5><b>ยืนยันรหัสผ่าน</b><b class="text-red">*</b></h5>
                            <input type="password" class="form-control" id="confirm_password" placeholder="ยืนยันรหัสผ่าน">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <h5><i class="fa fa-exclamation-circle"></i><b>ข้อมูลส่วนตัวผู้ใช้งาน</b></h5>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <h5><b>ชื่อผู้ใช้งาน</b><b class="text-red">*</b></h5>
                            <input type="text" class="form-control required" id="name" placeholder="ชื่อผู้ใช้งาน">
                        </div>
                        <div class="col-md-6">
                            <h5><b>นามสกุล</b><b class="text-red">*</b></h5>
                            <input type="text" class="form-control required" id="lastname" placeholder="นามสกุล">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <h5><b>รหัสประจำตัวประชาชน</b><b class="text-red">*</b></h5>
                            <input type="text" class="form-control required" id="PID" placeholder="รหัสประจำตัวประชาชน" maxlength="13">
                        </div>
                        <div class="col-md-4">
                            <h5><b>อีเมล์</b><b class="text-red">*</b></h5>
                            <input type="text" class="form-control required" id="email" placeholder="อีเมลล์">
                        </div>
                        <div class="col-md-4">
                            <h5><b>เบอร์โทรศัพท์</b></h5>
                            <input type="text" class="form-control" id="tel" placeholder="เบอร์โทรศัพท์">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <h5><b>วันเดือนปีเกิด</b><b class="text-red">*</b></h5>
                            <input type="text" class="form-control required" id="birthday" placeholder="วัน/เดือน/ปี" data-date-format="dd/mm/yyyy">
                        </div>
                        <div class="col-md-4">
                            <h5><b>หน่วยงาน</b><b class="text-red">*</b></h5>
                            <select class="form-control required" id="department" data-placeholder="หน่วยงาน">
                                <option selected></option>
                            </select>
                        </div>
                        <div class="col-md-4">
                            <h5><b>ระดับสิทธิ</b><b class="text-red">*</b></h5>
                            <select class="form-control required" id="user_role" data-placeholder="ระดับสิทธิ">
                                <option selected></option>
                            </select>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <h5><b>กลุ่มผู้ใช้งาน</b><b class="text-red">*</b></h5>
                            <select class="form-control required" id="user_group" data-placeholder="กลุ่มผู้ใช้งาน">
                                <option selected></option>
                            </select>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="text-center divbutton">
                                <a href="/user/user_account" class="btn btnback">
                                    <i class="fa fa-angle-left" aria-hidden="true"></i>ยกเลิก
                                </a>
                                <a href="#" class="btn btnsave">
                                    <i class="fa fa-check" aria-hidden="true"></i>บันทึกข้อมูล
                                </a>
                                <input class="hidden" type="text" name="id" id="id" />
                            </div>
                        </div>
                    </div>
                </div>
            </form>
            <div id="select-template" class="hidden">
                <select>
                    <option></option>
                </select>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JsContent" runat="server">
    <script src="/resources_constant/AdminLTE2/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.js"></script>
    <script src="/resources_constant/AdminLTE2/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker-custom.js"></script>
    <script src="/resources_constant/AdminLTE2/bower_components/bootstrap-datepicker/js/locales/bootstrap-datepicker.th.js"></script>
    <script src="/resources_constant/AdminLTE2/bower_components/moment/min/moment-with-locales.min.js"></script>
    <script src="/resources_constant/js/bootstrap-switch/bootstrap-switch.js"></script>
    <script src="/resources_constant/js/jquery.mask/dist/jquery.mask.js"></script>
    <script src="/resources_constant/js/select2/select2.js"></script>
    <script src="/resource/js/select_role.js"></script>
    <script src="/resource/js/select_department.js"></script>
    <script src="/resource/js/select_group.js"></script>
    <script src="/resource/js/custom.js"></script>
    <script type="text/javascript">
        $(function () {
            set_navigation($(".btnback").attr("href"));
            phone("#tel");
            validemail("#email");
            character("#name, #surname");
            usepass("#username, #password, #confirm_password");
            select_role("#user_role");
            select_group("#user_group");
            select_department("#department");
            $("#PID").NSNumberboxInteger();
            $("#birthday").datepicker({
                format: 'dd/mm/yyyy',
                todayBtn: true,
                language: 'th',
                thaiyear: true,
            }).datepicker("", new Date());

            $("#birthday").mask("AA/AA/AAAA", {
                "translation": { 0: { pattern: /[0-9]/g } }, onKeyPress: function (value, event) {
                    event.currentTarget.value = value.toUpperCase();
                }
            });

            var param = getparameters();
            if (param.data != null) { set_data(); }

            function set_data() {
                $("#btnstatus").removeClass("hidden");
                $("#status").bootstrapSwitch("size", "small");
                $("#status").bootstrapSwitch("state", true);
                $("#subject").html("แก้ไขข้อมูลบัญชีผู้ใช้งาน");
                $.ajax({
                    url: "../control/user.aspx",
                    data: {
                        action: "user_account",
                        account_id: param.data,
                    },
                    beforeSend: function () {
                        $.busyLoadFull("show", { spinner: "circles" });
                    },
                    type: "POST",
                    dataType: "json",
                    error: function () {
                        alertify.alert("error display data.");
                        $.busyLoadFull("hide");
                    },
                    success: function (data) {
                        setTimeout(function () {
                            if (!data.onError) {
                                items = jQuery.parseJSON(data.getItems);
                                $("#id").NSValue(param.data);
                                $("#username").NSValue(items.username).NSDisable();
                                $("#password").NSValue(items.password).NSDisable();
                                $("#confirm_password").NSValue(items.password).NSDisable();
                                $("#name").NSValue(items.name);
                                $("#lastname").NSValue(items.lastname);
                                $("#PID").NSValue(items.PID);
                                $("#email").NSValue(items.email);
                                $("#tel").NSValue(items.tel);
                                $("#birthday").datepicker("setDate", items.birthdate);
                                $("#department").NSValue(items.department);
                                $("#role").NSValue(items.user_role);
                                $("#group").NSValue(items.user_group);
                                if (items.status == "Active") { $("#status").bootstrapSwitch("state", true); }
                                else { $("#status").bootstrapSwitch("state", false); }
                            }
                            $.busyLoadFull("hide");
                        }, 2000);
                    }
                });
            }

            $(".btnsave").click(function () {
                var status = true;
                $("form .required").each(function () {
                    if ($(this).id == "username" && $(this).NSValue().length < 6) {
                        status = false; $($(this)).NSFocus();
                        alertify.alert("กรุณากรอกบัญชีผู้ใช้งานตั้งแต่ 6 ตัวอักษรขึ้นไป");

                    } else if ($(this).id == "password" && $(this).NSValue().length < 6) {
                        status = false; $($(this)).NSFocus();
                        alertify.alert("กรุณากรอกรหัสผ่านตั้งแต่ 6 ตัวอักษรขึ้นไป");

                    } else if ($(this).id == "PID" && $(this).NSValue().length != 13) {
                        status = false; $($(this)).NSFocus();
                        alertify.alert("กรุณากรอกเลขบัตรประชาชน 13 หลัก");

                    } else if ($(this).id == "password" || $(this).id == "confirm_password") {
                        if ($("#password").NSValue() != $("#confirm_password").NSValue()) {
                            status = false; $("#confirm_password").NSFocus();
                            alertify.alert("รหัสผ่านไม่เหมือนกัน กรุณากรอกใหม่อีกครั้ง");
                        }
                    } else {
                        if ($.trim($(this).NSValue()) == "") {
                            status = false;
                            if ($(this).is("input")) {
                                $($(this)).NSFocus();
                                alertify.alert("กรุณากรอก " + $(this).attr("placeholder"));
                            } else {
                                alertify.alert("กรุณาเลือก " + $(this).data("placeholder"), function () {
                                    $($(this)).select2("open");
                                });
                            }
                        }
                    }
                    if (!status) { return false; }
                });

                if (status) {
                    $.ajax({
                        url: "../control/user.aspx",
                        data: {
                            action: "save_account",
                            account_id: $("#id").NSValue() || null,
                            user_name: $("#username").NSValue(),
                            password: $("#password").NSValue(),
                            name: $("#name").NSValue(),
                            lastname: $("#lastname").NSValue(),
                            PID: $("#PID").NSValue(),
                            email: $("#email").NSValue(),
                            telephone: $("#tel").NSValue() || null,
                            birthday: $("#birthday").NSValue(),
                            department: $("#department").NSValue(),
                            user_role: $("#role").NSValue(),
                            user_group: $("#group").NSValue(),
                        },
                        beforeSend: function () {
                            $.busyLoadFull("show", { spinner: "circles" });
                        },
                        type: "POST",
                        dataType: "json",
                        error: function () {
                            alertify.alert("error display data.");
                            $.busyLoadFull("hide");
                        },
                        success: function (data) {
                            if (!data.onError) {
                                data.getItems = jQuery.parseJSON(data.getItems);
                                if (data.getItems.status == "success") {
                                    alertify.alert(data.getItems.txtAlert, function () {
                                        window.location.href = $(".btnback").attr('href');
                                    });
                                } else {
                                    alertify.alert(data.getItems.txtAlert);
                                }
                            } else {
                                alertify.alert("ไม่สามารถเพิ่มข้อมูลบัญชีผู้ใช้งานได้");
                            }
                            $.busyLoadFull("hide");
                        }
                    });
                }
            });
        });
    </script>
</asp:Content>
