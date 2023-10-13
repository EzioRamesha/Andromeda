var benefitCodeCount = 0;
var distributionChannelCount = 0;

var tokenfieldReady = {
    benefitCode: false,
    distributionChannel: false,
};

var loadingDiv = $("#loadingSpinner");

$(document).ready(function () {
    $('#BenefitCodeTokenField')
        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            $.each(existingTokens, function (index, token) {
                if (benefitCodeCount != 0) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                }
            });
        })
        .on('tokenfield:createdtoken', function (e) {
            var valid = BenefitCodes.includes(e.attrs.value)
            if (!valid) {
                $(e.relatedTarget).addClass('invalid');
            }
            benefitCodeCount += 1;
            $('#BenefitCodeTokenField-tokenfield').removeAttr('placeholder');
        })
        .on('tokenfield:edittoken', function (e) {
            var valid = BenefitCodes.includes(e.attrs.value)
            if (valid) {
                e.preventDefault();
            }
        })
        .on('tokenfield:removedtoken', function (e) {
            benefitCodeCount -= 1;
            if (benefitCodeCount == 0) {
                $("#BenefitCodeTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })
        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.benefitCode = true;
            var isReady = true;
            Object.keys(tokenfieldReady).forEach(function (key) {
                if (tokenfieldReady[key] == false) {
                    isReady = false;
                }
            });
            if (isReady && loadingDiv != null) {
                loadingDiv.addClass('hide-loading-spinner');
            }
        })
        .tokenfield({
            autocomplete: {
                source: BenefitCodes,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });

    $('#DistributionChannelTokenField')
        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            $.each(existingTokens, function (index, token) {
                if (distributionChannelCount != 0) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                }
            });
        })
        .on('tokenfield:createdtoken', function (e) {
            var valid = DistributionChannels.includes(e.attrs.value)
            if (!valid) {
                $(e.relatedTarget).addClass('invalid');
            }
            distributionChannelCount += 1;
            $('#DistributionChannelTokenField-tokenfield').removeAttr('placeholder');
        })
        .on('tokenfield:edittoken', function (e) {
            var valid = DistributionChannels.includes(e.attrs.value)
            if (valid) {
                e.preventDefault();
            }
        })
        .on('tokenfield:removedtoken', function (e) {
            distributionChannelCount -= 1;
            if (distributionChannelCount == 0) {
                $("#DistributionChannelTokenField-tokenfield").attr("placeholder", "Type here");
            }
        })
        .on('tokenfield:initialize', function (e) {
            tokenfieldReady.distributionChannel = true;
            var isReady = true;
            Object.keys(tokenfieldReady).forEach(function (key) {
                if (tokenfieldReady[key] == false) {
                    isReady = false;
                }
            });
            if (isReady && loadingDiv != null) {
                loadingDiv.addClass('hide-loading-spinner');
            }
        })
        .tokenfield({
            autocomplete: {
                source: DistributionChannels,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });

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
    var financialTables = [];
    var uploads = [];
    $.ajax({
        url: GetVersionDetailUrl,
        type: "POST",
        cache: false,
        async: false,
        data: {
            financialTableId: Model.Id,
            financialTableVersion: version,
            //treatyPricingFinancialTableVersionId: version,
        },
        success: function (data) {
            financialTables = data.financialTableBos;
            uploads = data.fileUploadBos;
        }
    });
    app.FinancialTableFiles = financialTables;
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
    var financialTables = [];
    var uploads = [];

    app.FinancialTableFiles = financialTables;
    app.Uploads = uploads;
    app.FinancialTableVersion = Model.CurrentVersionObject;

    app.DisabledUpload = false;
}

function getUploadedData(detail) {
    var uploadLegends = [];
    var uploadCells = [];
    $.ajax({
        url: GetUploadedDataUrl,
        type: "POST",
        cache: false,
        async: false,
        data: {
            treatyPricingFinancialTableVersionDetailId: detail,
        },
        success: function (data) {
            uploadLegends = data.legendBos;
            uploadCells = data.cellBos;
        }
    });
    app.UploadLegends = uploadLegends;
    app.UploadCells = uploadCells;
}

