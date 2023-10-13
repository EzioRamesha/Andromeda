var app = new Vue({
    el: '#app',
    data: {
        PerLifeClaimRetroData: Model,
        RecoveryDetails: RecoveryDetails ? RecoveryDetails : [],
        ComparisonPreviousQuarter: ComparisonPreviousQuarter ? ComparisonPreviousQuarter : [],
        ComparisonCurrentQuarter: ComparisonCurrentQuarter ? ComparisonCurrentQuarter : [],
        DropDownClaimRecoveryDecision: [],
        PerLifeAggregationDetailData: Model.PerLifeAggregationDetailDataBo,
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
    },
    methods: {
        editRecoveryDetails: function (id) {
            var url = EditRecoveryDetailUrl + "/" + id;
            return url
        },

        //Remarks
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.CreatedByName = AuthUserName;
            this.RemarkModal.ModuleId = this.PerLifeClaimRetroData.ModuleId;
            this.RemarkModal.ObjectId = this.PerLifeClaimRetroData.TreatyPricingCedantId;
            this.RemarkModal.SubModuleController = SubModuleController;
            this.RemarkModal.SubObjectId = this.PerLifeClaimRetroData.Id;
            this.RemarkModal.Version = Model.CurrentVersion;
            this.RemarkModal.ShowSubjectSelect = false;
            this.RemarkModal.Content = null;

            this.toggleRemarkSubject();
            clearSelectedFiles('remark');
        },
        processClaimRecovery: function () {
            $.ajax({
                url: ProcessClaimRecoveryUrl,
                type: "POST",
                cache: false,
                async: false,
                data: { id: this.PerLifeClaimRetroData.PerLifeClaimDataBo.PerLifeClaimBo.Id },
                success: function (data) {
                    if (data.errors == "success") {
                        location.reload();
                    } else if (data.errors == "fail") {
                        $('#processClaimRecoveryModal').modal('show');
                    }
                    else {
                        // show error
                    }
                }
            });
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
            remark.DocumentBos = this.saveDocuments(this.RemarkMaxIndex, remark.Id);

            if (remark) {
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

            var parentId = this.PerLifeClaimRetroData.PerLifeClaimId;
            var document = {
                ModuleId: this.PerLifeClaimRetroData.ModuleId,
                ObjectId: parentId,
                SubModuleController: SubModuleController,
                SubObjectId: this.PerLifeClaimRetroData.Id,
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
    }
});