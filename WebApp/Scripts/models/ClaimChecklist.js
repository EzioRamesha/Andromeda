
var app = new Vue({
    el: '#app',
    data: {
        ClaimChecklist: ClaimChecklistModel,
        Details: Details ? Details : [],
        DetailMaxIndex: 0,
        textAreaWidth: 150,
        textAreaHeight: 21,
        Disabled: Disabled,
    },
    methods: {
        addNew: function () {
            this.Details.push({ Name: "", Remark: "" });
            this.DetailMaxIndex += 1;
        },
        removeDetail: function (index) {
            this.Details.splice(index, 1);
            this.DetailMaxIndex -= 1;
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
        expandTextarea: function () {
            if (this.Disabled == 'True') {
                var expandElementHeight = this.expandElementHeight;
                expandElementHeight('.textarea-auto-expand');
            }
        },
        expandElementHeight: function (selector, extraHeight = 0) {
            $(selector).each(
                function () {
                    var height = $(this)[0].scrollHeight + (extraHeight);
                    if (height != 0)
                        $(this).css('height', height + 'px');
                }
            )
        },
    },
    created: function () {
        this.DetailMaxIndex = (this.Details) ? this.Details.length - 1 : -1;
    },
    mounted: function () {
        this.$nextTick(function () {
            this.expandTextarea();
        })
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});
