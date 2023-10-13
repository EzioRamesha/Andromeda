$(document).ready(function () {
    refreshRiDataMatchInput();
    refreshClaimDataMatchInput();
    getTreatyCodeByCedant(Model.CedingCompany);
    //getVueTreatyCodeByCedant(Model.CedingCompany);
    getConfigListByCedant(Model.CedingCompany);

    refreshSoaDataMatchInput();

    if (ReadOnly) {
        disableFields();
    } else {
        if (Model.IsClaim) {
            $("#generalTab input:not(.is-claim, .is-claim-only):not('[name=__RequestVerificationToken]'):not('[type=hidden]')").prop("disabled", "disabled");
            $("#generalTab select:not(.is-claim, .is-claim-only)").prop("disabled", true);
            $("#generalTab button:not(.is-claim, .is-claim-only):not('.dropdown-toggle')").prop("disabled", true);
            //$('#generalTab *:not(.is-claim, .is-claim-only)').prop('disabled', true);
        } else {
            $('#generalTab .is-claim-only').prop('disabled', true);
        }

        $('.selectpicker').selectpicker('refresh');
    }
});

function refreshSoaDataMatchInput() {
    if ($('#SoaDataBatchId').val() && $('#SoaDataBatchId').val() > 0) {
        $('#SoaDataStr').val('Matched');
    } else {
        $('#SoaDataStr').val('');
    }
}


function openYearPicker(id) {
    var id = '#' + id;
    if (typeof $(id).data("datepicker") === 'undefined') {
        $(id).datepicker({
            format: "yyyy",
            viewMode: "years",
            minViewMode: "years",
            autoclose: true,
        });
    }
    $(id).focus();
}

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

