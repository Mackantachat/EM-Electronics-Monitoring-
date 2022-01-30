<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="default.aspx.vb" Inherits="EM._default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link rel="apple-touch-icon" sizes="76x76" href="resources_constant/images/logo.png" />
    <link rel="icon" type="image/png" sizes="96x96" href="resources_constant/images/logo.png" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Sarabun" />
    <link rel="stylesheet" href="resources_constant/css/bootstrap/bootstrap.min.css" />
    <link rel="stylesheet" href="/resources_constant/css/alertify/alertify.rtl.css"/>
    <link rel="stylesheet" href="/resources_constant/css/alertify/themes/default.rtl.css" />
    <link rel="stylesheet" href="resource/css/login.css" />
</head>
<body>
    <div class="container">
        <div class="row">
            <div class="col-xs-12 text-center">
                <img src="resources_constant/images/01_Libra.png" />
            </div>
        </div>
        <div class="row main">
            <div class="col-xs-12 ">
                <div class="row input">
                    <div class="col-xs-12 col-md-4 col-lg-4">
                        <h5 class="text-white">รหัสศาล:</h5>
                    </div>
                    <div class="col-xs-12 col-md-8 col-lg-8">
                        <input type="text" class="form-control" id="code" placeholder="eg. C001" />
                    </div>
                </div>
                <div class="row input">
                    <div class="col-xs-12 col-md-4 col-lg-4">
                        <h5 class="text-white">ชื่อบัญชีผู้ใช้งาน:</h5>
                    </div>
                    <div class="col-xs-12 col-md-8 col-lg-8">
                        <input type="text" class="form-control" id="username" placeholder="eg. ADMIN" />
                    </div>
                </div>
                <div class="row input">
                    <div class="col-xs-12 col-md-4 col-lg-4">
                        <h5 class="text-white">รหัสผ่าน:</h5>
                    </div>
                    <div class="col-xs-12 col-md-8 col-lg-8">
                        <input type="password" class="form-control" id="password" placeholder="&#9679;&#9679;&#9679;&#9679;&#9679;&#9679;&#9679;&#9679;" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-md-4 col-lg-4"></div>
                    <div class="col-xs-12 col-md-8 col-lg-8">
                        <button type="button" class="btn btn-block btn-primary" id="login">
                            เข้าใช้งานระบบ
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="/resources_constant/js/jquery.3.3.1/jquery-3.3.1.js"></script>
    <script src="/resources_constant/js/bootstrap/bootstrap.min.js"></script>
    <script src="/resources_constant/js/alertify/alertify.js"></script>
    <script src="/resource/js/extension.js"></script>
    <script src="/resource/js/default.js"></script>
</body>
</html>
