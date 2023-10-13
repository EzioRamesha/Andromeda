function changeCedant(cedantId) {
    var claimDataConfigId = Model.ClaimDataConfigId;

    $('#ClaimDataConfigId option').remove();
    $('#ClaimDataConfigId').append(new Option("Please select", null));

    if (cedantId == null)
        return;

    if (!isNaN(cedantId)) {
        $.ajax({
            url: GetClaimDataConfigListUrl,
            type: "POST",
            data: { cedantId: cedantId, claimDataConfigId: claimDataConfigId },
            cache: false,
            async: false,
            success: function (data) {
                var claimDataConfigList = data.claimDataConfigBos;
                refreshDropDownItems('ClaimDataConfigId', claimDataConfigList, claimDataConfigId, 'Code', 'Name');
            }
        });
    } else {
        refreshDropDownItems('ClaimDataConfigId', [], null, 'Code', 'Name');
    }
}

function changeClaimDataConfig(claimDataConfigId) {
    var oriClaimDataConfigId = Model.ClaimDataConfigId;
    var oriClaimDataFileConfig = Model.ClaimDataFileConfig;

    $.ajax({
        url: GetClaimDataConfigUrl,
        type: "POST",
        data: { id: claimDataConfigId },
        cache: false,
        async: false,
        success: function (data) {
            claimDataConfig = data.claimDataConfigBo;
        }
    });

    if (oriClaimDataConfigId == claimDataConfigId && oriClaimDataFileConfig != null)
        claimDataConfig.ClaimDataFileConfig = oriClaimDataFileConfig;

    setConfigValues(claimDataConfig);
    toggleFileConfigView();
    toggleHeader();
}

function setConfigValues(claimDataConfig = null) {
    var properties = [
        "TreatyId", "TreatyIdCode", "FileType", "FileTypeName"
    ];

    properties.forEach(function (property) {
        var value = claimDataConfig ? claimDataConfig[property] : '';
        $('#' + property).val(value);
        app.ClaimDataBatch[property] = value;
    });

    var fileConfigProperties = [
        "Worksheet", "Delimiter", "HasHeader", "HeaderRow", "StartRow", "EndRow", "StartColumn", "EndColumn"
    ];

    fileConfigProperties.forEach(function (property) {
        var value = '';
        if (claimDataConfig) {
            value = claimDataConfig.ClaimDataFileConfig[property];
            if (value === true)
                value = 'true';
            else if (value === false)
                value = 'false';
        }

        $('#' + property).val(value);
    });

    $('#Delimiter').selectpicker('refresh');
    $('#HasHeader').selectpicker('refresh');
}

function toggleFileConfigView() {
    var claimDataConfigId = $('#ClaimDataConfigId').val();

    if (isNaN(claimDataConfigId) || !claimDataConfigId || claimDataConfigId.length === 0) {
        $('#fileConfig').hide();
    } else {
        $('#fileConfig').show();
        $('#worksheetDiv').hide();
        $('#delimiterDiv').hide();
        var fileType = $('#FileType').val();
        if (fileType == FileTypeExcel) {
            $('#worksheetDiv').show();
        } else if (fileType == FileTypePlainText) {
            $('#delimiterDiv').show();
        }
    }
}

function toggleHeader() {
    var hasHeader = $('#HasHeader').val();

    $('#headerRowDiv').hide();
    if (hasHeader == 'true')
        $('#headerRowDiv').show();
}

var selDiv = "";
document.addEventListener("DOMContentLoaded", init, false);
function init() {
    var fileSelector = document.querySelector('#files');

    if (fileSelector) {
        fileSelector.addEventListener('change', handleFileUpload, false);
        selDiv = document.querySelector("#selectedFiles");
    }

}

function handleFileUpload(e) {
    if (!e.target.files) return;
    selDiv.innerHTML = "";
    var files = e.target.files;
    var list = "";
    for (var i = 0; i < files.length; i++) {
        var f = files[i];

        // Prevent exceeds file size from uploading
        var fileSize = f.size / 1024 / 1024 / 1024; // in GB
        if (fileSize >= 2) {
            $("#errorSizeExceeds").css("display", "block");
            $("#errorSizeExceeds").text('Maximum allowed size is : 2 GB');
            //reset file upload control
            e.target.value = null;
            return;
        } else {
            $("#errorSizeExceeds").css("display", "none");
        }

        list += "<li>" + f.name + "</li>";
    }
    selDiv.innerHTML = "<ul>" + list + "</ul>";
}

