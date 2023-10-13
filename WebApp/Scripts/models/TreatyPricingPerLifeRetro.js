var tokenfieldReady = {
    targetSegment: false,
};

$(document).ready(function () {
    $('#EffectiveDateStr').datepicker({
        format: DateFormatDatePickerJs,
    });

    // Product Modal    
    
});

function changeProfitSharing(val) {
    if (!val) {
        $('#NetProfitPercentageDiv').hide();
        $('#TierProfitCommissionDiv').hide();
    } else if (val == ProfitSharingFlat) {
        $('#NetProfitPercentageDiv').show();
        $('#TierProfitCommissionDiv').hide();
    } else if (val == ProfitSharingTier) {
        $('#NetProfitPercentageDiv').hide();
        $('#TierProfitCommissionDiv').show();
    }
}

function changeIsEnabled(id) {
    var index = id.slice(15);
    var valueId = '#Value' + index;
    var isChecked = $('#' + id).prop("checked");
    if (!isChecked) {
        if (app.ProfitCommissionDetailRows && app.ProfitCommissionDetailRows[index])
            app.ProfitCommissionDetailRows[index].Value = "";
        $(valueId).prop('readonly', true);
        return;
    }
    $(valueId).prop('readonly', false);
}

function changeDropDown(val) {
    if (val == DropDownString) {
        if (app.ProfitCommissionDetailRows && app.ProfitCommissionDetailRows[12])
            app.ProfitCommissionDetailRows[12].Value = "";
        $('#Value12').prop('readonly', true);
        return;
    }
    $('#Value12').prop('readonly', false);
}

function updateProfitCommission(profitSharing) {
    changeProfitSharing(profitSharing);

    var detailValue = $('#ProfitCommissionDetail').val();
    if (detailValue) {
        app.ProfitCommissionDetailRows = JSON.parse(detailValue);
    } else {
        app.ProfitCommissionDetailRows = [];
    }

    var tierValue = $('#TierProfitCommission').val();
    if (tierValue) {
        app.TierProfitCommissionRows = JSON.parse(tierValue);
    } else {
        app.TierProfitCommissionRows = [];
    }

    app.Disable = disableVersion;

    app.$nextTick(function () {
        app.ProfitCommissionDetailRows.forEach(function (item, index) {
            if (!item.IsEnabled && item.IsEnabled != null) {
                var valueId = '#Value' + index;
                $(valueId).prop('readonly', true);
            }
        });

        var selectValue = $("#DropDownSelect").val();
        if (selectValue == DropDownString) {
            $('#Value12').prop('readonly', true);
        }
    })
}

var profitCommissionCallBack = updateProfitCommission;

