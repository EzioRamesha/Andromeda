
$(document).ready(function () {
    dateOffAutoComplete();

    $('#Quarter').datepicker({
        format: QuarterFormat,
        minViewMode: 1,
        autoclose: true,
        language: "qtrs",
        forceParse: false,
    })
        .on('show', function (e) {
            var datepickerDropDown = $('.datepicker');
            datepickerDropDown.addClass('quarterpicker');
        })
        .on('changeDate', function () {
            Model.Quarter = $('#Quarter').val();
        });

    $('#StatementReceivedAt').datepicker({
        format: DateFormatDatePickerJs,
    });

    if (Model.CedantId == 0) {
        $("#CedantId").prop("selectedIndex", 0);
        $('#CedantId').selectpicker('refresh');
    }

    var cedantId = $('#CedantId').val();
    getTreaty(cedantId);

    getBusinessType(Model.TreatyId);

    loadTab(1);

    $('.tbs').on('click', function () {
        var info = $(this).data('info');

        loadTab(info);
    });

    loadingDiv.addClass('hide-loading-spinner');
});

function loadTab(tab) {
    loadingDiv.removeClass('hide-loading-spinner');
    switch (tab) {
        case 1:
            $('#soaDataReinsuranceMyrCurrency').load(GetSoaDataReinsuranceMyrCurrency, function () {
                attachHandlers();
                loadingDiv.addClass('hide-loading-spinner');
            });
            $('#soaDataRetakaful').load(GetSoaDataRetakaful, function () {
                attachHandlers2();
                loadingDiv.addClass('hide-loading-spinner');
            });
            $('#soaDataReinsuranceOriginalCurrency').load(GetSoaDataReinsuranceOriginalCurrency, function () {
                attachHandlers3();
                loadingDiv.addClass('hide-loading-spinner');
            });
            break;
        case 2:
            $('#soaValidationReinsuranceMyrCurrency').load(GetSoaDataValidationReinsuranceMyrCurrency, function () { loadingDiv.addClass('hide-loading-spinner'); });
            $('#soaValidationReinsuranceOriginalCurrency').load(GetSoaDataValidationReinsuranceOriginalCurrency, function () { loadingDiv.addClass('hide-loading-spinner'); });
            $('#soaValidationRetakaful').load(GetSoaValidationRetakaful, function () { loadingDiv.addClass('hide-loading-spinner'); });
            break;
        case 3:
            $('#mlreShareMyrCurrency').load(GetSoaDataPostValidationSummaryMlreShareMyrCurrency, function () { loadingDiv.addClass('hide-loading-spinner'); });
            $('#mlreShareOriginalCurrency').load(GetSoaDataPostValidationSummaryMlreShareOriginalCurrency, function () { loadingDiv.addClass('hide-loading-spinner'); });
            $('#postValidationLayerShare').load(GetSoaDataPostValidationSummaryLayerShare, function () { loadingDiv.addClass('hide-loading-spinner'); });
            $('#postValidationRetakaful').load(GetSoaDataPostValidationSummaryRetakaful);
            break;
        case 4:
            app.getCompiledSummary();
            loadingDiv.addClass('hide-loading-spinner');
            break;
    }
}

function getTreaty(cedantId) {

    if (!isNaN(cedantId)) {
        $.ajax({
            url: GetTreatyUrl,
            type: "POST",
            data: { cedantId: cedantId },
            cache: false,
            async: false,
            success: function (data) {
                var treatyList = data.treatyBos;
                refreshDropDownItems('TreatyId', treatyList, Model.TreatyId, 'TreatyIdCode', 'Description');
            }
        });
    }
    else {
        refreshDropDownItems('TreatyId', [], null, 'TreatyIdCode', 'Description');
    }
}

function getBusinessType(treatyId) {

    if (!isNaN(treatyId)) {
        $.ajax({
            url: GetTypeUrl,
            type: "POST",
            data: { treatyId: treatyId },
            cache: false,
            async: false,
            success: function (data) {
                var typeKey = data.typeKey;
                var typeName = data.typeName;

                if (typeKey == "") {
                    $('#Type').val(DefaultType);
                    $('#TypeStr').val(DefaultTypeName);
                }
                else {
                    $('#Type').val(typeKey);
                    $('#TypeStr').val(typeName);
                }
            }
        });
    }
    else {
        $("#Type").val('');
        $('#TypeStr').val('');
    }
}

