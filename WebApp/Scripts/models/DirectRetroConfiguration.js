
var app = new Vue({
    el: '#app',
    data: {
        DirectRetroConfiguration: DirectRetroConfigurationModel,
        DirectRetroConfigurationDetails: DirectRetroConfigurationDetails ? DirectRetroConfigurationDetails : [],
        DirectRetroConfigurationDetailMaxIndex: 0,
    },
    methods: {
        addNew: function () {
            this.DirectRetroConfigurationDetails.push({
                RiskPeriodStartDateStr: "",
                RiskPeriodEndDateStr: "",
                IssueDatePolStartDateStr: "",
                IssueDatePolEndDateStr: "",
                ReinsEffDatePolStartDateStr: "",
                ReinsEffDatePolEndDateStr: "",
                IsDefault: false,
                RetroPartyId: "",
                TreatyNo: "",
                Schedule: "",
                ShareStr: "",
                PremiumSpreadTableId: "",
                TreatyDiscountTableId: "",
            });
            this.DirectRetroConfigurationDetailMaxIndex += 1;
        },
        removeDirectRetroConfigurationDetail: function (index) {
            this.DirectRetroConfigurationDetails.splice(index, 1);
            this.DirectRetroConfigurationDetailMaxIndex -= 1;
        },
        openDatePicker: function (currentId) {
            var idStr = currentId.split("_");
            var field = idStr[0];
            var index = idStr[1];

            var id = "#" + currentId;
            if ($(id).data("datepicker") != null) {
                $(id).datepicker("destroy");
            }

            $(id).datepicker({
                format: DateFormatDatePickerJs,
                autoclose: true,
            });

            var updateDateValue = this.updateDateValue;
            $(id).on('changeDate', function () {
                updateDateValue(field, index, $(id).val());
            });

            $(id).focus();
        },
        updateDateValue: function (field, index, value) {
            this.DirectRetroConfigurationDetails[index][field] = value;
        },
    },
    created: function () {
        this.DirectRetroConfigurationDetailMaxIndex = (this.DirectRetroConfigurationDetails) ? this.DirectRetroConfigurationDetails.length - 1 : -1;
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    },
});

$(document).ready(function () {
    $('#RetroPartyTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (retroPartyCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })

        .on('tokenfield:createdtoken', function (e) {
            var valid = RetroPartyParties.includes(e.attrs.value)
            if (!valid) {
                $(e.relatedTarget).addClass('invalid');
            }
            retroPartyCount += 1;
            $('#RetroPartyTokenField-tokenfield').removeAttr('placeholder');
        })

        .on('tokenfield:edittoken', function (e) {
            var valid = RetroPartyParties.includes(e.attrs.value)
            if (valid) {
                e.preventDefault();
            }
        })

        .on('tokenfield:removedtoken', function (e) {
            retroPartyCount -= 1;
            if (retroPartyCount == 0) {
                $("#RetroPartyTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.retroParty = true;
            var isReady = true;
            Object.keys(tokenfieldReady).forEach(function (key) {
                if (tokenfieldReady[key] == false) {
                    isReady = false;
                }
            });
            if (isReady && loadingDiv != null) {
                $('#loadingSpinner').addClass('hide-loading-spinner');
            }
        })

        .tokenfield({
            autocomplete: {
                source: RetroPartyParties,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });
});

function focusOnDate(val) {
    $('#' + val).focus();
}
