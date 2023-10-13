var cedingPlanCodeCount = 0;
var tokenfieldReady = {
    cedingPlanCode: false,
};

$(document).ready(function () {

    $('#cedingPlanCodeTokenField').on('tokenfield:createtoken', function (e) {
        var existingTokens = $(this).tokenfield('getTokens');
        $.each(existingTokens, function (el, token) {
            if (token.value === e.attrs.value) {
                e.preventDefault();
            }
        });
    })
        .on('tokenfield:createdtoken', function (e) {
            cedingPlanCodeCount += 1;
            $("#cedingPlanCodeTokenField-tokenfield").removeAttr('placeholder');
        })
        .on('tokenfield:removedtoken', function (e) {
            cedingPlanCodeCount -= 1;
            if (cedingPlanCodeCount == 0) {
                $("#cedingPlanCodeTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })
        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.cedingPlanCode = true;
            var isReady = true;
            Object.keys(tokenfieldReady).forEach(function (key) {
                if (tokenfieldReady[key] == false) {
                    isReady = false;
                }
            });
        })
        .tokenfield({
            autocomplete: {
                source: app.CedingPlanCodes,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });

});

var app = new Vue({
    el: '#app',
    data: {
        CedantDetails: CedantDetailsList ? CedantDetailsList : [],
        Mfrs17TreatyCodeDetails: Mfrs17TreatyCodeDetailsList ? Mfrs17TreatyCodeDetailsList : [],
        Remarks: RemarksList ? RemarksList : [],
        RemarkModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            Status: StatusPending ? StatusPending : null,
            CreatedAtStr: null,
            Content: null,
        },
        Mfrs17Reporting: Mfrs17ReportingModel,
        StatusHistories: StatusHistoriesList ? StatusHistoriesList : [],
        TotalRecord: TotalRecord,
        Files: Mfrs17ReportingFilesList ? Mfrs17ReportingFilesList : [],
        IsMultipleFileExist: IsMultipleFileExist,

        //Cedant Data Max Index
        CedantDetailMaxIndex: 0,

        NewCedantData: {
            cedantId: null,
            treatyCode: "",
            paymentMode: "",
            riskQuarter: null,
            riskQuarterMonth: "",
            cedingPlanCode: "",
        },
        NewCedantDetail: null,
        CedantDataValidation: [],

        DropDownCedants: DropDownCedants,
        DropDownTreatyCodes: DropDownEmpty,
        DropDownRiskQuarterMonths: DropDownEmpty,
        DropDownCedingPlanCodes: DropDownEmpty,
        DropDownPaymentModes: DropDownPaymentModes,
        DropDownEmpty: DropDownEmpty,

        CedingPlanCodes: [],

        isCheckAll: false,
        selectedCheck: [],

        textAreaWidth: 150,
        textAreaHeight: 21,
    },
    methods: {
        // Cedant Data
        addNew: function () {
            this.CedantDetailMaxIndex++;
        },
        reprocessCedantDetail: function (index) {
            var resetDates = false;
            if (this.CedantDetails[index].Status == StatusDeleted)
                resetDates = true;

            this.CedantDetails[index].OldStatus = this.CedantDetails[index].Status;
            this.CedantDetails[index].Status = StatusReprocess;

            if (resetDates) {
                var treatyCode = this.CedantDetails[index].TreatyCode;
                var premiumFrequencyCodePickListDetailId = this.CedantDetails[index].PremiumFrequencyCodePickListDetailId;
                this.resetLatestDates(treatyCode, premiumFrequencyCodePickListDetailId);
            }
        },
        deleteCedantDetail: function (index) {
            var treatyCode = this.CedantDetails[index].TreatyCode;
            var premiumFrequencyCodePickListDetailId = this.CedantDetails[index].PremiumFrequencyCodePickListDetailId;
            if (this.CedantDetails[index].Status == StatusPending) {
                this.CedantDetails.splice(index, 1);
                this.CedantDetailMaxIndex--;
            } else {
                this.CedantDetails[index].OldStatus = this.CedantDetails[index].Status;
                this.CedantDetails[index].Status = StatusPendingDelete;
            }
            this.resetLatestDates(treatyCode, premiumFrequencyCodePickListDetailId);
        },
        revertCedantDetail: function (index) {
            var resetDates = false;
            if (this.CedantDetails[index].Status == StatusPendingDelete)
                resetDates = true;

            this.CedantDetails[index].Status = this.CedantDetails[index].OldStatus;
            this.CedantDetails[index].OldStatus = null;

            if (resetDates) {
                var treatyCode = this.CedantDetails[index].TreatyCode;
                var premiumFrequencyCodePickListDetailId = this.CedantDetails[index].PremiumFrequencyCodePickListDetailId;
                this.resetLatestDates(treatyCode, premiumFrequencyCodePickListDetailId);
            }
        },
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.Status = this.Mfrs17Reporting.Status == 0 ? StatusPending : this.Mfrs17Reporting.Status;
            this.RemarkModal.ModuleId = this.Mfrs17Reporting.ModuleId;
            this.RemarkModal.ObjectId = this.Mfrs17Reporting.Id;
            this.RemarkModal.Content = null;
        },
        addRemark: function () {
            var remark = null;
            if (!this.Mfrs17Reporting.Id) {
                remark = this.RemarkModal;
                remark.StatusName = StatusPendingName ? StatusPendingName : null;
            } else {
                remark = createRemark(this.RemarkModal);
            }

            if (remark) {
                this.Remarks.unshift(Object.assign({}, remark));
            }
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
        downloadFile: function (index) {
            var item = this.Files[index];
            var url = FileDownloadUrl + '?quarter=' + Mfrs17ReportingModel.Quarter + '&type=' + item.SubFolder + '&fileName=' + item.FileName;
            return url
        },
        getTreatyCodeByCedant: function () {
            var cedantId = this.NewCedantData.cedantId;
            var treatyCodes = this.DropDownEmpty;
            $.ajax({
                url: getTreatyCodeByCedantUrl,
                type: "POST",
                data: {
                    CedantId: cedantId,
                    SelectedId: null,
                    Status: null,
                },
                cache: false,
                async: false,
                success: function (data) {
                    treatyCodes = data.TreatyCodes;
                }
            });
            this.NewCedantData.treatyCode = "";
            this.DropDownTreatyCodes = treatyCodes;

            this.NewCedantData.cedingPlanCode = "";
            this.CedingPlanCodes = [];

            this.$nextTick(function () {
                $("#treatyCode").selectpicker('refresh');
                $('#cedingPlanCodeTokenField').data('bs.tokenfield').$input.autocomplete({ source: [] });
                $('#cedingPlanCodeTokenField').tokenfield('setTokens', []);
            })
        },
        getMonthByRiskQuarter: function () {
            this.NewCedantData.riskQuarterMonth = "";
            var riskQuarter = this.NewCedantData.riskQuarter;
            var riskQuarterMonths = this.DropDownEmpty;

            $.ajax({
                url: getMonthByRiskQuarterUrl,
                type: "POST",
                data: {
                    riskQuarter: riskQuarter
                },
                cache: false,
                async: false,
                success: function (data) {
                    riskQuarterMonths = data.RiskQuarterMonths;
                }
            });
            this.DropDownRiskQuarterMonths = riskQuarterMonths;
            var lastMonth = riskQuarterMonths[2] ? riskQuarterMonths[2].Value : "";
            this.NewCedantData.riskQuarterMonth = lastMonth;

            this.$nextTick(function () {
                $("#riskQuarterMonth").selectpicker('refresh');
            })
        },
        openRiskQuarterPicker: function (currentId) {
            var id = "#" + currentId;
            if ($(id).data("datepicker") != null) {
                $(id).datepicker("destroy");
            }

            $(id).datepicker({
                format: RiskQuarterFormat,
                minViewMode: 1,
                autoclose: true,
                language: "qtrs",
                forceParse: false
            }).on('show', function (e) {
                var datepickerDropDown = $('.datepicker');
                datepickerDropDown.addClass('quarterpicker');
            });

            var updateRiskQuarterValue = this.updateRiskQuarterValue;
            $(id).on('changeDate', function () {
                updateRiskQuarterValue(currentId);
            });

            $(id).focus();
        },
        updateRiskQuarterValue: function (currentId) {
            var id = "#" + currentId;
            var value = $(id).val();

            this.NewCedantData[currentId] = value;

            if (this.NewCedantData.paymentMode != null &&
                this.NewCedantData.paymentMode != "" &&
                this.NewCedantData.paymentMode != 0) {
                var paymentModeCode = PaymentModes[this.NewCedantData.paymentMode];
                if (paymentModeCode == 'M') {
                    this.getMonthByRiskQuarter();
                }
            }
        },
        resetModalData: function () {
            this.CedantDataValidation = [];
            this.NewCedantData = {
                cedantId: null,
                treatyCode: "",
                paymentMode: "",
                riskQuarter: null,
                riskQuarterMonth: "",
                cedingPlanCode: "",
            }
            this.NewCedantDetail = null;
            this.CedingPlanCodes = [];
            this.getTreatyCodeByCedant();
            this.$nextTick(function () {
                $("#cedantId").selectpicker('refresh');
                $("#paymentMode").selectpicker('refresh');
            })
            $('#riskQuarterMonthDiv').addClass("d-none");
        },
        changePaymentMode: function () {
            var paymentModeCode = PaymentModes[this.NewCedantData.paymentMode];

            if (paymentModeCode != 'M') {
                this.NewCedantData.riskQuarterMonth = "";
                $('#riskQuarterMonthDiv').addClass("d-none");
            } else {
                if (this.NewCedantData.riskQuarter != null &&
                    this.NewCedantData.riskQuarter != "" &&
                    /[0-9]{4} Q{1}([1-4]){1}/.test(this.NewCedantData.riskQuarter)) {
                    this.getMonthByRiskQuarter();
                }
                $('#riskQuarterMonthDiv').removeClass("d-none");
            }
        },
        validateCedantData: function () {
            this.CedantDataValidation = [];
            if (this.NewCedantData.cedantId == null || this.NewCedantData.cedantId == "" || this.NewCedantData.cedantId == 0)
                this.CedantDataValidation.push("Ceding Company is required");
            if (this.NewCedantData.treatyCode == null || this.NewCedantData.treatyCode == "" || this.NewCedantData.treatyCode == 0)
                this.CedantDataValidation.push("Treaty Code is required");
            if (this.NewCedantData.paymentMode == null || this.NewCedantData.paymentMode == "" || this.NewCedantData.paymentMode == 0)
                this.CedantDataValidation.push("Payment Mode is required");
            if (this.NewCedantData.riskQuarter == null || this.NewCedantData.riskQuarter == "")
                this.CedantDataValidation.push("Risk Quarter is required");
            else if (!/[0-9]{4} Q{1}([1-4]){1}/.test(this.NewCedantData.riskQuarter))
                this.CedantDataValidation.push("Risk Quarter format is incorrect");
            else {
                var riskQuarter = $('#Quarter').val().split(" Q");
                var newRiskQuarter = this.NewCedantData.riskQuarter.split(" Q");

                if (riskQuarter[0] < newRiskQuarter[0] || ((riskQuarter[0] == newRiskQuarter[0]) && riskQuarter[1] < newRiskQuarter[1]))
                    this.CedantDataValidation.push("Risk Quarter must not be greater than Reporting Quarter");
            }

            return this.CedantDataValidation.length == 0;
        },
        searchCedantData: function () {
            if (!this.validateCedantData())
                return;

            this.NewCedantData.cedingPlanCode = $('#cedingPlanCodeTokenField').tokenfield('getTokensList', ',');

            var obj = {
                cedantId: this.NewCedantData.cedantId,
                treatyCodeId: this.NewCedantData.treatyCode,
                paymentMode: this.NewCedantData.paymentMode,
                riskQuarter: this.NewCedantData.riskQuarter,
                cedingPlanCode: this.NewCedantData.cedingPlanCode,
                cutOffId: this.Mfrs17Reporting.CutOffId,
                riskQuarterMonth: this.NewCedantData.riskQuarterMonth,
            };

            var mfrs17ReportingDetailBos = [];
            $.ajax({
                url: countCedantDataUrl,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    mfrs17ReportingDetailBos = data.mfrs17ReportingDetailBos;
                }
            });

            this.selectedCheck = [];
            this.isCheckAll = false;

            this.NewCedantDetail = (mfrs17ReportingDetailBos.length === 0) ? null : mfrs17ReportingDetailBos;
            if (this.NewCedantDetail == null)
                this.CedantDataValidation.push("No record found");
        },
        validateAddCedantData: function () {
            this.CedantDataValidation = [];
            for (var i = 0; i < this.selectedCheck.length; i++) {
                var newCedantDetail = this.NewCedantDetail[this.selectedCheck[i]];

                var exists = true;
                var paymentModeCode = PaymentModes[newCedantDetail.PremiumFrequencyCodePickListDetailId];

                if (paymentModeCode == 'M') {
                    exists = this.CedantDetails.some(
                        q => q.CedantId == newCedantDetail.CedantId &&
                            q.TreatyCode == newCedantDetail.TreatyCode &&
                            q.PremiumFrequencyCodePickListDetailId == newCedantDetail.PremiumFrequencyCodePickListDetailId &&
                            q.CedingPlanCode == newCedantDetail.CedingPlanCode &&
                            q.RiskQuarter == newCedantDetail.RiskQuarter &&
                            q.LatestDataStartDateStr == newCedantDetail.LatestDataStartDateStr &&
                            q.LatestDataEndDateStr == newCedantDetail.LatestDataEndDateStr);
                } else {
                    exists = this.CedantDetails.some(
                        q => q.CedantId == this.NewCedantDetail.CedantId &&
                            q.TreatyCode == this.NewCedantDetail.TreatyCode &&
                            q.PremiumFrequencyCodePickListDetailId == this.NewCedantDetail.PremiumFrequencyCodePickListDetailId &&
                            q.CedingPlanCode == newCedantDetail.CedingPlanCode &&
                            q.RiskQuarter == this.NewCedantDetail.RiskQuarter);
                }

                if (exists)
                    this.CedantDataValidation.push("The record #" + (this.selectedCheck[i] + 1) + " already exists in the listing");
                //else {
                //    var count = this.CedantDetails.filter(
                //        q => q.CedantId == this.NewCedantDetail.CedantId &&
                //            q.TreatyCode == this.NewCedantDetail.TreatyCode &&
                //            q.PremiumFrequencyCodePickListDetailId == this.NewCedantDetail.PremiumFrequencyCodePickListDetailId &&
                //            q.Status != StatusPendingDelete && q.Status != StatusDeleted).length;

                //    var paymentModeCode = PaymentModes[this.NewCedantDetail.PremiumFrequencyCodePickListDetailId];
                //    if (((paymentModeCode == 'M' || paymentModeCode == 'Q') && count > 0) ||
                //        (paymentModeCode == 'S' && count > 1) ||
                //        (paymentModeCode == 'A' && count > 3)) {
                //        this.CedantDataValidation.push("The number of risk quarter has exceeded the maximum amount available for this payment mode in this treaty code");
                //    }
                //}
            }

            return this.CedantDataValidation.length == 0;
        },
        addCedantData: function () {
            if (!this.validateAddCedantData())
                return;

            for (var i = 0; i < this.selectedCheck.length; i++) {
                var newCedantDetail = this.NewCedantDetail[this.selectedCheck[i]];

                this.CedantDetails.push(newCedantDetail);
                this.CedantDetailMaxIndex++;

                this.resetLatestDates(newCedantDetail.TreatyCode, newCedantDetail.PremiumFrequencyCodePickListDetailId);
            }

            //this.CedantDetails.push(this.NewCedantDetail);
            //this.CedantDetailMaxIndex++;

            //this.resetLatestDates(this.NewCedantDetail.TreatyCode, this.NewCedantDetail.PremiumFrequencyCodePickListDetailId);

            $('#newCedantDataModal').modal('toggle');
        },
        resetLatestDates: function (treatyCode, premiumFrequencyCodePickListDetailId) {
            var paymentModeCode = PaymentModes[premiumFrequencyCodePickListDetailId];
            if (paymentModeCode == 'M' || paymentModeCode == 'Q')
                return;

            //paymentModeCode = PaymentModes[this.NewCedantDetail.PremiumFrequencyCodePickListDetailId];
            var relevantCedantDetails = this.CedantDetails.filter(
                q => q.TreatyCode == treatyCode &&
                    q.PremiumFrequencyCodePickListDetailId == premiumFrequencyCodePickListDetailId &&
                    q.Status != StatusPendingDelete && q.Status != StatusDeleted);

            var firstQuarter = null;
            var lastQuarter = null;
            var compareQuarter = this.compareQuarter;
            relevantCedantDetails.forEach(function (item, index) {
                if (firstQuarter == null || compareQuarter(firstQuarter, item.RiskQuarter) == 1)
                    firstQuarter = item.RiskQuarter;

                if (lastQuarter == null || compareQuarter(lastQuarter, item.RiskQuarter) == -1)
                    lastQuarter = item.RiskQuarter;
            });

            var dataStartDate = this.quarterToDate(firstQuarter);
            var dataEndDate = this.quarterToDate(lastQuarter, true);
            relevantCedantDetails.forEach(function (item, index) {
                item.LatestDataStartDateStr = dataStartDate;
                item.LatestDataEndDateStr = dataEndDate;
            });

        },
        quarterToDate: function (riskQuarter, lastDay = false) {
            var quarter = riskQuarter.split(" Q");

            var year = quarter[0];
            var month = quarter[1] * 3;
            if (!lastDay)
                month = month - 2;
            var date = moment(year + "-" + month + "-1");
            if (lastDay)
                date = date.endOf('month');

            return date.format(DateFormat);
        },
        compareQuarter: function (q1, q2) {
            var quarter1 = q1.split(" Q");
            var quarter2 = q2.split(" Q");

            if (quarter1[0] == quarter2[0]) {
                if (quarter1[1] == quarter2[1])
                    return 0;
                else if (quarter1[1] > quarter2[1])
                    return 1;
                else
                    return -1;
            } else {
                if (quarter1[0] > quarter2[0])
                    return 1;
                else
                    return -1;
            }
        },
        getCedingPlanCodeByCedingCompanyTreatyCode: function () {
            this.NewCedantData.cedingPlanCode = "";

            var cedantId = this.NewCedantData.cedantId;
            var treatyCodeId = this.NewCedantData.treatyCode;
            var cutOffId = this.Mfrs17Reporting.CutOffId;

            var cedingPlanCodes = null;
            $.ajax({
                url: getCedingPlanCodeByCedingCompanyTreatyCodeUrl,
                type: "POST",
                data: {
                    cedantId: cedantId,
                    treatyCodeId: treatyCodeId,
                    cutOffId: cutOffId,
                },
                cache: false,
                async: false,
                success: function (data) {
                    cedingPlanCodes = data.CedingPlanCodes;
                }
            });
            this.CedingPlanCodes = cedingPlanCodes;

            this.$nextTick(function () {
                $('#cedingPlanCodeTokenField').data('bs.tokenfield').$input.autocomplete({ source: cedingPlanCodes });
                $('#cedingPlanCodeTokenField').tokenfield('setTokens', []);
            })
        },
        checkAll: function () {
            this.isCheckAll = !this.isCheckAll;
            var selectedCheck = new Array();
            if (this.isCheckAll) { // Check all
                this.NewCedantDetail.forEach(function (item, index) {
                    selectedCheck.push(index);
                });
            } else {
                selectedCheck = [];
            }

            this.selectedCheck = selectedCheck;
            console.log(this.selectedCheck);
        },
        updateCheck: function (index) {
            if (this.selectedCheck.length == this.NewCedantDetail.length) {
                this.isCheckAll = true;
            } else {
                this.isCheckAll = false;
            }
        },
    },
    created: function () {
        this.CedantDetailMaxIndex = (this.CedantDetails) ? this.CedantDetails.length - 1 : -1;
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});

function validateQuarter(qtr) {
    if (qtr == null || qtr == "")
        return;
    if (!/[0-9]{4} Q{1}([1-4]){1}/.test(qtr)) {
        $("#Quarter").val(null);
        alert('Invalid Quarter Format: ' + qtr);
    }
}

function validateForm() {
    var forms = document.getElementsByClassName('needs-validation');
    // Loop over them and prevent submission
    var validation = Array.prototype.filter.call(forms, function (form) {
        form.addEventListener('submit', function (event) {
            if (form.checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });
}

function resetGenerateModal() {
    $('#GenerateTypeId').val('');
    changeGenerateType('');

    $('#GenerateTypeId').selectpicker('refresh');
}

function changeGenerateType(generateType) {
    $('.isResumeRow').hide();
    $('#IsResumeSwitch').prop('disabled', true);

    $('#GenerateModifiedOnlySwitch').prop('disabled', true);
    if (generateType == GenerateTypeMultiple) {
        $('#GenerateModifiedOnlySwitch').prop('disabled', false);
        /*$('#GenerateModifiedOnlySwitch').prop('checked', DefaultGenerateModified);*/

        if (Mfrs17ReportingModel.Status == StatusFailedGenerate) {
            $('.isResumeRow').show();

            $('#IsResumeSwitch').prop('disabled', false);
            /*$('#IsResumeSwitch').prop('checked', DefaultIsResume);*/
        }
    }
    else {
        $('#GenerateModifiedOnlySwitch').prop('checked', false);
        $('#IsResumeSwitch').prop('checked', false);
    }
}

function finalised() {
    $('#reportingStatus').val(StatusFinalised);
    $('form').submit();
}

function submitForProcessing() {
    $('#reportingStatus').val(StatusSubmitForProcessing);
    $('form').submit();
}

function submitForUpdating() {
    $('#reportingStatus').val(StatusPendingUpdate);
    $('form').submit();
}

function submitForGenerate() {
    $('#reportingStatus').val(StatusPendingGenerate);
    $('#GenerateType').val($('#GenerateTypeId').val());
    $('#GenerateModifiedOnly').val($('#GenerateModifiedOnlySwitch').prop("checked"));
    $('#IsResume').val($('#IsResumeSwitch').prop("checked"));
    $('form').submit();
}