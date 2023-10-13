
var app = new Vue({
    el: '#app',
    data: {
        SanctionKeyword: SanctionKeywordModel,
        SanctionKeywordDetails: SanctionKeywordDetails ? SanctionKeywordDetails : [],
        DetailMaxIndex: 0,
    },
    methods: {
        addNew: function () {
            this.SanctionKeywordDetails.push({ Keyword: "" });
            this.DetailMaxIndex += 1;
        },
        removeDetail: function (index) {
            this.SanctionKeywordDetails.splice(index, 1);
            this.SanctionKeywordDetailMaxIndex -= 1;
        },
    },
    created: function () {
        this.DetailMaxIndex = (this.SanctionKeywordDetails) ? this.SanctionKeywordDetails.length - 1 : -1;
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});