var app = new Vue({
    el: '#app',
    data: {
        ClaimRegister: Model,
        // RI Data
        RiDataWarehouse: Model.RiDataWarehouseBo,
        MatchRiData: {
            PolicyNumber: null,
            CedingPlanCode: null,
            RiskYear: null,
            RiskMonth: "",
            SoaQuarter: null,
            MlreBenefitCode: "",
            CedingBenefitRiskCode: null,
            TreatyCode: "",
            DateOfEvent: null,
        },
        MatchRiDataValidation: [],
        MatchedRiData: [],
        // Claim Data
        MatchClaimData: {
            PolicyNumber: null,
            CedingPlanCode: null,
            InsuredName: null,
            SoaQuarter: null,
            RiskQuarter: null,
            DateOfEvent: null,
            ClaimCode: null,
            TreatyCode: null,
            DateOfBirth: null,
            Searched: false,
            Error: false,
        },
        MatchClaimDataValidation: [],
        MatchedClaimDatas: [],
        // Soa Data
        SoaDataBatches: [],
        // Claim Data Lists
        ClaimCodes: DropDownClaimCodes,
        TreatyCodes: DropDownTreatyCodes,
        Months: Months,
        CedingBenefitTypeCodes: CedingBenefitTypeCodes,
        // Remark
        Remarks: RemarksList ? RemarksList : [],
        RemarkMaxIndex: 0,
        RemarkModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            Status: ClaimStatusReported ? ClaimStatusReported : null,
            CreatedAtStr: null,
            RemarkIsPrivate: false,
            Content: null,
            DocumentIsPrivate: false,
            HasFollowUp: false,
            FollowUpAtStr: null,
            FollowUpUserId: null,
        },
        UnderwritingRemarks: UnderwritingRemarks ? UnderwritingRemarks : [],
        // Status History
        StatusHistories: StatusHistoriesList ? StatusHistoriesList : [],
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
        // Changelog / UserTrails
        UserTrails: UserTrails ? UserTrails : [],
        // Related Claims
        RelatedClaims: RelatedClaims,
        // Finance Provisioning
        FinanceProvisioningTransactions: FinanceProvisioningTransactions,
        // Others
        ProvisionStatusProvisioned: ProvisionStatusProvisioned,
        ProvisionStatusPending: ProvisionStatusPending,
        ProvisionStatusFailed: ProvisionStatusFailed,
        // Auto Expand TextArea
        textAreaWidth: 150,
        textAreaHeight: 21,
    },
    methods: {
        // RI Data
        ResetMatchRiDataModal: function () {
            MatchRiDataValidation = [];
            this.MatchRiData = {
                PolicyNumber: this.ClaimRegister.PolicyNumber,
                CedingPlanCode: this.ClaimRegister.CedingPlanCode,
                RiskYear: this.ClaimRegister.RiskPeriodYear,
                RiskMonth: this.ClaimRegister.RiskPeriodMonth,
                SoaQuarter: this.ClaimRegister.SoaQuarter,
                MlreBenefitCode: this.ClaimRegister.MlreBenefitCode,
                CedingBenefitRiskCode: this.ClaimRegister.CedingBenefitRiskCode,
                TreatyCode: this.ClaimRegister.TreatyCode,
            };

            this.resetTreatyCodes();

            this.$nextTick(function () {
                $('.selectpicker').selectpicker('refresh');
            })

            MatchedRiData = [];
        },
        ValidateMatchRiData: function () {
            this.MatchRiDataValidation = [];
            if (this.MatchRiData.PolicyNumber == null || this.MatchRiData.PolicyNumber == "")
                this.MatchRiDataValidation.push("Policy Number is required");

            if (this.MatchRiData.RiskYear != null && this.MatchRiData.RiskYear != "" && !/[0-9]{4}/.test(this.MatchRiData.RiskYear))
                this.MatchRiDataValidation.push("Risk Year format is incorrect");

            if (this.MatchRiData.SoaQuarter != null && this.MatchRiData.SoaQuarter != "" && !/[0-9]{4} Q{1}([1-4]){1}/.test(this.MatchRiData.SoaQuarter))
                this.MatchRiDataValidation.push("SOA Quarter format is incorrect");

            if (this.MatchRiData.TreatyCode == null || this.MatchRiData.TreatyCode == "" || this.MatchRiData.TreatyCode == 0)
                this.MatchRiDataValidation.push("Treaty Code is required");

            return this.MatchRiDataValidation.length == 0;
        },
        SearchRiData: function () {
            if (!this.ValidateMatchRiData())
                return;

            var obj = {
                policyNumber: this.MatchRiData.PolicyNumber,
                CedingPlanCode: this.MatchRiData.CedingPlanCode,
                riskYear: this.MatchRiData.RiskYear,
                riskMonth: this.MatchRiData.RiskMonth,
                soaQuarter: this.MatchRiData.SoaQuarter,
                mlreBenefitCode: this.MatchRiData.MlreBenefitCode,
                cedingBenefitRiskCode: this.MatchRiData.CedingBenefitRiskCode,
                treatyCode: this.MatchRiData.TreatyCode,
                dateOfEventStr: this.MatchRiData.DateOfEvent,
            };

            var riDataWarehouseBos = [];
            $.ajax({
                url: SearchRIDataUrl,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    riDataWarehouseBos = data.riDataWarehouseBos;
                }
            });

            this.MatchedRiData = riDataWarehouseBos;
            if (this.MatchedRiData == null || !this.MatchedRiData.length)
                this.MatchRiDataValidation.push("No record found");
        },
        SetRiData: function () {
            if (document.querySelector('input[name="matchedRiDataOptions"]:checked')) {
                var index = document.querySelector('input[name="matchedRiDataOptions"]:checked').value;
                this.RiDataWarehouse = this.MatchedRiData[index];
                $('#RiDataWarehouseId').val(this.RiDataWarehouse.Id);
                $('#matchRiDataModal').modal('toggle');
                refreshRiDataMatchInput();
            } else {
                this.MatchRiDataValidation.push("No RI Data Selected");
                return;
            }
        },
        viewRiData: function (index) {
            var riData = this.MatchedRiData[index];
            return EditRiDataWarehouseUrl + '/' + riData.Id;
        },
        // Claim Data
        resetMatchClaimData: function () {
            this.resetMatchClaimDataSearch();

            this.MatchClaimData.PolicyNumber = this.ClaimRegister.PolicyNumber;
            this.MatchClaimData.CedingPlanCode = this.ClaimRegister.CedingPlanCode;
            this.MatchClaimData.InsuredName = this.ClaimRegister.InsuredName;
            this.MatchClaimData.SoaQuarter = this.ClaimRegister.SoaQuarter;
            this.MatchClaimData.RiskQuarter = this.ClaimRegister.RiskQuarter;
            this.MatchClaimData.DateOfEvent = this.ClaimRegister.DateOfEventStr;
            this.MatchClaimData.ClaimCode = this.ClaimRegister.ClaimCode;
            this.MatchClaimData.TreatyCode = this.ClaimRegister.TreatyCode;
            this.MatchClaimData.DateOfBirth = this.ClaimRegister.DateOfBirthStr;

            this.resetTreatyCodes();

            this.$nextTick(function () {
                $("#MatchClaimDataClaimCode").selectpicker('refresh');
                $("#MatchClaimDataTreatyCode").selectpicker('refresh');
            })
        },
        resetMatchClaimDataSearch: function () {
            this.MatchClaimData.Searched = false;
            this.MatchClaimData.Error = false;
            this.MatchedClaimDatas = [];
            this.MatchClaimDataValidation = [];
        },
        validateClaimData: function () {
            this.resetMatchClaimDataSearch();
            if (this.MatchClaimData.InsuredName == null || this.MatchClaimData.InsuredName == "")
                this.MatchClaimDataValidation.push("Insured Name is required");

            if (this.MatchClaimData.SoaQuarter != null && this.MatchClaimData.SoaQuarter != "" && !/[0-9]{4} Q{1}([1-4]){1}/.test(this.MatchClaimData.SoaQuarter))
                this.MatchClaimDataValidation.push("SOA Quarter format is incorrect");

            if (this.MatchClaimData.RiskQuarter != null && this.MatchClaimData.RiskQuarter != "" && !/[0-9]{4} Q{1}([1-4]){1}/.test(this.MatchClaimData.RiskQuarter))
                this.MatchClaimDataValidation.push("Risk Quarter format is incorrect");

            if (this.MatchClaimData.TreatyCode == null || this.MatchClaimData.TreatyCode == "" || this.MatchClaimData.TreatyCode == 0)
                this.MatchClaimDataValidation.push("Treaty Code is required");

            return this.MatchClaimDataValidation.length == 0;
        },
        searchClaimData: function () {
            if (!this.validateClaimData())
                return;

            var obj = {
                claimRegisterId: this.ClaimRegister.Id,
                policyNumber: this.MatchClaimData.PolicyNumber,
                cedingPlanCode: this.MatchClaimData.CedingPlanCode,
                insuredName: this.MatchClaimData.InsuredName,
                soaQuarter: this.MatchClaimData.SoaQuarter,
                riskQuarter: this.MatchClaimData.RiskQuarter,
                dateOfEventStr: this.MatchClaimData.DateOfEvent,
                claimCode: this.MatchClaimData.ClaimCode,
                treatyCode: this.MatchClaimData.TreatyCode,
                dateOfBirthStr: this.MatchClaimData.DateOfBirth,
            }

            var claimRegisterBos = [];
            var validations = [];
            $.ajax({
                url: SearchClaimDataUrl,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    claimRegisterBos = data.claimRegisterBos;
                    if (claimRegisterBos.length == 0) {
                        validations = ["No records found"]
                    }
                }
            });

            this.MatchClaimDataValidation = validations;
            this.MatchedClaimDatas = claimRegisterBos;
            this.MatchClaimData.Searched = true;
        },
        selectClaimData: function () {
            var index = $('input[name="selectClaimRegister"]:checked').val();
            if (typeof index == 'undefined') {
                this.MatchClaimDataValidation.push("No Claim Data Selected");
                return;
            }

            var id = this.ClaimRegister.Id; 
            var transactionType = this.ClaimRegister.ClaimTransactionType; 
            var entryNo = this.ClaimRegister.EntryNo; 

            this.ClaimRegister = this.MatchedClaimDatas[index];
            $('#OriginalClaimRegisterId').val(this.ClaimRegister.Id);
            this.ClaimRegister.Id = id;
            this.ClaimRegister.ClaimTransactionType = transactionType;
            this.ClaimRegister.EntryNo = entryNo;

            $('#matchClaimsDataModal').modal('toggle');

            refreshClaimDataMatchInput();
            getTreatyCodeByCedant(this.ClaimRegister.CedingCompany);
            //getVueTreatyCodeByCedant(this.ClaimRegister.CedingCompany);
            getConfigListByCedant(this.ClaimRegister.CedingCompany);

            this.$nextTick(function () {
                $(".selectpicker").selectpicker('refresh');
            })
        },
        // SOA Data
        resetSoaDataError: function () {
            $('#matchSoaDataError').empty();
            $('#matchSoaDataError').hide();
        },
        validateSoaDataBatch: function () {
            this.resetSoaDataError();
            var errors = [];

            if (this.ClaimRegister.SoaQuarter == null)
                errors.push("SOA Quarter is required");

            if (this.ClaimRegister.CedingCompany == null || this.ClaimRegister.CedingCompany == 0)
                errors.push("Ceding Company is required");

            if (this.ClaimRegister.TreatyCode == null || this.ClaimRegister.TreatyCode == 0)
                errors.push("Treaty Code is required");

            if (errors.length > 0) {
                $('#matchSoaDataError').append(arrayToUnorderedList(errors));
                $('#matchSoaDataError').show();
            }

            return errors.length == 0;
        },
        searchSoaDataBatch: function () {
            this.SoaDataBatches = [];
            if (!this.validateSoaDataBatch())
                return;

            var soaDataBatches = [];
            $.ajax({
                url: GetSoaDataBatchUrl,
                type: "POST",
                data: {
                    CedingCompany: this.ClaimRegister.CedingCompany,
                    TreatyCode: this.ClaimRegister.TreatyCode,
                    Quarter: this.ClaimRegister.SoaQuarter,
                },
                cache: false,
                async: false,
                success: function (data) {
                    soaDataBatches = data.soaDataBatchBos;
                }
            });
            this.SoaDataBatches = soaDataBatches;

            if (soaDataBatches.length == 0) {
                $('#matchSoaDataError').append(arrayToUnorderedList(["No match found"]));
                $('#matchSoaDataError').show();
            }
        },
        selectSoaDataBatch: function () {
            this.resetSoaDataError();

            var index = $('input[name="checkedSoadata"]:checked').val();
            if (index == null) {
                $('#matchSoaDataError').append(arrayToUnorderedList(["No SOA Data selected"]));
                $('#matchSoaDataError').show();
                return;
            }

            var item = this.SoaDataBatches[index];

            $('#SoaDataBatchId').val(item.Id);
            this.ClaimRegister.SoaDataBatchId = item.Id;
            refreshSoaDataMatchInput();

            $('#matchSoaDataModal').modal('hide');
        },
        createSoaDataBatch: function () {
            if (!this.validateSoaDataBatch())
                return;

            var soaDataBatchId = null;
            var result = null;
            var message = null;
            $.ajax({
                url: CreateSoaDataBatchUrl,
                type: "POST",
                data: {
                    CedingCompany: this.ClaimRegister.CedingCompany,
                    TreatyCode: this.ClaimRegister.TreatyCode,
                    Quarter: this.ClaimRegister.Quarter,
                },
                cache: false,
                async: false,
                success: function (data) {
                    result = data.success;
                    soaDataBatchId = data.resultId;
                    message = data.message;
                }
            });

            if (result == false) {
                $('#matchSoaDataError').append(arrayToUnorderedList([message]));
                $('#matchSoaDataError').show();
                return;
            }

            $('#SoaDataBatchId').val(soaDataBatchId);
            this.ClaimRegister.SoaDataBatchId = soaDataBatchId;
            refreshSoaDataMatchInput();

            $('#matchSoaDataModal').modal('hide');
        },
        viewSoaDataBatch: function () {
            var url = ViewSoaDataBatchUrl + '/' + this.ClaimRegister.SoaDataBatchId;
            return window.open(url, "_blank");
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
        setMatchValue: function (id, value) {
            if (id.includes("Modal")) {
                var idDetails = id.split("Modal");
                var model = idDetails[0] + "Modal";
                var field = idDetails[1];
            } else if (id.includes("Data")) {
                var idDetails = id.split("Data");
                var model = idDetails[0] + "Data";
                var field = idDetails[1];
            } else {
                var model = 'ClaimRegister';
                var field = id;
            }
            this[model][field] = value;
        },
        resetTreatyCodes: function () {
            var treatyCodes = DropDownTreatyCodes;
            $.ajax({
                url: GetDropDownTreatyCodeByCedantUrl,
                type: "POST",
                data: {
                    CedantCode: this.ClaimRegister.CedingCompany,
                    Status: TreatyCodeActive,
                },
                cache: false,
                async: false,
                success: function (data) {
                    treatyCodes = data.TreatyCodes;
                }
            });
            this.TreatyCodes = treatyCodes;
        },
        // Remark
        resetRemarkInfo: function (subModule = null) {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.Status = this.ClaimRegister.ClaimStatus == 0 ? ClaimStatusReported : this.ClaimRegister.ClaimStatus;
            this.RemarkModal.ModuleId = this.ClaimRegister.ModuleId;
            this.RemarkModal.ObjectId = this.ClaimRegister.Id;
            this.RemarkModal.RemarkIsPrivate = false;
            this.RemarkModal.Content = null;
            this.RemarkModal.DocumentIsPrivate = false;
            this.RemarkModal.HasFollowUp = false;
            this.RemarkModal.SubModuleController = subModule;
            this.clearFollowUpDetails();

            this.resetUnderwritingRemarkError();

            clearSelectedFiles('remark');
        },
        clearFollowUpDetails: function (check) {
            this.RemarkModal.FollowUpAtStr = null;
            this.RemarkModal.FollowUpUserId = null;

            $('#FollowUpUserId').selectpicker("refresh");

            if ($('#hasFollowUp')[0].checked)
                $('#FollowUpUserId').prop("disabled", false);
            else 
                $('#FollowUpUserId').prop("disabled", true);

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

            if (!this.ClaimRegister.Id) {
                remark = remarkModal;
                remark.StatusName = remark.Status == ClaimStatusRegistered ? "Registered" : "Reported" ;
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
        resetUnderwritingRemarkError: function () {
            $('#addUnderwritingRemarkError').empty();
            $('#addUnderwritingRemarkError').hide();
        },
        addUnderwritingRemark: function () {
            this.resetUnderwritingRemarkError();
            if (this.RemarkModal.Content == null || this.RemarkModal.Content == '') {
                $('#addUnderwritingRemarkError').append(arrayToUnorderedList(['Remark is required']));
                $('#addUnderwritingRemarkError').show();
                return;
            }

            var remark = null;
            if (!this.ClaimRegister.Id) {
                remark = this.RemarkModal;
            } else {
                remark = createRemark(this.RemarkModal);
            }

            if (remark) {
                remark.ShortContent = (remark.Content.length > 100) ? remark.Content.substring(0, 97) + "..." : remark.Content;
                this.UnderwritingRemarks.unshift(Object.assign({}, remark));

                $('#underwritingRemarkModal').modal('hide');
            }

            //var claimRegisterId = this.ClaimRegister.Id;
            //var remark = null;

            //if (!claimRegisterId) {
            //    remark = this.RemarkModal;
            //} else {
            //    var content = this.RemarkModal.Content;

            //    $.ajax({
            //        url: AddUnderwritingRemarkUrl ? AddUnderwritingRemarkUrl : null,
            //        type: "POST",
            //        data: {
            //            claimRegisterId: claimRegisterId,
            //            content: content,
            //        },
            //        cache: false,
            //        async: false,
            //        success: function (data) {
            //            remark = data.underwritingRemarkBo;
            //        }
            //    });
            //}

        },
        viewUnderwritingRemark: function (index) {
            var underwritingRemark = this.UnderwritingRemarks[index];
            this.RemarkModal.CreatedByName = underwritingRemark.CreatedByName;
            this.RemarkModal.CreatedAtStr = underwritingRemark.CreatedAtStr;
            this.RemarkModal.Content = underwritingRemark.Content;
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
        toggleChangelogDataView: function (index) {
            $('#showAll_' + index).toggle();
            $('#showLess_' + index).toggle();
            $('#showAllBtn_' + index).toggle();
            $('#collapseAllBtn_' + index).toggle();
        },
        // Documents
        saveDocuments(remarkIndex, remarkId = null) {
            var files = getFiles();

            if (!files) return;

            var parentId = this.ClaimRegister.Id;
            var document = {
                ModuleId: this.ClaimRegister.ModuleId,
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
        // Related Claims
        urlReferralEditLink: function (index) {
            var item = this.RelatedClaims[index];
            var url = UrlReferralEditLink + '/' + item.Id;
            return url
        }
    }
});

function focusOnDate(val) {
    $('#' + val).focus();
}

function getRelatedDropDown(cedantCode) {
    getTreatyCodeByCedant(cedantCode);
    //getVueTreatyCodeByCedant(cedantCode);
    getConfigListByCedant(cedantCode);
}

function getTreatyCodeByCedant(cedantCode) {
    var treatyCode = Model.TreatyCode;

    $.ajax({
        url: GetTreatyCodeByCedantUrl,
        type: "POST",
        data: {
            CedantCode: cedantCode,
            Status: TreatyCodeActive,
            SelectedCode: treatyCode,
        },
        cache: false,
        async: false,
        success: function (data) {
            var treatyCodes = data.TreatyCodes;
            refreshTreatyCodes('TreatyCode', treatyCodes, treatyCode, 'Code', 'Description')
        }
    });
}

function refreshTreatyCodes(id, items, selectedCode, first, second = "", style = true) {
    $(`#${id} option`).remove();
    $(`#${id}`).append(new Option("Please select", ""));
    items.forEach(function (obj) {
        var text = obj[first];
        if (second && obj[second])
            text += ` - ${obj[second]}`;
        $(`#${id}`).append(new Option(text, obj.Code, false, obj.Code == selectedCode));
    });
    if (style) {
        $(`#${id}`).selectpicker('refresh');
    }
}

function getConfigListByCedant(cedantCode) {
    var claimDataConfigId = Model.ClaimDataConfigId;

    if (cedantCode != null) {
        $.ajax({
            url: GetClaimDataConfigListUrl,
            type: "POST",
            data: { cedantCode: cedantCode },
            cache: false,
            async: false,
            success: function (data) {
                var claimDataConfigList = data.claimDataConfigBos;
                refreshDropDownItems('ClaimDataConfigId', claimDataConfigList, claimDataConfigId, 'Code', 'Name');
            }
        });
    } else {
        refreshDropDownItems('ClaimDataConfigId', [], null, 'Code', 'Name');
    }
}

function refreshRiDataMatchInput() {
    if (($('#RiDataWarehouseId').val() && $('#RiDataWarehouseId').val() > 0) || ($('#ReferralRiDataId').val() && $('#ReferralRiDataId').val() > 0)) {
        $('#RiDataStr').val('Matched');
    } else {
        $('#RiDataStr').val('');
    }
}

function refreshClaimDataMatchInput() {
    if ($('#OriginalClaimRegisterId').val() && $('#OriginalClaimRegisterId').val() > 0) {
        $('#ClaimRegisterStr').val('Matched');
    } else {
        $('#ClaimRegisterStr').val('');
    }
}

function SearchFilter() {
    var searchVal = $("#Label").val().toUpperCase();

    $("table tbody tr").each(function (index) {
        $row = $(this);
        //if (index !== 0) { }

        var codeVal = $row.find("td:first").text();

        if (searchVal !== '') {
            if (codeVal.indexOf(searchVal) === -1) {
                $row.hide();
            }
            else {
                $row.show();
            }
        }
        else {
            if (codeVal.indexOf(searchVal) === -1) {
                $row.hide();
            }
            else {
                $row.show();
            }
        }
    });
}

function SearchClear() {
    $("#Label").val('');
    var rows = $('table tbody tr');
    rows.show();
}