var dayNames = ["วันอาทิตย์", "วันจันทร์", "วันอังคาร", "วันพุธ", "วันพฤหัสบดี", "วันศุกร์","วันเสาร์"];
var monthNames = ["ม.ค.", "ก.พ.", "มี.ค.", "เม.ย.", "พ.ค.", "มิ.ย.",
    "ก.ค.", "ส.ค.", "ก.ย.", "ต.ค.", "พ.ย.", "ธ.ค."];
var monthNamesFull = ["มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน",
    "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม"];
var NS = {
    DEBUG: function () {
        return (location.hostname === 'localhost') ? true : false;
    },
    Browser: {
        Chrome: window.navigator.userAgent.indexOf('Chrome') > -1 || window.navigator.userAgent.indexOf('Firefox') > -1 ? true : false,
        IE: false
    },
    postedData: function (xobj) {
        var retValue = ''
        $(xobj).each(function () {
            $.each(this.attributes, function () {
                if (this.specified) {
                    if (!(this.name == 'name' || this.name == 'type' || this.name == 'id' || this.name == 'style')) {
                        retValue += '<input name="' + this.name + '" value="' + this.value + '" style="display:none;" />';
                    }
                }
            });
        });
        return retValue;
    },
    ajaxData: function (setting) {
        var self = this;
        self.config = {};
        $.extend(self.config, setting);
        $("#data").each(function () {
            $.each(this.attributes, function () {
                if (this.specified) {
                    if (!(this.name == 'name' || this.name == 'type' || this.name == 'id' || this.name == 'style')) {
                        self.config[this.name] = this.value;
                    }
                }
            });
        });
        return this;
    },
    IsEmail: function (email) {
        var regex = /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/g;
        if (!regex.test(email)) { return false; } else { return true; }
    },
    calAge: function (birthday) {
        var age = '';
        if (!(birthday == '' || birthday == undefined)) {
            var today = new Date(), // today date object
                birthday_val = birthday.split('/'), // input value
                birthday = new Date(birthday_val[2], birthday_val[1] - 1, birthday_val[0]); // birthday date object
            today.setFullYear(today.getFullYear() + 543);
            // calculate age
            age = (today.getMonth() == birthday.getMonth() && today.getDate() > birthday.getDate()) ?
                today.getFullYear() - birthday.getFullYear() : (today.getMonth() > birthday.getMonth()) ?
                    today.getFullYear() - birthday.getFullYear() :
                    today.getFullYear() - birthday.getFullYear() - 1;
            age = age;
            return age;
        }
    },
    dateText: function (dateIn, fullText) {
        fullText = fullText || false;
        var arMonth = fullText ? monthNamesFull : monthNames;
        if (dateIn != undefined && dateIn != "") {
            dateIn = dateIn.split('/')
            var d = new Date(dateIn[2], dateIn[1] - 1, dateIn[0]);
            return dateIn[0] + ' ' + arMonth[d.getMonth()] + ' ' + dateIn[2];
        }
        return dateIn;
    },
    jsDateDiff: function (Date1, Date2) {
        return (((Date.parse(Date2) / 1000) - Date.parse(Date1) / 1000)) / (60 * 60 * 24);
    },
    // sort on values
    srt: function (desc) {
        return function (a, b) {
            return desc ? ~~(a < b) : ~~(a > b);
        };
    },
    // sort on key values
    keysrt: function (key, desc) {
        return function (a, b) {
            return desc ? ~~(a[key] < b[key]) : ~~(a[key] > b[key]);
        }
    },
    // sort on key values
    keysrtnumber: function (key, desc) {
        return function (a, b) {
            return desc ? ~~(b[key] - a[key]) : ~~(a[key] - b[key]);
        }
    },
    unique: function (a) {
        var seen = {};
        return a.filter(function (item) {
            return seen.hasOwnProperty(item) ? false : (seen[item] = true);
        });
    },
    today: function () {
        retToday = new Date();
        return new Date(retToday.setFullYear(retToday.getFullYear()));
    },
    addDays: function (date, days) {
        var result = new Date(date);
        result.setDate(result.getDate() + parseInt(days));
        return result;
    },
}
$.extend($.fn, {
    setDropdownList: function (setting) {
        var config = {
            defaultSelect: "",
        };
        $.extend(config, setting);
        var dclass = "";
        if ($(this).attr("class") != undefined) {
            dclass = $(this).attr("class");
        }
        $.selecter("defaults", { customClass: dclass });
        $(this).selecter().change();
        if (config.defaultSelect != "") { $(this).val(config.defaultSelect).change(); }

        return this;
    },
    setDropdownListValue: function (setting) {
        var self = this;
        var config = {
            url: '',
            data: {},
            type: "POST",
            dataType: "json",
            enable: true,
            defaultSelect: "",
            dataObject: [],
            tempData: false,
        };
        $.extend(config, setting);
        //var timeStamp = new Performance(config.data.action);
        var compile = function (data) {
            if (config.tempData) { $(self).data("data-ddl", data); }
            $(self).html('');
            var placeholder = $(self).NSAttr("placeholder") || "กรุณาเลือก";
            $("<option>", { value: "" }).html(placeholder).appendTo(self);
            for (i = 0; i < data.length; i++) {
                $("<option>", {}).NSFill(data[i]).appendTo(self);
            }
            //timeStamp.Check("added " + data.length + " items of " + config.data.action);
            $(self).setDropdownList().selecter("update");
            if (config.defaultSelect != "") { $(self).NSValue(config.defaultSelect); }
            if (config.enable) { $(self).NSEnable(); } else { $(self).NSDisable(); }
            //timeStamp.Stop("stop success of " + config.data.action);
        }
        //timeStamp.Start("start of " + config.data.action);
        if (config.dataObject.length == 0) {
            $.ajax({
                url: config.url,
                data: config.data,
                type: config.type,
                dataType: config.dataType,
                error: function (xhr, s, err) {
                    console.log(s, err);
                    //timeStamp.Stop("stop error of " + config.data.action);
                },
                success: function (data) {
                    //timeStamp.Check("received response of " + config.data.action);
                    if (!data.onError) {
                        data.getItems = jQuery.parseJSON(data.getItems);
                        if (config.tempData) { $(self).data("data-ddl", data.getItems); }
                        compile(data.getItems);
                    } else {
                        $(self).setDropdownList().selecter("update");
                        //timeStamp.Stop("stop onError of " + config.data.action);
                    }
                }
            });    //End ajax
        } else {
            compile(config.dataObject);
        }
        return this;
    },
    setAutoList: function (setting) {
        self = $(this);
        var config = {
            defaultSelect: "",
            enableDefSelFun: true,
            selectItem: function () { },
        };
        $.extend(config, setting);
        $(self).addClass("selecte-box-custom");
        if (config.defaultSelect != "") {
            if (config.enableDefSelFun) {
                $(self).NSValue(config.defaultSelect);
            } else { $(self).val(config.defaultSelect); }
        } else { $(self).val(config.defaultSelect); }
        $(this).unbind("change").bind("change", config.selectItem);
        return this;
    },
    setAutoListValue: function (setting) {
        var self = this;
        var config = {
            url: '',
            data: {},
            type: "POST",
            dataType: "json",
            selectItem: function () { },
            closeItem: function () { },
            buildFunction: function () { },
            enable: true,
            defaultSelect: "",
            defaultFirst: false,
            dataObject: [],
            tempData: false,
            enableDefSelFun: true,
        };
        $.extend(config, setting);
        var compile = function (data) {
            if (config.tempData) { $(self).data("data-ddl", data); }
            $(self).html('');
            if (($(self).NSAttr("placeholder") || "") != "") { $("<option>", { value: '', text: $(self).NSAttr("placeholder") }).appendTo($(self)); }
            for (i = 0; i < data.length; i++) {
                $("<option>", {}).NSFill(data[i]).appendTo(self);
            }
            $(self).addClass("selecte-box-custom").unbind("change").bind("change", config.selectItem);
            config.buildFunction();
            if (config.defaultSelect != "") {
                if (config.enableDefSelFun) {
                    (self).NSValue(config.defaultSelect);
                } else { (self).val(config.defaultSelect); }
            } else if (config.defaultFirst) {
                if (config.enableDefSelFun) {
                    $(self).NSValue($(self).find('option:first').val());
                } else { (self).val($(self).find('option:first').val()); }
            } else { $(self).val(config.defaultSelect); }
            //if (config.enable) { $(self).NSEnable(); } else { $(self).NSDisable(); }
        }
        if (config.url != "") {
            $.ajax({
                url: config.url,
                data: config.data,
                type: config.type,
                dataType: config.dataType,
                error: function (xhr, s, err) {
                    console.log(s, err);
                    //timeStamp.Stop("stop error of " + config.data.action);
                },
                success: function (data) {
                    //timeStamp.Check("received response of " + config.data.action);
                    if (!data.onError) {
                        data.getItems = jQuery.parseJSON(data.getItems);
                        if (config.tempData) { $(self).data("data-ddl", data.getItems); }
                        compile(data.getItems);
                    } else {
                        $(self).combobox({
                            select: config.selectItem,
                            placeholder: config.placeholder,
                        });
                        //timeStamp.Stop("stop onError of " + config.data.action);
                    }
                }
            });    //End ajax
        } else {
            compile(config.dataObject);
        }
        return this;
    },
    setCalendar: function (setting) {
        var config = {
            month: 1,
            maxDate: null,
            minDate: null,
            format: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            showButtonPanel: false,
            yearRange: "c-20:c+20",
            onSelect: function (selectedDate, objDate) { },
            onClose: function () { },
            onEnterKey: function () { },
        };
        $.extend(config, setting);
        //$(this).bind("keydown", function (e) {
        //    if (e.which == 13) {
        //        $(this).datepicker("hide");
        //        config.onEnterKey();
        //        e.preventDefault();
        //        return false;
        //    }
        //}).datepicker({
        //    numberOfMonths: config.month,
        //    maxDate: config.maxDate,
        //    minDate: config.minDate,
        //    dateFormat: config.format,
        //    constrainInput: true,
        //    changeMonth: config.changeMonth,
        //    changeYear: config.changeYear,
        //    showButtonPanel: config.showButtonPanel,
        //    yearOffSet: 543,
        //    monthNames: ["มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถนายน",
        //    	"กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤษจิกายน", "ธันวาคม"], 
        //    monthNamesShort: ["ม.ค.", "ก.พ.", "มี.ค.", "เม.ย.", "พ.ค.", "มิ.ย.", "ก.ค.", "ส.ค.", "ก.ย.", "ต.ค.", "พ.ย.", "ธ.ค."], 
        //    dayNames: ["อาทิตย์", "จันทร์", "อังคาร", "พุธ", "พฤหัสบดี", "ศุกร์", "เสาร์"], 
        //    dayNamesShort: ["อา.", "จ.", "อ.", "พ.", "พฤ.", "ศ.", "ส."], 
        //    dayNamesMin: ["อ", "จ", "อ", "พ", "พ", "ศ", "ส"], 
        //    yearRange: config.yearRange,
        //    onSelect: config.onSelect,
        //    onClose: config.onClose,
        //});
        return this;
    },
    toggleSlide: function (setting) {
        var config = {
            id: ''
        };
        $.extend(config, setting);
        if (config.id == '') { config.id = ($(this).attr("targetToggle") || "") == "" ? $(this).NSAttr("id").replace("tog", "div") : $(this).attr("targetToggle") }
        $(this).click(function () {
            if ($("#" + config.id).css("display") == "none") {
                $("#" + config.id).slideDown();
                $(this).removeClass("glyphicon-menu-down").addClass("glyphicon-menu-up");
            } else {
                $("#" + config.id).slideUp();
                $(this).removeClass("glyphicon-menu-up").addClass("glyphicon-menu-down");
            }
        });
        return this;
    },
    NSValue: function (value, e) {
        var self = this;
        if (value != undefined && value != null) {
            switch ($(self).prop('tagName')) {
                case 'INPUT':
                    var attrType = $(self).attr('type');
                    if (attrType == 'text') {
                        var pretext = $(self).attr('pretext') || '';
                        $(self).val(value);
                        if (!NS.Browser.Chrome && value === '') $(self).val(pretext);
                    } else if (attrType == 'radio' || attrType == 'checkbox') {
                        $(self).prop('checked', value.toBoolean());
                    } else if (attrType == 'password') {
                        $(self).val(value);
                    } else {
                        $(self).val(value);
                    }
                    break;
                case 'SELECT':
                    if ($(self).NSAttr("combobox") == "Y") {
                        $(self).combobox("setvalue", value)
                    } else {
                        $(self).val(value);
                        $(self).change();
                    }
                    break;
                case 'OPTION':
                    $(self).val(value);
                    break;
                default:
                    $(self).html(value).val(value);
                    break;
            }
            return self;
        } else {
            var result = null;
            switch ($(self).prop('tagName')) {
                case 'INPUT':
                    if ($(self).attr('type') == 'text') {
                        result = ($.trim($(self).val()) == $(self).attr('pretext') || $.trim($(self).val()) == '') ? '' : $.trim($(self).val());
                    } else if ($(self).attr('type') == 'radio') {
                        result = ($(self).prop('checked') == true || $(self).attr('checked') == 'checked') ? 'Y' : 'N';
                    } else if ($(self).attr('type') == 'password') {
                        result = $(self).val();
                    } else {
                        result = $.trim($(self).val());
                    }
                    break;
                case 'SELECT':
                    result = $(self).val();
                    if (result == null) { result = ''; }
                    break;
                case 'TEXTAREA': result = $.trim($(self).val()); break;
                default: result = $(self).html(); break;
            }
            return result;
        }
    },
    NSAttr: function (name, value) {
        if (name != undefined && value == undefined) {
            return $(this).attr(name);
        } else if (value != undefined) {
            $(this).attr(name, value);
        } else {
            return $(this).attr('default-data');
        }
    },
    NSFill: function (e, attr) {
        if (e == undefined && attr == undefined) attr = '';
        if (attr == undefined) attr = e;
        var Obj = (typeof (attr) !== 'object') ? ((attr == undefined) ? {} : { value: attr }) : attr;
        for (prop in Obj) {
            var eAttr = null, value = null;
            try {
                eAttr = $(e).attr(Obj[prop]) || $(e).find(Obj[prop]).val()
                value = (typeof (e) === 'object') ? ((eAttr != undefined) ? eAttr : (attr == e) ? Obj[prop] : '') : e;
            } catch (err) { value = (e != undefined && Obj[prop] != undefined) ? e : ''; }
            switch (prop) {
                case 'value':
                    $(this).attr('default-data', value);
                    $(this).NSValue(value, e);
                    break;
                case 'text':
                    if ($(this).prop('tagName') == "OPTION") {
                        $(this).html(value);
                    }
                    break;
                default: $(this).attr(prop, value); break;
            }
        }
        return this;
    },
    NSNumberbox: function () {
        $(this).keypress(function (e) {
            var keyCode = e.keyCode || e.which;
            var ValueNullKey = ($.trim($(this).val()) === $(this).attr('pretext')) || ($.trim($(this).val()) === "");
            var OtherKey = ((e.ctrlKey && keyCode != 86) || e.ctrlKey || e.altKey || keyCode == 8 || keyCode == 46 || keyCode == 13);
            var NumKey = (keyCode >= 48 && keyCode <= 57);
            if (!NumKey && !OtherKey) { return false; } else { if (ValueNullKey) $(this).val(''); }
        });
        return this;
    },
    NSMoneybox: function () {
        $(this).addClass("text-right").val($(this).NSNumber());
        $(this).bind({
            keypress: function (e) {
                var keyCode = e.keyCode || e.which;
                var ValueNullKey = ($.trim($(this).val()) === $(this).attr('pretext')) || ($.trim($(this).val()) === "");
                var MinusKey = $(this)[0].selectionStart == 0 && $(this)[0].selectionEnd == 0;
                var OtherKey = (e.ctrlKey || e.altKey || keyCode == 8 || keyCode == 46 || (MinusKey && keyCode == 45));
                var MoneyKey = (keyCode >= 48 && keyCode <= 57) || keyCode == 46;
                if (!MoneyKey && !OtherKey) { return false; } else { if (ValueNullKey) $(this).val(""); }
            },
            focusin: function (e) { $(this).val($(this).val().replace(/,/g, '')).select(); },
            focusout: function (e) { $(this).val($(this).NSNumber()); }
        });
        return this;
    },
    NSNumber: function () {
        return ($(this).val() != undefined) ? (!isNaN($(this).val().toNumber()) ? $(this).val().toNumber() : 0).toMoney() : 0;
    },
    NSNumberboxInteger: function () {
        $(this).keypress(function (e) {
            var keyCode = e.keyCode || e.which;
            var ValueNullKey = ($.trim($(this).val()) === $(this).attr('pretext')) || ($.trim($(this).val()) === "");
            var OtherKey = ((e.ctrlKey && keyCode != 86) || e.ctrlKey || e.altKey || keyCode == 8 || keyCode == 13);
            var NumKey = (keyCode >= 48 && keyCode <= 57);
            if (!NumKey && !OtherKey) { return false; } else { if (ValueNullKey) $(this).val(''); }
            $(this).focusout(function (e) {
                if ($(this).NSValue() !== "") {
                    var textInt = Math.round($(this).val())
                    if (!textInt) {
                        textInt = ''
                    }
                    $(this).val(textInt)
                }
            });
        });
        return this;
    },
    NSNumberAlphabetbox: function () {
        $(this).keypress(function (e) {
            var keyCode = e.keyCode || e.which;
            var ValueNullKey = ($.trim($(this).val()) === $(this).attr('pretext')) || ($.trim($(this).val()) === "");
            var OtherKey = ((e.ctrlKey && keyCode != 86) || e.ctrlKey || e.altKey || keyCode == 8 || keyCode == 46 || keyCode == 13);
            var NumKey = ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122));
            if (!NumKey && !OtherKey) { return false; } else { if (ValueNullKey) $(this).val(''); }
        });
        return this;
    },
    NSPhonebox: function () {
        $(this).keypress(function (e) {
            var keyCode = e.keyCode || e.which;
            var ValueNullKey = ($.trim($(this).val()) === $(this).attr('pretext')) || ($.trim($(this).val()) === "");
            var OtherKey = ((e.ctrlKey && keyCode != 86) || e.ctrlKey || e.altKey || keyCode == 8 || keyCode == 45 || keyCode == 35 || keyCode == 16 || keyCode == 13 || (keyCode >= 40
                && keyCode <= 43));
            var NumKey = (keyCode >= 47 && keyCode <= 57);
            if (e.shiftKey || keyCode == 3665 || keyCode == 3666 || keyCode == 42 || keyCode == 43 || keyCode == 45 || keyCode == 47) { return false; }
            if (!NumKey && !OtherKey) { return false; } else { if (ValueNullKey) $(this).val(''); }
        });
        return this;
    },
    NSThaibox: function (optionKey) {
        $(this).keypress(function (e) {
            var keyCode = e.keyCode || e.which;
            var ValueNullKey = ($.trim($(this).val()) === $(this).attr('pretext')) || ($.trim($(this).val()) === "");
            var OtherKey = ((e.ctrlKey && keyCode != 86) || e.ctrlKey || e.altKey || keyCode == 8 || keyCode == 45 || keyCode == 13);
            OtherKey = (OtherKey || keyCode == optionKey)
            var NumKey = ((keyCode >= 3585 && keyCode <= 3641) || (keyCode >= 3648 && keyCode <= 3660));
            if (!NumKey && !OtherKey) { return false; } else { if (ValueNullKey) $(this).val(''); }
        });
        return this;
    },
    NSEnglishbox: function (optionKey) {
        $(this).keypress(function (e) {
            var keyCode = e.keyCode || e.which;
            var ValueNullKey = ($.trim($(this).val()) === $(this).attr('pretext')) || ($.trim($(this).val()) === "");
            var OtherKey = ((e.ctrlKey && keyCode != 86) || e.ctrlKey || e.altKey || keyCode == 8 || keyCode == 45 || keyCode == 13);
            OtherKey = (OtherKey || keyCode == optionKey)
            var NumKey = ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122));
            if (!NumKey && !OtherKey) { return false; } else { if (ValueNullKey) $(this).val(''); }
        });
        return this;
    },
    NSNamebox: function (optionKey) {
        $(this).keypress(function (e) {
            var keyCode = e.keyCode || e.which;
            var ValueNullKey = ($.trim($(this).val()) === $(this).attr('pretext')) || ($.trim($(this).val()) === "");
            var OtherKey = ((e.ctrlKey && keyCode != 86) || e.ctrlKey || e.altKey || keyCode == 8 || keyCode == 45 || keyCode == 13);
            OtherKey = (OtherKey || keyCode == optionKey)
            var NumKey = ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122)
                || (keyCode >= 3585 && keyCode <= 3641) || (keyCode >= 3648 && keyCode <= 3660));
            if (!NumKey && !OtherKey) { return false; } else { if (ValueNullKey) $(this).val(''); }
        });
        return this;
    },
    NSDatebox: function (setting) {
        // Can't use date below 01-01-1900
        var self = $(this);
        var config = {
            pattern: "dd-MM-yyyy",
            allowPastDate: true,
            allowFutureDate: true,
            lockDate: formatDate(NS.today(), "dd-MM-yyyy"),
            focusOut: function () { },
        };
        $.extend(config, setting);

        $(self).data('error', false);
        if (typeof (config.pattern) === 'undefined') config.pattern = "dd-MM-yyyy";
        $(self).attr('maxlength', '10').css({ 'text-align': 'center' });
        $(self).keypress(function (e) {
            var keyCode = e.keyCode || e.which;
            var OtherKey = ((e.ctrlKey && keyCode != 86) || e.altKey || keyCode == 8 || keyCode == 9 || keyCode == 46);
            var DateKey = (keyCode >= 48 && keyCode <= 57) || String.fromCharCode(keyCode) === '-';
            if (!DateKey && !OtherKey) { return false; }
        });
        $(self).focusout(function (e) {
            var checkDate = function () {
                $(self).val($(self).val().replace(/\W+/g, ''));
                $(self).next().remove();
                if (isDate($(self).val(), config.pattern.replace(/\W+/g, ''))) {
                    var checkLockDate = formatDate(NS.today(), "dd-MM-yyyy");
                    if (config.lockDate.indexOf("#") > -1) {
                        if ($(config.lockDate).length > 0) {
                            checkLockDate = $(config.lockDate).NSValue();
                        }
                    }
                    //// ตรวจสอบวันที่ห้ามมากหรือห้ามน้อยกว่าวันปัจจุบัน
                    if (!config.allowPastDate && formatDate(new Date(getDateFromFormat($(self).val(), config.pattern.replace(/\W+/g, ''))), 'yyyyMMdd') < formatDate(new Date(getDateFromFormat(checkLockDate.replace(/\W+/g, ''), config.pattern.replace(/\W+/g, ''))), 'yyyyMMdd')) {
                        $(self).focus().val('');
                        notiWarning("Date can not less than " + formatDate(new Date(getDateFromFormat(checkLockDate.replace(/\W+/g, ''), config.pattern.replace(/\W+/g, ''))), 'dd NNN yyyy'));
                    }
                    if (!config.allowFutureDate && formatDate(new Date(getDateFromFormat($(self).val(), config.pattern.replace(/\W+/g, ''))), 'yyyyMMdd') > formatDate(new Date(getDateFromFormat(checkLockDate.replace(/\W+/g, ''), config.pattern.replace(/\W+/g, ''))), 'yyyyMMdd')) {
                        $(self).focus().val('');
                        notiWarning("Date can not more than " + formatDate(new Date(getDateFromFormat(checkLockDate.replace(/\W+/g, ''), config.pattern.replace(/\W+/g, ''))), 'dd NNN yyyy'));
                    }
                    if ($(self).val() != "") {
                        var isValue = new Date(getDateFromFormat($(self).val(), config.pattern.replace(/\W+/g, '')));
                        $(self).val(formatDate(isValue, config.pattern));
                    }
                } else if ($.trim($(self).val()) !== "") {
                    $(self).NSValue("").focus();
                    notiWarning("Incorrect date.");
                }
                config.focusOut();
            }
            var checkLenght = function () {
                if ($(self).NSValue().length == 1) {
                    $(self).NSValue("0" + $(self).NSValue() + ("0" + (NS.today().getMonth() + 1)).slice(-2));
                    checkLenght();
                } else if ($(self).NSValue().length == 2) {
                    $(self).NSValue($(self).NSValue() + ("0" + (NS.today().getMonth() + 1)).slice(-2));
                    checkLenght();
                } else if ($(self).NSValue().length == 3) {
                    $(self).NSValue("0" + $(self).NSValue() + NS.today().getFullYear());
                    checkLenght();
                } else if ($(self).NSValue().length == 4) {
                    $(self).NSValue($(self).NSValue() + NS.today().getFullYear());
                    checkLenght();
                } else if ($(self).NSValue().length == 8) {
                    checkDate();
                } else {
                    checkDate();
                }
            }
            if ($(self).NSValue() != "") {
                checkLenght();
            }
        });

        $(self).focusin(function (e) {
            $(self).data('error', false);
            if ($(self).NSValue() !== "") { $(self).val($(self).val().replace(/\W+/g, '')).select(); }
        });

        return self;
    },
    hasScrollBar: function () {
        return this.get(0).scrollHeight > this.height();
    },
    NSDisable: function () {
        switch ($(this).prop('tagName')) {
            case 'INPUT':
                $(this).prop('disabled', true);
                break;
            case 'SELECT':
                if ($(this).NSAttr("combobox") == undefined) {
                    $(this).prop('disabled', true); //.selecter("disable");
                } else {
                    $(this).combobox("disable");
                }
                break;
            default:
                $(this).prop('disabled', true);
                break;
        }
        return this;
    },
    NSEnable: function () {
        switch ($(this).prop('tagName')) {
            case 'INPUT':
                $(this).prop('disabled', false);
                break;
            case 'SELECT':
                if ($(this).NSAttr("combobox") == undefined) {
                    $(this).prop('disabled', false); //.selecter("enable");
                } else {
                    $(this).combobox("enable");
                }
                break;
            default:
                $(this).prop('disabled', false);
                break;
        }
        return this;
    },
    NSRemoveAttr: function (nameList) {
        var nameList = nameList.split(",")
        for (i = 0; i < nameList.length; i++) {
            $(this).removeAttr(nameList[i].replace(/ /g, ''));
        }
        return this;
    },
    NSFocus: function () {
        switch ($(this).prop('tagName')) {
            case 'INPUT':
                $(this).focus();
                break;
            case 'SELECT':
                if ($(this).NSAttr("combobox") == "Y") {
                    $(this).closest("div").find("input").focus();
                } else if ($(this).hasClass("selecter-element")) {
                    $(this).closest("div").focus();
                } else {
                    $(this).focus();
                }
                break;
            default:
                $(this).focus();
                break;
        }
        return this;
    },
    anyKey: function (fnc) {
        return this.each(function () {
            $(this).keydown(function (ev) {
                var keycode = (ev.keyCode ? ev.keyCode : ev.which);
                if (!(keycode == '9' || keycode == '16' || keycode == '17' || keycode == '18')) {
                    fnc.call(this, ev);
                    if ($("#data").NSAttr("logged") != "Y" && $("#data").NSAttr("donorID") != undefined) { return false; }
                }
            })
        })
    },
    enterKey: function (fnc) {
        return this.each(function () {
            $(this).keypress(function (ev) {
                var keycode = (ev.keyCode ? ev.keyCode : ev.which);
                if (keycode == '13') {
                    fnc.call(this, ev);
                }
            })
        })
    },
    tabKey: function (fnc, enableTab) {
        enableTab = enableTab || false
        return this.each(function () {
            $(this).keydown(function (ev) {
                var keycode = (ev.keyCode ? ev.keyCode : ev.which);
                if (keycode == '9') {
                    fnc.call(this, ev);
                    if (!enableTab) { ev.preventDefault(); }
                }
            })
        })
    },
    NSReadOnly: function () {
        switch ($(this).prop('tagName')) {
            case 'INPUT':
                $(this).prop('readonly', true);
                break;
            case 'SELECT':
                if ($(this).NSAttr("combobox") == undefined) {
                    $(this).prop('readonly', true); //.selecter("disable");
                } else {
                    $(this).combobox("readonly");
                }
                break;
            default:
                $(this).prop('readonly', true);
                break;
        }
        return this;
    },
    NSWriteAble: function () {
        switch ($(this).prop('tagName')) {
            case 'INPUT':
                $(this).NSRemoveAttr("readonly");
                break;
            case 'SELECT':
                if ($(this).NSAttr("combobox") == undefined) {
                    $(this).NSRemoveAttr("readonly"); //.selecter("disable");
                } else {
                    $(this).NSRemoveAttr("readonly");
                }
                break;
            default:
                $(this).NSRemoveAttr("readonly");
                break;
        }
        return this;
    },
    MBOSAjax: function (type, setting) {
        var e = this;
        var config = {
            url: '',
            data: {},
            type: "POST",
            beforeSend: function () { },
        };
        if (setting == undefined) {
            setting = type;
            type = 'JSON';
        }
        $.extend(config, setting);
        $.ajax({
            type: 'POST', dataType: type,
            url: config.url,
            data: config.data,
            beforeSend: function () { config.beforeSend(); },
            error: function (xhr, s, err) {
                if (config.error != undefined) {
                    config.error(xhr, s, err);
                }
            },
            success: function (data) {
                if (config.success != undefined) {
                    config.success(data);
                } else {
                    notiError("Please add success function.");
                }
            }
        });
        return this;
    },
    NSTimeFormate: function () {
        self = $(this);
        if ($(self).NSValue() != "__:__") {
            var strTime = $(self).NSValue().replace(/_/g, "").replace(/:/g, "");
            var len = strTime.length;
            var time = "000000";
            var finalTime = "";
            if (len == 1) { finalTime = "0" + strTime + "0000"; }
            else if (len == 3) { finalTime = "0" + strTime + "00"; }
            else if (len == 5) { finalTime = "0" + strTime; }
            else { finalTime = strTime + "" + time.substring(len); }
            if (parseInt(finalTime.substring(0, 2)) > 23) { finalTime = ""; }
            if (parseInt(finalTime.substring(2, 4)) > 59) { finalTime = ""; }
            if (parseInt(finalTime.substring(4)) > 59) { finalTime = ""; }
            if (len == 0) { finalTime = ""; }

            if (finalTime == "") {
                notiWarning("Time is invalid");
                $(self).NSValue("").NSFocus();
            } else {
                $(self).NSValue(finalTime);
            }
        }
        return self;
    },
});
$.extend(Number.prototype, {
    toMoney: function () {
        var n = this, c = 2, d = ".", t = ",", s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
        return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
    },
    toCRate: function () {
        var n = this, c = 5, d = ".", t = ",", s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
        return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
    }
});
$.extend(String.prototype, {
    toNumber: function () { var n = this, f = n != undefined ? parseFloat(parseFloat(n.replace(/,/g, '')).toFixed(2)) : 0; return (!isNaN(f)) ? f : 0; },
    toBoolean: function () {
        var m = { 'n': false, 'N': false, 'no': false, 'NO': false, 'FALSE': false, 'y': true, 'Y': true, 'false': false, 'yes': true, 'YES': true, 'TRUE': true, 'true': true };
        return (m.hasOwnProperty(this)) ? m[this] : false;
    },
    toRate: function () { var n = this, f = n != undefined ? parseFloat(parseFloat(n.replace(/,/g, '')).toFixed(5)) : 0; return (!isNaN(f)) ? f : 0; }
});
$.extend(window, {
    CallbackException: function (m1, m2) {
        this.onError = m1.onError || true;
        this.exTitle = m1.exTitle || ((m2 != undefined) ? m1 : undefined) || "ERROR";
        this.exMessage = m1.exMessage || m2 || m1 || "";
        this.getItems = m1.getItems || {};
        if (m1.getItems != undefined) this.getItems = jQuery.parseJSON(this.getItems);
        this.toString = function () { return this.exTitle + ' >>> ' + this.exMessage; }
    },
    Performance: function (funcName) {
        var EnablePerformance = performance != undefined;
        var BeginTime = 0.0, ElapsedTime = 0.0, FinishTime = 0.0;
        var func_name = funcName || "Performance"
        this.Start = function () {
            if (EnablePerformance) {
                var time = new Date().getHours() + ':' + new Date().getMinutes() + ':' + new Date().getSeconds();
                BeginTime = performance.now();
                ElapsedTime = BeginTime;
                log(func_name + "() Performance >>> Starting on " + time);
            }
        }
        this.Check = function (msg) {
            if (EnablePerformance) {
                var now = performance.now();
                log((msg == undefined ? func_name + "() elapsed time is" : msg) + "\n\r", (now - ElapsedTime), "ms");
                ElapsedTime = now;
            }
        }
        this.Stop = function (msg) {
            if (EnablePerformance) {
                var time = new Date().getHours() + ':' + new Date().getMinutes() + ':' + new Date().getSeconds();
                var now = performance.now();
                FinishTime = now;
                log((msg == undefined ? func_name + "() elapsedtime is" : msg) + "\n\r", (now - ElapsedTime), "ms (" + ((FinishTime - BeginTime) / 1000).toFixed(2) + " s)");
                log(func_name + "() Performance >>> Stoped on " + time);
                ElapsedTime = performance.now();
            }
        }
        this.toString = function () { return func_name + "() elapsedtime is " + ((FinishTime - BeginTime) / 1000).toFixed(2) + " s"; }
    },
    log: function (msg1, msg2, msg3, msg4, msg5) {
        if (console != undefined) {
            if (msg2 == undefined) { console.log(msg1); }
            else if (msg3 == undefined) { console.log(msg1, msg2); }
            else if (msg4 == undefined) { console.log(msg1, msg2, msg3); }
            else if (msg5 == undefined) { console.log(msg1, msg2, msg3, msg4); }
            else if (msg5 != undefined) { console.log(msg1, msg2, msg3, msg4, msg5); }
        }
    },
});
function notiSuccess(value) { showNoti(value, "ok"); }
function notiError(value) { showNoti(value, "remove"); }
function notiInfo(value) { showNoti(value, "info"); }
function notiWarning(value) { showNoti(value, "question"); }
function showNoti(value, type) {
    var windowWidth = parseInt($(window).width());
    $("#divNoti").find(".noti-message").html(value);
    $("#divNoti").find(".sign").addClass("glyphicon-" + type + "-sign");
    //$("#divNoti").addClass("noti-" + type).css({ left: parseInt(windowWidth) - ($("#divNoti").outerWidth() + 20) });
    $("#divNoti").addClass("noti-" + type).css({ right: 20 });
    if ($("#divNoti").css('display') == 'none') {
        $("#divNoti").slideDown(function () {
            setTimeout(function () {
                $("#divNoti").slideUp(function () {
                    $(this).removeClass("noti-" + type).find(".sign").removeClass("glyphicon-" + type + "-sign");
                });
            }, 3000);
        });
    }
}
function NSOpenPopupBox(setting) {
    var config = {
        header: "Please check",
        detail: "",
        container: $("#divAlertContainer"),
        confirmFunction: function () { },
        cancelFunction: function () { },
        buildFunction: function () { },
        isAlert: true,
        autoClose: true,
        defaultFocus: "#divContent input.btn-success",
        setTop: 0,
    };
    $.extend(config, setting);
    console.log("alert container 1", $(config.container));
    if ($(config.container).NSValue().trim() == "" && $(config.container).NSAttr("id").indexOf("divAlertContainer") > -1) {
        if ($(config.container).NSAttr("id") == "divAlertContainer") { config.container = $("#divAlertContainer2"); } else { config.container = $("#divAlertContainer"); }
    }
    console.log("alert container", $(config.container));
    $(config.container).find(".popupheader").NSValue(config.header);
    $(config.container).find(".spWarning").NSValue(config.detail);
    if (config.isAlert) {
        $(config.container).find("input.btn-success").unbind("click")
            .bind("click", function () { config.confirmFunction(); })
            .closest("div")
            .removeClass("col-md-offset-18")
            .addClass("col-md-offset-27");
        $(config.container).find("input.btn-cancel").closest("div").hide()
        if (config.autoClose) { $(config.container).find("input.btn-success").bind("click", function () { return NSClosePopupBox(); }) }

    } else {
        $(config.container).find("input.btn-success").unbind("click")
            .bind("click", function () { config.confirmFunction(); })
            .closest("div")
            .removeClass("col-md-offset-27")
            .addClass("col-md-offset-18");
        $(config.container).find("input.btn-cancel").unbind("click")
            .bind("click", function () { config.cancelFunction(); return NSClosePopupBox(); }).closest("div").show();
        if (config.autoClose) { $(config.container).find("input.btn-success").bind("click", function () { return NSClosePopupBox(); }) }
    }
    $(config.container).find("input.btn-success").unbind("keydown").bind("keydown", function (ev) {
        var keycode = (ev.keyCode ? ev.keyCode : ev.which);
        if (keycode == '9') {
            config.confirmFunction();
            if (config.autoClose) { NSClosePopupBox(); }
            ev.preventDefault();
        }
    });
    if ($("#divFrame").is(":visible")) {
        $("#" + $("#divContent").NSAttr("containerID")).append($("#divContent").children());
    }
    config.buildFunction();
    if (config.setTop > 0) { $("#divDialog").addClass("settop").css({ top: config.setTop }); }
    $('html').css("overflow-y", "hidden");
    $("#divFrame").css({ height: $(window).height(), width: $(window).width() }).fadeIn();
    $("#divBG").css({ height: $(document).height(), width: $(document).width() }).fadeIn();
    $("#divContent").append($(config.container).children()).NSFill({ containerID: $(config.container).NSAttr("id") });
    $(config.defaultFocus).NSFocus();
    setWorkDesk();
}
function NSClosePopupBox() {
    $("#divFrame").fadeOut();
    $("#divBG").fadeOut();
    $("#divDialog").removeClass("settop");
    $("#" + $("#divContent").NSAttr("containerID")).append($("#divContent").children());
}
function showMsgTopPage(msg, setting) {
    let config = {
        backgroundcolor: "red",
        color: "white",
        fontweight: "inherit",
    };
    $.extend(config, setting);
    let divMsg = $("<div>", { style: "width:100%;text-align:center;" });
    $(divMsg).css("background-color", config.backgroundcolor).css("color", config.color);
    $(divMsg).append($("<span>").css("font-weight", config.fontweight).append(msg));
    $("body").prepend($(divMsg));
}

