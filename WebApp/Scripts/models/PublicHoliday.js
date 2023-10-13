
var app = new Vue({
    el: '#app',
    data: {
        PublicHoliday: PublicHolidayModel,
        PublicHolidayDetails: PublicHolidayDetails,
        SortIndex: 0,
    },
    methods: {
        addNew: function () {
            this.SortIndex++;
            this.PublicHolidayDetails.push({ SortIndex: this.SortIndex });
        },
        removePublicHolidayDetail: function (index) {
            this.PublicHolidayDetails.splice(index, 1);
            this.SortIndex--;
        },
        autoExpandTextarea: function (id) {
            var tArea = $('#' + id);
            this.textAreaWidth = tArea.width();
            this.textAreaHeight = tArea.height();
            tArea.autoResize();
            tArea.trigger('keyup');

            tArea.on('keypress', function (evt) {
                var evt = (evt) ? evt : ((event) ? event : null);
                if (evt.keyCode == 13)
                    return false;
            });
        },
        autoCollapseTextarea: function (id) {
            var tArea = $('#' + id);
            tArea.height(this.textAreaHeight);
        },
        openDatePicker: function (currentId) { 
            var idStr = currentId.split("_");
            var type = idStr[0];
            var field = idStr[1];
            var index = idStr[2];

            var id = "#" + currentId;
            if ($(id).data("datepicker") != null) {
                $(id).datepicker("destroy");
            }

            // Display calendar based on selected year
            var year = $('#YearStr').val();
            if (year === '' || year === null) {
                year = new Date().getFullYear();
            }

            $(id).datepicker({
                format: DateFormatDatePickerJs,
                autoclose: true,
                startDate: new Date(year, '0', '01'),
                endDate: new Date(year, '11', '31'),
            });

            var updateDateValue = this.updateDateValue;
            $(id).on('changeDate', function () {
                updateDateValue(type, field, index, $(id).val());
            });

            $(id).focus();
        },
        updateDateValue: function (type, field, index, value) {
            var typeStr = type + "s";
            this[typeStr][index][field] = value;
        },
    },
});

$(document).ready(function () {

    $("#YearStr").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });
    //.on('changeDate', function (e) {
    //    console.log($(this).val());
    //});
});

function focusOnDate(val) {
    $('#' + val).focus();
}