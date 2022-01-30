var map;
var drawingManager;
var searchBox;
var overlay;
var polygon = [];
var param = getparameters();

function prohibited_area() {
    select_area("#groupArea");
    set_navigation($(".btnback").attr("href"));

    if (param.data != null) { set_data(); } else { initMap(); }

    for (i = 0; i < monthNamesFull.length; i++) {
        let item = $("#checkbox-template > div").clone();
        item.find('#checkboxorder').html(monthNamesFull[i]);
        item.find('input[type="checkbox"]').val(i + 1);
        if (i < 6) {
            $("#box_month1").append($(item));
        } else {
            $("#box_month2").append($(item));
        }
    }

    for (i = 0; i < dayNames.length; i++) {
        let item = $("#checkbox-template > div").clone();
        item.removeClass("col-md-2").addClass("col-md-3");
        item.find('#checkboxorder').html(dayNames[i]);
        if (i < 4) {
            $(".box_day1").append($(item));
        } else {
            $(".box_day2").append($(item));
        }
    }

    $(".btntime").click(function () {
        $("div.modal-body").find("input[type=checkbox]:checked").prop("checked", false);
        $("div.modal-body").find("input[type=text]").NSValue("");
        $("#myModal").modal();
        $("#time1").timepicker({
            zindex: 9999999,
            timeFormat: 'HH:mm',
            interval: 15,
            minTime: '00:00',
            maxTime: '23:45',
            startTime: '00:00',
            dynamic: false,
            dropdown: true,
            scrollbar: true,
            append_to: 'div#myModal'
        });

        $("#time2").timepicker({
            zindex: 9999999,
            timeFormat: 'HH:mm',
            interval: 15,
            minTime: '01:00',
            maxTime: '23:59',
            startTime: '01:00',
            dynamic: false,
            dropdown: true,
            scrollbar: true,
            append_to: 'div#myModal',
            _lasttime: '23:59'
        });
    });

    $("#btnclose").click(function () {
        let range = parseInt($("#rangeCount").NSValue());
        $("div.modal-body input:checkbox:checked").each(function () {
            range = range + 1;
            let item = $("#timepick > div.col-md-4").clone();
            $(item).attr("id", "Div" + range);
            $(item).find("button").attr("id", range);
            $(item).find(".day").text($(this).next().text());
            $(item).find(".time_start").NSValue($("#time1").NSValue());
            $(item).find(".time_end").NSValue($("#time2").NSValue());
            $("#box_choose").append($(item));

        });
        $("#rangeCount").NSValue(range);

        $(".remove").click(function () {
            $("#Div" + $(this).attr("id")).remove();
        });

        $(".time_start").timepicker({
            zindex: 9999999,
            timeFormat: 'HH:mm',
            interval: 15,
            minTime: '00:00',
            maxTime: '23:45',
            startTime: '00:00',
            dynamic: false,
            dropdown: true,
            scrollbar: true,
            append_to: 'div#myModal'
        });

        $(".time_end").timepicker({
            zindex: 9999999,
            timeFormat: 'HH:mm',
            interval: 15,
            minTime: '01:00',
            maxTime: '23:59',
            startTime: '01:00',
            dynamic: false,
            dropdown: true,
            scrollbar: true,
            append_to: 'div#myModal',
            _lasttime: '23:59'
        });
    });


    $(".btnsave").click(function () {
        if ($.trim($("#area_name").NSValue()) == "") {
            $("#area_name").NSFocus(); alertify.alert("กรุณากรอกพื้นที่ควบคุม");
            return false;
        }

        if ($.trim($("#groupArea").NSValue()) == "") {
            alertify.alert("กรุณาเลือกกลุ่มพื้นที่", function () {
                $(input).select2("open");
            });
            return false;
        }

        if ($('#box_month input[type=checkbox]:checked').length == 0) {
            alertify.alert("กรุณาระบุเดือนที่ควบคุม");
            return false;
        }

        if ($(".daychoose:visible").length == 0) {
            alertify.alert("กรุณาระบุเวลาที่ควบคุม");
            return false;

        } else {
            var month = [];
            $('#box_month input[type=checkbox]').each(function (i, e) {
                month[i] = {
                    status: $(e).is(":checked") == true ? "Y" : "N"
                };
            });

            var daytime = [];
            $('.daychoose:visible').each(function (j, e) {
                daytime[j] = {
                    day: $(e).find("span").text(),
                    time_start: $(e).find(".time_start").NSValue(),
                    time_end: $(e).find(".time_end").NSValue(),
                };
            });

            $.ajax({
                url: "../control/phohibited.aspx",
                data: {
                    action: "save_area",
                    area_id: param.data || null,
                    area_name: $("#area_name").NSValue(),
                    group_area: $("#groupArea").NSValue(),
                    location: $("#location").NSValue(),
                    month: JSON.stringify(month),
                    day: JSON.stringify(daytime),
                    area_status: $("#status").is(":checked") == true ? "Active" : "Inactive"
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
                    if (!data.onError) {
                        data.getItems = jQuery.parseJSON(data.getItems);
                        if (data.getItems.status == "success") {
                            alertify.alert(data.getItems.txtAlert, function () {
                                window.location.href = $(".btnback").attr('href');
                            });
                        } else {
                            alertify.alert(data.getItems.txtAlert);
                        }
                    } else {
                        alertify.alert("ไม่สามารถเพิ่มข้อมูลได้");
                    }
                    $.busyLoadFull("hide");
                }
            });
        }
    });
}

