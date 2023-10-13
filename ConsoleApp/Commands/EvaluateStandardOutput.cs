using BusinessObject;
using BusinessObject.RiDatas;
using Shared;
using System;

namespace ConsoleApp.Commands
{
    public class EvaluateStandardOutput : Command
    {
        public EvaluateStandardOutput()
        {
            Title = "EvaluateStandardOutput";
            Description = "To test evalute script for Standard Output Evalute";
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            EvalValue1();
            EvalValue2();
            EvalValue3();
            EvalValue4();
            EvalValue5();
            EvalValue6();
            EvalTranTypeCode();

            PrintEnding();
        }

        public void EvalValue1()
        {
            PrintOutputTitle("EvalValue1");

            var soe = new StandardOutputEval()
            {
                Condition = "{REINS_EFF_DATE_POL}.Date > DateTime.Now.Date",
                Formula = "{NET_PREMIUM} + {STANDARD_PREMIUM} + 9",
                //Formula = "{NET_PREMIUM} * {STANDARD_PREMIUM} * 9",
                //Nullable = true,

                RiDataBo = new RiDataBo
                {
                    ReinsEffDatePol = DateTime.Now.AddMonths(1),
                    NetPremium = 100,
                    //StandardPremium = 2, // to test null value
                },
            };

            PrintResult(soe);
        }

        public void EvalValue2()
        {
            PrintOutputTitle("EvalValue2");

            var soe = new StandardOutputEval()
            {
                Condition = "true",
                Formula = "{CEDANT_RI_RATE}/({TEMP_A_2}+{TEMP_A_3}+{TEMP_A_4}+{TEMP_A_5})*{TEMP_A_1}",

                RiDataBo = new RiDataBo
                {
                    CedantRiRate = 4,
                    TempA1 = 7335,
                    TempA2 = 26,
                    TempA3 = 0,
                    TempA4 = 4,
                    TempA5 = 4,
                },
            };

            PrintResult(soe);
        }

        public void EvalValue3()
        {
            PrintOutputTitle("EvalValue3");

            var soe = new StandardOutputEval()
            {
                Condition = "string.IsNullOrEmpty({TREATY_CODE})",

                RiDataBo = new RiDataBo
                {
                },
            };

            PrintResult(soe);
        }

        public void EvalValue4()
        {
            PrintOutputTitle("EvalValue4");

            var soe = new StandardOutputEval()
            {
                Condition = "true",
                Formula = "{STANDARD_PREMIUM}+{SUBSTANDARD_PREMIUM}",

                RiDataBo = new RiDataBo
                {
                    StandardPremium = 123456.00009,
                    SubstandardPremium = 0.00011,
                },
            };

            PrintResult(soe);
        }

        public void EvalValue5()
        {
            PrintOutputTitle("EvalValue5");

            var soe = new StandardOutputEval()
            {
                Condition = "true",
                Formula = "{STANDARD_PREMIUM}+{SUBSTANDARD_PREMIUM}",
            };

            PrintResult(soe);
        }

        public void EvalValue6()
        {
            PrintOutputTitle("EvalValue6");

            var soe = new StandardOutputEval()
            {
                //Condition = "{INSURED_DATE_OF_BIRTH} == null",
                Condition = "{INSURED_DATE_OF_BIRTH} == DateTime.MinValue",

                RiDataBo = new RiDataBo
                {
                    InsuredDateOfBirth = DateTime.Now
                },
            };

            PrintResult(soe);
        }

        public void EvalTranTypeCode()
        {
            PrintOutputTitle("EvalTranTypeCode");

            var soe = new StandardOutputEval()
            {
                Condition = "{TRANSACTION_TYPE_CODE} == \"AL\" && {TEMP_D_1} != null",
                Formula = "{TEMP_D_1}.ToString(\"yyyyMMMdd\")",

                RiDataBo = new RiDataBo
                {
                    TransactionTypeCode = "AL",
                    TempD1 = DateTime.Now,
                },
            };

            PrintResult(soe);
        }

        public void PrintResult(StandardOutputEval soe)
        {
            var valid = soe.EvalCondition();
            PrintDetail("Condition", soe.Condition);
            PrintDetail("FormattedCondition", soe.FormattedCondition);
            PrintDetail("Condition true/false", valid);
            PrintErrors(soe);
            PrintMessage();
            if (valid)
            {
                var value = soe.EvalFormula();
                PrintDetail("Formula", soe.Formula);
                PrintDetail("FormattedFormula", soe.FormattedFormula);
                PrintDetail("Value", value);
                PrintDetail("Value Data Type", value == null ? Util.Null : value.GetType().ToString());
                PrintErrors(soe);
                PrintMessage();
            }
        }

        public void PrintErrors(StandardOutputEval soe)
        {
            if (soe.Errors != null && soe.Errors.Count > 0)
            {
                PrintMessage();
                PrintMessage("-----Errors-----");
                foreach (var error in soe.Errors)
                {
                    PrintMessage(error);
                }
            }
        }
    }
}
