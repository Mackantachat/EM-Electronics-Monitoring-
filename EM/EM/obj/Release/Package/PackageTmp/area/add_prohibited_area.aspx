<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="add_prohibited_area.aspx.vb" Inherits="EM.add_prohibited_area" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/resources_constant/css/jquery-timepicker/jquery.timepicker.css">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2.min.css">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2-bootstrap.css">
    <link rel="stylesheet" href="/resources_constant/css/bootstrap-switch/bootstrap-switch.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box">
            <div class="row text-header datacontent">
                <div class="col-md-6">
                    <h3 class="text-light-blue"><span id="subject">เพิ่มรายการพื้นที่หวงห้าม</span></h3>
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="datacontent">
                <div class="row">
                    <div class="col-md-12 text-right hidden" id="btnstatus">
                        <h5><b>สถานะ<b class="text-red">*</b></b></h5>
                        <input id="status" type="checkbox" checked data-on-color="success" data-on-text="ใช้งาน" data-off-color="danger" data-off-text="ไม่ใช้งาน">
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <h5><b>พื้นที่ควบคุม</b><b class="text-red">*</b></h5>
                        <input type="text" class="form-control" id="area_name" placeholder="พื้นที่ควบคุม" />
                    </div>
                    <div class="col-md-6">
                        <h5><b>กลุ่มพื้นที่</b></h5>
                        <select class="form-control" id="groupArea" data-placeholder="กลุ่มพื้นที่">
                            <option value=""></option>
                        </select>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <h5 class="title"><b>ระบุเดือนที่ควบคุม</b></h5>
                        <div class="col-md-12" id="box_month">
                            <div class="row" id="box_month1"></div>
                            <div class="row" id="box_month2"></div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <h5 class="title"><b>ระบุช่วงเวลาควบคุม</b></h5>
                        <div class="col-md-4">
                            <input type="text" class="hidden" id="rangeCount" value="0">
                            <button type="button" class="btntime">
                                <i class="fa fa-plus" aria-hidden="true"></i>เพิ่มช่วงเวลา
                            </button>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12" id="box_choose"></div>
                </div>
                <div class="row mt25">
                    <div class="col-md-12">
                        <i class="fa fa-map-marker" style="padding-right: 3px;"></i>กำหนดพื้นที่ควบคุม     
                        <hr>
                    </div>
                    <div class="col-md-12">
                        <input type="text" class="form-control" id="pac-input" placeholder="ค้นหาสถานที่">
                        <input type="text" class="hidden" id="location">
                    </div>
                    <div class="col-md-12">
                        <div id="map"></div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="text-center divbutton">
                            <a href="/area/prohibited_area" class="btn btnback">
                                <i class="fa fa-angle-left" aria-hidden="true"></i>ยกเลิก
                            </a>
                            <a href="#" class="btn btnsave">
                                <i class="fa fa-check" aria-hidden="true"></i>บันทึกข้อมูล
                            </a>
                            <input class="hidden" type="text" name="data_id" id="data_id" />
                        </div>
                    </div>
                </div>
                <div class="modal fade" id="myModal" role="dialog">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal"></button>
                                <h4 class="modal-title"><b>เพิ่มช่วงเวลา</b></h4>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box_day1"></div>
                                        <div class="box_day2"></div>
                                    </div>
                                    <div class="col-md-12 mt15">
                                        <div class="col-md-6">
                                            <div class="col-md-3 clear-pleft">
                                                <h5>เวลาเริ่มต้น:</h5>
                                            </div>
                                            <div class="col-md-9 text-right clear-pleft">
                                                <input type="text" class="form-control" id="time1">
                                                <button type="button" class="btn searchbt">
                                                    <span class="fa fa-caret-down text-black"></span>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="col-md-3 clear-pleft">
                                                <h5>เวลาสิ้นสุด:</h5>
                                            </div>
                                            <div class="col-md-9 text-right clear-pleft">
                                                <input type="text" class="form-control" id="time2">
                                                <button type="button" class="btn searchbt">
                                                    <span class="fa fa-caret-down text-black"></span>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" data-dismiss="modal" id="btnclose">ยืนยันข้อมูล</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="select-template" class="hidden">
                    <select>
                        <option></option>
                    </select>
                </div>
                <div id="checkbox-template" class="hidden">
                    <div class="col-md-2">
                        <div class="checkbox">
                            <label class="custom">
                                <input type="checkbox">
                                <span id="checkboxorder"></span>
                                <span class="checkboxtext"></span>
                            </label>
                        </div>
                    </div>
                </div>
                <div id="timepick" class="hidden">
                    <div class="col-md-4 daychoose">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-4 day-padding text-day">
                                    <span class="day"></span><b>:</b>
                                    <button type="button" class="remove"><i class="fas fa-times text-red"></i></button>
                                </div>
                                <div class="col-md-4 day-padding text-right">
                                    <input type="text" class="form-control time_start">
                                    <button type="button" class="btn searchbt">
                                        <span class="fa fa-caret-down text-black"></span>
                                    </button>
                                </div>
                                <div class="col-md-4 day-padding text-right">
                                    <input type="text" class="form-control time_end">
                                    <button type="button" class="btn searchbt">
                                        <span class="fa fa-caret-down text-black"></span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JsContent" runat="server">
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDYiIkSi4TDBCuYj93dQOcNByklPEcD9dg&libraries=places,drawing" async defer></script>
    <script src="/resources_constant/js/select2/select2.js"></script>
    <script src="/resources_constant/js/bootstrap-switch/bootstrap-switch.js"></script>
    <script src="/resource/js/custom.js"></script>
    <script src="/resources_constant/js/jquery-timepicker/jquery.timepicker.js"></script>
    <script src="/resource/js/select_area.js"></script>
    <script src="/resource/js/prohibited_area.js"></script>
    <script type="text/javascript">
        $(function () {
            prohibited_area();
        });
    </script>
</asp:Content>
