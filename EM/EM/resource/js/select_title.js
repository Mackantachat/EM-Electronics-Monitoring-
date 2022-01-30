function select_title(title) {
    $(title).select2({
        placeholder: "คำนำหน้าชื่อ *",
        width: "100%",
        theme: "bootstrap"
    });

    $.ajax({
        url: "../control/probationer.aspx",
        data: { action: "get_title" },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (!data.onError) {
                data.getItems = jQuery.parseJSON(data.getItems);
                $.each((data.getItems), function (index, e) {
                    let item = $("#select-template > select > option").clone();
                    $(item).attr("value", e.id).text(e.name);
                    $(title).append($(item));
                });
            }
        }
    });
}