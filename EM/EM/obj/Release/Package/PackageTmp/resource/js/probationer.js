var param = getparameters();
function probationer() {
    set_navigation($(".btnback").attr("href"));
    Dropdown(".address");
    select_title(".title");
    phone("#probationer_telephone");
    telephone("#probationer_mobile");
    homeno("#probationer_address_number");
    select_IMEI("#probationer_EM_devices");
    text("#probationer_road, #probationer_lane");
    select_department("#probationer_department");

    $(".number").NSNumberbox();
    $(".cardid").NSNumberboxInteger();

    $(".gender").select2({
        width: "100%",
        theme: "bootstrap",
        minimumResultsForSearch: -1
    });

    $('#probationer').bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            probationer_IMEI_start_date: {
                validators: {
                    date: {
                        format: 'dd/mm/yyyy',
                    }
                }
            }
        }
    });

    $("#probationer_IMEI_start_date, #probationer_IMEI_end_date, #probationer_judge_end_date").datepicker({
        format: 'dd/mm/yyyy',
        todayBtn: true,
        language: 'th',
        thaiyear: true,
    }).datepicker("", new Date());

    $("#probationer_IMEI_start_date ,#probationer_IMEI_end_date, #probationer_judge_end_date").mask("AA/AA/AAAA", {
        "translation": { 0: { pattern: /[0-9]/g } }, onKeyPress: function (value, event) {
            event.currentTarget.value = value.toUpperCase();
        }
    });

    $.ajax({
        url: "/control/probationer.aspx",
        data: {
            action: "getdata",
        },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (!data.onError) {
                data.getItems = jQuery.parseJSON(data.getItems);

                $.each((data.getItems.offence), function (index, e) {
                    let item = $("#checkbox-template > div").clone();
                    item.find('input[type="checkbox"]').val(e.offence_id).addClass("checkboxoffence");
                    item.find("#checkboxorder").html(e.offence_name).removeAttr("id");
                    item.find(".sub_offence").attr("data_id", e.offence_id)
                    $("#list_offense").append($(item));

                    var offence = (data.getItems.sub_offence).filter(function (element) {
                        return element.offence_id == e.offence_id;
                    });

                    $.each((offence), function (row, data) {
                        let itemsub = $("#checkbox-template > div > .offence").clone();
                        itemsub.removeClass("col-md-12 offence").addClass("col-md-6 clear-pleft");
                        itemsub.find('input[type="checkbox"]').attr("value", data.suboffence_offid);
                        itemsub.find("#checkboxorder").html(data.suboffence_name).removeAttr("id");
                        item.find(".sub_offence").append($(itemsub));
                    });
                });

                $(".checkboxoffence").change(function () {
                    if ($(this).is(":checked")) {
                        $("body").find(".sub_offence[data_id='" + $(this).val() + "'] ").removeClass("hidden");
                    }
                    else {
                        $("body").find(".sub_offence[data_id='" + $(this).val() + "'] ").addClass("hidden");
                    }
                });

                $.each((data.getItems.condition), function (index, e) {
                    let item = $("div > .condition-template > div").clone();
                    item.find('input[type="checkbox"]').val(e.criminal_id).addClass("checkboxoffence");
                    item.find("#condition_name").html(e.criminal_name).removeAttr("id");
                    item.find("input:text").NSDisable();
                    $("#condition").append($(item));
                });

                $(".rowchoose").on("click", ":checkbox", function () {
                    if ($(this).is(":checked")) {
                        $(this).parents(".rowchoose").find(":text").NSEnable();
                    } else {
                        $(this).parents(".rowchoose").find(":text").NSDisable();
                    }
                });

                $(".start_time").timepicker({
                    zindex: 9999999,
                    timeFormat: 'HH:mm',
                    interval: 15,
                    minTime: '00:00',
                    maxTime: '23:45',
                    startTime: '00:00',
                    dynamic: false,
                    dropdown: true,
                    scrollbar: true
                });

                $(".end_time").timepicker({
                    zindex: 9999999,
                    timeFormat: 'HH:mm',
                    interval: 15,
                    minTime: '01:00',
                    maxTime: '23:59',
                    startTime: '01:00',
                    dynamic: false,
                    dropdown: true,
                    scrollbar: true,
                    _lasttime: '23:59'
                });
            }
        }
    });
    
    $("button.permission").click(function () {
        $("div.modal-body").find("input[type=checkbox]:checked").prop("checked", false);
        $("div.modal-body").find("input[type=text]").NSValue("");
        $("#chkSelectGroup, #chkSelectOfficer").NSEnable();
        $("#myModal").modal();

        $.ajax({
            url: "/control/user.aspx",
            data: {
                action: "get_account",
            },
            type: "POST",
            dataType: "json",
            success: function (data) {
                if (!data.onError) {
                    data.getItems = jQuery.parseJSON(data.getItems);
                    $.each((data.getItems), function (index, e) {
                        let item = $("#testchk > div ").clone();
                        $(item).find('input').val(e.id);
                        $(item).find("label").text(e.name_th);
                        $(item).find(".sub_offence").attr("account_id", e.id)
                        $("#chkinmodal").append($(item));
                    });
                }
            }
        });

        $("#chkSelectOfficer").click(function () {
            if ($("#chkSelectOfficer").is(":checked") == true) {
                $.ajax({
                    url: "/control/user.aspx",
                    data: {
                        action: "GetAccount",
                    },
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        if (!data.onError) {
                            $("#chkinmodal").empty();
                            data.getItems = jQuery.parseJSON(data.getItems);
                            list_staffname = data.getItems;
                            $.each((data.getItems), function (index, e) {
                                let item = $("#testchk3 > div ").clone();
                                $(item).find('input').val(e.account_id);
                                $(item).find("label").text(e.account_name);
                                $(item).find(".sub_offence3").attr("account_id", e.account_id)
                                $("#chkinmodal").append($(item)).show();
                                $("#chkSelectGroup").NSDisable();
                            });
                        }
                    }
                });
            } else {
                $("#chkinmodal").hide();
                $("#chkSelectGroup").NSEnable();
            }
        });

        $("#chkSelectGroup").click(function () {
            if ($("#chkSelectGroup").is(":checked") == true) {
                $.ajax({
                    url: "/control/user.aspx",
                    data: {
                        action: "GetGroup",
                    },
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        if (!data.onError) {
                            $("#chkinmodal").empty();

                            data.getItems = jQuery.parseJSON(data.getItems);
                            list_staffname = data.getItems;
                            $.each((data.getItems), function (index, e) {
                                let item = $("#testchk > div ").clone();
                                $(item).find('input').val(e.group_id);
                                $(item).find("label").text(e.group_name_th);
                                $(item).find(".sub_offence3").attr("group_id", e.group_id)
                                $("#chkinmodal").append($(item)).show();

                                $("#chkSelectOfficer").NSDisable();
                            });
                        }
                    }
                });
            } else {
                $("#chkinmodal").hide();
                $("#chkSelectOfficer").NSEnable();
            }
        });
    });

    $("button.notification").click(function () {
        $("div.modal-body").find("input[type=checkbox]:checked").prop("checked", false);
        $("div.modal-body").find("input[type=text]").NSValue("");
        $("#chkSelectGroup2").NSEnable();
        $("#chkSelectOfficer2").NSEnable();
        $("#myModal2").modal();

        $("#chkSelectOfficer2").click(function () {
            if ($("#chkSelectOfficer2").is(":checked") == true) {
                $.ajax({
                    url: "/control/user.aspx",
                    data: {
                        action: "GetAccount",
                    },
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        if (!data.onError) {
                            $("#chkinmodal2").empty();

                            data.getItems = jQuery.parseJSON(data.getItems);
                            list_staffname = data.getItems;
                            $.each((data.getItems), function (index, e) {
                                //if
                                let item = $("#testchk2 > div ").clone();
                                $(item).find('input').val(e.account_id);
                                $(item).find("label").text(e.account_name);
                                $(item).find(".sub_offence2").attr("account_id", e.account_id)
                                $("#chkinmodal2").append($(item)).show();

                                $("#chkSelectGroup2").NSDisable();
                            });
                        }
                    }
                });
            } else {
                $("#chkinmodal2").hide();
                $("#chkSelectGroup2").NSEnable();
            }
        });

        $("#chkSelectGroup2").click(function () {
            if ($("#chkSelectGroup2").is(":checked") == true) {    // เรียกกลุ่มใน Modal2
                $.ajax({
                    url: "/control/user.aspx",
                    data: {
                        action: "GetGroup",
                    },
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        if (!data.onError) {
                            $("#chkinmodal").empty();

                            data.getItems = jQuery.parseJSON(data.getItems);
                            list_staffname = data.getItems;
                            $.each((data.getItems), function (index, e) {
                                let item = $("#testchk2 > div ").clone();
                                $(item).find('input').val(e.group_id);
                                $(item).find("label").text(e.group_name_th);
                                $(item).find(".sub_offence2").attr("group_id", e.group_id)
                                $("#chkinmodal2").append($(item)).show();
                                $("#chkSelectOfficer2").NSDisable();
                            });
                        }
                    }
                });
            } else {
                $("#chkinmodal2").hide();
                $("#chkSelectOfficer2").NSEnable();
            }
        });
    });

    $("#btnclose").click(function () {
        let range = parseInt($("#rangeCount").NSValue());
        $("div.offence3  input:checkbox:checked").each(function (i, e) {
            range = range + 1;
            let item = $("#row_show > div.col-md-12").clone();
            $(item).attr("id", "Div" + range);
            $(item).find("button").attr("id", range);
            $(item).find("#acc").text($(this).next().text());
            $(item).find("#role").text(e.role_id).text(e.role_name_th).next().text();
            $(item).find("#dep").text(e.department_id).text(e.department_name_th).next().text();
            $("#row_choose").append($(item));
        });
        $("#rangeCount").NSValue(range);
        $(".remove").click(function () {
            $("#Div" + $(this).attr("id")).remove();
        });
    });

    $("#btnclose2").click(function () {
        let range2 = parseInt($("#rangeCount2").NSValue());
        $("div.offence2 input:checkbox:checked").each(function () {
            range2 = range2 + 1;
            let item2 = $("#row_show2 > div.col-md-12").clone();
            $(item2).attr("id", "Divweb" + range2);
            $(item2).find("button").attr("id", range2);
            $(item2).find("#acc2").text($(this).next().text());
            $(item2).find("#role2").text($(this).next().text());
            $(item2).find("#dep2").text($(this).next().text());
            $("#row_choose2").append($(item2));
        });
        $("#rangeCount2").NSValue(range2);
        $(".remove").click(function () {
            $("#Divweb" + $(this).attr("id")).remove();
        });
    });

    $(".btnsave").click(function () {
        var result = CheckData();
        if (result[0]) {
            $.ajax({
                url: "../control/probationer.aspx",
                data: {
                    action: "save_probationer",
                    probationer: JSON.stringify(result[1])
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
                            alertify.alert(data.getItems.txtAlert, function (closeEvent) {
                                window.location.href = $('.btnback').attr('href');
                            });
                        } else {
                            alertify.alert(data.getItems.txtAlert);
                        }
                    } else {
                        alertify.alert("ไม่สามารถเพิ่มข้อมูลผู้ถุกควบคุมความประพฤติได้");
                    }
                    $.busyLoadFull("hide");
                }
            });
        }
    });

    if (param.data != null) { set_data(); }
}

