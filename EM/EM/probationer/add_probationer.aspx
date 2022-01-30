<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="add_probationer.aspx.vb" Inherits="EM.AddProbationer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/resources_constant/AdminLTE2/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css">
    <link rel="stylesheet" href="/resources_constant/css/jquery-timepicker/jquery.timepicker.css">
    <link rel="stylesheet" href="/resources_constant/css/bootstrap-switch/bootstrap-switch.min.css">
    <link rel="stylesheet" href="/resources_constant/css/bootstrapvalidator/bootstrapValidator.min.css">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2.min.css">
    <link rel="stylesheet" href="/resources_constant/css/select2/select2-bootstrap.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <input id="add" class="hidden" />
        <div class="box">
            <form action="#" id="probationer">
                <div class="row text-header probationer">
                    <div class="col-md-6">
                        <h3 class="text-light-blue"><span id="subject">เพิ่มข้อมูลผู้ถูกควบคุมความประพฤติ</span></h3>
                    </div>
                    <div class="col-md-6 text-right">
                        <h5><b>สถานะ<b class="text-red">*</b></b></h5>
                        <input type="checkbox" class="hidden" id="probationer_status" name="status" checked data-on-color="success" data-on-text="Active" data-off-color="danger" data-off-text="Inactive">
                    </div>
                </div>
                <div class="probationer">
                    <div class="panel-group" id="information" role="tablist" aria-multiselectable="true">
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="heading_probationer">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#information" href="#info_probationer" aria-expanded="true" aria-controls="info_probationer">ข้อมูลผู้ถูกควบคุมความประพฤติ</a>
                                </h4>
                            </div>
                            <div id="info_probationer" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="heading_probationer">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h5><i class="fas fa-user"></i><b>ข้อมูลส่วนตัว</b></h5>
                                            <hr />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">
                                            <h5><b>คำนำหน้า</b><b class="text-red">*</b></h5>
                                            <select class="form-control required title" id="probationer_title" data-placeholder="คำนำหน้า">
                                                <option selected></option>
                                            </select>
                                        </div>
                                        <div class="col-md-4">
                                            <h5><b>ชื่อ</b><b class="text-red">*</b></h5>
                                            <input type="text" class="form-control required" id="probationer_name" placeholder="ชื่อ" />
                                        </div>
                                        <div class="col-md-4">
                                            <h5><b>นามสกุล</b><b class="text-red">*</b></h5>
                                            <input type="text" class="form-control required" id="probationer_last_name" placeholder="นามสกุล" />
                                        </div>
                                        <div class="col-md-2">
                                            <h5><b>เพศ</b><b class="text-red">*</b></h5>
                                            <select class="form-control required gender" id="probationer_gender" data-placeholder="เพศ">
                                                <option selected></option>
                                                <option value="1">หญิง</option>
                                                <option value="2">ชาย</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">
                                            <h5><b>อายุ</b><b class="text-red">*</b></h5>
                                            <input type="text" class="form-control required number" id="probationer_age" placeholder="อายุ" maxlength="3" />
                                        </div>
                                        <div class="col-md-4">
                                            <h5><b>เลขประจำตัวประชาชน</b><b class="text-red">*</b></h5>
                                            <input type="text" class="form-control required cardid" id="probationer_cardid" placeholder="เลขประจำตัวประชาชน" maxlength="13" />
                                        </div>
                                        <div class="col-md-3">
                                            <h5><b>การศึกษาสูงสุด</b></h5>
                                            <select class="form-control required" id="probationer_education">
                                                <option selected></option>
                                            </select>
                                        </div>
                                        <div class="col-md-3">
                                            <h5><b>อาชีพ</b><b class="text-red">*</b></h5>
                                            <input type="text" class="form-control required" id="probationer_career" placeholder="อาชีพ" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <h5><b>เบอร์โทรศัพท์(มือถือ)</b><b class="text-red">*</b></h5>
                                            <input type="text" class="form-control required" id="probationer_mobile" placeholder="เบอร์โทรศัพท์(มือถือ)" />
                                        </div>
                                        <div class="col-md-3">
                                            <h5><b>เบอร์โทรศัพท์</b></h5>
                                            <input type="text" class="form-control" id="probationer_telephone" placeholder="เบอร์โทรศัพท์" />
                                        </div>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="row mt-15">
                                        <div class="col-md-12">
                                            <h5><i class="fas fa-home" aria-hidden="true"></i><b>ข้อมูลที่อยู่ตามทะเบียนราษฎร์</b></h5>
                                            <hr />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <h5><b>เลขที่</b><b class="text-red">*</b></h5>
                                            <input type="text" class="form-control required homeno" id="probationer_address_number" placeholder="เลขที่" />
                                        </div>
                                        <div class="col-md-2">
                                            <h5><b>หมู่</b></h5>
                                            <input type="text" class="form-control number" id="probationer_moo" placeholder="หมู่" />
                                        </div>
                                        <div class="col-md-5">
                                            <h5><b>ถนน</b></h5>
                                            <input type="text" class="form-control" id="probationer_road" placeholder="ถนน" />
                                        </div>
                                        <div class="col-md-2">
                                            <h5><b>ซอย</b></h5>
                                            <input type="text" class="form-control" id="probationer_lane" placeholder="ซอย" />
                                        </div>
                                    </div>
                                    <div class="row address">
                                        <div class="col-md-4">
                                            <h5><b>จังหวัด</b><b class="text-red">*</b></h5>
                                            <select class="form-control required province" id="probationer_province" data-placeholder="จังหวัด">
                                                <option selected></option>
                                            </select>
                                        </div>
                                        <div class="col-md-4">
                                            <h5><b>อำเภอ/เขต</b><b class="text-red">*</b></h5>
                                            <select class="form-control required district" id="probationer_district" data-placeholder="อำเภอ/เขต"></select>
                                        </div>
                                        <div class="col-md-4">
                                            <h5><b>ตำบล/แขวง</b><b class="text-red">*</b></h5>
                                            <select class="form-control required subdistrict" id="probationer_subdistrict" data-placeholder="ตำบล/แขวง"></select>
                                        </div>
                                    </div>
                                    <div class="row mt-15">
                                        <div class="col-md-12">
                                            <h5><i class="fas fa-home" aria-hidden="true"></i><b>ข้อมูลที่อยู่ปัจจุบัน</b></h5>
                                            <hr />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="checkbox">
                                                <label class="custom">
                                                    <input id="same_address" type="checkbox">
                                                    <span>เหมือนกับที่อยู่ตามทะเบียนราษฎร์</span>
                                                    <span class="checkboxtext"></span>
                                                </label>
                                            </div>
                                        </div>
                                        <hr />
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <h5><b>เลขที่</b><b class="text-red">*</b></h5>
                                            <input type="text" class="form-control required homeno" id="current_address_number" placeholder="เลขที่" />
                                        </div>
                                        <div class="col-md-2">
                                            <h5><b>หมู่</b></h5>
                                            <input type="text" class="form-control" id="current_moo" placeholder="หมู่" />
                                        </div>
                                        <div class="col-md-5">
                                            <h5><b>ถนน</b></h5>
                                            <input type="text" class="form-control" id="current_road" placeholder="ถนน" />
                                        </div>
                                        <div class="col-md-2">
                                            <h5><b>ซอย</b></h5>
                                            <input type="text" class="form-control" id="current_lane" placeholder="ซอย" />
                                        </div>
                                    </div>
                                    <div class="row address">
                                        <div class="col-md-4">
                                            <h5><b>จังหวัด</b><b class="text-red">*</b></h5>
                                            <select class="form-control required province" id="current_province" data-placeholder="จังหวัด">
                                                <option selected></option>
                                            </select>
                                        </div>
                                        <div class="col-md-4">
                                            <h5><b>อำเภอ/เขต</b><b class="text-red">*</b></h5>
                                            <select class="form-control required district" id="current_district" data-placeholder="อำเภอ/เขต"></select>
                                        </div>
                                        <div class="col-md-4">
                                            <h5><b>ตำบล/แขวง</b><b class="text-red">*</b></h5>
                                            <select class="form-control required subdistrict" id="current_subdistrict" data-placeholder="ตำบล/แขวง"></select>
                                        </div>
                                    </div>
                                    <div class="row mt-15">
                                        <div class="col-md-12">
                                            <h5><i class="fas fa-users" aria-hidden="true"></i>กำหนดเจ้าหน้าที่ที่มีสิทธิ์ในการตรวจสอบติดตามผู้ถูกคุมรายนี้ <b class="text-red">*</b></h5>
                                            <hr />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="col-md-3">
                                                <h5>เจ้าหน้าที่</h5>
                                            </div>
                                            <div class="col-md-3">
                                                <h5>ระดับสิทธิ</h5>
                                            </div>
                                            <div class="col-md-3">
                                                <h5>หน่วยงาน</h5>
                                            </div>
                                            <div class="col-md-3">
                                                <h5>จัดการ</h5>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <input type="text" class="hidden" id="count_permis" value="0">
                                        <div class="col-md-12" id="row_permission"></div>
                                    </div>
                                    <div class="row col-md-12" id="show_permission"></div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <button type="button" class="btnofficial permission">
                                                <i class="fa fa-user-plus" aria-hidden="true"></i>กำหนดเจ้าหน้าที่
                                           
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row mt-15">
                                        <div class="col-md-12">
                                            <h5><i class="fas fa-bell"></i>กำหนดการส่งแจ้งเตือนผ่านหน้าเว็บไซต์</h5>
                                            <hr />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12" id="web">
                                            <div class="col-md-3">
                                                <h5>เจ้าหน้าที่</h5>
                                            </div>
                                            <div class="col-md-3">
                                                <h5>ระดับสิทธิ</h5>
                                            </div>
                                            <div class="col-md-3">
                                                <h5>หน่วยงาน</h5>
                                            </div>
                                            <div class="col-md-3">
                                                <h5>จัดการ</h5>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <input type="text" class="hidden" id="count_noti" value="0">
                                        <div class="col-md-12" id="row_notification"></div>
                                    </div>
                                    <div class="row" style="padding-bottom: 1.4em;">
                                        <div class="col-md-12">
                                            <button type="button" class="btnofficial notification">
                                                <i class="fas fa-user-plus" aria-hidden="true"></i>กำหนดเจ้าหน้าที่
                                           
                                            </button>
                                        </div>
                                    </div>
                                    <fieldset class="noti-border">
                                        <legend class="noti-border">กำหนดการแจ้งเตือนผ่านอีเมล์</legend>
                                        <div class="row">
                                            <div class="col-md-7">
                                                <input type="text" class="form-control required noti" id="notification_by_email" placeholder="ระบุอีเมล์สำหรับการแจ้งเตือน" />
                                            </div>
                                            <div class="clearfix"></div>
                                            <div class="col-md-12">
                                                <p class="small noti">หากต้องการ การแจ้งเตือนผ่านอีเมล์มากกว่า 1 Email Address สามารถใส่ , (Comma) เพื่อระบุ Email Address เพิ่มได้เช่น one@example.com,two@example.com</p>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <fieldset class="noti-border">
                                        <legend class="noti-border">กำหนดการแจ้งเตือนผ่าน SMS</legend>
                                        <div class="row">
                                            <div class="col-md-7">
                                                <input type="text" class="form-control required noti" id="notification_by_sms" placeholder="ระบุหมายเลขโทรศัพท์" />
                                            </div>
                                            <div class="clearfix"></div>
                                            <div class="col-md-12">
                                                <p class="small noti">หากต้องการ การแจ้งเตือนผ่าน SMS มากกว่า 1 หมายเลขสามารถใส่ , (Comma) เพื่อระบุหมายเลขโทรศัพท์เพิ่มได้เช่น 0xx-xxxxxxx,0xx-xxxxxxx</p>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <%--End panel-body--%>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="heading_reference">
                                <h4 class="panel-title">
                                    <a class="collapsed" role="button" data-toggle="collapse" data-parent="#information" href="#info_reference" aria-expanded="false" aria-controls="info_reference">ข้อมูลบุคคลอ้างอิง
                                    </a>
                                </h4>
                            </div>
                            <div id="info_reference" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading_reference">
                                <div class="panel-body">
                                    <fieldset class="scheduler-border" id="reference1">
                                        <legend class="scheduler-border">บุคคลอ้างอิงที่ 1</legend>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h5><i class="fas fa-user"></i><b>ข้อมูลส่วนตัว</b></h5>
                                                <hr />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2">
                                                <h5><b>คำนำหน้า</b><b class="text-red">*</b></h5>
                                                <select class="form-control required title" id="reference_title1" name="reference_title" data-placeholder="คำนำหน้า">
                                                    <option selected></option>
                                                </select>
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>ชื่อ</b><b class="text-red">*</b></h5>
                                                <input type="text" class="form-control required" id="reference_name1" name="reference_name" placeholder="ชื่อ" />
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>นามสกุล</b><b class="text-red">*</b></h5>
                                                <input type="text" class="form-control required" id="reference_last_name1" name="reference_last_name" placeholder="นามสกุล" />
                                            </div>
                                            <div class="col-md-2">
                                                <h5><b>เพศ</b><b class="text-red">*</b></h5>
                                                <select class="required form-control gender" id="reference_gender1" name="reference_gender" data-placeholder="เพศ">
                                                    <option selected></option>
                                                    <option value="1">หญิง</option>
                                                    <option value="2">ชาย</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2">
                                                <h5><b>อายุ</b><b class="text-red">*</b></h5>
                                                <input type="text" class="form-control required" id="reference_age1" name="reference_age" placeholder="อายุ" maxlength="3" />
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>เลขประจำตัวประชาชน</b><b class="text-red">*</b></h5>
                                                <input type="text" class="form-control required cardid" id="reference_cardid1" name="reference_cardid" placeholder="เลขประจำตัวประชาชน" maxlength="13" />
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>เบอร์โทรศัพท์(มือถือ)</b><b class="text-red">*</b></h5>
                                                <input type="text" class="form-control required" id="reference_mobile1" name="reference_mobile" placeholder="เบอร์โทรศัพท์(มือถือ)" />
                                            </div>
                                        </div>
                                        <div class="row mt-15">
                                            <div class="col-md-12">
                                                <h5><i class="fas fa-home" aria-hidden="true"></i><b>ข้อมูลที่อยู่ตามทะเบียนราษฎร์</b></h5>
                                                <hr />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <h5><b>เลขที่</b><b class="text-red">*</b></h5>
                                                <input type="text" class="form-control required homeno" id="reference_address_number1" name="reference_address_number" placeholder="เลขที่" />
                                            </div>
                                            <div class="col-md-2">
                                                <h5><b>หมู่</b></h5>
                                                <input type="text" class="form-control" id="reference_moo1" name="reference_moo" placeholder="หมู่" />
                                            </div>
                                            <div class="col-md-5">
                                                <h5><b>ถนน</b></h5>
                                                <input type="text" class="form-control" id="reference_road1" name="reference_road" placeholder="ถนน" />
                                            </div>
                                            <div class="col-md-2">
                                                <h5><b>ซอย</b></h5>
                                                <input type="text" class="form-control" id="reference_lane1" name="reference_lane" placeholder="ซอย" />
                                            </div>
                                        </div>
                                        <div class="row address">
                                            <div class="col-md-4">
                                                <h5><b>จังหวัด</b><b class="text-red">*</b></h5>
                                                <select class="form-control required province" id="reference_province1" name="reference_province" data-placeholder="จังหวัด">
                                                    <option selected></option>
                                                </select>
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>อำเภอ/เขต</b><b class="text-red">*</b></h5>
                                                <select class="form-control required district" id="reference_district1" name="reference_district" data-placeholder="อำเภอ/เขต"></select>
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>ตำบล/แขวง</b><b class="text-red">*</b></h5>
                                                <select class="form-control required subdistrict" id="reference_subdistrict1" name="reference_subdistrict" data-placeholder="ตำบล/แขวง"></select>
                                            </div>
                                        </div>
                                        <div class="row mt-15">
                                            <div class="col-md-12">
                                                <h5><i class="fas fa-home" aria-hidden="true"></i><b>ข้อมูลที่อยู่ปัจจุบัน</b></h5>
                                                <hr />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="checkbox">
                                                    <label class="custom">
                                                        <input class="same_address" type="checkbox">
                                                        <span>เหมือนกับที่อยู่ตามทะเบียนราษฎร์</span>
                                                        <span class="checkboxtext"></span>
                                                    </label>
                                                </div>
                                            </div>
                                            <hr />
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <h5><b>เลขที่</b><b class="text-red">*</b></h5>
                                                <input type="text" class="form-control required homeno" id="reference_current_address_number1" name="reference_current_address_number" placeholder="เลขที่" />
                                            </div>
                                            <div class="col-md-2">
                                                <h5><b>หมู่</b></h5>
                                                <input type="text" class="form-control" id="reference_current_moo1" name="reference_current_moo" placeholder="หมู่" />
                                            </div>
                                            <div class="col-md-5">
                                                <h5><b>ถนน</b></h5>
                                                <input type="text" class="form-control" id="reference_current_road1" name="reference_current_road" placeholder="ถนน" />
                                            </div>
                                            <div class="col-md-2">
                                                <h5><b>ซอย</b></h5>
                                                <input type="text" class="form-control" id="reference_current_lane1" name="reference_current_lane" placeholder="ซอย" />
                                            </div>
                                        </div>
                                        <div class="row address">
                                            <div class="col-md-4">
                                                <h5><b>จังหวัด</b><b class="text-red">*</b></h5>
                                                <select class="form-control required province" id="reference_current_province1" name="reference_current_province" data-placeholder="จังหวัด">
                                                    <option selected></option>
                                                </select>
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>อำเภอ/เขต</b><b class="text-red">*</b></h5>
                                                <select class="form-control required district" id="reference_current_district1" name="reference_current_district" data-placeholder="อำเภอ/เขต"></select>
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>ตำบล/แขวง</b><b class="text-red">*</b></h5>
                                                <select class="form-control required subdistrict" id="reference_current_subdistrict1" name="reference_current_subdistrict" data-placeholder="ตำบล/แขวง"></select>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <fieldset id="reference2" class="scheduler-border">
                                        <legend class="scheduler-border">บุคคลอ้างอิงที่ 2</legend>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h5><i class="fas fa-user"></i><b>ข้อมูลส่วนตัว</b></h5>
                                                <hr />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2">
                                                <h5><b>คำนำหน้า</b><b class="text-red">*</b></h5>
                                                <select class="form-control required title" id="reference_title2" name="reference_title" data-placeholder="คำนำหน้า">
                                                    <option selected></option>
                                                </select>
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>ชื่อ</b><b class="text-red">*</b></h5>
                                                <input type="text" class="form-control required" id="reference_name2" name="reference_name" placeholder="ชื่อ" />
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>นามสกุล</b><b class="text-red">*</b></h5>
                                                <input type="text" class="form-control required" id="reference_last_name2" name="reference_last_name" placeholder="นามสกุล" />
                                            </div>
                                            <div class="col-md-2">
                                                <h5><b>เพศ</b><b class="text-red">*</b></h5>
                                                <select class="required form-control gender" id="reference_gender2" name="reference_gender" data-placeholder="เพศ">
                                                    <option selected></option>
                                                    <option value="1">หญิง</option>
                                                    <option value="2">ชาย</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2">
                                                <h5><b>อายุ</b><b class="text-red">*</b></h5>
                                                <input type="text" class="form-control required" id="reference_age2" name="reference_age" placeholder="อายุ" maxlength="3" />
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>เลขประจำตัวประชาชน</b><b class="text-red">*</b></h5>
                                                <input type="text" class="form-control required cardid" id="reference_cardid2" name="reference_cardid" placeholder="เลขประจำตัวประชาชน" maxlength="13" />
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>เบอร์โทรศัพท์(มือถือ)</b><b class="text-red">*</b></h5>
                                                <input type="text" class="form-control required" id="reference_mobile2" name="reference_mobile" placeholder="เบอร์โทรศัพท์(มือถือ)" />
                                            </div>
                                        </div>
                                        <div class="row mt-15">
                                            <div class="col-md-12">
                                                <h5><i class="fas fa-home" aria-hidden="true"></i><b>ข้อมูลที่อยู่ตามทะเบียนราษฎร์</b></h5>
                                                <hr />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <h5><b>เลขที่</b><b class="text-red">*</b></h5>
                                                <input type="text" class="form-control required homeno" id="reference_address_number2" name="reference_address_number" placeholder="เลขที่" />
                                            </div>
                                            <div class="col-md-2">
                                                <h5><b>หมู่</b></h5>
                                                <input type="text" class="form-control" id="reference_moo2" name="reference_moo" placeholder="หมู่" />
                                            </div>
                                            <div class="col-md-5">
                                                <h5><b>ถนน</b></h5>
                                                <input type="text" class="form-control" id="reference_road2" name="reference_road" placeholder="ถนน" />
                                            </div>
                                            <div class="col-md-2">
                                                <h5><b>ซอย</b></h5>
                                                <input type="text" class="form-control" id="reference_lane2" name="reference_lane" placeholder="ซอย" />
                                            </div>
                                        </div>
                                        <div class="row address">
                                            <div class="col-md-4">
                                                <h5><b>จังหวัด</b><b class="text-red">*</b></h5>
                                                <select class="form-control required province" id="reference_province2" name="reference_province" data-placeholder="จังหวัด">
                                                    <option selected></option>
                                                </select>
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>อำเภอ/เขต</b><b class="text-red">*</b></h5>
                                                <select class="form-control required district" id="reference_district2" name="reference_district" data-placeholder="อำเภอ/เขต"></select>
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>ตำบล/แขวง</b><b class="text-red">*</b></h5>
                                                <select class="form-control required subdistrict" id="reference_subdistrict2" name="reference_subdistrict" data-placeholder="ตำบล/แขวง"></select>
                                            </div>
                                        </div>
                                        <div class="row mt-15">
                                            <div class="col-md-12">
                                                <h5><i class="fas fa-home" aria-hidden="true"></i><b>ข้อมูลที่อยู่ปัจจุบัน</b></h5>
                                                <hr />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="checkbox">
                                                    <label class="custom">
                                                        <input class="same_address" type="checkbox">
                                                        <span>เหมือนกับที่อยู่ตามทะเบียนราษฎร์</span>
                                                        <span class="checkboxtext"></span>
                                                    </label>
                                                </div>
                                            </div>
                                            <hr />
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <h5><b>เลขที่</b><b class="text-red">*</b></h5>
                                                <input type="text" class="form-control required homeno" id="reference_current_address_number2" name="reference_current_address_number" placeholder="เลขที่" />
                                            </div>
                                            <div class="col-md-2">
                                                <h5><b>หมู่</b></h5>
                                                <input type="text" class="form-control" id="reference_current_moo2" name="reference_current_moo" placeholder="หมู่" />
                                            </div>
                                            <div class="col-md-5">
                                                <h5><b>ถนน</b></h5>
                                                <input type="text" class="form-control" id="reference_current_road2" name="reference_current_road" placeholder="ถนน" />
                                            </div>
                                            <div class="col-md-2">
                                                <h5><b>ซอย</b></h5>
                                                <input type="text" class="form-control" id="reference_current_lane2" name="reference_current_lane" placeholder="ซอย" />
                                            </div>
                                        </div>
                                        <div class="row address">
                                            <div class="col-md-4">
                                                <h5><b>จังหวัด</b><b class="text-red">*</b></h5>
                                                <select class="form-control required province" id="reference_current_province2" name="reference_current_province" data-placeholder="จังหวัด">
                                                    <option selected></option>
                                                </select>
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>อำเภอ/เขต</b><b class="text-red">*</b></h5>
                                                <select class="form-control required distric" id="reference_current_district2" name="reference_current_district" data-placeholder="อำเภอ/เขต"></select>
                                            </div>
                                            <div class="col-md-4">
                                                <h5><b>ตำบล/แขวง</b><b class="text-red">*</b></h5>
                                                <select class="form-control required subdistrict" id="reference_current_subdistrict2" name="reference_current_subdistrict" data-placeholder="ตำบล/แขวง"></select>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <%--End panel-body--%>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="heading_probation">
                                <h4 class="panel-title">
                                    <a class="collapsed" role="button" data-toggle="collapse" data-parent="#information" href="#collapse_probation" aria-expanded="false" aria-controls="collapse_probation">ข้อมูลการคุมความประพฤติอยู่</a>
                                </h4>
                            </div>
                            <div id="collapse_probation" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading_probation">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <h5><b>ชื่อผู้สั่งใช้</b><b class="text-red">*</b></h5>
                                            <input id="probation_Officer_Name" type="text" class="required form-control" placeholder="ชื่อผู้สั่งใช้" />
                                        </div>
                                        <div class="col-md-4">
                                            <h5><b>นามสกุลผู้สั่งใช้</b></h5>
                                            <input id="probation_Officer_LastName" type="text" class="form-control" placeholder="นามสกุลผู้สั่งใช้" />
                                        </div>
                                        <div class="col-md-4">
                                            <h5><b>หมายเลขโทรศัพท์ผู้สั่งใช้</b></h5>
                                            <input id="probation_Officer_Telephone" type="text" class="form-control" placeholder="หมายเลขโทรศัพท์ผู้สั่งใช้" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <h5><b>คดีอาญาหมายเลขดำที่</b></h5>
                                            <input id="probationer_case_black_number" type="text" class="form-control" placeholder="......../........" />
                                        </div>
                                        <div class="col-md-4">
                                            <h5><b>คดีอาญาหมายเลขแดงที่</b></h5>
                                            <input id="probationer_case_red_number" type="text" class="form-control" placeholder="......../........" />
                                        </div>
                                        <div class="col-md-4">
                                            <h5><b>เลขทะเบียนคดี</b></h5>
                                            <input id="probationer_caseID" type="text" class="form-control" placeholder="......../........" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h5><b>หน่วยงานผู้รับผิดชอบ</b><b class="text-red">*</b></h5>
                                            <select class="required form-control" id="probationer_department" data-placeholder="หน่วยงานผู้รับผิดชอบ">
                                                <option>เลือก</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="row mt-15">
                                        <div class="col-md-12">
                                            <h5><i class="fas fa-book-open"></i><b>ฐานความผิด</b></h5>
                                            <hr />
                                        </div>
                                    </div>
                                    <div class="row all_main_offence">
                                        <div id="list_offense"></div>
                                    </div>
                                    <div class="row mt-15">
                                        <div class="col-md-12">
                                            <h5><i class="fa fa-database" aria-hidden="true"></i><b>เงื่อนไขตามประมวลกฏหมายอาญามาตรา 56</b></h5>
                                            <hr />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12 mt-15" id="condition"></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h5><b>เงื่อนไขต่างๆเพิ่มเติม</b></h5>
                                            <textarea id="probationer_remark" class="form-control" rows="5" placeholder="เงื่อนไขต่างๆเพิ่มเติม"></textarea>
                                        </div>
                                    </div>
                                    <%--End panel-body--%>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="heading_device">
                                <h4 class="panel-title">
                                    <a class="collapsed" role="button" data-toggle="collapse" data-parent="#information" href="#collapse_device" aria-expanded="false" aria-controls="collapse_device">เครื่องมือติดตามตัวอิเล็กทรอนิกส์</a>
                                </h4>
                            </div>
                            <div id="collapse_device" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading_device">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h5><b>รหัสประจำเครื่อง</b><b class="text-red">*</b></h5>
                                            <select class="required form-control" id="probationer_EM_devices" data-placeholder="รหัสประจำเครื่อง">
                                                <option selected></option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="checkbox">
                                                <label class="custom">
                                                    <input type="checkbox" id="probationer_activate_homebase">
                                                    <span>เปิดการใช้งาน Homebase</span>
                                                    <span class="checkboxtext"></span>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-10">
                                        <div class="col-md-3">
                                            <h5><b>วันที่ติดตั้งอุปกรณ์</b><b class="text-red">*</b></h5>
                                            <input id="probationer_IMEI_start_date" name="probationer_IMEI_start_date" type="text" class="required form-control calendar" placeholder="วัน/เดือน/ปี" data-date-format="dd/mm/yyyy" />
                                        </div>
                                        <div class="col-md-3">
                                            <h5><b>วันที่ครบกำหนด</b><b class="text-red">*</b></h5>
                                            <input id="probationer_judge_end_date" type="text" class="required form-control calendar" placeholder="วัน/เดือน/ปี" data-date-format="dd/mm/yyyy" />
                                        </div>
                                        <div class="col-md-3">
                                            <h5><b>วันที่ถอดอุปกรณ์</b><b class="text-red">*</b></h5>
                                            <input id="probationer_IMEI_end_date" type="text" class="required form-control calendar" placeholder="วัน/เดือน/ปี" data-date-format="dd/mm/yyyy" />
                                        </div>
                                        <div class="col-md-3">
                                            <h5><b>จำนวนวันที่ติดตั้ง</b><b class="text-red">*</b></h5>
                                            <input id="probationer_date_total" type="text" class="required form-control number" placeholder="จำนวนวันที่ติดตั้ง" />
                                        </div>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h5><b>ข้อมูลอื่นเพิ่มเติม</b></h5>
                                            <textarea id="probationer_other" class="form-control" rows="3" placeholder="ข้อมูลอื่นเพิ่มเติม"></textarea>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <h5><i class="fa fa-area-chart" aria-hidden="true"></i>รูปภาพผู้ถูกคุมความประพฤติ</h5>
                                            <hr />
                                        </div>
                                        <div class="col-md-6">
                                            <h5><i class="fa fa-hdd-o" aria-hidden="true"></i>เอกสารหลักฐาน</h5>
                                            <hr />
                                        </div>
                                    </div>
                                    <div class="row" id="upimg">
                                        <div class="col-md-6">
                                            <div class="col-md-12">
                                                <h5>ไฟล์รูปที่ 1 <b class="text-red">*</b>:</h5>
                                                <input id="image1" type="file" name="pic" accept="image/jpg, image/png, image/gif, image/bmp">
                                                <hr class="hrbt" />
                                            </div>
                                            <div class="col-md-12">
                                                <h5>ไฟล์รูปที่ 2 :</h5>
                                                <input id="image2" type="file" name="pic" accept="image/jpg, image/png, image/gif, image/bmp">
                                                <hr class="hrbt" />
                                            </div>
                                            <div class="col-md-12">
                                                <button type="button" class="btnnoti">
                                                    <i class="fas fa-plus"></i>เพิ่ม
                                               
                                                </button>
                                            </div>
                                            <div class="col-md-12">
                                                <h5 class="text-blue">รองรับไฟล์รูปภาพเท่านั้น(.jpg .png .gif .bmp) ขนาดไม่เกิน 5 เมกะไบต์/1 ไฟล์</h5>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="col-md-12">
                                                <h5>ไฟล์เอกสารที่ 1 <b class="text-red">*</b>:</h5>
                                                <input id="doc1" type="file" name="pic" accept="image/jpg, image/png, image/gif, image/bmp">
                                                <hr class="hrbt" />
                                            </div>
                                            <div class="col-md-12">
                                                <h5>ไฟล์เอกสารที่ 2 :</h5>
                                                <input id="doc2" type="file" name="pic" accept="image/jpg, image/png, image/gif, image/bmp">
                                                <hr class="hrbt" />
                                            </div>
                                            <div class="col-md-12">
                                                <button type="button" class="btnnoti">
                                                    <i class="fas fa-plus"></i>เพิ่ม
                                               
                                                </button>
                                            </div>
                                            <div class="col-md-12">
                                                <h5 class="text-blue">รองรับไฟล์เอกสารเท่านั้น(.pdf) ขนาดไม่เกิน 5 เมกะไบต์/1 ไฟล์</h5>
                                            </div>
                                        </div>
                                    </div>
                                    <%--End panel-body--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="text-center divbutton">
                            <a href="/probationer/probationer" class="btn btnback">
                                <i class="fa fa-angle-left" aria-hidden="true"></i>ยกเลิก
                            </a>
                            <a href="#" class="btn btnsave">
                                <i class="fa fa-check" aria-hidden="true"></i>บันทึกข้อมูล
                            </a>
                            <input class="hidden" type="text" id="pid" />
                        </div>
                    </div>
                </div>
            </form>
            <div id="select-template" class="hidden">
                <select>
                    <option></option>
                </select>
            </div>
            <div id="timepick" class="hidden">
                <div class="col-md-4 daychoose">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-4 day-padding">
                                <span class="day"></span><b>:</b>
                                <button type="button" class="remove"><i class="fa fa-remove text-red"></i></button>
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
        <div id="row_show" class="row hidden">
            <div class="col-md-12">
                <div class="col-md-3">
                    <span id="acc"></span>
                </div>
                <div class="col-md-3">
                    <span id="role"></span>
                </div>
                <div class="col-md-3">
                    <span id="dep"></span>
                </div>
                <div class="col-md-3">
                    <button type="button" class="remove"><i class="fa fa-remove text-red"></i></button>
                </div>
            </div>
        </div>
        <div id="checkbox-template" class="hidden">
            <div class="col-md-12 clear-pleft">
                <div class="col-md-12 offence">
                    <div class="checkbox">
                        <label class="custom">
                            <input type="checkbox">
                            <span id="checkboxorder"></span>
                            <span class="checkboxtext"></span>
                        </label>
                    </div>
                </div>
                <div class="col-md-12 sub_offence hidden"></div>
            </div>
        </div>
        <div class="condition-template hidden">
            <div>
                <div class="rowchoose">
                    <div class="col-md-3 clear-pleft">
                        <div class="checkbox">
                            <label class="custom">
                                <input type="checkbox">
                                <span id="condition_name"></span>
                                <span class="checkboxtext"></span>
                            </label>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <input type="text" class="form-control text_condition" />
                    </div>
                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-3 text-center">
                                <h5><b>ระหว่างเวลา:</b></h5>
                            </div>
                            <div class="col-md-4 day-padding text-right">
                                <input type="text" class="form-control start_time">
                                <button type="button" class="btn searchbt">
                                    <span class="fa fa-caret-down text-black"></span>
                                </button>
                            </div>
                            <div class="col-md-1">
                                <h5><b>ถึง</b></h5>
                            </div>
                            <div class="col-md-4 day-padding text-right">
                                <input type="text" class="form-control end_time">
                                <button type="button" class="btn searchbt">
                                    <span class="fa fa-caret-down text-black"></span>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"></button>
                        <h4 class="fa fa-user-plus"><b>กำหนดเจ้าหน้าที่</b></h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="checkbox">
                                    <label class="custom">
                                        <input id="staff" type="checkbox" checked="checked">
                                        <span>เลือกจากรายชื่อเจ้าหน้าที่</span>
                                        <span class="checkboxtext"></span>
                                    </label>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="checkbox">
                                    <label class="custom">
                                        <input id="group_staff" type="checkbox">
                                        <span>เลือกจากกลุ่มเจ้าหน้าที่</span>
                                        <span class="checkboxtext"></span>
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12" id="listdata"></div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal" id="btnclose">ยืนยันข้อมูล</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JsContent" runat="server">
    <script src="/resources_constant/AdminLTE2/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.js"></script>
    <script src="/resources_constant/AdminLTE2/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker-custom.js"></script>
    <script src="/resources_constant/AdminLTE2/bower_components/bootstrap-datepicker/js/locales/bootstrap-datepicker.th.js"></script>
    <script src="/resources_constant/AdminLTE2/bower_components/moment/min/moment-with-locales.min.js"></script>
    <script src="/resources_constant/js/jquery-timepicker/jquery.timepicker.js"></script>
    <script src="/resources_constant/js/bootstrap-switch/bootstrap-switch.min.js"></script>
    <script src="/resources_constant/js/bootstrapvalidator/bootstrapValidator.min.js"></script>
    <script src="/resources_constant/js/jquery.ui.datepicker.validation/jquery.ui.datepicker.validation.js"></script>
    <script src="/resources_constant/js/bootstrapvalidator/language/th_TH.js"></script>
    <script src="/resources_constant/js/jquery.mask/dist/jquery.mask.js"></script>
    <script src="/resources_constant/js/select2/select2.js"></script>
    <script src="/resource/js/custom.js"></script>
    <script src="/resource/js/select_title.js"></script>
    <script src="/resource/js/select_address.js"></script>
    <script src="/resource/js/select_department.js"></script>
    <script src="/resource/js/probationer.js"></script>
    <script type="text/javascript">
        $(function () {
            $.busyLoadFull("show", { spinner: "circles" });
            probationer();
            setTimeout(function () {
                $.busyLoadFull("hide");
            }, 2000);
        });
    </script>
</asp:Content>