function focusOnDate(val) {
    $('#' + val).focus();
}

var selDiv = "";
document.addEventListener("DOMContentLoaded", init, false);
function init() {
    var control = document.querySelector('#files');
    if (control) {
        document.querySelector('#files').addEventListener('change', handleFileSelect, false);
        selDiv = document.querySelector("#selectedFiles");
    }
}
function handleFileSelect(e) {
    if (!e.target.files) return;
    selDiv.innerHTML = "";
    var files = e.target.files;
    var list = "";
    for (var i = 0; i < files.length; i++) {
        var f = files[i];

        var fileSize = f.size / 1024 / 1024 / 1024; // in GB
        if (fileSize >= 2) {
            $("#errorSizeExceeds").css("display", "block");
            $("#errorSizeExceeds").text('Maximum allowed size is : 2 GB');
            //reset file upload control
            e.target.value = null;
            return;
        } else {
            $("#errorSizeExceeds").css("display", "none");
        }

        list += "<li>" + f.name + "</li>";
    }
    selDiv.innerHTML = "<ul>" + list + "</ul>";
}

function updateStatus(status) {
    $('#Status').val(status);
    $('#editForm').submit();
}

function updateDataStatus(status) {
    $('#DataUpdateStatus').val(status);
    $('#IsUpdateData').val(true);
    $('#editForm').submit();
}

function saveBatch(status) {
    $('#IsSave').val(status);
    $('#editForm').submit();
}

function downloadSoaValidationFile(key) {
    app.downloadSoaValidationTab(key);
}

function downloadSoaPostValidationFile(key) {
    app.downloadSoaPostValidationTab(key);
}

function retrieveErrors(Error) {
    app.getErrors(Error);
}

