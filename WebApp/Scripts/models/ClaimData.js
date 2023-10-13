function toggleEdit() {
    if (!readOnly) {
        $('.toggleView').toggle();
        editMode = true;
    }
    $('#editButton').prop('disabled', true);
}

function focusOnDate(val) {
    $('#' + val).click();
}

function toggleRowView() {
    var search = $('#SearchProperty').val();
    var filter = search.toUpperCase();
    var hasValueOnly = $('#HasValue').prop("checked");
    var hasErrorOnly = $('#HasError').prop("checked");

    $("table tbody tr").each(function (index) {
        row = $(this);

        var code = row.find("td:first").text();
        var error = row.find("td:nth-last-child(1)").text();

        var value = '';
        var valueCol = row.find("td:nth-last-child(2)");
        if (editMode) {
            var input = valueCol.find("div:last-child").find("input");
            var select = valueCol.find("div:last-child").find("select option:selected");

            if (input.length != 0) {
                value = input.val();
            } else if (select.length != 0) {
                value = select.text();
            }

            if (typeof (value) == 'undefined')
                value = '';
            value.trim();
        } else {
            value = valueCol.find("div:first").text().trim();
        }

        if (hasErrorOnly == true && error.trim() === '') {
            row.hide();
        } else if (hasValueOnly == true && value === '') {
            row.hide();
        } else if (code.toUpperCase().indexOf(filter) > -1) {
            row.show();
        } else {
            row.hide();
        }
    });
}

$(document).ready(function () {
    dateOffAutoComplete();

    $(".datepicker").click(function () {
        if ($(this).data("datepicker") == null) {
            $(this).datepicker({
                format: DateFormatDatePickerJs,
                autoclose: true,
            });
        }
        $(this).focus();
    });

    if (readOnly) {
        disableFields();
    }
});