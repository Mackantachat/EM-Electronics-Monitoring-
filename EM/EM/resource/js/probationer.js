var param = getparameters();
function probationer() {
    set_navigation($(".btnback").attr("href"));
    Dropdown(".address");
    select_title(".title");
    phone("#probationer_telephone");
    telephone("#probationer_mobile");
    homeno(".homeno");
    select_IMEI("#probationer_EM_devices");
    text("#probationer_road, #probationer_lane");
    select_Education("#probationer_education");

    validemail("#notification_by_email");
    select_department("#probationer_department");

    $(".number").NSNumberbox();
    $(".cardid").NSNumberboxInteger();

    $(".gender").select2({
        width: "100%",
        theme: "bootstrap",
        minimumResultsForSearch: -1
    });

    $(".province, .district, .subdistrict").select2({
        width: "100%",
        theme: "bootstrap"
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
            action: "get_data",
        },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (!data.onError) {
                data.getItems = jQuery.parseJSON(data.getItems);

                $.each((data.getItems.offence), function (index, e) {
                    let item = $("#checkbox-template > div").clone();
                    item.find('input[type="checkbox"]').attr("offence_id", e.offence_id).addClass("checkboxoffence");
                    item.find("#checkboxorder").html(e.offence_name).removeAttr("id");
                    item.find(".sub_offence").attr("data_id", e.offence_id)
                    $("#list_offense").append($(item));

                    var offence = (data.getItems.sub_offence).filter(function (element) {
                        return element.offence_id == e.offence_id;
                    });
                    $.each((offence), function (row, data) {
                        let itemsub = $("#checkbox-template > div > .offence").clone();
                        itemsub.removeClass("col-md-12 offence").addClass("col-md-6 clear-pleft");
                        itemsub.find('input[type="checkbox"]').attr("value", data.suboffence_id);
                        itemsub.find("#checkboxorder").html(data.suboffence_name).removeAttr("id");
                        item.find(".sub_offence").append($(itemsub));

                        //console.log(" data.suboffence_id", data.suboffence_id);

                    });

                });

                $(".checkboxoffence").change(function () {
                    if ($(this).is(":checked")) {
                        $("body").find(".sub_offence[data_id='" + $(this).attr("offence_id") + "'] ").removeClass("hidden");
                        //console.log("this offence", $(".checkboxoffence").is(":checked"));
                    }
                    else {
                        $("body").find(".sub_offence[data_id='" + $(this).attr("offence_id") + "'] ").addClass("hidden");
                        $(".sub_offence[data_id='" + $(this).attr("offence_id") + "']").find("input[type=checkbox]:checked").prop("checked", false);
                    }
                });

                $.each((data.getItems.condition), function (index, e) {
                    let item = $("div > .condition-template > div").clone();
                    item.find('input[type="checkbox"]').attr("criminal_id", e.criminal_id).addClass("checkboxcondition");
                    item.find("#condition_name").html(e.criminal_name).removeAttr("id");
                    item.find("input:text").NSDisable();
                    $("#condition").append($(item));

                });

                $(".rowchoose").on("click", ":checkbox", function () {
                    if ($(this).is(":checked")) {
                        $(this).parents(".rowchoose").find(":text").NSEnable();
                    } else {
                        $(this).parents(".rowchoose").find(":text").NSDisable().NSValue("");
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

            var refproba = [];

            $('fieldset.scheduler-border').each(function (j, e) {
                refproba[j] = {
                    reference_title: $(e).find("select[name=reference_title]").NSValue(),
                    reference_name: $(e).find("input[name=reference_name]").NSValue(),
                    reference_last_name: $(e).find("input[name=reference_last_name]").NSValue(),
                    reference_cardid: $(e).find("input[name=reference_cardid]").NSValue(),
                    reference_mobile: $(e).find("input[name=reference_mobile]").NSValue(),
                    reference_gender: $(e).find("select[name=reference_gender]").NSValue(),
                    reference_age: $(e).find("input[name=reference_age]").NSValue(),
                    reference_address_number: $(e).find("input[name=reference_address_number]").NSValue(),
                    reference_moo: $(e).find("input[name=reference_moo]").NSValue(),
                    reference_road: $(e).find("input[name=reference_road]").NSValue(),
                    reference_lane: $(e).find("input[name=reference_lane]").NSValue(),
                    reference_subdistrict: $(e).find("select[name=reference_subdistrict]").NSValue(),
                    reference_district: $(e).find("select[name=reference_district]").NSValue(),
                    reference_province: $(e).find("select[name=reference_province]").NSValue(),
                    //RefProba_add_postcode: $(e).find("input[name=reference_postcode]").NSValue(),
                    reference_current_address_number: $(e).find("input[name=reference_current_address_number]").NSValue(),
                    reference_current_moo: $(e).find("input[name=reference_current_moo]").NSValue(),
                    reference_current_road: $(e).find("input[name=reference_current_road]").NSValue(),
                    reference_current_lane: $(e).find("input[name=reference_current_lane]").NSValue(),
                    reference_current_subdistrict: $(e).find("select[name=reference_current_subdistrict]").NSValue(),
                    reference_current_district: $(e).find("select[name=reference_current_district]").NSValue(),
                    reference_current_province: $(e).find("select[name=reference_current_province]").NSValue()
                    //RefProba_current_postcode: $(e).find("input[name=reference_last_name]").NSValue(),
                };
                //console.log(refproba[j]);
            });

            //var offandsub = [];
            //var count = 0;
            //$(".offence").each(function (index, e) {
            //    if ($(e).find('input[type="checkbox"]').prop('checked')) {

            //        //offandsub[count] = {
            //        //    offence_id: $(e).find('input[type="checkbox"]:checked').attr('offence_id'),

            //        //};

            //        count++
            //    }

            var offandsub = [];
            var count = 0;
            $(".offence:visible").each(function (index, e) {
                if ($(e).find('input[type="checkbox"]').prop('checked')) {
                    var oid = $(e).find('input[type="checkbox"]:checked').attr('offence_id');
                    
                    $(".sub_offence[data_id='" + oid + "']:visible input[type='checkbox']:checked").each(function (i, v) {
                            offandsub[count] = {
                                offence_id: oid,
                                suboffence_id: $(v).attr('value')
                            };

                           count++
                        

                    });

                    
                }
            });

            var criminal = [];
            var row = 0;
            $("#condition .rowchoose").each(function (index, e) {
                if (!($(e).find("input.text_condition:visible").prop('disabled'))) {
                    criminal[row] = {
                        criminal_id: $(e).find("input[type='checkbox'][class='checkboxcondition']").attr("criminal_id"),
                        criminal_text: $(e).find("input.text_condition:visible").NSValue(),
                        start_time: $(e).find("input.start_time:visible").NSValue(),
                        end_time: $(e).find("input.end_time:visible").NSValue()
                    };
                    row++;
                }
                //console.log("criminal[row]", criminal[row]);
            });

            $.ajax({
                url: "../control/probationer.aspx",
                data: {
                    action: "save_probationer",
                    probationer: JSON.stringify(result[1]),
                    refprobaItem: JSON.stringify(refproba),
                    offenceItem: JSON.stringify(offandsub),
                    criminalItem: JSON.stringify(criminal),
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
        //console.log("offenceItem",offenceItem);

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
                    data.getItems = jQuery.parseJSON(data.getItems);
                    item = data.getItems.Probationer;
                    $.each((item), function (index, e) {
                        itemid = "#" + index;
                        if ($(itemid).prop("type") == "text" && !($(itemid).hasClass("calendar"))) { $(itemid).NSValue(e); }
                        //console.log("item", item);
                    });

                    criminalItem = data.getItems.probationerCriminal;
                    criminalItem.criminal_id
                    //console.log("criminalItem", criminalItem);
                    $.each((criminalItem), function (index, value) {

                        itemid = $('input[class="checkboxcondition"][criminal_id=' + value.criminal_id + ']');
                        itemid.prop("checked", true);
                        itemid = itemid.closest('div.rowchoose');
                        itemid.find(".text_condition").NSValue(value.criminal_text).prop("disabled", false);
                        itemid.find(".start_time").NSValue(value.start_time).prop("disabled", false);
                        itemid.find(".end_time").NSValue(value.end_time).prop("disabled", false);

                        //console.log("criminalItem[index]", value.criminal_id);
                    });

                    probaoffenceitem = data.getItems.ProbationerOffence;

                    $.each((probaoffenceitem), function (index, value) {
                        let checkboxoffence = $("input[type='checkbox'][offence_id=" + value.offence_id + "]");
                        if (!(checkboxoffence.is(":checked"))) {
                            checkboxoffence.prop("checked", true);
                            $("body").find(".sub_offence[data_id='" + value.offence_id + "'] ").removeClass("hidden");
                        }

                        $("input[type='checkbox'][value=" + value.suboffence_id + "]").prop("checked", true);
                        //console.log("offence_id[value]", value.offence_id);
                    });

                    let refid1;
                    refitem1 = data.getItems.ProbationerReference[0];

                    $.each((refitem1), function (index1, value) {
                        refid1 = "#" + index1 + 1;
                        if ($(refid1).prop("type") == "text" && !($(refid1).hasClass("calendar"))) { $(refid1).NSValue(value); }
                        //console.log("refitem1", refitem1);
                    });

                    let refid2;
                    refitem2 = data.getItems.ProbationerReference[1];

                    $.each((refitem2), function (index2, value) {
                        refid2 = "#" + index2 + 2;
                        if ($(refid2).prop("type") == "text" && !($(refid2).hasClass("calendar"))) { $(refid2).NSValue(value); }
                        //console.log("refitem2", refitem2);
                    });

                    $("#pid").NSValue(param.data);
                    $("#probationer_remark").NSValue(item.probationer_remark);
                    $("#probationer_other").NSValue(item.probationer_other);
                    $("#probationer_title").NSValue(item.probationer_title);
                    $("#probationer_department").NSValue(item.probationer_department);
                    $("#probationer_EM_devices").NSValue(item.probationer_EM_devices);
                    $("#probationer_gender").NSValue(item.probationer_gender);
                    $("#probationer_education").NSValue(item.probationer_education);
                    $("#probationer_province").NSValue(item.probationer_province);
                    RenderDistrict(item.probationer_province, "#probationer_district");
                    $("#probationer_district").NSValue(item.probationer_district).trigger("change");
                    RenderSubDistrict(item.probationer_district, "#probationer_subdistrict");
                    $("#probationer_subdistrict").NSValue(item.probationer_subdistrict).trigger("change");
                    $("#probationer_IMEI_start_date").datepicker("setDate", item.probationer_IMEI_start_date);
                    $("#probationer_judge_end_date").datepicker("setDate", item.probationer_judge_end_date);
                    $("#probationer_IMEI_end_date").datepicker("setDate", item.probationer_IMEI_end_date);
                    $("#current_province").NSValue(item.current_province);
                    RenderDistrict(item.current_province, "#current_district");
                    $("#current_district").NSValue(item.current_district).trigger("change");
                    RenderSubDistrict(item.current_district, "#current_subdistrict");
                    $("#current_subdistrict").NSValue(item.current_subdistrict).trigger("change");
                    $("#reference_province1").NSValue(refitem1.reference_province);
                    RenderDistrict(refitem1.reference_province, "#reference_district1");
                    $("#reference_district1").NSValue(refitem1.reference_district).trigger("change");
                    RenderSubDistrict(refitem1.reference_district, "#reference_subdistrict1");
                    $("#reference_subdistrict1").NSValue(refitem1.reference_subdistrict).trigger("change");
                    $("#reference_current_province1").NSValue(refitem1.reference_current_province);
                    RenderDistrict(refitem1.reference_current_province, "#reference_current_district1");
                    $("#reference_current_district1").NSValue(refitem1.reference_current_district).trigger("change");
                    RenderSubDistrict(refitem1.reference_current_district, "#reference_current_subdistrict1");
                    $("#reference_current_subdistrict1").NSValue(refitem1.reference_current_subdistrict).trigger("change");
                    $("#reference_province2").NSValue(refitem2.reference_province);
                    RenderDistrict(refitem2.reference_province, "#reference_district2");
                    $("#reference_district2").NSValue(refitem2.reference_district).trigger("change");
                    RenderSubDistrict(refitem2.reference_district, "#reference_subdistrict2");
                    $("#reference_subdistrict2").NSValue(refitem2.reference_subdistrict).trigger("change");
                    $("#reference_current_province2").NSValue(refitem2.reference_current_province);
                    RenderDistrict(refitem2.reference_current_province, "#reference_current_district2");
                    $("#reference_current_district2").NSValue(refitem2.reference_current_district).trigger("change");
                    RenderSubDistrict(refitem2.reference_current_district, "#reference_current_subdistrict2");
                    $("#reference_current_subdistrict2").NSValue(refitem2.reference_current_subdistrict).trigger("change");
                    $("#reference_title1").NSValue(refitem1.reference_title);
                    $("#reference_gender1").NSValue(refitem1.reference_gender);
                    $("#reference_title2").NSValue(refitem2.reference_title);
                    $("#reference_gender2").NSValue(refitem2.reference_gender);

                    if (item.probationer_activate_homebase == "Y") {
                        $('#probationer_activate_homebase').prop("checked", true);
                    };

                    if (item.probationer_status == "Active") { $("#probationer_status").bootstrapSwitch("state", true); }
                    else { $("#probationer_status").bootstrapSwitch("state", false); }
                    $("#probationer_status").removeClass("hidden");
                }
                $.busyLoadFull("hide");
            }, 3000);
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

function select_Education(probationer_education) {
    $(probationer_education).select2({
        placeholder: "การศึกษาสูงสุด",
        width: "100%",
        theme: "bootstrap"
    });

    $.ajax({
        url: "../control/probationer.aspx",
        data: {
            action: "list_Education",
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
                    $(item).attr("value", e.Education_id).text(e.Education_name_th);
                    $(probationer_education).append($(item));
                });
            }
        }
    });
}



