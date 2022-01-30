function select_group(group) {
    $(group).select2({
        placeholder: "กลุ่มผู้ใช้งาน",
        width: "100%",
        theme: "bootstrap"
    });

    $.ajax({
        url: "../control/user.aspx",
        data: { action: "get_group" },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (!data.onError) {
                data.getItems = jQuery.parseJSON(data.getItems);
                $.each((data.getItems), function (index, e) {
                    let item = $("#select-template > select > option").clone();
                    $(item).attr("value",e.id).text(e.name);
                    $(group).append($(item));
                });
            }
        }
    });
}