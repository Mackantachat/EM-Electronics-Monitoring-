function transformToAssocArray(prmstr) {
    var params = {};
    var prmarr = prmstr.split("&");
    for (var i = 0; i < prmarr.length; i++) {
        var tmparr = prmarr[i].split("=");
        params[tmparr[0]] = tmparr[1];
    }
    return params;
}

function getparameters() {
    var prmstr = window.location.search.substr(1);
    return prmstr != null && prmstr != "" ? transformToAssocArray(prmstr) : {};
}

function selectdate(selectYear, selectMonth, selectDay) {
    var qntYears = 60;
    var currentYearTH = new Date().getFullYear() + 543;

    for (var y = 0; y < qntYears; y++) {
        var yearElem = document.createElement("option");
        yearElem.value = currentYearTH
        yearElem.textContent = currentYearTH;
        selectYear.append(yearElem);
        currentYearTH--;
    }

    for (var m = 0; m < 12; m++) {
        let monthNum = new Date(2018, m).getMonth()
        let month = monthNamesFull[monthNum];
        var monthElem = document.createElement("option");
        monthElem.value = monthNum + 1;
        monthElem.textContent = month;
        selectMonth.append(monthElem);
    }

    for (var d = 1; d <= 31; d++) {
        var dayElem = document.createElement("option");
        dayElem.value = d;
        dayElem.textContent = d;
        selectDay.append(dayElem);
    }
}

function CheckData() {
    var alert = ""; var status = true;
    $("form .required").each(function () {
        if ($(this).hasClass == "cardid" && $(this).NSValue().length != 13) {
            status = false;
            alert = "รหัสประจำตัวประชาชนไม่ถูกต้อง กรุณาตรวจสอบ";
        } else {
            if ($.trim($(this).NSValue()) == "") {
                status = false;
                if ($(this).is("input")) {
                    alert = "กรุณากรอก " + $(this).attr("placeholder");
                } else {
                    alert = "กรุณาเลือก " + $(this).data("placeholder");
                }
            }
        }

        if (!status) {
            let input = $(this);
            if ($(this).is("input")) {
                $(input).NSFocus();
                alertify.alert(alert);
            } else {
                alertify.alert(alert, function () {
                    $(input).select2("open");
                });
            }
            return false;
        }
    });

    var data = {};
    if (status) {
        $("form input, form select").each(function () {
            if (this.name == "status") {
                data[this.id] = this.checked == true ? "Active" : "Inactive"
            } else {
                data[this.id] = $(this).val() || undefined;
            }
        });
        return [status, data];
    } else {
        return [status, ""];
    }
}

function number(txtid) {
    $(txtid).on("keypress", function (event) {
        var englishAlphabetAndWhiteSpace = /^[0-9\-]/g;
        var key = String.fromCharCode(event.which);
        if (event.keyCode == 8 || event.keyCode == 53 || event.keyCode == 222 || englishAlphabetAndWhiteSpace.test(key)) {
            return true;
        }
        return false;
    });
}

function character(txtid) {
    $(txtid).on("keypress", function (event) {
        var SpacialCharacter = /^[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/฿\s]/gi;
        var keyChar = String.fromCharCode(event.keyCode);
        var output = SpacialCharacter.test(keyChar);
        var text = $(this).val();
        $(this).val(text.replace(SpacialCharacter, ''));
        return !output;
    });
}

function usepass(txtid) {
    $(txtid).on("keypress", function (event) {
        var englishAlphabetAndWhiteSpace = /^[A-Za-z0-9]/g;
        var key = String.fromCharCode(event.which);
        if (event.keyCode == 8 || event.keyCode == 53 || event.keyCode == 222 || englishAlphabetAndWhiteSpace.test(key)) {
            return true;
        }
        return false;
    });
}

function text(txtid) {
    $(txtid).on("keypress", function (event) {
        var englishAlphabetAndWhiteSpace = /^[\u0E01-\u0E3A\u0E40-\u0E4DA-Za-z0-9/\(\)\-\s]/g;
        var key = String.fromCharCode(event.which);
        if (event.keyCode == 8 || event.keyCode == 53 || event.keyCode == 222 || englishAlphabetAndWhiteSpace.test(key)) {
            return true;
        }
        return false;
    });
}

function phone(txtid) {
    $(txtid).on("keypress", function (event) {
        var englishAlphabetAndWhiteSpace = /^[\u0E01-\u0E3A\u0E40-\u0E4DA-Za-z0-9\-\,\s]+/g;
        var key = String.fromCharCode(event.which);
        if (event.keyCode == 8 || event.keyCode == 53 || event.keyCode == 222 || englishAlphabetAndWhiteSpace.test(key)) {
            return true;
        }
        return false;
    });
}

function telephone(txtid) {
    $(txtid).on("keypress", function (event) {
        var englishAlphabetAndWhiteSpace = /^[0-9\-\,\s]+/g;
        var key = String.fromCharCode(event.which);
        if (event.keyCode == 8 || event.keyCode == 53 || event.keyCode == 222 || englishAlphabetAndWhiteSpace.test(key)) {
            return true;
        }
        return false;
    });
}

function txt(txtid) {
    $(txtid).on("keypress", function (event) {
        var englishAlphabetAndWhiteSpace = /^[\u0E01-\u0E3A\u0E40-\u0E4D0A-Za-z0-9/\-\.\,()\s]+/g;
        var key = String.fromCharCode(event.which);
        if (event.keyCode == 8 || event.keyCode == 53 || event.keyCode == 222 || englishAlphabetAndWhiteSpace.test(key)) {
            return true;
        }
        return false;
    });
}

function homeno(txtid) {
    $(txtid).on("keypress", function (event) {
        var englishAlphabetAndWhiteSpace = /^[0-9\/\-]+/g;
        var key = String.fromCharCode(event.which);
        if (event.keyCode == 8 || event.keyCode == 53 || event.keyCode == 222 || englishAlphabetAndWhiteSpace.test(key)) {
            return true;
        }
        return false;
    });
}

function validemail(email) {
    $(email).on("keypress", function (event) {
        var SpacialCharacter = /[`~!#$%^&*()|+\=?;:'",<>\{\}\[\]\\\/฿]/gi;
        var keyChar = String.fromCharCode(event.keyCode);
        var output = SpacialCharacter.test(keyChar);
        var text = $(this).val();
        $(this).val(text.replace(SpacialCharacter, ''));
        return !output;
    });
}


