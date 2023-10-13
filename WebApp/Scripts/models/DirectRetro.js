$(document).ready(function () {
    if (!$('input').is('[readonly]')) {
        $('#SoaQuarter').datepicker({
            format: QuarterFormat,
            minViewMode: 1,
            autoclose: true,
            language: "qtrs",
            forceParse: false
        }).on('show', function (e) {
            var datepickerDropDown = $('.datepicker');
            datepickerDropDown.addClass('quarterpicker');
        });
    }

    refreshTreatyCode(SelectedCedantId);
});

function validateQuarter(qtr) {
    if (qtr == null || qtr == "")
        return;
    if (!/[0-9]{4} Q{1}([1-4]){1}/.test(qtr)) {
        $("#SoaQuarter").val(null);
        alert("SOA Quarter format is incorrect");
    }
    getSoaDataBatch();
}

function refreshTreatyCode(selectedCedantId) {
    $.ajax({
        url: GetTreatyCodeByCedantUrl,
        type: "POST",
        data: {
            CedantId: selectedCedantId,
            indexItem: false,
        },
        cache: false,
        async: false,
        success: function (data) {
            var TreatyCodes = data.TreatyCodes;

            $('#TreatyCodeId option').remove();
            $('#TreatyCodeId').append(new Option("Please select", ""));
            TreatyCodes.forEach(function (obj) {
                var text = obj.Code + ' - ' + obj.TreatyBo.TreatyIdCode;
                $('#TreatyCodeId').append(new Option(text, obj.Id, false, obj.Id == SelectedTreatyCodeId));
            });
            $('#TreatyCodeId').selectpicker('refresh');
        }
    });
    getSoaDataBatch();
}

function changeTreatyCode() {
    getSoaDataBatch();
}

function getSoaDataBatch() {
    var cedantId = $('#CedantId').val();
    var treatyCodeId = $('#TreatyCodeId').val();
    var soaQuarter = $('#SoaQuarter').val();

    var selectedId = SelectedSoaDataBatchId;

    if (!cedantId || !treatyCodeId || !soaQuarter) {
        $('#SoaDataBatchId option').remove();
        $('#SoaDataBatchId').append(new Option("Please select", ""));
        $('#SoaDataBatchId').selectpicker('refresh');
    } else {
        $.ajax({
            url: GetSoaDataBatchUrl,
            type: "POST",
            data: {
                cedantId: cedantId,
                treatyCodeId: treatyCodeId,
                soaQuarter: soaQuarter,
            },
            cache: false,
            async: false,
            success: function (data) {
                var SoaDataBatches = data.SoaDataBatches;

                $('#SoaDataBatchId option').remove();
                $('#SoaDataBatchId').append(new Option("Please select", ""));
                SoaDataBatches.forEach(function (obj) {
                    var text = obj.Id + ' - ' + obj.Quarter;
                    if (obj.IsProfitCommissionData) {
                        text = text + ' - ' + "Profit Comm";
                    }
                    $('#SoaDataBatchId').append(new Option(text, obj.Id, false, obj.Id == selectedId));
                });
                $('#SoaDataBatchId').selectpicker('refresh');
            }
        });
    }
}

var app = new Vue({
    el: '#app',
    data: {
        // Models
        DirectRetro: DirectRetroModel,
        // Listing
        RetroSummaries: RetroSummaries ? RetroSummaries : [],
        TotalRetroSummary: TotalRetroSummary ? TotalRetroSummary : {},
        RetroStatements: RetroStatements ? RetroStatements : [],
        StatusHistories: StatusHistories ? StatusHistories : [],
        StatusFiles: StatusFiles ? StatusFiles : [],
        // Others
        Disabled: Disabled,
    },
    methods: {
        logFile: function (index) {
            var item = this.StatusHistories[index];
            if (item != null && item.Id != '') {
                return this.StatusFiles.filter(function (statusFileItem) {
                    return statusFileItem.StatusHistoryId === item.Id;
                })
            }
        },
        urlDownloadLink: function (index) {
            var item = this.StatusHistories[index];
            if (item != null && item.Id != '') {
                var url = DownloadStatusFileUrl + item.Id;
                return url
            }
        },
        retroStatementLink: function (index) {
            var item = this.RetroStatements[index];
            if (item != null && item.Id != '') {
                var url = RetroStatementUrl + item.Id;
                return url
            }
        },
    },
    created: function () {

    },
});

function submitForProcessing() {
    $('#RetroStatus').val(RetroStatusSubmitForProcessing);
    $('form').submit();
}

function submitForApproval() {
    $('#RetroStatus').val(RetroStatusPendingApproval);
    $('form').submit();
}

function approveDirectRetro() {
    $('#RetroStatus').val(RetroStatusApproved);
    $('form').submit();
}

function rejectDirectRetro() {
    $('#RetroStatus').val(RetroStatusCompleted);
    $('form').submit();
}