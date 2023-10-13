var app = new Vue({
    el: '#app',
    data: {
        // Models
        DiscountTable: DiscountTableModel,
        RiDiscounts: RiDiscounts ? RiDiscounts : [],
        LargeDiscounts: LargeDiscounts ? LargeDiscounts : [],
        GroupDiscounts: GroupDiscounts ? GroupDiscounts : [],
        // Indexes
        RiDiscountMaxIndex: 0,
        LargeDiscountMaxIndex: 0,
        GroupDiscountMaxIndex: 0,
        // Others
        Disabled: Disabled,
    },
    methods: {
        // Add Remove
        addRiDiscount: function () {
            this.RiDiscountMaxIndex++;
            this.RiDiscounts.push({ DiscountCode: null, EffectiveStartDateStr: null, EffectiveEndDateStr: null, DurationFromStr: null, DurationToStr: null, DiscountStr: null });
        },
        removeRiDiscount: function (index) {
            var item = this.RiDiscounts[index];
            var riDiscounts = this.RiDiscounts;
            var updatedRiDiscountIndex = this.RiDiscountMaxIndex;
            if (item != null && item.Id != null && item.Id != 0) {
                $.ajax({
                    url: RiDiscountValidateDeleteUrl,
                    type: "POST",
                    data: { id: item.Id },
                    cache: false,
                    async: false,
                    success: function (data) {
                        var valid = data.Valid;
                        if (valid) {
                            riDiscounts.splice(index, 1);
                            updatedRiDiscountIndex -= 1;
                        } else {
                            alert('The RI Discount "' + item.DiscountCode + '" In Use');
                        }
                    }
                });
                this.RiDiscountMaxIndex = updatedRiDiscountIndex;
            } else {
                this.RiDiscounts.splice(index, 1);
                this.RiDiscountMaxIndex -= 1;
            }
        },
        addLargeDiscount: function () {
            this.LargeDiscountMaxIndex++;
            this.LargeDiscounts.push({ DiscountCode: null, EffectiveStartDateStr: null, EffectiveEndDateStr: null, AarFromStr: null, AarToStr: null, DiscountStr: null });
        },
        removeLargeDiscount: function (index) {
            var item = this.LargeDiscounts[index];
            var largeDiscounts = this.LargeDiscounts;
            var updatedLargeDiscountIndex = this.LargeDiscountMaxIndex;
            if (item != null && item.Id != null && item.Id != 0) {
                $.ajax({
                    url: LargeDiscountValidateDeleteUrl,
                    type: "POST",
                    data: { id: item.Id },
                    cache: false,
                    async: false,
                    success: function (data) {
                        var valid = data.Valid;
                        if (valid) {
                            largeDiscounts.splice(index, 1);
                            updatedLargeDiscountIndex -= 1;
                        } else {
                            alert('The Large Discount "' + item.DiscountCode + '" In Use');
                        }
                    }
                });
                this.LargeDiscountMaxIndex = updatedLargeDiscountIndex;
            } else {
                this.LargeDiscounts.splice(index, 1);
                this.LargeDiscountMaxIndex -= 1;
            }
        },
        addGroupDiscount: function () {
            this.GroupDiscountMaxIndex++;
            this.GroupDiscounts.push({ DiscountCode: null, EffectiveStartDateStr: null, EffectiveEndDateStr: null, GroupSizeFromStr: null, GroupSizeToStr: null, DiscountStr: null });
        },
        removeGroupDiscount: function (index) {
            var item = this.GroupDiscounts[index];
            var groupDiscounts = this.GroupDiscounts;
            var updatedGroupDiscountIndex = this.GroupDiscountMaxIndex;
            if (item != null && item.Id != null && item.Id != 0) {
                $.ajax({
                    url: GroupDiscountValidateDeleteUrl,
                    type: "POST",
                    data: { id: item.Id },
                    cache: false,
                    async: false,
                    success: function (data) {
                        var valid = data.Valid;
                        if (valid) {
                            groupDiscounts.splice(index, 1);
                            updatedGroupDiscountIndex -= 1;
                        } else {
                            alert('The Group Discount "' + item.DiscountCode + '" In Use');
                        }
                    }
                });
                this.GroupDiscountMaxIndex = updatedGroupDiscountIndex;
            } else {
                this.GroupDiscounts.splice(index, 1);
                this.GroupDiscountMaxIndex -= 1;
            }
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
            //this.CurrentEvalSingle.StandardOutputBos[index].DummyValue = value;
        },
    },
    created: function () {
        this.RiDiscountMaxIndex = (this.RiDiscounts) ? this.RiDiscounts.length - 1 : -1;
        this.LargeDiscountMaxIndex = (this.LargeDiscounts) ? this.LargeDiscounts.length - 1 : -1;
        this.GroupDiscountMaxIndex = (this.GroupDiscounts) ? this.GroupDiscounts.length - 1 : -1;
    },
});

function uploadRiDiscountFile(form) {
    form.action = UploadRiDiscountFileUrl;
    form.submit();
}

function uploadLargeDiscountFile(form) {
    form.action = UploadLargeDiscountFileUrl;
    form.submit();
}

function uploadGroupDiscountFile(form) {
    form.action = UploadGroupDiscountFileUrl;
    form.submit();
}