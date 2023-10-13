var benefitCodeCount = 0;
var distributionChannelCount = 0;

var tokenfieldReady = {
    benefitCode: false,
    distributionChannel: false,
};

var loadingDiv = $("#loadingSpinner");

$(document).ready(function () {
    initializeTokenField('#BenefitCodeTokenField', BenefitCodes, 'benefitCodeCount');
    initializeTokenField('#DistributionChannelTokenField', DistributionChannels, 'distributionChannelCount');

    if (ReadOnly) {
        $('#BenefitCodeTokenField').tokenfield('disable');
        $('#DistributionChannelTokenField').tokenfield('disable');
    }

    var version = $('#CurrentVersion').val();
    getVersionDetails(version);
});

function focusOnDate(val) {
    $('#' + val).focus();
}

function getVersionDetails(version) {
    var medicalTables = [];
    var uploads = [];
    $.ajax({
        url: GetVersionDetailUrl,
        type: "POST",
        cache: false,
        async: false,
        data: {
            medicalTableId: Model.Id,
            medicalTableVersion: version,
            //treatyPricingMedicalTableVersionId: version,
        },
        success: function (data) {
            medicalTables = data.medicalTableBos;
            uploads = data.fileUploadBos;
        }
    });
    app.MedicalTableFiles = medicalTables;
    app.Uploads = uploads;

    //if (version != Model.CurrentVersionObject.Id) {
    if (version != Model.CurrentVersion || ReadOnly) {
        app.DisabledUpload = true;
    }
    else {
        app.DisabledUpload = false;
    }
}

function resetVersionDetails() {
    var medicalTables = [];
    var uploads = [];

    app.MedicalTableFiles = medicalTables;
    app.Uploads = uploads;
    app.MedicalTableVersion = Model.CurrentVersionObject;

    app.DisabledUpload = false;
}

function getUploadedData(detail) {
    var uploadLegends = [];
    var uploadColumns = [];
    var uploadRows = [];
    var uploadCells = [];
    $.ajax({
        url: GetUploadedDataUrl,
        type: "POST",
        cache: false,
        async: false,
        data: {
            treatyPricingMedicalTableVersionDetailId: detail,
        },
        success: function (data) {
            uploadLegends = data.legendBos;
            uploadColumns = data.columnBos;
            uploadRows = data.rowBos;
            uploadCells = data.cellBos;
        }
    });
    app.UploadLegends = uploadLegends;
    app.UploadColumns = uploadColumns;
    app.UploadRows = uploadRows;
    app.UploadCells = uploadCells;

    var th = document.getElementById("medicalRequirementsHeader");
    th.setAttribute("colspan", app.UploadColumns.length);
}

