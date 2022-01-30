<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="add_province.aspx.vb" Inherits="EM.province" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/resources_constant/css/bootstrap-switch/bootstrap-switch.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box">
            <div class="row text-header datacontent">
                <div class="col-md-6">
                    <h3 class="text-light-blue"><span id="subject">เพิ่มข้อมูลจังหวัด</span></h3>
                </div>
                <div class="clearfix"></div>
            </div>
            <form action="#" id="province">
                <div class="datacontent">
                    <div class="row">
                        <div class="col-md-12 text-right hidden" id="btnstatus">
                            <h5><b>สถานะ<b class="text-red">*</b></b></h5>
                            <input type="checkbox" id="data_status" name="status" checked data-on-color="success" data-on-text="ใช้งาน" data-off-color="danger" data-off-text="ไม่ใช้งาน">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <h5><b>รหัสจังหวัด<b class="text-red">*</b></b></h5>
                            <input type="text" class="form-control required" id="data_code" placeholder="รหัสจังหวัด" />
                        </div>
                        <div class="col-md-6">
                            <h5><b>จังหวัด<b class="text-red">*</b></b></h5>
                            <input type="text" class="form-control required" id="data_name" placeholder="จังหวัด" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="text-center divbutton">
                                <a href="/location/province" class="btn btnback">
                                    <i class="fa fa-angle-left" aria-hidden="true"></i>ยกเลิก
                                </a>
                                <a href="#" class="btn btnsave">
                                    <i class="fa fa-check" aria-hidden="true"></i>บันทึกข้อมูล
                                </a>
                                <input type="text" class="hidden" id="data_id" />
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
            $("#data_name").NSThaibox();
            $("#data_code").NSNumberboxInteger();

            var param = getparameters();
            if (param.data != null) { set_data(); }

            function set_data() {
                $("#subject").html("แก้ไขข้อมูลจังหวัด");
                $("#btnstatus").removeClass("hidden");
                $("#data_status").bootstrapSwitch("size", "small");
                $("#data_status").bootstrapSwitch("state", true);
                $.ajax({
                    url: "../control/location.aspx",
                    data: {
                        action: "province",
                        pid: param.data,
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
                            $("#data_id").NSValue(param.data);
                            $("#data_code").NSValue(data.getItems.data_code).NSDisable();
                            $("#data_name").NSValue(data.getItems.data_name);
                            if (data.getItems.data_status == "Active") { $("#data_status").bootstrapSwitch("state", true); }
                            else { $("#data_status").bootstrapSwitch("state", false); }
                        }
                        $.busyLoadFull("hide");
                    }
                });
            }

            $(".btnsave").click(function () {
                var result = CheckData();
                if (result[0]) {
                    $.ajax({
                        url: "../control/location.aspx",
                        data: {
                            action: "save_province",
                            da: JSON.stringify(result[1])
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
                                alertify.alert("ไม่สามารถเพิ่มข้อมูลจังหวัดได้");
                            }
                            $.busyLoadFull("hide");
                        }
                    });
                }
            });
        });
    </script>
</asp:Content>