function updateStatus(status) {
    $('#status').val(status);
    submitBatch();
}

function submitBatch() {
    $('#IncludedFiles').val($('#IncludedFilesApp').val());
    $('#editForm').submit();
}

$(document).ready(function () {
    dateOffAutoComplete();

    var cedantId = $('#CedantId').val();
    changeCedant(cedantId);
    if (Model.ClaimDataConfigBo != null) {
        var claimDataConfigBo = Model.ClaimDataConfigBo;
        $('#FileType').val(claimDataConfigBo.FileType);
        $('#FileTypeName').val(claimDataConfigBo.FileTypeName);
    }
    toggleFileConfigView();
    toggleHeader();
    refreshSoaDataMatchInput();

    $("#loadingSpinner").addClass('hide-loading-spinner');
});

function refreshSoaDataMatchInput() {
    if ($('#SoaDataBatchId').val() && $('#SoaDataBatchId').val() > 0) {
        $('#SoaDataMatchStr').val('Matched - ' + $('#SoaDataBatchId').val());
    } else {
        $('#SoaDataMatchStr').val('');
    }
}

function startDownload(download) {
    loadingDiv.removeClass('hide-loading-spinner');
    var downloadToken = (new Date()).getTime();

    if (download.href.includes("downloadToken=")) {
        download.href = download.href.replace(/(downloadToken=).*?/, '$1' + downloadToken + '$2')
    } else {
        download.href += "&downloadToken=" + downloadToken;
    }

    var cookiePattern = new RegExp(("downloadToken=" + downloadToken), "i");
    var cookieTimer = setInterval(checkCookies, 500);
    var refreshSession = setInterval(
        function () {
            $.ajax({
                url: RefreshUserSessionUrl,
                type: "POST",
                cache: false,
                async: false,
                success: function (data) {
                    if (data.logout == true) {
                        window.location.href = LoginUrl;
                    }
                },
            });
        }, 60 * 1000
    );

    function checkCookies() {
        if (document.cookie.search(cookiePattern) >= 0) {
            loadingDiv.addClass('hide-loading-spinner');
            clearInterval(cookieTimer);
            clearInterval(refreshSession);
        }
    }
}