function pbt_time_start(id, append) {
    $(id).timepicker({
        zindex: 9999999,
        timeFormat: 'HH:mm',
        interval: 15,
        minTime: '00:00',
        maxTime: '23:45',
        startTime: '00:00',
        dynamic: false,
        dropdown: true,
        scrollbar: true,
        append_to: append
    });
}

function pbt_time_end(id, append) {
    $(id).timepicker({
        zindex: 9999999,
        timeFormat: 'HH:mm',
        interval: 15,
        minTime: '01:00',
        maxTime: '23:59',
        startTime: '01:00',
        dynamic: false,
        dropdown: true,
        scrollbar: true,
        append_to: append,
        _lasttime: '23:59'
    });
}

function set_data() {
    $("#probationer_status").bootstrapSwitch("size", "small");
    $("#probationer_status").bootstrapSwitch("state", true);
    $("#subject").html("แก้ไขข้อมูลผู้ถูกควบคุมความประพฤติ");
    $.ajax({
        url: "../control/probationer.aspx",
        data: {
            action: "probationer",
            probationer_id: param.data,
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
                    let itemid;
                    item = jQuery.parseJSON(data.getItems);
                    $.each((item), function (index, e) {
                        itemid = "#" + index;
                        if ($(itemid).prop("type") == "text" && !($(itemid).hasClass("calendar"))) { $(itemid).NSValue(e); }
                    });

                    $("#probationer_title").NSValue(item.probationer_title);
                    $("#probationer_gender").NSValue(item.probationer_gender);
                    $("#probationer_province").NSValue(item.probationer_province);
                    RenderDistrict(item.probationer_province, "#probationer_district");
                    $("#probationer_district").NSValue(item.probationer_district).trigger("change");
                    RenderSubDistrict(item.probationer_district, "#probationer_subdistrict");
                    $("#probationer_subdistrict").NSValue(item.probationer_subdistrict).trigger("change");
                    $("#probationer_IMEI_start_date").datepicker("setDate", item.probationer_IMEI_start_date);
                    $("#probationer_judge_end_date").datepicker("setDate", item.probationer_judge_end_date);
                    $("#probationer_IMEI_end_date").datepicker("setDate", item.probationer_IMEI_end_date);
                    if (item.probationer_status == "Active") { $("#probationer_status").bootstrapSwitch("state", true); }
                    else { $("#probationer_status").bootstrapSwitch("state", false); }
                    $("#probationer_status").removeClass("hidden");
                }
                $.busyLoadFull("hide");
            }, 2000);
        }
    });
}

function select_IMEI(imei) {
    $(imei).select2({
        placeholder: "หมายเลข IMEI",
        width: "100%",
        theme: "bootstrap"
    });

    $.ajax({
        url: "../control/device.aspx",
        data: {
            action: "get_device",
        },
        type: "POST",
        dataType: "json",
        error: function () {
            alertify.alert("error display data.");
        },
        success: function (data) {
            if (!data.onError) {
                data.getItems = jQuery.parseJSON(data.getItems);
                $.each((data.getItems), function (index, e) {
                    let item = $("#select-template > select > option").clone();
                    $(item).attr("value", e.device_id).text(e.device_IMEI);
                    $(imei).append($(item));
                });
            }
        }
    });
}