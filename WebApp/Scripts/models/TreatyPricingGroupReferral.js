var UnderwritingMethodCount = 0;
var RecipientEmailCount = 0;

$(document).ready(function () {   
    if (!$('#hasRiGroupSlip').is(':checked')) {
        $('#riGroupSlipWonVersion').prop('disabled', true);
        $('#riGroupSlipPersonInChargeId').prop('disabled', true);
        $('#riGroupSlipStatus').prop('disabled', true);
        $('#riGroupSlipConfirmationDateStr').prop('readonly', true);
        $('#riGroupSlipVersionId').prop('disabled', true);
        $('#riGroupSlipTemplateId').prop('disabled', true);
        $('#riGroupSlipSharePointLink').prop('disabled', true);
        $('#riGroupSlipGenerateSharePointLink').prop('disabled', true);
        $('#riGroupSlipEditSharePointLink').prop('disabled', true);
    } else {
        $('#riGroupSlipWonVersion').prop('disabled', false);
        $('#riGroupSlipPersonInChargeId').prop('disabled', false);
        $('#riGroupSlipStatus').prop('disabled', false);
        $('#riGroupSlipConfirmationDateStr').prop('readonly', false);
        $('#riGroupSlipVersionId').prop('disabled', false);
        $('#riGroupSlipTemplateId').prop('disabled', false);
        $('#riGroupSlipSharePointLink').prop('disabled', false);
        $('#riGroupSlipGenerateSharePointLink').prop('disabled', false);
        $('#riGroupSlipEditSharePointLink').prop('disabled', false);
    }
    $('#riGroupSlipWonVersion').selectpicker('refresh');
    $('#riGroupSlipPersonInChargeId').selectpicker('refresh');
    $('#dropDownTreatyPricingClaimApprovalLimits').selectpicker('refresh');
    $('#riGroupSlipStatus').selectpicker('refresh');
    $('#riGroupSlipVersionId').selectpicker('refresh');
    $('#riGroupSlipTemplateId').selectpicker('refresh');
    $('#riGroupSlipSharePointLink').selectpicker('refresh');

    initializeTokenField('#UnderwritingMethod', UnderwritingMethodCodes, 'UnderwritingMethodCount');
    initializeTokenField('#recipientEmail', RecipientEmails, 'RecipientEmailCount', false, '', false);

    var readOnly = ReadOnly;
    if (readOnly) {
        disableFields();
        $('#UnderwritingMethod').tokenfield('disable');
    }

    var version = $('#CurrentVersion').val();
    //getVersionDetails(version);
    //getDropdownVersions(version);

    resetVersionCkEditorInput();

    if (ViewVersionOnly) {
        $("#CurrentVersion").prop("disabled", false);
        $("#CurrentVersion").selectpicker('refresh');
    }
    
    loadingDiv.addClass('hide-loading-spinner');
});

function getChecklistsVersionDetails(version) {
    getVersionDetails(version);
    getChecklistVersionHistories(version);
}

function getVersionDetails(version) {
    var checklists = [];
    var checklistDetails = [];
    var remark = '';
   
    $.ajax({
        url: GetVersionDetailUrl,
        type: "POST",
        cache: false,
        async: false,
        data: {
            groupReferralId: Model.Id,
            groupReferralVersion: version,
            isEditMode: IsEditMode
            //treatyPricingGroupReferralVersionId: version,
        },
        success: function (data) {
            checklists = data.checklistBos;
            checklistDetails = data.checklistDetailBos;
            remark = data.checklistRemark;
        }
    });
    app.Checklists = checklists;
    app.ChecklistDetails = checklistDetails;
    app.Model.ChecklistRemark = remark;

    app.saveChecklist();
    app.saveChecklistDetail();

    resetVersionTokenfields();

    var isChecklistLatestVersion = false;
    if (parseInt(version) === LatestVersion) {
        isChecklistLatestVersion = true;
    }
    app.disableChecklistDetails(!isChecklistLatestVersion, IsEditMode);
}

function getChecklistVersionHistories(version) {
    var checklistHistories = [];
    $.ajax({
        url: GetChecklistHistoriesVersionUrl,
        type: "POST",
        cache: false,
        async: false,
        data: {
            groupReferralId: Model.Id,
            groupReferralVersion: version,
        },
        success: function (data) {
            checklistHistories = data.checklistStatusHistoryBos;
        }
    });

    app.ChecklistStatusHistories = checklistHistories;
}

function resetVersionTokenfields() {
    app.Checklists.forEach(function (item, index) {
        if (item.InternalTeamPersonInCharge != null && item.InternalTeamPersonInCharge != "") {
            $('#InternalTeamPersonInCharge_' + index).tokenfield('setTokens', item.InternalTeamPersonInCharge);
        } else {
            $('#InternalTeamPersonInCharge_' + index).tokenfield('setTokens', []);
            $('#InternalTeamPersonInCharge_' + index + '-tokenfield').attr("placeholder", "Type here");
        }

        var action = item.DisablePersonInCharge ? 'disable' : 'enable';
        $('#InternalTeamPersonInCharge_' + index).tokenfield(action);
        
    });
}

function getDropdownVersions(version) {
    $.ajax({
        url: DropDownVersionUrl,
        type: "POST",
        cache: false,
        async: false,
        data: {
            treatyPricingGroupReferralId: Model.Id,
        },
        success: function (data) {
            refreshDropDownItems('riGroupSlipWonVersion', data.versions2, Model.WonVersion, 'Text', "", true, 'Value', true);
            refreshDropDownItems('riGroupSlipVersionId', data.versions, Model.RiGroupSlipVersionId, 'Text', "", true, 'Value', true);
            refreshDropDownItems('ReplyVersionId', data.versions, Model.ReplyVersionId, 'Text', "", true, 'Value', true);
            refreshDropDownItems('ChecklistVersionId', data.versions2, version, 'Text', "", true, 'Value', false);
        }
    });
}

var UsersUnderwritingCount = 0;
var UsersHealthCount = 0;
var UsersClaimCount = 0;
var UsersBDCount = 0;
var UsersCRCount = 0;
var UsersGroupCount = 0;
var UsersReviewerCount = 0;
var UsersHODCount = 0;
var UsersCEOCount = 0;

