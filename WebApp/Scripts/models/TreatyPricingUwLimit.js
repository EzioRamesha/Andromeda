var benefitCodeCount = 0;

var tokenfieldReady = {
    benefitCode: false,
};

var loadingDiv = $("#loadingSpinner");

$(document).ready(function () {
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
        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.benefitCode = true;
            var isReady = true;
            Object.keys(tokenfieldReady).forEach(function (key) {
                if (tokenfieldReady[key] == false) {
                    isReady = false;
                }
            });
            if (isReady && loadingDiv != null) {
                loadingDiv.addClass('hide-loading-spinner');
            }
        })
        .tokenfield({
            autocomplete: {
                source: BenefitCodes,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });

    $('#EffectiveAt').datepicker({
        format: DateFormatDatePickerJs,
        autoclose: true
    }).on('changeDate', function () {
        Model.EffectiveAtStr = $('#EffectiveAt').val();
    });
});

function focusOnDate(val) {
    $('#' + val).focus();
}

var app = new Vue({
    el: '#app',
    data: {
        UwLimit: Model,
        UwLimitVersion: Model.CurrentVersionObject,        
        VersionItems: [],
        VersionDetails: VersionDetails ? VersionDetails : [],

        AuthUserId: AuthUserId,
        AuthUserName: AuthUserName,
    },
    methods: {
        getVersionList() {
            var limitId = this.UwLimit.Id;
            this.VersionItems = [];

            var selectListItems = [];
            $.ajax({
                url: GetVersionItemUrl,
                type: "POST",
                data: {
                    treatyPricingUwLimitId: limitId
                },
                cache: false,
                async: false,
                success: function (data) {
                    selectListItems = data.items;
                }
            });
            return this.VersionItems = selectListItems;
        },
        //get
        resetVersionDetail() {
            this.getVersionList();

            this.UwLimit.VersionId = this.UwLimitVersion.Id;
            this.UwLimit.Version = this.UwLimitVersion.Version;
            this.UwLimit.PersonInChargeId = this.UwLimitVersion.CreatedById;
            this.UwLimit.PersonInChargeName = this.UwLimitVersion.CreatedByName;
            this.UwLimit.EffectiveAtStr = "";
            this.UwLimit.CurrencyCode = "";
            this.UwLimit.UwLimit = "";
            this.UwLimit.Remarks1 = "";
            this.UwLimit.AblSumAssured = "";
            this.UwLimit.Remarks2 = "";
            this.UwLimit.AblMaxUwRating = "";
            this.UwLimit.Remarks3 = "";
            this.UwLimit.MaxSumAssured = "";
            this.UwLimit.PerLifeIndustry = "";
            this.UwLimit.IssuePayoutLimit = "";
            this.UwLimit.Remarks4 = "";
        },
        createVersion() {
            var limitId = this.UwLimit.Id;

            var versionData = null;
            var versionDataList = [];
            $.ajax({
                url: CreateVersionUrl,
                type: "POST",
                data: {
                    treatyPricingUwLimitId: limitId
                },
                cache: false,
                async: false,
                success: function (data) {
                    versionData = data.limitVersionBo;
                    versionDataList = data.limitVersionBos;
                }
            });
            
            this.UwLimitVersion = versionData;
            this.VersionDetails = versionDataList;
            this.resetVersionDetail();
        },
        getVersionDetail() {
            var VersionId = this.UwLimit.VersionId;
            
            if (VersionId != null || VersionId != '') {
                var versionData;
                this.VersionDetails.forEach(function (obj) {
                    if (obj.Id == VersionId)
                        versionData = obj;
                });
                
                this.UwLimit.VersionId = versionData.Id;
                this.UwLimit.Version = versionData.Version;
                this.UwLimit.PersonInChargeId = versionData.PersonInChargeId;
                this.UwLimit.PersonInChargeName = versionData.PersonInChargeName;
                this.UwLimit.EffectiveAtStr = versionData.EffectiveAtStr;
                this.UwLimit.CurrencyCode = versionData.CurrencyCode;
                this.UwLimit.UwLimit = versionData.UwLimit;
                this.UwLimit.Remarks1 = versionData.Remarks1;
                this.UwLimit.AblSumAssured = versionData.AblSumAssured;
                this.UwLimit.Remarks2 = versionData.Remarks2;
                this.UwLimit.AblMaxUwRating = versionData.AblMaxUwRating;
                this.UwLimit.Remarks3 = versionData.Remarks3;
                this.UwLimit.MaxSumAssured = versionData.MaxSumAssured;
                this.UwLimit.PerLifeIndustry = versionData.PerLifeIndustry;
                this.UwLimit.IssuePayoutLimit = versionData.IssuePayoutLimit;
                this.UwLimit.Remarks4 = versionData.Remarks4;
            }
            else {
                this.UwLimit.VersionId = "";
                this.UwLimit.Version = "";
                this.UwLimit.PersonInChargeId = AuthUserId;
                this.UwLimit.PersonInChargeName = AuthUserName;
                this.UwLimit.EffectiveAtStr = "";
                this.UwLimit.CurrencyCode = "";
                this.UwLimit.UwLimit = "";
                this.UwLimit.Remarks1 = "";
                this.UwLimit.AblSumAssured = "";
                this.UwLimit.Remarks2 = "";
                this.UwLimit.AblMaxUwRating = "";
                this.UwLimit.Remarks3 = "";
                this.UwLimit.MaxSumAssured = "";
                this.UwLimit.PerLifeIndustry = "";
                this.UwLimit.IssuePayoutLimit = "";
                this.UwLimit.Remarks4 = "";
            }
        },
    },
    created: function () {

        if (this.UwLimit.VersionId == 0)
            this.UwLimit.VersionId = "";
    },
    updated() {
        //$(this.$refs.select).selectpicker('refresh');
        this.$nextTick(function () { $('.selectpicker').selectpicker('refresh'); });
    }
});