function set_data() {
    $("#btnstatus").removeClass("hidden");
    $("#status").bootstrapSwitch("size", "small");
    $("#status").bootstrapSwitch("state", true);
    $("#subject").html("แก้ไขข้อมูลรายการพื้นที่หวงห้าม");
    $.ajax({
        url: "../control/phohibited.aspx",
        data: {
            action: "area",
            area_id: param.data,
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
                    getItems = jQuery.parseJSON(data.getItems);
                    let area = getItems.area;
                    let mon = area.area_month.split(",");
                    let detail = getItems.area_details;
                   
                    $('#box_month input[type=checkbox]').each(function (i, e) {
                        if (mon[i] == 'Y') {
                            $(this).prop("checked", true);
                        }
                    });

                    let day = detail;
                    $("#rangeCount").NSValue((day.length - 1));

                    for (i = 0; i < day.length; i++) {
                        range = i + 1;
                        let item = $("#timepick > div.col-md-4").clone();
                        item.attr("id", "Div" + range);
                        item.find("button").attr("id", range);
                        item.find(".day").NSValue(day[i].data_day);
                        item.find(".time_start").NSValue(day[i].data_start.slice(0, 5));
                        item.find(".time_end").NSValue(day[i].data_end.slice(0, 5));
                        $("#box_choose").append($(item));
                    }

                    $(".remove").click(function () {
                        $("#Div" + $(this).attr("id")).remove();
                    });

                    $(".time_start").timepicker({
                        zindex: 9999999,
                        timeFormat: 'HH:mm',
                        interval: 15,
                        minTime: '00:00',
                        maxTime: '23:45',
                        startTime: '00:00',
                        dynamic: false,
                        dropdown: true,
                        scrollbar: true,
                        append_to: 'div#myModal'
                    });

                    $(".time_end").timepicker({
                        zindex: 9999999,
                        timeFormat: 'HH:mm',
                        interval: 15,
                        minTime: '01:00',
                        maxTime: '23:59',
                        startTime: '01:00',
                        dynamic: false,
                        dropdown: true,
                        scrollbar: true,
                        append_to: 'div#myModal',
                        _lasttime: '23:59'
                    });

                    $("#area_name").NSValue(area.area_name);
                    $('#groupArea').NSValue(area.area_type);
                    $("#location").NSValue(area.area_location);
                    if (area.area_status == "Active") { $("#status").bootstrapSwitch("state", true); }
                    else { $("#status").bootstrapSwitch("state", false); }
                    initMap();
                }
                $.busyLoadFull("hide");
            }, 2000);
        }
    });
}

function initMap() {
    var map = new google.maps.Map(document.getElementById("map"), {
        zoom: 8,
        center: { lat: 13.736717, lng: 100.523186 }
    });

    var triangleCoords = JSON.parse("[" + $('#location').val().split('(').join('[').split(')').join(']') + "]");
    var points = [];
    for (var i = 0; i < triangleCoords.length; i++) {
        points.push({
            lat: triangleCoords[i][0],
            lng: triangleCoords[i][1]
        });
    }

    //show the polygon
    var bermudaTriangle = new google.maps.Polygon({
        paths: points,
        strokeColor: '#FF0000',
        strokeOpacity: 0.5,
        strokeWeight: 2,
        fillColor: '#FF0000',
        fillOpacity: 0.35
    });
    bermudaTriangle.setMap(map);

    drawingManager = new google.maps.drawing.DrawingManager({
        drawingMode: google.maps.drawing.OverlayType.POLYGON,
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.TOP_CENTER,
            drawingModes: [google.maps.drawing.OverlayType.POLYGON]
        }
    });

    drawingManager.setMap(map);

    google.maps.event.addListener(drawingManager, "overlaycomplete", function (event) {
        var newShape = event.overlay;
        newShape.type = event.type;
    });

    google.maps.event.addListener(drawingManager, "overlaycomplete", function (event) {
        google.maps.event.addListener(event.overlay, 'click', function (evt) {
            overlay = this;
            getCoordinates();
        })

        $('#location').val(event.overlay.getPath().getArray());
        overlay = event.overlay;
        google.maps.event.addListener(event.overlay.getPath(), 'insert_at', getCoordinates);
        google.maps.event.addListener(event.overlay.getPath(), 'remove_at', getCoordinates);
        google.maps.event.addListener(event.overlay.getPath(), 'set_at', getCoordinates);
    });

    var input = document.getElementById('pac-input');
    searchBox = new google.maps.places.SearchBox(input);
    searchBox.addListener('places_changed', function () {
        var places = searchBox.getPlaces();
        if (places.length == 0) {
            return;
        }

        var bounds = new google.maps.LatLngBounds();
        places.forEach(function (place) {
            if (!place.geometry) {
                console.log("Returned place contains no geometry");
                return;
            }

            if (place.geometry.viewport) {
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
    });
}

function getCoordinates(index, element) {
    $('#location').val(overlay.getPath().getArray());
}

function overlayClickListener(overlay) {
    google.maps.event.addListener(overlay, "click", function (event) {
        overlay = this;
        $('#location').val(overlay.getPath().getArray());
    });
}