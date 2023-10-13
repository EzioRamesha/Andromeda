$(document).ready(function () {
    dateOffAutoComplete();

    $('#BatchDate, #FollowUpDate').datepicker({
        format: DateFormatDatePickerJs,
    });
});

function getTreatyByCedant(cedantId) {

    if (!isNaN(cedantId)) {
        $.ajax({
            url: TreatyByCedantUrl,
            type: "POST",
            data: { cedantId: cedantId },
            cache: false,
            async: false,
            success: function (data) {
                var treatyList = data.treatyBos;
                if (typeof treatyList != "undefined")
                    refreshDropDownItems('treatyId', treatyList, null, 'TreatyIdCode', 'Description');
                else
                    refreshDropDownItems('treatyId', [], null, 'TreatyIdCode', 'Description');
            }
        });
    }
    else {
        refreshDropDownItems('treatyId', [], null, 'TreatyIdCode', 'Description');
    }
}

function focusOnDate(val) {
    $('#' + val).focus();
}

var app = new Vue({
    el: '#app',
    data: {
        InvoiceRegisterBatch: Model,
        // Compiled Summary        
        WMCompiledSummaryMYRs: [],
        WMCompiledSummaryOCs: [],
        SFCompiledSummarys: [],
        InvoiceTypes: InvoiceTypeList ? InvoiceTypeList : [],
        ReportingTypes: ReportingTypeList,
        ReportingTypeId: ReportingTypeIFRS4,
        // Remark        
        Remarks: RemarksList ? RemarksList : [],
        RemarkMaxIndex: 0,
        RemarkModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            Status: StatusPending ? StatusPending : null,
            CreatedAtStr: null,
            RemarkIsPrivate: false,
            Content: null,
            DocumentIsPrivate: false,
            HasFollowUp: false,
            FollowUpAtStr: null,
            FollowUpUserId: null,
        },
        // Document
        Documents: DocumentBos ? DocumentBos : [],
        DocumentTypeItems: DocumentTypeItems,
        DocumentMaxIndex: 0,
        DocumentModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            File: null,
            Type: 0,
            Description: '',
        },
        // Status History
        StatusHistories: StatusHistoriesList ? StatusHistoriesList : [],
        StatusFiles: StatusFiles ? StatusFiles : [],
        // Soa Data
        SoaDataDetails: SoaDataDetailsList ? SoaDataDetailsList : [],
        SoaDataDetailMaxIndex: 0,
        newSoaData: {
            cedantId: null,
            soaQuarter: null,
            treatyId: null,
        },
        newSoaDataDetail: null,
        SoaDataValidation: [],
        isCheckAll: false,
        selected: [],
        // SUNGL Files
        SunglFiles: SunglFileList ? SunglFileList : [],
        Uploads: UploadList ? UploadList : [],
        // Others
        DropDownCedants: DropDownCedants,
        DropDownUsers: DropDownUsers,
        DropDownTypes: DropDownTypes,
        DropDownTreatys: DropDownTreatys,

        textAreaWidth: 150,
        textAreaHeight: 21,
    },
    methods: {
        // Soa Data
        addNew: function () {
            this.SoaDataDetailMaxIndex++;
        },
        deleteSoaDataDetail: function (index) {
            this.SoaDataDetails.splice(index, 1);
            this.SoaDataDetailMaxIndex--;
        },
        resetModalData: function () {
            this.CedantDataValidation = [];
            this.newSoaData.cedantId = null;
            this.newSoaData.treatyId = '';
            this.newSoaData.soaQuarter = null;

            getTreatyByCedant(null);

            this.newSoaDataDetail = null;
            this.$nextTick(function () {
                $("#cedantId").selectpicker('refresh');
                $("#treatyId").selectpicker('refresh');
            })
        },
        validateSoaData: function () {
            this.SoaDataValidation = [];
            if (this.newSoaData.cedantId == null || this.newSoaData.cedantId == "" || this.newSoaData.cedantId == 0)
                this.SoaDataValidation.push("Ceding Company is required");
            if (this.newSoaData.soaQuarter != null && this.newSoaData.soaQuarter != "")
                if (!/[0-9]{4} Q{1}([1-4]){1}/.test(this.newSoaData.soaQuarter))
                    this.SoaDataValidation.push("Soa Quarter format is incorrect");

            return this.SoaDataValidation.length == 0;
        },
        searchSoaData: function () {
            if (!this.validateSoaData())
                return;

            var obj = {
                cedantId: this.newSoaData.cedantId,                
                treatyId: this.newSoaData.treatyId,
                soaQuarter: this.newSoaData.soaQuarter,
            };

            var soaDataDetailBos;
            $.ajax({
                url: SoaDataUrl ? SoaDataUrl : null,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    soaDataDetailBos = data.soaDataBatchBos;
                }
            });

            this.selected = [];
            this.isCheckAll = false;

            this.newSoaDataDetail = soaDataDetailBos;
            if (this.newSoaDataDetail == null)
                this.SoaDataValidation.push("No record found");
        },
        addSoaData: function () {
            if (this.selected.length == 0) {
                this.SoaDataValidation.push("No Soa Data selected.");
                return;
            }

            var details = new Array();
            details = this.newSoaDataDetail.filter(f => this.selected.includes(f.Id))

            for (var k in details) {
                this.SoaDataDetails.push(details[k]);
            }
            this.SoaDataDetailMaxIndex++;

            $('#newSoaDataModal').modal('toggle');
        },
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.Status = this.InvoiceRegisterBatch.Status == 0 ? StatusPending : this.InvoiceRegisterBatch.Status;
            this.RemarkModal.ModuleId = this.InvoiceRegisterBatch.ModuleId;
            this.RemarkModal.ObjectId = this.InvoiceRegisterBatch.Id;
            this.RemarkModal.RemarkIsPrivate = false;
            this.RemarkModal.Content = null;
            this.RemarkModal.DocumentIsPrivate = false;
            this.RemarkModal.HasFollowUp = false;
            this.clearFollowUpDetails();

            clearSelectedFiles('remark');
        },
        clearFollowUpDetails: function (check) {
            this.RemarkModal.FollowUpAtStr = null;
            this.RemarkModal.FollowUpUserId = null;

            $('#FollowUpUserId').selectpicker("refresh");

            if ($('#hasFollowUp')[0].checked)
                $('#FollowUpUserId').prop("disabled", false);
            else {
                $('#FollowUpUserId').prop("disabled", true);
                console.log('entered');
            }

            $('#FollowUpUserId').selectpicker("refresh");
        },
        addRemark: function () {
            var remark = null;
            var remarkModal = this.RemarkModal;
            remarkModal.IsPrivate = this.RemarkModal.RemarkIsPrivate;
            if (this.RemarkModal.HasFollowUp) {
                remarkModal.RemarkFollowUpBo = {
                    FollowUpAtStr: this.RemarkModal.FollowUpAtStr,
                    FollowUpUserId: this.RemarkModal.FollowUpUserId,
                    Status: this.RemarkModal.HasFollowUp ? FollowUpStatusPending : FollowUpStatusCompleted,
                    StatusName: this.RemarkModal.HasFollowUp ? FollowUpStatusPendingName : FollowUpStatusCompletedName
                };
            }

            if (!this.InvoiceRegisterBatch.Id) {
                remark = remarkModal;
                remark.StatusName = StatusPendingName;
                remark.PermissionName = this.RemarkModal.RemarkIsPrivate ? "Private" : "Public";

                remark.DocumentBos = this.saveDocuments(this.RemarkMaxIndex);
            } else {
                var remark = createRemark(this.RemarkModal);
                if (remark)
                    remark.DocumentBos = this.saveDocuments(this.RemarkMaxIndex, remark.Id);
            }

            if (remark) {
                this.Remarks.unshift(Object.assign({}, remark));
                this.RemarkMaxIndex++;
            }
        },
        completeFollowUp: function (index) {
            if (this.Remarks[index].RemarkFollowUpBo == null)
                return;

            this.Remarks[index].RemarkFollowUpBo.Status = FollowUpStatusCompleted;
            this.Remarks[index].RemarkFollowUpBo.StatusName = FollowUpStatusCompletedName;
            if (this.InvoiceRegisterBatch.Id) {
                completeFollowUp(this.Remarks[index].RemarkFollowUpBo.Id);
            }
        },
        // Documents
        saveDocuments(remarkIndex, remarkId = null) {
            var files = getFiles();

            if (!files) return;

            var parentId = this.InvoiceRegisterBatch.Id;
            var document = {
                ModuleId: this.InvoiceRegisterBatch.ModuleId,
                ObjectId: parentId,
                RemarkId: remarkId,
                RemarkIndex: remarkIndex,
                IsPrivate: this.RemarkModal.DocumentIsPrivate,
                CreatedByName: AuthUserName,
                CreatedAtStr: this.RemarkModal.CreatedAtStr,
            };

            var save = true;
            if (!parentId)
                save = false;

            var documents = [];

            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                var document = saveDocument(document, file, save);

                if (document != null) {
                    document.Index = this.DocumentMaxIndex;

                    this.Documents.push(Object.assign({}, document));
                    documents.push(Object.assign({}, document));

                    this.DocumentMaxIndex++;
                }
            }

            return documents;
        },
        removeDocument(index) {
            var document = this.Documents[index];

            if (deleteDocument(document)) {
                if (document.RemarkId) {
                    var remark = this.Remarks.find(r => { return r.Id == document.RemarkId });
                    remark.DocumentBos = remark.DocumentBos.filter(d => { return d.Id != document.Id });
                } else {
                    for (var i = 0; i < this.Remarks.length; i++) {
                        var remark = this.Remarks[i];
                        if (remark.DocumentBos.filter(d => { return d.HashFileName == document.HashFileName }).length > 0) {
                            remark.DocumentBos = remark.DocumentBos.filter(d => { return d.HashFileName != document.HashFileName });
                            break;
                        }
                    }
                }

                this.Documents.splice(index, 1);
                this.DocumentMaxIndex--;
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
        openQuarterPicker: function (currentId) {
            var id = "#" + currentId;
            if ($(id).data("datepicker") != null) {
                $(id).datepicker("destroy");
            }

            $(id).datepicker({
                format: QuarterDateFormat,
                minViewMode: 1,
                autoclose: true,
                language: "qtrs",
                forceParse: false
            }).on('show', function (e) {
                var datepickerDropDown = $('.datepicker');
                datepickerDropDown.addClass('quarterpicker');
            });

            var updateQuarterValue = this.updateQuarterValue;
            $(id).on('changeDate', function () {
                updateQuarterValue(currentId);
            });

            $(id).focus();
        },
        openDatePicker: function (field) {
            var config = {
                format: DateFormatDatePickerJs,
                autoclose: true,
            };
            var id = '#' + field;

            if (id.includes("Quarter")) {
                var config = {
                    format: QuarterDateFormat,
                    minViewMode: 1,
                    autoclose: true,
                    language: "qtrs",
                    forceParse: false
                };
            }

            if (typeof $(id).data("datepicker") === 'undefined') {
                $(id).datepicker(config);

                if (id.includes("Quarter")) {
                    $(id).on('show', function (e) {
                        $('.datepicker').addClass('quarterpicker');
                    });
                }

                var setMatchValue = this.setMatchValue;
                $(id).on('changeDate', function () {
                    setMatchValue(field, $(id).val());
                });
            }

            $(id).focus();
        },
        setMatchValue: function (field, value) {
            var model = "InvoiceRegisterBatch";
            var fieldName = field;
            if (field.startsWith("RemarkModal")) {
                fieldName = field.replace('RemarkModal', '');

                model = "RemarkModal";
            }

            this[model][fieldName] = value;
        },
        updateQuarterValue: function (currentId) {
            var id = "#" + currentId;
            var value = $(id).val();

            this.newSoaData[currentId] = value;
        },
        setDateFormat: function (date) {
            if (date != '')
                return moment(date).format('DD MMM YYYY');
            return '';
        },
        setAmountFormat: function (amount) {
            if (amount == '' || amount == null)
                amount = 0;

            return amount.toFixed(5).replace(/(\d)(?=(\d{3})+\.)/g, "$1,");
        },
        checkAll: function () {
            this.isCheckAll = !this.isCheckAll;
            var selectedCheck = new Array();
            if (this.isCheckAll) { // Check all
                this.newSoaDataDetail.forEach(function (item) {
                    selectedCheck.push(item.Id);
                });
            } else {
                selectedCheck = [];
            }

            this.selected = selectedCheck;
        },
        updateCheckall: function () {
            if (this.selected.length == this.newSoaDataDetail.length) {
                this.isCheckAll = true;
            } else {
                this.isCheckAll = false;
            }
        },
        // Upload SUNGL File
        uploadFileUpdate: function () {
            var sunglFile = {
                CreatedAtStr: moment().format(DateTimeFormat),
                CreatedByName: AuthUserName,
            };

            var upload = $('#dataFile');
            var files = upload[0].files;

            if (!files) return;

            var sunglFiles = [];

            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                var filename = file.name;

                sunglFile.FileName = filename;

                var fileData = new FormData();
                fileData.append(filename, file);

                // Save Temp File
                $.ajax({
                    url: SaveTempUploadFileUrl,
                    type: "POST",
                    contentType: false,
                    processData: false,
                    cache: false,
                    async: false,
                    data: fileData,
                    success: function (data) {
                        if (data.error) {
                            alert(data.error);
                        } else {
                            sunglFile.TempFilePath = data.tempFilePath;
                            sunglFile.HashFileName = data.hashFileName;
                        }
                    }
                });
                console.log(sunglFile);
                this.Uploads.push(Object.assign({}, sunglFile));
                sunglFile.Index = this.DocumentMaxIndex;
                sunglFiles.push(Object.assign({}, sunglFile));
            }

            return sunglFiles;
        },
        urlDownloadLink: function (index) {
            var item = this.SunglFiles[index];
            var url = DownloadFileUrl + '?id=' + item.Id;
            return url
        },
        // Status History
        logFile: function (index) {
            var item = this.StatusHistories[index];
            if (item != null && item.Id != '') {
                return this.StatusFiles.filter(function (statusFileItem) {
                    return statusFileItem.StatusHistoryId === item.Id;
                })
            }
        },
        urlLogDownloadLink: function (index) {
            var item = this.StatusHistories[index];
            if (item != null && item.Id != '') {
                var url = DownloadStatusFileUrl + item.Id;
                return url
            }
        },
        // Compiled Summary
        changeReportingType: function () {
            this.ReportingTypeId = $('#ReportingTypeId').val();
            this.getCompiledSummary();
        },
        getCompiledSummary: function () {

            var obj = {
                InvoiceRegisterBatchId: this.InvoiceRegisterBatch.Id,
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
    },
    created: function () {
        this.CedantDetailMaxIndex = (this.CedantDetails) ? this.CedantDetails.length - 1 : -1;
        this.RemarkMaxIndex = (this.Remarks) ? this.Remarks.length - 1 : -1;

        this.SoaDataDetailMaxIndex = (this.SoaDataDetails) ? this.SoaDataDetails.length : 0;

        this.getCompiledSummary();
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});

function openDatePicker(id) {
    var id = '#' + id;
    if (typeof $(id).data("datepicker") === 'undefined') {
        $(id).datepicker({
            format: DateFormatDatePickerJs,
            autoclose: true,
        });
    }
    $(id).focus();
}

function updateStatus(status) {
    $('#Status').val(status);
    $('form').submit();
}