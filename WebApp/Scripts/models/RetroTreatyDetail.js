$(document).ready(function () {
    $('#Remark').autoResize();
    $('#Remark').trigger('keyup');
});

var app = new Vue({
    el: '#app',
    data: {
        RetroTreatyDetailModel: RetroTreatyDetailModel,
        StandardRetroOutputList: StandardRetroOutputList,
        OtherOutputList: OtherOutputList,
        // Eval
        CurrentEvalMsg: [],
        CurrentEvalSuccess: true,
        CurrentEvalSingle: {
            Formula: "",
            StandardRetroOutputBos: [],
            ShowResult: false,
            Result: [],
            Errors: [],
            Success: true,
        },
    },
    methods: {
        eval: function () {
            var success = false;
            var messages = [];

            var formulas = {
                'Gross Retro Premium': this.RetroTreatyDetailModel.GrossRetroPremium,
                'Treaty Discount': this.RetroTreatyDetailModel.TreatyDiscount,
                'Net Retro Premium': this.RetroTreatyDetailModel.NetRetroPremium,
            }

            var obj = {
                'formulas': formulas,
                'mlreShareStr': this.RetroTreatyDetailModel.MlreShareStr,
                'premiumSpreadTableId': this.RetroTreatyDetailModel.PremiumSpreadTableId,
                'treatyDiscountTableId': this.RetroTreatyDetailModel.TreatyDiscountTableId,
            }
            var url = EvaluateFormulasUrl;

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
        openSingleEval: function (formula) {
            this.CurrentEvalSingle.Formula = formula;

            var standardRetroOutputBos = [];
            var errors = [];
            var success = false;
            $.ajax({
                url: GetEvaluateVariableUrl,
                type: "POST",
                dataType: 'json',
                data: { script: formula, mlreShareStr: this.RetroTreatyDetailModel.MlreShareStr },
                cache: false,
                async: false,
                success: function (data) {
                    success = data.success;
                    if (data.success) {
                        standardRetroOutputBos = data.standardRetroOutputBos;
                    } else {
                        errors = data.errors;
                    }
                }
            });
            this.CurrentEvalSingle.StandardRetroOutputBos = standardRetroOutputBos;
            this.CurrentEvalSingle.ShowResult = !success ? true : false;
            this.CurrentEvalSingle.Result = [];
            this.CurrentEvalSingle.Errors = errors;
            this.CurrentEvalSingle.Success = success;

            $('#evaluationModal').modal('show');
        },
        evalSingle: function () {
            var obj = {
                formula: this.CurrentEvalSingle.Formula,
                standardRetroOutputBos: this.CurrentEvalSingle.StandardRetroOutputBos
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
        openDatePicker: function (index) {
            var standardOutputBo = this.CurrentEvalSingle.StandardRetroOutputBos[index];
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
            this.CurrentEvalSingle.StandardRetroOutputBos[index].DummyValue = value;
        },
        getPlaceHolder: function (dataType) {
            if (dataType == DataTypeDate)
                return "DD MM YYYY";

            return "Type Here";
        }
    }
});