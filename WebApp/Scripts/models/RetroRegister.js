$(document).ready(function () {
    dateOffAutoComplete();

    $('#BatchDate, #FollowUpDate').datepicker({
        format: DateFormatDatePickerJs,
    });
});

function getTreatyCodeByCedant(cedantId) {

    if (!isNaN(cedantId)) {
        $.ajax({
            url: TreatyCodeByCedantUrl,
            type: "POST",
            data: { cedantId: cedantId },
            cache: false,
            async: false,
            success: function (data) {
                var treatyCodeList = data.treatyCodeBos;
                if (typeof treatyCodeList != "undefined")
                    refreshDropDownItems('treatyCodeId', treatyCodeList, null, 'Code', 'Description');
                else
                    refreshDropDownItems('treatyCodeId', [], null, 'Code', 'Description');
            }
        });
    }
    else {
        refreshDropDownItems('treatyCodeId', [], null, 'Code', 'Description');
    }
}


function focusOnDate(val) {
    $('#' + val).focus();
}

var app = new Vue({
    el: '#app',
    data: {
        RetroRegisterBatch: Model,
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
        BatchStatus: BatchStatusList ? BatchStatusList : [],
        StatusFiles: StatusFiles ? StatusFiles : [],
        // Retro Data
        RetroDataDetails: RetroDataDetailsList ? RetroDataDetailsList : [],
        RetroDataDetailMaxIndex: 0,
        newRetroData: {
            cedantId: null,
            soaQuarter: null,
            treatyCodeId: null,
        },
        newRetroDataDetail: null,
        RetroDataValidation: [],
        isCheckAll: false,
        selected: [],
        // SUNGL Files
        SunglFiles: SunglFileList ? SunglFileList : [],
        // Others
        DropDownCedants: DropDownCedants,
        DropDownUsers: DropDownUsers,
        DropDownTreatys: DropDownTreatys,

        textAreaWidth: 150,
        textAreaHeight: 21,
    },
    methods: {
        // Retro Data
        addNew: function () {
            this.RetroDataDetailMaxIndex++;
        },
        deleteRetroDataDetail: function (index) {
            this.RetroDataDetails.splice(index, 1);
            this.RetroDataDetailMaxIndex--;
        },
        resetModalData: function () {
            this.CedantDataValidation = [];
            this.newRetroData.cedantId = null;
            this.newRetroData.treatyCodeId = '';
            this.newRetroData.soaQuarter = null;

            getTreatyCodeByCedant(null);

            this.newRetroDataDetail = null;
            this.$nextTick(function () {
                $("#cedantId").selectpicker('refresh');
                $("#treatyCodeId").selectpicker('refresh');
            })
        },
        validateRetroData: function () {
            this.RetroDataValidation = [];
            if (this.newRetroData.cedantId == null || this.newRetroData.cedantId == "" || this.newRetroData.cedantId == 0)
                this.RetroDataValidation.push("Ceding Company is required");
            if (this.newRetroData.soaQuarter != null && this.newRetroData.soaQuarter != "")
                if (!/[0-9]{4} Q{1}([1-4]){1}/.test(this.newRetroData.soaQuarter))
                    this.RetroDataValidation.push("Soa Quarter format is incorrect");

            return this.RetroDataValidation.length == 0;
        },
        searchRetroData: function () {
            if (!this.validateRetroData())
                return;

            var obj = {
                cedantId: this.newRetroData.cedantId,
                treatyCodeId: this.newRetroData.treatyCodeId,
                soaQuarter: this.newRetroData.soaQuarter,
            };

            var retroDataDetailBos;
            $.ajax({
                url: RetroDataUrl ? RetroDataUrl : null,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    retroDataDetailBos = data.directRetroBos;
                }
            });

            this.selected = [];
            this.isCheckAll = false;

            this.newRetroDataDetail = retroDataDetailBos;
            if (this.newRetroDataDetail == null)
                this.RetroDataValidation.push("No record found");
        },
        addRetroData: function () {
            if (this.selected.length == 0) {
                this.RetroDataValidation.push("No Retro Data selected.");
                return;
            }

            var details = new Array();
            details = this.newRetroDataDetail.filter(f => this.selected.includes(f.Id));

            for (var k in details) {
                this.RetroDataDetails.push(details[k]);
            }
            this.RetroDataDetailMaxIndex++;

            $('#newRetroModal').modal('toggle');
        },
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.Status = this.RetroRegisterBatch.Status == 0 ? StatusPending : this.RetroRegisterBatch.Status;
            this.RemarkModal.ModuleId = this.RetroRegisterBatch.ModuleId;
            this.RemarkModal.ObjectId = this.RetroRegisterBatch.Id;
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

            if (!this.RetroRegisterBatch.Id) {
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
            if (this.ReferralClaim.Id) {
                completeFollowUp(this.Remarks[index].RemarkFollowUpBo.Id);
            }
        },
        // Documents
        saveDocuments(remarkIndex, remarkId = null) {
            var files = getFiles();

            if (!files) return;

            var parentId = this.RetroRegisterBatch.Id;
            var document = {
                ModuleId: this.RetroRegisterBatch.ModuleId,
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
            var model = "RetroRegisterBatch";
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

            this.newRetroData[currentId] = value;
        },
        setDateFormat: function (date) {
            if (date != '')
                return moment(date).format('DD MMM YYYY');
            return '';
        },
        checkAll: function () {
            this.isCheckAll = !this.isCheckAll;
            var selectedCheck = new Array();
            if (this.isCheckAll) { // Check all
                this.newRetroDataDetail.forEach(function (item) {
                    selectedCheck.push(item.Id);
                });
            } else {
                selectedCheck = [];
            }

            this.selected = selectedCheck;
        },
        updateCheckall: function () {
            if (this.selected.length == this.newRetroDataDetail.length) {
                this.isCheckAll = true;
            } else {
                this.isCheckAll = false;
            }
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
    },
    created: function () {
        this.CedantDetailMaxIndex = (this.CedantDetails) ? this.CedantDetails.length - 1 : -1;
        this.RemarkMaxIndex = (this.Remarks) ? this.Remarks.length - 1 : -1;
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


function submitForProcessing() {
    $('#Status').val(StatusSubmitForProcessing);
    $('form').submit();
}

function submitForGenerate() {
    $('#Status').val(StatusSubmitForGenerate);
    $('form').submit();
}