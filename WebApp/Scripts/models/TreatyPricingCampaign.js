var CampaignTypeCount = 0;
var UnderwritingMethodCount = 0;
var DistributionChannelCount = 0;

var tokenfieldReady = {
    underwritingMethod: false,
    distributionChannel: false,
    campaignType: false,
};

$(document).ready(function () {

    // Version
    initializeTokenField('#Type', CampaignTypeCodes, 'CampaignTypeCount');
    //initializeTokenField('#UnderwritingMethod', UnderwritingMethodCodes, 'UnderwritingMethodCount');
    //initializeTokenField('#DistributionChannel', DistributionChannelCodes, 'DistributionChannelCount');

    $('#EffectiveAtStr').datepicker({
        format: DateFormatDatePickerJs,
    });
    
    $('#campaignTypeTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (CampaignTypeCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })

        .on('tokenfield:createdtoken', function (e) {
            var valid = CampaignTypes.includes(e.attrs.value)
            if (!valid) {
                $(e.relatedTarget).addClass('invalid');
            }
            CampaignTypeCount += 1;
            $('#campaignTypeTokenField-tokenfield').removeAttr('placeholder');
        })

        .on('tokenfield:edittoken', function (e) {
            var valid = CampaignTypes.includes(e.attrs.value)
            if (valid) {
                e.preventDefault();
            }
        })

        .on('tokenfield:removedtoken', function (e) {
            CampaignTypeCount -= 1;
            if (CampaignTypeCount == 0) {
                $("#campaignTypeTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.campaignType = true;
            var isReady = true;
            Object.keys(tokenfieldReady).forEach(function (key) {
                if (tokenfieldReady[key] == false) {
                    isReady = false;
                }
            });
        })

        .tokenfield({
            autocomplete: {
                source: CampaignTypes,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });

    if (ReadOnly) {
        $('#campaignTypeTokenField').tokenfield('disable');
    }
});

var app = new Vue({
    el: '#app',
    data: {
        Model: Model,
        ModelVersion: Model.CurrentVersionObject,
        DropDownCedants: DropDownCedants,
        DropDownProductTypes: DropDownProductTypes,
        DropDownProducts: DropDownProducts,
        DropDownProductQuotations: DropDownProductQuotations,
        DropDownAgeBasis: DropDownAgeBasis,
        // Product
        CampaignProducts: CampaignProducts,
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
        // Version
        changeIsEnabled: function (checked, id) {
            $('#' + id).prop('disabled', checked);
        },
        changeIsEnabledDrpDwn: function (checked, id) {            
            $('#' + id).prop('disabled', checked);
            $('#' + id).selectpicker('refresh');
        },
        changeIsEnabledToken: function (checked, id) {
            if (checked)
                $('#' + id).tokenfield('disable');
            else
                $('#' + id).tokenfield('enable');
        }
    },
    created: function () {
        this.CampaignProductsMaxIndex = (this.CampaignProducts) ? this.CampaignProducts.length : 0;
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});

var setVersionCallBack = function (bo) {
    initializeTokenField('#UnderwritingMethod', UnderwritingMethodCodes, 'UnderwritingMethodCount', 'UnderwritingMethod_ModelVersion');
    initializeTokenField('#DistributionChannel', DistributionChannelCodes, 'DistributionChannelCount', 'DistributionChannel_ModelVersion');

    setEnableDisable(bo);
}

function focusOnDate(val) {
    $('#' + val).focus();
}

function initializeTokenField(id, options, countName, currentModel = '') {
    var tokenFieldId = id + '-tokenfield';
    $(id).tokenfield('destroy');
    $(id)
        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            $.each(existingTokens, function (index, token) {
                if (window[countName] != 0) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                }
            });
        })
        .on('tokenfield:createdtoken', function (e) {
            if (!options.includes(e.attrs.value)) {
                $(e.relatedTarget).addClass('invalid');
            }
            window[countName] += 1;
            $(tokenFieldId).removeAttr('placeholder');

            var idDetails = currentModel.split("_");
            if (idDetails.length == 2) {
                var fieldName = idDetails[0];
                var type = idDetails[1];
                if ($(this).tokenfield('getTokens').map(e => e.value).length != 0)
                    app[type][fieldName] = $(this).tokenfield('getTokens').map(e => e.value).join(",");
            }
        })
        .on('tokenfield:edittoken', function (e) {
            if (options.includes(e.attrs.value)) {
                e.preventDefault();
            }
        })
        .on('tokenfield:removedtoken', function (e) {
            window[countName] -= 1;
            if (window[countName] == 0) {
                $(tokenFieldId).attr("placeholder", "Type here");
            }

            var idDetails = currentModel.split('_');
            if (idDetails.length == 2) {
                var fieldName = idDetails[0];
                var type = idDetails[1];
                app[type][fieldName] = $(this).tokenfield('getTokens').map(e => e.value).join(",");
            }
        })
        .tokenfield({
            autocomplete: {
                source: options,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });
}

function setEnableDisable(bo) {
    app.changeIsEnabledDrpDwn(bo.IsPerAgeBasis, 'AgeBasisPickListDetailId');
    app.changeIsEnabledDrpDwn(bo.IsPerReinsuranceRate, 'ReinsRateTreatyPricingRateTableSelect');
    app.changeIsEnabledDrpDwn(bo.IsPerReinsuranceDiscount, 'ReinsDiscountTreatyPricingRateTableSelect');
    app.changeIsEnabledDrpDwn(bo.IsPerProfitComm, 'TreatyPricingProfitCommissionSelect');
    app.changeIsEnabledDrpDwn(bo.IsPerUnderwritingQuestion, 'TreatyPricingUwQuestionnaireSelect');
    app.changeIsEnabledDrpDwn(bo.IsPerMedicalTable, 'TreatyPricingMedicalTableSelect');
    app.changeIsEnabledDrpDwn(bo.IsPerFinancialTable, 'TreatyPricingFinancialTableSelect');
    app.changeIsEnabledDrpDwn(bo.IsPerAdvantageProgram, 'TreatyPricingAdvantageProgramSelect');

    app.changeIsEnabledToken(bo.IsPerDistributionChannel, 'DistributionChannel');
    app.changeIsEnabledToken(bo.IsPerUnderwritingMethod, 'UnderwritingMethod');
}