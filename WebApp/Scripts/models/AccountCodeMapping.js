$(document).ready(function () {

    $('#TreatyTypeTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (treatyTypeCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })

        .on('tokenfield:createdtoken', function (e) {
            var valid = TreatyTypes.includes(e.attrs.value)
            if (!valid) {
                $(e.relatedTarget).addClass('invalid');
            }
            treatyTypeCount += 1;
            $('#TreatyTypeTokenField-tokenfield').removeAttr('placeholder');
        })

        .on('tokenfield:edittoken', function (e) {
            var valid = TreatyTypes.includes(e.attrs.value)
            if (valid) {
                e.preventDefault();
            }
        })

        .on('tokenfield:removedtoken', function (e) {
            treatyTypeCount -= 1;
            if (treatyTypeCount == 0) {
                $("#TreatyTypeTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.treatyType = true;
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
                source: TreatyTypes,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });

    $('#ClaimCodeTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (claimCodeCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })

        .on('tokenfield:createdtoken', function (e) {
            var valid = ClaimCodes.includes(e.attrs.value)
            if (!valid) {
                $(e.relatedTarget).addClass('invalid');
            }
            claimCodeCount += 1;
            $('#ClaimCodeTokenField-tokenfield').removeAttr('placeholder');
        })

        .on('tokenfield:edittoken', function (e) {
            var valid = ClaimCodes.includes(e.attrs.value)
            if (valid) {
                e.preventDefault();
            }
        })

        .on('tokenfield:removedtoken', function (e) {
            claimCodeCount -= 1;
            if (claimCodeCount == 0) {
                $("#ClaimCodeTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.claimCode = true;
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
                source: ClaimCodes,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });

    $('#BusinessOriginTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (businessOriginCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })

        .on('tokenfield:createdtoken', function (e) {
            var valid = BusinessOrigins.includes(e.attrs.value)
            if (!valid) {
                $(e.relatedTarget).addClass('invalid');
            }
            businessOriginCount += 1;
            $('#BusinessOriginTokenField-tokenfield').removeAttr('placeholder');
        })

        .on('tokenfield:edittoken', function (e) {
            var valid = BusinessOrigins.includes(e.attrs.value)
            if (valid) {
                e.preventDefault();
            }
        })

        .on('tokenfield:removedtoken', function (e) {
            businessOriginCount -= 1;
            if (businessOriginCount == 0) {
                $("#BusinessOriginTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.businessOrigin = true;
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
                source: BusinessOrigins,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });

    $('#InvoiceFieldTokenField')

        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (invoiceFieldCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })

        .on('tokenfield:createdtoken', function (e) {
            var valid = InvoiceFields.includes(e.attrs.value)
            if (!valid) {
                $(e.relatedTarget).addClass('invalid');
            }
            invoiceFieldCount += 1;
            $('#InvoiceFieldTokenField-tokenfield').removeAttr('placeholder');
        })

        .on('tokenfield:edittoken', function (e) {
            var valid = InvoiceFields.includes(e.attrs.value)
            if (valid) {
                e.preventDefault();
            }
        })

        .on('tokenfield:removedtoken', function (e) {
            invoiceFieldCount -= 1;
            if (invoiceFieldCount == 0) {
                $("#InvoiceFieldTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })

        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.invoiceField = true;
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
                source: InvoiceFields,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });
});
