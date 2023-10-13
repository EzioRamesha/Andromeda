$(document).ready(function () {
    $('#EffectiveDate').datepicker({
        format: DateFormatDatePickerJs,
        autoclose: true
    }).on('changeDate', function () {
        Model.EffectiveDateStr = $(this).val();
    });

    //var version = $('#CurrentVersion').val();
    //getBenefits(Model.CurrentVersionObject.Version); //Already get from callback
});

function getBenefits(versionId) {
    var benefits = [];
    $.ajax({
        url: GetVersionDetailUrl,
        type: "POST",
        cache: false,
        async: false,
        data: {
            treatyPricingAdvantageProgramVersionId: versionId,
        },
        success: function (data) {
            benefits = data.benefitBos;
        }
    });
    app.Benefits = benefits;
}

var app = new Vue({
    el: '#app',
    data: {
        Model: Model,
        ModelVersion: Model.CurrentVersionObject,
        Disable: false,
        // Benefit
        Benefits: AdvantageProgramBenefit,
        BenefitCodes: [],
        ExistingBenefitCodes: [],
        BenefitMaxIndex: 0,        
        BenefitModal: {
            BenefitId: "",
            IsDuplicateExisting: false,
            DuplicateBenefitId: 0,
        },
        BenefitDataValidation: [],
        RemoveBenefitCode: null,
        RemoveBenefitIndex: null,
        // Product
        Products: Products ? Products : [],
        ProductMaxIndex: 0,
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
        // Benefit
        refreshBenefitCodeSelection() {
            var benefitIds = this.Benefits.map(b => b.BenefitId);
            this.BenefitCodes = Benefits.filter(function (benefit) {
                return !benefitIds.includes(benefit.Id);
            });
            this.ExistingBenefitCodes = Benefits.filter(function (benefit) {
                return benefitIds.includes(benefit.Id);
            });

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
        },
        addBenefit() {
            var benefit = {};
            var errorList = [];

            if ($('#CurrentVersion').val() != this.ModelVersion.Version) {
                errorList.push("You can only upload data for the latest version");
            }

            if (!this.BenefitModal.BenefitId) {
                errorList.push("Benefit Code is Required.");
            }

            if (errorList.length == 0) {

                if (this.BenefitModal.IsDuplicateExisting) {
                    var duplicateBenefitId = this.BenefitModal.DuplicateBenefitId;

                    var duplicateBenefit = this.Benefits.find(e => e.BenefitId == duplicateBenefitId);
                    benefit = $.extend({}, duplicateBenefit);
                }

                benefit.Id = 0;
                benefit.BenefitId = this.BenefitModal.BenefitId;
                benefit.BenefitCode = $("#BenefitId option:selected").text();

                this.Benefits.push(benefit);

                this.refreshBenefitCodeSelection();

                this.BenefitMaxIndex++;
                $('#benefitModal').modal('hide');
            }

            if (errorList.length > 0) {
                this.BenefitDataValidation = errorList;
                return;
            }
        },
        //removeBenefit: function (index) {
        //    this.Benefits.splice(index, 1);
        //    this.BenefitMaxIndex--;
        //},
        removeBenefit: function (index) {
            if (ReadOnly || this.Disable)
                return;

            var benefit = this.Benefits[index];
            console.log(benefit)
            this.RemoveBenefitCode = benefit.BenefitCode;
            this.RemoveBenefitIndex = index;
            $('#removeBenefitModal').modal('show');
        },
        confirmRemoveBenefit: function () {
            if (ReadOnly)
                return;
            console.log(this.RemoveBenefitIndex)
            this.Benefits.splice(this.RemoveBenefitIndex, 1);
            this.BenefitMaxIndex--;
            this.RemoveBenefitIndex = null;
            $('#removeBenefitModal').modal('hide');

            this.refreshBenefitCodeSelection();
        },
        cloneBenefit: function (index) {
            var item = this.Benefits[index];  
            this.resetBenefitModal();

            this.BenefitModal.IsDuplicateExisting = true;
            this.BenefitModal.DuplicateBenefitId = item.BenefitId;

            $('#benefitModal').modal('toggle');
        },
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.CreatedByName = AuthUserName;
            this.RemarkModal.ModuleId = this.Model.ModuleId;
            this.RemarkModal.ObjectId = this.Model.TreatyPricingCedantId;
            this.RemarkModal.SubModuleController = SubModuleController;
            this.RemarkModal.SubObjectId = this.Model.Id;
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

            var parentId = this.Model.TreatyPricingCedantId;
            var document = {
                ModuleId: this.Model.ModuleId,
                ObjectId: parentId,
                SubModuleController: SubModuleController,
                SubObjectId: this.Model.Id,
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
    },
    created: function () {
        if (this.Benefits)
            this.BenefitMaxIndex = this.Benefits.length - 1;
        if (this.Products)
            this.ProductMaxIndex = this.Products.length - 1;
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});

var setVersionCallBack = function (bo) {
    getBenefits(bo.Id);
    app.refreshBenefitCodeSelection();
    app.Disable = disableVersion;
    app.ModelVersion = bo;

    //if (disableVersion) {
    //    $("#version a.accordion-link").click(function (e) {
    //        e.preventDefault();
    //    });
    //}
}