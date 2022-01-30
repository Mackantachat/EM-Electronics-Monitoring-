<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="add_prohibited_group.aspx.vb" Inherits="EM.add_prohibited_group" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/resources_constant/css/bootstrap-switch/bootstrap-switch.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box">
            <div class="row text-header datacontent">
                <div class="col-md-6">
                    <h3 class="text-light-blue"><span id="subject">เพิ่มกลุ่มพื้นที่หวงห้าม</span></h3>
                </div>
                <div class="clearfix"></div>
            </div>
            <form>
                <div class="datacontent">
                    <div class="row">
                        <div class="col-md-12 text-right hidden" id="btnstatus">
                            <h5><b>สถานะ<b class="text-red">*</b></b></h5>
                            <input id="status" type="checkbox" checked data-on-color="success" data-on-text="ใช้งาน" data-off-color="danger" data-off-text="ไม่ใช้งาน">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <h5><b>กลุ่มพื้นที่หวงห้าม</b><b class="text-red">*</b> </h5>
                            <input class="form-control" type="text" placeholder="กลุ่มพื้นที่ควบคุม" id="phohibited_group">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="text-center divbutton">
                                <a href="/area/prohibited_group" class="btn btnback">
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
    <script src="/resources_constant/js/select2/select2.min.js"></script>
    <script src="/resources_constant/js/bootstrap-switch/bootstrap-switch.js"></script>
    <script src="/resource/js/custom.js"></script>
    <script type="text/javascript">    
        $(function () {
            txt("#phohibited_group");
            var param = getparameters();
            if (param.data != null) { set_data(); }

            function set_data() {
                $("#btnstatus").removeClass("hidden");
                $("#status").bootstrapSwitch("size", "small");
                $("#status").bootstrapSwitch("state", true);
                $("#subject").html("แก้ไขข้อมูลกลุ่มพื้นที่หวงห้าม");
                $.ajax({
                    url: "../control/phohibited.aspx",
                    data: {
                        action: "group",
                        group_id: param.data,
                    },
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        if (!data.onError) {
                            data.getItems = jQuery.parseJSON(data.getItems);
                            $("#phohibited_group").NSValue(data.getItems.group_name);
                            if (data.getItems.group_status == "Active") { $("#status").bootstrapSwitch("state", true); }
                            else { $("#status").bootstrapSwitch("state", false); }
                        }
                        $.busyLoadFull("hide");
                    }
                });
            }

            $(".btnsave").click(function () {
                if ($.trim($("#phohibited_group").NSValue()) == "") {
                    $("#phohibited_group").focus(); alertify.alert("กรุณากรอกกลุ่มพื้นที่หวงห้าม");
                    return false;
                }

                $.ajax({
                    url: "../control/phohibited.aspx",
                    data: {
                        action: "save_group",
                        area_group_id: param.data || null,
                        area_group_name: $("#phohibited_group").NSValue(),
                        area_group_status: $("#status").is(":checked") == true ? "Active" : "Inactive"
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
                            alertify.alert("ไม่สามารถเพิ่มข้อมูลได้");
                        }
                        $.busyLoadFull("hide");
                    }
                });
            });
        });
    </script>
</asp:Content>
