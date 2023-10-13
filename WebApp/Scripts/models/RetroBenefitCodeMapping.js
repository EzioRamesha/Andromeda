$(document).ready(function () {
    $('#TreatyCodeTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (treatyCodeCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })

        .on('tokenfield:createdtoken', function (e) {
            var valid = TreatyCodes.includes(e.attrs.value)
            if (!valid) {
                $(e.relatedTarget).addClass('invalid');
            }
            treatyCodeCount += 1;
            $('#TreatyCodeTokenField-tokenfield').removeAttr('placeholder');
        })

        .on('tokenfield:edittoken', function (e) {
            var valid = TreatyCodes.includes(e.attrs.value)
            if (valid) {
                e.preventDefault();
            }
        })

        .on('tokenfield:removedtoken', function (e) {
            treatyCodeCount -= 1;
            if (treatyCodeCount == 0) {
                $("#TreatyCodeTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.treatyCode = true;
            var isReady = true;
            Object.keys(tokenfieldReady).forEach(function (key) {
                if (tokenfieldReady[key] == false) {
                    isReady = false;
                }
            });
            if (isReady && $("#loadingSpinner") != null) {
                $("#loadingSpinner").addClass('hide-loading-spinner');
            }
        })

        .tokenfield({
            autocomplete: {
                source: TreatyCodes,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });

    changePerAnnum();

    $('#IsPerAnnum').change(function () {
        changePerAnnum();
    });
});

function changePerAnnum() {
    var isChecked = $('#IsPerAnnum').prop("checked");
    if (isChecked) {
        $('#TreatyCodeDiv').show();
    } else {
        $('#TreatyCodeDiv').hide();
    }
}

var app = new Vue({
    el: '#app',
    data: {
        RetroBenefitCodeMapping: RetroBenefitCodeMappingModel,
        RetroBenefitCodeMappingDetails: RetroBenefitCodeMappingDetails ? RetroBenefitCodeMappingDetails : [],
        DropDownYesNo: DropDownYesNo,
        DropDownRetroBenefitCodes: DropDownRetroBenefitCodes,
        RetroBenefitCodeMappingDetailMaxIndex: 0,
    },
    methods: {
        addNew: function () {
            this.RetroBenefitCodeMappingDetails.push({ RetroBenefitCodeId: "", IsComputePremium: false });
            this.RetroBenefitCodeMappingDetailMaxIndex += 1;
        },
        removeRetroBenefitCodeMappingDetail: function (index) {
            this.RetroBenefitCodeMappingDetails.splice(index, 1);
            this.RetroBenefitCodeMappingDetailMaxIndex -= 1;
        },
    },
    created: function () {
        this.RetroBenefitCodeMappingDetailMaxIndex = (this.RetroBenefitCodeMappingDetails) ? this.RetroBenefitCodeMappingDetails.length - 1 : -1;
    },
    updated() {
        //$(this.$refs.select).selectpicker('refresh');
    }
});
