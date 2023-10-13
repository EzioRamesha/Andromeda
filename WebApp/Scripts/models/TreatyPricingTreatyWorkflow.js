var RecipientEmailCount = 0;
$(document).ready(function () {
    initializeTokenField('#recipientEmail', RecipientEmails, 'RecipientEmailCount', false, '', false);

    $('#addLinkedObjectModal').on("hidden.bs.modal", function (e) {
        if ($('#addNewModal:visible').length) { //Check if parent modal is opend after child modal is closed
            $('body').addClass('modal-open');
        }
    });

    loadingDiv.addClass('hide-loading-spinner');
});

function focusOnDate(val) {
    $('#' + val).focus();
}

function getVersionDetails(version) {
    //for quotation checklist use
}

function resetVersionDetails() {
    //for quotation checklist use
}

function setVersionCallBack(bo) {
    app.TreatyWorkflow.RequestDateStr = $('#RequestDateStr').val();
    app.TreatyWorkflow.TargetSentDateStr = $('#TargetSentDateStr').val();
    app.TreatyWorkflow.DateSentToReviewer1stStr = $('#DateSentToReviewer1stStr').val();
    app.TreatyWorkflow.DateSentToClient1stStr = $('#DateSentToClient1stStr').val();
    app.TreatyWorkflow.LatestRevisionDateStr = $('#LatestRevisionDateStr').val();
    app.TreatyWorkflow.SignedDateStr = $('#SignedDateStr').val();
    app.TreatyWorkflow.ReportedDateStr = $('#ReportedDateStr').val();
}

function calculateOrionGroup() {
    var effectiveAtModal = $("#EffectiveAtStr").val();
    var documentStatus = $("#DocumentStatus").val();
    var orionGroup = 1;
    var orionGroupStr = "";
    if (documentStatus != 2) {
        $.ajax({
            url: GetOrionGroupUrl,
            type: "POST",
            cache: false,
            async: false,
            data: { effectiveAt: effectiveAtModal },
            success: function (data) {
                if (data.OrionGroup >= 1 && data.OrionGroup <= 3) {
                    orionGroup = data.OrionGroup;
                } else {
                    orionGroup = "";
                }
            },
            error: function (request, status, error) {
                orionGroup = "";
            }
        });
        if (orionGroup == 1) {
            orionGroupStr = "<= 6 months";
        } else if (orionGroup == 2) {
            orionGroupStr = "<= 12 months";
        } else if (orionGroup == 3) {
            orionGroupStr = "> 12 months";
        } else {
            orionGroupStr = "";
        }
    }
    
    $("#orionGroupStr").val(orionGroupStr);
}

function orionGroupEmpty() {
    $("#orionGroupStr").val("");
}

