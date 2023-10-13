var PerLifeRetroTreatyCodeCount = 0;
var UnderwritingMethodCount = 0;
var TargetSegmentCount = 0;
var DistributionChannelCount = 0;
var CessionTypeCount = 0;
var ProductLineCount = 0;

$(document).ready(function () {
    $('.textarea-auto-expand').autoResize();
    $('.textarea-auto-expand').trigger('keyup');
    $('.bootstrap-select').removeClass('dropup');

    initializeTokenField('#PerLifeRetroTreatyCode', PerLifeRetroCodes, 'PerLifeRetroTreatyCodeCount');
    initializeTokenField('#UnderwritingMethod', UnderwritingMethodCodes, 'UnderwritingMethodCount');
    initializeTokenField('#TargetSegment', TargetSegmentCodes, 'TargetSegmentCount');
    initializeTokenField('#DistributionChannel', DistributionChannelCodes, 'DistributionChannelCount');
    initializeTokenField('#CessionType', CessionTypeCodes, 'CessionTypeCount');
    initializeTokenField('#ProductLine', ProductLineCodes, 'ProductLineCount');

    if (ReadOnly) {
        $('#PerLifeRetroTreatyCode').tokenfield('disable');
        $('#UnderwritingMethod').tokenfield('disable');
    }

    resetPerLifeRetro();
    resetDisableFields();

    $('#generalTabBtn, #benefitTabBtn').on('shown.bs.tab', function (event) {
        $('#generalTab .textarea-auto-expand').trigger('keyup');
    });

    $('#pricingTabBtn').on('shown.bs.tab', function (event) {
        $('#pricingTab .textarea-auto-expand').trigger('keyup');
    });
});

function resetPerLifeRetro() {
    if (ReadOnly)
        return;

    var action = document.getElementById('HasPerLifeRetro').checked ? 'enable' : 'disable';
    $('#PerLifeRetroTreatyCode').tokenfield(action);
}

function resetField(id, disable, clear = true) {
    if (clear)
        $(id).val(null);
    $(id).prop('disabled', disable);
}

function resetTab(id, clear = true) {
    var tab = document.getElementById(id);
    if (tab == null)
        return;

    var isChecked = document.getElementById(id).checked;
    var type = id.substring(2);
    var tabBtn = type[0].toLowerCase() + type.substring(1) + 'TabBtn';

    if (!disableVersion) {
        if (type == 'RetakafulService') {
            resetField('#InvestmentProfitSharing', !isChecked, clear);
            resetField('#RetakafulModel', !isChecked, clear);
            if (clear)
                $("#retakafulServiceTab input:not('[name=__RequestVerificationToken]'):not('[type=hidden]'):enabled").val(null);
        } else {
            resetField('#' + type + 'ProfitCommission', !isChecked, clear);
            resetField('#' + type + 'TerminationClause', !isChecked, clear);
            resetField('#' + type + 'RecaptureClause', !isChecked, clear);
            resetField('#' + type + 'QuarterlyRiskPremium', !isChecked, clear);

            if (clear) {
                if (type == 'DirectRetro')
                    app.clearDirectRetros();
                else {
                    $("#inwardRetroTab input:not('[name=__RequestVerificationToken]'):not('[type=hidden]'):enabled").val(null);
                    $("#inwardRetroTab select:enabled").val(null);
                    $("#inwardRetroTab input:checkbox").prop('checked', false);

                    $("#inwardRetroTab .selectpicker").selectpicker('refresh');
                }
            }
        }
    }

    if (isChecked)
        $('#' + tabBtn).show();
    else
        $('#' + tabBtn).hide();

    $('#generalTabBtn').click();
}

function resetDirectRetro(clear = true) {
    resetTab('IsDirectRetro', clear);
}

function resetInwardRetro(clear = true) {
    resetTab('IsInwardRetro', clear);
}

function resetRetakafulService(clear = true) {
    resetTab('IsRetakafulService', clear);
}

