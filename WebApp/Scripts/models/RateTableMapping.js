
$(document).ready(function () {
    dateOffAutoComplete();

    getRelatedDropDown(cedantId);

    $('#ReinsEffDatePolStartDate').datepicker({
        format: DateFormatDatePickerJs,
        autoclose: true
    });
    $('#ReinsEffDatePolEndDate').datepicker({
        format: DateFormatDatePickerJs,
        autoclose: true
    });
    $('#ReportingStartDate').datepicker({
        format: DateFormatDatePickerJs,
        autoclose: true
    });
    $('#ReportingEndDate').datepicker({
        format: DateFormatDatePickerJs,
        autoclose: true
    });

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
            if (isReady && loadingDiv != null) {
                loadingDiv.addClass('hide-loading-spinner');
            }
        })

        .tokenfield({
            autocomplete: {
                source: TreatyCodes,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });

    $('#CedingTreatyCodeTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            $.each(existingTokens, function (index, token) {
                if (cedingTreatyCodeCount != 0) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                }
            });
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

    $('#CedingPlanCodeTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            $.each(existingTokens, function (index, token) {
                if (cedingPlanCodeCount != 0) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                }
            });
        })

        .on('tokenfield:createdtoken', function (e) {
            cedingPlanCodeCount += 1;
            $('#CedingPlanCodeTokenField-tokenfield').removeAttr('placeholder');
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

    $('#CedingPlanCode2TokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            $.each(existingTokens, function (index, token) {
                if (cedingPlanCode2Count != 0) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                }
            });
        })

        .on('tokenfield:createdtoken', function (e) {
            cedingPlanCode2Count += 1;
            $('#CedingPlanCode2TokenField-tokenfield').removeAttr('placeholder');
        })

        .on('tokenfield:removedtoken', function (e) {
            cedingPlanCode2Count -= 1;
            if (cedingPlanCode2Count == 0) {
                $("#CedingPlanCode2TokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.cedingPlanCode2 = true;
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
            $.each(existingTokens, function (index, token) {
                if (cedingBenefitRiskCodeCount != 0) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                }
            });
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

    $('#GroupPolicyNumberTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            $.each(existingTokens, function (index, token) {
                if (groupPolicyNumberCount != 0) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                }
            });
        })

        .on('tokenfield:createdtoken', function (e) {
            groupPolicyNumberCount += 1;
            $('#GroupPolicyNumberTokenField-tokenfield').removeAttr('placeholder');
        })

        .on('tokenfield:removedtoken', function (e) {
            groupPolicyNumberCount -= 1;
            if (groupPolicyNumberCount == 0) {
                $("#GroupPolicyNumberTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.groupPolicyNumber = true;
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

function focusOnDate(val) {
    $('#' + val).focus();
}

function getRelatedDropDown(selectedCedantId) {
    getRiDiscount(selectedCedantId);
    getLargeDiscount(selectedCedantId);
    getGroupDiscount(selectedCedantId);
    getTreatyCodeByCedant(selectedCedantId);
}

function getTreatyCodeByCedant(selectedCedantId) {
    $.ajax({
        url: getTreatyCodeByCedantUrl,
        type: "POST",
        data: {
            CedantId: selectedCedantId,
        },
        cache: false,
        async: false,
        success: function (data) {
            TreatyCodes = data.TreatyCodes;
            if (TreatyCodes != null && TreatyCodes.length > 0 && typeof $('#TreatyCodeTokenField').data('bs.tokenfield') != "undefined") {
                $('#TreatyCodeTokenField').tokenfield('setTokens', []);
                $('#TreatyCodeTokenField').data('bs.tokenfield').$input.autocomplete({ source: TreatyCodes });
            }
        }
    });
}


function getRiDiscount(selectedCedantId) {
    $.ajax({
        url: getRiDiscountByCedantUrl,
        type: "POST",
        data: {
            CedantId: selectedCedantId,
            isDistinctCode: true
        },
        cache: false,
        async: false,
        success: function (data) {
            var RiDiscounts = data.RiDiscounts;
            console.log(RiDiscounts);
            refreshDropDownItems('RiDiscountCode', RiDiscounts, selectedRiDiscount, 'DiscountCode', null, true, 'DiscountCode')
        }
    });
}

function getLargeDiscount(selectedCedantId) {
    $.ajax({
        url: getLargeDiscountByCedantUrl,
        type: "POST",
        data: {
            CedantId: selectedCedantId,
            isDistinctCode: true
        },
        cache: false,
        async: false,
        success: function (data) {
            var LargeDiscounts = data.LargeDiscounts;
            refreshDropDownItems('LargeDiscountCode', LargeDiscounts, selectedLargeDiscount, 'DiscountCode', null, true, 'DiscountCode')
        }
    });
}

function getGroupDiscount(selectedCedantId) {
    $.ajax({
        url: getGroupDiscountByCedantUrl,
        type: "POST",
        data: {
            CedantId: selectedCedantId,
            isDistinctCode: true
        },
        cache: false,
        async: false,
        success: function (data) {
            var GroupDiscounts = data.GroupDiscounts;
            refreshDropDownItems('GroupDiscountCode', GroupDiscounts, selectedGroupDiscount, 'DiscountCode', null, true, 'DiscountCode')
        }
    });
}
