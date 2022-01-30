<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="probationer.aspx.vb" Inherits="EM.DTBinsert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/resources_constant/AdminLTE2/bower_components/datatables.net-bs/css/dataTables.bootstrap.css">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2.min.css">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2-bootstrap.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box">
            <div class="row text-header">
                <div class="col-md-7">
                    <h3 class="text-blue">ผู้ถูกคุมความประพฤติ</h3>
                </div>
                <div class="col-md-5">
                    <div class="col-md-6">
                        <button type="button" class="btn btn-block btn-warning btn-xs btntop ml-15" id="changestatus">
                            <i class="fas fa-sync-alt"></i>เปลี่ยนสถานะ
                        </button>
                    </div>
                    <div class="col-md-6">
                        <a href="/probationer/add_probationer" class="btn btn-block btn-primary btntop">
                            <i class="fas fa-plus"></i>เพิ่มผู้ถูกคุมความประพฤติ
                        </a>
                    </div>
                </div>
            </div>
            <div class="row text-header">
                <div class="col-md-7">
                    <label class="label-length">
                        แสดง
                        <select class="form-control select-length" id="length_change">
                            <option value="10">10</option>
                            <option value="25">25</option>
                            <option value="50">50</option>
                        </select>
                        แถว</label>
                </div>
                <div class="col-md-5">
                    <div class="col-md-4 btn-search">
                        <select class="form-control" id="search_status">
                            <option value="" selected>ทั้งหมด</option>
                            <option value="Active">ใช้งาน</option>
                            <option value="Inactive">ไม่ใช้งาน</option>
                        </select>
                    </div>
                    <div class="col-md-8 text-right text-secontrolarch">
                        <input type="text" class="form-control" id="search_data" placeholder="ค้นหา">
                        <button type="button" class="btn searchbt" id="search_bt">
                            <span class="fa fa-search text-black"></span>
                        </button>
                    </div>
                </div>
            </div>
            <div class="row text-content">
                <div class="col-md-12 box-body table-responsive">
                    <table id="info_probationer" class="table table-striped">
                        <thead>
                            <tr>
                                <th class="aligncenter">
                                    <div class="checkbox">
                                        <label class="custom">
                                            <input type="checkbox" id="main-check-all">
                                            <span class="checkboxorder"></span>
                                        </label>
                                    </div>
                                </th>
                                <th>จัดการ</th>
                                <th>ลำดับ</th>
                                <th>ชื่อ-สกุล</th>
                                <th>เลขบัตรประจำตัว</th>
                                <th>เลขทะเบียนคดี</th>
                                <th>หน่วยงาน</th>
                                <th>เบอร์ติดต่อ</th>
                                <th>อุปกรณ์ EM </th>
                                <th>วันที่ติดตั้งอุปกรณ์</th>
                                <th>ตั้งค่า</th>
                                <th>สถานะ</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>

            <div class="col-md-8 text-right text-search">
                        <input type="text" class="form-control" id="text_line" placeholder="กรอกข้อความไลน์กลุ่ม">
                        <button type="button" class="btn searchbt" id="sendline_btn">
                            <span class="fa fa-paper-plane"></span>
                        </button>
                    </div>

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JsContent" runat="server">
    <script src="/resources_constant/AdminLTE2/bower_components/datatables.net/js/jquery.dataTables.js"></script>
    <script src="/resources_constant/AdminLTE2/bower_components/datatables.net-bs/js/dataTables.bootstrap.js"></script>
    <script src="/resources_constant/js/select2/select2.min.js"></script>
    <script src="/resource/js/datatable.js"></script>
    <script>
        $(function () {
            $("#info_probationer").DataTable({
                aaSorting: [],
                ajax: {
                    url: "../control/probationer.aspx",
                    data: {
                        action: "list_probationer",
                    },
                    beforeSend: function () {
                        $.busyLoadFull("show", { spinner: "circles" });
                    },
                    type: "POST",
                    dataType: "json",
                    dataSrc: function (data) {
                        if (!data.onError) {
                            data.getItems = jQuery.parseJSON(data.getItems);
                            return data.getItems;
                        }
                    }, complete: function () {
                        $.busyLoadFull("hide");
                    }
                },
                "columns": [
                    {
                        data: null,
                        render: function (data) {
                            let result = data.pid + '|' + data.status
                            return '<div class="checkbox"><label class="custom"><input type="checkbox" value="' + result + '"><span class="checkboxorder"></span></label></div>'
                        }
                    },
                    {
                        data: "status",
                        render: function (data) {
                            var button = '<a class="edit"><i Class="fas fa-edit"></i></a><a class="update">';
                            if (data == "Active") {
                                button += '<i Class="fas fa-sync-alt text-red"></i></a>';
                            } else {
                                button += '<i Class="fas fa-sync-alt text-green"></i></a>';
                            }
                            return button;
                        }
                    },
                    { data: null },
                    {
                        width: 200,
                        data: "name"
                    },
                    {
                        width: 150,
                        data: "personal_ID"
                    },
                    {
                        width: 150,
                        data: "caseID"
                    },
                    {
                        width: 200,
                        data: "department"
                    },
                    {
                        width: 100,
                        data: "mobile"
                    },
                    {
                        width: 150,
                        data: "device",
                        render: function (data) {
                            return data + '<a href="#" class="btn btnsend"><i class="fas fa-sms"></i>ส่งข้อความ</a>'
                        }
                    },
                    {
                        width: 150,
                        data: "IMEI_start_date",
                        render: function (data, type) {
                            let d = new Date(data);
                            let month = d.getMonth();
                            let text_date = "";
                            if (type === "display" || type === "filter") {
                                text_date = d.getDate() + " " + monthNamesFull[month] + " " + (d.getFullYear() + 543);
                            }
                            if (type === "sort") {
                                return d;
                            }
                            return text_date;
                        }
                    },
                    {
                        width: 150,
                        orderable: false,
                        data: "pid",
                        render: function (data) {
                            return '<a href="#" class="btn btnsetting"><i class="fas fa-pencil-alt"></i>กำหนดเงื่อนไข</a>'
                        }
                    },
                    {
                        width: 70,
                        data: "status"
                    }
                ],
                createdRow: function (row, data, indice) {
                    $(row).find("td:eq(1)").attr('data-id', data.pid);
                }
            });
            control_datatable("#info_probationer", "11");
            $("#info_probationer").on("click", "a.update", function () {
                update_status($(this).closest("tr").find('input[type="checkbox"]').NSValue(), "N");
            });

            $("#changestatus").click(function () {
                if ($('input[type="checkbox"]:not(#main-check-all):checked').length > 0) {
                    let result = "";
                    $.each($('input[type="checkbox"]:not(#main-check-all):checked'), function () {
                        result += $(this).val() + ",";
                    });
                    update_status(result, "Y");
                } else {
                    alertify.alert("กรุณากดเลือกรายการที่ต้องการเปลี่ยนสถานะ");
                }
            });

            function update_status(data, list) {
                alertify.confirm("กรุณายืนยันการเปลี่ยนสถานะผู้ถูกคุมความประพฤติ", "คุณต้องการที่จะเปลี่ยนสถานะผู้ถูกคุมความประพฤตินี้หรือไม่? กดตกลงเพื่อเปลี่ยนสถานะ").set("onok", function (closeEvent) {
                    $.ajax({
                        url: "../control/probationer.aspx",
                        data: {
                            action: "status_probationer",
                            data: data,
                            list: list,
                        },
                        beforeSend: function () {
                            $.busyLoadFull("show", {
                                spinner: "circles"
                            });
                        },
                        type: "POST",
                        dataType: "json",
                        error: function () {
                            $.busyLoadFull("hide");
                        },
                        success: function (data) {
                            if (!data.onError) {
                                data.getItems = jQuery.parseJSON(data.getItems);
                                if (data.getItems.status.length > 0) {
                                    alertify.alert(data.getItems.txtAlert, function () {
                                        location.reload();
                                    });
                                }
                            } else {
                                alertify.alert("ไม่สามารถเปลี่ยนสถานะผู้ถูกคุมความประพฤตินี้ได้");
                            }
                            $.busyLoadFull("hide");
                        }
                    });
                });
            }

            $("#text_line").on("keypress", function (e) {
                if (e.keyCode == 13) {
                    $.ajax({
                        url: "/control/LineNotifications.aspx",
                        data: {
                            action: "LineNoti",
                            line_text: $("#text_line").NSValue(),

                        },
                        type: "POST",
                        dataType: "json",
                        success: function (data) {
                            if (!data.onError) {
                                data.getItems = jQuery.parseJSON(data.getItems);

                            }
                        }
                    });
                    $("#text_line").NSValue("");
                }
            }); 

            $("#sendline_btn").click(function  () {
                $.ajax({
                    url: "/control/LineNotifications.aspx",
                    data: {
                        action: "LineNoti",
                        line_text: $("#text_line").NSValue(),

                    },
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        if (!data.onError) {
                            data.getItems = jQuery.parseJSON(data.getItems);

                        }
                    }
                });
                $("#text_line").NSValue("");
            });
        });
    </script>
</asp:Content>
