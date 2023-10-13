using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Retrocession;
using DataAccess.Entities.Identity;
using Services;
using Services.Identity;
using Services.Retrocession;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessValidDuplicationListing : Command
    {
        public List<Column> Columns { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public char[] charsToTrim = { ',', '.', ' ' };

        public ProcessValidDuplicationListing()
        {
            Title = "ProcessValidDuplicationListing";
            Description = "To read Valid Duplication Listing csv file and insert into database";
            Arguments = new string[]
            {
                "filePath",
            };
            Hide = true;
            Errors = new List<string>();
            GetColumns();
        }

        public override bool Validate()
        {
            string filepath = CommandInput.Arguments[0];
            if (!File.Exists(filepath))
            {
                PrintError(string.Format(MessageBag.FileNotExists, filepath));
                return false;
            }
            else
            {
                FilePath = filepath;
            }
            return base.Validate();
        }

        public override void Run()
        {
            PrintStarting();

            Process();

            PrintEnding();
        }

        public void Process()
        {
            if (PostedFile != null)
            {
                TextFile = new TextFile(PostedFile.InputStream);
            }
            else if (File.Exists(FilePath))
            {
                TextFile = new TextFile(FilePath);
            }
            else
            {
                throw new Exception("No file can be read");
            }

            TrailObject trail;
            Result result;
            while (TextFile.GetNextRow() != null)
            {
                if (TextFile.RowIndex == 1)
                    continue; // Skip header row

                if (PrintCommitBuffer())
                {
                }
                SetProcessCount();

                bool error = false;

                ValidDuplicationListBo b = null;
                try
                {
                    b = SetData();

                    if (!string.IsNullOrEmpty(b.TreatyCode))
                    {
                        var treatyCode = TreatyCodeService.FindByCode(b.TreatyCode);
                        if (treatyCode == null)
                        {
                            SetProcessCount("Treaty Code Not Found");
                            Errors.Add(string.Format("The Treaty Code doesn't exists: {0} at row {1}", b.TreatyCode, TextFile.RowIndex));
                            error = true;
                        }
                        else if (treatyCode.Status == TreatyCodeBo.StatusInactive)
                        {
                            SetProcessCount("Treaty Code Inactive");
                            Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.TreatyCodeStatusInactive, b.TreatyCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            b.TreatyCodeId = treatyCode.Id;
                            b.TreatyCodeBo = treatyCode;
                        }
                    }
                    else
                    {
                        SetProcessCount("Treaty Code Empty");
                        Errors.Add(string.Format("Please enter the Treaty Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(b.CedantPlanCode))
                    {
                        SetProcessCount("Ceding Plan Code Empty");
                        Errors.Add(string.Format("Please enter the Treaty Plan Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(b.InsuredName))
                    {
                        SetProcessCount("Insured Name Empty");
                        Errors.Add(string.Format("Please enter the Insured Name at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(b.InsuredDateOfBirthStr))
                    {
                        if (!ValidateDateTimeFormat(ValidDuplicationListBo.ColumnInsuredDateOfBirth, ref b))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Insured Date of Birth Empty");
                        Errors.Add(string.Format("Please enter the Insured Date of Birth at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(b.PolicyNumber))
                    {
                        SetProcessCount("Policy Number Empty");
                        Errors.Add(string.Format("Please enter the Policy Number at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(b.InsuredGenderCodePickList))
                    {
                        var insuredGenderCodePickList = PickListDetailService.FindByPickListIdCode(PickListBo.InsuredGenderCode, b.InsuredGenderCodePickList);

                        if (insuredGenderCodePickList == null)
                        {
                            SetProcessCount("Insured Gender Code Not Found");
                            Errors.Add(string.Format("The Insured Gender Code doesn't exists: {0} at row {1}", b.InsuredGenderCodePickList, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            b.InsuredGenderCodePickListDetailId = insuredGenderCodePickList.Id;
                            b.InsuredGenderCodePickListDetailBo = insuredGenderCodePickList;
                        }
                    }
                    else
                    {
                        SetProcessCount("Insured Gender Code Empty");
                        Errors.Add(string.Format("Please enter the Insured Gender Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(b.MLReBenefitCode))
                    {
                        var mlreBenefitCode = BenefitService.FindByCode(b.MLReBenefitCode);

                        if (mlreBenefitCode == null)
                        {
                            SetProcessCount("MLRe Benefit Code Not Found");
                            Errors.Add(string.Format("The MLRe Benefit Code doesn't exists: {0} at row {1}", b.InsuredGenderCodePickList, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            b.MLReBenefitCodeId = mlreBenefitCode.Id;
                            b.MLReBenefitCodeBo = mlreBenefitCode;
                        }
                    }
                    else
                    {
                        SetProcessCount("MLRe Benefit Code Code Empty");
                        Errors.Add(string.Format("Please enter the MLRe Benefit Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(b.CedingBenefitRiskCode))
                    {
                        SetProcessCount("Ceding Benefit Risk Code Empty");
                        Errors.Add(string.Format("Please enter the Ceding Benefit Risk Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(b.CedingBenefitTypeCode))
                    {
                        SetProcessCount("Ceding Benefit Type Code Empty");
                        Errors.Add(string.Format("Please enter the Ceding Benefit Type Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(b.ReinsuranceEffectiveDateStr))
                    {
                        if (!ValidateDateTimeFormat(ValidDuplicationListBo.ColumnReinsuranceEffectiveDate, ref b))
                        {
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Reinsurance Effective Date Empty");
                        Errors.Add(string.Format("Please enter the Reinsurance Effective Date at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(b.FundsAccountingTypePickList))
                    {
                        var fundsAccountingTypePickList = PickListDetailService.FindByPickListIdCode(PickListBo.FundsAccountingType, b.FundsAccountingTypePickList);

                        if (fundsAccountingTypePickList == null)
                        {
                            SetProcessCount("FDS Accounting Type Not Found");
                            Errors.Add(string.Format("The FDS Accounting Type doesn't exists: {0} at row {1}", b.FundsAccountingTypePickList, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            b.FundsAccountingTypePickListDetailId = fundsAccountingTypePickList.Id;
                            b.FundsAccountingTypePickListDetailBo = fundsAccountingTypePickList;
                        }
                    }
                    else
                    {
                        SetProcessCount("FDS Accounting Type Empty");
                        Errors.Add(string.Format("Please enter the FDS Accounting Type at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(b.ReinsBasisCodePickList))
                    {
                        var reinsBasisCodePickList = PickListDetailService.FindByPickListIdCode(PickListBo.ReinsBasisCode, b.ReinsBasisCodePickList);

                        if (reinsBasisCodePickList == null)
                        {
                            SetProcessCount("Reinsurance Risk Basis Code Not Found");
                            Errors.Add(string.Format("The Reinsurance Risk Basis Code doesn't exists: {0} at row {1}", b.ReinsBasisCodePickList, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            b.ReinsBasisCodePickListDetailId = reinsBasisCodePickList.Id;
                            b.ReinsBasisCodePickListDetailBo = reinsBasisCodePickList;
                        }
                    }
                    else
                    {
                        SetProcessCount("Reinsurance Risk Basis Code Empty");
                        Errors.Add(string.Format("Please enter the Reinsurance Risk Basis Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(b.TransactionTypePickList))
                    {
                        var transactionTypePickList = PickListDetailService.FindByPickListIdCode(PickListBo.TransactionTypeCode, b.TransactionTypePickList);

                        if (transactionTypePickList == null)
                        {
                            SetProcessCount("Transaction Type Code Not Found");
                            Errors.Add(string.Format("The Transaction Type Code doesn't exists: {0} at row {1}", b.TransactionTypePickList, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            b.TransactionTypePickListDetailId = transactionTypePickList.Id;
                            b.TransactionTypePickListDetailBo = transactionTypePickList;
                        }
                    }
                    else
                    {
                        SetProcessCount("Transaction Type Code Empty");
                        Errors.Add(string.Format("Please enter the Transaction Type Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }

                if (error)
                {
                    continue;
                }

                trail = new TrailObject();
                result = ValidDuplicationListService.Create(ref b, ref trail);

                Trail(result, b, trail, "Create");

            }


            PrintProcessCount();

            TextFile.Close();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public ValidDuplicationListBo SetData()
        {
            var b = new ValidDuplicationListBo
            {
                Id = 0,
                TreatyCode = TextFile.GetColValue(ValidDuplicationListBo.ColumnTreatyCode),
                CedantPlanCode = TextFile.GetColValue(ValidDuplicationListBo.ColumnCedantPlanCode),
                InsuredName = TextFile.GetColValue(ValidDuplicationListBo.ColumnInsuredName),
                InsuredDateOfBirthStr = TextFile.GetColValue(ValidDuplicationListBo.ColumnInsuredDateOfBirth),
                PolicyNumber = TextFile.GetColValue(ValidDuplicationListBo.ColumnPolicyNumber),
                InsuredGenderCodePickList = TextFile.GetColValue(ValidDuplicationListBo.ColumnInsuredGenderCodePickList),
                MLReBenefitCode = TextFile.GetColValue(ValidDuplicationListBo.ColumnMLReBenefitCode),
                CedingBenefitRiskCode = TextFile.GetColValue(ValidDuplicationListBo.ColumnCedingBenefitRiskCode),
                CedingBenefitTypeCode = TextFile.GetColValue(ValidDuplicationListBo.ColumnCedingBenefitTypeCode),
                ReinsuranceEffectiveDateStr = TextFile.GetColValue(ValidDuplicationListBo.ColumnCedingBenefitRiskCode),
                FundsAccountingTypePickList = TextFile.GetColValue(ValidDuplicationListBo.ColumnFundsAccountingTypePickList),
                ReinsBasisCodePickList = TextFile.GetColValue(ValidDuplicationListBo.ColumnReinsBasisCodePickList),
                TransactionTypePickList = TextFile.GetColValue(ValidDuplicationListBo.ColumnTransactionTypePickList),
                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            string idStr = TextFile.GetColValue(ValidDuplicationListBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                b.Id = id;
            }
            return b;
        }

        public void Trail(Result result, ValidDuplicationListBo b, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    b.Id,
                    string.Format("{0} Valid Duplication Listing", action),
                    result,
                    trail,
                    b.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public int GetAuthUserId()
        {
            return AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId;
        }

        public bool ValidateDateTimeFormat(int type, ref ValidDuplicationListBo b)
        {
            string header = Columns.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Columns.Where(m => m.ColIndex == type).Select(q => q.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    b.SetPropertyValue(property, datetime.Value);
                }
                else
                {
                    SetProcessCount(string.Format(header, "Error"));
                    Errors.Add(string.Format("The {0} format can not be read: {1} at row {2}", header, value, TextFile.RowIndex));
                    valid = false;
                }
            }
            return valid;
        }

        public List<Column> GetColumns()
        {
            Columns = ValidDuplicationListBo.GetColumns();
            return Columns;
        }
    }
}
