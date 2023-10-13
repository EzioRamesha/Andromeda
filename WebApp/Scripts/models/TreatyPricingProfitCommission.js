$(document).ready(function () {
    $('#EffectiveDateStr').datepicker({
        format: DateFormatDatePickerJs,
    });
    $('#StartDateStr').datepicker({
        format: DateFormatDatePickerJs,
    });
    $('#EndDateStr').datepicker({
        format: DateFormatDatePickerJs,
    });

    $('#BenefitCodeTokenField')
        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            $.each(existingTokens, function (index, token) {
                if (benefitCodeCount != 0) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                }
            });
        })
        .on('tokenfield:createdtoken', function (e) {
            var valid = BenefitCodes.includes(e.attrs.value)
            if (!valid) {
                $(e.relatedTarget).addClass('invalid');
            }
            benefitCodeCount += 1;
            $('#BenefitCodeTokenField-tokenfield').removeAttr('placeholder');
        })
        .on('tokenfield:edittoken', function (e) {
            var valid = BenefitCodes.includes(e.attrs.value)
            if (valid) {
                e.preventDefault();
            }
        })
        .on('tokenfield:removedtoken', function (e) {
            benefitCodeCount -= 1;
            if (benefitCodeCount == 0) {
                $("#BenefitCodeTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })
        .tokenfield({
            autocomplete: {
                source: BenefitCodes,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });

    $('#EntitlementTokenField')
        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            $.each(existingTokens, function (index, token) {
                if (entitlementCount != 0) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                }
            });
        })
        .on('tokenfield:createdtoken', function (e) {
            var valid = Entitlements.includes(e.attrs.value)
            if (!valid) {
                $(e.relatedTarget).addClass('invalid');
            }
            entitlementCount += 1;
            $('#EntitlementTokenField-tokenfield').removeAttr('placeholder');
        })
        .on('tokenfield:edittoken', function (e) {
            var valid = Entitlements.includes(e.attrs.value)
            if (valid) {
                e.preventDefault();
            }
        })
        .on('tokenfield:removedtoken', function (e) {
            entitlementCount -= 1;
            if (entitlementCount == 0) {
                $("#EntitlementTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })
        .tokenfield({
            autocomplete: {
                source: Entitlements,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });

    if (ReadOnly) {
        $('#BenefitCodeTokenField').tokenfield('disable');
        $('#EntitlementTokenField').tokenfield('disable');
    }
});

function focusOnDate(val) {
    $('#' + val).focus();
}

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
        ProfitCommission: Model,
        ItemList: ItemList,
        Products: Products ? Products : [],
        ProductMaxIndex: 0,
        ProfitCommissionDetailRows: [],
        TierProfitCommissionRows: [],
        DropDownDropDowns: DropDownDropDowns ? DropDownDropDowns : [],
        DropDownString: DropDownString,
        Disable: false,
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
        updateProfitCommissionDetail: function () {
            var json = JSON.stringify(this.ProfitCommissionDetailRows);
            $('#ProfitCommissionDetail').val(json);
        },
        updateTierProfitCommission: function () {
            var json = JSON.stringify(this.TierProfitCommissionRows);
            $('#TierProfitCommission').val(json);
        },
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.CreatedByName = AuthUserName;
            this.RemarkModal.ModuleId = this.ProfitCommission.ModuleId;
            this.RemarkModal.ObjectId = this.ProfitCommission.TreatyPricingCedantId;
            this.RemarkModal.SubModuleController = SubModuleController;
            this.RemarkModal.SubObjectId = this.ProfitCommission.Id;
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

            var parentId = this.ProfitCommission.TreatyPricingCedantId;
            var document = {
                ModuleId: this.ProfitCommission.ModuleId,
                ObjectId: parentId,
                SubModuleController: SubModuleController,
                SubObjectId: this.ProfitCommission.Id,
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
    },
    created: function () {
        if (this.Products)
            this.ProductMaxIndex = this.Products.length - 1;
    },
    updated() {

    }
});
