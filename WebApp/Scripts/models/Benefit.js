
var app = new Vue({
    el: '#app',
    data: {
        Benefit: BenefitModel,
        BenefitDetails: BenefitDetails,
        MLReEventCodes: MLReEventCodes,
        ClaimCodes: ClaimCodes,
        SortIndex: 0,
    },
    methods: {
        addNew: function () {
            this.SortIndex++;
            this.BenefitDetails.push({ SortIndex: this.SortIndex, ClaimCodeId: "", EventCodeId: "" });
        },
        removeBenefitDetail: function (index) {
            this.BenefitDetails.splice(index, 1);
            this.SortIndex--;
        },
        autoExpandTextarea: function (id) {
            var tArea = $('#' + id);
            this.textAreaWidth = tArea.width();
            this.textAreaHeight = tArea.height();
            tArea.autoResize();
            tArea.trigger('keyup'); 

            tArea.on('keypress', function (evt) {
                var evt = (evt) ? evt : ((event) ? event : null);
                if (evt.keyCode == 13)
                    return false;
            });
        },
        autoCollapseTextarea: function (id) {
            var tArea = $('#' + id);
            tArea.height(this.textAreaHeight);
        },
        filterClaimCodes: function (index) {
            var item = this.BenefitDetails[index];

            var ccId = null;
            if (item != null && item.Id != null) {
                ccId = item.ClaimCodeId;
            }

            var ClaimCodesData;
            $.ajax({
                url: GetClaimCodesUrl,
                type: "POST",
                data: { claimCodeId: ccId },
                cache: false,
                async: false,
                success: function (data) {
                    ClaimCodesData = data.claimCodes
                }
            });
            return this.ClaimCodes = ClaimCodesData;
        },
        filterEventCodes: function (index) {
            var item = this.BenefitDetails[index];

            var ccId = null;
            if (item != null && item.Id != null) {
                ccId = item.EventCodeId;
            }

            var EventCodesData;
            $.ajax({
                url: GetEventCodesUrl,
                type: "POST",
                data: { eventCodeId: ccId },
                cache: false,
                async: false,
                success: function (data) {
                    EventCodesData = data.eventCodes
                }
            });
            return this.MLReEventCodes = EventCodesData;
        },
    },
    created: function () {
        this.SortIndex = (this.BenefitDetails) ? this.BenefitDetails.length - 1 : -1;

        if (this.BenefitDetails.length > 0) {
            this.BenefitDetails.forEach(function (benefitDetail) {

                if (benefitDetail != null) {
                    if (benefitDetail.ClaimCodeId == 0) {
                        benefitDetail.ClaimCodeId = "";
                    }

                    if (benefitDetail.EventCodeId == 0) {
                        benefitDetail.EventCodeId = "";
                    }
                }
            });
        }
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});

$(document).ready(function () {
    dateOffAutoComplete();

    $('#EffectiveStartDate').datepicker({
        format: DateFormatDatePickerJs,
        autoclose: true
    });

    $('#EffectiveEndDate').datepicker({
        format: DateFormatDatePickerJs,
        autoclose: true
    });
});

function focusOnDate(val) {
    $('#' + val).focus();
}