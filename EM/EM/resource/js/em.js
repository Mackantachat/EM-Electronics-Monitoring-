function set_navigation(url) {
    $('ul.sidebar-menu a').filter(function () {
        return $(this).attr("href") == url;
    }).parent().addClass('active');

    $('ul.treeview-menu a').filter(function () {
        return $(this).attr("href") == url;
    }).parentsUntil(".sidebar-menu > .treeview-menu").addClass('active');
}

function em_function() {
    $.busyLoadSetup({
        animation: "fade",
        background: "rgba(71, 71, 71, 0.8)",
    });

    set_navigation(window.location.pathname);
    //$("input:text:not(#searchdata)").bind('copy paste', function (e) {
    //    alertify.alert("ไม่สามารถทำการคัดลอกรายการได้");
    //    e.preventDefault(); return false;
    //});
}
