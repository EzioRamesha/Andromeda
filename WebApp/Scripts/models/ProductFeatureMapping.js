
$(document).ready(function () {
    dateOffAutoComplete();
    getTreatyCode(model.CedantId, model.TreatyCodeId);

    $('#ReinsEffDatePolStartDate').datepicker({
        format: DateFormatDatePickerJs,
    });

    $('#ReinsEffDatePolEndDate').datepicker({
        format: DateFormatDatePickerJs,
    });

    $('#ReportingStartDate').datepicker({
        format: DateFormatDatePickerJs,
    });

    $('#ReportingEndDate').datepicker({
        format: DateFormatDatePickerJs,
    });

    $('#EffectiveDate').datepicker({
        format: DateFormatDatePickerJs,
    });

    $('#CedingPlanCodeTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (cedingPlanCodeCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })

        .on('tokenfield:createdtoken', function (e) {
            cedingPlanCodeCount += 1;
            $("#CedingPlanCodeTokenField-tokenfield").removeAttr('placeholder');
        })

        .on('tokenfield:removedtoken', function (e) {
            cedingPlanCodeCount -= 1;
            if (cedingPlanCodeCount == 0) {
                $("#CedingPlanCodeTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.cedingPlanCode = true;
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

    $('#CedingBenefitRiskCodeTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (cedingBenefitRiskCodeCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })

        .on('tokenfield:createdtoken', function (e) {
            cedingBenefitRiskCodeCount += 1;
            $('#CedingBenefitRiskCodeTokenField-tokenfield').removeAttr('placeholder');
        })

        .on('tokenfield:removedtoken', function (e) {
            cedingBenefitRiskCodeCount -= 1;
            if (cedingBenefitRiskCodeCount == 0) {
                $("#CedingBenefitRiskCodeTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.cedingBenefitRiskCode = true;
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

    $('#CedingTreatyCodeTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (cedingTreatyCodeCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })

        .on('tokenfield:createdtoken', function (e) {
            cedingTreatyCodeCount += 1;
            $('#CedingTreatyCodeTokenField-tokenfield').removeAttr('placeholder');
        })

        .on('tokenfield:removedtoken', function (e) {
            cedingTreatyCodeCount -= 1;
            if (cedingTreatyCodeCount == 0) {
                $("#CedingTreatyCodeTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.cedingTreatyCode = true;
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

    $('#CampaignCodeTokenField')
        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (campaignCodeCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })

        .on('tokenfield:createdtoken', function (e) {
            campaignCodeCount += 1;
            $('#CampaignCodeTokenField-tokenfield').removeAttr('placeholder');
        })

        .on('tokenfield:removedtoken', function (e) {
            campaignCodeCount -= 1;
            if (campaignCodeCount == 0) {
                $("#CampaignCodeTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.campaignCode = true;
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
});

function getTreatyCode(selectedCedantId, selectedTreatyCodeId = null) {
    $.ajax({
        url: getTreatyCodeByCedantUrl,
        type: "POST",
        data: {
            CedantId: selectedCedantId,
            Status: treatyCodeActive,
            SelectedId: selectedTreatyCodeId,
        },
        cache: false,
        async: false,
        success: function (data) {
            var TreatyCodes = data.TreatyCodes;
            refreshDropDownItems('TreatyCodeId', TreatyCodes, selectedTreatyCodeId, 'Code', 'Description')
        }
    });
}

function focusOnDate(val) {
    $('#' + val).focus();
}
