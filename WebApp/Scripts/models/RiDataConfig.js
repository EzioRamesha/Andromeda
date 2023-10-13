function confirmImportConfig() {
    var filename = $('#importFile')[0].files[0].name;
    $('#fileName').html('<strong>' + filename + '</strong>');
    $('#importConfigModal').modal('show');
}

function initTooltip() {
    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });
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

$(document).ready(function () {
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

    $("#loadingSpinner").addClass('hide-loading-spinner');
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

    app.RiDataConfig.CedantId = cedantId;

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

var app = new Vue({
    el: '#app',
    data: {
        // Models
        RiDataConfig: RiDataConfigModel,
        RawDataMappings: RawDataMappings,
        PreComputation1: PreComputation1,
        PreComputation2: PreComputation2,
        PreValidations: PreValidations,
        PostComputations: PostComputations,
        PostValidations: PostValidations,
        StatusHistories: StatusHistories,
        Remarks: Remarks,
        // Indexes
        RawDataMappingMaxIndex: 0,
        PreComputation1MaxIndex: 0,
        PreComputation2MaxIndex: 0,
        PreValidationMaxIndex: 0,
        PostComputationMaxIndex: 0,
        PostValidationMaxIndex: 0,
        // List items from Backend
        StandardOutputList: StandardOutputList,
        TransformFormulaList: TransformFormulaList,
        ComputationModeList: ComputationModeList,
        ComputationTableList: ComputationTableList,
        ComputationRiskDateList: ComputationRiskDateList,
        StatusList: StatusList,
        InputRequiredTransformFormulas: InputRequiredTransformFormulas,
        DelimiterItems: DelimiterItems,
        BenefitBos: BenefitBos,
        TreatyCodeBos: [],
        // Mapping Detail
        MappingDetails: [],
        CurrentMappingDetailIndex: 0,
        PickListDetailList: [],
        NullRawValue: null,
        // Status
        StatusRemark: '',
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
            HasQuarter: false,
            HasOriQuarter: false,
            Quarter: "",
            OriQuarter: "",
            ShowResult: false,
            Result: [],
            Errors: [],
            Success: true,
            EnableOriginal: false
        },
        // Others
        Disabled: Disabled,
        textAreaWidth: 150,
        textAreaHeight: 18,
        currentExpandedKey: null
    },
    methods: {
        // Add Remove
        addRawDataMapping: function () {
            this.RawDataMappingMaxIndex++;
            this.RawDataMappings.push({ SortIndex: this.RawDataMappingMaxIndex, RiDataMappingDetailBos: [], StandardOutputId: null, TransformFormula: 0, Row: 0 });

            this.toggleInputValue(this.RawDataMappingMaxIndex);
            initTooltip();
        },
        removeRawDataMapping: function (index) {
            this.RawDataMappings.splice(index, 1);
            this.RawDataMappingMaxIndex--;

            this.RawDataMappings.forEach(
                function (rawDataMapping, mappingIndex) {
                    var id = "#mappingStdOutput_" + mappingIndex;
                    $(id).val(rawDataMapping.StandardOutputId);
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
        addNewPreComputation1: function () {
            this.PreComputation1MaxIndex++;
            var sortIndex = (this.PreComputation1.length ? Math.max.apply(Math, this.PreComputation1.map(function (c) { return c.SortIndex; })) : 0) + 1;
            this.PreComputation1.push({ SortIndex: sortIndex, StandardOutputId: null, Mode: 0, Step: 1 });
        },
        removePreComputation1: function (index) {
            this.PreComputation1.splice(index, 1);
            this.PreComputation1MaxIndex--;

            this.PreComputation1.forEach(
                function (computation, computationIndex) {
                    var id = "#preCom1StdOutput_" + computationIndex;
                    $(id).val(computation.StandardOutputId);
                    $(id).trigger("chosen:updated");

                    var id = "#preCom1Table_" + computationIndex;
                    $(id).val(computation.CalculationFormula);
                    $(id).trigger("chosen:updated");
                }
            );
        },
        addNewPreComputation2: function () {
            this.PreComputation2MaxIndex++;
            var sortIndex = (this.PreComputation2.length ? Math.max.apply(Math, this.PreComputation2.map(function (c) { return c.SortIndex; })) : 0) + 1;
            this.PreComputation2.push({ SortIndex: sortIndex, StandardOutputId: null, Mode: 0, Step: 2 });
        },
        removePreComputation2: function (index) {
            this.PreComputation2.splice(index, 1);
            this.PreComputation2MaxIndex--;

            this.PreComputation2.forEach(
                function (computation, computationIndex) {
                    var id = "#preCom2StdOutput_" + computationIndex;
                    $(id).val(computation.StandardOutputId);
                    $(id).trigger("chosen:updated");

                    var id = "#preCom2Table_" + computationIndex;
                    $(id).val(computation.CalculationFormula);
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
        addNewPostComputation: function () {
            this.PostComputationMaxIndex++;
            var sortIndex = (this.PostComputations.length ? Math.max.apply(Math, this.PostComputations.map(function (c) { return c.SortIndex; })) : 0) + 1;
            this.PostComputations.push({ SortIndex: sortIndex, StandardOutputId: null, Mode: 0, Step: 3 });
        },
        removePostComputation: function (index) {
            this.PostComputations.splice(index, 1);
            this.PostComputationMaxIndex--;

            this.PostComputations.forEach(
                function (computation, computationIndex) {
                    var id = "#postComStdOutput_" + computationIndex;
                    $(id).val(computation.StandardOutputId);
                    $(id).trigger("chosen:updated");

                    var id = "#postComTable_" + computationIndex;
                    $(id).val(computation.CalculationFormula);
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
            this.MappingDetails = this.RawDataMappings[index].RiDataMappingDetailBos.slice();
            var stdOutputId = this.RawDataMappings[index].StandardOutputId;
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
                data: { standardOutputId: stdOutputId },
                cache: false,
                async: false,
                success: function (data) {
                    list = data.pickListDetailBos;
                }
            });
            this.PickListDetailList = list;
        },
        saveMappingDetail: function () {
            var currentRawDataMapping = this.RawDataMappings[this.CurrentMappingDetailIndex];
            currentRawDataMapping.DetailMaxIndex = this.MappingDetails.length - 1;
            currentRawDataMapping.RiDataMappingDetailBos = this.MappingDetails.slice();

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
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.Status = this.RiDataConfig.Status == 0 ? StatusDraft : this.RiDataConfig.Status;
            this.RemarkModal.ModuleId = this.RiDataConfig.ModuleId;
            this.RemarkModal.ObjectId = this.RiDataConfig.Id;
            this.RemarkModal.Content = null;
        },
        addRemark: function () {
            var remark = null;
            if (!this.RiDataConfig.Id) {
                remark = this.RemarkModal;
                remark.StatusName = StatusDraftName;
            } else {
                var remark = createRemark(this.RemarkModal);
            }

            if (remark) {
                this.Remarks.unshift(Object.assign({}, remark));
                this.RemarkMaxIndex++;
            }
        },
        // Computation
        changePreComputation1Mode: function (index) {
            var riDataComputation = this.PreComputation1[index];
            riDataComputation.CalculationFormula = "";
            var id = "#preCom1StdOutput_" + index;
            var chosenId = id + "_chosen";
            this.$nextTick(function () {
                $(id).trigger("chosen:updated");
            })
            if ($(chosenId).hasClass(".chosen-disabled")) {
                $(chosenId).removeClass(".chosen-disabled").addClass(".chosen-with-drop .chosen-container-active");
            }
        },
        changePreComputation2Mode: function (index) {
            var riDataComputation = this.PreComputation2[index];
            riDataComputation.CalculationFormula = "";
            var id = "#preCom2StdOutput_" + index;
            var chosenId = id + "_chosen";
            this.$nextTick(function () {
                $(id).trigger("chosen:updated");
            })
            if ($(chosenId).hasClass(".chosen-disabled")) {
                $(chosenId).removeClass(".chosen-disabled").addClass(".chosen-with-drop .chosen-container-active");
            }
        },
        changePostComputationMode: function (index) {
            var riDataPostComputation = this.PostComputations[index];
            riDataPostComputation.CalculationFormula = "";
            var id = "#postComStdOutput_" + index;
            var chosenId = id + "_chosen";
            this.$nextTick(function () {
                $(id).trigger("chosen:updated");
            })
            if ($(chosenId).hasClass(".chosen-disabled")) {
                $(chosenId).removeClass(".chosen-disabled").addClass(".chosen-with-drop .chosen-container-active");
            }
        },
        // Eval
        eval: function (type) {
            var success = false;
            var messages = [];

            var obj = null;
            var url = EvaluateObjectsUrl;
            if (type == "PV") {
                obj = { 'bos': this.PreValidations, 'enableOriginal': true };
            } else if (type == "PC1") {
                obj = { 'bos': this.PreComputation1, 'enableOriginal': false };
            } else if (type == "PC2") {
                obj = { 'bos': this.PreComputation2, 'enableOriginal': true };
            } else if (type == "PoC") {
                obj = { 'bos': this.PostComputations, 'enableOriginal': true };
            } else if (type == "PoV") {
                obj = { 'bos': this.PostValidations, 'enableOriginal': true };
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
        openSingleEval: function (obj, enableOriginal) {
            this.CurrentEvalSingle.Condition = obj.Condition;
            if (obj.Mode == ComputationModeFormula) {
                this.CurrentEvalSingle.Formula = obj.CalculationFormula;
            } else {
                this.CurrentEvalSingle.Formula = "";
            }

            var script = this.CurrentEvalSingle.Condition + this.CurrentEvalSingle.Formula;

            var standardOutputBos = [];
            var errors = [];
            var success = false;
            var hasQuarter = false;
            var hasOriQuarter = false;
            $.ajax({
                url: GetEvaluateVariableUrl,
                type: "POST",
                dataType: 'json',
                data: { script: script, enableOriginal: enableOriginal },
                cache: false,
                async: false,
                success: function (data) {
                    success = data.success;
                    if (data.success) {
                        standardOutputBos = data.standardOutputBos;
                        hasQuarter = data.hasQuarter;
                        hasOriQuarter = data.hasOriQuarter;
                    } else {
                        errors = data.errors;
                    }
                }
            });
            this.CurrentEvalSingle.StandardOutputBos = standardOutputBos;
            this.CurrentEvalSingle.Quarter = "";
            this.CurrentEvalSingle.OriQuarter = "";
            this.CurrentEvalSingle.HasQuarter = hasQuarter;
            this.CurrentEvalSingle.HasOriQuarter = hasOriQuarter;
            this.CurrentEvalSingle.ShowResult = !success ? true : false;
            this.CurrentEvalSingle.Result = [];
            this.CurrentEvalSingle.Errors = errors;
            this.CurrentEvalSingle.Success = success;
            this.CurrentEvalSingle.EnableOriginal = enableOriginal;

            $('#evaluationModal').modal('show');
        },
        evalSingle: function () {
            var obj = {
                condition: this.CurrentEvalSingle.Condition,
                formula: this.CurrentEvalSingle.Formula,
                standardOutputBos: this.CurrentEvalSingle.StandardOutputBos,
                quarter: this.CurrentEvalSingle.HasQuarter ? this.CurrentEvalSingle.Quarter : null,
                oriQuarter: this.CurrentEvalSingle.HasOriQuarter ? this.CurrentEvalSingle.OriQuarter : null,
                enableOriginal: this.CurrentEvalSingle.EnableOriginal,
            };

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
            var standardOutputId = this.RawDataMappings[index].StandardOutputId;
            var transformFormula = this.RawDataMappings[index].TransformFormula;

            if (standardOutputId && standardOutputId != CustomFieldKey) {
                var dataType = this.StandardOutputList[this.indexOfStandardOutput(standardOutputId)].DataType;
                this.RawDataMappings[index].DefaultObjectId = null;

                if (transformFormula == 0) {
                    this.RawDataMappings[index].DefaultValueType = null;
                } else if (transformFormula == InputTableKey) {
                    this.RawDataMappings[index].DefaultValueType = DefaultValueMappingDetail;
                } else if (transformFormula == FixedValueKey && dataType == DataTypeBoolean) {
                    this.RawDataMappings[index].DefaultValueType = DefaultValueBoolean;
                } else if (transformFormula && dataType == DataTypeDropDown) {
                    this.RawDataMappings[index].DefaultValueType = DefaultValueDropDown;
                    var list = [];
                    $.ajax({
                        url: GetPickListDetailUrl,
                        type: "POST",
                        data: { standardOutputId: standardOutputId },
                        cache: false,
                        async: false,
                        success: function (data) {
                            list = data.pickListDetailBos;
                        }
                    });
                    this.RawDataMappings[index].DefaultObjectList = list;
                } else if (transformFormula == FixedValueKey && (standardOutputId == MlreBenefitCodeKey || standardOutputId == TreatyCodeKey)) {
                    this.RawDataMappings[index].DefaultValueType = DefaultValueDropDown;
                } else {
                    this.RawDataMappings[index].DefaultValueType = DefaultValueString;
                }
            }

            this.toggleTransformFormula(index);
        },
        toggleTransformFormula: function (index) {
            var transformFormula = this.RawDataMappings[index].TransformFormula;
            var standardOutputId = this.RawDataMappings[index].StandardOutputId;

            this.RawDataMappings[index].ShowTransformFormula = false;
            this.RawDataMappings[index].DisableInputValueFormula = true;

            if (standardOutputId && standardOutputId != CustomFieldKey) {
                this.RawDataMappings[index].ShowTransformFormula = true;
            }

            if (!transformFormula || !this.InputRequiredTransformFormulas.includes(transformFormula) || (this.RawDataMappings[index].DefaultValueType != DefaultValueString && this.RawDataMappings[index].DefaultValueType != DefaultValueBoolean)) {
                this.RawDataMappings[index].DefaultValue = '';
            } else {
                this.RawDataMappings[index].DisableInputValueFormula = false;
            }
        },
        indexOfStandardOutput: function (standardOutputId) {
            standardOutputId = parseInt(standardOutputId);
            return this.StandardOutputList.map(function (x) { return x.Id; }).indexOf(standardOutputId);
        },
        changeStandardOutput: function (id, standardOutputId) {
            var index = (id.split("_"))[1];

            if (id.includes("mapping")) {
                this.RawDataMappings[index].StandardOutputId = standardOutputId;
                this.toggleInputValue(index);
            } else if (id.includes("preCom1")) {
                this.PreComputation1[index].StandardOutputId = standardOutputId;
            } else if (id.includes("preCom2")) {
                this.PreComputation2[index].StandardOutputId = standardOutputId;
            } else if (id.includes("postCom")) {
                this.PostComputations[index].StandardOutputId = standardOutputId;
            }
        },
        changeComputationTable: function (id, calculationFormula) {
            var index = (id.split("_"))[1];

            if (id.includes("preCom1")) {
                this.PreComputation1[index].CalculationFormula = calculationFormula;
            } else if (id.includes("preCom2")) {
                this.PreComputation2[index].CalculationFormula = calculationFormula;
            } else if (id.includes("postCom")) {
                this.PostComputations[index].CalculationFormula = calculationFormula;
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
            this.RiDataConfig.Delimiter = $('#delimiterSelect').val();
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

        this.RawDataMappingMaxIndex = (this.RawDataMappings) ? this.RawDataMappings.length - 1 : -1;
        this.PreComputation1MaxIndex = (this.PreComputation1) ? this.PreComputation1.length - 1 : -1;
        this.PreComputation2MaxIndex = (this.PreComputation2) ? this.PreComputation2.length - 1 : -1;
        this.PreValidationMaxIndex = (this.PreValidations) ? this.PreValidations.length - 1 : -1;
        this.PostComputationMaxIndex = (this.PostComputations) ? this.PostComputations.length - 1 : -1;
        this.PostValidationMaxIndex = (this.PostValidations) ? this.PostValidations.length - 1 : -1;

        var toggleTransformFormula = this.toggleTransformFormula;
        this.RawDataMappings.forEach(
            function (rawDataMapping, index) {
                rawDataMapping.DetailMaxIndex = (rawDataMapping.RiDataMappingDetailBos) ? rawDataMapping.RiDataMappingDetailBos.length - 1 : -1;
                toggleTransformFormula(index);
            }
        )
    },
    mounted: function () {
        $('.chosen').chosen({
            width: '100%'
        });

        var changeStandardOutput = this.changeStandardOutput;
        $('.stdOutput.chosen').on('change', function (evt, params) {
            var id = this.id;
            changeStandardOutput(id, params.selected);
        });

        var changeComputationTable = this.changeComputationTable;
        $('.comTable.chosen').on('change', function (evt, params) {
            var id = this.id;
            changeComputationTable(id, params.selected);
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
        $('.stdOutput.chosen').on('change', function (evt, params) {
            var id = this.id;
            changeStandardOutput(id, params.selected);
        });

        var changeComputationTable = this.changeComputationTable;
        $('.comTable.chosen').on('change', function (evt, params) {
            var id = this.id;
            changeComputationTable(id, params.selected);
        });
    },
});