function initializeUserTokenFields() {
    $(document).ready(function () {
        initializeTokenField('#InternalTeamPersonInCharge_0', UsersUnderwriting, 'UsersUnderwritingCount', true, 'Checklists');
        initializeTokenField('#InternalTeamPersonInCharge_1', UsersHealth, 'UsersHealthCount', true, 'Checklists');
        initializeTokenField('#InternalTeamPersonInCharge_2', UsersClaim, 'UsersClaimCount', true, 'Checklists');
        initializeTokenField('#InternalTeamPersonInCharge_3', UsersBD, 'UsersBDCount', true, 'Checklists');
        initializeTokenField('#InternalTeamPersonInCharge_4', UsersCR, 'UsersCRCount', true, 'Checklists');
        initializeTokenField('#InternalTeamPersonInCharge_6', UsersGroup, 'UsersGroupCount', true, 'Checklists');
        initializeTokenField('#InternalTeamPersonInCharge_7', UsersReviewer, 'UsersReviewerCount', true, 'Checklists');
        initializeTokenField('#InternalTeamPersonInCharge_8', UsersHOD, 'UsersHODCount', true, 'Checklists');
        initializeTokenField('#InternalTeamPersonInCharge_9', UsersCEO, 'UsersCEOCount', true, 'Checklists');

        resetVersionTokenfields();
    });
}

