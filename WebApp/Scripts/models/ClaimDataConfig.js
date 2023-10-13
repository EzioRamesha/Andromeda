function confirmImportConfig() {
    var filename = $('#importFile')[0].files[0].name;
    $('#fileName').html('<strong>' + filename + '</strong>');
    $('#importConfigModal').modal('show');
}

function changeFileType(fileType) {
    $('#delimiterDiv').hide();
    $('#worksheetDiv').hide();

    if (fileType == FileTypePlainText) {
        $('#delimiterDiv').show();
    } else if (fileType == FileTypeExcel) {
        $('#worksheetDiv').show();
    }
}

function initTooltip() {
    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });
}

function openInfoModal() {
    $('#infoTableInput').val('');
    searchStandardOutput();
}

$(document).ready(function () {
    dateOffAutoComplete();

    var cedantId = $('#CedantId').val();
    var fileType = $('#fileTypeSelect').val();

    if (cedantId != null)
        getTreaty(cedantId);

    if (fileType != null)
        changeFileType(fileType);

    $('input').on('keypress', function (evt) {
        var evt = (evt) ? evt : ((event) ? event : null);
        if (evt.keyCode == 13)
            return false;
    });

    $("#importConfigModal").on('hidden.bs.modal', function () {
        $('#importFile').val('');
    });
});

function getTreaty(cedantId) {
    var treatyId = isNaN(TreatyId) ? $('#TreatyId').val() : TreatyId;

    $('#TreatyId option').remove();
    $('#TreatyId').append(new Option("Please select", null));

    $.ajax({
        url: GetTreatyUrl,
        type: "POST",
        data: { cedantId: cedantId },
        cache: false,
        async: false,
        success: function (data) {
            var treatyList = data.treatyBos;
            refreshDropDownItems('TreatyId', treatyList, treatyId, 'TreatyIdCode', 'Description', false)
        }
    });

    getTreatyCode($('#TreatyId').val());
}

function getTreatyCode(treatyId) {
    var treatyCodes = [];

    $.ajax({
        url: GetTreatyCodeUrl,
        type: "POST",
        data: { treatyId: treatyId },
        cache: false,
        async: false,
        success: function (data) {
            treatyCodes = data.TreatyCodes;
        }
    });

    app.TreatyCodeBos = treatyCodes;
}

function updateStatus(status) {
    $('#configStatus').val(status);
}