var app = new Vue({
    el: '#app',
    data: {
        TreatyPricingTreatyWorkflows: TreatyPricingTreatyWorkflows ? TreatyPricingTreatyWorkflows : [],
        TreatyWorkflow: Model,
        TreatyWorkflowVersion: Model.CurrentVersionObject,
        LinkedObjects: [],
        TreatyWorkflowModal: {
            ReinsuranceTypePickListDetailId: "",
            DocumentType: "",
            CounterPartyDetailId: "",
            InwardRetroPartyDetailId: "",
            DocumentId: "",
            Description: "",
            TreatyPricingTreatyWorkflowObjectBos: [{
                ObjectModuleId: "",
                ObjectId: "",
                ObjectVersionId: "",
            }],
        },
        TreatyWorkflowAssignModal: {
            ReinsuranceTypePickListDetailId: "",
            DocumentType: "",
            CounterPartyDetailId: "",
            InwardRetroPartyDetailId: "",
            DocumentId: "",
            Description: "",
            DraftingPersonInCharge: "",
            Remarks: "",
        },
        TreatyWorkflowAssignId: "",
        LinkObjectModal: {
            ObjectModuleId: "",
            ObjectId: "",
            ObjectVersionId: "",
            ObjectModuleName: "",
            ObjectCode: "",
            ObjectName: "",
            ObjectVersion: "",
        },
        WorkflowObjects: TreatyPricingWorkflowObjectBos,

        DropDownObjects: [],
        DropDownObjectVersions: [],
        EditModuleCode: "",
        EditId: "",
        EditVersionId: "",
        // Remark
        Remarks: Remarks,
        RemarkSubjects: RemarkSubjects,
        RemarkMaxIndex: 0,
        RemarkModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            Status: null,
            CreatedAtStr: null,
            Content: null,
            ShowSubjectSelect: true,
        },
        // Status Histories
        StatusHistories: StatusHistories,
        StatusSelection: DraftingStatuses,
        StatusHistoryModal: {
            Version: 0,
            VersionStr: '0.0',
            Status: null,
            Remark: '',
            ModuleId: null,
            ObjectId: null,
        },
    },
    methods: {
        resetTreatyWorkflowModal() {
            $('#dropDownReinsuranceType').selectpicker('val', '');
            $('#dropDownDocumentType').selectpicker('val', '');
            $('#dropDownCounterParty').selectpicker('val', '');
            $('#dropDownInwardRetroParty').selectpicker('val', '');
            this.TreatyWorkflowModal.ReinsuranceTypePickListDetailId = "";
            this.TreatyWorkflowModal.DocumentType = "";
            this.TreatyWorkflowModal.CounterPartyDetailId = "";
            this.TreatyWorkflowModal.InwardRetroPartyDetailId = "";
            this.TreatyWorkflowModal.DocumentId = "";
            this.TreatyWorkflowModal.Description = "";
            this.TreatyWorkflowModal.TreatyPricingTreatyWorkflowObjectBos.length = 0;

            this.WorkflowObjects = [];

            $('#newTreatyError').empty();
            $('#newTreatyError').hide();
        },
        resetLinkObjectModal() {
            $('#dropDownObjectType').selectpicker('val', '');
            $('#dropDownObjectId').selectpicker('val', '');
            $('#dropDownObjectVersionId').selectpicker('val', '');
            this.LinkObjectModal.ObjectModuleId = "";
            this.LinkObjectModal.ObjectType = "";
            this.LinkObjectModal.ObjectId = "";
            this.LinkObjectModal.ObjectVersionId = "";
            this.LinkObjectModal.ObjectModuleName = "";
            this.LinkObjectModal.ObjectCode = "";
            this.LinkObjectModal.ObjectName = "";
            this.LinkObjectModal.ObjectVersion = "";

            $('#linkObjectError').empty();
            $('#linkObjectError').hide();

            if (ReadOnly) {
                $('#dropDownObjectType').prop('disabled', true);
                $('#dropDownObjectId').prop('disabled', true);
                $('#dropDownObjectVersionId').prop('disabled', true);
            }

            var objectEmpty = [];
            refreshDropDownItems("dropDownObjectId", objectEmpty, null, "Text");
            refreshDropDownItems("dropDownObjectVersionId", objectEmpty, null, "Text");
            this.$nextTick(function () {
                $('#dropDownObjectType').selectpicker('refresh');
            });
        },
        updateDropDownObjects() {
            var items = [];
            var objectTypeName = "";
            $.ajax({
                url: GetTreatyPricingObjectsUrl,
                type: "POST",
                cache: false,
                async: false,
                data: { type: this.LinkObjectModal.ObjectType },
                success: function (data) {
                    items = data.items;
                    objectTypeName = data.objectTypeName;
                }
            });
            this.LinkObjectModal.ObjectTypeName = objectTypeName;

            this.LinkObjectModal.ObjectVersionId = "";
            this.$nextTick(function () {
                refreshDropDownItems("dropDownObjectId", items, null, "Text", "", true, "Value");
                refreshDropDownItems("dropDownObjectVersionId", [], null, "Text");
            });

        },
        updateDropDownObjectVersions() {
            var items = [];
            var objectCode = "";
            var objectName = "";
            var objectType = this.LinkObjectModal.ObjectType;
            var objectId = this.LinkObjectModal.ObjectId;
            var ids = [];
            ids = this.WorkflowObjects.filter(function (o) { return o.ObjectType == objectType && o.ObjectId == objectId }).map(function (o) { return o.ObjectVersionId; });

            $.ajax({
                url: GetTreatyPricingObjectVersionsUrl,
                type: "POST",
                cache: false,
                async: false,
                data: {
                    type: objectType,
                    objectId: objectId,
                    existingVersionIds: ids,
                },
                success: function (data) {
                    items = data.items;
                    moduleName = data.moduleName;
                    objectCode = data.objectCode;
                    objectName = data.objectName;
                }
            });

            this.LinkObjectModal.ObjectCode = objectCode;
            this.LinkObjectModal.ObjectName = objectName;
            refreshDropDownItems("dropDownObjectVersionId", items, null, "Text", "", true, "Value");
        },
        // Edit 
        browseSharepoint() {
            var url = $('#SharepointLink').val()
            window.open(url, '_blank').focus();
        },
        // Index
        validateAddTreatyWorkflow() {
            $('#newTreatyError').empty();
            $('#newTreatyError').hide();

            var errors = [];

            if (this.TreatyWorkflowModal.ReinsuranceTypePickListDetailId == null || this.TreatyWorkflowModal.ReinsuranceTypePickListDetailId == "" || this.TreatyWorkflowModal.ReinsuranceTypePickListDetailId == "0")
                errors.push("Reinsurance Type is required");
            if (this.TreatyWorkflowModal.DocumentType == null || this.TreatyWorkflowModal.DocumentType == "" || this.TreatyWorkflowModal.DocumentType == "0")
                errors.push("Document Type is required");
            if (this.TreatyWorkflowModal.CounterPartyDetailId == null || this.TreatyWorkflowModal.CounterPartyDetailId == "" || this.TreatyWorkflowModal.CounterPartyDetailId == "0")
                errors.push("Counter Party is required");
            //if (this.WorkflowObjects.length == 0)
            //    errors.push("At least one object is required");

            if (errors.length > 0) {
                $('#newTreatyError').append(arrayToUnorderedList(errors));
                $('#newTreatyError').show();
            }

            return errors.length == 0;
        },
        saveTreatyWorkflow() {
            if (!this.validateAddTreatyWorkflow())
                return;

            var treatyWorkflowBo = $.extend({}, this.TreatyWorkflowModal);
            treatyWorkflowBo.TreatyPricingWorkflowObjectBos = this.WorkflowObjects;

            var errorList = [];
            var resultBo = [];
            $.ajax({
                url: AddTreatyWorkflowUrl,
                type: "POST",
                data: treatyWorkflowBo,
                cache: false,
                async: false,
                success: function (data) {
                    errorList = data.errors;
                    resultBo = data.bo;
                }
            });

            if (errorList.length > 0) {
                $('#newTreatyError').append(arrayToUnorderedList(errors));
                $('#newTreatyError').show();
            }
            else {
                window.location.href = EditTreatyWorkflowUrl + resultBo.Id;
            }
        },
        validateAddObject() {
            $('#linkObjectError').empty();
            $('#linkObjectError').hide();

            var errors = [];

            if (this.LinkObjectModal.ObjectType == null || this.LinkObjectModal.ObjectType == "" || this.LinkObjectModal.ObjectType == "0")
                errors.push("Object is required");
            if (this.LinkObjectModal.ObjectId == null || this.LinkObjectModal.ObjectId == "" || this.LinkObjectModal.ObjectId == "0")
                errors.push("Object ID is required");
            if (this.LinkObjectModal.ObjectVersionId == null || this.LinkObjectModal.ObjectVersionId == "" || this.LinkObjectModal.ObjectVersionId == "0")
                errors.push("Version is required");

            if (errors.length > 0) {
                $('#linkObjectError').append(arrayToUnorderedList(errors));
                $('#linkObjectError').show();
            }

            return errors.length == 0;
        },
        addWorkflowObject(save = false) {
            if (!this.validateAddObject())
                return;

            this.LinkObjectModal.ObjectVersion = $('#dropDownObjectVersionId option:selected').text();

            var workflowObjectBo = $.extend({}, this.LinkObjectModal);
            var workflowObject = null;
            if (save) {
                workflowObjectBo.Type = DocumentType;
                workflowObjectBo.WorkflowId = this.TreatyWorkflow.Id;

                $.ajax({
                    url: CreateWorkflowObjectUrl,
                    type: "POST",
                    cache: false,
                    async: false,
                    data: workflowObjectBo,
                    success: function (data) {
                        workflowObject = data.bo;
                    }
                });
            } else {
                workflowObject = workflowObjectBo;
            }

            if (workflowObject)
                this.WorkflowObjects.push(workflowObject);

            $('#addLinkedObjectModal').modal('hide');
        },
        removeWorkflowObject(index, save = false) {
            if (ReadOnly)
                return;

            if (save) {
                var id = this.WorkflowObjects[index].Id;

                $.ajax({
                    url: DeleteWorkflowObjectUrl,
                    type: "POST",
                    cache: false,
                    async: false,
                    data: { id },
                });
            }

            this.WorkflowObjects.splice(index, 1);
        },
        editObjectLink: function (index) {
            if (ReadOnly)
                return null;

            var item = this.WorkflowObjects[index];
            if (item != null && item.ObjectClassName != '' && item.ObjectId != '' && item.ObjectVersionId != '') {
                var editObjectUrl = BaseUrl + "/" + item.ObjectClassName + "/Edit?Id="; //editObjectUrl + item.Id;
                editObjectUrl = editObjectUrl + item.ObjectId + "&versionId=" + item.ObjectVersionId + "&isCalledFromWorkflow=true" + "&isQuotationWorkflow=false" + "&workflowId=" + this.TreatyWorkflow.Id;

                return editObjectUrl;
            }
        },
        // Shared
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

            if (!$(id).attr("readonly") && typeof $(id).data("datepicker") === 'undefined') {
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
        // Shared
        openDatePickerTargetSentDate: function (field) {

            var disabledDates = TargetSentDateDisableDates;
            var config = {
                format: DateFormatDatePickerJs,
                datesDisabled: disabledDates,
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

            if (!$(id).attr("readonly") && typeof $(id).data("datepicker") === 'undefined') {
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
        setMatchValue: function (id, value) {

            this.TreatyWorkflow[id] = value;
        },
        // Assign
        resetTreatyWorkflowAssignModal(id) {
            $('#dropDownAssignDraftingPersonInCharge').selectpicker('val', '');
            this.TreatyWorkflowAssignId = id;
            this.TreatyWorkflowAssignModal.DraftingPersonInCharge = "";
        },
        saveAssign() {
            var error = "";
            $.ajax({
                url: AssignPersonInChargeUrl,
                type: "POST",
                cache: false,
                async: false,
                data: {
                    id: this.TreatyWorkflowAssignId,
                    personInChargeId: this.TreatyWorkflowAssignModal.DraftingPersonInChargeId
                },
                success: function (data) {
                    error = data
                }
            });

            window.location.reload();
        },
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.CreatedByName = AuthUserName;
            this.RemarkModal.ModuleId = this.TreatyWorkflow.ModuleId;
            this.RemarkModal.ObjectId = this.TreatyWorkflow.Id;
            this.RemarkModal.SubModuleController = SubModuleController;
            this.RemarkModal.SubObjectId = this.TreatyWorkflow.Id;
            this.RemarkModal.Version = Model.CurrentVersion;
            this.RemarkModal.ShowSubjectSelect = false;
            this.RemarkModal.Content = null;

            this.toggleRemarkSubject();
            clearSelectedFiles('remark');
        },
        toggleRemarkSubject: function () {
            this.RemarkModal.Subject = null;
            this.$nextTick(function () {
                $('#subjectSelect').selectpicker('refresh');
            });

            this.RemarkModal.ShowSubjectSelect = !this.RemarkModal.ShowSubjectSelect;
            if (this.RemarkModal.ShowSubjectSelect) {
                $('#subjectSelect').selectpicker('show');
                $('#subjectText').hide();
            } else {
                $('#subjectSelect').selectpicker('hide');
                $('#subjectText').show();
            }
        },
        addRemark: function () {
            var remark = createRemark(this.RemarkModal);

            if (remark) {
                remark.DocumentBos = this.saveDocuments(this.RemarkMaxIndex, this.RemarkModal, remark.Id);

                this.Remarks.unshift(Object.assign({}, remark));

                if (!this.RemarkSubjects.includes(remark.Subject)) {
                    this.RemarkSubjects.push(remark.Subject);

                    this.$nextTick(function () {
                        $('#subjectSelect').selectpicker('refresh');
                    });
                }

                this.RemarkMaxIndex++;
            }
        },
        saveDocuments(remarkIndex, parent, remarkId = null) {
            var files = getFiles();

            if (!files) return;

            var parentId = this.TreatyWorkflow.Id;
            var document = {
                ModuleId: this.TreatyWorkflow.ModuleId,
                ObjectId: parentId,
                RemarkId: remarkId,
                RemarkIndex: remarkIndex,
                CreatedByName: AuthUserName,
                CreatedAtStr: parent.CreatedAtStr,
            };

            var documents = [];

            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                var document = saveDocument(document, file, true);

                if (document != null) {
                    documents.push(Object.assign({}, document));
                }
            }

            return documents;
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
        // Status History
        resetStatusHistoryModal: function () {
            this.StatusHistoryModal.Version = this.TreatyWorkflow.EditableVersion;
            this.StatusHistoryModal.VersionStr = this.TreatyWorkflow.EditableVersion + '.0';
            this.StatusHistoryModal.Status = null;
            this.StatusHistoryModal.Remark = '';
            this.StatusHistoryModal.ModuleId = this.TreatyWorkflow.ModuleId;
            this.StatusHistoryModal.ObjectId = this.TreatyWorkflow.Id;

            var status = this.TreatyWorkflow.DraftingStatus;
            this.StatusSelection = DraftingStatuses.filter(function (s) { return s.Value != status });

            clearSelectedFiles('status');

            $('#recipientEmail').tokenfield('setTokens', []);

            this.$nextTick(function () {
                $('#statusSelect').selectpicker('refresh');
            });
        },
        resetAddStatusHistoryError() {
            $('#addStatusHistoryError').empty();
            $('#addStatusHistoryError').hide();
        },
        validateStatus: function () {
            var errors = [];

            if (this.StatusHistoryModal.Status == null || this.StatusHistoryModal.Status == 0)
                errors.push("Status is required");

            if (this.StatusHistoryModal.Remark == '')
                errors.push("Remark is required");

            var fileErrors = validateFiles();
            errors = errors.concat(fileErrors);

            if (errors.length > 0) {
                $('#addStatusHistoryError').append(arrayToUnorderedList(errors));
                $('#addStatusHistoryError').show();
                return false;
            }
            return true;
        },
        updateStatus: function () {
            loadingDiv.removeClass('hide-loading-spinner');

            this.resetAddStatusHistoryError();
            if (!this.validateStatus()) {
                loadingDiv.addClass('hide-loading-spinner');
                return;
            }

            this.StatusHistoryModal.Emails = $('#recipientEmail').val();
            var statusHistory = createStatusHistory(this.StatusHistoryModal, "TreatyWorkflow");

            //if (statusHistory) {
            //    var statusName = statusHistory.StatusName;
            //    $("#draftingStatus").val(statusName);

            //    var remark = statusHistory.RemarkBo;
            //    if (remark) {
            //        remark.DocumentBos = this.saveDocuments(this.StatusHistories.length, this.StatusHistoryModal, remark.Id);
            //        statusHistory.RemarkBo = remark;
            //    }

            //    this.StatusHistories.unshift(Object.assign({}, statusHistory));

            //    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
            //        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            //    ];

            //    var now = new Date();
            //    now.setDate(now.getDate());

            //    if (now.getDate().length = 1) {
            //        now = String(now.getDate()).padStart(2, '0') + " " + monthNames[now.getMonth()] + " " + now.getFullYear();
            //    } else {
            //        now = now.getDate() + " " + monthNames[now.getMonth()] + " " + now.getFullYear();
            //    }

            //    this.TreatyWorkflow.LatestRevisionDateStr = now;
            //    this.updateLatestRevisionDate();
            //}
            //$('#statusHistoryModal').modal('hide');
        },
        updateLatestRevisionDate: function () {
            $.ajax({
                url: UpdateLatestRevisionDateUrl,
                type: "POST",
                cache: false,
                async: false,
                data: {
                    id: this.TreatyWorkflow.Id,
                    version: this.TreatyWorkflow.EditableVersion,
                    date: this.TreatyWorkflow.LatestRevisionDateStr
                },
            });
        },
        //Edit page
        removeLinkedObject(id) {
            var linkedObjects = [];
            $.ajax({
                url: RemoveLinkedObjectUrl,
                type: "POST",
                cache: false,
                async: false,
                data: {
                    workflowId: this.TreatyWorkflow.Id,
                    id: id,
                },
                success: function (data) {
                    linkedObjects = data.objectBos;
                }
            });

            this.LinkedObjects = linkedObjects;
        },
        saveSelectedObject() {
            if (this.LinkObjectModal.ObjectModuleId == null || this.LinkObjectModal.ObjectModuleId == "" || this.LinkObjectModal.ObjectModuleId == "0")
                this.LinkObjectDataValidation.push("Object is required");
            if (this.LinkObjectModal.ObjectId == null || this.LinkObjectModal.ObjectId == "" || this.LinkObjectModal.ObjectId == "0")
                this.LinkObjectDataValidation.push("Object ID is required");
            if (this.LinkObjectModal.ObjectVersionId == null || this.LinkObjectModal.ObjectVersionId == "" || this.LinkObjectModal.ObjectVersionId == "0")
                this.LinkObjectDataValidation.push("Version is required");

            if (this.LinkObjectDataValidation.length == 0) {
                var linkedObjects = [];
                var errorList = [];
                $.ajax({
                    url: AddLinkedObjectUrl,
                    type: "POST",
                    cache: false,
                    async: false,
                    data: {
                        treatyWorkflowObjectBos: this.TreatyWorkflow.TreatyPricingTreatyWorkflowObjectBos,
                        id: this.TreatyWorkflow.Id,
                        moduleId: this.LinkObjectModal.ObjectModuleId,
                        objectId: this.LinkObjectModal.ObjectId,
                        versionId: this.LinkObjectModal.ObjectVersionId,
                    },
                    success: function (data) {
                        linkedObjects = data.objectBos;
                        errorList = data.errors;
                    }
                });

                this.LinkedObjects = linkedObjects;

                if (errorList.length > 0) {
                    this.TreatyWorkflowDataValidation.push(errorList);
                }
            }
            else {
                text = "<ul>";
                for (i = 0; i < this.LinkObjectDataValidation.length; i++) {
                    text += "<li>" + this.LinkObjectDataValidation[i] + "</li>";
                }
                text += "</ul>";
                $('#linkObjectError').append(text);
                $('#linkObjectError').show();
                this.LinkObjectDataValidation = [];
                return;
            }

            $('#addLinkedObjectModal').modal('hide');
        },
    },

    created: function () {

    },
    updated() {

    }
});

$('#searchBtn').click(function (e) {
    var url = IndexUrl;

    routeValue.forEach(function (entry, index) {
        var inputValue = $('#' + entry.Key).val() ? $('#' + entry.Key).val() : "";

        if (index == 0) {
            url = url + "?" + entry.Key + "=" + inputValue;
        }
        else {
            url = url + "&" + entry.Key + "=" + inputValue;
        }
    });

    window.location.href = url;
});