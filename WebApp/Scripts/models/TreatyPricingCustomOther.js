var CampaignTypeCount = 0;
var UnderwritingMethodCount = 0;
var DistributionChannelCount = 0;

$(function () {
    $("#CurrentVersion").on('change', function () {
        if ($("#CurrentVersion").val() != $("#EditableVersion").val()) {
            console.log("cannot edit");
            $("#verUpload").css("display", "none");
        } else {
            console.log("editable");
            $("#verUpload").css("display", "block");
        }
    })
});

function setVersionCallBack(bo) {
    var selectedFiles = document.querySelector('#uploadselectedFiles');
    selectedFiles.innerHTML = "";

    var content = "";
    if (bo.FileName != null) {
        var input = "<input class='btn btn-transparent' type='submit' style='color:#01509F!important; font-size:12px;' value='" + bo.FileName + "' formaction='" + DownloadFileUrl + "?versionId=" + bo.Id + "'>";
        var list = "<li>" + input + "</li>";
        content = "<ul>" + list + "</ul>";
        $('#uploadselectedFiles').addClass("file-list-display")
    } else {
        content = "No File";
        $('#uploadselectedFiles').removeClass("file-list-display")
    }

    $('#files').prop('disabled', disableVersion);

    selectedFiles.innerHTML = content;
}

var app = new Vue({
    el: '#app',
    data: {
        TreatyPricingCustomOther: Model,
        TreatyPricingCustomOtherVersion: Model.CurrentVersionObject,
        DropDownCedants: DropDownCedants,
        DropDownProductTypes: DropDownProductTypes,
        DropDownProducts: DropDownProducts,
        DropDownProductQuotations: DropDownProductQuotations,
        // Product
        CustomOtherProducts: CustomOtherProducts,
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
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.CreatedByName = AuthUserName;
            this.RemarkModal.ModuleId = this.TreatyPricingCustomOther.ModuleId;
            this.RemarkModal.ObjectId = this.TreatyPricingCustomOther.TreatyPricingCedantId;
            this.RemarkModal.SubModuleController = SubModuleController;
            this.RemarkModal.SubObjectId = this.TreatyPricingCustomOther.Id;
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

            var parentId = this.TreatyPricingCustomOther.TreatyPricingCedantId;
            var document = {
                ModuleId: this.TreatyPricingCustomOther.ModuleId,
                ObjectId: parentId,
                SubModuleController: SubModuleController,
                SubObjectId: this.TreatyPricingCustomOther.Id,
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
        this.CustomOtherProductsMaxIndex = (this.CustomOtherProducts) ? this.CustomOtherProducts.length : 0;
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});

function setVal(switchId) {
    console.log(switchId)
    setDisabled(switchId);
}

function setDisabled(switchId) {
    if ($('#' + switchId).is(":checked")) {
        return true;
    }
    return false;
}