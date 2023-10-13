var cedingEventCodeCount = 0;
var cedingClaimTypeCount = 0;
var tokenfieldReady = {
    cedingEventCode: false,
    cedingClaimType: false,
};

var loadingDiv = $("#loadingSpinner");

$(document).ready(function () {

    $('#CedingEventCodeTokenField')
        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (cedingEventCodeCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })
        .on('tokenfield:createdtoken', function (e) {
            cedingEventCodeCount += 1;
            $("#CedingEventCodeTokenField-tokenfield").removeAttr('placeholder');
        })
        .on('tokenfield:removedtoken', function (e) {
            cedingEventCodeCount -= 1;
            if (cedingEventCodeCount == 0) {
                $("#CedingEventCodeTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })
        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.cedingEventCode = true;
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

    $('#CedingClaimTypeTokenField')
        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (cedingClaimTypeCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })
        .on('tokenfield:createdtoken', function (e) {
            cedingClaimTypeCount += 1;
            $("#CedingClaimTypeTokenField-tokenfield").removeAttr('placeholder');
        })
        .on('tokenfield:removedtoken', function (e) {
            cedingClaimTypeCount -= 1;
            if (cedingClaimTypeCount == 0) {
                $("#CedingClaimTypeTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })
        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.cedingClaimType = true;
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