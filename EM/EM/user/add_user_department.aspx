<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="add_user_department.aspx.vb" Inherits="EM.add_user_department" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/resources_constant/css/bootstrap-switch/bootstrap-switch.css">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2.min.css">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2-bootstrap.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input id="add" class="hidden">
    <div class="col-md-12">
        <div class="box">
            <div class="row text-header datacontent">
                <div class="col-md-6">
                    <h3 class="text-light-blue"><span id="subject">เพิ่มหน่วยงาน</span></h3>
                </div>
                <div class="clearfix"></div>
            </div>
            <form action="#" id="department">
                <div class="datacontent">
                    <div class="row">
                        <div class="col-md-12 text-right hidden" id="btnstatus">
                            <h5><b>สถานะ<b class="text-red">*</b></b></h5>
                            <input type="checkbox" id="department_status" name="status" checked data-on-color="success" data-on-text="ใช้งาน" data-off-color="danger" data-off-text="ไม่ใช้งาน">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <h5><b>ชื่อหน่วยงาน</b><b class="text-red">*</b></h5>
                            <input type="text" class="form-control required" id="department_name" placeholder="ชื่อหน่วยงาน">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <h5><b>รหัสหน่วยงาน</b><b class="text-red">*</b></h5>
                            <input type="text" class="form-control required numberbox" id="department_code" placeholder="รหัสหน่วยงาน" maxlength="10">
                        </div>
                        <div class="col-md-4">
                            <h5><b>เบอร์โทรศัพท์ (มือถือ)</b></h5>
                            <input type="text" class="form-control" id="department_mobile" placeholder="เบอร์โทรศัพท์ (มือถือ)">
                        </div>
                        <div class="col-md-4">
                            <h5><b>เบอร์โทรศัพท์</b></h5>
                            <input type="text" class="form-control" id="department_telephone" placeholder="เช่น xx-xxx-xxxx # xx,xx-xxx-xxxx ต่อ xx">
                        </div>
                    </div>
                    <div class="row mt-12">
                        <div class="col-md-12">
                            <h5><i class="fa fa-home"></i><b>ข้อมูลที่อยู่</b></h5>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <h5><b>เลขที่</b><b class="text-red">*</b></h5>
                            <input type="text" class="form-control required" id="department_number" placeholder="เลขที่">
                        </div>
                        <div class="col-md-3">
                            <h5><b>หมู่</b></h5>
                            <input type="text" class="form-control numberbox" id="department_moo" placeholder="หมู่">
                        </div>
                        <div class="col-md-3">
                            <h5><b>ถนน</b></h5>
                            <input type="text" class="form-control" id="department_road" placeholder="ถนน">
                        </div>
                        <div class="col-md-3">
                            <h5><b>ซอย</b></h5>
                            <input type="text" class="form-control" id="department_lane" placeholder="ซอย">
                        </div>
                    </div>
                    <div class="row address">
                        <div class="col-md-4">
                            <h5><b>จังหวัด</b><b class="text-red">*</b></h5>
                            <select class="form-control required" id="department_province" data-placeholder="จังหวัด">
                                <option></option>
                            </select>
                        </div>
                        <div class="col-md-4">
                            <h5><b>อำเภอ/เขต</b><b class="text-red">*</b></h5>
                            <select class="form-control required" id="department_district" data-placeholder="อำเภอ/เขต"></select>
                        </div>
                        <div class="col-md-4">
                            <h5><b>ตำบล/แขวง</b><b class="text-red">*</b></h5>
                            <select class="form-control required" id="department_sub_district" data-placeholder="ตำบล/แขวง"></select>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="text-center divbutton">
                                <a href="/user/user_department" class="btn btnback">
                                    <i class="fa fa-angle-left" aria-hidden="true"></i>ยกเลิก
                                </a>
                                <a href="#" class="btn btnsave">
                                    <i class="fa fa-check" aria-hidden="true"></i>บันทึกข้อมูล
                                </a>
                                <input type="text" class="hidden" id="department_id" />
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
    <script src="/resources_constant/js/bootstrap-switch/bootstrap-switch.js"></script>
    <script src="/resources_constant/js/select2/select2.min.js"></script>
    <script src="/resource/js/select_address.js"></script>
    <script src="/resource/js/custom.js"></script>
    <script>
        $(function () {
            set_navigation($(".btnback").attr("href"));
            $("#department_province, #department_district, #department_sub_district").select2({
                width: "100%",
                theme: "bootstrap"
            });

            var param = getparameters();
            if (param.data != null) { set_data(); }

            Dropdown(".address");
            text("#depart_rode, #depart_soi");
            txt("#department_name");
            phone("#department_mobile");
            telephone("#department_telephone");
            homeno("#department_number");
            $(".numberbox").NSNumberbox();

            function set_data() {
                $("#btnstatus").removeClass("hidden");
                $("#subject").html("แก้ไขข้อมูลหน่วยงาน");
                $("#status").bootstrapSwitch("size", "small");
                $("#status").bootstrapSwitch("state", true);
                $.ajax({
                    url: "../control/user.aspx",
                    data: {
                        action: "department",
                        department_id: param.data,
                    },
                    beforeSend: function () {
                        $.busyLoadFull("show", { spinner: "circles" });
                    },
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        setTimeout(function () {
                            if (!data.onError) {
                                data.getItems = jQuery.parseJSON(data.getItems);
                                $("#department_id").NSValue(param.data);
                                $("#department_code").NSValue(data.getItems.department_code).NSDisable();
                                $("#department_name").NSValue(data.getItems.department_name);
                                $("#department_mobile").NSValue(data.getItems.department_mobile);
                                $("#department_telephone").NSValue(data.getItems.department_telephone);
                                $("#department_number").NSValue(data.getItems.department_number);
                                $("#department_moo").NSValue(data.getItems.department_moo);
                                $("#department_road").NSValue(data.getItems.department_road);
                                $("#department_lane").NSValue(data.getItems.department_lane);
                                $("#department_province").NSValue(data.getItems.department_province);
                                RenderDistrict(data.getItems.department_province, "#department_district");
                                $("#department_district").NSValue(data.getItems.department_district).trigger("change");
                                RenderSubDistrict(data.getItems.department_district, "#department_district");
                                $("#department_district").NSValue(data.getItems.department_sub_district).trigger("change");
                                if (data.getItems.department_status == "Active") { $("#status").bootstrapSwitch("state", true); }
                                else { $("#status").bootstrapSwitch("state", false); }
                            }
                            $.busyLoadFull("hide");
                        }, 2000);
                    }
                });
            }

            $(".btnsave").click(function () {
                var result = CheckData();
                if (result[0]) {
                    $.ajax({
                        url: "../control/user.aspx",
                        data: {
                            action: "save_department",
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
                }
            });
        });
    </script>
</asp:Content>
