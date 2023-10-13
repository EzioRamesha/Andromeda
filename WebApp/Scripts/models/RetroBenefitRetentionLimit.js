$(document).ready(function () {
    $('#EffectiveStartDate').datepicker({
        format: DateFormatDatePickerJs,
    });
    $('#EffectiveEndDate').datepicker({
        format: DateFormatDatePickerJs,
    });
});

function focusOnDate(val) {
    $('#' + val).focus();
}

var app = new Vue({
    el: '#app',
    data: {
        RetroBenefitRetentionLimit: RetroBenefitRetentionLimitModel,
        RetroBenefitRetentionLimitDetails: RetroBenefitRetentionLimitDetails ? RetroBenefitRetentionLimitDetails : [],
        RetroBenefitRetentionLimitDetailMaxIndex: 0,
    },
    methods: {
        addNew: function () {
            this.RetroBenefitRetentionLimitDetails.push(
                {
                    MinIssueAgeStr: "",
                    MaxIssueAgeStr: "",
                    MortalityLimitFromStr: "",
                    MortalityLimitToStr: "",
                    ReinsEffStartDateStr: "",
                    ReinsEffEndDateStr: "",
                    MlreRetentionAmountStr: "",
                    MinReinsAmountStr: ""
                }
            );
            this.RetroBenefitRetentionLimitDetailMaxIndex += 1;
        },
        removeRetroBenefitRetentionLimitDetail: function (index) {
            this.RetroBenefitRetentionLimitDetails.splice(index, 1);
            this.RetroBenefitRetentionLimitDetailMaxIndex -= 1;
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
            this.RetroBenefitRetentionLimitDetails[index][field] = value;
        },
    },
    created: function () {
        this.RetroBenefitRetentionLimitDetailMaxIndex = (this.RetroBenefitRetentionLimitDetails) ? this.RetroBenefitRetentionLimitDetails.length - 1 : -1;
    },
    updated() {
        //$(this.$refs.select).selectpicker('refresh');
    }
});