var app = new Vue({
    el: '#app',
    data: {
        MedicalTable: Model,
        MedicalTableVersion: Model.CurrentVersionObject,
        // Version - Medical Table
        MedicalTableFiles: [],
        // Version - Upload
        MedicalTableUploadModal: {
            DistributionTier: "",
            Description: "",
        },
        Uploads: [],
        UploadErrors: [],
        DisabledUpload: false,
        // Uploaded data
        UploadLegends: [],
        UploadColumns: [],
        UploadRows: [],
        UploadCells: [],
        // Product
        Products: Products ? Products : [],
        ProductMaxIndex: 0,
        // Remark
        Remarks: Remarks,
        RemarkSubjects: RemarkSubjects,
        RemarkMaxIndex: 0,
        RemarkModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            Status: null,
            CreatedAtStr: null,
            Content: null,
            ShowSubjectSelect: true,
        },
        // Changelog
        Changelogs: Changelogs,
        Trails: [],
        VersionTrail: "",
    },
    methods: {
        resetUploadFile() {
            this.MedicalTableUploadModal.DistributionTier = "";
            this.MedicalTableUploadModal.Description = "";
            $('#dropDownDistributionTier').selectpicker('val', '');

            var upload = $('#medicalTableFiles');
            upload.val(null);
            var selectedFiles = document.querySelector(".medicalTable-upload#selectedFiles");
            selectedFiles.innerHTML = "";

            $('#uploadMedicalTableError').empty();
            $('#uploadMedicalTableError').hide();
        },
        uploadFile() {
            var selectedFiles = document.querySelector(".medicalTable-upload#selectedFiles");
            var upload = $('#medicalTableFiles');
            selectedFiles.innerHTML = "";
            if (!upload[0].files[0]) return;

            var files = upload[0].files;
            var list = "<li>" + files[0].name + "</li>";
            selectedFiles.innerHTML = "<ul>" + list + "</ul>";
        },
        saveUploadFile() {
            var errorList = [];
            $('#uploadMedicalTableError').empty();

            //if ($('#CurrentVersion').val() != this.MedicalTableVersion.Id) {
            if ($('#CurrentVersion').val() != this.MedicalTableVersion.Version) {
                errorList.push("You can only upload data for the latest version");
            }

            if (this.MedicalTableUploadModal.DistributionTier == "" || this.MedicalTableUploadModal.Description == "") {
                errorList.push("Distribution Tier and Description are required");
            }

            //check for existing tier list in the version
            for (i = 0; i < this.MedicalTableFiles.length; i++) {
                if (this.MedicalTableUploadModal.DistributionTier == this.MedicalTableFiles[i].DistributionTierPickListDetailId) {
                    errorList.push("Distribution Tier already exist in this version");
                    break;
                }
            }

            if (errorList.length == 0) {
                var upload = $('#medicalTableFiles');
                var file = upload[0].files[0];
                var fileData = new FormData()
                fileData.append(file.name, file);

                fileData.append('medicalTableId', this.MedicalTable.Id);
                fileData.append('medicalTableVersion', $('#CurrentVersion').val());
                fileData.append('uploadDistributionTier', this.MedicalTableUploadModal.DistributionTier);
                fileData.append('uploadDescription', this.MedicalTableUploadModal.Description);

                $.ajax({
                    url: UploadMedicalTableUrl,
                    type: "POST",
                    contentType: false,
                    processData: false,
                    cache: false,
                    async: false,
                    data: fileData,
                    success: function (data) {
                        if (data.errors && data.errors.length > 0) {
                            errorList = data.errors;
                        }
                    }
                });
            }

            if (errorList.length > 0) {
                text = "<ul>";
                for (i = 0; i < errorList.length; i++) {
                    text += "<li>" + errorList[i] + "</li>";
                }
                text += "</ul>";
                $('#uploadMedicalTableError').append(text);
                $('#uploadMedicalTableError').show();
                return;
            }

            $('#addUploadModal').modal('hide');
            //getVersionDetails(this.MedicalTableVersion.Id);
            getVersionDetails(this.MedicalTableVersion.Version);
        },
        getUploadFileError: function (index) {
            var item = this.Uploads[index];
            if (item.Errors != null) {
                this.UploadErrors = JSON.parse(item.Errors);
            }
        },
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.CreatedByName = AuthUserName;
            this.RemarkModal.ModuleId = this.MedicalTable.ModuleId;
            this.RemarkModal.ObjectId = this.MedicalTable.TreatyPricingCedantId;
            this.RemarkModal.SubModuleController = SubModuleController;
            this.RemarkModal.SubObjectId = this.MedicalTable.Id;
            this.RemarkModal.Version = Model.CurrentVersion;
            this.RemarkModal.ShowSubjectSelect = false;
            this.RemarkModal.Content = null;

            this.toggleRemarkSubject();
            clearSelectedFiles('remark');
        },
        toggleRemarkSubject: function () {
            this.RemarkModal.Subject = null;
            this.$nextTick(function () {
                $('#subjectSelect').selectpicker('refresh');
            });

            this.RemarkModal.ShowSubjectSelect = !this.RemarkModal.ShowSubjectSelect;
            if (this.RemarkModal.ShowSubjectSelect) {
                $('#subjectSelect').selectpicker('show');
                $('#subjectText').hide();
            } else {
                $('#subjectSelect').selectpicker('hide');
                $('#subjectText').show();
            }
        },
        addRemark: function () {
            var remark = createRemark(this.RemarkModal);

            if (remark) {
                remark.DocumentBos = this.saveDocuments(this.RemarkMaxIndex, remark.Id);

                this.Remarks.unshift(Object.assign({}, remark));

                if (!this.RemarkSubjects.includes(remark.Subject)) {
                    this.RemarkSubjects.push(remark.Subject);

                    this.$nextTick(function () {
                        $('#subjectSelect').selectpicker('refresh');
                    });
                }

                this.RemarkMaxIndex++;
            }
        },
        saveDocuments(remarkIndex, remarkId = null) {
            var files = getFiles();

            if (!files) return;

            var parentId = this.MedicalTable.TreatyPricingCedantId;
            var document = {
                ModuleId: this.MedicalTable.ModuleId,
                ObjectId: parentId,
                SubModuleController: SubModuleController,
                SubObjectId: this.MedicalTable.Id,
                RemarkId: remarkId,
                RemarkIndex: remarkIndex,
                CreatedByName: AuthUserName,
                CreatedAtStr: this.RemarkModal.CreatedAtStr,
            };

            var documents = [];

            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                var document = saveDocument(document, file, true);

                if (document != null) {
                    documents.push(Object.assign({}, document));
                }
            }

            return documents;
        },
        // Auto Expand TextArea
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
        // Changelog
        viewChangelog: function (index) {
            this.Trails = this.Changelogs[index].UserTrailBos;
            this.VersionTrail = this.Changelogs[index].BetweenVersionTrail;

            $('[id^=showAll_]').hide();
            $('[id^=showLess_]').show();
            $('[id^=showAllBtn_]').show();
            $('[id^=collapseAllBtn_]').hide();

            $('#changelogModal').modal('show');
        },
        toggleChangelogDataView: function (index) {
            $('#showAll_' + index).toggle();
            $('#showLess_' + index).toggle();
            $('#showAllBtn_' + index).toggle();
            $('#collapseAllBtn_' + index).toggle();
        },
    },
    created: function () {
        if (this.Products)
            this.ProductMaxIndex = this.Products.length - 1;
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
        //this.$nextTick(function () { $('.selectpicker').selectpicker('refresh'); });
    }
});
