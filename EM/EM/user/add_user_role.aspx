<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="add_user_role.aspx.vb" Inherits="EM.add_user_role" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/resources_constant/css/bootstrap-switch/bootstrap-switch.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box">
            <div class="row text-header datacontent">
                <div class="col-md-6">
                    <h3 class="text-light-blue"><span id="subject">เพิ่มสิทธิผู้ใช้งาน</span></h3>
                </div>
                <div class="clearfix"></div>
            </div>
            <form action="#" id="user_role">
                <div class="datacontent">
                    <div class="row">
                        <div class="col-md-12 text-right hidden" id="btnstatus">
                            <h5><b>สถานะ<b class="text-red">*</b></b></h5>
                            <input type="checkbox" id="status" name="status" checked data-on-color="success" data-on-text="ใช้งาน" data-off-color="danger" data-off-text="ไม่ใช้งาน">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <h5><b>สิทธิผู้ใช้งาน</b><b class="text-red">*</b></h5>
                            <input type="text" class="form-control" id="role_name" placeholder="สิทธิผู้ใช้งาน">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="text-center divbutton">
                                <a href="/user/user_role" class="btn btnback">
                                    <i class="fa fa-angle-left" aria-hidden="true"></i>ยกเลิก
                                </a>
                                <a href="#" class="btn btnsave">
                                    <i class="fa fa-check" aria-hidden="true"></i>บันทึกข้อมูล
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JsContent" runat="server">
    <script src="/resources_constant/js/select2/select2.min.js"></script>
    <script src="/resources_constant/js/bootstrap-switch/bootstrap-switch.js"></script>
    <script src="/resource/js/custom.js"></script>
    <script type="text/javascript">
        $(function () {
            set_navigation($(".btnback").attr("href"));
            txt("#role_name");

            var param = getparameters();
            if (param.data != null) { set_data(); }

            function set_data() {
                $("#btnstatus").removeClass("hidden");
                $("#subject").html("แก้ไขข้อมูลสิทธิผู้ใช้งาน");
                $("#status").bootstrapSwitch("size", "small");
                $("#status").bootstrapSwitch("state", true);
                $.ajax({
                    url: "../control/user.aspx",
                    data: {
                        action: "role",
                        role_id: param.data,
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
                            items = jQuery.parseJSON(data.getItems);
                            $("#role_id").NSValue(param.data);
                            $("#role_name").NSValue(items.role_name);
                            if (items.role_status == "Active") { $("#status").bootstrapSwitch("state", true); }
                            else { $("#status").bootstrapSwitch("state", false); }
                            
                        }
                        $.busyLoadFull("hide");
                    }
                });
            }

            $(".btnsave").click(function () {
                if ($.trim($("#role_name").NSValue()) == "") {
                    $("#role_name").focus();
                    alertify.alert("กรุณากรอกสิทธิผู้ใช้งาน");
                    return false;
                }

                $.ajax({
                    url: "../control/user.aspx",
                    data: {
                        action: "save_role",
                        role_id: param.data || null,
                        role_name: $("#role_name").NSValue(),
                        role_status: $("#status").is(":checked") == true ? "Active" : "Inactive"
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
                                alertify.alert(data.getItems.txtAlert, function (closeEvent) {
                                    window.location.href = $(".btnback").attr('href');
                                });
                            } else {
                                alertify.alert(data.getItems.txtAlert);
                            }
                        } else {
                            alertify.alert("ไม่สามารถเพิ่มข้อมูลสิทธิผู้ใช้งานได้");
                        }
                        $.busyLoadFull("hide");
                    }
                });
            });
        });
    </script>
</asp:Content>