var app = new Vue({
    el: '#app',
    data: {
        SoaDataModel: Model,
        RiDataMatchStatus: '',
        ClaimDataMatchStatus: '',
        SoaDataErrorsMsg: [],
        // Match Ri Data Batch
        RiDataBatches: [],
        MatchRiDataBatchesValidation: [],
        // Match Claim Data Batch
        ClaimDataBatches: [],
        MatchClaimDataBatchesValidation: [],
        // Remark
        Remarks: RemarksList ? RemarksList : [],
        RemarkModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            Status: StatusDraft ? StatusDraft : null,
            CreatedAtStr: null,
            Content: null,
        },
        // Status History
        StatusHistories: StatusHistoriesList ? StatusHistoriesList : [],
        // File History
        FileHistories: FileHistoriesList ? FileHistoriesList : [],
        checkedExclude: CheckedExclude ? CheckedExclude : [],
        SoaDataFileErrors: [],
        // Auto Expand TextArea
        textAreaWidth: 150,
        textAreaHeight: 21,
        // SOA Compiled Summary
        WMCompiledSummaryMYRs: [],
        WMCompiledSummaryOCs: [],
        SFCompiledSummarys: [],
        AccountForCodes: AccountForList ? AccountForList : [],
        InvoiceTypes: InvoiceTypeList ? InvoiceTypeList : [],

        ReportingTypes: ReportingTypeList,
        ReportingTypeId: ReportingTypeIFRS4,
    },
    methods: {
        // Match Ri Data
        validateRiDataMatchData: function () {
            var validation = new Array();

            if (this.SoaDataModel.CedantId == null || this.SoaDataModel.CedantId == "" || this.SoaDataModel.CedantId == 0)
                validation.push("Ceding Company is required");
            if (this.SoaDataModel.TreatyId == null || this.SoaDataModel.TreatyId == "" || this.SoaDataModel.TreatyId == 0)
                validation.push("Treaty ID is required");
            if (this.SoaDataModel.Quarter == null || this.SoaDataModel.Quarter == "")
                validation.push("Quarter is required");
            else if (!/[0-9]{4} Q{1}([1-4]){1}/.test(this.SoaDataModel.Quarter))
                validation.push("Quarter format is incorrect");

            this.MatchRiDataBatchesValidation = validation;

            return this.MatchRiDataBatchesValidation.length == 0;
        },
        searchRiDataBatch: function () {
            if (!this.validateRiDataMatchData())
                return;

            var obj = {
                CedantId: this.SoaDataModel.CedantId,
                TreatyId: this.SoaDataModel.TreatyId,
                Quarter: this.SoaDataModel.Quarter,
            };

            var riDataBatchBos = [];
            $.ajax({
                url: SearchRIDataBatchUrl ? SearchRIDataBatchUrl : null,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    riDataBatchBos = data.riDataBatchBos;
                }
            });

            this.RiDataBatches = riDataBatchBos;
            if (this.RiDataBatches == null || !this.RiDataBatches.length)
                this.MatchRiDataBatchesValidation.push("No match found");
        },
        selectRiDataBatch: function () {
            var index = $('input[name="checkedRidataBatch"]:checked').val();
            if (typeof index == 'undefined') {
                this.MatchRiDataBatchesValidation.push("No RI Data selected.");
                return;
            }

            var item = this.RiDataBatches[index];
            $('#RiDataBatchId').val(item.Id);
            this.SoaDataModel.RiDataBatchId = item.Id;
            this.RiDataMatchStatus = 'Matched';

            $('#matchRiDataModal').modal('toggle');
        },
        viewRiDataBatch: function () {
            var url = ViewRiDataBatchUrl + '/' + this.SoaDataModel.RiDataBatchId;
            return window.open(url, "_blank");
        },
        // Match Claim Data
        validateClaimMatchData: function () {
            var validation = new Array();

            if (this.SoaDataModel.CedantId == null || this.SoaDataModel.CedantId == "" || this.SoaDataModel.CedantId == 0)
                validation.push("Ceding Company is required");
            if (this.SoaDataModel.TreatyId == null || this.SoaDataModel.TreatyId == "" || this.SoaDataModel.TreatyId == 0)
                validation.push("Treaty ID is required");
            if (this.SoaDataModel.Quarter == null || this.SoaDataModel.Quarter == "")
                validation.push("Quarter is required");
            else if (!/[0-9]{4} Q{1}([1-4]){1}/.test(this.SoaDataModel.Quarter))
                validation.push("Quarter format is incorrect");

            this.MatchClaimDataBatchesValidation = validation;

            return this.MatchClaimDataBatchesValidation.length == 0;
        },
        searchClaimDataBatch: function () {
            if (!this.validateClaimMatchData())
                return;

            var obj = {
                CedantId: this.SoaDataModel.CedantId,
                TreatyId: this.SoaDataModel.TreatyId,
                Quarter: this.SoaDataModel.Quarter,
            }

            var claimDataBatchBos = [];
            $.ajax({
                url: SearchClaimDataBatchUrl ? SearchClaimDataBatchUrl : null,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    claimDataBatchBos = data.claimDataBatchBos;
                }
            });

            this.ClaimDataBatches = claimDataBatchBos;
            if (this.ClaimDataBatches == null || !this.ClaimDataBatches.length)
                this.MatchClaimDataBatchesValidation.push("No match found");
        },
        selectClaimDataBatch: function () {
            var index = $('input[name="checkedClaimdataBatch"]:checked').val();
            if (typeof index == 'undefined') {
                this.MatchClaimDataBatchesValidation.push("No Claim Data selected.");
                return;
            }

            var item = this.ClaimDataBatches[index];
            $('#ClaimDataBatchId').val(item.Id);
            this.SoaDataModel.ClaimDataBatchId = item.Id;
            this.ClaimDataMatchStatus = 'Matched';

            $('#matchClaimsDataModal').modal('toggle');
        },
        viewClaimDataBatch: function () {
            var url = ViewClaimDataBatchUrl + '/' + this.SoaDataModel.ClaimDataBatchId;
            return window.open(url, "_blank");
        },
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.Status = this.SoaDataModel.Status;
            this.RemarkModal.ModuleId = this.SoaDataModel.ModuleId;
            this.RemarkModal.ObjectId = this.SoaDataModel.Id;
            this.RemarkModal.Content = null;
        },
        addRemark: function () {
            var remark = createRemark(this.RemarkModal);

            if (remark) {
                this.Remarks.unshift(Object.assign({}, remark));
            }
        },
        // Auto Expand TextArea
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
        // File Hostory
        check: function () {
            if (this.checkedExclude != null) {
                $("#Mode").val(this.checkedExclude.join(","));
            }
        },
        downloadSoaDataFileUrl: function (index) {
            return DownloadSoaDataFileUrl + "?rawFileId=" + this.FileHistories[index].RawFileBo.Id;
        },
        getSoaDataFileErrors: function (index) {
            var item = this.FileHistories[index];
            if (item.Errors != null) {
                this.SoaDataFileErrors = JSON.parse(item.Errors);
            }
        },
        // SoaData Compiled Summary
        changeReportingType: function () {
            this.ReportingTypeId = $('#ReportingTypeId').val();
            loadTab(4);
        },
        getCompiledSummary: function () {
            this.WMCompiledSummaryMYRs = [];
            this.WMCompiledSummaryOCs = [];
            this.SFCompiledSummarys = [];

            var obj = {
                SoaDataBatchId: this.SoaDataModel.Id,
                ReportingTypeId: this.ReportingTypeId,
            }

            var wmCompiledSummaryMYRBos = [];
            var wmCompiledSummaryOCBos = [];
            var sfCompiledSummaryBos = [];
            $.ajax({
                url: GetSoaDataCompiledSummaryUrl ? GetSoaDataCompiledSummaryUrl : null,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    wmCompiledSummaryMYRBos = data.WMCompiledSummaryMYRBos;
                    wmCompiledSummaryOCBos = data.WMCompiledSummaryOCBos;
                    sfCompiledSummaryBos = data.SFCompiledSummaryBos;
                }
            });

            this.WMCompiledSummaryMYRs = wmCompiledSummaryMYRBos;
            this.WMCompiledSummaryOCs = wmCompiledSummaryOCBos;
            this.SFCompiledSummarys = sfCompiledSummaryBos;
        },
        // Year Picker
        openYearPicker: function (currentId) {
            var idStr = currentId.split("_");
            var type = idStr[0];
            var field = idStr[1];
            var index = idStr[2];

            var id = "#" + currentId;
            if ($(id).data("datepicker") != null) {
                $(id).datepicker("destroy");
            }

            $(id).datepicker({
                format: "yyyy",
                viewMode: "years",
                minViewMode: "years"
            });

            var updateDateValue = this.updateDateValue;
            $(id).on('changeDate', function () {
                updateDateValue(type, field, index, $(id).val());
            });

            $(id).focus();
        },
        // Quarter Picker
        openQuarterPicker: function (currentId) {
            var idStr = currentId.split("_");
            var type = idStr[0];
            var field = idStr[1];
            var index = idStr[2];

            var id = "#" + currentId;
            if ($(id).data("datepicker") != null) {
                $(id).datepicker("destroy");
            }

            $(id).datepicker({
                format: "yyMM",
                minViewMode: 1,
                autoclose: true,
                language: "qtrs",
                forceParse: false,
            }).on('show', function (e) {
                var datepickerDropDown = $('.datepicker');
                datepickerDropDown.addClass('quarterpicker');
            });

            var updateDateValue = this.updateDateValue;
            $(id).on('changeDate', function () {
                updateDateValue(type, field, index, $(id).val());
            });

            $(id).focus();
        },
        // Date Picker
        openDatePicker: function (currentId) {
            var idStr = currentId.split("_");
            var type = idStr[0];
            var field = idStr[1];
            var index = idStr[2];

            var id = "#" + currentId;
            if ($(id).data("datepicker") != null) {
                $(id).datepicker("destroy");
            }

            $(id).datepicker({
                format: DateFormatDatePickerJs,
                autoclose: true,
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
        getErrors: function (errors) {
            console.log(1);
            console.log(errors);
            if (errors != null) {
                this.SoaDataErrorsMsg = errors;
            }
        },
        setDateFormat: function (date) {
            if (date != '')
                return moment(date).format('DD MMM YYYY');
        },
        setAmountFormat: function (amount) {
            if (amount == '' || amount == null)
                amount = 0;

            return amount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,");
        },
        // Download SOA tab
        downloadSoaValidationTab: function (type) {
            var rows = [];
            console.log(type);
            var tabRows = "";
            if (type == 1)
                tabRows = "SoaValidationReinsuranceMyr";
            else if (type == 2)
                tabRows = "SoaValidationReinsuranceOri";
            else if (type == 3)
                tabRows = "SoaValidationRetakaful";

            rows.push("");

            $('#' + tabRows + 'SoaDataTabRows thead tr').each(function () {
                var row = "";
                for (var i = 0; i < 17; i++) {
                    row += $(this).find('th').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            $('#' + tabRows + 'SoaDataTabRows tbody tr').each(function () {
                var row = "";
                for (var i = 0; i < 17; i++) {
                    row += $(this).find('td').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            rows.push("");

            $('#' + tabRows + 'RiSummaryTabRows thead tr').each(function () {
                var row = "";
                for (var i = 0; i < 17; i++) {
                    row += $(this).find('th').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            $('#' + tabRows + 'RiSummaryTabRows tbody tr').each(function () {
                var row = "";
                for (var i = 0; i < 17; i++) {
                    row += $(this).find('td').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            rows.push("");

            $('#' + tabRows + 'DifferencesTabRows thead tr').each(function () {
                var row = "";
                for (var i = 0; i < 17; i++) {
                    row += $(this).find('th').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            $('#' + tabRows + 'DifferencesTabRows tbody tr').each(function () {
                var row = "";
                for (var i = 0; i < 17; i++) {
                    row += $(this).find('td').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            this.downloadHtmlTable(type, rows);
        },
        downloadSoaPostValidationTab: function (type) {
            var rows = [];

            var tabRows = "";
            if (type == 4)
                tabRows = "SoaPostValidationMLReShareMyr";
            else if (type == 5)
                tabRows = "SoaPostValidationMLReShareOri";
            else if (type == 6)
                tabRows = "SoaPostValidationLayerShare";
            else if (type == 7)
                tabRows = "SoaPostValidationRetakaful";

            rows.push("");

            $('#' + tabRows + 'CheckingTabRows thead tr:first-child').each(function () {
                var row = "";

                for (var i = 0; i < 10; i++) {
                    if (!$(this).find('th').eq(i).attr('colspan')) {
                        row += $(this).find('th').eq(i).text().trim() + "|";
                    } else {
                        var colspanCount = $(this).find('th').eq(i).attr('colspan');
                        for (var j = 0; j < colspanCount; j++) {
                            if (j === 0)
                                row += $(this).find('th').eq(i).text().trim() + "|";
                            else
                                row += "|";
                        }
                    }
                }
                rows.push(row);
            });

            $('#' + tabRows + 'CheckingTabRows thead tr:last-child').each(function () {
                var row = "";
                for (var i = 0; i < 32; i++) {
                    row += $(this).find('th').eq(i).text().trim() + "|";
                }
                //var column1 = $(this).find('th').eq(0).text().trim();
                //var column2 = $(this).find('th').eq(1).text().trim();
                //var column3 = $(this).find('th').eq(2).text().trim();
                //var column4 = $(this).find('th').eq(3).text().trim();
                //var column5 = $(this).find('th').eq(4).text().trim();
                //var column6 = $(this).find('th').eq(5).text().trim();
                //var column7 = $(this).find('th').eq(6).text().trim();
                //var column8 = $(this).find('th').eq(7).text().trim();
                //var column9 = $(this).find('th').eq(8).text().trim();
                //var column10 = $(this).find('th').eq(9).text().trim();
                //var column11 = $(this).find('th').eq(10).text().trim();
                //var column12 = $(this).find('th').eq(11).text().trim();
                //var column13 = $(this).find('th').eq(12).text().trim();
                //var column14 = $(this).find('th').eq(13).text().trim();
                //var column15 = $(this).find('th').eq(14).text().trim();
                //var column16 = $(this).find('th').eq(15).text().trim();
                //var column17 = $(this).find('th').eq(16).text().trim();
                //var row = column1 + "|" + column2 + "|" + column3 + "|" + column4 + "|" + column5
                //    + "|" + column6 + "|" + column7 + "|" + column8 + "|" + column9 + "|" + column10
                //    + "|" + column11 + "|" + column12 + "|" + column13 + "|" + column14 + "|" + column15 + "|"
                //    + column16 + "|" + column17 + "|";
                rows.push(row);
            });

            $('#' + tabRows + 'CheckingTabRows tbody tr').each(function () {
                var row = "";
                for (var i = 0; i < 32; i++) {
                    row += $(this).find('td').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            rows.push("");

            $('#' + tabRows + 'CedantTabRows thead tr:first-child').each(function () {
                var row = "";

                for (var i = 0; i < 10; i++) {
                    if (!$(this).find('th').eq(i).attr('colspan')) {
                        row += $(this).find('th').eq(i).text().trim() + "|";
                    } else {
                        var colspanCount = $(this).find('th').eq(i).attr('colspan');
                        for (var j = 0; j < colspanCount; j++) {
                            if (j === 0)
                                row += $(this).find('th').eq(i).text().trim() + "|";
                            else
                                row += "|";
                        }
                    }
                }
                rows.push(row);
            });

            $('#' + tabRows + 'CedantTabRows thead tr:last-child').each(function () {
                var row = "";
                for (var i = 0; i < 32; i++) {
                    row += $(this).find('th').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            $('#' + tabRows + 'CedantTabRows tbody tr').each(function () {
                var row = "";
                for (var i = 0; i < 32; i++) {
                    row += $(this).find('td').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            rows.push("");

            $('#' + tabRows + 'DiscrepancyTabRows thead tr:first-child').each(function () {
                var row = "";

                for (var i = 0; i < 10; i++) {
                    if (!$(this).find('th').eq(i).attr('colspan')) {
                        row += $(this).find('th').eq(i).text().trim() + "|";
                    } else {
                        var colspanCount = $(this).find('th').eq(i).attr('colspan');
                        for (var j = 0; j < colspanCount; j++) {
                            if (j === 0)
                                row += $(this).find('th').eq(i).text().trim() + "|";
                            else
                                row += "|";
                        }
                    }
                }
                rows.push(row);
            });

            $('#' + tabRows + 'DiscrepancyTabRows thead tr:last-child').each(function () {
                var row = "";
                for (var i = 0; i < 32; i++) {
                    row += $(this).find('th').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            $('#' + tabRows + 'DiscrepancyTabRows tbody tr').each(function () {
                var row = "";
                for (var i = 0; i < 32; i++) {
                    row += $(this).find('td').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });


            rows.push("");

            $('#' + tabRows + 'DifferencesTabRows thead tr').each(function () {
                var row = "";
                for (var i = 0; i < 7; i++) {
                    row += $(this).find('th').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            $('#' + tabRows + 'DifferencesTabRows tbody tr').each(function () {
                var row = "";
                for (var i = 0; i < 7; i++) {
                    row += $(this).find('td').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            this.downloadHtmlTable(type, rows);
        },
        downloadSoaCompiledTab: function (type) {
            var rows = [];

            rows.push("");
            rows.push($('#summaryByWMOMRowsLabel1')[0].innerHTML);
            $('#SoaCompiledSummaryMyrWmOM' + type + 'TabRows thead tr:first-child').each(function () {
                var row = "";

                for (var i = 0; i < 17; i++) {
                    if (!$(this).find('th').eq(i).attr('colspan')) {
                        row += $(this).find('th').eq(i).text().trim() + "|";
                    } else {
                        var colspanCount = $(this).find('th').eq(i).attr('colspan');
                        for (var j = 0; j < colspanCount; j++) {
                            if (j === 0)
                                row += $(this).find('th').eq(i).text().trim() + "|";
                            else
                                row += "|";
                        }
                    }
                }
                rows.push(row);
            });

            $('#SoaCompiledSummaryMyrWmOM' + type + 'TabRows thead tr:last-child').each(function () {
                var row = "";

                for (var i = 0; i < 52; i++) {
                    row += $(this).find('th').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            $('#SoaCompiledSummaryMyrWmOM' + type + 'TabRows tbody tr').each(function () {
                var row = "";
                for (var i = 0; i < 52; i++) {
                    if ((type == ReportingTypeIFRS4 && i == 4) || (type == ReportingTypeIFRS17 && i == 6))
                        row += $(this).find("td:eq(" + i + ") select option:selected").val() + "|";
                    else if ($(this).find("td:eq(" + i + ") input:not([type=hidden])").length)
                        row += $(this).find("td:eq(" + i + ") input:not([type=hidden])").val() + "|";
                    else
                        row += $(this).find('td').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            rows.push("");

            $('#SoaCompiledSummaryOriWmOM' + type + 'TabRows thead tr:first-child').each(function () {
                var row = "";

                for (var i = 0; i < 17; i++) {
                    if (!$(this).find('th').eq(i).attr('colspan')) {
                        row += $(this).find('th').eq(i).text().trim() + "|";
                    } else {
                        var colspanCount = $(this).find('th').eq(i).attr('colspan');
                        for (var j = 0; j < colspanCount; j++) {
                            if (j === 0)
                                row += $(this).find('th').eq(i).text().trim() + "|";
                            else
                                row += "|";
                        }
                    }
                }
                rows.push(row);
            });

            $('#SoaCompiledSummaryOriWmOM' + type + 'TabRows thead tr:last-child').each(function () {
                var row = "";
                for (var i = 0; i < 52; i++) {
                    row += $(this).find('th').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            $('#SoaCompiledSummaryOriWmOM' + type + 'TabRows tbody tr').each(function () {
                var row = "";
                for (var i = 0; i < 52; i++) {
                    if ((type == ReportingTypeIFRS4 && i == 4) || (type == ReportingTypeIFRS17 && i == 6))
                        row += $(this).find("td:eq(" + i + ") select option:selected").val() + "|";
                    else if ($(this).find("td:eq(" + i + ") input:not([type=hidden])").length)
                        row += $(this).find("td:eq(" + i + ") input:not([type=hidden])").val() + "|";
                    else
                        row += $(this).find('td').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            rows.push("");
            rows.push($('#summaryBySFWMRowsLabel2')[0].innerHTML);
            $('#SoaCompiledSummarySfwm' + type + 'TabRows thead tr:first-child').each(function () {
                var row = "";

                for (var i = 0; i < 7; i++) {
                    if (!$(this).find('th').eq(i).attr('colspan')) {
                        row += $(this).find('th').eq(i).text().trim() + "|";
                    } else {
                        var colspanCount = $(this).find('th').eq(i).attr('colspan');
                        for (var j = 0; j < colspanCount; j++) {
                            if (j === 0)
                                row += $(this).find('th').eq(i).text().trim() + "|";
                            else
                                row += "|";
                        }
                    }
                }
                rows.push(row);
            });

            $('#SoaCompiledSummarySfwm' + type + 'TabRows thead tr:last-child').each(function () {
                var row = "";
                for (var i = 0; i < 42; i++) {
                    row += $(this).find('th').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            $('#SoaCompiledSummarySfwm' + type + 'TabRows tbody tr').each(function () {
                var row = "";
                for (var i = 0; i < 42; i++) {
                    if ((type == ReportingTypeIFRS4 && i == 4) || (type == ReportingTypeIFRS17 && i == 6))
                        row += $(this).find("td:eq(" + i + ") select option:selected").val() + "|";
                    else if ($(this).find("td:eq(" + i + ") input:not([type=hidden])").length)
                        row += $(this).find("td:eq(" + i + ") input:not([type=hidden])").val() + "|";
                    else
                        row += $(this).find('td').eq(i).text().trim() + "|";
                }
                rows.push(row);
            });

            this.downloadHtmlTable((type == ReportingTypeIFRS4 ? 8 : 9), rows);
        },
        downloadHtmlTable: function (type, rows) {
            $.ajax({
                url: DownloadHtmlTableUrl,
                type: "POST",
                data: {
                    id: this.SoaDataModel.Id,
                    type: type,
                    rows: JSON.stringify(rows),
                },
                cache: false,
                async: false,
                success: function (data) {
                    console.log(data);
                    if (data.errors != null && data.errors.length > 0) {
                        if (data.errors = "Access is denied") {
                            window.location = '/SoaData/DownloadSoaDataHtmlTable'
                                + '?fileName=' + data.fileName;
                        }
                        else {
                            console.log(data.errors);
                        }
                    }
                    else {
                        window.location = '/SoaData/DownloadSoaDataHtmlTable'
                            + '?fileName=' + data.fileName;
                    }
                },
            });
        }
    },
    created: function () {
        this.RiDataMatchStatus = (this.SoaDataModel.RiDataBatchId) ? 'Matched' : '';
        this.ClaimDataMatchStatus = (this.SoaDataModel.ClaimDataBatchId) ? 'Matched' : '';

        /*this.getCompiledSummary();*/
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});