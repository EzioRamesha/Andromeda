function initTokenfield(index, isAdd = false) {
    var cedingPlanCodeCount = 0;
    var benefitCodeCount = 0;

    $(document).ready(function () {
        $('#cedingPlanCode' + index + 'TokenField').on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (cedingPlanCodeCount != 0) {
                $.each(existingTokens, function (el, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })
        .on('tokenfield:createdtoken', function (e) {
            cedingPlanCodeCount += 1;
            $("#cedingPlanCode" + index + "TokenField-tokenfield").removeAttr('placeholder');
            if (isAdd) {
                app.TreatyDiscountTableDetails[index].CedingPlanCode = $(this).tokenfield('getTokens').map(e => e.value).join(",");
            }
        })
        .on('tokenfield:removedtoken', function (e) {
            cedingPlanCodeCount -= 1;
            if (cedingPlanCodeCount == 0) {
                $("#cedingPlanCode" + index + "TokenField-tokenfield").attr("placeholder", "Type here");
            }
            if (isAdd) {
                app.TreatyDiscountTableDetails[index].CedingPlanCode = $(this).tokenfield('getTokens').map(e => e.value).join(",");
            }
        })
            .tokenfield();

        $('#benefitCode' + index + 'TokenField')
            .on('tokenfield:createtoken', function (e) {
                var existingTokens = $(this).tokenfield('getTokens');
                if (benefitCodeCount != 0) {
                    $.each(existingTokens, function (el, token) {
                        if (token.value === e.attrs.value) {
                            e.preventDefault();
                        }
                    });
                }
            })
            .on('tokenfield:createdtoken', function (e) {
                var valid = BenefitCodes.includes(e.attrs.value)
                if (!valid) {
                    $(e.relatedTarget).addClass('invalid');
                }
                benefitCodeCount += 1;
                $("#benefitCode" + index + "TokenField-tokenfield").removeAttr('placeholder');
                if (isAdd) {
                    app.PremiumSpreadTableDetails[index].BenefitCode = $(this).tokenfield('getTokens').map(e => e.value).join(",");
                }
            })
            .on('tokenfield:edittoken', function (e) {
                var valid = BenefitCodes.includes(e.attrs.value)
                if (!valid) {
                    e.preventDefault();
                }
            })
            .on('tokenfield:removedtoken', function (e) {
                benefitCodeCount -= 1;
                if (benefitCodeCount == 0) {
                    $("#benefitCode" + index + "TokenField-tokenfield").attr("placeholder", "Type here");
                }
                if (isAdd) {
                    app.PremiumSpreadTableDetails[index].BenefitCode = $(this).tokenfield('getTokens').map(e => e.value).join(",");
                }
            })
            .tokenfield({
                autocomplete: {
                    source: BenefitCodes,
                    delay: 100
                },
                showAutocompleteOnFocus: true
            });

    });
}

var app = new Vue({
    el: '#app',
    data: {
        TreatyDiscountTable: TreatyDiscountTableModel,
        TreatyDiscountTableDetails: TreatyDiscountTableDetails ? TreatyDiscountTableDetails : [],
        Benefits: Benefits,
        TreatyDiscountTableDetailMaxIndex: 0,
        Type: TreatyDiscountTableModel.Type,
    },
    methods: {
        addNew: function () {
            this.TreatyDiscountTableDetails.push({ CedingPlanCode: "", BenefitCode: "", AgeFromStr: "", AgeToStr: "", DiscountStr: "" });
            this.TreatyDiscountTableDetailMaxIndex += 1;
            initTokenfield(this.TreatyDiscountTableDetailMaxIndex, true);
        },
        removeTreatyDiscountTableDetail: function (index) {
            var planCodeArr = [];
            var benefitCodeArr = [];
            this.TreatyDiscountTableDetails.forEach(
                function (treatyDiscountTableDetail, detailIndex) {
                    planCodeArr.push($('#cedingPlanCode' + detailIndex + 'TokenField').tokenfield('getTokens'));
                    benefitCodeArr.push($('#benefitCode' + detailIndex + 'TokenField').tokenfield('getTokens'));
                }
            );

            this.TreatyDiscountTableDetails.splice(index, 1);
            this.TreatyDiscountTableDetailMaxIndex -= 1;

            var details = this.TreatyDiscountTableDetails;
            var i = 0;
            planCodeArr.forEach(
                function (a, arrIndex) {
                    if (arrIndex != index) {
                        $('#cedingPlanCode' + i + 'TokenField').tokenfield('setTokens', a);
                        details[i].CedingPlanCode = a.map(e => e.value).join(",");
                        i++;
                    }
                }
            );
            var i = 0;
            benefitCodeArr.forEach(
                function (a, arrIndex) {
                    if (arrIndex != index) {
                        $('#benefitCode' + i + 'TokenField').tokenfield('setTokens', a);
                        details[i].BenefitCode = a.map(e => e.value).join(",");
                        i++;
                    }
                }
            );
            this.TreatyDiscountTableDetails = details;
        },
    },
    created: function () {
        var i = 0;
        if (this.TreatyDiscountTableDetails) {
            this.TreatyDiscountTableDetailMaxIndex = this.TreatyDiscountTableDetails.length - 1;
            this.TreatyDiscountTableDetails.forEach(function (treatyDiscountTableDetail) {
                if (treatyDiscountTableDetail != null) {
                    initTokenfield(i);
                    i++;
                }
            });
        } else {
            this.TreatyDiscountTableDetailMaxIndex = -1;
        }
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});
