<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="add_em_device.aspx.vb" Inherits="EM.addem_device" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2.min.css">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2-bootstrap.css">
    <link rel="stylesheet" href="/resources_constant/css/bootstrap-switch/bootstrap-switch.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box">
            <div class="row text-header datacontent">
                <div class="col-md-6">
                    <h3 class="text-light-blue"><span id="subject">เพิ่มข้อมูลอุปกรณ์ EM</span></h3>
                </div>
                <div class="clearfix"></div>
            </div>
            <form action="#" id="em_device">
                <div class="datacontent">
                    <div class="row">
                        <div class="col-md-12 text-right hidden" id="btnstatus">
                            <h5><b>สถานะ<b class="text-red">*</b></b></h5>
                            <input type="checkbox" id="status" name="status" checked data-on-color="success" data-on-text="Active" data-off-color="danger" data-off-text="Inactive">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <h5><b>Serial No.<b class="text-red">*</b></b></h5>
                            <input type="text" class="form-control required" id="serial_no" placeholder="Serial No." maxlength="10" />
                        </div>
                        <div class="col-md-6">
                            <h5><b>เลข IMEI ประจำเครื่อง<b class="text-red">*</b></b></h5>
                            <input type="text" class="form-control required" id="imei" placeholder="เลข IMEI ประจำเครื่อง" maxlength="15" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <h5><b>Mac Address<b class="text-red">*</b></b></h5>
                            <input type="text" class="form-control required" id="mac_address" placeholder="Mac Address" maxlength="17" />
                        </div>
                        <div class="col-md-6">
                            <h5><b>หน่วยงาน<b class="text-red">*</b></b></h5>
                            <select class="form-control hidden" id="department" data-placeholder="หน่วยงาน">
                                <option selected></option>
                            </select>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="text-center divbutton">
                                <a href="/device/em_device" class="btn btnback">
                                    <i class="fa fa-angle-left" aria-hidden="true"></i>ยกเลิก
                                </a>
                                <a href="#" class="btn btnsave">
                                    <i class="fa fa-check" aria-hidden="true"></i>บันทึกข้อมูล
                                </a>
                                <input class="hidden" type="text" name="device_id" id="device_id" />
                            </div>
                        </div>
                    </div>
                    <div id="select-template" class="hidden">
                        <select>
                            <option></option>
                        </select>
                    </div>
                </div>
            </form>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JsContent" runat="server">
    <script src="/resources_constant/js/select2/select2.min.js"></script>
    <script src="/resources_constant/js/bootstrap-switch/bootstrap-switch.js"></script>
    <script src="/resources_constant/js/jquery.mask/dist/jquery.mask.js"></script>
    <script src="/resource/js/select_department.js"></script>
    <script src="/resource/js/custom.js"></script>
    <script>
        $(function () {
            set_navigation($(".btnback").attr("href"));
            select_department("#department");

            number("#serial_no");
            $("#imei").NSNumberbox();
            $("#serial_no").mask("AAAA-AAAAA", {
                'translation': { 0: { pattern: /[0-9]/g } }, onKeyPress: function (value, event) {
                    event.currentTarget.value = value.toUpperCase();
                }
            });

            $("#mac_address").mask("AA:AA:AA:AA:AA:AA", {
                'translation': { 0: { pattern: /[A-Z0-9]/g } }, onKeyPress: function (value, event) {
                    event.currentTarget.value = value.toUpperCase();
                }
            });

            var param = getparameters();
            if (param.data != null) { set_data(); }

            function set_data() {
                $("#btnstatus").removeClass("hidden");
                $("#status").bootstrapSwitch("size", "small");
                $("#status").bootstrapSwitch("state", true);
                $("#subject").html("แก้ไขข้อมูลอุปกรณ์ EM");

                $.ajax({
                    url: "../control/device.aspx",
                    data: {
                        action: "device",
                        device_id: param.data,
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
                                $("#device_id").NSValue(param.data);
                                $("#serial_no").NSValue(items.device_SN).NSDisable();
                                $("#imei").NSValue(items.device_IMEI).NSDisable();
                                $("#mac_address").NSValue(items.device_Mac_address).NSDisable();
                                $("#department").NSValue(items.device_department);
                                if (items.device_status == "Active") { $("#status").bootstrapSwitch("state", true); }
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
                    if ($(this).id == "serial_no" && $(this).NSValue().length != 10) {
                        status = false; $($(this)).NSFocus();
                        alertify.alert("กรุณากรอก Serial No ให้ครบถ้วน");
                    } else if ($(this).id == "imei" && $(this).NSValue().length != 15) {
                        status = false; $($(this)).NSFocus();
                        alertify.alert("กรุณากรอก IMEI ให้ครบถ้วน");
                    } else if ($(this).id == "mac_address" && $(this).NSValue().length != 17) {
                        status = false; $($(this)).NSFocus();
                        alertify.alert("กรุณากรอก Mac Address ให้ครบถ้วน");
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
                        url: "../control/device.aspx",
                        data: {
                            action: "save_device",
                            device_id: $("#device_id").NSValue() || null,
                            serial_no: $("#serial_no").NSValue(),
                            imei: $("#imei").NSValue(),
                            mac_address: $("#mac_address").NSValue(),
                            department_id: $("#department").NSValue(),
                            status: $("#status").is(":checked") == true ? "Active" : "Inactive"
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
                                alertify.alert("ไม่สามารถเพิ่มข้อมูลอุปกรณ์ EM ได้");
                            }
                            $.busyLoadFull("hide");
                        }
                    });
                }
            });
        });
    </script>
</asp:Content>