var app = new Vue({
    el: '#app',
    data: {
        FinancialTable: Model,
        FinancialTableVersion: Model.CurrentVersionObject,
        // Version - Financial Table
        FinancialTableFiles: [],
        // Version - Upload
        FinancialTableUploadModal: {
            DistributionTier: "",
            Description: "",
        },
        Uploads: [],
        UploadErrors: [],
        DisabledUpload: false,
        // Uploaded data
        UploadLegends: [],
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
            this.FinancialTableUploadModal.DistributionTier = "";
            this.FinancialTableUploadModal.Description = "";
            $('#dropDownDistributionTier').selectpicker('val', '');

            var upload = $('#financialTableFiles');
            upload.val(null);
            var selectedFiles = document.querySelector(".financialTable-upload#selectedFiles");
            selectedFiles.innerHTML = "";

            $('#uploadFinancialTableError').empty();
            $('#uploadFinancialTableError').hide();
        },
        uploadFile() {
            var selectedFiles = document.querySelector(".financialTable-upload#selectedFiles");
            var upload = $('#financialTableFiles');
            selectedFiles.innerHTML = "";
            if (!upload[0].files[0]) return;

            var files = upload[0].files;
            var list = "<li>" + files[0].name + "</li>";
            selectedFiles.innerHTML = "<ul>" + list + "</ul>";
        },
        saveUploadFile() {
            var errorList = [];
            $('#uploadFinancialTableError').empty();

            console.log($('#CurrentVersion').val());
            console.log(this.FinancialTableVersion.Version);

            //if ($('#CurrentVersion').val() != this.FinancialTableVersion.Id) {
            if ($('#CurrentVersion').val() != this.FinancialTableVersion.Version) {
                errorList.push("You can only upload data for the latest version");
            }

            if (this.FinancialTableUploadModal.DistributionTier == "" || this.FinancialTableUploadModal.Description == "") {
                errorList.push("Distribution Tier and Description are required");
            }

            //check for existing tier list in the version
            for (i = 0; i < this.FinancialTableFiles.length; i++) {
                if (this.FinancialTableUploadModal.DistributionTier == this.FinancialTableFiles[i].DistributionTierPickListDetailId) {
                    errorList.push("Distribution Tier already exist in this version");
                    break;
                }
            }

            if (errorList.length == 0) {
                var upload = $('#financialTableFiles');
                var file = upload[0].files[0];
                var fileData = new FormData()
                fileData.append(file.name, file);

                fileData.append('financialTableId', this.FinancialTable.Id);
                fileData.append('financialTableVersion', $('#CurrentVersion').val());
                fileData.append('uploadDistributionTier', this.FinancialTableUploadModal.DistributionTier);
                fileData.append('uploadDescription', this.FinancialTableUploadModal.Description);

                $.ajax({
                    url: UploadFinancialTableUrl,
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
                $('#uploadFinancialTableError').append(text);
                $('#uploadFinancialTableError').show();
                return;
            }

            $('#addUploadModal').modal('hide');
            //getVersionDetails(this.FinancialTableVersion.Id);
            getVersionDetails(this.FinancialTableVersion.Version);
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
            this.RemarkModal.ModuleId = this.FinancialTable.ModuleId;
            this.RemarkModal.ObjectId = this.FinancialTable.TreatyPricingCedantId;
            this.RemarkModal.SubModuleController = SubModuleController;
            this.RemarkModal.SubObjectId = this.FinancialTable.Id;
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

            var parentId = this.FinancialTable.TreatyPricingCedantId;
            var document = {
                ModuleId: this.FinancialTable.ModuleId,
                ObjectId: parentId,
                SubModuleController: SubModuleController,
                SubObjectId: this.FinancialTable.Id,
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
