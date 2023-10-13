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
            if (distributionChannelCount != 0) {
                $.each(existingTokens, function (index, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })
        .on('tokenfield:createdtoken', function (e) {
            distributionChannelCount += 1;
            $("#DistributionChannelTokenField-tokenfield").removeAttr('placeholder');
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

    $('#EffectiveAtStr').datepicker({
        format: DateFormatDatePickerJs,
    });

    if (ReadOnly) {
        $('#BenefitCodeTokenField').tokenfield('disable');
        $('#DistributionChannelTokenField').tokenfield('disable');
    }

    ignoreFields = ['Description'];
    var version = $('#CurrentVersion').val();
    getVersionDetails(version);
});

function focusOnDate(val) {
    $('#' + val).focus();
}

function getVersionDetails(version) {
    var questionnaires = [];
    var uploads = [];
    $.ajax({
        url: GetVersionDetailUrl,
        type: "POST",
        cache: false,
        async: false,
        data: {
            uwQuestionnaireId: Model.Id,
            uwQuestionnaireVersion: version,
        },
        success: function (data) {
            questionnaires = data.questionnaireBos;
            uploads = data.fileUploadBos;
        }
    });
    app.Questionnaires = questionnaires;
    app.Uploads = uploads;
}

function resetVersionDetails() {
    var questionnaires = [];
    var uploads = [];

    app.Questionnaires = questionnaires;
    app.Uploads = uploads;
    app.ModelVersion = Model.CurrentVersionObject;

    app.Disable = false;
}

var app = new Vue({
    el: '#app',
    data: {
        Model: Model,
        ModelVersion: Model.CurrentVersionObject,
        Disable: false,
        // Version - Questionnaire
        Questionnaires: [],
        // Version - Upload
        Uploads: [],
        UploadErrors: [],
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
        // Upload
        resetUploadFile() {
            var upload = $('#questionnaireFiles');
            upload.val(null);
            var selectedFiles = document.querySelector(".questionnaire-upload#selectedFiles");
            selectedFiles.innerHTML = "";

            $('#uploadQuestionnaireError').empty();
            $('#uploadQuestionnaireError').hide();
        },
        uploadFile() {
            var selectedFiles = document.querySelector(".questionnaire-upload#selectedFiles");
            var upload = $('#questionnaireFiles');
            selectedFiles.innerHTML = "";
            if (!upload[0].files[0]) return;

            var files = upload[0].files;
            var list = "<li>" + files[0].name + "</li>";
            selectedFiles.innerHTML = "<ul>" + list + "</ul>";
        },
        saveUploadFile() {
            var errorList = [];
            var upload = $('#questionnaireFiles');
            var file = upload[0].files[0];
            console.log(this.ModelVersion)
            console.log($('#CurrentVersion').val())
            if ($('#CurrentVersion').val() != this.ModelVersion.Version) {
                errorList.push("You can only upload data for the latest version");
            }

            if (!file) {
                errorList.push("File Upload is Required.");
            }

            if (errorList.length == 0) {                
                var fileData = new FormData()
                fileData.append(file.name, file);

                fileData.append('uwQuestionnaireId', this.Model.Id);
                fileData.append('uwQuestionnaireVersion', $('#CurrentVersion').val());

                $.ajax({
                    url: UploadQuestionnaireUrl,
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
                $('#uploadQuestionnaireError').append(text);
                $('#uploadQuestionnaireError').show();
                return;
            }

            $('#addUploadModal').modal('hide');
            getVersionDetails(this.ModelVersion.Version);
        },
        getUploadFileError: function(index) {
            var item = this.Uploads[index];
            if (item.Errors != null) {
                this.UploadErrors = JSON.parse(item.Errors);
            }
        },
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.CreatedByName = AuthUserName;
            this.RemarkModal.ModuleId = this.Model.ModuleId;
            this.RemarkModal.ObjectId = this.Model.TreatyPricingCedantId;
            this.RemarkModal.SubModuleController = SubModuleController;
            this.RemarkModal.SubObjectId = this.Model.Id;
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

            var parentId = this.Model.TreatyPricingCedantId;
            var document = {
                ModuleId: this.Model.ModuleId,
                ObjectId: parentId,
                SubModuleController: SubModuleController,
                SubObjectId: this.Model.Id,
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
        if (this.Model.Type == 0)
            this.Model.Type = "";

        if (this.Products)
            this.ProductMaxIndex = this.Products.length - 1;
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
        //this.$nextTick(function () { $('.selectpicker').selectpicker('refresh'); });
    }
});

var setVersionCallBack = function (bo) {
    getVersionDetails(bo.Version);
    app.Disable = disableVersion;
}
