function Dropdown(prmt) {
    var province; var district; var subdistrict;
    $.ajax({
        url: "/control/location.aspx",
        data: {
            action: "list_address",
        },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (!data.onError) {
                data.getItems = jQuery.parseJSON(data.getItems);
                $("#add").data("addressList", data.getItems);
            }
        }
    });

    $(prmt).each(function () {
        var dropdown = $(this).find("select");
        if (dropdown.length == 1) { province = "#" + dropdown[0].id; }
        else if (dropdown.length == 2) { province = "#" + dropdown[0].id; district = "#" + dropdown[1].id; }
        else if (dropdown.length == 3) { province = "#" + dropdown[0].id; district = "#" + dropdown[1].id; subdistrict = "#" + dropdown[2].id; }
        RenderDropdown(province, district, subdistrict);
    });
}

function RenderDropdown(province, district, subdistrict) {
    setTimeout(function () {
        RenderProvince(province);
        $(province).on("select2:selecting", function (e) {
            if (district != undefined) {
                $(district).html("<option selected></option>");
                RenderDistrict(e.params.args.data.id, district);
            }

            if (subdistrict != undefined) {
                $(subdistrict).html("<option selected></option>");
            }
        });

        if (subdistrict != undefined) {
            $(district).on("select2:selecting", function (e) {
                $(subdistrict).html("<option selected></option>");
                RenderSubDistrict(e.params.args.data.id, subdistrict);
            });
        }
    }, 2000);
}

function RenderProvince(select_item) {
    let p = $("#add").data("addressList").province;
    $.each((p), function (index, e) {
        let item = $("#select-template > select > option").clone().show();
        $(item).attr("value", e.data_id).text(e.data_name);
        $(select_item).append($(item));
    });
}

function RenderDistrict(did, select_item) {
    let district;
    if (did == null) {
        district = $("#add").data("addressList").district;
    } else {
        district = $("#add").data("addressList").district.filter(function (item) {
            return item.data_key == did;
        });
    }

    $.each((district), function (index, e) {
        let item = $("#select-template > select > option").clone().show();
        $(item).attr("value", e.data_id).text(e.data_name);
        $(select_item).append($(item));
    });
}

function RenderSubDistrict(subid, select_item) {
    let subdistrict;
    if (subid == null) {
        subdistrict = $("#add").data("addressList").subdistrict;
    } else {
        subdistrict = $("#add").data("addressList").subdistrict.filter(function (item) {
            return item.data_key == subid;
        });
    }

    $.each((subdistrict), function (index, e) {
        let item = $("#select-template > select > option").clone().show();
        $(item).attr("value", e.data_id).text(e.data_name);
        $(select_item).append($(item));
    });
}