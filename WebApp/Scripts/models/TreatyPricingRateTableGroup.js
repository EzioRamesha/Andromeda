
var app = new Vue({
    el: '#app',
    data: {
        RateTableGroup: RateTableGroupModel,
        TreatyPricingRateTables: TreatyPricingRateTables ? TreatyPricingRateTables : [],
        TreatyPricingRateTableMaxIndex: 0,
    },
    methods: {
        editRateTableLink: function (index) {
            var item = this.TreatyPricingRateTables[index];
            if (item != null && item.Id != '') {
                var url = EditRateTableUrl + item.Id;
                return url
            }
        },
    },
    created: function () {
        if (this.TreatyPricingRateTables)
            this.TreatyPricingRateTableMaxIndex = this.TreatyPricingRateTables.length - 1;
    },
    updated() {

    }
});
