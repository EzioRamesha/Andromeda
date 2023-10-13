
var app = new Vue({
    el: '#app',
    data: {
        CalCedant: CalCedantModel,
        ClaimCodes: ClaimCodeData,
        DropDownFundsAccountingTypeCodes: DropDownFundsAccountingTypeCodes,
        DropDownCalCedantTypes: DropDownCalCedantTypes,
        CalCedantDetails: CALCedantDetailList ? CALCedantDetailList : [],
        SortIndex: 0,
        ClaimCodeCedant: [],
    },
    methods: {
        addNew: function () {            
            this.CalCedantDetails.push({ ClaimCodeId: "", Type: TypeBoth, FundsAccountingTypePickListDetailId: "" });
            this.SortIndex += 1;
        },
        removeDetail: function (index) {
            this.CalCedantDetails.splice(index, 1);
            this.ClaimCodeCedant.splice(index, 1);
            this.SortIndex -= 1;
        },
        openDatePicker: function (currentId) {
            var idStr = currentId.split("_");
            var type = idStr[0];
            var field = idStr[1];
            var index = idStr[2];

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
                updateDateValue(type, field, index, $(id).val());
            });

            $(id).focus();
        },
        updateDateValue: function (type, field, index, value) {
            var typeStr = type + "s";
            this[typeStr][index][field] = value;
        },
        getClaimCodeVal: function (index) {
            var item = this.CalCedantDetails[index];
            this.ClaimCodeCedant[index] = item.ClaimCodeId;            
        },
        filterClaimCodes: function (index) {
            var item = this.CalCedantDetails[index];

            var ccId = null;
            if (item != null && item.Id != null) {
                ccId = item.ClaimCodeId;
            }

            //var itemsWithoutCurrent;
            //if (item != null) {
            //    var exceptIndex = index;
            //    itemsWithoutCurrent = this.ClaimCodeCedant.filter((value, index) => exceptIndex !== index);
            //}

            var claimCodes;
            $.ajax({
                url: GetClaimCodesUrl,
                type: "POST",
                data: { claimCodeId: ccId },
                cache: false,
                async: false,
                success: function (data) {
                    claimCodes = data.claimCodes
                }
            });

            //this.ClaimCodes = [];
            var filteredClaimCodes = [];
            claimCodes.forEach((c) => {
                var details = this.CalCedantDetails.filter(e => e.ClaimCodeId == c.Value);
                if (c.Value == ccId || details.length == 0 || (details.length == 1 && details[0].Type != TypeBoth)) {
                    filteredClaimCodes.push(c);
                }
            });
            console.log(filteredClaimCodes);

            return this.ClaimCodes = filteredClaimCodes;
        }
    },
    created: function () {
        //this.SortIndex = (this.CalCedantDetails) ? this.CalCedantDetails.length - 1 : -1;

        //var items = new Array();
        //if (this.CalCedantDetails.length > 0) {
        //    this.CalCedantDetails.forEach(function (calCedantDetail) {

        //        if (calCedantDetail != null) {
        //            if (calCedantDetail.ClaimCodeId == 0) {
        //                calCedantDetail.ClaimCodeId = "";

        //                items.push(calCedantDetail.ClaimCodeId);
        //            }
        //            else {
        //                items.push(calCedantDetail.ClaimCodeId.toString());
        //            }
        //        }
        //    });
        //}

        //this.ClaimCodeCedant = items;
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});