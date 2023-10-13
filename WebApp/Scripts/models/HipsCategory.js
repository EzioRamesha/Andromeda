
var app = new Vue({
    el: '#app',
    data: {
        HipsCategory: HipsCategoryModel,
        HipsCategoryDetails: HipsCategoryDetails ? HipsCategoryDetails : [],
        ItemTypes: ItemTypes,
        HipsCategoryDetailMaxIndex: 0,
        SubcategoryModal: { index: "", row: "", code: "" },
    },
    methods: {
        addNew: function () {
            this.HipsCategoryDetails.push({ Subcategory: "", Description: "", ItemType: "" });
            this.HipsCategoryDetailMaxIndex += 1;
        },
        openDeleteSubcategoryModal: function (index) {
            this.SubcategoryModal.index = index;
            this.SubcategoryModal.row = index + 1;
            this.SubcategoryModal.code = this.HipsCategoryDetails[index].Subcategory;

            $("#deleteSubcategoryModal").modal()
        },
        removeHipsCategoryDetail: function () {
            //this.HipsCategoryDetails.splice(this.SubcategoryModal.index, 1);
            //this.HipsCategoryDetailMaxIndex -= 1;
            //$("#deleteSubcategoryModal").modal('hide');

            var index = this.SubcategoryModal.index;
            var item = this.HipsCategoryDetails[index];
            var hipsCategoryDetails = this.HipsCategoryDetails;
            var updatedHipsCategoryDetailIndex = this.HipsCategoryDetailMaxIndex;
            if (item != null && item.Id != null && item.Id != 0) {
                $.ajax({
                    url: ValidateHipsSubCategoryDeleteUrl,
                    type: "POST",
                    data: { hipsSubCategoryId: item.Id },
                    cache: false,
                    async: false,
                    success: function (data) {
                        var valid = data.valid;
                        if (valid) {
                            hipsCategoryDetails.splice(index, 1);
                            updatedHipsCategoryDetailIndex -= 1;
                        } else {
                            $("#errorRecordInUsed").css("display", "block");
                            $("#errorRecordInUsed").text('The Subcategory "' + item.Subcategory + '" In Use');
                        }
                    }
                });
                this.HipsCategoryDetailMaxIndex = updatedHipsCategoryDetailIndex;
            } else {
                this.HipsCategoryDetails.splice(index, 1);
                this.HipsCategoryDetailMaxIndex -= 1;
            }
            $("#deleteSubcategoryModal").modal('hide');
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
    },
    created: function () {
        this.HipsCategoryDetailMaxIndex = (this.HipsCategoryDetails) ? this.HipsCategoryDetails.length - 1 : -1;
    },
    updated() {
        //$(this.$refs.select).selectpicker('refresh');
    }
});
