<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="user_account_change_password.aspx.vb" Inherits="EM.user_account_change_password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2.min.css">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2-bootstrap.css">
    <link rel="stylesheet" href="/resources_constant/css/bootstrap-switch/bootstrap-switch.css">
    <link rel="stylesheet" href="/resources_constant/AdminLTE2/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box ">
            <div class="row text-header datacontent">
                <div class="col-md-6">
                    <h3 class="text-light-blue"><span>แก้ไขรหัสผ่าน</span></h3>
                </div>
                <div class="clearfix"></div>
            </div>
            <form>
                <div class="datacontent">
                    <div class="row">
                        <div class="col-md-6">
                            <h5><b>รหัสผ่านใหม่</b></h5>
                            <input type="password" class="form-control" id="password" placeholder="รหัสผ่านใหม่" >
                            <h5><b>ยืนยันรหัสผ่านใหม่</b></h5>
                            <input type="password" class="form-control" id="confirm_password" placeholder="ยืนยันรหัสผ่านใหม่" >
                        </div>
                        <div class="col-md-6 info">
                            <small class="text-aqua">รหัสผ่านควรจะมีความยาวอย่างน้อย 6 - 20 ตัวอักษร โดยใช้อักษรหรือตัวเลขในการกำหนด.<br>
                                ไม่ควรประกอบด้วยชื่อหรือคำที่ใช้โดยทั่วไป หรือคำที่ใกล้เคียงชื่อหรือคำที่ใช้โดยทั่วไป.                                            
                            </small>
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
                                <input class="hidden" type="text" name="data_id" id="data_id" />
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JsContent" runat="server">
    <script src="/resources_constant/AdminLTE2/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.js"></script>
    <script src="/resources_constant/AdminLTE2/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker-custom.js"></script>
    <script src="/resources_constant/AdminLTE2/bower_components/bootstrap-datepicker/js/locales/bootstrap-datepicker.th.js"></script>
    <script src="/resources_constant/AdminLTE2/bower_components/moment/min/moment-with-locales.min.js"></script>
    <script src="/resource/js/custom.js"></script>
    <script>
        $(function () {
            set_navigation($(".btnback").attr("href"));
            usepass("#confirm_password, #password");

            $(".btnsave").click(function () {
                let param = getparameters();
                let password = $("#password").NSValue();
                let confirm = $("#confirm_password").NSValue();
                if (password == null || password == undefined || password == "") {
                    $("#password").focus();
                    alertify.alert("กรุณากรอกรหัสผ่านที่ต้องการเปลี่ยน");
                    return false;

                } else if (password.length < 6) {
                    $("#password").focus();
                    alertify.alert("กรุณากรอกรหัสผ่านตั้งแต่ 6 ตัวอักษรขึ้นไป");
                    return false;

                } else if (password != confirm) {
                    $("#confirm_password").focus();
                    alertify.alert("รหัสผ่านไม่เหมือนกัน กรุณาตรวจสอบอีกครั้ง");
                    return false;
                }

                $.ajax({
                    url: "../control/user.aspx",
                    data: {
                        action: "changepassword",
                        account_id: param.data || null,
                        password: password,
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
                                    history.go(-1)
                                });
                            } else {
                                alertify.alert(data.getItems.txtAlert);
                            }
                        } else {
                            alertify.alert("ไม่สามารถเปลี่ยนรหัสผ่านได้");
                        }
                        $.busyLoadFull("hide");
                    }
                });

            });
        });
    </script>
</asp:Content>
