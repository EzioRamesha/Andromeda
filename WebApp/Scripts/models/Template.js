
var app = new Vue({
    el: '#app',
    data: {
        Template: Model,
        TemplateDetails: TemplateDetailList ? TemplateDetailList : [],
        SortIndex: 0,
    },
    methods: {
        removeDetail: function (index) {
            this.TemplateDetails.splice(index, 1);
            this.SortIndex -= 1;
        },
        uploadNew: function () {
            var upload = $('#dataFile');
            var files = upload[0].files;

            if (!files) return;

            var templateFiles = [];

            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                var filename = file.name;

                var fileData = new FormData();
                fileData.append(filename, file);

                // Save Temp File
                var bo = null;
                $.ajax({
                    url: SaveTempUploadFileUrl,
                    type: "POST",
                    contentType: false,
                    processData: false,
                    cache: false,
                    async: false,
                    data: fileData,
                    success: function (data) {
                        if (data.error) {
                            alert(data.error);
                        } else {
                            bo = data.TemplateUploadBo;
                        }
                    }
                });

                bo.TemplateVersion = (this.TemplateDetails) ? this.TemplateDetails.length + 1 : 1;
                this.TemplateDetails.push(Object.assign({}, bo));
                templateFiles.push(Object.assign({}, bo));
            }

            $('#dataFile').val(null);
            return templateFiles;
        },
        downloadTemplateLink: function (index) {
            var item = this.TemplateDetails[index];
            if (item != null && item.Id != '') {
                var url = DownloadTemplateUrl + '/' + item.Id;
                return url;
            }
        }
    },
    created: function () {
        this.SortIndex = (this.TemplateDetails) ? this.TemplateDetails.length - 1 : -1;
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});