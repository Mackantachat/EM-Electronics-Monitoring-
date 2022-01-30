sessionStorage.clear();
$("#login").on('click', function () {
    if ($("#username").val().length == 0) {
        $("#username").focus();
        alertify.alert("โปรดระบุบัญชีผู้ใช้");
        return false;

    } else if ($("#password").val().length == 0) {
        $("#password").focus();
        alertify.alert("โปรดระบุรหัสผ่าน");
        return false;

    } else {
        CheckLogin();
    }
});

function CheckLogin() {
    $.ajax({
        url: "control/login.aspx",
        data: {
            action: "login",
            username: $("#username").NSValue(),
            password: $("#password").NSValue()
        },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (!data.onError) {
                data.getItems = jQuery.parseJSON(data.getItems);
                if (data.getItems.status) {
                    sessionStorage.setItem("staff_id", (btoa(data.getItems.id)));
                    sessionStorage.setItem("name", data.getItems.name);
                    window.open("home/allhome", "_self");
                } else {
                    alertify.alert(data.getItems.text_alert)
                    $("#code").NSValue("").focus();
                    $("#username").NSValue("");
                    $("#password").NSValue("");
                }
            }
        }
    });
}



