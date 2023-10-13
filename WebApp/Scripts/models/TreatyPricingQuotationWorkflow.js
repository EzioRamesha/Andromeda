function focusOnDate(val) {
    $('#' + val).focus();
}

function getVersionDetails(version) {
    var checklistItems = [];
    $.ajax({
        url: GetVersionDetailUrl,
        type: "POST",
        cache: false,
        async: false,
        data: {
            quotationWorkflowId: Model.Id,
            quotationWorkflowVersion: version,
            isEditMode: IsEditMode
        },
        success: function (data) {
            checklistItems = data.quotationChecklistBos;
        }
    });
    app.ChecklistItems = checklistItems;
    resetVersionTokenfields();
}

function resetVersionTokenfields() {
    app.ChecklistItems.forEach(function (item, index) {
        var action = item.DisablePersonInCharge ? 'disable' : 'enable';
        $('#InternalTeamPersonInCharge_' + index).tokenfield(action);
    });
}

function resetVersionDetails() {
    //Get and set latest version
    var version = 0;
    $.ajax({
        url: GetLatestVersionUrl,
        type: "POST",
        cache: false,
        async: false,
        data: {
            id: Model.Id,
        },
        success: function (data) {
            version = data.version;
        }
    });

    if (version > 0) {
        $('#LatestVersion').val(version);
    }
}

var setVersionCallBack = function (bo) {
    if (disableVersion) {
        $('#quoteSpecFile').prop('disabled', true);
        $('#rateTableFile').prop('disabled', true);
    } else {
        $('#quoteSpecFile').prop('disabled', false);
        $('#rateTableFile').prop('disabled', false);
    }
}

function togglePricingTeam() {
    var code = "";

    $.ajax({
        url: GetPickListDetailCodeUrl,
        type: "POST",
        cache: false,
        async: false,
        data: {
            pricingTeamPickListDetailId: $("#PricingTeamPickListDetailId").val(),
        },
        success: function (data) {
            code = data.code;
            var dropdownList;

            if (code == "PP") {
                dropdownList = DropDownPersonInChargePPT;
            } else {
                dropdownList = DropDownPersonInChargeGroupPricing;
            }

            $("#PersonInChargeId").empty();
            $("#PersonInChargeTechReviewerId").empty();
            $("#PersonInChargePeerReviewerId").empty();
            $("#PersonInChargePricingAuthorityReviewerId").empty();

            $.each(dropdownList, function (key, value) {
                $("#PersonInChargeId").append($("<option></option>")
                    .attr("value", value.Value).text(value.Text));

                $("#PersonInChargeTechReviewerId").append($("<option></option>")
                    .attr("value", value.Value).text(value.Text));

                $("#PersonInChargePeerReviewerId").append($("<option></option>")
                    .attr("value", value.Value).text(value.Text));

                $("#PersonInChargePricingAuthorityReviewerId").append($("<option></option>")
                    .attr("value", value.Value).text(value.Text));
            });

            $("#PersonInChargeId").selectpicker('refresh');
            $("#PersonInChargeTechReviewerId").selectpicker('refresh');
            $("#PersonInChargePeerReviewerId").selectpicker('refresh');
            $("#PersonInChargePricingAuthorityReviewerId").selectpicker('refresh');
        }
    });
}

var UsersCEOCount = 0;
var UsersPricingCount = 0;
var UsersUnderwritingCount = 0;
var UsersHealthCount = 0;
var UsersClaimsCount = 0;
var UsersBDCount = 0;
var UsersTGCount = 0;

var RecipientEmailCount = 0;

$(document).ready(function () {
    var version = $('#CurrentVersion').val();
    getVersionDetails(version);

    initializeTokenField('#recipientEmail', RecipientEmails, 'RecipientEmailCount', false, '', false);

    $('#addLinkedObjectModal').on("hidden.bs.modal", function (e) {
        if ($('#addQuotationWorkflowModal:visible').length) { //Check if parent modal is opend after child modal is closed
            $('body').addClass('modal-open');
        }
    });

    togglePricingTeam();

    $("#DownloadQuoteSpecFinalFile").click(function (e) {
        if (app.QuotationWorkflowVersion.FinalQuoteSpecHashFileName == null || app.QuotationWorkflowVersion.FinalQuoteSpecHashFileName == "") {
            e.preventDefault();
            $('#downloadFinalFileErrorModal').modal('show');
        }
    });

    $("#DownloadRateTableFinalFile").click(function (e) {
        if (app.QuotationWorkflowVersion.FinalRateTableHashFileName == null || app.QuotationWorkflowVersion.FinalRateTableHashFileName == "") {
            e.preventDefault();
            $('#downloadFinalFileErrorModal').modal('show');
        }
    });

    loadingDiv.addClass('hide-loading-spinner');
});

