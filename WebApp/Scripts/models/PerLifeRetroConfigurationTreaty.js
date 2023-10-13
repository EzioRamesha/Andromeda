$(document).ready(function () {
    dateOffAutoComplete();

    $('#ReinsEffectiveStartDate, #ReinsEffectiveEndDate, #RiskQuarterStartDate, #RiskQuarterEndDate').datepicker({
        format: DateFormatDatePickerJs,
        autoclose: true,
    });
});

function focusOnDate(val) {
    $('#' + val).focus();
}

function changeTreatyCode(val) {
    var treatyCode = null;
    $.ajax({
        url: FindTreatyCodeUrl,
        type: "POST",
        data: {
            treatyCodeId: val,
        },
        cache: false,
        async: false,
        success: function (data) {
            treatyCode = data.TreatyCodeBo;
        }
    });

    if (treatyCode != null) {
        $('#TreatyTypePickListDetailId').val(treatyCode.TreatyTypePickListDetailId);
    } else {
        $('#TreatyTypePickListDetailId').val("");
    }

    $("#TreatyTypePickListDetailId").selectpicker('refresh');
}