//### Paging 
function genGridPage(tbData, doFunction) {
    var totalPage = ($(tbData).attr("totalPage") || "1").toNumber(); var currentPage = ($(tbData).attr("currentPage") || "1").toNumber();
    var divpage = null; var page = "";
    divpage = $(tbData).find("div.page").NSValue("");

    var backward = currentPage == 1 ? 1 : currentPage - 1;
    var forward = currentPage < totalPage ? currentPage + 1 : totalPage;
    if (totalPage > 0) {
        $(divpage).append($("<span>", { style: "vertical-align:text-top;" }).NSValue("Page "));
        $(divpage).append($("<button>", { page: 1 }).append($("<i>", { class: "glyphicon glyphicon-fast-backward" })).click(function () { changePage($(this), doFunction); return false; }));
        $(divpage).append($("<button>", { page: backward }).append($("<i>", { class: "glyphicon glyphicon-backward" })).click(function () { changePage($(this), doFunction); return false; }));
        $(divpage).append($("<input>", { type: "text", style: "background-color: transparent;width:2.5em;margin-left: 6px;", value: currentPage }).focusin(function () { $(this).select(); }).NSNumberbox()
            .change(function () {
                if ($(this).NSValue() != "") {
                    if (!($(this).NSValue().toNumber() == 0 || $(this).NSValue().toNumber() > totalPage)) {
                        changePage($(this).NSFill({ page: $(this).NSValue() }), doFunction);
                    } else { $(this).focus(); /*notiWarning("เลขหน้าไม่ถูกต้อง");*/ alertify.alert("เลขหน้าไม่ถูกต้อง"); }
                } else { $(this).focus(); /*notiWarning("กรุณากรอกเลขหน้าที่ต้องการ");*/ alertify.alert("กรุณากรอกเลขหน้าที่ต้องการ"); }
            }).select());
        $(divpage).append($("<span>", { class: "total-page", style: "vertical-align:text-top;" }).NSValue(" / " + totalPage + " "));
        $(divpage).append($("<button>", { page: forward }).append($("<i>", { class: "glyphicon glyphicon-forward" })).click(function () { changePage($(this), doFunction); return false; }));
        $(divpage).append($("<button>", { page: totalPage }).append($("<i>", { class: "glyphicon glyphicon-fast-forward" })).click(function () { changePage($(this), doFunction); return false; }));
        if (currentPage <= 1) {
            $(divpage).find(".glyphicon-fast-backward").closest("button").prop('disabled', true);
            $(divpage).find(".glyphicon-backward").closest("button").prop('disabled', true);
        }
        if (currentPage == totalPage) {
            $(divpage).find(".glyphicon-fast-forward").closest("button").prop('disabled', true);
            $(divpage).find(".glyphicon-forward").closest("button").prop('disabled', true);
        }
    }
}
function changePage(xobj, doFunction) {
    if ($(xobj).closest("table").NSAttr("wStatus") != "working") {
        $(xobj).closest("table").NSAttr("currentPage", $(xobj).NSAttr("page"))
        doFunction(false);
    }
}
function sortButton(xobj, doFunction) {
    //ถ้าเป็นการ sort field เดิมให้ทำการเปลี่ยน direction 
    if ($(xobj).NSAttr("sortOrder") != undefined) {
        if ($(xobj).closest("table").NSAttr("wStatus") != "working") {
            if ($(xobj).NSAttr("sortOrder").toUpperCase() == $(xobj).closest("table").NSAttr("sortOrder").toUpperCase()) {
                if ($(xobj).closest("table").NSAttr("sortDirection").toLowerCase() == "asc") {
                    $(xobj).closest("table").NSFill({ sortDirection: "desc" });
                    $(xobj).find(".glyphicon").removeClass("glyphicon-triangle-top").addClass("glyphicon-triangle-bottom");
                }
                else {
                    $(xobj).closest("table").NSAttr("sortDirection", "asc");
                    $(xobj).find(".glyphicon").removeClass("glyphicon-triangle-bottom").addClass("glyphicon-triangle-top");
                }
            } else {
                //ถ้าเป็นการ sort field ใหม่ให้ทำการเปลี่ยน field และเริ่ม direction ที่ desc
                $(xobj).closest("table").NSFill({ sortDirection: "desc", sortOrder: $(xobj).NSAttr("sortOrder") });
                //ต้องทำการย้าย direct sign ไปไว้กับหัวข้อที่เลือกใหม่ด้วย
                $(xobj).closest("table")
                $(xobj).closest("table").find("thead > tr > th .glyphicon").removeClass("glyphicon-triangle-top").addClass("glyphicon-triangle-bottom")
                    .appendTo($(xobj).closest("table").find("thead button[sortOrder='" + $(xobj).NSAttr("sortOrder") + "']"));
            }
            doFunction(false, xobj);
        }
    }
}
//### Paging 
Storage.prototype.setObject = function (key, value) {
    this.setItem(key, JSON.stringify(value));
}
Storage.prototype.getObject = function (key) {
    var value = this.getItem(key);
    return value && JSON.parse(value);
}
//### Session timed out
var _Timer;
var _UrlTimeout = 'default.aspx';
function SessionExpire() {
    NSOpenPopupBox({
        header: "Session timeout",
        detail: "Your Session has timed out.<br>Please press OK if you want to stay this page.",
        isAlert: false,
        confirmFunction: function () {
            _Timer = setTimeout('SessionExpire();', 1200000);
        },
        cancelFunction: function () {
            var targetUrl = "../../login.aspx";
            var form = '<form action="' + targetUrl + '" name="finishPage" method="post" style="display:none;">';
            form += '</form>';
            $(form).appendTo('body').appendTo("body").submit();
        },
    });
}
//### Session timed out
function getSelectBoxs(sql, selectId, defaultdata, textCode) {
    var deferred = $.Deferred();
    $.ajax({
        url: '../../ajaxAction/masterAction.aspx',
        data: NS.ajaxData({
            action: 'getSelectBoxList',
            sql: sql
        }).config,
        type: "POST",
        dataType: "json",
        error: function (xhr, s, err) {
            console.log(s, err);
        },
        success: function (data) {
            if (!data.onError) {
                data.getItems = jQuery.parseJSON(data.getItems);
                //console.log(" ITEM = ", data.getItems);
                for (var i = 0; i < data.getItems.length; i++) {
                    $("#" + selectId).append($(
                        "<option value='" + data.getItems[i].value +
                        "' valueID='" + data.getItems[i].valueID +
                        "' code='" + data.getItems[i].code + "'>"
                        + data.getItems[i].text + "</option>"));
                }
                if (defaultdata) {
                    $("#" + selectId).val(defaultdata);
                }
                if (textCode != "") {
                    $("#" + textCode).val($("#" + selectId + " option:selected").attr("code"));
                }
                deferred.resolve("Ok");
            } else {
                console.log("Error = ", data.exMessage);
                deferred.reject("Error");
            }
        }
    });
    return deferred.promise();
}
function sortButtonByArry(xobj, doFunction) {
    //ถ้าเป็นการ sort field เดิมให้ทำการเปลี่ยน direction 
    if ($(xobj).NSAttr("sortOrder") != undefined) {
        if ($(xobj).closest("table").NSAttr("wStatus") != "working") {
            if ($(xobj).NSAttr("sortOrder") == $(xobj).closest("table").NSAttr("sortOrder")) {
                if ($(xobj).closest("table").NSAttr("sortDirection") == "asc") {
                    $(xobj).closest("table").NSFill({ sortDirection: "desc" });
                    $(xobj).find(".glyphicon").removeClass("glyphicon-triangle-top").addClass("glyphicon-triangle-bottom");
                }
                else {
                    $(xobj).closest("table").NSAttr("sortDirection", "asc");
                    $(xobj).find(".glyphicon").removeClass("glyphicon-triangle-bottom").addClass("glyphicon-triangle-top");
                }
            } else {
                //ถ้าเป็นการ sort field ใหม่ให้ทำการเปลี่ยน field และเริ่ม direction ที่ desc
                $(xobj).closest("table").NSFill({ sortDirection: "desc", sortOrder: $(xobj).NSAttr("sortOrder") });
                //ต้องทำการย้าย direct sign ไปไว้กับหัวข้อที่เลือกใหม่ด้วย
                $(xobj).closest("table")
                $(xobj).closest("table").find("thead > tr > th .glyphicon").removeClass("glyphicon-triangle-top").addClass("glyphicon-triangle-bottom")
                    .appendTo($(xobj).closest("table").find("thead button[sortOrder='" + $(xobj).NSAttr("sortOrder") + "']"));
            }
            doFunction(false, xobj);
        }
    }
}
var THAISMILEPath = function () {
    var local = "";
    return local;
}
var popupWindow = function (urlpath) {
    console.log(urlpath);
    var d = new Date();
    var onHandler = 'onHandlerWindows_' + (d.getTime() / 1000);
    var jWin = $(parent.window);
    var w = Math.round(jWin.width() * 0.8, 0);
    var h = Math.round(jWin.height() * 0.85, 0);
    var spec = ',width=' + w + ',height=' + h + ',left=' + Math.round((jWin.width() - w) / 2, 0) + ',top=' + Math.round((jWin.height() - h) / 2, 0);
    var f = $('<form>', { "id": "onHandler", "name": "onHandler", "method": (typeof (urlpath) === "object") ? 'post' : 'get', "target": onHandler });
    if (typeof (urlpath) === "object") {
        $.each(urlpath, function (key, value) { f.append($('<input name="' + key + '" id="' + key + '" type="hidden" value="' + value + '"/>')); });
        urlpath = urlpath.url;
    }
    f.attr('action', (urlpath.indexOf('http://') > -1) ? urlpath : THAISMILEPath() + urlpath);

    f.appendTo(document.body).submit(function () {
        var handler = (typeof onHandler !== 'undefined') ? onHandler : "onHandlerWindows"
        try {
            var win = window.open('about:blank', handler, 'toolbar=no,scrollbars=no,location=no,statusbar=no,menubar=no,resizable=1' + spec, true);
            if (!win || win.closed || typeof win.closed == 'undefined') {
                //POPUP BLOCKED
                alert("Popup Blocker is enabled! Please add this site to your exception list and click print again.");
            }
        }
        catch (err) { /*FIXED IE BROWSER*/ }
    }).appendTo("body").submit().remove();
}
var downloadData = function (urlpath) {
    console.log(urlpath);
    var d = new Date();
    var onHandler = 'onHandlerWindows_' + (d.getTime() / 1000);
    var jWin = $(parent.window);
    var w = Math.round(jWin.width() * 0.8, 0);
    var h = Math.round(jWin.height() * 0.85, 0);
    var spec = ',width=' + w + ',height=' + h + ',left=' + Math.round((jWin.width() - w) / 2, 0) + ',top=' + Math.round((jWin.height() - h) / 2, 0);
    var f = $('<form>', { "id": "onHandler", "name": "onHandler", "method": (typeof (urlpath) === "object") ? 'post' : 'get', "target": onHandler });
    if (typeof (urlpath) === "object") {
        $.each(urlpath, function (key, value) { f.append($('<input name="' + key + '" id="' + key + '" type="hidden" value="' + value + '"/>')); });
        urlpath = urlpath.url;
    }
    f.attr('action', (urlpath.indexOf('http://') > -1) ? urlpath : THAISMILEPath() + urlpath);

    f.appendTo(document.body).submit(function () {
        var handler = (typeof onHandler !== 'undefined') ? onHandler : "onHandlerWindows"
        try {
            var win = window.parent('about:blank', handler, 'toolbar=0,scrollbars=0,location=0,statusbar=0,menubar=0,resizable=1' + spec, true);
            if (!win || win.closed || typeof win.closed == 'undefined') {
                //POPUP BLOCKED
                alert("Popup Blocker is enabled! Please add this site to your exception list and click print again.");
            }
        }
        catch (err) { /*FIXED IE BROWSER*/}
    }).appendTo("body").submit().remove();
}
var tableSorter = function () {
    console.log("tableSorter");
}
// ### popup login key
function checkUserChange(setting) {
    var config = {
        //thisControl: null,
        confirmFunction: function () { },
        cancelFunction: function () { },
        autoClose: true,
    };
    $.extend(config, setting);
    var checkLogin = function (doFunc) {
        if ($("#txtUserCheck").NSValue() == "") {
            notiError("Please enter username");
            $("#txtUserCheck").NSFocus()
        } else if ($("#txtPasswordCheck").NSValue() == "") {
            notiError("Please enter password");
            $("#txtPasswordCheck").NSFocus()
        } else {
            $("#txtUserCheck").closest("div.popupbody").find("input.btn-success").NSDisable();
            $.ajax({
                url: '../ajaxAction/donorAction.aspx', //userAction.aspx
                data: { action: 'userchangesample', user: $("#txtUserCheck").NSValue(), password: $("#txtPasswordCheck").NSValue() }, //check
                type: "POST", dataType: "json",
                error: function (xhr, s, err) { console.log(s, err); $("#txtUserCheck").closest("div.popupbody").find("input.btn-success").NSEnable(); },
                success: function (data) {
                    if (!data.onError) {
                        doFunc();
                        $("#txtPasswordCheck").NSValue();
                        if (config.autoClose) { NSClosePopupBox(); }
                    } else {
                        notiError(data.exTitle);
                        $("#txtPasswordCheck").NSFocus();
                    }
                    $("#txtUserCheck").closest("div.popupbody").find("input.btn-success").NSEnable();
                }
            });    //End ajax
        }
    }
    $("#divUserContainer").find("input.btn-success")
        .unbind("click").bind("click", function () { checkLogin(config.confirmFunction); })
        .unbind("keydown").bind("keydown", function (ev) {
            var keycode = (ev.keyCode ? ev.keyCode : ev.which);
            if (keycode == '9') {
                checkLogin(config.confirmFunction);
                ev.preventDefault();
            }
        });
    $("#divUserContainer").find("input.btn-cancel")
        .unbind("click").bind("click", function () {
            $('#txtPasswordCheck').NSValue('');
            config.cancelFunction()
            return NSClosePopupBox();
        });
    openPopup($("#divUserContainer"));
    $('#txtPasswordCheck').NSFocus().NSValue('');
}
function openPopup(container) {
    $("#" + $("#divContent").NSAttr("containerID")).append($("#divContent").children());
    $('html').css("overflow-y", "hidden");
    $("#divFrame").css({ height: $(window).height(), width: $(window).width() }).fadeIn();
    $("#divBG").css({ height: $(document).height(), width: $(document).width() }).fadeIn();
    $("#divContent").append($(container).children()).NSFill({ containerID: $(container).NSAttr("id") }).find("input:not(input[type=button],input[type=submit],button):visible:first").focus();
    setWorkDesk();
}
function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function delete_cookie(name) {
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    window.open("login.aspx", "_self")
}
function checkCookie() {
    if (getCookie("user") == "") alertify.alert("กรุณา Login เข้าระบบก่อนทำรายการ", function () { window.open("login.aspx", "_self"); })
}

