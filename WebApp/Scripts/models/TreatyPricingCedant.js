var app = new Vue({
    el: '#app',
    data: {
        Cedant: CedantModel,
        RepoCedant: Model,
        CurrentYear: CurrentYear,
        // Product
        DropDownTreatyPricingProducts: [],
        DropDownTreatyPricingProductVersions: [],
        TreatyPricingProducts: TreatyPricingProducts ? TreatyPricingProducts : [],
        ProductModal: {
            QuotationName: "",
            IsDuplicateExisting: false,
            CedantId: null,
            DuplicateTreatyPricingProductId: 0,
            DuplicateFromList: false,
        },
        ProductSummaryModal: {
            Code: "",
            Name: "",
            WorkflowObjects: [],
        },
        // RateTableGroup
        TreatyPricingRateTableGroups: TreatyPricingRateTableGroups ? TreatyPricingRateTableGroups : [],
        RateTableGroupModal: {
            Name: "",
            Description: "",
            Filename: "",
            StatusName: RateTableGroupStatusPendingName,
            NoOfRateTable: 0,
        },
        RateTableGroupError: "",
        // Underwriting Limit
        DropDownTreatyPricingUwLimits: [],
        DropDownTreatyPricingUwLimitVersions: [],
        TreatyPricingUwLimits: TreatyPricingUwLimits ? TreatyPricingUwLimits : [],
        UwLimitModal: {
            Name: "",
            IsDuplicateExisting: false,
            CedantId: null,
            DuplicateTreatyPricingUwLimitId: "",
            DuplicateFromList: false,
        },
        UwLimitDataValidation: [],
        // Underwriting Questionnaire
        TreatyPricingUwQuestionnaires: TreatyPricingUwQuestionnaires ? TreatyPricingUwQuestionnaires : [],
        UwQuestionnaireModal: {
            Name: "",
            IsDuplicateExisting: false,
            CedantId: null,
            DuplicateTreatyPricingUwQuestionnaireId: "",
            DuplicateTreatyPricingUwQuestionnaireVersionId: "",
            DuplicateFromList: false
        },
        DropDownTreatyPricingUwQuestionnaires: [],
        DropDownTreatyPricingUwQuestionnaireVersions: [],
        // Claim Approval Limit
        DropDownTreatyPricingCedants: DropDownTreatyPricingCedants,
        TreatyPricingClaimApprovalLimits: TreatyPricingClaimApprovalLimits ? TreatyPricingClaimApprovalLimits : [],
        DropDownTreatyPricingClaimApprovalLimits: [],
        DropDownTreatyPricingClaimApprovalLimitVersions: [],
        ClaimApprovalLimitModal: {
            Code: "",
            Name: "",
            IsDuplicateExisting: false,
            ClaimApprovalLimitId: "",
            ClaimApprovalLimitVersionId: "",
        },
        // Medical Table
        DropDownTreatyPricingMedicalTables: [],
        DropDownTreatyPricingMedicalTableVersions: [],
        TreatyPricingMedicalTables: TreatyPricingMedicalTables ? TreatyPricingMedicalTables : [],
        MedicalTableModal: {
            Name: "",
            IsDuplicateExisting: false,
            CedantId: null,
            DuplicateTreatyPricingMedicalTableId: "",
            DuplicateTreatyPricingMedicalTableVersionId: "",
        },
        // Financial Table
        DropDownTreatyPricingFinancialTables: [],
        DropDownTreatyPricingFinancialTableVersions: [],
        TreatyPricingFinancialTables: TreatyPricingFinancialTables ? TreatyPricingFinancialTables : [],
        FinancialTableModal: {
            Name: "",
            IsDuplicateExisting: false,
            CedantId: null,
            DuplicateTreatyPricingFinancialTableId: "",
        },
        FinancialTableDataValidation: [],
        // Advantage Program
        TreatyPricingAdvantagePrograms: TreatyPricingAdvantagePrograms ? TreatyPricingAdvantagePrograms : [],
        AdvantageProgramModal: {
            Name: "",
            IsDuplicateExisting: false,
            DuplicateTreatyPricingAdvantageProgramId: "",
        },
        DropDownTreatyPricingAdvantagePrograms: [],
        DropDownTreatyPricingAdvantageProgramVersions: [],
        // Definition And Exclusion
        DropDownTreatyPricingCedants: DropDownTreatyPricingCedants,
        TreatyPricingDefinitionAndExclusions: TreatyPricingDefinitionAndExclusions ? TreatyPricingDefinitionAndExclusions : [],
        DropDownTreatyPricingDefinitionAndExclusions: [],
        DropDownTreatyPricingDefinitionAndExclusionVersions: [],
        DefinitionAndExclusionModal: {
            Name: "",
            IsDuplicateExisting: false,
            Status: 2,
            DefinitionAndExclusionId: "",
        },
        // ProfitCommission
        DropDownTreatyPricingProfitCommissions: [],
        DropDownTreatyPricingProfitCommissionVersions: [],
        TreatyPricingProfitCommissions: TreatyPricingProfitCommissions ? TreatyPricingProfitCommissions : [],
        ProfitCommissionModal: {
            Name: "",
            IsDuplicateExisting: false,
            CedantId: "",
            DuplicateTreatyPricingProfitCommissionId: "",
            DuplicateTreatyPricingProfitCommissionVersionId: "",
        },
        // Custom Other
        DropDownTreatyPricingCustomOthers: [],
        DropDownTreatyPricingCustomOtherVersions: [],
        TreatyPricingCustomOthers: TreatyPricingCustomOthers ? TreatyPricingCustomOthers : [],
        CustomOtherModal: {
            Name: "",
            IsDuplicateExisting: false,
            CustomOtherId: "",
        },
        // Campaign
        TreatyPricingCampaigns: TreatyPricingCampaigns ? TreatyPricingCampaigns : [],
        CampaignModal: {
            Name: "",
            IsDuplicateExisting: false,
            CampaignId: "",
        },
        CampaignDataValidation: [],
        DropDownTreatyPricingCampaigns: [],
        DropDownTreatyPricingCampaignVersions: [],
        // Shared
        WorkflowObjects: TreatyPricingWorkflowObjectBos,
        DropDownObjects: [],
        DropDownObjectVersions: [],

        LinkObjectModal: {
            ObjectType: "",
            ObjectId: "",
            ObjectVersionId: "",
            ObjectTypeName: "",
            ObjectCode: "",
            ObjectName: "",
            ObjectVersion: "",
        },

        WorkflowModal: {
            WorkflowType: "",
            CedantId: "",
            ReinsuranceTypePickListDetailId: "",
            Name: "",
            Description: "",
            TreatyPricingQuotationWorkflowObjectBos: [{
                ObjectType: "",
                ObjectId: "",
                ObjectVersionId: "",
            }],
        },

        CloneModal: {
            ObjectName: "",
            ObjectId: "",
            Index: "",
        }
    },
    methods: {
        tab: function (id) {
            console.log(id);
            window.location.href = (window.location.href.split('?')[0]) + "?TabIndex=" + id;
        },
        // Product
        resetAddProductModal(duplicateFromList = false) {
            this.ProductModal.TreatyPricingCedantId = this.Cedant.Id;
            this.ProductModal.QuotationName = "";
            this.ProductModal.IsDuplicateExisting = duplicateFromList;
            this.ProductModal.DuplicateFromList = duplicateFromList;

            this.resetAddProductError();
            this.resetProductDuplicate();
        },
        resetProductDuplicate() {
            this.ProductModal.CedantId = null;
            this.ProductModal.DuplicateTreatyPricingProductId = null;
            this.ProductModal.DuplicateTreatyPricingProductVersionId = null;

            this.updateDropDownDuplicateObjects(GetProductByCedantUrl, 'Product');
            this.updateDropDownDuplicateObjectVersions(GetProductVersionByIdUrl, 'Product');

            this.$nextTick(function () {
                $('#dropDownTreatyPricingCedants').prop('disabled', !this.ProductModal.IsDuplicateExisting);
                $('#dropDownTreatyPricingProducts').prop('disabled', !this.ProductModal.IsDuplicateExisting);
                $('#dropDownTreatyPricingProductVersions').prop('disabled', !this.ProductModal.IsDuplicateExisting);

                $('#dropDownTreatyPricingCedants').selectpicker('refresh');
                $('#dropDownTreatyPricingProducts').selectpicker('refresh');
                $('#dropDownTreatyPricingProductVersions').selectpicker('refresh');
            });
        },
        resetAddProductError() {
            $('#addProductError').empty();
            $('#addProductError').hide();
        },
        saveProduct() {
            this.resetAddProductError();
            var isSuccess = false;
            var errors = [];
            var products = [];

            $.ajax({
                url: AddProductUrl,
                type: "POST",
                cache: false,
                async: false,
                data: { productBo: this.ProductModal },
                success: function (data) {
                    if (data.errors && data.errors.length > 0) {
                        errors = data.errors;
                    } else {
                        isSuccess = true;
                        products = data.productBos;
                    }
                }
            });

            if (isSuccess && products) {
                this.TreatyPricingProducts = products;
                this.tab(ActiveTab);
                //$('#addProductModal').modal('hide');
            } else if (errors.length > 0) {
                $('#addProductError').append(arrayToUnorderedList(errors));
                $('#addProductError').show();
            }
        },
        editProduct: function (index) {
            var item = this.TreatyPricingProducts[index];
            if (item != null && item.Id != '') {
                var url = EditProductUrl + item.Id;
                return url;
            }
        },
        cloneProduct: function (index) {
            var item = this.TreatyPricingProducts[index];
            console.log(index)
            console.log(item)
            this.resetAddProductModal(true);
            this.ProductModal.DuplicateTreatyPricingProductId = item.Id;
            this.saveProduct();
        },
        viewProductSummary: function (index) {
            var product = this.TreatyPricingProducts[index];
            this.ProductSummaryModal.Code = product.Code;
            this.ProductSummaryModal.Name = product.Name;

            var workflowObjects = [];
            $.ajax({
                url: GetProductWorkflowObjectsUrl ? GetProductWorkflowObjectsUrl : null,
                type: "POST",
                data: { id: product.Id },
                cache: false,
                async: false,
                success: function (data) {
                    workflowObjects = data.bos;
                }
            });
            this.ProductSummaryModal.WorkflowObjects = workflowObjects;

            $('#productSummaryModal').modal('show');
        },
        // RateTableGroup
        resetAddRateTableGroupValidation() {
            $('#addRateTableGroupError').empty();
            $('#addRateTableGroupError').hide();
        },
        resetAddRateTableGroupModal() {
            this.RateTableGroupModal.Name = "";
            this.RateTableGroupModal.Description = "";
            this.RateTableGroupModal.Filename = "";
            this.RateTableGroupModal.StatusName = RateTableGroupStatusPendingName;
            this.RateTableGroupModal.NoOfRateTable = 0;

            var upload = $('#rateTableGroupFiles');
            upload.val(null);
            var selectedFiles = document.querySelector(".rate-table-group#selectedFiles");
            selectedFiles.innerHTML = "";

            this.resetAddRateTableGroupValidation();
        },
        resetSearchRateTableGroup() {
            $("#RateTableGroupProduct").val("");

            $(".rateTableGroupTable table tbody tr").each(function (index) {
                $row = $(this);
                $row.show();
            });
        },
        uploadRateTableGroupFile() {
            var selectedFiles = document.querySelector(".rate-table-group#selectedFiles");
            var upload = $('#rateTableGroupFiles');
            selectedFiles.innerHTML = "";
            if (!upload[0].files[0]) return;

            var files = upload[0].files;
            this.RateTableGroupModal.Filename = files[0].name;
            var list = "<li>" + files[0].name + "</li>";
            selectedFiles.innerHTML = "<ul>" + list + "</ul>";
        },
        saveRateTableGroup() {
            this.resetAddRateTableGroupValidation();
            var isSuccess = false;
            var errorList = [];
            var rateTableGroups = [];

            var fileData = new FormData()
            if (this.RateTableGroupModal.Filename) {
                var upload = $('#rateTableGroupFiles');
                var file = upload[0].files[0];
                fileData.append(file.name, file);
            }

            fileData.append('treatyPricingCedantId', this.Cedant.Id);
            fileData.append('code', this.RateTableGroupModal.Code);
            fileData.append('name', this.RateTableGroupModal.Name);
            fileData.append('description', this.RateTableGroupModal.Description);

            // Save Temp File
            $.ajax({
                url: AddRateTableGroupUrl,
                type: "POST",
                contentType: false,
                processData: false,
                cache: false,
                async: false,
                data: fileData,
                success: function (data) {
                    if (data.errors && data.errors.length > 0) {
                        errorList = data.errors;
                    } else {
                        isSuccess = true;
                        rateTableGroups = data.rateTableGroupBos;
                    }
                }
            });

            if (isSuccess) {
                this.TreatyPricingRateTableGroups = rateTableGroups;
                this.resetSearchRateTableGroup();
                this.tab(ActiveTab);
                //$('#addRateTableGroupModal').modal('hide');
            } else if (errorList.length > 0) {
                $('#addRateTableGroupError').append(arrayToUnorderedList(errorList));
                $('#addRateTableGroupError').show();
            }
        },
        editRateTableGroupLink: function (index) {
            var item = this.TreatyPricingRateTableGroups[index];
            if (item != null && item.Id != '') {
                var url = EditRateTableGroupUrl + item.Id;
                return url
            }
        },
        showErrorModal: function (index) {
            this.RateTableGroupError = this.TreatyPricingRateTableGroups[index].FormattedErrors;
            $("#errorModal").modal()
        },
        // Underwriting Limit
        resetUwLimitModal(duplicateFromList = false) {
            this.UwLimitModal.TreatyPricingCedantId = this.Cedant.Id;
            this.UwLimitModal.Name = "";
            this.UwLimitModal.IsDuplicateExisting = duplicateFromList;
            this.UwLimitModal.DuplicateTreatyPricingUwLimitId = "";
            this.UwLimitModal.DuplicateTreatyPricingProductVersionId = "";
            this.UwLimitModal.DuplicateFromList = duplicateFromList;

            this.resetAddUwLimitValidation();
            this.resetUwLimitDuplicate();
        },
        resetSearchUwLimit() {
            $("#UwLimitProduct").val("");

            $(".uwLimitTable table tbody tr").each(function (index) {
                $row = $(this);
                $row.show();
            });
        },
        resetUwLimitDuplicate() {
            this.UwLimitModal.CedantId = null;
            this.UwLimitModal.DuplicateTreatyPricingUwLimitId = "";
            this.UwLimitModal.DuplicateTreatyPricingUwLimitVersionId = "";

            this.updateDropDownDuplicateObjects(GetUwLimitByCedantUrl, 'UwLimit', 'LimitId');
            this.updateDropDownDuplicateObjectVersions(GetUwLimitVersionByIdUrl, 'UwLimit');

            if (!this.UwLimitModal.IsDuplicateExisting) {
                $('#dropDownUwLimitCedants').prop('disabled', true);
                $('#dropDownTreatyPricingUwLimits').prop('disabled', true);
                $('#dropDownTreatyPricingUwLimitVersions').prop('disabled', true);
            } else {
                $('#dropDownUwLimitCedants').prop('disabled', false);
                $('#dropDownTreatyPricingUwLimits').prop('disabled', false);
                $('#dropDownTreatyPricingUwLimitVersions').prop('disabled', false);
            }

            this.$nextTick(function () {
                $('#dropDownUwLimitCedants').selectpicker('refresh');
                $('#dropDownTreatyPricingUwLimits').selectpicker('refresh');
                $('#dropDownTreatyPricingUwLimitVersions').selectpicker('refresh');
            });
        },
        resetAddUwLimitValidation() {
            $('#addUwLimitError').empty();
            $('#addUwLimitError').hide();
        },
        saveUwLimit() {
            this.resetAddUwLimitValidation();
            var isSuccess = false;
            var errors = [];
            var uwLimits = [];

            $.ajax({
                url: AddUwLimitUrl ? AddUwLimitUrl : null,
                type: "POST",
                data: { uwLimitBo: this.UwLimitModal },
                cache: false,
                async: false,
                success: function (data) {
                    if (data.errors && data.errors.length > 0) {
                        errors = data.errors;
                    } else {
                        isSuccess = true;
                        uwLimits = data.uwLimitBos;
                    }
                }
            });

            if (isSuccess && uwLimits) {
                this.TreatyPricingUwLimits = uwLimits;
                this.resetSearchUwLimit();
                this.tab(ActiveTab);
                //$('#addUwLimitModal').modal('hide');
            } else if (errors.length > 0) {
                $('#addUwLimitError').append(arrayToUnorderedList(errors));
                $('#addUwLimitError').show();
            }
        },
        editUwLimitLink: function (index) {
            var item = this.TreatyPricingUwLimits[index];
            if (item != null && item.Id != '') {
                var url = EditUwLimitUrl + item.Id;
                return url
            }
        },
        cloneUwLimit: function (index) {
            var item = this.TreatyPricingUwLimits[index];

            this.resetUwLimitModal(true);
            this.UwLimitModal.DuplicateTreatyPricingUwLimitId = item.Id;
            this.saveUwLimit();
        },
        // ClaimApprovalLimit
        resetAddClaimApprovalLimitValidation() {
            $('#addClaimApprovalLimitError').empty();
            $('#addClaimApprovalLimitError').hide();
        },
        resetAddClaimApprovalLimitModal(duplicateFromList = false) {
            this.ClaimApprovalLimitModal.TreatyPricingCedantId = this.Cedant.Id;
            this.ClaimApprovalLimitModal.Name = "";
            this.ClaimApprovalLimitModal.IsDuplicateExisting = duplicateFromList;
            this.ClaimApprovalLimitModal.DuplicateFromList = duplicateFromList;

            this.resetAddClaimApprovalLimitValidation();
            this.resetClaimApprovalLimitDuplicate();
        },
        resetClaimApprovalLimitDuplicate() {
            this.ClaimApprovalLimitModal.CedantId = "";
            this.ClaimApprovalLimitModal.DuplicateTreatyPricingClaimApprovalLimitId = "";
            this.ClaimApprovalLimitModal.DuplicateTreatyPricingClaimApprovalLimitVersionId = "";

            this.updateDropDownDuplicateObjects(GetClaimApprovalLimitByCedantUrl, 'ClaimApprovalLimit');
            this.updateDropDownDuplicateObjectVersions(GetClaimApprovalLimitVersionByIdUrl, 'ClaimApprovalLimit');

            if (!this.ClaimApprovalLimitModal.IsDuplicateExisting) {
                $('#dropDownClaimApprovalLimitCedants').prop('disabled', true);
                $('#dropDownTreatyPricingClaimApprovalLimits').prop('disabled', true);
                $('#dropDownTreatyPricingClaimApprovalLimitVersions').prop('disabled', true);
            } else {
                $('#dropDownClaimApprovalLimitCedants').prop('disabled', false);
                $('#dropDownTreatyPricingClaimApprovalLimits').prop('disabled', false);
                $('#dropDownTreatyPricingClaimApprovalLimitVersions').prop('disabled', false);
            }

            this.$nextTick(function () {
                $('#dropDownClaimApprovalLimitCedants').selectpicker('refresh');
                $('#dropDownTreatyPricingClaimApprovalLimits').selectpicker('refresh');
                $('#dropDownTreatyPricingClaimApprovalLimitVersions').selectpicker('refresh');
            });
        },
        resetSearchClaimApprovalLimit() {
            $("#ClaimApprovalLimitProduct").val("");

            $(".claimApprovalLimitTable table tbody tr").each(function (index) {
                $row = $(this);
                $row.show();
            });
        },
        saveClaimApprovalLimit() {
            this.resetAddClaimApprovalLimitValidation();
            var isSuccess = false;
            var errors = [];

            $.ajax({
                url: AddClaimApprovalLimitUrl,
                type: "POST",
                cache: false,
                async: false,
                data: { claimApprovalLimitBo: this.ClaimApprovalLimitModal },
                success: function (data) {
                    if (data.errors && data.errors.length > 0) {
                        errors = data.errors;
                    } else {
                        isSuccess = true;
                        claimApprovalLimitList = data.claimApprovalLimitBos;
                    }
                }
            });

            if (isSuccess) {
                this.TreatyPricingClaimApprovalLimits = claimApprovalLimitList;
                this.resetSearchClaimApprovalLimit();
                this.tab(ActiveTab);
                //$('#addClaimApprovalLimitModal').modal('hide');
            } else if (errors.length > 0) {
                $('#addClaimApprovalLimitError').append(arrayToUnorderedList(errors));
                $('#addClaimApprovalLimitError').show();
            }
        },
        editClaimApprovalLimitLink: function (index) {
            var item = this.TreatyPricingClaimApprovalLimits[index];
            if (item != null && item.Id != '') {
                var url = EditClaimApprovalLimitUrl + item.Id;
            }
            return url;
        },
        cloneClaimApprovalLimit: function (index) {
            var item = this.TreatyPricingClaimApprovalLimits[index];

            this.resetAddClaimApprovalLimitModal(true);
            this.ClaimApprovalLimitModal.DuplicateTreatyPricingClaimApprovalLimitId = item.Id;
            this.saveClaimApprovalLimit();
        },
        // Underwriting Questionnaire
        resetAddUwQuestionnaireValidation() {
            $('#addUwQuestionnaireError').empty();
            $('#addUwQuestionnaireError').hide();
        },
        resetUwQuestionnaireModal(duplicateFromList = false) {
            this.UwQuestionnaireModal.TreatyPricingCedantId = this.Cedant.Id;
            this.UwQuestionnaireModal.Name = "";
            this.UwQuestionnaireModal.IsDuplicateExisting = duplicateFromList;
            this.UwQuestionnaireModal.DuplicateTreatyPricingUwQuestionnaireId = "";
            this.UwQuestionnaireModal.DuplicateTreatyPricingUwQuestionnaireVersionId = "";
            this.UwQuestionnaireModal.DuplicateFromList = duplicateFromList;

            this.resetAddUwQuestionnaireValidation();
            this.resetUwQuestionnaireDuplicate();
        },
        resetUwQuestionnaireDuplicate() {
            this.UwQuestionnaireModal.CedantId = "";
            this.UwQuestionnaireModal.DuplicateTreatyPricingUwQuestionnaireId = "";
            this.UwQuestionnaireModal.DuplicateTreatyPricingUwQuestionnaireVersionId = "";

            this.updateDropDownDuplicateObjects(GetUwQuestionnaireByCedantUrl, 'UwQuestionnaire');
            this.updateDropDownDuplicateObjectVersions(GetUwQuestionnaireVersionByIdUrl, 'UwQuestionnaire');

            if (!this.UwQuestionnaireModal.IsDuplicateExisting) {
                $('#dropDownUwQuestionnaireCedants').prop('disabled', true);
                $('#dropDownTreatyPricingUwQuestionnaires').prop('disabled', true);
                $('#dropDownTreatyPricingUwQuestionnaireVersions').prop('disabled', true);
            } else {
                $('#dropDownUwQuestionnaireCedants').prop('disabled', false);
                $('#dropDownTreatyPricingUwQuestionnaires').prop('disabled', false);
                $('#dropDownTreatyPricingUwQuestionnaireVersions').prop('disabled', false);
            }

            this.$nextTick(function () {
                $('#dropDownUwQuestionnaireCedants').selectpicker('refresh');
                $('#dropDownTreatyPricingUwQuestionnaires').selectpicker('refresh');
                $('#dropDownTreatyPricingUwQuestionnaireVersions').selectpicker('refresh');
            });

            refreshDropDownItems("dropDownTreatyPricingUWQuestionnaires", this.TreatyPricingUwQuestionnaires, null, "Code", "Name");
        },
        resetSearchUwQuestionnaire() {
            $("#UwQuestionnaireProduct").val("");

            $(".uwQuestionnaireTable table tbody tr").each(function (index) {
                $row = $(this);
                $row.show();
            });
        },
        saveUwQuestionnaire() {
            this.resetAddUwQuestionnaireValidation();
            var isSuccess = false;
            var errors = [];
            var uwQuestionnaires = [];

            $.ajax({
                url: AddUwQuestionnaireUrl ? AddUwQuestionnaireUrl : null,
                type: "POST",
                data: { uwQuestionnaireBo: this.UwQuestionnaireModal },
                cache: false,
                async: false,
                success: function (data) {
                    if (data.errors && data.errors.length > 0) {
                        errors = data.errors;
                    } else {
                        isSuccess = true;
                        uwQuestionnaires = data.uwQuestionnaireBos;
                    }
                }
            });

            if (isSuccess) {
                this.TreatyPricingUwQuestionnaires = uwQuestionnaires;
                this.resetSearchUwQuestionnaire();
                this.tab(ActiveTab);
                //$('#addUwQuestionnaireModal').modal('hide');
            } else if (errors.length > 0) {
                $('#addUwQuestionnaireError').append(arrayToUnorderedList(errors));
                $('#addUwQuestionnaireError').show();
            }
        },
        editUwQuestionnaireLink: function (index) {
            var item = this.TreatyPricingUwQuestionnaires[index];
            if (item != null && item.Id != '') {
                var url = EditUwQuestionnaireUrl + item.Id;
                return url
            }
        },
        cloneUwQuestionnaire: function (index) {
            var item = this.TreatyPricingUwQuestionnaires[index];

            this.resetUwQuestionnaireModal(true);
            this.UwQuestionnaireModal.DuplicateTreatyPricingUwQuestionnaireId = item.Id;
            this.saveUwQuestionnaire();
        },
        // Medical Table
        resetAddMedicalTableValidation() {
            $('#addMedicalTableError').empty();
            $('#addMedicalTableError').hide();
        },
        resetMedicalTableModal(duplicateFromList = false) {
            this.MedicalTableModal.TreatyPricingCedantId = this.Cedant.Id;
            this.MedicalTableModal.Name = "";
            this.MedicalTableModal.IsDuplicateExisting = duplicateFromList;
            this.MedicalTableModal.DuplicateTreatyPricingMedicalTableId = "";
            this.MedicalTableModal.DuplicateFromList = duplicateFromList;

            this.resetAddMedicalTableValidation();
            this.resetMedicalTableDuplicate();
        },
        resetSearchMedicalTable() {
            $("#MedicalTableProduct").val("");

            $(".medicalTableTable table tbody tr").each(function (index) {
                $row = $(this);
                $row.show();
            });
        },
        resetMedicalTableDuplicate() {
            this.MedicalTableModal.CedantId = "";
            this.MedicalTableModal.DuplicateTreatyPricingMedicalTableId = "";
            this.MedicalTableModal.DuplicateTreatyPricingMedicalTableVersionId = "";

            this.updateDropDownDuplicateObjects(GetMedicalTableByCedantUrl, 'MedicalTable', 'MedicalTableId');
            this.updateDropDownDuplicateObjectVersions(GetMedicalTableVersionByIdUrl, 'MedicalTable');

            if (!this.MedicalTableModal.IsDuplicateExisting) {
                $('#dropDownMedicalTableCedants').prop('disabled', true);
                $('#dropDownTreatyPricingMedicalTables').prop('disabled', true);
                $('#dropDownTreatyPricingMedicalTableVersions').prop('disabled', true);
            } else {
                $('#dropDownMedicalTableCedants').prop('disabled', false);
                $('#dropDownTreatyPricingMedicalTables').prop('disabled', false);
                $('#dropDownTreatyPricingMedicalTableVersions').prop('disabled', false);
            }

            this.$nextTick(function () {
                $('#dropDownMedicalTableCedants').selectpicker('refresh');
                $('#dropDownTreatyPricingMedicalTables').selectpicker('refresh');
                $('#dropDownTreatyPricingMedicalTableVersions').selectpicker('refresh');
            });
        },
        saveMedicalTable() {
            this.resetAddMedicalTableValidation();
            var isSuccess = false;
            var errors = [];
            var medicalTables = [];

            $.ajax({
                url: AddMedicalTableUrl ? AddMedicalTableUrl : null,
                type: "POST",
                data: { medicalTableBo: this.MedicalTableModal },
                cache: false,
                async: false,
                success: function (data) {
                    if (data.errors && data.errors.length > 0) {
                        errors = data.errors;
                    } else {
                        isSuccess = true;
                        medicalTables = data.medicalTableBos;
                    }
                }
            });

            if (isSuccess) {
                this.TreatyPricingMedicalTables = medicalTables;
                this.resetSearchMedicalTable();
                this.tab(ActiveTab);
                //$('#addMedicalTableModal').modal('hide');
            } else if (errors.length > 0) {
                $('#addMedicalTableError').append(arrayToUnorderedList(errors));
                $('#addMedicalTableError').show();
            }
        },
        editMedicalTableLink: function (index) {
            var item = this.TreatyPricingMedicalTables[index];
            if (item != null && item.Id != '') {
                var url = EditMedicalTableUrl + item.Id;
                return url
            }
        },
        cloneMedicalTable: function (index) {
            var item = this.TreatyPricingMedicalTables[index];

            this.resetMedicalTableModal(true);
            this.MedicalTableModal.DuplicateTreatyPricingMedicalTableId = item.Id;
            this.saveMedicalTable();
        },
        // Financial Table
        resetAddFinancialTableValidation() {
            $('#addFinancialTableError').empty();
            $('#addFinancialTableError').hide();
        },
        resetFinancialTableModal(duplicateFromList = false) {
            this.FinancialTableModal.TreatyPricingCedantId = this.Cedant.Id;
            this.FinancialTableModal.Name = "";
            this.FinancialTableModal.IsDuplicateExisting = duplicateFromList;
            this.FinancialTableModal.DuplicateTreatyPricingFinancialTableId = "";
            this.FinancialTableModal.DuplicateFromList = duplicateFromList;

            this.resetAddFinancialTableValidation();
            this.resetFinancialTableDuplicate();
        },
        resetSearchFinancialTable() {
            $("#FinancialTableProduct").val("");

            $(".financialTableTable table tbody tr").each(function (index) {
                $row = $(this);
                $row.show();
            });
        },
        resetFinancialTableDuplicate() {
            this.FinancialTableModal.CedantId = "";
            this.FinancialTableModal.DuplicateTreatyPricingFinancialTableId = "";

            this.updateDropDownDuplicateObjects(GetFinancialTableByCedantUrl, 'FinancialTable', 'FinancialTableId');
            this.updateDropDownDuplicateObjectVersions(GetFinancialTableVersionByIdUrl, 'FinancialTable');

            if (!this.FinancialTableModal.IsDuplicateExisting) {
                $('#dropDownFinancialTableCedants').prop('disabled', true);
                $('#dropDownTreatyPricingFinancialTables').prop('disabled', true);
                $('#dropDownTreatyPricingFinancialTableVersions').prop('disabled', true);
            } else {
                $('#dropDownFinancialTableCedants').prop('disabled', false);
                $('#dropDownTreatyPricingFinancialTables').prop('disabled', false);
                $('#dropDownTreatyPricingFinancialTableVersions').prop('disabled', false);
            }

            this.$nextTick(function () {
                $('#dropDownFinancialTableCedants').selectpicker('refresh');
                $('#dropDownTreatyPricingFinancialTables').selectpicker('refresh');
                $('#dropDownTreatyPricingFinancialTableVersions').selectpicker('refresh');
            });
        },
        saveFinancialTable() {
            this.resetAddFinancialTableValidation();
            var isSuccess = false;
            var errors = [];
            var financialTables = [];

            $.ajax({
                url: AddFinancialTableUrl ? AddFinancialTableUrl : null,
                type: "POST",
                data: { financialTableBo: this.FinancialTableModal },
                cache: false,
                async: false,
                success: function (data) {
                    if (data.errors && data.errors.length > 0) {
                        errors = data.errors;
                    } else {
                        isSuccess = true;
                        financialTables = data.financialTableBos;
                    }
                }
            });

            if (isSuccess) {
                this.TreatyPricingFinancialTables = financialTables;
                this.resetSearchFinancialTable();
                this.tab(ActiveTab);
                //$('#addFinancialTableModal').modal('hide');
            } else if (errors.length > 0) {
                $('#addFinancialTableError').append(arrayToUnorderedList(errors));
                $('#addFinancialTableError').show();
            }
        },
        editFinancialTableLink: function (index) {
            var item = this.TreatyPricingFinancialTables[index];
            if (item != null && item.Id != '') {
                var url = EditFinancialTableUrl + item.Id;
                return url
            }
        },
        cloneFinancialTable: function (index) {
            var item = this.TreatyPricingFinancialTables[index];

            this.resetFinancialTableModal(true);
            this.FinancialTableModal.DuplicateTreatyPricingFinancialTableId = item.Id;
            this.saveFinancialTable();
        },
        // Advantage Program
        resetAddAdvantageProgramValidation() {
            $('#addAdvantageProgramError').empty();
            $('#addAdvantageProgramError').hide();
        },
        resetAdvantageProgramModal(duplicateFromList = false) {
            this.AdvantageProgramModal.TreatyPricingCedantId = this.Cedant.Id;
            this.AdvantageProgramModal.Name = "";
            this.AdvantageProgramModal.IsDuplicateExisting = duplicateFromList;
            this.AdvantageProgramModal.AdvantageProgramId = "";
            this.AdvantageProgramModal.DuplicateFromList = duplicateFromList;

            this.resetAddAdvantageProgramValidation();
            this.resetAdvantageProgramDuplicate();
        },
        resetAdvantageProgramDuplicate() {
            this.AdvantageProgramModal.CedantId = "";
            this.AdvantageProgramModal.DuplicateTreatyPricingAdvantageProgramId = "";
            this.AdvantageProgramModal.DuplicateTreatyPricingAdvantageProgramVersionId = "";

            this.updateDropDownDuplicateObjects(GetAdvantageProgramByCedantUrl, 'AdvantageProgram');
            this.updateDropDownDuplicateObjectVersions(GetAdvantageProgramVersionByIdUrl, 'AdvantageProgram');

            if (!this.AdvantageProgramModal.IsDuplicateExisting) {
                $('#dropDownAdvantageProgramCedants').prop('disabled', true);
                $('#dropDownTreatyPricingAdvantagePrograms').prop('disabled', true);
                $('#dropDownTreatyPricingAdvantageProgramVersions').prop('disabled', true);
            } else {
                $('#dropDownAdvantageProgramCedants').prop('disabled', false);
                $('#dropDownTreatyPricingAdvantagePrograms').prop('disabled', false);
                $('#dropDownTreatyPricingAdvantageProgramVersions').prop('disabled', false);
            }

            this.$nextTick(function () {
                $('#dropDownAdvantageProgramCedants').selectpicker('refresh');
                $('#dropDownTreatyPricingAdvantagePrograms').selectpicker('refresh');
                $('#dropDownTreatyPricingAdvantageProgramVersions').selectpicker('refresh');
            });
        },
        resetSearchAdvantageProgram() {
            $("#AdvantageProgramProduct").val("");

            $(".advantageProgramTable table tbody tr").each(function (index) {
                $row = $(this);
                $row.show();
            });
        },
        saveAdvantageProgram() {
            this.resetAddAdvantageProgramValidation();
            var isSuccess = false;
            var errors = [];
            var advantagePrograms = [];

            $.ajax({
                url: AddAdvantageProgramUrl ? AddAdvantageProgramUrl : null,
                type: "POST",
                data: { advantageProgramBo: this.AdvantageProgramModal },
                cache: false,
                async: false,
                success: function (data) {
                    if (data.errors && data.errors.length > 0) {
                        errors = data.errors;
                    } else {
                        isSuccess = true;
                        advantagePrograms = data.advantageProgramBos;
                    }
                }
            });

            if (isSuccess) {
                this.TreatyPricingAdvantagePrograms = advantagePrograms;
                this.resetSearchAdvantageProgram();
                this.tab(ActiveTab);
                /*$('#addAPModal').modal('hide');*/
            } else if (errors.length > 0) {
                $('#addAdvantageProgramError').append(arrayToUnorderedList(errors));
                $('#addAdvantageProgramError').show();
            }
        },
        editAdvantageProgramLink: function (index) {
            var item = this.TreatyPricingAdvantagePrograms[index];
            if (item != null && item.Id != '') {
                var url = EditAdvantageProgramUrl + item.Id;
                return url
            }
        },
        cloneAdvantageProgram: function (index) {
            var item = this.TreatyPricingAdvantagePrograms[index];

            this.resetAdvantageProgramModal(true);
            this.AdvantageProgramModal.DuplicateTreatyPricingAdvantageProgramId = item.Id;
            this.saveAdvantageProgram();
        },
        // Definition and Exclusion
        resetAddDefinitionAndExclusionValidation() {
            $('#addDefinitionAndExclusionError').empty();
            $('#addDefinitionAndExclusionError').hide();
        },
        resetAddDefinitionAndExclusionModal(duplicateFromList = false) {
            this.DefinitionAndExclusionModal.TreatyPricingCedantId = this.Cedant.Id;
            this.DefinitionAndExclusionModal.Name = "";
            this.DefinitionAndExclusionModal.IsDuplicateExisting = duplicateFromList;
            this.DefinitionAndExclusionModal.DefinitionAndExclusionId = "";
            this.DefinitionAndExclusionModal.DuplicateFromList = duplicateFromList;

            this.resetAddDefinitionAndExclusionValidation();
            this.resetDefinitionAndExclusionDuplicate();
        },
        resetDefinitionAndExclusionDuplicate() {
            this.DefinitionAndExclusionModal.CedantId = "";
            this.DefinitionAndExclusionModal.DuplicateTreatyPricingDefinitionAndExclusionId = "";
            this.DefinitionAndExclusionModal.DuplicateTreatyPricingDefinitionAndExclusionVersionId = "";

            this.updateDropDownDuplicateObjects(GetDefinitionAndExclusionByCedantUrl, 'DefinitionAndExclusion');
            this.updateDropDownDuplicateObjectVersions(GetDefinitionAndExclusionVersionByIdUrl, 'DefinitionAndExclusion');

            if (!this.DefinitionAndExclusionModal.IsDuplicateExisting) {
                $('#dropDownDefinitionAndExclusionCedants').prop('disabled', true);
                $('#dropDownTreatyPricingDefinitionAndExclusions').prop('disabled', true);
                $('#dropDownTreatyPricingDefinitionAndExclusionVersions').prop('disabled', true);
            } else {
                $('#dropDownDefinitionAndExclusionCedants').prop('disabled', false);
                $('#dropDownTreatyPricingDefinitionAndExclusions').prop('disabled', false);
                $('#dropDownTreatyPricingDefinitionAndExclusionVersions').prop('disabled', false);
            }

            this.$nextTick(function () {
                $('#dropDownDefinitionAndExclusionCedants').selectpicker('refresh');
                $('#dropDownTreatyPricingDefinitionAndExclusions').selectpicker('refresh');
                $('#dropDownTreatyPricingDefinitionAndExclusionVersions').selectpicker('refresh');
            });
        },
        resetSearchDefinitionAndExclusion() {
            $("#definitionAndExclusionProduct").val("");

            $(".definitionAndExclusionTable table tbody tr").each(function (index) {
                $row = $(this);
                $row.show();
            });
        },
        saveDefinitionAndExclusion() {
            this.resetAddDefinitionAndExclusionValidation();
            var isSuccess = false;
            var errors = [];
            var definitionAndExclusions = [];

            $.ajax({
                url: AddDefinitionAndExclusionUrl ? AddDefinitionAndExclusionUrl : null,
                type: "POST",
                data: { definitionAndExclusionBo: this.DefinitionAndExclusionModal },
                cache: false,
                async: false,
                success: function (data) {
                    if (data.errors && data.errors.length > 0) {
                        errors = data.errors;
                    } else {
                        isSuccess = true;
                        definitionAndExclusions = data.definitionAndExclusionBos;
                    }
                }
            });

            if (isSuccess) {
                this.TreatyPricingDefinitionAndExclusions = definitionAndExclusions;
                this.resetSearchDefinitionAndExclusion();
                this.tab(ActiveTab);
                //$('#addDefinitionAndExclusionModal').modal('hide');
            } else if (errors.length > 0) {
                $('#addDefinitionAndExclusionError').append(arrayToUnorderedList(errors));
                $('#addDefinitionAndExclusionError').show();
            }
        },
        editDefinitionAndExclusionLink: function (index) {
            var item = this.TreatyPricingDefinitionAndExclusions[index];
            if (item != null && item.Id != '') {
                var url = EditDefinitionAndExclusionUrl + item.Id;
            }
            return url;
        },
        cloneDefinitionAndExclusion: function (index) {
            var item = this.TreatyPricingDefinitionAndExclusions[index];

            this.resetAddDefinitionAndExclusionModal(true);
            this.DefinitionAndExclusionModal.DuplicateTreatyPricingDefinitionAndExclusionId = item.Id;
            this.saveDefinitionAndExclusion();
        },
        // ProfitCommission
        resetAddProfitCommissionModal(duplicateFromList = false) {
            this.ProfitCommissionModal.TreatyPricingCedantId = this.Cedant.Id;
            this.ProfitCommissionModal.Name = "";
            this.ProfitCommissionModal.IsDuplicateExisting = duplicateFromList;
            this.ProfitCommissionModal.DuplicateFromList = duplicateFromList;

            this.resetAddProfitCommissionError();
            this.resetProfitCommissionDuplicate();
        },
        resetProfitCommissionDuplicate() {
            this.ProfitCommissionModal.CedantId = "";
            this.ProfitCommissionModal.DuplicateTreatyPricingProfitCommissionId = "";
            this.ProfitCommissionModal.DuplicateTreatyPricingProfitCommissionVersionId = "";

            this.updateDropDownDuplicateObjects(GetProfitCommissionByCedantUrl, 'ProfitCommission');
            this.updateDropDownDuplicateObjectVersions(GetProfitCommissionVersionByIdUrl, 'ProfitCommission');

            if (!this.ProfitCommissionModal.IsDuplicateExisting) {
                $('#dropDownProfitCommissionCedants').prop('disabled', true);
                $('#dropDownTreatyPricingProfitCommissions').prop('disabled', true);
                $('#dropDownTreatyPricingProfitCommissionVersions').prop('disabled', true);
            } else {
                $('#dropDownProfitCommissionCedants').prop('disabled', false);
                $('#dropDownTreatyPricingProfitCommissions').prop('disabled', false);
                $('#dropDownTreatyPricingProfitCommissionVersions').prop('disabled', false);
            }

            this.$nextTick(function () {
                $('#dropDownProfitCommissionCedants').selectpicker('refresh');
                $('#dropDownTreatyPricingProfitCommissions').selectpicker('refresh');
                $('#dropDownTreatyPricingProfitCommissionVersions').selectpicker('refresh');
            });
        },
        resetAddProfitCommissionError() {
            $('#addProfitCommissionError').empty();
            $('#addProfitCommissionError').hide();
        },
        resetSearchProfitCommission() {
            $("#ProfitCommissionProduct").val("");

            $(".profitCommissionTable table tbody tr").each(function (index) {
                $row = $(this);
                $row.show();
            });
        },
        saveProfitCommission() {
            this.resetAddProfitCommissionError();
            var isSuccess = false;
            var errors = [];
            var profitCommissions = [];

            $.ajax({
                url: AddProfitCommissionUrl,
                type: "POST",
                cache: false,
                async: false,
                data: { profitCommissionBo: this.ProfitCommissionModal },
                success: function (data) {
                    if (data.errors && data.errors.length > 0) {
                        errors = data.errors;
                    } else {
                        isSuccess = true;
                        profitCommissions = data.profitCommissionBos;
                    }
                }
            });

            if (isSuccess) {
                this.TreatyPricingProfitCommissions = profitCommissions;
                this.resetSearchProfitCommission();
                this.tab(ActiveTab);
                //$('#addProfitCommissionModal').modal('hide');
            } else if (errors.length > 0) {
                $('#addProfitCommissionError').append(arrayToUnorderedList(errors));
                $('#addProfitCommissionError').show();
            }
        },
        editProfitCommissionLink: function (index) {
            var item = this.TreatyPricingProfitCommissions[index];
            if (item != null && item.Id != '') {
                var url = EditProfitCommissionUrl + item.Id;
                return url;
            }
        },
        cloneProfitCommission: function (index) {
            var item = this.TreatyPricingProfitCommissions[index];

            this.resetAddProfitCommissionModal(true);
            this.ProfitCommissionModal.DuplicateTreatyPricingProfitCommissionId = item.Id;
            this.saveProfitCommission();
        },
        // CustomOther
        resetAddCustomOtherValidation() {
            $('#addCustomOtherError').empty();
            $('#addCustomOtherError').hide();
        },
        resetAddCustomOtherModal(duplicateFromList = false) {
            this.CustomOtherModal.TreatyPricingCedantId = this.Cedant.Id;
            this.CustomOtherModal.Name = "";
            this.CustomOtherModal.IsDuplicateExisting = duplicateFromList;
            this.CustomOtherModal.DuplicateFromList = duplicateFromList;

            this.resetAddCustomOtherValidation();
            this.resetCustomOtherDuplicate();
        },
        resetCustomOtherDuplicate() {
            this.CustomOtherModal.CedantId = "";
            this.CustomOtherModal.DuplicateTreatyPricingCustomOtherId = "";
            this.CustomOtherModal.DuplicateTreatyPricingCustomOtherVersionId = "";

            this.updateDropDownDuplicateObjects(GetCustomOtherByCedantUrl, 'CustomOther');
            this.updateDropDownDuplicateObjectVersions(GetCustomOtherVersionByIdUrl, 'CustomOther');

            if (!this.CustomOtherModal.IsDuplicateExisting) {
                $('#dropDownCustomOtherCedants').prop('disabled', true);
                $('#dropDownTreatyPricingCustomOthers').prop('disabled', true);
                $('#dropDownTreatyPricingCustomOtherVersions').prop('disabled', true);
            } else {
                $('#dropDownCustomOtherCedants').prop('disabled', false);
                $('#dropDownTreatyPricingCustomOthers').prop('disabled', false);
                $('#dropDownTreatyPricingCustomOtherVersions').prop('disabled', false);
            }

            this.$nextTick(function () {
                $('#dropDownCustomOtherCedants').selectpicker('refresh');
                $('#dropDownTreatyPricingCustomOthers').selectpicker('refresh');
                $('#dropDownTreatyPricingCustomOtherVersions').selectpicker('refresh');
            });
        },
        resetSearchCustomOther() {
            $("#CustomOtherProduct").val("");

            $(".customOtherTable table tbody tr").each(function (index) {
                $row = $(this);
                $row.show();
            });
        },
        saveCustomOther() {
            this.resetAddCustomOtherValidation();
            var isSuccess = false;
            var errors = [];
            var customOthers = [];

            $.ajax({
                url: AddCustomOtherUrl,
                type: "POST",
                cache: false,
                async: false,
                data: { customOtherBo: this.CustomOtherModal },
                success: function (data) {
                    if (data.errors && data.errors.length > 0) {
                        errors = data.errors;
                    } else {
                        isSuccess = true;
                        customOthers = data.customOtherBos;
                    }
                }
            });

            if (isSuccess) {
                this.TreatyPricingCustomOthers = customOthers;
                this.resetSearchCustomOther();
                this.tab(ActiveTab);
                //$('#addCustomOtherModal').modal('hide');
            } else if (errors.length > 0) {
                $('#addCustomOtherError').append(arrayToUnorderedList(errors));
                $('#addCustomOtherError').show();
            }
        },
        editCustomOtherLink: function (index) {
            var item = this.TreatyPricingCustomOthers[index];
            if (item != null && item.Id != '') {
                var url = EditCustomOtherUrl + item.Id;
            }
            return url;
        },
        cloneCustomOther: function (index) {
            var item = this.TreatyPricingCustomOthers[index];

            this.resetAddCustomOtherModal(true);
            this.CustomOtherModal.DuplicateTreatyPricingCustomOtherId = item.Id;
            this.saveCustomOther();
        },
        // Campaign
        resetAddCampaignValidation() {
            $('#addCampaignError').empty();
            $('#addCampaignError').hide();
        },
        resetCampaignModal(duplicateFromList = false) {
            this.CampaignModal.TreatyPricingCedantId = this.Cedant.Id;
            this.CampaignModal.Name = "";
            this.CampaignModal.IsDuplicateExisting = duplicateFromList;
            this.CampaignModal.CampaignId = "";
            this.CampaignModal.DuplicateFromList = duplicateFromList;

            this.resetAddCampaignValidation();
            this.resetCampaignDuplicate();
        },
        resetCampaignDuplicate() {
            this.CampaignModal.CedantId = "";
            this.CampaignModal.DuplicateTreatyPricingCampaignId = "";
            this.CampaignModal.DuplicateTreatyPricingCampaignVersionId = "";

            this.updateDropDownDuplicateObjects(GetCampaignByCedantUrl, 'Campaign');
            this.updateDropDownDuplicateObjectVersions(GetCampaignVersionByIdUrl, 'Campaign');

            if (!this.CampaignModal.IsDuplicateExisting) {
                $('#dropDownCampaignCedants').prop('disabled', true);
                $('#dropDownTreatyPricingCampaigns').prop('disabled', true);
                $('#dropDownTreatyPricingCampaignVersions').prop('disabled', true);
            } else {
                $('#dropDownCampaignCedants').prop('disabled', false);
                $('#dropDownTreatyPricingCampaigns').prop('disabled', false);
                $('#dropDownTreatyPricingCampaignVersions').prop('disabled', false);
            }

            this.$nextTick(function () {
                $('#dropDownCampaignCedants').selectpicker('refresh');
                $('#dropDownTreatyPricingCampaigns').selectpicker('refresh');
                $('#dropDownTreatyPricingCampaignVersions').selectpicker('refresh');
            });
        },
        resetSearchCampaign() {
            $("#CampaignProduct").val("");

            $(".campaignTable table tbody tr").each(function (index) {
                $row = $(this);
                $row.show();
            });
        },
        saveCampaign() {
            this.resetAddCampaignValidation();
            var isSuccess = false;
            var errors = [];
            var campaigns = [];

            $.ajax({
                url: AddCampaignUrl ? AddCampaignUrl : null,
                type: "POST",
                data: { campaignBo: this.CampaignModal },
                cache: false,
                async: false,
                success: function (data) {
                    if (data.errors && data.errors.length > 0) {
                        errors = data.errors;
                    } else {
                        isSuccess = true;
                        campaigns = data.campaignBos;
                    }
                }
            });

            if (isSuccess) {
                this.TreatyPricingCampaigns = campaigns;
                this.resetSearchCampaign();
                this.tab(ActiveTab);
                //$('#addCampaignModal').modal('hide');
            } else if (errors.length > 0) {
                $('#addCampaignError').append(arrayToUnorderedList(errors));
                $('#addCampaignError').show();
            }
        },
        editCampaignLink: function (index) {
            var item = this.TreatyPricingCampaigns[index];
            if (item != null && item.Id != '') {
                var url = EditCampaignUrl + item.Id;
                return url
            }
        },
        cloneCampaign: function (index) {
            var item = this.TreatyPricingCampaigns[index];

            this.resetCampaignModal(true);
            this.CampaignModal.DuplicateTreatyPricingCampaignId = item.Id;
            this.saveCampaign();
        },
        openDatePicker: function (currentId) {
            var id = "#" + currentId;
            if ($(id).data("datepicker") != null) {
                $(id).datepicker("destroy");
            }

            $(id).datepicker({
                format: DateFormatDatePickerJs,
                autoclose: true,
            });

            $(id).focus();
        },
        // Shared
        updateDropDownDuplicateObjects(url, object, field = "Code") {
            var modalName = object + 'Modal';
            var idName = 'CedantId';
            var id = this[modalName][idName];

            var objects = [];
            $.ajax({
                url: url,
                type: "POST",
                cache: false,
                async: false,
                data: { id: id },
                success: function (data) {
                    objects = data.bos;
                }
            });

            var dropdownId = 'dropDownTreatyPricing' + object + 's';

            refreshDropDownItems(dropdownId, objects, null, field);
        },
        updateDropDownDuplicateObjectVersions(url, object, field = "Version") {
            var modalName = object + 'Modal';
            var idName = 'DuplicateTreatyPricing' + object + 'Id';
            var id = this[modalName][idName];

            var objectVersions = [];
            $.ajax({
                url: url,
                type: "POST",
                cache: false,
                async: false,
                data: { id: id },
                success: function (data) {
                    objectVersions = data.versionBos;
                }
            });

            if (objectVersions) {
                objectVersions.forEach(function (objectVersion, index) {
                    objectVersion.Version = objectVersion.Version + ".0";
                });
            }

            var dropdownId = 'dropDownTreatyPricing' + object + 'Versions';

            refreshDropDownItems(dropdownId, objectVersions, null, field);
        },
        resetWorkflowModal() {
            var currentCedant = this.RepoCedant.CedantId;
            var currentReinsuranceType = this.RepoCedant.ReinsuranceTypePickListDetailId;

            $('#dropDownCedant').selectpicker('val', currentCedant);
            $('#dropDownReinsuranceType').selectpicker('val', currentReinsuranceType);
            this.WorkflowModal.WorkflowType = "";
            this.WorkflowModal.CedantId = currentCedant;
            this.WorkflowModal.ReinsuranceTypePickListDetailId = currentReinsuranceType;
            this.WorkflowModal.Name = "";
            this.WorkflowModal.Description = "";
            this.WorkflowModal.TreatyPricingQuotationWorkflowObjectBos.length = 0;

            this.WorkflowObjects = [];

            $('#newWorkflowError').empty();
            $('#newWorkflowError').hide();
        },
        changeWorkflowType() {
            this.$nextTick(function () {
                $('#dropDownDocumentType').selectpicker('refresh');
                $('#dropDownRetroParty').selectpicker('refresh');
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
                data: { type: this.LinkObjectModal.ObjectType, cedantId: this.RepoCedant.Id },
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

            var objectEmpty = [];
            refreshDropDownItems("dropDownObjectId", objectEmpty, null, "Text");
            refreshDropDownItems("dropDownObjectVersionId", objectEmpty, null, "Text");
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
        validateAddWorkflow() {
            $('#newWorkflowError').empty();
            $('#newWorkflowError').hide();

            var errors = [];

            if (this.WorkflowModal.CedantId == null || this.WorkflowModal.CedantId == "")
                errors.push("Ceding Company is required");
            if (this.WorkflowModal.ReinsuranceTypePickListDetailId == null || this.WorkflowModal.ReinsuranceTypePickListDetailId == "" || this.WorkflowModal.ReinsuranceTypePickListDetailId == "0")
                errors.push("Reinsurance Type is required");
            if (this.WorkflowModal.WorkflowType) {
                if (this.WorkflowModal.DocumentType == null || this.WorkflowModal.DocumentType == "" || this.WorkflowModal.DocumentType == "0")
                    errors.push("Document Type is required");

                if (this.WorkflowModal.RetroPartyId == null || this.WorkflowModal.RetroPartyId == "" || this.WorkflowModal.RetroPartyId == "0")
                    errors.push("Inward Retro Party is required");
            }
            if (this.WorkflowModal.Name == null || this.WorkflowModal.Name == "")
                errors.push("Quotation Name is required");
            if (this.WorkflowModal.Description == null || this.WorkflowModal.Description == "")
                errors.push("Description is required");
            if (this.WorkflowObjects.length == 0)
                errors.push("At least one object is required");

            if (errors.length > 0) {
                $('#newWorkflowError').append(arrayToUnorderedList(errors));
                $('#newWorkflowError').show();
            }

            return errors.length == 0;
        },
        saveWorkflow() {
            if (!this.validateAddWorkflow())
                return;

            var workflowBo = $.extend({}, this.WorkflowModal);
            workflowBo.TreatyPricingWorkflowObjectBos = this.WorkflowObjects;

            var errorList = [];
            var resultBo = [];

            if (this.WorkflowModal.WorkflowType == null || this.WorkflowModal.WorkflowType == false) {
                $.ajax({
                    url: AddQuotationWorkflowUrl,
                    type: "POST",
                    data: workflowBo,
                    cache: false,
                    async: false,
                    success: function (data) {
                        errorList = data.errors;
                        resultBo = data.quotationWorkflowBo;
                    }
                });
            }
            else {
                var treatyWorkflowModal = {
                    ReinsuranceTypePickListDetailId: this.WorkflowModal.ReinsuranceTypePickListDetailId,
                    DocumentType: this.WorkflowModal.DocumentType,
                    InwardRetroPartyDetailId: this.WorkflowModal.RetroPartyId,
                    CounterPartyDetailId: this.WorkflowModal.CedantId,
                    DocumentId: this.WorkflowModal.Name,
                    Description: this.WorkflowModal.Description,
                    TreatyPricingWorkflowObjectBos: this.WorkflowObjects,
                };

                workflowBo = $.extend({}, treatyWorkflowModal);

                $.ajax({
                    url: AddTreatyWorkflowUrl,
                    type: "POST",
                    data: workflowBo,
                    cache: false,
                    async: false,
                    success: function (data) {
                        errorList = data.errors;
                        resultBo = data.bo;
                    }
                });
            }

            if (errorList.length > 0) {
                $('#newWorkflowError').append(arrayToUnorderedList(errorList));
                $('#newWorkflowError').show();
            }
            else {
                //redirect to workflow
                var workflowId = resultBo.Id;
                console.log("Workflow created");
                console.log(workflowId);

                var editUrl = EditTreatyWorkflowUrl;
                if (this.WorkflowModal.WorkflowType == null || this.WorkflowModal.WorkflowType == false) {
                    editUrl = EditQuotationWorkflowUrl;
                }
                window.location.href = editUrl + resultBo.Id;
            }
        },
        resetCloneModal: function (objectName, objectId, index) {
            this.CloneModal.ObjectName = objectName;
            this.CloneModal.ObjectId = objectId;
            this.CloneModal.Index = index;
        },
        cloneObject: function () {
            switch (this.CloneModal.ObjectName) {
                case 'Product':
                    this.cloneProduct(this.CloneModal.Index);
                    break;
                case 'Underwriting Limit':
                    this.cloneUwLimit(this.CloneModal.Index);
                    break;
                case 'Medical Table':
                    this.cloneMedicalTable(this.CloneModal.Index);
                    break;
                case 'Financial Table':
                    this.cloneFinancialTable(this.CloneModal.Index);
                    break;
                case 'Underwriting Questionnaire':
                    this.cloneUwQuestionnaire(this.CloneModal.Index);
                    break;
                case 'Advantage Program':
                    this.cloneAdvantageProgram(this.CloneModal.Index);
                    break;
                case 'Claim Approval Limit':
                    this.cloneClaimApprovalLimit(this.CloneModal.Index);
                    break;
                case 'Definitions & Exclusions':
                    this.cloneDefinitionAndExclusion(this.CloneModal.Index);
                    break;
                case 'Campaign':
                    this.cloneCampaign(this.CloneModal.Index);
                    break;
                case 'Profit Commission':
                    this.cloneProfitCommission(this.CloneModal.Index);
                    break;
                case 'Custom / Other ID':
                    this.cloneCustomOther(this.CloneModal.Index);
                    break;
                default:
            }

            $("#cloneModal").modal('hide');
        },
    },
});

function SearchRateTableGroup() {
    var searchVal = $("#RateTableGroupProduct").val();
    var RateTableGroup = [];
    var cedantId = app.Cedant.Id;

    if (searchVal) {
        $.ajax({
            url: SearchRateTableGroupUrl ? SearchRateTableGroupUrl : null,
            type: "POST",
            data: { cedantId: cedantId, productName: searchVal },
            cache: false,
            async: false,
            success: function (data) {
                RateTableGroup = data.RateTableGroup;
            }
        });
    }

    rateTableGroupCodeFilters = !searchVal ? null : RateTableGroup.map(v => v.toLowerCase());
    filterTable('TreatyPricingRateTableGroups', 'rateTableGroup', 'rateTableGroupCodeFilters');
}

function SearchProfitCommission() {
    var searchVal = $("#ProfitCommissionProduct").val();
    var ProfitCommission = [];
    var cedantId = app.Cedant.Id;

    if (searchVal) {
        $.ajax({
            url: SearchProfitCommissionUrl ? SearchProfitCommissionUrl : null,
            type: "POST",
            data: { cedantId: cedantId, productName: searchVal },
            cache: false,
            async: false,
            success: function (data) {
                ProfitCommission = data.ProfitCommission;
            }
        });
    }

    profitCommissionCodeFilters = !searchVal ? null : ProfitCommission.map(v => v.toLowerCase());
    filterTable('TreatyPricingProfitCommissions', 'profitCommission', 'profitCommissionCodeFilters');
}

function SearchUwLimit() {
    var searchVal = $("#UwLimitProduct").val();
    var UwLimit = [];
    var cedantId = app.Cedant.Id;

    if (searchVal) {
        $.ajax({
            url: SearchUwLimitUrl ? SearchUwLimitUrl : null,
            type: "POST",
            data: { cedantId: cedantId, productName: searchVal },
            cache: false,
            async: false,
            success: function (data) {
                UwLimit = data.UwLimit;
            }
        });
    }

    uwLimitCodeFilters = !searchVal ? null : UwLimit.map(v => v.toLowerCase());
    filterTable('TreatyPricingUwLimits', 'uwLimit', 'uwLimitCodeFilters');
}

function SearchMedicalTable() {
    var searchVal = $("#MedicalTableProduct").val();
    var MedicalTable = [];
    var cedantId = app.Cedant.Id;

    if (searchVal) {
        $.ajax({
            url: SearchMedicalTableUrl ? SearchMedicalTableUrl : null,
            type: "POST",
            data: { cedantId: cedantId, productName: searchVal },
            cache: false,
            async: false,
            success: function (data) {
                MedicalTable = data.MedicalTable;
            }
        });
    }

    medicalTableCodeFilters = !searchVal ? null : MedicalTable.map(v => v.toLowerCase());
    filterTable('TreatyPricingMedicalTables', 'medicalTable', 'medicalTableCodeFilters');
}

function SearchFinancialTable() {
    var searchVal = $("#FinancialTableProduct").val();
    var FinancialTable = [];
    var cedantId = app.Cedant.Id;

    if (searchVal) {
        $.ajax({
            url: SearchFinancialTableUrl ? SearchFinancialTableUrl : null,
            type: "POST",
            data: { cedantId: cedantId, productName: searchVal },
            cache: false,
            async: false,
            success: function (data) {
                FinancialTable = data.FinancialTable;
            }
        });
    }

    financialTableCodeFilters = !searchVal ? null : FinancialTable.map(v => v.toLowerCase());
    filterTable('TreatyPricingFinancialTables', 'financialTable', 'financialTableCodeFilters');
}

function SearchClaimApprovalLimit() {
    var searchVal = $("#ClaimApprovalLimitProduct").val();
    var ClaimApprovalLimit = [];
    var cedantId = app.Cedant.Id;

    if (searchVal) {
        $.ajax({
            url: SearchClaimApprovalLimitUrl ? SearchClaimApprovalLimitUrl : null,
            type: "POST",
            data: { cedantId: cedantId, productName: searchVal },
            cache: false,
            async: false,
            success: function (data) {
                ClaimApprovalLimit = data.ClaimApprovalLimit;
            }
        });
    }
    claimApprovalLimitCodeFilters = !searchVal ? null : ClaimApprovalLimit.map(v => v.toLowerCase());
    filterTable('TreatyPricingClaimApprovalLimits', 'claimApprovalLimit', 'claimApprovalLimitCodeFilters');

    //$(".claimApprovalLimitTable table tbody tr").each(function (index) {
    //    $row = $(this);
    //    $row.show();

    //    var codeVal = $row.find("td:first").text();

    //    if (!searchVal) {
    //        $row.show();
    //    } else if (!ClaimApprovalLimit || !ClaimApprovalLimit.includes(codeVal)) {
    //        $row.hide();
    //    }
    //});
}

function SearchDefinitionAndExclusion() {
    var searchVal = $("#DefinitionAndExclusionProduct").val();
    var DefinitionAndExclusion = [];
    var cedantId = app.Cedant.Id;

    if (searchVal) {
        $.ajax({
            url: SearchDefinitionAndExclusionUrl ? SearchDefinitionAndExclusionUrl : null,
            type: "POST",
            data: { cedantId: cedantId, productName: searchVal },
            cache: false,
            async: false,
            success: function (data) {
                DefinitionAndExclusion = data.DefinitionAndExclusion;
            }
        });
    }

    definitionAndExclusionCodeFilters = !searchVal ? null : DefinitionAndExclusion.map(v => v.toLowerCase());
    filterTable('TreatyPricingDefinitionAndExclusions', 'definitionAndExclusion', 'definitionAndExclusionCodeFilters');

    //$(".definitionAndExclusionTable table tbody tr").each(function (index) {
    //    $row = $(this);
    //    $row.show();

    //    var codeVal = $row.find("td:first").text();

    //    if (!searchVal) {
    //        $row.show();
    //    } else if (!DefinitionAndExclusion || !DefinitionAndExclusion.includes(codeVal)) {
    //        $row.hide();
    //    }
    //});
}

function SearchUWQuestionnaire() {
    var searchVal = $("#UwQuestionnaireProduct").val();
    var UWQuestionnaire = [];
    var cedantId = app.Cedant.Id;

    if (searchVal) {
        $.ajax({
            url: SearchUwQuestionnaireUrl ? SearchUwQuestionnaireUrl : null,
            type: "POST",
            data: { cedantId: cedantId, productName: searchVal },
            cache: false,
            async: false,
            success: function (data) {
                UWQuestionnaire = data.UWQuestionnaire;
            }
        });
    }

    uwQuestionnaireCodeFilters = !searchVal ? null : UWQuestionnaire.map(v => v.toLowerCase());
    filterTable('TreatyPricingUwQuestionnaires', 'uwQuestionnaire', 'uwQuestionnaireCodeFilters');
}

function SearchAdvantageProgram() {
    var searchVal = $("#AdvantageProgramProduct").val();
    var AdvantageProgram = [];
    var cedantId = app.Cedant.Id;

    if (searchVal) {
        $.ajax({
            url: SearchAdvantageProgramUrl ? SearchAdvantageProgramUrl : null,
            type: "POST",
            data: { cedantId: cedantId, productName: searchVal },
            cache: false,
            async: false,
            success: function (data) {
                AdvantageProgram = data.AdvantageProgram;
            }
        });
    }

    advantageProgramCodeFilters = !searchVal ? null : AdvantageProgram.map(v => v.toLowerCase());
    filterTable('TreatyPricingAdvantagePrograms', 'advantageProgram', 'advantageProgramCodeFilters');
}

function SearchCampaign() {
    var searchVal = $("#CampaignProduct").val();
    var Campaign = [];

    if (searchVal) {
        $.ajax({
            url: SearchCampaignUrl ? SearchCampaignUrl : null,
            type: "POST",
            data: { productName: searchVal },
            cache: false,
            async: false,
            success: function (data) {
                Campaign = data.Campaign;
            }
        });
    }

    campaignCodeFilters = !searchVal ? null : Campaign.map(v => v.toLowerCase());
    filterTable('TreatyPricingCampaigns', 'campaign', 'campaignCodeFilters');
}

function SearchCustomOther() {
    var searchVal = $("#CustomOtherProduct").val();
    var CustomOther = [];

    if (searchVal) {
        $.ajax({
            url: SearchCustomOtherUrl ? SearchCustomOtherUrl : null,
            type: "POST",
            data: { productName: searchVal },
            cache: false,
            async: false,
            success: function (data) {
                CustomOther = data.CustomOther;
            }
        });
    }
    customOtherCodeFilters = !searchVal ? null : CustomOther.map(v => v.toLowerCase());
    filterTable('TreatyPricingCustomOthers', 'customOther', 'customOtherCodeFilters');

    //$(".customOtherTable table tbody tr").each(function (index) {
    //    $row = $(this);
    //    $row.show();

    //    var codeVal = $row.find("td:first").text();

    //    if (!searchVal) {
    //        $row.show();
    //    } else if (!CustomOther || !CustomOther.includes(codeVal)) {
    //        $row.hide();
    //    }
    //});

}

function editWorkflow(index) {
    var workflowObject = app.ProductSummaryModal.WorkflowObjects[index];
    var url = BaseUrl + '/TreatyPricing' + workflowObject.TypeName + 'Workflow/Edit/' + workflowObject.WorkflowId;

    window.open(url, '_blank');

    return false;
}

// Code Filters
var advantageProgramCodeFilters = null;
var rateTableGroupCodeFilters = null;
var profitCommissionCodeFilters = null;
var uwLimitCodeFilters = null;
var medicalTableCodeFilters = null;
var financialTableCodeFilters = null;
var uwQuestionnaireCodeFilters = null;
var campaignCodeFilters = null;
var claimApprovalLimitCodeFilters = null;
var definitionAndExculsionCodeFilters = null;
var customOtherCodeFilters = null;