var app = new Vue({
    el: '#app',
    data: {
        // Models
        ClaimDataConfig: ClaimDataConfigModel,
        Mappings: ClaimDataMappings,
        Computations: ClaimDataComputations,
        PreValidations: ClaimDataPreValidations,
        PostComputations: ClaimDataPostComputations,
        PostValidations: ClaimDataPostValidations,
        StatusHistories: StatusHistories,
        Remarks: Remarks,
        // Indexes
        MappingMaxIndex: 0,
        ComputationMaxIndex: 0,
        PreValidationMaxIndex: 0,
        PostComputationMaxIndex: 0,
        PostValidationMaxIndex: 0,
        // List items from Backend
        StandardOutputList: StandardOutputList,
        StandardClaimDataOutputList: StandardClaimDataOutputList,
        TransformFormulaList: TransformFormulaList,
        InputRequiredTransformFormulas: InputRequiredTransformFormulas,
        BenefitBos: BenefitBos,
        TreatyCodeBos: [],
        ComputationModeList: ComputationModeList,
        ComputationTableList: ComputationTableList,
        // Mapping Detail
        MappingDetails: [],
        CurrentMappingDetailIndex: 0,
        PickListDetailList: [],
        NullRawValue: null,
        // Status
        StatusRemark: "",
        RemarkModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            Status: StatusDraft ? StatusDraft : null,
            CreatedAtStr: null,
            Content: null,
        },
        // Eval
        CurrentEvalMsg: [],
        CurrentEvalSuccess: true,
        CurrentEvalSingle: {
            Condition: "",
            Formula: "",
            StandardOutputBos: [],
            ShowResult: false,
            Result: [],
            Errors: [],
            Success: true,
            EnableRiData: false,
            EnableOriginal: false,
        },
        // Others
        Disabled: Disabled,
        textAreaWidth: 150,
        textAreaHeight: 21,
        currentExpandedKey: null,
    },
    methods: {
        // Add Remove
        addMapping: function () {
            this.MappingMaxIndex++;
            this.Mappings.push({ SortIndex: this.MappingMaxIndex, ClaimDataMappingDetailBos: [], StandardClaimDataOutputId: null, TransformFormula: 0, Row: 0 });

            initTooltip();
            this.toggleInputValue(this.MappingMaxIndex);
        },
        removeMapping: function (index) {
            this.Mappings.splice(index, 1);
            this.MappingMaxIndex--;

            this.Mappings.forEach(
                function (mapping, mappingIndex) {
                    var id = "#mappingStdOutput_" + mappingIndex;
                    $(id).val(mapping.StandardClaimDataOutputId);
                    $(id).trigger("chosen:updated");
                }
            );
        },
        addMappingDetail: function () {
            this.MappingDetails.push({ IsRawValueEmpty: false, PickListDetailId: null, IsPickDetailIdEmpty: false });
        },
        removeMappingDetail: function (index) {
            if (this.NullRawValue != null && this.NullRawValue == index) {
                this.NullRawValue = null;
            }
            this.MappingDetails.splice(index, 1);
        },
        addComputation: function () {
            this.ComputationMaxIndex++;
            var sortIndex = (this.Computations.length ? Math.max.apply(Math, this.Computations.map(function (c) { return c.SortIndex; })) : 0) + 1;
            this.Computations.push({ SortIndex: sortIndex, StandardClaimDataOutputId: null, Mode: 0, Step: 1 });
        },
        removeComputation: function (index) {
            this.Computations.splice(index, 1);
            this.ComputationMaxIndex--;

            this.Computations.forEach(
                function (computation, computationIndex) {
                    var id = "#preComStdOutput_" + computationIndex;
                    $(id).val(computation.StandardClaimDataOutputId);
                    $(id).trigger("chosen:updated");
                }
            );
        },
        addPreValidation: function () {
            this.PreValidationMaxIndex++;
            this.PreValidations.push({ SortIndex: this.PreValidationMaxIndex, Step: 1 });
        },
        removePreValidation: function (index) {
            this.PreValidations.splice(index, 1);
            this.PreValidationMaxIndex--;
        },
        addPostComputation: function () {
            this.PostComputationMaxIndex++;
            var sortIndex = (this.PostComputations.length ? Math.max.apply(Math, this.PostComputations.map(function (c) { return c.SortIndex; })) : 0) + 1;
            this.PostComputations.push({ SortIndex: sortIndex, StandardClaimDataOutputId: null, Mode: 0, Step: 2 });
        },
        removePostComputation: function (index) {
            this.PostComputations.splice(index, 1);
            this.PostComputationMaxIndex--;

            this.PostComputations.forEach(
                function (computation, computationIndex) {
                    var id = "#postComStdOutput_" + computationIndex;
                    $(id).val(computation.StandardClaimDataOutputId);
                    $(id).trigger("chosen:updated");
                }
            );
        },
        addPostValidation: function () {
            this.PostValidationMaxIndex++;
            this.PostValidations.push({ SortIndex: this.PostValidationMaxIndex, Step: 2 });
        },
        removePostValidation: function (index) {
            this.PostValidations.splice(index, 1);
            this.PostValidationMaxIndex--;
        },
        // Mapping Detail 
        openDropDownMapping: function (index) {
            this.CurrentMappingDetailIndex = index;
            this.MappingDetails = this.Mappings[index].ClaimDataMappingDetailBos.slice();
            var stdOutputId = this.Mappings[index].StandardClaimDataOutputId;
            var list = [];

            var nullRawValue = null;
            this.MappingDetails.forEach(
                function (detail, detailIndex) {
                    if (detail.IsRawValueEmpty == true) {
                        nullRawValue = detailIndex;
                    }
                }
            )
            this.NullRawValue = nullRawValue;

            $.ajax({
                url: GetPickListDetailUrl,
                type: "POST",
                data: { standardClaimDataOutputId: stdOutputId },
                cache: false,
                async: false,
                success: function (data) {
                    list = data.pickListDetailBos;
                }
            });
            this.PickListDetailList = list;
        },
        saveMappingDetail: function () {
            var currentRawDataMapping = this.Mappings[this.CurrentMappingDetailIndex];
            currentRawDataMapping.DetailMaxIndex = this.MappingDetails.length - 1;
            currentRawDataMapping.ClaimDataMappingDetailBos = this.MappingDetails.slice();

            this.MappingDetails = [];
            this.CurrentMappingDetailIndex = 0;
        },
        resetRawValue: function (index) {
            this.MappingDetails[index].RawValue = null;

            if (this.NullRawValue == null)
                this.NullRawValue = index;
            else
                this.NullRawValue = null;
        },
        resetPickListDetailId: function (index) {
            this.MappingDetails[index].PickListDetailId = null;
        },
        // Computation
        changeComputationMode: function (index) {
            var claimDataComputation = this.Computations[index];
            claimDataComputation.CalculationFormula = null;
        },
        changePostComputationMode: function (index) {
            var claimDataPostComputation = this.PostComputations[index];
            claimDataPostComputation.CalculationFormula = null;
        },
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.Status = this.ClaimDataConfig.Status == 0 ? StatusDraft : this.ClaimDataConfig.Status;
            this.RemarkModal.ModuleId = this.ClaimDataConfig.ModuleId;
            this.RemarkModal.ObjectId = this.ClaimDataConfig.Id;
            this.RemarkModal.Content = null;
        },
        addRemark: function () {
            var remark = null;
            if (!this.ClaimDataConfig.Id) {
                remark = this.RemarkModal;
                remark.StatusName = StatusDraftName;
            } else {
                remark = createRemark(this.RemarkModal);
            }

            if (remark) {
                this.Remarks.unshift(Object.assign({}, remark));
            }
        },
        // Eval
        eval: function (type) {
            var success = false;
            var messages = [];

            var obj = null;
            var url = EvaluateObjectsUrl;
            if (type == "PV") {
                obj = { 'bos': this.PreValidations, 'enableRiData': false, 'enableOriginal': false };
            } else if (type == "C") {
                obj = { 'bos': this.Computations, 'enableRiData': false, 'enableOriginal': false };
            } else if (type == "PoC") {
                obj = { 'bos': this.PostComputations, 'enableRiData': true, 'enableOriginal': true };
            } else if (type == "PoV") {
                obj = { 'bos': this.PostValidations, 'enableRiData': true, 'enableOriginal': true };
            } else {
                return;
            }

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                url: url,
                type: "POST",
                //dataType: 'json',
                data: JSON.stringify(obj),
                cache: false,
                async: false,
                success: function (data) {
                    if (data.success) {
                        messages = ["Test Successful"];
                        success = true;
                    } else {
                        messages = data.errors;
                    }
                },
                error: function () {
                    messages = ["Error: Evaluation did not manage to execute."];
                }
            });

            this.CurrentEvalMsg = messages;
            this.CurrentEvalSuccess = success;
            $('#evaluationResultModal').modal('show');
        },
        openSingleEval: function (obj, enableRiData, enableOriginal) {
            this.CurrentEvalSingle.Condition = obj.Condition;
            if (obj.CalculationFormula) {
                this.CurrentEvalSingle.Formula = obj.CalculationFormula;
            } else {
                this.CurrentEvalSingle.Formula = "";
            }

            var script = this.CurrentEvalSingle.Condition + this.CurrentEvalSingle.Formula;

            var standardOutputBos = [];
            var errors = [];
            var success = false;
            $.ajax({
                url: GetEvaluateVariableUrl,
                type: "POST",
                dataType: 'json',
                data: { script: script, enableRiData: enableRiData, enableOriginal: enableOriginal },
                cache: false,
                async: false,
                success: function (data) {
                    success = data.success;
                    if (data.success) {
                        standardOutputBos = data.standardClaimDataOutputBos;
                    } else {
                        errors = data.errors;
                    }
                }
            });
            this.CurrentEvalSingle.StandardOutputBos = standardOutputBos;
            this.CurrentEvalSingle.ShowResult = !success ? true : false;
            this.CurrentEvalSingle.Result = [];
            this.CurrentEvalSingle.Errors = errors;
            this.CurrentEvalSingle.Success = success;
            this.CurrentEvalSingle.EnableRiData = enableRiData;
            this.CurrentEvalSingle.EnableOriginal = enableOriginal;

            $('#evaluationModal').modal('show');
        },
        evalSingle: function () {
            var obj = {
                condition: this.CurrentEvalSingle.Condition,
                formula: this.CurrentEvalSingle.Formula,
                bos: this.CurrentEvalSingle.StandardOutputBos,
                enableRiData: this.CurrentEvalSingle.EnableRiData,
                enableOriginal: this.CurrentEvalSingle.EnableOriginal
            }

            var result = [];
            var errors = [];
            var success = false;
            $.ajax({
                url: EvaluateSingleUrl,
                type: "POST",
                dataType: 'json',
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    success = data.success;
                    if (data.success) {
                        for (var title in data.result) {
                            result.push({ title: title, value: data.result[title] });
                        }
                    } else {
                        errors = data.errors;
                    }
                }
            });
            this.CurrentEvalSingle.ShowResult = true;
            this.CurrentEvalSingle.Result = result;
            this.CurrentEvalSingle.Errors = errors;
            this.CurrentEvalSingle.Success = success;
        },
        // Misc
        toggleInputValue: function (index) {
            var standardOutputId = this.Mappings[index].StandardClaimDataOutputId;
            var transformFormula = this.Mappings[index].TransformFormula;
            
            if (standardOutputId) {
                var dataType = this.StandardClaimDataOutputList[this.indexOfStandardOutput(standardOutputId)].DataType;
                this.Mappings[index].DefaultObjectId = null;
                
                if (transformFormula == 0) {
                    this.Mappings[index].DefaultValueType = null;
                } else if (transformFormula == InputTableKey) {
                    this.Mappings[index].DefaultValueType = DefaultValueMappingDetail;
                } else if (transformFormula && dataType == DataTypeDropDown) {
                    this.Mappings[index].DefaultValueType = DefaultValueDropDown;
                    var list = [];
                    $.ajax({
                        url: GetPickListDetailUrl,
                        type: "POST",
                        data: { standardClaimDataOutputId: standardOutputId },
                        cache: false,
                        async: false,
                        success: function (data) {
                            list = data.pickListDetailBos;
                        }
                    });
                    this.Mappings[index].DefaultObjectList = list;
                } else if (transformFormula == FixedValueKey && (standardOutputId == MlreBenefitCodeKey || standardOutputId == TreatyCodeKey)) {
                    this.Mappings[index].DefaultValueType = DefaultValueDropDown;
                } else {
                    this.Mappings[index].DefaultValueType = DefaultValueString;
                }
            }

            this.toggleTransformFormula(index);
        },
        toggleTransformFormula: function (index) {
            var transformFormula = this.Mappings[index].TransformFormula;
            var standardOutputId = this.Mappings[index].StandardClaimDataOutputId;

            this.Mappings[index].ShowTransformFormula = false;
            this.Mappings[index].DisableInputValueFormula = true;

            if (standardOutputId) {
                this.Mappings[index].ShowTransformFormula = true;
            }

            if (!transformFormula || !this.InputRequiredTransformFormulas.includes(transformFormula) || this.Mappings[index].DefaultValueType != DefaultValueString) {
                this.Mappings[index].DefaultValue = '';
            } else {
                this.Mappings[index].DisableInputValueFormula = false;
            }
        },
        indexOfStandardOutput: function (standardOutputId) {
            standardOutputId = parseInt(standardOutputId);
            return this.StandardClaimDataOutputList.map(function (x) { return x.Id; }).indexOf(standardOutputId);
        },
        changeStandardOutput: function (id, standardOutputId) {
            var index = (id.split("_"))[1];

            if (id.includes("mapping")) {
                this.Mappings[index].StandardClaimDataOutputId = standardOutputId;
                this.toggleInputValue(index);
            } else if (id.includes("preCom")) {
                this.Computations[index].StandardClaimDataOutputId = standardOutputId;
            } else if (id.includes("postCom")) {
                this.PostComputations[index].StandardClaimDataOutputId = standardOutputId;
            }
        },
        autoExpandTextarea: function (id) {
            var key = (id.split('_'))[0];
            if (this.currentExpandedKey != null) {
                if (key != this.currentExpandedKey) {
                    this.collapseTextareaGroup(this.currentExpandedKey);
                    this.currentExpandedKey = key;
                }
            } else {
                this.currentExpandedKey = key;
            }

            if (key.startsWith('RawColumnName')) {
                this.expandTextAreaId(id);
            } else {
                this.expandTextAreaId(key + '_' + 'Description');
                this.expandTextAreaId(key + '_' + 'Condition');
                if (key.startsWith('PreCom') || key.startsWith('PostCom')) {
                    this.expandTextAreaId(key + '_' + 'Formula');
                }
            }
        },
        expandTextAreaId: function (id) {
            var tArea = $('#' + id);

            if (tArea.height() > this.textAreaHeight)
                return;

            this.textAreaWidth = tArea.width();
            //this.textAreaHeight = tArea.height();
            tArea.autoResize();
            tArea.trigger('keyup');

            tArea.on('keypress', function (evt) {
                var evt = (evt) ? evt : ((event) ? event : null);
                if (evt.keyCode == 13)
                    return false;
            });
        },
        collapseTextareaGroup: function (key) {
            $('#' + key + '_' + 'Description').height(this.textAreaHeight);
            $('#' + key + '_' + 'Condition').height(this.textAreaHeight);
            if (key.startsWith('PreCom') || key.startsWith('PostCom')) {
                $('#' + key + '_' + 'Formula').height(this.textAreaHeight);
            }
        },
        autoCollapseTextarea: function (id) {
            var tArea = $('#' + id);
            tArea.height(this.textAreaHeight);
        },
        changeDelimiter: function () {
            this.ClaimDataConfig.Delimiter = $('#delimiterSelect').val();
        },
        expandTextarea: function (onload = false) {
            if (this.Disabled == 'True') {
                var expandElementHeight = this.expandElementHeight;
                if (!onload) {
                    $('.nav-tabs a').on('shown.bs.tab', function () {
                        expandElementHeight('.textarea-auto-expand');
                    });
                } else {
                    expandElementHeight('.textarea-auto-expand');
                }
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
        openDatePicker: function (index) {
            var standardOutputBo = this.CurrentEvalSingle.StandardOutputBos[index];
            if (standardOutputBo.DataType != DataTypeDate)
                return;

            var id = "#" + standardOutputBo.Code + "_" + index;
            if ($(id).data("datepicker") != null) {
                $(id).datepicker("destroy");
            }

            $(id).datepicker({
                format: DateFormatDatePickerJs,
                autoclose: true,
            });

            var changeDummyValue = this.changeDummyValue;
            $(id).on('changeDate', function () {
                changeDummyValue($(id).val(), index);
            });

            $('#evaluationModal').on('hidden.bs.modal', function () {
                if ($(id).data("datepicker") != null) {
                    $(id).datepicker("destroy");
                }
            })

            $(id).focus();
        },
        changeDummyValue: function (value, index) {
            this.CurrentEvalSingle.StandardOutputBos[index].DummyValue = value;
        },
        getPlaceHolder: function (dataType) {
            if (dataType == DataTypeDate)
                return "DD MM YYYY";

            return "Type Here";
        }
    },
    created: function () {
        initTooltip();

        this.MappingMaxIndex = (this.Mappings) ? this.Mappings.length - 1 : -1;
        this.ComputationMaxIndex = (this.Computations) ? this.Computations.length - 1 : -1;
        this.PreValidationMaxIndex = (this.PreValidations) ? this.PreValidations.length - 1 : -1;
        this.PostComputationMaxIndex = (this.PostComputations) ? this.PostComputations.length - 1 : -1;
        this.PostValidationMaxIndex = (this.PostValidations) ? this.PostValidations.length - 1 : -1;

        var toggleTransformFormula = this.toggleTransformFormula;
        this.Mappings.forEach(
            function (claimDataMapping, index) {
                claimDataMapping.DetailMaxIndex = (claimDataMapping.ClaimDataMappingDetailBos) ? claimDataMapping.ClaimDataMappingDetailBos.length - 1 : -1;
                toggleTransformFormula(index);
            }
        )
    },
    mounted: function () {
        $('.chosen').chosen({
            width: '100%'
        });

        var changeStandardOutput = this.changeStandardOutput;
        $('.chosen').on('change', function (evt, params) {
            var id = this.id;
            changeStandardOutput(id, params.selected);
        });

        this.$nextTick(function () {
            this.expandTextarea(true);
        })
    },
    updated() {
        $('.chosen').chosen({
            width: '100%'
        });

        var changeStandardOutput = this.changeStandardOutput;
        $('.chosen').on('change', function (evt, params) {
            var id = this.id;
            changeStandardOutput(id, params.selected);
        });
    },
});