//วุดเพิ่ม 
function toDate(s) {
    if (!s) {
        return ''
    }
    if (s instanceof Date) {
        var y = s.getFullYear()
        var m = s.getMonth()
        var d = s.getDate()
        return new Date(y - (y > 2400 && y < 9000 ? 543 : 0), m, d)
    }
    var a = s.split('/')
    var y = parseInt(a[2])
    var m = parseInt(a[1]) - 1
    var d = parseInt(a[0])

    if (y > 2400 && y < 9000) {
        y -= 543
    }

    return new Date(y, m, d)
}
var MONTH_NAMES = new Array('January', 'Febuary', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'); var DAY_NAMES = new Array('อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสบดี', 'ศุกร์', 'เสาร์', 'อา.', 'จ.', 'อ.', 'พ.', 'พฤ.', 'ศ.', 'ส.');
function LZ(x) { return (x < 0 || x > 9 ? "" : "0") + x }
function isDate(val, format) { var date = getDateFromFormat(val, format); if (date == 0) { return false; } return true; }
function compareDates(date1, dateformat1, date2, dateformat2) { var d1 = getDateFromFormat(date1, dateformat1); var d2 = getDateFromFormat(date2, dateformat2); if (d1 == 0 || d2 == 0) { return -1; } else if (d1 > d2) { return 1; } return 0; }
function formatDate(date, format) { format = format + ""; var result = ""; var i_format = 0; var c = ""; var token = ""; var y = date.getFullYear() + ""; var M = date.getMonth() + 1; var d = date.getDate(); var E = date.getDay(); var H = date.getHours(); var m = date.getMinutes(); var s = date.getSeconds(); var yyyy, yy, MMM, MM, dd, hh, h, mm, ss, ampm, HH, H, KK, K, kk, k; var value = new Object(); if (y.length < 4) { y = "" + (y - 0 + 1900); } value["y"] = "" + y; value["yyyy"] = y; value["yy"] = y.substring(2, 4); value["M"] = M; value["MM"] = LZ(M); value["MMM"] = MONTH_NAMES[M - 1]; value["NNN"] = MONTH_NAMES[M + 11]; value["d"] = d; value["dd"] = LZ(d); value["E"] = DAY_NAMES[E + 7]; value["EE"] = DAY_NAMES[E]; value["H"] = H; value["HH"] = LZ(H); if (H == 0) { value["h"] = 12; } else if (H > 12) { value["h"] = H - 12; } else { value["h"] = H; } value["hh"] = LZ(value["h"]); if (H > 11) { value["K"] = H - 12; } else { value["K"] = H; } value["k"] = H + 1; value["KK"] = LZ(value["K"]); value["kk"] = LZ(value["k"]); if (H > 11) { value["a"] = "PM"; } else { value["a"] = "AM"; } value["m"] = m; value["mm"] = LZ(m); value["s"] = s; value["ss"] = LZ(s); while (i_format < format.length) { c = format.charAt(i_format); token = ""; while ((format.charAt(i_format) == c) && (i_format < format.length)) { token += format.charAt(i_format++); } if (value[token] != null) { result = result + value[token]; } else { result = result + token; } } return result; }
function _isInteger(val) { var digits = "1234567890"; for (var i = 0; i < val.length; i++) { if (digits.indexOf(val.charAt(i)) == -1) { return false; } } return true; }
function _getInt(str, i, minlength, maxlength) { for (var x = maxlength; x >= minlength; x--) { var token = str.substring(i, i + x); if (token.length < minlength) { return null; } if (_isInteger(token)) { return token; } } return null; }
function getDateFromFormat(val, format) { val = val + ""; format = format + ""; var i_val = 0; var i_format = 0; var c = ""; var token = ""; var token2 = ""; var x, y; var now = new Date(); var year = now.getFullYear(); var month = now.getMonth() + 1; var date = 1; var hh = now.getHours(); var mm = now.getMinutes(); var ss = now.getSeconds(); var ampm = ""; while (i_format < format.length) { c = format.charAt(i_format); token = ""; while ((format.charAt(i_format) == c) && (i_format < format.length)) { token += format.charAt(i_format++); } if (token == "yyyy" || token == "yy" || token == "y") { if (token == "yyyy") { x = 4; y = 4; } if (token == "yy") { x = 2; y = 2; } if (token == "y") { x = 2; y = 4; } year = _getInt(val, i_val, x, y); if (year == null) { return 0; } i_val += year.length; if (year.length == 2) { if (year > 70) { year = 1900 + (year - 0); } else { year = 2000 + (year - 0); } } } else if (token == "MMM" || token == "NNN") { month = 0; for (var i = 0; i < MONTH_NAMES.length; i++) { var month_name = MONTH_NAMES[i]; if (val.substring(i_val, i_val + month_name.length).toLowerCase() == month_name.toLowerCase()) { if (token == "MMM" || (token == "NNN" && i > 11)) { month = i + 1; if (month > 12) { month -= 12; } i_val += month_name.length; break; } } } if ((month < 1) || (month > 12)) { return 0; } } else if (token == "EE" || token == "E") { for (var i = 0; i < DAY_NAMES.length; i++) { var day_name = DAY_NAMES[i]; if (val.substring(i_val, i_val + day_name.length).toLowerCase() == day_name.toLowerCase()) { i_val += day_name.length; break; } } } else if (token == "MM" || token == "M") { month = _getInt(val, i_val, token.length, 2); if (month == null || (month < 1) || (month > 12)) { return 0; } i_val += month.length; } else if (token == "dd" || token == "d") { date = _getInt(val, i_val, token.length, 2); if (date == null || (date < 1) || (date > 31)) { return 0; } i_val += date.length; } else if (token == "hh" || token == "h") { hh = _getInt(val, i_val, token.length, 2); if (hh == null || (hh < 1) || (hh > 12)) { return 0; } i_val += hh.length; } else if (token == "HH" || token == "H") { hh = _getInt(val, i_val, token.length, 2); if (hh == null || (hh < 0) || (hh > 23)) { return 0; } i_val += hh.length; } else if (token == "KK" || token == "K") { hh = _getInt(val, i_val, token.length, 2); if (hh == null || (hh < 0) || (hh > 11)) { return 0; } i_val += hh.length; } else if (token == "kk" || token == "k") { hh = _getInt(val, i_val, token.length, 2); if (hh == null || (hh < 1) || (hh > 24)) { return 0; } i_val += hh.length; hh--; } else if (token == "mm" || token == "m") { mm = _getInt(val, i_val, token.length, 2); if (mm == null || (mm < 0) || (mm > 59)) { return 0; } i_val += mm.length; } else if (token == "ss" || token == "s") { ss = _getInt(val, i_val, token.length, 2); if (ss == null || (ss < 0) || (ss > 59)) { return 0; } i_val += ss.length; } else if (token == "a") { if (val.substring(i_val, i_val + 2).toLowerCase() == "am") { ampm = "AM"; } else if (val.substring(i_val, i_val + 2).toLowerCase() == "pm") { ampm = "PM"; } else { return 0; } i_val += 2; } else { if (val.substring(i_val, i_val + token.length) != token) { return 0; } else { i_val += token.length; } } } if (i_val != val.length) { return 0; } if (month == 2) { if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) { if (date > 29) { return 0; } } else { if (date > 28) { return 0; } } } if ((month == 4) || (month == 6) || (month == 9) || (month == 11)) { if (date > 30) { return 0; } } if (hh < 12 && ampm == "PM") { hh = hh - 0 + 12; } else if (hh > 11 && ampm == "AM") { hh -= 12; } var newdate = new Date(year, month - 1, date, hh, mm, ss); return newdate.getTime(); }
function parseDate(val) { var preferEuro = (arguments.length == 2) ? arguments[1] : false; generalFormats = new Array('y-M-d', 'MMM d, y', 'MMM d,y', 'y-MMM-d', 'd-MMM-y', 'MMM d'); monthFirst = new Array('M/d/y', 'M-d-y', 'M.d.y', 'MMM-d', 'M/d', 'M-d'); dateFirst = new Array('d/M/y', 'd-M-y', 'd.M.y', 'd-MMM', 'd/M', 'd-M'); var checkList = new Array('generalFormats', preferEuro ? 'dateFirst' : 'monthFirst', preferEuro ? 'monthFirst' : 'dateFirst'); var d = null; for (var i = 0; i < checkList.length; i++) { var l = window[checkList[i]]; for (var j = 0; j < l.length; j++) { d = getDateFromFormat(val, l[j]); if (d != 0) { return new Date(d); } } } return null; }
function formatDateToString(date) {
    // 01, 02, 03, ... 29, 30, 31
    var dd = (date.getDate() < 10 ? '0' : '') + date.getDate();
    // 01, 02, 03, ... 10, 11, 12
    var MM = ((date.getMonth() + 1) < 10 ? '0' : '') + (date.getMonth() + 1);
    // 1970, 1971, ... 2015, 2016, ...
    var yyyy = date.getFullYear();
    // create the format you want
    return (dd + "-" + MM + "-" + yyyy);
}