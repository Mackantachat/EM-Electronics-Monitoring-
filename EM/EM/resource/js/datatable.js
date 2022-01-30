$.extend(true, $.fn.dataTable.defaults, {
    pageLength: 10,
    autoWidth: true,
    scrollX: true,
    scrollCollapse: true,
    searching: true,
    lengthChange: false,
    dom: "ltipr",
    language: {
        "lengthMenu": "แสดง _MENU_ แถว",
        "zeroRecords": "ไม่พบข้อมูล",
        "info": "แสดง _START_ ถึง _END_ จากทั้งหมด _TOTAL_ รายการ",
        "infoFiltered": "",
        "infoEmpty": "ไม่พบข้อมูลจากทั้งหมด _MAX_ รายการ",
        "paginate": {
            "previous": "ก่อนหน้า",
            "next": "ถัดไป"
        }
    },
    columnDefs: [
        {
            targets: [0, 1],
            className: "aligncenter"
        },
        {
            targets: "_all",
            className: "text-left"
        },
        {
            targets: [0, 1, 2],
            orderable: false,
            searchable: false
        },
        {
            targets: 1,
            width: 50
        },
        {
            targets: -1,
            render: function (data, type) {
                let status = "";
                if (data == "Active") {
                    status = '<span class="text-active">ใช้งาน</span>'
                } else {
                    status = '<span class="text-inactive">ไม่ใช้งาน</span>'
                }
                if (type === "display") {
                    return status;
                }
                return data;
            }
        },
    ]
});

function control_datatable(table_id, columnNo) {
    let table = $(table_id).DataTable();
    $("#search_status").select2({
        width: "100%",
        theme: "bootstrap",
        minimumResultsForSearch: -1
    });

    $("#main-check-all").on("click", function () {
        $(table_id + ' tbody input[type="checkbox"]').prop('checked', this.checked);
    });

    table.on("order.dt search.dt", function () {
        table.column(2, { search: "applied", order: "applied" }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();

    $(table_id).on("page.dt", function () {
        $("input[type=checkbox]").prop("checked", false);
    });

    $(table_id).on("click", "a.edit", function () {
        window.open($(".btn-primary").attr("href") + "?data=" + $(this).closest("tr").find("td").eq(1).attr("data-id"), "_self");
    });

    $("#length_change").change(function () {
        $(table_id).DataTable().page.len($(this).val()).draw();
    });

    $("#search_data").on("keypress", function (e) {
        if (e.keyCode == 13) {
            table.search(this.value).draw();
        }
    }); 

    $("#search_bt").on("click", function () {
        table.search("").search($("#search_data").NSValue()).draw();
    });

    $("#search_status").on("change", function (e) {
        let regExSearch = "^" + $("#search_status").NSValue();
        table.search("").column(columnNo).search(regExSearch, true, false).draw();
    });
}