function initializeUserTokenFields() {
    $(document).ready(function () {
        initializeTokenField('#InternalTeamPersonInCharge_0', UsersCEO, 'UsersCEOCount', true, 'ChecklistItems');
        initializeTokenField('#InternalTeamPersonInCharge_1', UsersPricing, 'UsersPricingCount', true, 'ChecklistItems');
        initializeTokenField('#InternalTeamPersonInCharge_2', UsersUnderwriting, 'UsersUnderwritingCount', true, 'ChecklistItems');
        initializeTokenField('#InternalTeamPersonInCharge_3', UsersHealth, 'UsersHealthCount', true, 'ChecklistItems');
        initializeTokenField('#InternalTeamPersonInCharge_4', UsersClaims, 'UsersClaimsCount', true, 'ChecklistItems');
        initializeTokenField('#InternalTeamPersonInCharge_5', UsersBD, 'UsersBDCount', true, 'ChecklistItems');
        initializeTokenField('#InternalTeamPersonInCharge_6', UsersTG, 'UsersTGCount', true, 'ChecklistItems');

        resetVersionTokenfields();
    });
}

var app = new Vue({
    el: '#app',
    data: {
        TreatyPricingQuotationWorkflows: TreatyPricingQuotationWorkflows ? TreatyPricingQuotationWorkflows : [],
        QuotationWorkflow: Model,
        QuotationWorkflowVersion: Model.CurrentVersionObject,
        ChecklistItems: [],
        QuotationWorkflowModal: {
            CedantId: "",
            ReinsuranceTypePickListDetailId: "",
            Name: "",
            Description: "",
            PricingTeamPickListDetailId: "",
            TreatyPricingQuotationWorkflowObjectBos: [{
                ObjectType: "",
                ObjectId: "",
                ObjectVersionId: "",
            }],
        },
        LinkObjectModal: {
            ObjectType: "",
            ObjectId: "",
            ObjectVersionId: "",
            ObjectTypeName: "",
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

        //SharePoint
        SharePointGenerateValidation: [],
        SharePointGenerateConfirmation: [],
        SharePointGenerateModal: {
            type: "",
            typeFull: "",
            template: "",
            sharePointFolderPath: "",
            sharePointLink: "",
            newFileName: "",
            localPath: "",
            quoteSpecEditLink: "",
            rateTableEditLink: "",
            isCampaignSpec: false,
        },

        //Final file upload
        QuoteSpecUploadValidation: [],
        RateTableUploadValidation: [],

        //Checklist
        NotifyChecklistValidation: [],

        //Remark
        Remarks: [],
        QuotationRemarks: QuotationRemarks,
        PricingRemarks: PricingRemarks,
        RemarkSubjects: RemarkSubjects,
        QuotationRemarkMaxIndex: 0,
        PricingRemarkMaxIndex: 0,
        RemarkModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            Status: null,
            CreatedAtStr: null,
            Content: null,
            ShowSubjectSelect: true,
        },

        // Status Histories
        StatusHistories: StatusHistories,
        PricingStatusHistories: PricingStatusHistories,
        ChecklistStatusHistories: ChecklistStatusHistories,
        StatusSelection: [],
        StatusHistoryModal: {
            Version: 0,
            VersionStr: '0.0',
            Status: null,
            Remark: '',
            ModuleId: null,
            ObjectId: null,
            SubModuleController: null,
        },
        ChecklistStatusHistoryModal: {
            Version: 0,
            VersionStr: '0.0',
            Status: null,
            Remark: '',
            ModuleId: null,
            ObjectId: null,
            SubModuleController: null,
            SubObjectId: 0,
        },
    },
    methods: {
        resetQuotationWorkflowModal() {
            $('#dropDownCedant').selectpicker('val', '');
            $('#dropDownReinsuranceType').selectpicker('val', '');
            $('#dropDownPricingTeam').selectpicker('val', '');
            this.QuotationWorkflowModal.CedantId = "";
            this.QuotationWorkflowModal.ReinsuranceTypePickListDetailId = "";
            this.QuotationWorkflowModal.Name = "";
            this.QuotationWorkflowModal.Description = "";
            this.QuotationWorkflowModal.PricingTeamPickListDetailId = "";
            this.QuotationWorkflowModal.TreatyPricingQuotationWorkflowObjectBos.length = 0;

            this.WorkflowObjects = [];

            $('#newQuotationError').empty();
            $('#newQuotationError').hide();
        },
        resetLinkObjectModal() {
            $('#dropDownObjectType').selectpicker('val', '');
            $('#dropDownObjectId').selectpicker('val', '');
            $('#dropDownObjectVersionId').selectpicker('val', '');
            this.LinkObjectModal.ObjectType = "";
            this.LinkObjectModal.ObjectId = "";
            this.LinkObjectModal.ObjectVersionId = "";
            this.LinkObjectModal.ObjectTypeName = "";
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
            var treatyPricingCedantId = 0;
            $.ajax({
                url: GetTreatyPricingCedantIdUrl,
                type: "POST",
                cache: false,
                async: false,
                data: {
                    cedantId: this.QuotationWorkflow.CedantId,
                    reinsuranceTypePickListDetailId: this.QuotationWorkflow.ReinsuranceTypePickListDetailId,
                },
                success: function (data) {
                    treatyPricingCedantId = data.treatyPricingCedantId;
                }
            });

            var items = [];
            var objectTypeName = "";
            $.ajax({
                url: GetTreatyPricingObjectsUrl,
                type: "POST",
                cache: false,
                async: false,
                data: {
                    type: this.LinkObjectModal.ObjectType,
                    cedantId: treatyPricingCedantId,
                },
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

        //Index page
        validateAddQuotationWorkflow() {
            $('#linkObjectError').empty();
            $('#linkObjectError').hide();

            var errors = [];

            if (this.QuotationWorkflowModal.CedantId == null || this.QuotationWorkflowModal.CedantId == "")
                errors.push("Ceding Company is required");
            if (this.QuotationWorkflowModal.ReinsuranceTypePickListDetailId == null || this.QuotationWorkflowModal.ReinsuranceTypePickListDetailId == "" || this.QuotationWorkflowModal.ReinsuranceTypePickListDetailId == "0")
                errors.push("Reinsurance Type is required");
            if (this.QuotationWorkflowModal.Name == null || this.QuotationWorkflowModal.Name == "")
                errors.push("Quotation Name is required");
            if (this.QuotationWorkflowModal.Description == null || this.QuotationWorkflowModal.Description == "")
                errors.push("Description is required");
            if (this.QuotationWorkflowModal.PricingTeamPickListDetailId == null || this.QuotationWorkflowModal.PricingTeamPickListDetailId == "" || this.QuotationWorkflowModal.PricingTeamPickListDetailId == "0")
                errors.push("Pricing Team is required");
            if (this.WorkflowObjects.length == 0)
                errors.push("At least one object is required");

            if (errors.length > 0) {
                $('#newQuotationError').append(arrayToUnorderedList(errors));
                $('#newQuotationError').show();
            }

            return errors.length == 0;
        },
        saveQuotationWorkflow() {
            if (!this.validateAddQuotationWorkflow())
                return;

            var quotationWorkflowBo = $.extend({}, this.QuotationWorkflowModal);
            quotationWorkflowBo.TreatyPricingWorkflowObjectBos = this.WorkflowObjects;

            var errorList = [];
            var resultBo = [];
            $.ajax({
                url: AddQuotationWorkflowUrl,
                type: "POST",
                data: quotationWorkflowBo,
                cache: false,
                async: false,
                success: function (data) {
                    errorList = data.errors;
                    resultBo = data.quotationWorkflowBo;
                }
            });

            if (errorList.length > 0) {
                $('#newQuotationError').append(arrayToUnorderedList(errors));
                $('#newQuotationError').show();
            }
            else {
                window.location.href = EditQuotationWorkflowUrl + resultBo.Id;
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

            for (var i = 0; i < this.WorkflowObjects.length; i++) {
                if (this.WorkflowObjects[i].ObjectType == this.LinkObjectModal.ObjectType) {
                    errors.push("An object for this module type has been selected");
                }
            }

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
                workflowObjectBo.WorkflowId = this.QuotationWorkflow.Id;

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
            //if (ReadOnly)
            //    return null;

            var item = this.WorkflowObjects[index];
            if (item != null && item.ObjectClassName != '' && item.ObjectId != '' && item.ObjectVersionId != '') {
                var editObjectUrl = BaseUrl + "/" + item.ObjectClassName + "/Edit?Id="; //editObjectUrl + item.Id;
                editObjectUrl = editObjectUrl + item.ObjectId + "&versionId=" + item.ObjectVersionId + "&isCalledFromWorkflow=true" + "&isQuotationWorkflow=true" + "&workflowId=" + this.QuotationWorkflow.Id;

                return editObjectUrl;
            }
        },

        //SharePoint
        resetSharePointGenerateModal() {
            this.SharePointGenerateModal.type = "";
            this.SharePointGenerateModal.typeFull = "";
            this.SharePointGenerateModal.template = "";
            this.SharePointGenerateModal.sharePointFolderPath = "";
            this.SharePointGenerateModal.sharePointLink = "";
            this.SharePointGenerateModal.newFileName = "";
            this.SharePointGenerateModal.localPath = "";
            this.SharePointGenerateModal.isCampaignSpec = false;
            //this.SharePointGenerateModal.quoteSpecEditLink = "";
            //this.SharePointGenerateModal.rateTableEditLink = "";

            this.SharePointGenerateValidation = [];
        },
        generateSharePointFile: function (type) {
            $("#loadingSpinner").removeClass('hide-loading-spinner');

            this.resetSharePointGenerateModal();
            this.SharePointGenerateModal.type = type;

            if (type == "QuoteSpec") {
                this.SharePointGenerateModal.template = this.QuotationWorkflowVersion.QuoteSpecTemplate;
                this.SharePointGenerateModal.sharePointFolderPath = this.QuotationWorkflowVersion.QuoteSpecSharePointFolderPath;
                this.SharePointGenerateModal.sharePointLink = this.QuotationWorkflowVersion.QuoteSpecSharePointLink;
                this.SharePointGenerateModal.typeFull = "Campaign / Quote Spec";
            }
            else {
                this.SharePointGenerateModal.template = this.QuotationWorkflowVersion.RateTableTemplate;
                this.SharePointGenerateModal.sharePointFolderPath = this.QuotationWorkflowVersion.RateTableSharePointFolderPath;
                this.SharePointGenerateModal.sharePointLink = this.QuotationWorkflowVersion.RateTableSharePointLink;
                this.SharePointGenerateModal.typeFull = "Rate Table";
            }

            if (this.SharePointGenerateModal.template == null || this.SharePointGenerateModal.template == "") {
                this.SharePointGenerateValidation.push(this.SharePointGenerateModal.typeFull + " Template is required");
                $("#loadingSpinner").addClass('hide-loading-spinner');
            }
            //if (this.SharePointGenerateModal.sharePointFolderPath == null || this.SharePointGenerateModal.sharePointFolderPath == "")
            //    this.SharePointGenerateValidation.push(this.SharePointGenerateModal.typeFull + " SharePoint Path is required");

            if (this.SharePointGenerateValidation.length > 0) {
                $('#sharePointGenerateErrorModal').modal('show');
                return;
            }
            else {
                var setEditLink = this.setEditLink;
                var displaySharePointErrors = this.displaySharePointErrors;
                var displaySharePointConfirmation = this.displaySharePointConfirmation;
                var setSharePointValues = this.setSharePointValues;

                $.ajax({
                    url: GenerateSharePointFileUrl ? GenerateSharePointFileUrl : null,
                    type: "POST",
                    data: {
                        versionId: this.QuotationWorkflowVersion.Id,
                        type: type,
                        typeFull: this.SharePointGenerateModal.typeFull,
                        templateCode: this.SharePointGenerateModal.template,
                        sharePointFolderPath: this.SharePointGenerateModal.sharePointFolderPath,
                        sharePointLink: this.SharePointGenerateModal.sharePointLink,
                    },
                    cache: false,
                    async: true,
                    success: function (data) {
                        $("#loadingSpinner").addClass('hide-loading-spinner');

                        setSharePointValues(data.fileName, data.path, data.isCampaignSpec);

                        if (data.errors.length > 0) {
                            displaySharePointErrors(data.errors);
                            return;
                        }
                        else {
                            if (data.confirmations.length > 0) {
                                displaySharePointConfirmation(data.confirmations);
                                return;
                            }
                            else {
                                setEditLink(data.editLink);

                                $('#sharePointGenerateSuccessModal').modal('show');
                                return;
                            }
                        }
                    }
                });
            }
        },
        generateSharePointFileConfirmed: function () {
            $("#loadingSpinner").removeClass('hide-loading-spinner');
            var setEditLink = this.setEditLink;
            var displaySharePointErrors = this.displaySharePointErrors;

            $.ajax({
                url: GenerateSharePointFileConfirmedUrl ? GenerateSharePointFileConfirmedUrl : null,
                type: "POST",
                data: {
                    versionId: this.QuotationWorkflowVersion.Id,
                    type: this.SharePointGenerateModal.type,
                    newFileName: this.SharePointGenerateModal.newFileName,
                    sharePointFolderPath: this.SharePointGenerateModal.sharePointFolderPath,
                    localPath: this.SharePointGenerateModal.localPath,
                    isCampaignSpec: this.SharePointGenerateModal.isCampaignSpec,
                },
                cache: false,
                async: true,
                success: function (data) {
                    $("#loadingSpinner").addClass('hide-loading-spinner');

                    if (data.errors.length > 0) {
                        displaySharePointErrors(data.errors);
                        return;
                    }
                    else {
                        setEditLink(data.editLink);

                        $('#sharePointGenerateSuccessModal').modal('show');
                        return;
                    }
                }
            });
        },
        setEditLink: function (editLink) {
            if (this.SharePointGenerateModal.type == "QuoteSpec") {
                this.SharePointGenerateModal.quoteSpecEditLink = editLink;
                $('#QuoteSpecSharePointLink').val = editLink;
                this.QuotationWorkflowVersion.QuoteSpecSharePointLink = editLink;
            }
            else {
                this.SharePointGenerateModal.rateTableEditLink = editLink;
                $('#RateTableSharePointLink').val = editLink;
                this.QuotationWorkflowVersion.RateTableSharePointLink = editLink;
            }
        },
        displaySharePointErrors: function (errorList) {
            this.SharePointGenerateValidation = errorList;
            $('#sharePointGenerateErrorModal').modal('show');
        },
        displaySharePointConfirmation: function (confirmationList) {
            this.SharePointGenerateConfirmation = confirmationList;
            $('#sharePointGenerateConfirmationModal').modal('show');
        },
        setSharePointValues: function (fileName, path, isCampaignSpec) {
            this.SharePointGenerateModal.newFileName = fileName;
            this.SharePointGenerateModal.localPath = path;
            this.SharePointGenerateModal.isCampaignSpec = isCampaignSpec;
        },
        editSharePointFile: function (type) {
            var editLink = "";
            if (type == "QuoteSpec") {
                editLink = this.QuotationWorkflowVersion.QuoteSpecSharePointLink;

                if (editLink != null && editLink != "") {
                    window.open(editLink, '_blank');
                }
                else {
                    editLink = this.SharePointGenerateModal.quoteSpecEditLink;
                    if (editLink != null && editLink != "") {
                        window.open(editLink, '_blank');
                    }
                    else {
                        $('#sharePointEditErrorModal').modal('show');
                        return;
                    }
                }
            }
            else {
                editLink = this.QuotationWorkflowVersion.RateTableSharePointLink;

                if (editLink != null && editLink != "") {
                    window.open(editLink, '_blank');
                }
                else {
                    editLink = this.SharePointGenerateModal.rateTableEditLink;
                    if (editLink != null && editLink != "") {
                        window.open(editLink, '_blank');
                    }
                    else {
                        $('#sharePointEditErrorModal').modal('show');
                        return;
                    }
                }
            }
        },

        //Final file upload
        uploadQuoteSpecFile: function () {
            var selectedFiles = document.querySelector("#quoteSpecSelectedFiles");
            var upload = $('#quoteSpecFile');
            selectedFiles.innerHTML = "";
            if (!upload[0].files[0]) return;

            var files = upload[0].files;
            var list = "<li>" + files[0].name + "</li>";
            selectedFiles.innerHTML = "<ul>" + list + "</ul>";
        },
        uploadRateTableFile: function () {
            var selectedFiles = document.querySelector("#rateTableSelectedFiles");
            var upload = $('#rateTableFile');
            selectedFiles.innerHTML = "";
            if (!upload[0].files[0]) return;

            var files = upload[0].files;
            var list = "<li>" + files[0].name + "</li>";
            selectedFiles.innerHTML = "<ul>" + list + "</ul>";
        },
        saveUploadQuoteSpecFile: function () {
            var upload = $('#quoteSpecFile');
            var file = upload[0].files[0];
            var fileData = new FormData()
            fileData.append(file.name, file);

            fileData.append('versionId', this.QuotationWorkflowVersion.Id);
            fileData.append('type', 'QuoteSpec');

            var errorList = [];
            var finalHashFileName = "";
            $.ajax({
                url: UploadFinalFileUrl,
                type: "POST",
                contentType: false,
                processData: false,
                cache: false,
                async: false,
                data: fileData,
                success: function (data) {
                    finalHashFileName = data.finalHashFileName;
                    if (data.errors && data.errors.length > 0) {
                        errorList = data.errors;
                    }
                }
            });

            if (errorList.length > 0) {
                this.QuoteSpecUploadValidation.push(errorList[i]);
                $('#uploadQuoteSpecErrorModal').modal('show');
                return;
            } else {
                //window.location.reload();
                $('#FinalQuoteSpecFileName').val(file.name);
                this.QuotationWorkflowVersion.FinalQuoteSpecHashFileName = finalHashFileName;
                $('#FinalQuoteSpecHashFileName').val(finalHashFileName);

                var selectedFiles = document.querySelector("#quoteSpecSelectedFiles");
                selectedFiles.innerHTML = "";
            }
        },
        saveUploadRateTableFile: function () {
            var upload = $('#rateTableFile');
            var file = upload[0].files[0];
            var fileData = new FormData()
            fileData.append(file.name, file);

            fileData.append('versionId', this.QuotationWorkflowVersion.Id);
            fileData.append('type', 'RateTable');

            var errorList = [];
            var finalHashFileName = "";
            $.ajax({
                url: UploadFinalFileUrl,
                type: "POST",
                contentType: false,
                processData: false,
                cache: false,
                async: false,
                data: fileData,
                success: function (data) {
                    finalHashFileName = data.finalHashFileName;
                    if (data.errors && data.errors.length > 0) {
                        errorList = data.errors;
                    }
                }
            });

            if (errorList.length > 0) {
                this.RateTableUploadValidation.push(errorList[i]);
                $('#uploadRateTableErrorModal').modal('show');
                return;
            } else {
                //window.location.reload();
                $('#FinalRateTableFileName').val(file.name);
                this.QuotationWorkflowVersion.FinalRateTableHashFileName = finalHashFileName;
                $('#FinalRateTableHashFileName').val(finalHashFileName);

                var selectedFiles = document.querySelector("#rateTableSelectedFiles");
                selectedFiles.innerHTML = "";
            }
        },

        //Checklist
        saveChecklist: function () {
            $('#QuotationChecklists').val(JSON.stringify(this.ChecklistItems));
        },
        getChecklistStatusName: function (status) {
            var statusName = "";
            $.ajax({
                url: GetChecklistStatusNameUrl,
                type: "POST",
                cache: false,
                async: false,
                data: { status: status },
                success: function (data) {
                    statusName = data.statusName;
                }
            });

            return statusName;
        },
        sendChecklistNotificationEmail: function (internalTeamPersonInCharge, quotationId, status) {
            var errorList = [];
            $.ajax({
                url: NotifyQuotationChecklistUrl,
                type: "POST",
                data: {
                    quotationId: quotationId,
                    internalTeam: internalTeamPersonInCharge,
                    status: status,
                },
                cache: false,
                async: false,
                success: function (data) {
                    errorList = data.errors;
                },
            });

            this.NotifyChecklistValidation = errorList;
        },
        checklistNotRequired: function (index) {
            this.ChecklistItems[index].Status = StatusNotRequired;
            this.ChecklistItems[index].StatusName = this.getChecklistStatusName(StatusNotRequired);
            this.ChecklistItems[index].InternalTeamPersonInCharge = null;

            // Reset InternalPersonInCharge
            $('#InternalTeamPersonInCharge_' + index).tokenfield('setTokens', []);
            $('#InternalTeamPersonInCharge_' + index + '-tokenfield').attr("placeholder", "Type here");

            this.saveChecklist();
            this.saveIndividualStatus(StatusNotRequired, this.ChecklistItems[index].Id, this.ChecklistItems[index].InternalTeamPersonInCharge);
        },
        checklistRequest: function (index) {
            this.NotifyChecklistValidation = [];

            if (this.ChecklistItems[index].InternalTeamPersonInCharge != null && this.ChecklistItems[index].InternalTeamPersonInCharge != "") {
                this.sendChecklistNotificationEmail(this.ChecklistItems[index].InternalTeamPersonInCharge, this.QuotationWorkflow.QuotationId, StatusRequested);

                this.ChecklistItems[index].Status = StatusRequested;
                this.ChecklistItems[index].StatusName = this.getChecklistStatusName(StatusRequested);
                this.saveChecklist();
                this.saveIndividualStatus(StatusRequested, this.ChecklistItems[index].Id, this.ChecklistItems[index].InternalTeamPersonInCharge);

                if (this.NotifyChecklistValidation.length > 0) {
                    $('#emailErrorModal').modal('show');
                }
                else {
                    $('#emailSentModal').modal('show');
                }
            } else {
                this.NotifyChecklistValidation.push("Person in charge is required.");
                $('#emailErrorModal').modal('show');
            }
        },
        checklistCompleted: function (index) {
            if (this.ChecklistItems[index].InternalTeamPersonInCharge != null && this.ChecklistItems[index].InternalTeamPersonInCharge != "") {
                this.ChecklistItems[index].Status = StatusCompleted;
                this.ChecklistItems[index].StatusName = this.getChecklistStatusName(StatusCompleted);
                this.saveChecklist();
                this.saveIndividualStatus(StatusCompleted, this.ChecklistItems[index].Id, this.ChecklistItems[index].InternalTeamPersonInCharge);
            }
        },
        checklistSignOff: function (index) {
            if (this.ChecklistItems[index].InternalTeamPersonInCharge != null && this.ChecklistItems[index].InternalTeamPersonInCharge != "") {
                this.ChecklistItems[index].Status = StatusApproved;
                this.ChecklistItems[index].StatusName = this.getChecklistStatusName(StatusApproved);
                this.saveChecklist();
                this.saveIndividualStatus(StatusApproved, this.ChecklistItems[index].Id, this.ChecklistItems[index].InternalTeamPersonInCharge);
            }
        },
        checklistFinalise: function () {
            var countFinalised = 0;

            var enableFinalise = true;
            var filtered = this.ChecklistItems.filter(function (item) { return item.Status != StatusCompleted && item.Status != StatusNotRequired });

            if (filtered.length > 0) {
                enableFinalise = false;
                $('#finaliseErrorModal').modal('show');
                return;
            }

            for (var i = 0; i < this.ChecklistItems.length; i++) {
                if (this.ChecklistItems[i].Status == StatusCompleted && this.ChecklistItems[i].InternalTeamPersonInCharge != null && this.ChecklistItems[i].InternalTeamPersonInCharge != "") {
                    this.sendChecklistNotificationEmail(this.ChecklistItems[i].InternalTeamPersonInCharge, this.QuotationWorkflow.QuotationId, StatusPendingSignOff);

                    this.ChecklistItems[i].Status = StatusPendingSignOff;
                    this.ChecklistItems[i].StatusName = this.getChecklistStatusName(StatusPendingSignOff);
                    this.saveChecklist();
                    this.saveIndividualStatus(StatusPendingSignOff, this.ChecklistItems[i].Id, this.ChecklistItems[i].InternalTeamPersonInCharge);

                    countFinalised++;
                }
            }

            if (countFinalised > 0) {
                this.QuotationWorkflowVersion.ChecklistFinalised = true;
                $('#ChecklistFinalised').val(true);

                $.ajax({
                    url: UpdateVersionChecklistFinalisedUrl,
                    type: "POST",
                    data: {
                        versionId: this.QuotationWorkflowVersion.Id,
                        isFinalise: true,
                    },
                    cache: false,
                    async: false,
                    success: function (data) { }
                });

                if (this.NotifyChecklistValidation.length > 0) {
                    $('#emailErrorModal').modal('show');
                }
                else {
                    $('#emailSentModal').modal('show');
                }
            }
        },
        checklistNotify: function () {
            this.NotifyChecklistValidation = [];
            var countNotified = 0;

            for (var i = 0; i < this.ChecklistItems.length; i++) {
                if ((this.ChecklistItems[i].Status == StatusRequested || this.ChecklistItems[i].Status == StatusPendingSignOff) && this.ChecklistItems[i].InternalTeamPersonInCharge != null && this.ChecklistItems[i].InternalTeamPersonInCharge != "") {
                    this.sendChecklistNotificationEmail(this.ChecklistItems[i].InternalTeamPersonInCharge, this.QuotationWorkflow.QuotationId, this.ChecklistItems[i].Status);
                    
                    countNotified++;
                }
            }

            if (countNotified > 0) {
                if (this.NotifyChecklistValidation.length > 0) {
                    $('#emailErrorModal').modal('show');
                }
                else {
                    $('#emailSentModal').modal('show');
                }
            }
        },
        checklistReset: function () {
            for (var i = 0; i < this.ChecklistItems.length; i++) {
                //if (this.ChecklistItems[i].Status == StatusApproved && this.ChecklistItems[i].InternalTeamPersonInCharge != null && this.ChecklistItems[i].InternalTeamPersonInCharge != "") {
                //    this.ChecklistItems[i].Status = StatusPendingSignOff;
                //    this.ChecklistItems[i].StatusName = this.getChecklistStatusName(StatusPendingSignOff);
                //    this.saveChecklist();
                //    this.saveIndividualStatus(StatusPendingSignOff, this.ChecklistItems[i].Id, this.ChecklistItems[i].InternalTeamPersonInCharge);
                //}

                //if (this.ChecklistItems[i].Status != StatusNotRequired) {
                    this.checklistNotRequired(i);
                //}
            }

            this.QuotationWorkflowVersion.ChecklistFinalised = false;

            $.ajax({
                url: UpdateVersionChecklistFinalisedUrl,
                type: "POST",
                data: {
                    versionId: this.QuotationWorkflowVersion.Id,
                    isFinalise: false,
                },
                cache: false,
                async: false,
                success: function (data) { }
            });
        },
        saveIndividualStatus: function (status, subObjectId, personInCharge = null) {
            this.ChecklistStatusHistoryModal.Version = this.QuotationWorkflow.EditableVersion;
            this.ChecklistStatusHistoryModal.VersionStr = this.QuotationWorkflow.EditableVersion + '.0';
            this.ChecklistStatusHistoryModal.Status = status;
            this.ChecklistStatusHistoryModal.ModuleId = this.QuotationWorkflow.ModuleId;
            this.ChecklistStatusHistoryModal.ObjectId = this.QuotationWorkflow.Id;
            this.ChecklistStatusHistoryModal.SubModuleController = "TreatyPricingQuotationWorkflowVersionQuotationChecklist";
            this.ChecklistStatusHistoryModal.SubObjectId = subObjectId;
            this.ChecklistStatusHistoryModal.PersonInCharge = personInCharge;

            var statusHistory = null;
            $.ajax({
                url: UpdateChecklistStatusUrl,
                type: "POST",
                data: { statusHistoryBo: this.ChecklistStatusHistoryModal },
                cache: false,
                async: false,
                success: function (data) {
                    statusHistory = data.statusHistoryBo;
                }
            });

            if (statusHistory) {
                this.ChecklistStatusHistories.unshift(Object.assign({}, statusHistory));
            }
        },

        // Remark
        resetRemarkInfo: function (subModuleController) {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.CreatedByName = AuthUserName;
            this.RemarkModal.ModuleId = this.QuotationWorkflow.ModuleId;
            this.RemarkModal.ObjectId = this.QuotationWorkflow.Id;
            this.RemarkModal.SubModuleController = subModuleController;
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

            if (this.RemarkModal.SubModuleController == "TreatyPricingQuotationWorkflowPricing") {
                this.Remarks = this.PricingRemarks;
            } else {
                this.Remarks = this.QuotationRemarks;
            }

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

            var document = {
                ModuleId: parent.ModuleId,
                ObjectId: parent.ObjectId,
                SubModuleController: parent.SubModuleController,
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

        // Status History
        resetStatusHistoryModal: function (subModuleController) {
            this.StatusHistoryModal.Version = this.QuotationWorkflow.EditableVersion;
            this.StatusHistoryModal.VersionStr = this.QuotationWorkflow.EditableVersion + '.0';
            this.StatusHistoryModal.Status = null;
            this.StatusHistoryModal.Remark = '';
            this.StatusHistoryModal.ModuleId = this.QuotationWorkflow.ModuleId;
            this.StatusHistoryModal.ObjectId = this.QuotationWorkflow.Id;
            this.StatusHistoryModal.SubModuleController = subModuleController;

            if (subModuleController == 'TreatyPricingQuotationWorkflowPricing') {
                var pricingStatus = this.QuotationWorkflow.PricingStatus;
                this.StatusSelection = DropDownPricingStatuses.filter(function (s) { return s.Value != pricingStatus });
            } else {
                var status = this.QuotationWorkflow.Status;
                this.StatusSelection = DropDownStatuses.filter(function (s) { return s.Value != status });
            }

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
            var statusHistory = createStatusHistory(this.StatusHistoryModal, "QuotationWorkflow");

            //if (statusHistory) {
            //    var remark = statusHistory.RemarkBo;
            //    if (remark) {
            //        remark.DocumentBos = this.saveDocuments(this.StatusHistories.length, this.StatusHistoryModal, remark.Id);
            //        statusHistory.RemarkBo = remark;
            //    }

            //    if (statusHistory.SubModuleController == 'TreatyPricingQuotationWorkflowPricing') {
            //        this.getStatusName("Pricing", this.StatusHistoryModal.Status, this.QuotationWorkflow.Id, "");
            //        statusHistory.StatusName = $('#PricingStatusName').val();

            //        this.PricingStatusHistories.unshift(Object.assign({}, statusHistory));
            //    } else {
            //        this.StatusHistories.unshift(Object.assign({}, statusHistory));

            //        var remarkText = this.StatusHistoryModal.Remark;
            //        this.getStatusName("Quotation", this.StatusHistoryModal.Status, this.QuotationWorkflow.Id, remarkText);
            //        $('#StatusRemarks').val(remarkText);
            //    }
            //}

            //$('#id').val(value);
        },
        getStatusName: function (type, status, id, remarks) {
            var statusText = "";
            var pendingOn = "";

            $.ajax({
                url: GetStatusNameUrl ? GetStatusNameUrl : null,
                type: "POST",
                data: {
                    type: type,
                    status: status,
                    id: id,
                    remarks: remarks,
                },
                cache: false,
                async: false,
                success: function (data) {
                    statusText = data.statusText;
                    pendingOn = data.pendingOn;
                }
            });

            if (type == "Pricing") {
                $('#PricingStatusName').val(statusText);
                $('#PendingOn').val(pendingOn);
            }
            else {
                $('#StatusName').val(statusText);
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
    },
    created: function () {
        initializeUserTokenFields();
    },
    updated() {

    }
});