var app = new Vue({
    el: '#app',
    data: {
        Model: Model,
        ModelVersion: Model.CurrentVersionObject,
        // Profit Comm
        ItemList: ItemList,
        ProfitCommissionDetailRows: [],
        TierProfitCommissionRows: [],
        DropDownDropDowns: DropDownDropDowns ? DropDownDropDowns : [],
        DropDownString: DropDownString,
        Disable: false,
        // Benefit
        Benefits: [],
        BenefitCodes: [],
        ExistingBenefitCodes: [],
        BenefitMaxIndex: 0,
        BenefitModal: {
            BenefitId: "",
            IsDuplicateExisting: false,
            DuplicateBenefitId: "",
        },
        BenefitDataValidation: [],
        DropDownAgeBasis: DropDownAgeBasis,
        DropDownArrangementRetrocessionnaireType: DropDownArrangementRetrocessionnaireType,
        //ListBenefits: ListBenefits,
        // Product
        PerLifeRetroProducts: PerLifeRetroProducts,
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
    },
    methods: {
        savePerLife: function () {
            $('#Benefits').val(JSON.stringify(this.Benefits));
        },
        // Profit Comm
        updateProfitCommissionDetail: function () {
            var json = JSON.stringify(this.ProfitCommissionDetailRows);
            $('#ProfitCommissionDetail').val(json);
        },
        updateTierProfitCommission: function () {
            var json = JSON.stringify(this.TierProfitCommissionRows);
            $('#TierProfitCommission').val(json);
        },
        // Benefit
        refreshBenefitCodeSelection() {
            var benefitIds = this.Benefits.map(b => b.BenefitId);
            this.BenefitCodes = Benefits.filter(function (benefit) {
                return !benefitIds.includes(benefit.Id);
            });
            this.ExistingBenefitCodes = Benefits.filter(function (benefit) {
                return benefitIds.includes(benefit.Id);
            });
            //console.log(this.BenefitCodes)
            //console.log(this.ExistingBenefitCodes)
            //refreshDropDownItems(id, items, selectedId, first, second = "", style = true, valueField = 'Id', displaySelect = true)
            //refreshDropDownItems("BenefitId", this.BenefitCodes, null, "Code", null);
            //refreshDropDownItems("DuplicateBenefitId", this.ExistingBenefitCodes, null, "Code", null);
            this.$nextTick(function () {
                $('#BenefitId').selectpicker('refresh');
                $('#DuplicateBenefitId').selectpicker('refresh');
            });
        },
        resetBenefitModal() {
            this.BenefitModal.BenefitId = "";
            this.BenefitModal.IsDuplicateExisting = false;
            this.BenefitModal.DuplicateBenefitId = "";

            this.$nextTick(function () {
                $('#BenefitId').selectpicker('refresh');
                $('#DuplicateBenefitId').selectpicker('refresh');
            });

            this.BenefitDataValidation = [];
            this.resetBenefitDuplicate();
            this.refreshBenefitCodeSelection();
        },
        resetBenefitDuplicate() {
            if (!this.BenefitModal.IsDuplicateExisting) {
                $('#DuplicateBenefitId').prop('disabled', true);
            } else {
                $('#DuplicateBenefitId').prop('disabled', false);
            }
            $('#DuplicateBenefitId').selectpicker('refresh');
        },
        addBenefit() {
            var errorList = [];

            if ($('#CurrentVersion').val() != this.ModelVersion.Version) {
                errorList.push("You can only upload data for the latest version");
            }

            if (!this.BenefitModal.BenefitId) {
                errorList.push("Benefit Code is Required.");
            }

            if (errorList.length == 0) {
                var benefit = {};

                if (this.BenefitModal.IsDuplicateExisting) {
                    var duplicateBenefitId = this.BenefitModal.DuplicateBenefitId;
                    var duplicateBenefit = this.Benefits.find(b => b.BenefitId == duplicateBenefitId);

                    benefit = $.extend({}, duplicateBenefit);
                }
                benefit.Id = 0;
                benefit.BenefitId = this.BenefitModal.BenefitId;

                var detail = this.BenefitCodes.filter(i => i.Id == benefit.BenefitId);
                benefit.BenefitCode = detail[0].Code;
                benefit.BenefitName = detail[0].Description;

                this.Benefits.push(benefit);

                this.refreshBenefitCodeSelection();
                this.$nextTick(function () {
                    $('.selectpicker').selectpicker('refresh');
                });

                $('#Benefit').val(JSON.stringify(this.Benefits));

                this.BenefitMaxIndex++;
                $('#benefitModal').modal('hide');
            }

            if (errorList.length > 0) {
                this.BenefitDataValidation = errorList;
                return;
            }
        },
        removeBenefit: function (index) {
            this.Benefits.splice(index, 1);
            this.BenefitMaxIndex--;
        },
        cloneBenefit: function (index) {
            var item = this.Benefits[index];
            this.resetBenefitModal();

            this.BenefitModal.IsDuplicateExisting = true;
            this.BenefitModal.DuplicateBenefitId = item.BenefitId;

            $('#benefitModal').modal('toggle');
        },
        disableBenefit: function (disableVersion) {
            this.$nextTick(function () {
                if (disableVersion) {
                    $("#benefit input:not('[type=hidden]')").prop("disabled", true);
                    $("#benefit select").prop("disabled", true);
                    $("#benefit button:not('.dropdown-toggle')").prop("disabled", true);
                } else {
                    $("#benefit input:not('[type=hidden]')").prop("disabled", false);
                    $("#benefit select").prop("disabled", false);
                    $("#benefit button:not('.dropdown-toggle')").prop("disabled", false);
                }
            });
        },
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.CreatedByName = AuthUserName;
            this.RemarkModal.ModuleId = this.Model.ModuleId;
            this.RemarkModal.ObjectId = this.Model.Id;
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

            var document = {
                ModuleId: this.Model.ModuleId,
                ObjectId: this.Model.Id,
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
        refreshSelectPicker: function () {
            this.$nextTick(function () {
                $('.selectpicker').selectpicker('refresh');
            });
        },
    },
    created: function () {
        if (this.Benefits)
            this.BenefitMaxIndex = this.Benefits.length - 1;
        if (this.PerLifeRetroProducts)
            this.TreatyPricingProductsMaxIndex = this.PerLifeRetroProducts.length - 1;
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});

var setVersionCallBack = function (bo) {
    app.Benefits = JSON.parse(bo.Benefits);
    app.refreshBenefitCodeSelection();
    app.Disable = disableVersion;
    app.ModelVersion = bo;

    app.disableBenefit(disableVersion);
    app.refreshSelectPicker();
}

function focusOnDate(val) {
    $('#' + val).focus();
}
