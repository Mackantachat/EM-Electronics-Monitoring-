function select_role(role) {
    $(role).select2({
        placeholder: "ระดับสิทธิ",
        width: "100%",
        theme: "bootstrap"
    });

    $.ajax({
        url: "../control/user.aspx",
        data: { action: "get_role" },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (!data.onError) {
                data.getItems = jQuery.parseJSON(data.getItems);
                $.each((data.getItems), function (index, e) {
                    let item = $("#select-template > select > option").clone();
                    $(item).attr("value", e.id).text(e.name);
                    $(role).append($(item));
                });
            }
        }
    });
}