function resetDisableFields() {
    resetDirectRetro(false);
    resetInwardRetro(false);
    resetRetakafulService(false);
}

$(".delete-direct-retro").click(function (e) {
    e.preventDefault();
});

var app = new Vue({
    el: '#app',
    data: {
        Product: Model,
        ProductVersion: Model.CurrentVersionObject,
        Campaigns: Campaigns,
        Groups: Groups,
        // Benefit
        Benefits: [],
        BenefitCodes: Benefits,
        ExistingBenefitCodes: [],
        BenefitModal: {
            BenefitId: null,
            Name: "",
            IsDuplicate: false,
            DuplicateBenefitCode: null,
        },
        // Benefit - Direct Retro
        RetroPartyCodes: [],
        ExistingRetroPartyCodes: [],
        DirectRetroModal: {
            BenefitIndex: 0,
            BenefitCode: '',
            RetroPartyId: null,
            IsDuplicate: false,
            DuplicateRetroPartyId: null,
        },
        RemoveBenefitCode: null,
        RemoveBenefitIndex: null,
        Disable: false,
        // DropDowns
        Mfrs17BasicRiders: Mfrs17BasicRiders,
        PayoutTypes: PayoutTypes,
        AgeBasis: AgeBasis,
        ArrangementReinsuranceTypes: ArrangementReinsuranceTypes,
        RiskPatternSums: RiskPatternSums,
        UnderwritingLimits: UnderwritingLimits,
        ClaimApprovalLimits: ClaimApprovalLimits,
        RateTables: RateTables,
        DefinitionExclusions: DefinitionExclusions,
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
        // Final Document
        DocumentMaxIndex: 0,
        Documents: FinalDocuments,
        DocumentModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            File: null,
            Description: '',
        },
        RemoveDocumentName: null,
        RemoveDocumentIndex: null,
        // Changelog
        Changelogs: Changelogs,
        Trails: [],
        VersionTrail: "",
    },
    methods: {
        refreshBenefitCodeSelection: function () {
            this.$nextTick(function () {
                $('#BenefitId').selectpicker('refresh');
                $('#DuplicateBenefitCode').selectpicker('refresh');
            });
        },
        resetBenefitModal: function () {
            this.BenefitModal.BenefitId = null;
            this.BenefitModal.DuplicateBenefitCode = null;
            this.BenefitModal.IsDuplicate = false;
            this.BenefitModal.Name = "";

            this.resetAddBenefitError();
            this.toggleBenefitIsDuplicate();

            this.$nextTick(function () {
                $('#BenefitId').selectpicker('refresh');
                $('#DuplicateBenefitCode').selectpicker('refresh');
            });
        },
        resetAddBenefitError() {
            $('#addBenefitError').empty();
            $('#addBenefitError').hide();
        },
        toggleBenefitIsDuplicate() {
            $('#DuplicateBenefitCode').prop('disabled', !this.BenefitModal.IsDuplicate);
            $('#DuplicateBenefitCode').selectpicker('refresh');
        },
        changeBenefitName(index) {
            var benefit = this.Benefits[index];

            benefit.Code = benefit.BenefitCode + ' - ' + benefit.Name;
        },
        validateAddBenefit() {
            var errors = [];
            var code = '';

            if (this.BenefitModal.BenefitId == null)
                errors.push("Benefit Code is required");
            else
                code = $("#BenefitId option:selected").text() + ' - ';

            this.BenefitModal.Name = this.BenefitModal.Name.trim();
            if (this.BenefitModal.Name == '')
                errors.push("Benefit Marketing Name is required");
            else if (this.Benefits.some(b => b.Code == code + this.BenefitModal.Name))
                errors.push("Duplicate benefit and marketing name combination found");

            if (this.BenefitModal.IsDuplicate && this.BenefitModal.DuplicateBenefitCode == null)
                errors.push("Duplicate Benefit is required when duplicate is set to Yes");

            if (errors.length == 0)
                return true;

            $('#addBenefitError').append(arrayToUnorderedList(errors));
            $('#addBenefitError').show();
            return false;
        },
        saveBenefit: function () {
            this.resetAddBenefitError();
            if (!this.validateAddBenefit())
                return;

            var benefit = {};
            var directRetros = [];

            if (this.BenefitModal.IsDuplicate) {
                var duplicateBenefitCode = this.BenefitModal.DuplicateBenefitCode;
                var duplicateBenefit = this.Benefits.find(b => b.Code == duplicateBenefitCode);

                benefit = $.extend({}, duplicateBenefit);
                benefit.TreatyPricingProductBenefitDirectRetroBos.forEach(function (dr) {
                    var directRetro = $.extend({}, dr);
                    directRetro.Id = 0;
                    directRetros.push(directRetro);
                });
            } 

            benefit.Id = 0;
            benefit.BenefitId = this.BenefitModal.BenefitId;
            benefit.BenefitCode = $("#BenefitId option:selected").text();
            benefit.Name = this.BenefitModal.Name;
            benefit.TreatyPricingProductBenefitDirectRetroBos = directRetros;

            benefit.Code = benefit.BenefitCode + ' - ' + benefit.Name;

            this.Benefits.push(benefit);

            this.refreshBenefitCodeSelection();

            $('#addBenefitModal').modal('hide');

            this.$nextTick(function () {
                $('.selectpicker').selectpicker('refresh');
                resetDisableFields();
            });
        },
        removeBenefit: function (index) {
            if (ReadOnly || this.Disable)
                return;

            var benefit = this.Benefits[index];

            this.RemoveBenefitCode = benefit.Code;
            this.RemoveBenefitIndex = index;
            $('#removeBenefitModal').modal('show');
        },
        confirmRemoveBenefit: function () {
            if (ReadOnly)
                return;

            this.Benefits.splice(this.RemoveBenefitIndex, 1);
            this.RemoveBenefitIndex = null;
            $('#removeBenefitModal').modal('hide');

            this.$nextTick(function () {
                $('.selectpicker').selectpicker('refresh');
            });
        },
        resetDirectRetroModal: function (benefitIndex) {
            var benefit = this.Benefits[benefitIndex];
            var directRetros = benefit.TreatyPricingProductBenefitDirectRetroBos ?? [];

            this.DirectRetroModal.BenefitIndex = benefitIndex;
            this.DirectRetroModal.BenefitCode = benefit.BenefitCode;
            this.DirectRetroModal.RetroPartyId = null;
            this.DirectRetroModal.IsDuplicate = false;
            this.DirectRetroModal.DuplicateRetroPartyId = null;

            var retroPartyIds = directRetros.map(b => b.RetroPartyId);
            this.RetroPartyCodes = RetroParties.filter(function (retroParty) {
                return !retroPartyIds.includes(retroParty.Id);
            });
            this.ExistingRetroPartyCodes = RetroParties.filter(function (retroParty) {
                return retroPartyIds.includes(retroParty.Id);
            });

            this.resetAddDirectRetroError();

            this.$nextTick(function () {
                $('#RetroPartyId').selectpicker('refresh');
                $('#DuplicateRetroPartyId').selectpicker('refresh');
            });
        },
        resetAddDirectRetroError() {
            $('#addDirectRetroError').empty();
            $('#addDirectRetroError').hide();
        },
        validateAddDirectRetro() {
            this.resetAddDirectRetroError();

            var errors = [];
            if (this.DirectRetroModal.RetroPartyId == null)
                errors.push("Direct Retro Party is required");

            if (this.DirectRetroModal.IsDuplicate && this.BenefitModal.DuplicateRetroPartyId == null)
                errors.push("Duplicate Direct Retro is required when duplicate is set to Yes");

            if (errors.length == 0)
                return true;

            $('#addDirectRetroError').append(arrayToUnorderedList(errors));
            $('#addDirectRetroError').show();
            return false;
        },
        saveDirectRetro: function () {
            if (!this.validateAddDirectRetro())
                return;

            var directRetro = {};

            var benefit = this.Benefits[this.DirectRetroModal.BenefitIndex];
            if (this.DirectRetroModal.IsDuplicate) {
                var duplicateRetroPartyId = this.BenefitModal.DuplicateRetroPartyId;
                var duplicateDirectRetro = benefit.TreatyPricingProductBenefitDirectRetroBos.find(b => b.RetroPartyId == duplicateRetroPartyId);

                directRetro = $.extend({}, duplicateDirectRetro);
            }
            directRetro.Id = 0;
            directRetro.RetroPartyId = this.DirectRetroModal.RetroPartyId;
            directRetro.RetroPartyCode = $("#RetroPartyId option:selected").text();

            benefit.TreatyPricingProductBenefitDirectRetroBos.push(directRetro);
            this.$nextTick(function () {
                $('.selectpicker').selectpicker('refresh');
            });

            $('#addDirectRetroModal').modal('hide');
        },
        removeDirectRetro: function (index, drIndex) {
            if (ReadOnly)
                return;

            var benefit = this.Benefits[index];
            benefit.TreatyPricingProductBenefitDirectRetroBos.splice(drIndex, 1);

            this.$nextTick(function () {
                $('.selectpicker').selectpicker('refresh');
            });
        },
        clearDirectRetros: function () {
            this.Benefits.forEach(function (benefit) {
                benefit.TreatyPricingProductBenefitDirectRetroBos = [];
            });

            //this.$nextTick(function () {
            //    $('.selectpicker').selectpicker('refresh');
            //});
        },
        saveProduct: function () {
            $('#TreatyPricingProductBenefit').val(JSON.stringify(this.Benefits));
        },
        refreshSelectPicker: function () {
            this.$nextTick(function () {
                $('.selectpicker').selectpicker('refresh');
                $('.bootstrap-select').removeClass('dropup');
            });
        },
        disableBenefit: function (disableVersion) {
            this.$nextTick(function () {
                if (disableVersion) {
                    $("#benefitTab input:not('[type=hidden]'):not('.force-disable')").prop("disabled", "disabled");
                    $("#benefitTab textarea").prop("disabled", "disabled");
                    $("#benefitTab select:not('.force-disable')").prop("disabled", true);
                    $("#benefitTab button:not('.dropdown-toggle'):not('.btn-collapse')").prop("disabled", true);
                } else {
                    $("#benefitTab input:not('[type=hidden]'):not('.force-disable')").prop("disabled", false);
                    $("#benefitTab select:not('.force-disable')").prop("disabled", false);
                    $("#benefitTab button:not('.dropdown-toggle'):not('.btn-collapse')").prop("disabled", false);
                }
            });
        },
        editCampaignLink: function (id) {
            return EditCampaignUrl + '/' + id;
        },
        editGroupLink: function (id) {
            return EditGroupReferralUrl + '/' + id;
        },
        uploadPricingFile: function (index) {
            uploadFile('pricing' + index, false);

            var files = getFiles();
            if (!files)
                return;

            var doc = {};

            var file = files[0];
            var document = saveDocument(doc, file, false);

            this.Benefits[index].PricingUploadFileName = document.FileName;
            this.Benefits[index].PricingUploadHashFileName = document.HashFileName;
            this.Benefits[index].CanDownloadFile = false;
        },
        removePricingFile: function (index) {
            this.Benefits[index].PricingUploadFileName = null;
            this.Benefits[index].PricingUploadHashFileName = null;
            this.Benefits[index].CanDownloadFile = false;
        },
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.CreatedByName = AuthUserName;
            this.RemarkModal.ModuleId = this.Product.ModuleId;
            this.RemarkModal.ObjectId = this.Product.TreatyPricingCedantId;
            this.RemarkModal.SubModuleController = SubModuleController;
            this.RemarkModal.SubObjectId = this.Product.Id;
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
                remark.DocumentBos = this.saveDocuments(this.RemarkMaxIndex, remark.Id);

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
        saveDocuments(remarkIndex, remarkId = null) {
            var files = getFiles();

            if (!files) return;

            var parentId = this.Product.TreatyPricingCedantId;
            var document = {
                ModuleId: this.Product.ModuleId,
                ObjectId: parentId,
                SubModuleController: SubModuleController,
                SubObjectId: this.Product.Id,
                RemarkId: remarkId,
                RemarkIndex: remarkIndex,
                CreatedByName: AuthUserName,
                CreatedAtStr: this.RemarkModal.CreatedAtStr,
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
        // Final Document
        resetDocumentModal: function () {
            this.DocumentModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.DocumentModal.ModuleId = this.Product.ModuleId;
            this.DocumentModal.ObjectId = this.Product.TreatyPricingCedantId;
            this.DocumentModal.SubModuleController = SubModuleController;
            this.DocumentModal.SubObjectId = this.Product.Id;
            this.DocumentModal.File = null;
            this.DocumentModal.Filename = null;
            this.DocumentModal.Description = "";
            clearSelectedFiles();
        },
        saveDocument() {
            var file = $('#files')[0].files[0];
            var document = saveDocument(this.DocumentModal, file, true);

            if (document != null) {
                this.Documents.push(Object.assign({}, document));
                this.DocumentMaxIndex++;
            }
        },
        removeDocument(index) {
            if (ReadOnly)
                return;

            var document = this.Documents[index];

            this.RemoveDocumentName = document.Description;
            this.RemoveDocumentIndex = index;
            $('#removeDocumentModal').modal('show');
        },
        confirmRemoveDocument: function () {
            if (ReadOnly)
                return;

            var index = this.RemoveDocumentIndex;
            var document = this.Documents[index];

            if (deleteDocument(document)) {
                this.Documents.splice(index, 1);
                this.DocumentMaxIndex--;
            }
            $('#removeDocumentModal').modal('hide');
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
        updateWorkflowData: function (workflowObject) {
            $.ajax({
                url: GetWorkflowDataUrl ? GetWorkflowDataUrl : null,
                type: "POST",
                data: {
                    workflowObjectBo: workflowObject,
                },
                cache: false,
                async: false,
                success: function (data) {
                    $('#TargetSendDate').val(data.targetSendDate);
                    $('#LatestRevisionDate').val(data.latestRevisionDate);
                    $('#QuotationStatus').val(data.quotationStatus);
                    $('#QuotationStatusRemark').val(data.quotationStatusRemark);
                }
            });
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
    }
});

var setVersionCallBack = function (bo) {
    app.Benefits = JSON.parse(bo.TreatyPricingProductBenefit);
    app.refreshBenefitCodeSelection();
    app.Disable = disableVersion;

    resetDisableFields();

    resetTokenField('#TargetSegment');
    resetTokenField('#DistributionChannel');
    resetTokenField('#CessionType');
    resetTokenField('#ProductLine')

    setCkEditorInput('#GroupFreeCoverLimitNonCi');
    setCkEditorInput('#GroupFreeCoverLimitCi');
    setCkEditorInput('#GroupProfitCommission');
    setCkEditorInput('#OccupationalClassification');

    //$("#benefitTab .selectpicker").selectpicker("refresh");

    app.disableBenefit(disableVersion);
    app.refreshSelectPicker();

    $('.textarea-auto-expand').trigger('keyup');
}

function setCkEditorInput(id) {
    var inputId = id + 'Input';
    var value = $(id).val();

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

function resetTokenField(id) {
    var value = $(id).val();
    $(id).tokenfield('setTokens', value.split(", "));

    if (disableVersion) {
        $(id).tokenfield('disable');
    } else {
        $(id).tokenfield('enable');
    }
}