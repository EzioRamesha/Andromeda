
var app = new Vue({
    el: '#app',
    data: {
        AnnuityFactor: AnnuityFactorModel,
        AnnuityFactorDetails: AnnuityFactorDetails ? AnnuityFactorDetails : [],
        InsuredGenderCodes: InsuredGenderCodes,
        InsuredTobaccoUses: InsuredTobaccoUses,
        AnnuityFactorDetailMaxIndex: 0,
    },
    methods: {
        addNew: function () {
            this.AnnuityFactorDetails.push({ PolicyTermRemainStr: "", InsuredGenderCodePickListDetailId: "", InsuredTobaccoUsePickListDetailId: "", InsuredAttainedAge: "", PolicyTermStr: "", AnnuityFactorValueStr: "" });
            this.AnnuityFactorDetailMaxIndex += 1;
        },
        removeAnnuityFactorDetail: function (index) {
            this.AnnuityFactorDetails.splice(index, 1);
            this.AnnuityFactorDetailMaxIndex -= 1;
        },
    },
    created: function () {
        this.AnnuityFactorDetailMaxIndex = (this.AnnuityFactorDetails) ? this.AnnuityFactorDetails.length - 1 : -1;
    },
});

$(document).ready(function () {
    dateOffAutoComplete();

    $(".dropdown-item").click(function () {
        $("#btnDownload").dropdown("toggle");
    });

    $('#ReinsEffDatePolStartDate').datepicker({
        format: DateFormatDatePickerJs,
    });

    $('#ReinsEffDatePolEndDate').datepicker({
        format: DateFormatDatePickerJs,
    });

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
                $('#loadingSpinner').addClass('hide-loading-spinner');
            }
        })

        .tokenfield();
});

function focusOnDate(val) {
    $('#' + val).focus();
}

function uploadFile(form) {
    form.action = UploadFileUrl;
    form.submit();
}
