
$(document).ready(function () {
    dateOffAutoComplete();

    $('#InsuredDateOfBirth').datepicker({
        format: DateFormatDatePickerJs,
    });

    $('#OfferLetterSentDate').datepicker({
        format: DateFormatDatePickerJs,
    });

    $('#UwOpinion').autoResize();
    $('#Remark').autoResize();

    $('#PolicyNumberTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            $.each(existingTokens, function (index, token) {
                if (policyNumberCount != 0) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                }
            });
        })

        .on('tokenfield:createdtoken', function (e) {
            policyNumberCount += 1;
            $('#PolicyNumberTokenField-tokenfield').removeAttr('placeholder');
        })

        .on('tokenfield:removedtoken', function (e) {
            policyNumberCount -= 1;
            if (policyNumberCount == 0) {
                $("#PolicyNumberTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.policyNumber = true;
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

        .tokenfield();

    $('#BenefitCodeTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (benefitCodeCount != 0) {
                $.each(existingTokens, function (index, token) {
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

    $('#CedingBenefitTypeCodeTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (cedingBenefitTypeCodeCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })

        .on('tokenfield:createdtoken', function (e) {
            var valid = CedingBenefitTypeCodes.includes(e.attrs.value)
            if (!valid) {
                $(e.relatedTarget).addClass('invalid');
            }
            cedingBenefitTypeCodeCount += 1;
            $('#CedingBenefitTypeCodeTokenField-tokenfield').removeAttr('placeholder');
        })

        .on('tokenfield:edittoken', function (e) {
            var valid = CedingBenefitTypeCodes.includes(e.attrs.value)
            if (valid) {
                e.preventDefault();
            }
        })

        .on('tokenfield:removedtoken', function (e) {
            cedingBenefitTypeCodeCount -= 1;
            if (cedingBenefitTypeCodeCount == 0) {
                $("#CedingBenefitTypeCodeTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.cedingBenefitTypeCode = true;
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
                source: CedingBenefitTypeCodes,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });
});

function focusOnDate(val) {
    $('#' + val).focus();
}
