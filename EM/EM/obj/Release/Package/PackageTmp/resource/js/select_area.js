function select_area(groupArea) {
    $(groupArea).select2({
        width: "100%",
        theme: "bootstrap"
    });

    $.ajax({
        url: "../control/phohibited.aspx",
        data: {
            action: "get_group",
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
                    $(item).attr("value", e.group_id).text(e.group_name);
                    $("#groupArea").append($(item));
                });
            }
        }
    });
}