var app = new Vue({
    el: '#app',
    data: {
        // Models
        ClaimDataBatch: Model,
        ClaimDataFiles: ClaimDataFiles,
        ClaimDataBatchStatusFiles: ClaimDataBatchStatusFiles,
        StatusHistories: StatusHistories,
        Remarks: Remarks,
        // Soa Data
        SoaDataBatches: [],
        MatchSoaDataBatchesValidation: [],
        SoaDataMatchStatus: "",
        // File History
        ClaimDataFileErrors: [],
        // Remark
        StatusRemark: "",
        RemarkModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            Status: null,
            CreatedAtStr: null,
            Content: null,
        },
    },
    methods: {
        // FIle History
        checkExcludeFile: function (index) {
            if ($('#claimDataFileExclude_' + index).prop("checked")) {
                this.ClaimDataFiles[index].Mode = ModeExclude;

                var index = this.ClaimDataBatch.IncludedFiles.indexOf(this.ClaimDataFiles[index].Id);
                this.ClaimDataBatch.IncludedFiles.splice(index, 1);
            }
            else {
                this.ClaimDataFiles[index].Mode = ModeInclude;
                this.ClaimDataBatch.IncludedFiles.push(this.ClaimDataFiles[index].Id);
            }
        },
        downloadClaimDataFileUrl: function (index) {
            return DownloadClaimDataFileUrl + "?rawFileId=" + this.ClaimDataFiles[index].RawFileBo.Id;
        },
        editClaimDataFileUrl: function (index) {
            return EditClaimDataFileUrl + '/' + this.ClaimDataFiles[index].Id;
        },
        getClaimDataFileErrors: function (index) {
            var item = this.ClaimDataFiles[index];
            if (item.Errors != null) {
                this.ClaimDataFileErrors = JSON.parse(item.Errors);
            }
        },
        editClaimDataConfigUrl: function (index) {
            var item = this.ClaimDataFiles[index];
            var url = EditClaimDataConfigUrl + '/' + item.ClaimDataConfigBo.Id;
            return url
        },
        // Remarks
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.Status = this.ClaimDataBatch.Status;
            this.RemarkModal.ModuleId = this.ClaimDataBatch.ModuleId;
            this.RemarkModal.ObjectId = this.ClaimDataBatch.Id;
            this.RemarkModal.Content = null;
        },
        addRemark: function () {
            var remark = createRemark(this.RemarkModal);

            if (remark) {
                this.Remarks.unshift(Object.assign({}, remark));
            }
        },
        // Others
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
        // SOA
        validateSoaDataMatchData: function () {
            var validation = new Array();

            this.ClaimDataBatch.TreatyId = $('#TreatyId').val();
            this.ClaimDataBatch.Quarter = $('#Quarter').val();

            if (this.ClaimDataBatch.CedantId == null || this.ClaimDataBatch.CedantId == "" || this.ClaimDataBatch.CedantId == 0)
                validation.push("Ceding Company is required");
            if (this.ClaimDataBatch.TreatyId == null || this.ClaimDataBatch.TreatyId == "" || this.ClaimDataBatch.TreatyId == 0)
                validation.push("Treaty ID is required");
            if (this.ClaimDataBatch.Quarter == null || this.ClaimDataBatch.Quarter == "")
                validation.push("Quarter is required");
            else if (!/[0-9]{4} Q{1}([1-4]){1}/.test(this.ClaimDataBatch.Quarter))
                validation.push("Quarter format is incorrect");

            this.MatchSoaDataBatchesValidation = validation;

            return this.MatchSoaDataBatchesValidation.length == 0;
        },
        searchSoaDataBatch: function () {
            if (!this.validateSoaDataMatchData())
                return;

            var obj = {
                CedantId: this.ClaimDataBatch.CedantId,
                TreatyId: this.ClaimDataBatch.TreatyId,
                Quarter: this.ClaimDataBatch.Quarter,
            };

            var soaDataBatchBos = [];
            $.ajax({
                url: GetSoaDataBatchUrl,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    soaDataBatchBos = data.soaDataBatchBos;
                }
            });

            this.SoaDataBatches = soaDataBatchBos;
            if (this.SoaDataBatches == null || !this.SoaDataBatches.length)
                this.MatchSoaDataBatchesValidation.push("No match found");
        },
        selectSoaDataBatch: function () {
            var index = $('input[name="checkedSoadata"]:checked').val();
            if (typeof index == 'undefined') {
                this.MatchSoaDataBatchesValidation.push("No SOA Data selected.");
                return;
            }

            var item = this.SoaDataBatches[index];
            $('#SoaDataBatchId').val(item.Id);
            this.ClaimDataBatch.SoaDataBatchId = item.Id;
            refreshSoaDataMatchInput();

            $('#matchSoaDataModal').modal('toggle');
        },
        createSoaDataBatch: function () {
            if (!this.validateSoaDataMatchData())
                return;

            var obj = {
                CedantId: this.ClaimDataBatch.CedantId,
                TreatyId: this.ClaimDataBatch.TreatyId,
                Quarter: this.ClaimDataBatch.Quarter,
            };

            var soaDataBatchId = null;
            var result = null;
            var message = null;
            $.ajax({
                url: CreateSoaDataBatchUrl,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    result = data.success;
                    soaDataBatchId = data.resultId;
                    message = data.message;
                }
            });

            if (result == false) {
                this.MatchSoaDataBatchesValidation.push(message);
                return;
            }

            $('#SoaDataBatchId').val(soaDataBatchId);
            this.ClaimDataBatch.SoaDataBatchId = soaDataBatchId;
            refreshSoaDataMatchInput();

            $('#matchSoaDataModal').modal('toggle');
        },
        viewSoaDataBatch: function () {
            var url = ViewSoaDataBatchUrl + '/' + this.ClaimDataBatch.SoaDataBatchId;
            return window.open(url, "_blank");
        },
    },
    mounted: function () {
        var includedFiles = [];
        this.ClaimDataFiles.forEach(
            function (claimDataFile, index) {
                if (claimDataFile.Mode == ModeExclude) {
                    $('#claimDataFileExclude_' + index).prop("checked", true);
                } else {
                    includedFiles.push(claimDataFile.Id);
                }
            }
        )
        this.ClaimDataBatch.IncludedFiles = includedFiles;
    },
})