var app = new Vue({
    el: '#app',
    data: {
        Model: Model,
        ModelVersion: Model.CurrentVersionObject,
        Editable: true,
        // Benefit
        Benefits: [],
        BenefitCodes: Benefits,
        DuplicateBenefitCodes: [],
        ExistingBenefitCodes: [],
        BenefitModal: {
            BenefitId: "",
            IsDuplicateExisting: false,
            DuplicateBenefitId: 0,
        },
        BenefitDataValidation: [],
        DisabledAdd: false,
        BenefitErrors: [],
        OverwriteGroupProfitCommissionRemarksTitle: "",
        OverwriteGroupProfitCommissionRemarks: "",
        OverwriteGroupProfitCommissionRemarksIndex: 0,
        // Uploaded Table
        TableGroupUploads: Uploads,
        TableGroupModal: {
            TableTypeId: "",
            Filename: "",
        },
        TableGroupDataValidation: [],
        TableTypes: TableTypes,
        TableGroupError: "",
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
        // Changelog
        Changelogs: Changelogs,
        Trails: [],
        VersionTrail: "",
        // Status Histories
        StatusHistories: StatusHistories,
        StatusSelection: DropDownGRStatus,
        StatusHistoryModal: {
            Version: 0,
            VersionStr: '0.0',
            Status: null,
            Remark: '',
            ModuleId: null,
            ObjectId: null,
            SubModuleController: null,
        },
        // SharePoint
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
        // Checklist
        DropDownUltimateApprovers: DropDownUltimateApprovers,
        ChecklistStatusHistories: [],
        ChecklistGroups: ChecklistGroups,
        Checklists: [],
        ChecklistDetails: [],
        NotifyChecklistValidation: [],
        DisabledChecklist: false,
    },
    mounted: function () {
        this.refreshBenefitCodeSelection();
        if ($("#CurrentVersion").val() != LatestVersion) {
            this.Editable = false;
        }
    },
    methods: {
        submitGroupReferral: function () {
            this.saveBenefits();
            this.saveChecklist();
            this.saveChecklistDetail();            
        },
        // Benefit
        refreshBenefitCodeSelection: function () {
            //    var benefitIds = Benefits.map(b => b.BenefitId);
            //    this.BenefitCodes = Benefits.filter(function (benefit) {
            //        return !benefitIds.includes(benefit.Value);
            //    });
            //    this.ExistingBenefitCodes = Benefits.filter(function (benefit) {
            //        return benefitIds.includes(benefit.Value);
            //    });
            //this.$nextTick(function () {
            //    $('#BenefitId').selectpicker('refresh');
            //    $('#DuplicateBenefitCode').selectpicker('refresh');
            //});

            this.$nextTick(function () {
                $('#BenefitId').selectpicker('refresh');
                $('#DuplicateBenefitCode').selectpicker('refresh');
            });

            this.DuplicateBenefitCodes = this.Benefits;

        },
        resetBenefitModal: function () {
            this.BenefitModal.BenefitId = null;
            this.BenefitModal.DuplicateBenefitId = null;
            this.BenefitModal.IsDuplicateExisting = false;
            this.BenefitModal.Name = "";
            this.BenefitDataValidation = [];


            this.$nextTick(function () {
                $('#BenefitId').selectpicker('refresh');
                $('#DuplicateBenefitId').selectpicker('refresh');
            });
        },
        addBenefit: function () {
            var errorList = [];
            var benefit = {};

            if ($("#CurrentVersion").val() != LatestVersion) {
                errorList.push("You can only upload data for the latest version");
            }

            if (!this.BenefitModal.BenefitId) {
                errorList.push("Benefit Code is Required.");
            }

            var benefitId = this.BenefitModal.BenefitId;
            duplicateFound = this.Benefits.find(item => item.BenefitId == benefitId);

            if (duplicateFound) {
                errorList.push("Benefit Code is already exist.");
            }

            if (this.BenefitModal.BenefitId)

                if (errorList.length == 0) {
                    if (this.BenefitModal.IsDuplicateExisting) {
                        var duplicateBenefitId = this.BenefitModal.DuplicateBenefitId;
                        duplicateBenefit = this.Benefits.find(item => item.Id == duplicateBenefitId);
                        benefit = $.extend({}, duplicateBenefit);
                    }

                    benefit.Id = 0;
                    benefit.BenefitId = this.BenefitModal.BenefitId;
                    benefit.BenefitCode = $("#BenefitId option:selected").text();
                    benefit.TreatyPricingGroupReferralVersionId = this.ModelVersion.Id;

                    this.Benefits.push(benefit);

                    this.refreshBenefitCodeSelection();

                    this.$nextTick(function () {
                        $('.selectpicker').selectpicker('refresh');
                    });

                    $('#benefitModal').modal('hide');
                }

            if (errorList.length > 0) {
                this.BenefitDataValidation = errorList;
                return;
            }
            this.refreshBenefitCodeSelection();
        },
        removeBenefit: function (index) {
            this.Benefits.splice(index, 1);

            this.$nextTick(function () {
                $('.selectpicker').selectpicker('refresh');
            });
            this.refreshBenefitCodeSelection();
        },
        cloneBenefit: function (index) {            
            var item = this.Benefits[index];

            this.BenefitModal.BenefitId = null;
            this.BenefitModal.IsDuplicateExisting = true;
            this.BenefitModal.DuplicateBenefitId = item.Id;
            this.BenefitDataValidation = [];

            this.refreshBenefitCodeSelection();
            $('#benefitModal').modal('toggle');
        },
        saveBenefits: function () {
            $('#TreatyPricingGroupReferralVersionBenefit').val(JSON.stringify(this.Benefits));
        },
        disableBenefit: function (disableVersion) {
            this.$nextTick(function () {
                if (disableVersion) {
                    $("#benefit input:not('[type=hidden]'):not('.force-disable')").prop("disabled", "disabled");
                    $("#benefit select:not('.force-disable')").prop("disabled", true);
                    $("#benefit button:not('.dropdown-toggle'):not('.btn-collapse')").prop("disabled", true);
                    
                } else {
                    $("#benefit input:not('[type=hidden]'):not('.force-disable')").prop("disabled", false);
                    $("#benefit select:not('.force-disable')").prop("disabled", false);
                    $("#benefit button:not('.dropdown-toggle'):not('.btn-collapse')").prop("disabled", false);
                    
                }
                $("#benefit select:not('.force-disable')").selectpicker('refresh');
            });
        },
        submitBenefits: function () {
            this.resetBenefitError();
            this.Benefits.forEach(this.validateBenefits);
            this.saveChecklist();
            this.saveChecklistDetail();
            //$("#myForm").submit();
            if (this.BenefitErrors.length == 0) {
                if (this.Benefits.length > 0) {
                    $.ajax({
                        url: SaveBenefitsUrl,
                        type: "POST",
                        data: {
                            versionBenefitBos: this.Benefits,
                        },
                        cache: false,
                        async: false,
                        success: function (data) {
                        },
                    });
                    $("#myForm").submit();
                }
                else {
                    this.BenefitErrors.push("At least 1 Benefit Code has to be added");
                }
            }
            if (this.BenefitErrors.length > 0) {
                text = "<ul>";
                for (i = 0; i < this.BenefitErrors.length; i++) {
                    text += "<li>" + this.BenefitErrors[i] + "</li>";
                }
                text += "</ul>";
                $('#benefitErrors').append(text);
                $('#benefitErrors').show();
                return false;
            }
        },
        validateBenefits: function (benefit, index) {
            var errorList = [];
            if (benefit.ReinsuranceArrangementPickListDetailId == null || benefit.ReinsuranceArrangementPickListDetailId == "" || benefit.ReinsuranceArrangementPickListDetailId == 0) {
                errorList.push("Reinsurance Arrangement is required in Benefit " + benefit.BenefitCode);
                this.BenefitErrors.push("Reinsurance Arrangement is required in Benefit " + benefit.BenefitCode);
            }
            if (benefit.OtherSpecialReinsuranceArrangement == null || benefit.OtherSpecialReinsuranceArrangement == "" || benefit.OtherSpecialReinsuranceArrangement == 0) {
                errorList.push("Other Special Reinsurance Arrangement is required in Benefit " + benefit.BenefitCode);
                this.BenefitErrors.push("Other Special Reinsurance Arrangement is required in Benefit " + benefit.BenefitCode);
            }
            if (benefit.ProfitMargin == null || benefit.ProfitMargin == "") {
                errorList.push("Profit Margin is required in Benefit " + benefit.BenefitCode);
                this.BenefitErrors.push("Profit Margin is required in Benefit " + benefit.BenefitCode);
            }
            if (benefit.ExpenseMargin == null || benefit.ExpenseMargin == "") {
                errorList.push("Expense Margin is required in Benefit " + benefit.BenefitCode);
                this.BenefitErrors.push("Expense Margin is required in Benefit " + benefit.BenefitCode);
            }
            if (benefit.CommissionMargin == null || benefit.CommissionMargin == "") {
                errorList.push("Commission Margin is required in Benefit " + benefit.BenefitCode);
                this.BenefitErrors.push("Commission Margin is required in Benefit " + benefit.BenefitCode);
            }
        },
        resetBenefitError: function () {
            this.BenefitErrors = [];
            $('#benefitErrors').empty();
            $('#benefitErrors').hide();
        },
        // Benefit Overwrite Benefit Commission Remarks
        saveOverwriteProfitCommRemarks: function (index) {
            var item = this.Benefits[index];
            if (item != null) {
                item.OverwriteGroupProfitCommissionRemarks = this.OverwriteGroupProfitCommissionRemarks;
            }
        },
        openOverwriteProfitCommRemarks: function (index) {
            var item = this.Benefits[index];
            if (item != null) {
                this.OverwriteGroupProfitCommissionRemarks = item.OverwriteGroupProfitCommissionRemarks;
                this.OverwriteGroupProfitCommissionRemarksTitle = "Overwrite Group Profit Commission Remarks for Benefit: " + item.BenefitCode;
                this.OverwriteGroupProfitCommissionRemarksIndex = index;
            }
        },
        // Benefit View Hyperlink to Treaty Pricing Objects
        linkToUnderwritingLimit: function (index) {
            var item = this.Benefits[index];
            if (item != null) {
                var url = UnderwritingLimitEditUrl + "/" + item.TreatyPricingUwLimitId + "?versionId=" + item.TreatyPricingUwLimitVersionId;
                window.open(url, '_blank');
            }
        },
        editEditor: function (title, id) {
            openEditor(title, id);
        },
        // Remark
        resetRemarkInfo: function (subModuleController) {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.CreatedByName = AuthUserName;
            this.RemarkModal.ModuleId = this.Model.ModuleId;
            this.RemarkModal.ObjectId = this.Model.Id;
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
        // Changelog
        viewChangelog: function (index) {
            this.Trails = this.Changelogs[index].UserTrailBos;
            this.VersionTrail = this.Changelogs[index].BetweenVersionTrail;

            $('[id^=showAll_]').hide();
            $('[id^=showLess_]').show();
            $('[id^=showAllBtn_]').show();
            $('[id^=collapseAllBtn_]').hide();

            $('#changelogModal').modal('show');
        },
        toggleChangelogDataView: function (index) {
            $('#showAll_' + index).toggle();
            $('#showLess_' + index).toggle();
            $('#showAllBtn_' + index).toggle();
            $('#collapseAllBtn_' + index).toggle();
        },
        // Status History
        resetStatusHistoryModal: function (subModuleController) {
            this.StatusHistoryModal.Version = this.Model.EditableVersion;
            this.StatusHistoryModal.VersionStr = this.Model.EditableVersion + '.0';
            this.StatusHistoryModal.Status = null;
            this.StatusHistoryModal.Remark = '';
            this.StatusHistoryModal.ModuleId = this.Model.ModuleId;
            this.StatusHistoryModal.ObjectId = this.Model.Id;
            this.StatusHistoryModal.SubModuleController = subModuleController;

            var status = this.Model.Status;
            this.StatusSelection = DropDownGRStatus.filter(function (s) { return s.Value != status });

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

            // Remarks is required if follow the others module
            // For Group Referral, remarks not required if any file uploaded remarks will be required
            var files = getFiles();
            if (files.length > 0) {
                if (this.StatusHistoryModal.Remark == '')
                    errors.push("Remark is required");
            }
            
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
            var statusHistory = createStatusHistory(this.StatusHistoryModal, "GroupReferral");

            //if (statusHistory) {
            //    var remark = statusHistory.RemarkBo;
            //    if (remark) {
            //        remark.DocumentBos = this.saveDocuments(this.StatusHistories.length, this.StatusHistoryModal, remark.Id);
            //        statusHistory.RemarkBo = remark;
            //    }

            //    this.StatusHistories.unshift(Object.assign({}, statusHistory));
            //}

            //$('#statusHistoryModal').modal('hide');
        },
        // Uploaded Table
        resetAddTableGroupModal() {
            this.TableGroupModal.TableTypeId = "";
            this.TableGroupModal.Filename = "";

            var upload = $('#tableGroupFiles');
            upload.val(null);
            var selectedFiles = document.querySelector(".table-group-files#selectedFiles");
            selectedFiles.innerHTML = "";

            this.TableGroupDataValidation = [];
        },
        uploadTableGroupFile() {
            var selectedFiles = document.querySelector(".table-group-files#selectedFiles");
            var upload = $('#tableGroupFiles');
            selectedFiles.innerHTML = "";
            if (!upload[0].files[0]) return;

            var files = upload[0].files;
            this.TableGroupModal.Filename = files[0].name;
            var list = "<li>" + files[0].name + "</li>";
            selectedFiles.innerHTML = "<ul>" + list + "</ul>";
        },
        addTableGroup() {
            this.TableGroupDataValidation = [];

            var isSuccess = false;
            var errorList = [];

            if (!this.TableGroupModal.TableTypeId) {
                errorList.push("Table Type is Required.");
            }
            if (!this.TableGroupModal.Filename) {
                errorList.push("File Upload is Required.");
            }

            if (errorList.length == 0) {
                var upload = $('#tableGroupFiles');
                var file = upload[0].files[0];
                var fileData = new FormData()
                fileData.append(file.name, file);

                fileData.append('treatyPricingGroupReferralId', this.Model.Id);
                fileData.append('tableTypeId', this.TableGroupModal.TableTypeId);
                fileData.append('uploadedType', UploadedType);

                var bos = [];
                $.ajax({
                    url: CreateUploadUrl,
                    type: "POST",
                    contentType: false,
                    processData: false,
                    cache: false,
                    async: false,
                    data: fileData,
                    success: function (data) {
                        if (data.errors && data.errors.length > 0) {
                            isSuccess = false;
                            errorList = data.errors;
                        } else {
                            isSuccess = true;
                            bos = data.treatyPricingGroupReferralFileBos;
                        }
                    }
                });
            }

            if (errorList.length > 0) {
                this.TableGroupDataValidation = errorList;
                return;
            }

            if (isSuccess) {
                this.TableGroupUploads = bos;
                $('#uploadedTableModal').modal('hide');
            }
        },
        showErrorModal: function (index) {
            this.TableGroupError = this.TableGroupUploads[index].FormattedErrors;
            $("#errorModal").modal()
        },
        // SharePoint
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
            loadingDiv.removeClass('hide-loading-spinner');
         
            this.resetSharePointGenerateModal();
            this.SharePointGenerateModal.type = type;
            
            if (type == "RiGroupSlip") {
                this.SharePointGenerateModal.template = this.Model.RiGroupSlipTemplateId;
                this.SharePointGenerateModal.sharePointFolderPath = this.Model.RiGroupSlipSharePointFolderPath;
                this.SharePointGenerateModal.sharePointLink = this.Model.RiGroupSlipSharePointLink;
                this.SharePointGenerateModal.typeFull = "Ri Group Slip";
            }
            else {
                this.SharePointGenerateModal.template = this.Model.ReplyTemplateId;
                this.SharePointGenerateModal.sharePointFolderPath = this.Model.ReplySharePointFolderPath;
                this.SharePointGenerateModal.sharePointLink = this.Model.ReplySharePointLink;
                this.SharePointGenerateModal.typeFull = "Reply Template";
            }
            
            if (this.SharePointGenerateModal.template == null || this.SharePointGenerateModal.template == "") {
                this.SharePointGenerateValidation.push(this.SharePointGenerateModal.typeFull + " Template is required");
                loadingDiv.addClass('hide-loading-spinner');
            }

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
                        versionId: this.ModelVersion.Id,
                        type: type,
                        typeFull: this.SharePointGenerateModal.typeFull,
                        templateCode: this.SharePointGenerateModal.template,
                        sharePointFolderPath: this.SharePointGenerateModal.sharePointFolderPath,
                        sharePointLink: this.SharePointGenerateModal.sharePointLink,
                    },
                    cache: false,
                    async: false,
                    success: function (data) {
                        loadingDiv.addClass('hide-loading-spinner');

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
            loadingDiv.removeClass('hide-loading-spinner');
            var setEditLink = this.setEditLink;
            var displaySharePointErrors = this.displaySharePointErrors;

            //console.log(this.ModelVersion.Id)
            //console.log(this.SharePointGenerateModal.type)
            //console.log(this.SharePointGenerateModal.sharePointFolderPath)
            //console.log(this.SharePointGenerateModal.localPath)
            //console.log(this.SharePointGenerateModal.isCampaignSpec)

            $.ajax({
                url: GenerateSharePointFileConfirmedUrl ? GenerateSharePointFileConfirmedUrl : null,
                type: "POST",
                data: {
                    versionId: this.ModelVersion.Id,
                    type: this.SharePointGenerateModal.type,
                    newFileName: this.SharePointGenerateModal.newFileName,
                    sharePointFolderPath: this.SharePointGenerateModal.sharePointFolderPath,
                    localPath: this.SharePointGenerateModal.localPath,
                    isCampaignSpec: false,
                },
                cache: false,
                async: false,
                success: function (data) {
                    loadingDiv.addClass('hide-loading-spinner');
                    
                    //errorList = data.errors;
                    //editLink = data.editLink;
                    //console.log(data)
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
            if (this.SharePointGenerateModal.type == "RiGroupSlip") {
                this.SharePointGenerateModal.quoteSpecEditLink = editLink;
                $('#riGroupSlipSharePointLink').val(editLink);
            }
            else {
                this.SharePointGenerateModal.rateTableEditLink = editLink;
                $('#ReplySharePointLink').val(editLink);
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
            if (type == "RiGroupSlip") {
                //editLink = this.QuotationWorkflowVersion.QuoteSpecSharePointLink;
                editLink = this.Model.RiGroupSlipSharePointLink
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
                editLink = this.Model.ReplySharePointLink;

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
        // Checklist
        checklistItem: function (index, object) {

            if (this.Checklists.length)
                return this.Checklists[index - 1][object];
            else
                return;
        },
        checklistGroupItem: function (index, object) {
            if (this.ChecklistDetails.length) {                

                if (object === 'UltimateApprover') {
                    this.$nextTick(function () {                        
                        if (this.ChecklistDetails[index - 1][object] != null && this.ChecklistDetails[index - 1][object] != "")
                            $('#UltimateApprover' + index + '_6').val(encodeURIComponent(this.ChecklistDetails[index - 1][object]));
                        else
                            $('#UltimateApprover' + index + '_6').val("null");
                        $('#UltimateApprover' + index + '_6').selectpicker('refresh');
                    });
                }
                else {
                    var checkedVal = this.ChecklistDetails[index - 1][object];

                    switch (object) {
                        case 'Underwriting': $('#' + index + '_1').prop('checked', checkedVal); break;
                        case 'Health': $('#' + index + '_2').prop('checked', checkedVal); break;
                        case 'Claim': $('#' + index + '_3').prop('checked', checkedVal); break;
                        case 'BD': $('#' + index + '_4').prop('checked', checkedVal); break;
                        case 'CnR': $('#' + index + '_5').prop('checked', checkedVal); break;
                        case 'GroupTeamApprover': $('#' + index + '_7').prop('checked', checkedVal); break;
                        case 'ReviewerApprover': $('#' + index + '_8').prop('checked', checkedVal); break;
                        case 'HODApprover': $('#' + index + '_9').prop('checked', checkedVal); break;
                        case 'CEOApprover': $('#' + index + '_10').prop('checked', checkedVal); break;
                    }                    
                    return checkedVal;
                }
            }                
            else
                return;
        },
        saveChecklist: function () {            
            $('#Checklists').val(JSON.stringify(this.Checklists));
            //console.log(this.Checklists)
        },
        saveChecklistDetail: function () {
            $('#ChecklistDetails').val(JSON.stringify(this.ChecklistDetails));
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
        sendChecklistNotificationEmail: function (internalTeamPersonInCharge, referralCode) {
            var errorList = [];
            $.ajax({
                url: NotifyQuotationChecklistUrl,
                type: "POST",
                data: {
                    referralCode: referralCode,
                    internalTeam: internalTeamPersonInCharge,
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
            this.Checklists[index - 1].Status = StatusNotRequired;
            this.Checklists[index - 1].StatusName = this.getChecklistStatusName(StatusNotRequired);

            // Reset InternalPersonInCharge
            $('#InternalTeamPersonInCharge_' + (index - 1)).tokenfield('setTokens', []);
            $('#InternalTeamPersonInCharge_' + (index - 1) + '-tokenfield').attr("placeholder", "Type here");

            this.saveChecklist();
            this.saveIndividualStatus(StatusNotRequired, this.Checklists[index - 1].Id, this.Checklists[index - 1].InternalTeamPersonInCharge);
        },
        checklistPendingReview: function (index) {
            this.resetChecklistError();

            if (this.ChecklistDetails.length) {
                var object = this.getDefaultInternalTeamObject(index);
                var checkboxCount = this.ChecklistDetails.filter(function (e) { return e[object] == true; }).length;

                if (checkboxCount > 0) {
                    this.NotifyChecklistValidation = [];

                    if (this.Checklists[index - 1].InternalTeamPersonInCharge != null && this.Checklists[index - 1].InternalTeamPersonInCharge != "") {
                        this.sendChecklistNotificationEmail(this.Checklists[index - 1].InternalTeamPersonInCharge, this.Model.Code);

                        this.Checklists[index - 1].Status = StatusPendingReview;
                        this.Checklists[index - 1].StatusName = this.getChecklistStatusName(StatusPendingReview);
                        this.saveChecklist();
                        this.saveIndividualStatus(StatusPendingReview, this.Checklists[index - 1].Id, this.Checklists[index - 1].InternalTeamPersonInCharge);

                        if (this.NotifyChecklistValidation.length > 0) {
                            $('#emailErrorModal').modal('show');
                        }
                        else {
                            $('#emailSentModal').modal('show');
                        }
                    }
                    else {
                        $('#checklistError').append('Person In-Charge for Department is required.');
                        $('#checklistError').show();
                    }
                }
                else {
                    $('#checklistError').append('Please check at leaset one option under respective Department to proceed.');
                    $('#checklistError').show();
                }
            }
        },
        checklistCompleted: function (index) {
            if (this.Checklists[index - 1].InternalTeamPersonInCharge != null && this.Checklists[index - 1].InternalTeamPersonInCharge != "") {
                this.Checklists[index - 1].Status = StatusCompleted;
                this.Checklists[index - 1].StatusName = this.getChecklistStatusName(StatusCompleted);
                this.saveChecklist();
                this.saveIndividualStatus(StatusCompleted, this.Checklists[index - 1].Id, this.Checklists[index - 1].InternalTeamPersonInCharge);
            }
        },
        checklistPendingApproval: function (index) {
            this.resetChecklistError();
            this.resetChecklistWarning();

            if (this.ChecklistDetails.length) {
                var key = this.GetUltimateApproverKey(index); 
                var object1 = this.getDefaultInternalTeamObject(6);
                var ultimaApproverCount = this.ChecklistDetails.filter(function (e) { return key <= e[object1]; }).length;
                //console.log(key)
                //console.log(ultimaApproverCount)
                if (ultimaApproverCount > 0) {
                    var object = this.getDefaultInternalTeamObject(index);
                    var checkboxCount = this.ChecklistDetails.filter(function (e) { return e[object] == true; }).length;

                    if (checkboxCount > 0) {
                        this.NotifyChecklistValidation = [];

                        if (this.Checklists[index - 1].InternalTeamPersonInCharge != null && this.Checklists[index - 1].InternalTeamPersonInCharge != "") {
                           
                            // checking and prompt error if user clicks 'Request' for the higher approver where the status for lower level of approver is not Approved / Rejected, got several items that tick and user have selected
                            // checking and prompt warning if user clicks 'Request' for the higher approver where the status for lower level of approver is not Approved / Rejected, no items tick and empty user.
                            var warning = 0;
                            var error = 0;
                            for (var i = 1; i < key; i++) { 
                                switch (i) {
                                    case 1: // I - Group Team
                                        var checkboxGroupCount = this.ChecklistDetails.filter(function (e) { return e['GroupTeamApprover'] == true; }).length;
                                        if (checkboxGroupCount > 0) {
                                            if (((this.Checklists[6].InternalTeamPersonInCharge != null && this.Checklists[6].InternalTeamPersonInCharge != "") || (this.Checklists[6].InternalTeamPersonInCharge == null || this.Checklists[6].InternalTeamPersonInCharge == ""))
                                                    && (this.Checklists[6].Status != StatusApproved && this.Checklists[6].Status != StatusRejected))
                                                error++;
                                        } else {
                                            if (this.Checklists[6].InternalTeamPersonInCharge != null && this.Checklists[6].InternalTeamPersonInCharge != "" && this.Checklists[6].Status != StatusApproved && this.Checklists[6].Status != StatusRejected)
                                                error++;
                                            if ((this.Checklists[6].InternalTeamPersonInCharge == null || this.Checklists[6].InternalTeamPersonInCharge == "") && this.Checklists[6].Status == StatusNotRequired)
                                                warning++; 
                                        }  
                                        break;
                                    case 2: // II - AM, T&P
                                        var checkboxReviewerCount = this.ChecklistDetails.filter(function (e) { return e['ReviewerApprover'] == true; }).length;
                                        if (checkboxReviewerCount > 0) {
                                            if (((this.Checklists[7].InternalTeamPersonInCharge != null && this.Checklists[7].InternalTeamPersonInCharge != "") || (this.Checklists[7].InternalTeamPersonInCharge == null || this.Checklists[7].InternalTeamPersonInCharge == ""))
                                                    && (this.Checklists[7].Status != StatusApproved && this.Checklists[7].Status != StatusRejected))
                                                error++;
                                        } else {
                                            if (this.Checklists[7].InternalTeamPersonInCharge != null && this.Checklists[7].InternalTeamPersonInCharge != "" && this.Checklists[7].Status != StatusApproved && this.Checklists[7].Status != StatusRejected)
                                                error++;
                                            if ((this.Checklists[7].InternalTeamPersonInCharge == null || this.Checklists[7].InternalTeamPersonInCharge == "") && this.Checklists[7].Status == StatusNotRequired)
                                                warning++;
                                        }
                                        break;
                                    case 3: // III - HOD, T&P
                                        var checkboxHodCount = this.ChecklistDetails.filter(function (e) { return e['HODApprover'] == true; }).length;
                                        if (checkboxHodCount > 0) {
                                            if (((this.Checklists[8].InternalTeamPersonInCharge != null && this.Checklists[8].InternalTeamPersonInCharge != "") || (this.Checklists[8].InternalTeamPersonInCharge == null || this.Checklists[8].InternalTeamPersonInCharge == ""))
                                                    && (this.Checklists[8].Status != StatusApproved && this.Checklists[8].Status != StatusRejected))
                                                error++;
                                        } else {
                                            if (this.Checklists[8].InternalTeamPersonInCharge != null && this.Checklists[8].InternalTeamPersonInCharge != "" && this.Checklists[8].Status != StatusApproved && this.Checklists[8].Status != StatusRejected)
                                                error++;
                                            if ((this.Checklists[8].InternalTeamPersonInCharge == null || this.Checklists[8].InternalTeamPersonInCharge == "") && this.Checklists[8].Status == StatusNotRequired)
                                                warning++;
                                        } 
                                        break;
                                    case 4: // VII - CEO
                                        var checkboxCeoCount = this.ChecklistDetails.filter(function (e) { return e['CEOApprover'] == true; }).length;
                                        if (checkboxCeoCount > 0) {
                                            if (((this.Checklists[9].InternalTeamPersonInCharge != null && this.Checklists[9].InternalTeamPersonInCharge != "") || (this.Checklists[9].InternalTeamPersonInCharge == null || this.Checklists[9].InternalTeamPersonInCharge == ""))
                                                    && (this.Checklists[9].Status != StatusApproved && this.Checklists[9].Status != StatusRejected))
                                                error++;
                                        } else {
                                            if (this.Checklists[9].InternalTeamPersonInCharge != null && this.Checklists[9].InternalTeamPersonInCharge != "" && this.Checklists[9].Status != StatusApproved && this.Checklists[9].Status != StatusRejected)
                                                error++;
                                            if ((this.Checklists[9].InternalTeamPersonInCharge == null || this.Checklists[9].InternalTeamPersonInCharge == "") && this.Checklists[9].Status == StatusNotRequired)
                                                warning++;
                                        } 
                                        //if (this.Checklists[9].Status != StatusApproved && this.Checklists[9].Status != StatusRejected) warning++; 
                                        break;
                                }
                            }

                            if (error > 0) {
                                $('#checklistError').append('Kindly make sure all lower level approver has been approved/rejected to proceed.');
                                $('#checklistError').show();
                            }
                            else {
                                if (warning > 0) {
                                    $('#checklistWarning').append('Kindly make sure all approvers have been selected.');
                                    $('#checklistWarning').show();
                                }

                                this.sendChecklistNotificationEmail(this.Checklists[index - 1].InternalTeamPersonInCharge, this.Model.Code);

                                this.Checklists[index - 1].Status = StatusPendingApproval;
                                this.Checklists[index - 1].StatusName = this.getChecklistStatusName(StatusPendingApproval);
                                this.saveChecklist();
                                this.saveIndividualStatus(StatusPendingApproval, this.Checklists[index - 1].Id, this.Checklists[index - 1].InternalTeamPersonInCharge);
                            }
                            
                        }
                        else {
                            $('#checklistError').append('Person In-Charge for Approver is required.');
                            $('#checklistError').show();
                        }
                    }
                    else {
                        $('#checklistError').append('Please check at least one option under respective Approver to proceed.');
                        $('#checklistError').show();
                    }
                }
                else {
                    $('#checklistError').append('Please select this Approver in Ultimate Approver to proceed.'); 
                    $('#checklistError').show();
                }
            }            
        },
        checklistApproved: function (index) {
            if (this.Checklists[index - 1].InternalTeamPersonInCharge != null && this.Checklists[index - 1].InternalTeamPersonInCharge != "") {
                this.Checklists[index - 1].Status = StatusApproved;
                this.Checklists[index - 1].StatusName = this.getChecklistStatusName(StatusApproved);
                this.saveChecklist();
                this.saveIndividualStatus(StatusApproved, this.Checklists[index - 1].Id, this.Checklists[index - 1].InternalTeamPersonInCharge);
            }
        },
        checklistRejected: function (index) {
            if (this.Checklists[index - 1].InternalTeamPersonInCharge != null && this.Checklists[index - 1].InternalTeamPersonInCharge != "") {
                this.Checklists[index - 1].Status = StatusRejected;
                this.Checklists[index - 1].StatusName = this.getChecklistStatusName(StatusRejected);
                this.saveChecklist();
                this.saveIndividualStatus(StatusRejected, this.Checklists[index - 1].Id, this.Checklists[index - 1].InternalTeamPersonInCharge);
            }
        },
        checklistNotify: function () {
            this.NotifyChecklistValidation = [];
            var countNotified = 0;

            for (var i = 0; i < this.Checklists.length; i++) {
                if ((this.Checklists[i].Status == StatusPendingReview || this.Checklists[i].Status == StatusPendingApproval) && this.Checklists[i].InternalTeamPersonInCharge != null && this.Checklists[i].InternalTeamPersonInCharge != "") {
                    this.sendChecklistNotificationEmail(this.Checklists[i].InternalTeamPersonInCharge, this.Model.Code);

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
            for (var i = 0; i < this.Checklists.length; i++) {
                this.Checklists[i].InternalTeamPersonInCharge = null;
                this.Checklists[i].Status = StatusNotRequired;
                this.Checklists[i].StatusName = this.getChecklistStatusName(StatusNotRequired);

                // Reset InternalPersonInCharge
                $('#InternalTeamPersonInCharge_' + (i)).tokenfield('setTokens', []);
                $('#InternalTeamPersonInCharge_' + (i) + '-tokenfield').attr("placeholder", "Type here");

                this.saveChecklist();
                //this.saveIndividualStatus(StatusNotRequired, this.Checklists[i].Id, this.Checklists[i].InternalTeamPersonInCharge);
            }

            for (var i = 0; i < this.ChecklistDetails.length; i++) {
                this.ChecklistDetails[i].Underwriting = false;
                this.ChecklistDetails[i].Health = false;
                this.ChecklistDetails[i].Claim = false;
                this.ChecklistDetails[i].BD = false;
                this.ChecklistDetails[i].CnR = false;
                this.ChecklistDetails[i].UltimateApprover = null;
                this.ChecklistDetails[i].GroupTeamApprover = false;
                this.ChecklistDetails[i].ReviewerApprover = false;
                this.ChecklistDetails[i].HODApprover = false;
                this.ChecklistDetails[i].CEOApprover = false;

                // Reset Checkbox checked
                $('#' + (i + 1) + '_1').prop('checked', false); // Underwriting
                $('#' + (i + 1) + '_2').prop('checked', false); // Health
                $('#' + (i + 1) + '_3').prop('checked', false); // Claim
                $('#' + (i + 1) + '_4').prop('checked', false); // BD
                $('#' + (i + 1) + '_5').prop('checked', false); // CnR
                $('#' + (i + 1) + '_7').prop('checked', false); // GroupTeamApprover
                $('#' + (i + 1) + '_8').prop('checked', false); // ReviewerApprover
                $('#' + (i + 1) + '_9').prop('checked', false); // HODApprover
                $('#' + (i + 1) + '_10').prop('checked', false); // CEOApprover

                this.saveChecklistDetail();
                //console.log(this.ChecklistDetails)
            }

            this.resetChecklistError();
        },
        saveIndividualStatus: function (status, subObjectId, personInCharge = null) {
            this.StatusHistoryModal.Version = this.Model.EditableVersion;
            this.StatusHistoryModal.VersionStr = this.Model.EditableVersion + '.0';
            this.StatusHistoryModal.Status = status;
            this.StatusHistoryModal.ModuleId = this.Model.ModuleId;
            this.StatusHistoryModal.ObjectId = this.Model.Id;
            this.StatusHistoryModal.SubModuleController = "TreatyPricingGroupReferralChecklist";
            this.StatusHistoryModal.SubObjectId = subObjectId;
            this.StatusHistoryModal.PersonInCharge = personInCharge;
                        
            var statusHistory = null;
            $.ajax({
                url: UpdateChecklistStatusUrl,
                type: "POST",
                data: { statusHistoryBo: this.StatusHistoryModal, checklistDetails: JSON.stringify(this.ChecklistDetails) },
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
        checklistChecked: function (index, object, event) {
            var checkedVal = event.target.checked;
            this.ChecklistDetails[index - 1][object] = checkedVal;
            this.saveChecklistDetail();
        },
        checklistApproverChecked: function (index, object, event) {
            var key = this.GetUltimateApproverKey(index);
            switch (object) {
                case 'GroupTeamApprover': key = '1'; break;
                case 'ReviewerApprover': key = '2'; break;
                case 'HODApprover': key = '3'; break;
                case 'CEOApprover': key = '4'; break;
            }
            
            var item = this.ChecklistDetails[index - 1];
            var ultimateApproverVal = item['UltimateApprover'];

            this.resetChecklistError();
            if (key <= ultimateApproverVal) { // If checked option within selected Ultimate Approver

                var checkedVal = event.target.checked;
                this.ChecklistDetails[index - 1][object] = checkedVal;

                if (key == ultimateApproverVal) {
                    for (var i = 1; i < key; i++) { // Auto tick for lower level of approver if the higher level has been ticked
                        //if (i == key) continue;

                        switch (i) {
                            case 1: if (!item['GroupTeamApprover'])
                                $('#' + index + '_7').prop('checked', checkedVal);
                                this.ChecklistDetails[index - 1]['GroupTeamApprover'] = checkedVal;
                                break;
                            case 2: if (!item['ReviewerApprover'])
                                $('#' + index + '_8').prop('checked', checkedVal);
                                this.ChecklistDetails[index - 1]['ReviewerApprover'] = checkedVal;
                                break;
                            case 3: if (!item['HODApprover'])
                                $('#' + index + '_9').prop('checked', checkedVal);
                                this.ChecklistDetails[index - 1]['HODApprover'] = checkedVal;
                                break;
                            case 4: if (!item['CEOApprover'])
                                $('#' + index + '_10').prop('checked', checkedVal);
                                this.ChecklistDetails[index - 1]['CEOApprover'] = checkedVal;
                                break;
                        }
                    }
                }
                this.saveChecklistDetail();       
            }
            else {  // If checked option outside selected Ultimate Approver
                var obj = '';
                if (key === '1') obj = '7';
                else if (key === '2') obj = '8';
                else if (key === '3') obj = '9';
                else if (key === '4') obj = '10';
                
                $('#' + index + '_' + obj).prop('checked', false);

                $('#checklistError').append('This Approver not included in Ultimate Approver.');
                $('#checklistError').show();
            }
            
        },
        checklistOnChange: function (index, object, event) {
            var selectVal = event.target.value;
            this.ChecklistDetails[index - 1][object] = selectVal;
            this.saveChecklistDetail();
        },
        resetChecklistError() {
            $('#checklistError').empty();
            $('#checklistError').hide();
        },
        resetChecklistWarning() {
            $('#checklistWarning').empty();
            $('#checklistWarning').hide();
        },
        disableChecklistDetails: function (disableVersion, editMode) {
            if (editMode) {
                this.$nextTick(function () {
                    if (disableVersion) {
                        $("#checklistTab input[type=checkbox]:not('.force-disable')").prop("disabled", true);
                        $("#checklistTab select:not('.force-disable')").prop("disabled", true);
                        $("#checklistTab textarea").prop("disabled", "disabled");

                    }
                    else {
                        $("#checklistTab input[type=checkbox]:not('.force-disable')").prop("disabled", false);
                        $("#checklistTab select:not('.force-disable')").prop("disabled", false);
                        $("#checklistTab textarea").prop("disabled", false);

                    }
                    $("#checklistTab select:not('.force-disable')").selectpicker('refresh');
                });
            }
            
        },
        getDefaultInternalTeamObject: function(index) {
            switch (index) {
                case 1: return 'Underwriting';  break;
                case 2: return 'Health'; break;
                case 3: return 'Claim'; break;
                case 4: return 'BD'; break;
                case 5: return 'CnR'; break;
                case 6: return 'UltimateApprover'; break;
                case 7: return 'GroupTeamApprover'; break;
                case 8: return 'ReviewerApprover'; break;
                case 9: return 'HODApprover'; break;
                case 10: return 'CEOApprover'; break;
            }                           
        },
        GetUltimateApproverKey: function (index) {
            switch (index) {
                case 7: return 1; break; // I - Group Team
                case 8: return 2; break; // II - AM, T&P
                case 9: return 3; break; // III - HOD, T&P
                case 10: return 4; break; // VII - CEO
            }        
        },
        // RI Group Slip
        riGroupSlipChange: function () {
            if (!$('#hasRiGroupSlip').is(':checked')) {
                $('#riGroupSlipWonVersion').prop('disabled', true);
                $('#riGroupSlipPersonInChargeId').prop('disabled', true);
                $('#riGroupSlipStatus').prop('disabled', true);
                $('#riGroupSlipConfirmationDateStr').prop('readonly', true);
                $('#riGroupSlipVersionId').prop('disabled', true);
                $('#riGroupSlipTemplateId').prop('disabled', true);
                $('#riGroupSlipSharePointLink').prop('disabled', true);
                $('#riGroupSlipGenerateSharePointLink').prop('disabled', true);
                $('#riGroupSlipEditSharePointLink').prop('disabled', true);
            } else {
                $('#riGroupSlipWonVersion').prop('disabled', false);
                $('#riGroupSlipPersonInChargeId').prop('disabled', false);
                $('#riGroupSlipStatus').prop('disabled', false);
                $('#riGroupSlipConfirmationDateStr').prop('readonly', false);
                $('#riGroupSlipVersionId').prop('disabled', false);
                $('#riGroupSlipTemplateId').prop('disabled', false);
                $('#riGroupSlipSharePointLink').prop('disabled', false);
                $('#riGroupSlipGenerateSharePointLink').prop('disabled', false);
                $('#riGroupSlipEditSharePointLink').prop('disabled', false);
            }


            this.$nextTick(function () {
                $('#riGroupSlipWonVersion').selectpicker('refresh');
                $('#riGroupSlipPersonInChargeId').selectpicker('refresh');
                $('#dropDownTreatyPricingClaimApprovalLimits').selectpicker('refresh');
                $('#riGroupSlipStatus').selectpicker('refresh');
                $('#riGroupSlipVersionId').selectpicker('refresh');
                $('#riGroupSlipTemplateId').selectpicker('refresh');
                $('#riGroupSlipSharePointLink').selectpicker('refresh');
            });
        },
        // Date Picker
        openDatePicker: function (currentId) {
            var idStr = currentId;

            var id = "#" + idStr;
            if ($(id).data("datepicker") != null) {
                $(id).datepicker("destroy");
            }

            $(id).datepicker({
                format: DateFormatDatePickerJs,
                autoclose: true,
            });
            
            var updateDateValue = this.updateDateValue;
            $(id).on('changeDate', function () {
                if (idStr == "CoverageStartDateStr") {
                    var dateMomentObject = moment($(id).val()).format('MM/DD/YYYY');
                    var coverageDate = new Date(dateMomentObject);
                    coverageDate.setFullYear(coverageDate.getFullYear() + 1);
                    coverageDate.setDate(coverageDate.getDate() - 1);

                    var endDate = moment(coverageDate).format('DD MMM YYYY');
                    $('#CoverageEndDateStr').datepicker('setDate', endDate);

                    updateDateValue(idStr, $(id).val());
                    updateDateValue('CoverageEndDateStr', endDate);

                } else {
                    updateDateValue(idStr, $(id).val());
                }                
            });

            $(id).focus();
        },
        updateDateValue: function (field, value) {
            var typeStr = "Model";
            this[typeStr][field] = value;
        },
    },   
    created: function () {
        //if (this.Benefits)
        //    this.BenefitMaxIndex = this.Benefits.length - 1;
    
        initializeUserTokenFields();
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});

var setVersionCallBack = function (bo) {
    app.Benefits = JSON.parse(bo.TreatyPricingGroupReferralVersionBenefit);
    app.refreshBenefitCodeSelection();   
    app.Disable = disableVersion;

    getChecklistsVersionDetails(bo.Version);
    getDropdownVersions(bo.Version);
    //VersionDropDowns = bo.TreatyPricingGroupReferralVersionBenefitBos;

    resetVersionCkEditorInput();
    app.disableBenefit(disableVersion);
}

function resetVersionCkEditorInput() {
    app.Benefits.forEach(function (item, index) {
        setCkEditorInput('#GroupFreeCoverLimitNonCiInput' + index, item.GroupFreeCoverLimitNonCI);
        setCkEditorInput('#GroupFreeCoverLimitCiInput' + index, item.GroupFreeCoverLimitCI);
        setCkEditorInput('#GroupProfitCommissionInput' + index, item.GroupProfitCommission);        
    });
}

function setCkEditorInput(inputId, value) {
    var placeholder = 'Click to ';
    if (value == null || value == '') {
        placeholder += 'Add';
        $(inputId).removeClass('ck-editor-edit-input');
    } else {
        placeholder += 'Edit';
        $(inputId).addClass('ck-editor-edit-input');
    }

    // border color rgba(1, 80, 159, 0.3);

    $(inputId).attr('placeholder', placeholder);
}