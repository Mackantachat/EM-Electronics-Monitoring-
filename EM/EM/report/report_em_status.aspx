<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="report_em_status.aspx.vb" Inherits="EM.report_em_status" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="box box-primary">
            <form action="/Home/%23" method="post">
                <div class="box-body">
                    <div class="form-group col-md-6">
                        <h4 style="color: #429cb6"><i class="fa fa-book"></i>สถานะอุปกรณ์</h4>
                    </div>
                    <div class="clearfix"></div>
                    <div class="col-md-1"></div>
                    <div class="container" style="padding-left: 6%;">
                        <div>
                            <h5>
                                <i class="fa fa-folder"></i><b>รูปแบบรายงาน</b>
                            </h5>
                        </div>
                        <hr align="left" style="margin-top: 0px; width: 80%;" />
                        <div class="clearfix"></div>
                        <div class="col-md-5">
                            <h5>
                                <b>ประเภทรายงาน</b>
                            </h5>
                            <select class="form-control col-md-3" name="">
                                <option value="">กรุณาเลือกประเภท</option>
                                <option value="1">รายวัน</option>
                                <option value="2">รายเดือน</option>
                                <option value="3">รายปี</option>
                            </select>
                        </div>
                        <div class="col-lg-2">
                            <h5>
                                <b>วันที่เริ่มต้น</b>
                            </h5>
                            <input class="form-control" type="date" data-date="" data-date-format="DD MMMM YYYY" value="2015-08-09">
                        </div>
                        <div class="col-md-3">
                            <h5>
                                <b>วันที่สิ้นสุด</b>
                            </h5>
                            <input class="form-control" type="date" data-date="" data-date-format="DD MMMM YYYY" value="2018-08-09">
                        </div>
                        <div class="clearfix"></div>
                        <div class="col-md-5">
                            <h5>
                                <b style="padding-left: 1%;">หน่วยงาน</b>
                            </h5>
                            <div>
                                <select class="form-control col-md-1" name="">
                                    <option value="">กรุณาเลือกประเภท</option>
                                    <option value="1">รายวัน</option>
                                    <option value="2">รายเดือน</option>
                                    <option value="3">รายปี</option>
                                </select>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <div>
                            <h5>
                                <b style="padding-left: 1%;">นำข้อมูลออก</b>
                            </h5>
                            <div class="col-md-1">
                                <button type="button" class="btnmenu-report"><i class="fa fa-file-pdf-o"></i>PDF</button>
                            </div>
                            <div class="col-md-1">
                                <button type="button" class="btnmenu-report"><i class="fa fa-file-excel-o"></i>Excel</button>
                            </div>
                            <div class="col-md-1">
                                <button type="button" class="btnmenu-report"><i class="fa fa-file-word-o"></i>Word</button>
                            </div>
                            <div class="col-md-1">
                                <button type="button" class="btnmenu-report"><i class="fa fa-file-code-o"></i>XML</button>
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JsContent" runat="server">
</asp:Content>
