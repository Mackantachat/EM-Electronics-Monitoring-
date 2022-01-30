﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="user_group.aspx.vb" Inherits="EM.user_group" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/resources_constant/AdminLTE2/bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2.min.css">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2-bootstrap.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box">
            <div class="row text-header">
                <div class="col-md-7">
                    <h3 class="text-blue">กลุ่มผู้ใช้งาน</h3>
                </div>
                <div class="col-md-5">
                    <div class="col-md-6">
                        <button type="button" class="btn btn-block btn-warning btn-xs btntop ml-15" id="changestatus">
                            <i class="fas fa-sync-alt"></i>เปลี่ยนสถานะ
                        </button>
                    </div>
                    <div class="col-md-6">
                        <a href="/user/add_user_group" class="btn btn-block btn-primary btntop">
                            <i class="fas fa-plus"></i>เพิ่มกลุ่มผู้ใช้งาน
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
                    <div class="col-md-8 text-right text-search">
                        <input type="text" class="form-control" id="search_data" placeholder="ค้นหา">
                        <button type="button" class="btn searchbt" id="search_bt">
                            <span class="fa fa-search text-black"></span>
                        </button>
                    </div>
                </div>
            </div>
            <div class="row text-content">
                <div class="col-md-12 box-body table-responsive">
                    <table id="info_usergroup" class="table table-striped">
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
                                <th style="width: 4%;">จัดการ</th>
                                <th style="width: 5%;">ลำดับ</th>
                                <th class="text-center" style="width: 50%;">กลุ่มผู้ใช้งาน</th>
                                <th class="text-center" style="width: 20%;">วันที่</th>
                                <th class="text-center" style="width: 20%;">สถานะ</th>
                            </tr>
                        </thead>
                    </table>
                </div>
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
            $('#info_usergroup').DataTable({
                aaSorting: [],
                ajax: {
                    url: "../control/user.aspx",
                    data: {
                        action: "list_group",
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
                            let result = data.group_id + '|' + data.group_status
                            return '<div class="checkbox"><Label class="custom"><input type = "checkbox" value="' + result + '"><span class="checkboxorder"></span></label></div>';
                        }
                    },
                    {
                        data: "group_status",
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
                    { data: "group_name" },
                    {
                        data: "group_date",
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
                    { data: "group_status" }
                ],
                createdRow: function (row, data, indice) {
                    $(row).find("td:eq(1)").attr("data-id", data.group_id);
                }
            });

            control_datatable("#info_usergroup", "6");

            $("#info_usergroup").on("click", "a.change", function () {
                window.open("user_account_change_password?data=" + $(this).closest("tr").find("td").eq(1).attr("data-id"), "_self");
            });

            $("#info_user_account").on("click", "a.update", function () {
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
                alertify.confirm("กรุณายืนยันการเปลี่ยนสถานะกลุ่มผู้ใช้งาน", "คุณต้องการที่จะเปลี่ยนสถานะกลุ่มผู้ใช้งานนี้หรือไม่? กด ok เพื่อลบ").set("onok", function (closeEvent) {
                    $.ajax({
                        url: "../control/user.aspx",
                        data: {
                            action: "status_group",
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
                        error: function (xhr, s, err) {
                            console.log(s, err);
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
                                alertify.alert("ไม่สามารถเปลี่ยนสถานะกลุ่มผู้ใช้งานได้");
                            }
                            $.busyLoadFull("hide");
                        }
                    });
                });
            }
        });
    </script>
</asp:Content>
