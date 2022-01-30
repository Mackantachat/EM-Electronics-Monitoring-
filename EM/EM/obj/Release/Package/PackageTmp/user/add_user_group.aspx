<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="add_user_group.aspx.vb" Inherits="EM.add_user_group" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/resources_constant/AdminLTE2/bower_components/datatables.net-bs/css/dataTables.bootstrap.css">
    <link rel="stylesheet" href="/resources_constant/css/bootstrap-switch/bootstrap-switch.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box">
            <div class="row text-header datacontent">
                <div class="col-md-6">
                    <h3 class="text-light-blue"><span id="subject">เพิ่มกลุ่มผู้ใช้งาน</span></h3>
                </div>
                <div class="clearfix"></div>
            </div>
            <form action="#" id="user_group">
                <div class="datacontent">
                    <div class="row">
                        <div class="col-md-12 text-right hidden" id="btnstatus">
                            <h5><b>สถานะ<b class="text-red">*</b></b></h5>
                            <input id="status" type="checkbox" checked data-on-color="success" data-on-text="ใช้งาน" data-off-color="danger" data-off-text="ไม่ใช้งาน">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <h5><b>กลุ่มผู้ใช้งาน</b><b class="text-red">*</b></h5>
                            <input id="usergroup" class="form-control" type="text" placeholder="กลุ่มผู้ใช้งาน">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="text-center divbutton">
                                <a href="/user/user_group" class="btn btnback">
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
    <script src="/resources_constant/js/bootstrap-switch/bootstrap-switch.js"></script>
    <script src="/resource/js/custom.js"></script>
    <script>
        $(function () {
            set_navigation($(".btnback").attr("href"));

            var param = getparameters();
            if (param.data != null) { set_data(); }

            function set_data() {
                $("#btnstatus").removeClass("hidden");
                $("#subject").html("แก้ไขกลุ่มผู้ใช้งาน");
                $("#status").bootstrapSwitch("size", "small");
                $("#status").bootstrapSwitch("state", true);
                $.ajax({
                    url: "../control/user.aspx",
                    data: {
                        action: "group",
                        user_group_id: param.data,
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
                            $("#usergroup").NSValue(data.getItems.group_name);
                            if (data.getItems.group_status == "Active") { $("#status").bootstrapSwitch("state", true); }
                            else { $("#status").bootstrapSwitch("state", false); }
                        }
                        $.busyLoadFull("hide");
                    }
                });
            }

            $(".btnsave").click(function () {
                if ($.trim($("#usergroup").NSValue()) == "") {
                    $("#usergroup").NSFocus(); alertify.alert("กรุณากรอกชื่อกลุ่มผู้ใช้งาน");
                    return false;
                }

                $.ajax({
                    url: "../control/user.aspx",
                    data: {
                        action: "save_group",
                        group_id: param.data || null,
                        group_name: $("#usergroup").NSValue(),
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
                            alertify.alert("ไม่สามารถเพิ่มข้อมูลกลุ่มผู้ใช้งานได้");
                        }
                        $.busyLoadFull("hide");
                    }
                });
            });
        });
    </script>
</asp:Content>
