
$(document).ready(function () {
    dateOffAutoComplete();

    $('#InsuredDateOfBirthStr').datepicker({
        format: DateFormatDatePickerJs,
    });

    if (model.CedantId)
        getTreatyCode(model.CedantId, model.TreatyCodeId);
});

function getTreatyCode(selectedCedantId, selectedTreatyCodeId = null) {
    $.ajax({
        url: getTreatyCodeByCedantUrl,
        type: "POST",
        data: {
            CedantId: selectedCedantId,
            Status: treatyCodeActive,
            SelectedId: selectedTreatyCodeId,
            foreign: false
        },
        cache: false,
        async: false,
        success: function (data) {
            var TreatyCodes = data.TreatyCodes;
            refreshDropDownItems('TreatyCodeId', TreatyCodes, selectedTreatyCodeId, 'Code', 'Description')
        }
    });
}

function focusOnDate(val) {
    $('#' + val).focus();
}