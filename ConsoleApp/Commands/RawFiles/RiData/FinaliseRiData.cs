using BusinessObject;
using BusinessObject.RiDatas;
using Services.RiDatas;
using Shared;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.RawFiles.RiData
{
    public class FinaliseRiData : Command
    {
        public FinaliseRiDataBatch FinaliseRiDataBatch { get; set; }
        public RiDataBo RiDataBo { get; set; }
        public bool Success { get; set; } = false;
        public bool Ignore { get; set; } = false;

        public FinaliseRiData(FinaliseRiDataBatch finaliseRiDataBatch, RiDataBo riDataBo)
        {
            FinaliseRiDataBatch = finaliseRiDataBatch;
            RiDataBo = riDataBo;
        }

        public void Finalise()
        {
            var riData = RiDataBo;
            if (riData.IgnoreFinalise)
            {
                riData.FinaliseStatus = RiDataBo.FinaliseStatusSuccess;
                Success = true;
                Ignore = true;
                Save();
                return;
            }

            var errors = riData.ValidateFinalise(FinaliseRiDataBatch.RiDataBatchBo.Quarter);
            ValidateTreatyCode(ref riData, ref errors);
            ValidateMfrsBenefitCode(ref riData, ref errors);
            //ValidateRateTableCode(ref riData, ref errors); //Skip this validation due to current rate table code combination
            ValidateMfrs17CellNames(ref riData, ref errors);
            ValidateMfrs17ContractCodes(ref riData, ref errors);
            ValidateLoaCodes(ref riData, ref errors);
            ValidateDropDowns(ref riData, ref errors);

            //if (RiDataService.IsDuplicate(riData))
            //    errors.Add("Duplicate record found!");

            if (errors.IsNullOrEmpty())
            {
                riData.FinaliseStatus = RiDataBo.FinaliseStatusSuccess;
                Success = true;
            }
            else
            {
                riData.FinaliseStatus = RiDataBo.FinaliseStatusFailed;
                riData.SetError("FinaliseError", errors);
                Success = false;
            }

            Save();
        }

        /// <summary>
        /// Check treaty code is exists in treaty code table
        /// </summary>
        /// <param name="riData"></param>
        /// <param name="errors"></param>
        public void ValidateTreatyCode(ref RiDataBo riData, ref List<string> errors)
        {
            if (string.IsNullOrEmpty(riData.TreatyCode))
                return;

            string error;

            // query from memory
            if (!FinaliseRiDataBatch.CacheService.TreatyCodes.IsNullOrEmpty())
            {
                var rd = riData;
                var tcs = FinaliseRiDataBatch.CacheService.TreatyCodes.Where(q => q == rd.TreatyCode).ToList();
                if (tcs.IsNullOrEmpty())
                {
                    error = riData.FormatFinaliseError(StandardOutputBo.TypeTreatyCode, "Treaty Code does not belong to Treaty: " + riData.TreatyCode);
                    errors.Add(error);
                    riData.SetError(StandardOutputBo.GetPropertyNameByType(StandardOutputBo.TypeTreatyCode), error);
                }
            }
        }

        /// <summary>
        /// Check mlre benefit code is exists in benefit table
        /// </summary>
        /// <param name="riData"></param>
        /// <param name="errors"></param>
        public void ValidateMfrsBenefitCode(ref RiDataBo riData, ref List<string> errors)
        {
            if (string.IsNullOrEmpty(riData.MlreBenefitCode))
                return;

            string error;

            // query from database
            //if (BenefitService.CountByCode(riData.MlreBenefitCode) == 0)
            //{
            //    error = riData.FormatFinaliseError(StandardOutputBo.TypeMlreBenefitCode, "Record not found in Benefit: " + riData.MlreBenefitCode);
            //    errors.Add(error);
            //    riData.SetError(StandardOutputBo.GetPropertyNameByType(StandardOutputBo.TypeMlreBenefitCode), error);
            //}

            // query from memory
            if (!FinaliseRiDataBatch.CacheService.BenefitCodes.IsNullOrEmpty())
            {
                var rd = riData;
                var bcs = FinaliseRiDataBatch.CacheService.BenefitCodes.Where(q => q == rd.MlreBenefitCode).ToList();
                if (bcs.IsNullOrEmpty())
                {
                    error = riData.FormatFinaliseError(StandardOutputBo.TypeMlreBenefitCode, "Record not found in Benefit: " + riData.MlreBenefitCode);
                    errors.Add(error);
                    riData.SetError(StandardOutputBo.GetPropertyNameByType(StandardOutputBo.TypeMlreBenefitCode), error);
                }
            }
        }

        /// <summary>
        /// Check rate table code is exists in rate table
        /// </summary>
        /// <param name="riData"></param>
        /// <param name="errors"></param>
        public void ValidateRateTableCode(ref RiDataBo riData, ref List<string> errors)
        {
            if (string.IsNullOrEmpty(riData.RateTable))
                return;

            string error;

            // query from database
            //if (RateTableService.CountByRateTableCode(riData.RateTable) == 0)
            //{
            //    error = riData.FormatFinaliseError(StandardOutputBo.TypeRateTable, "Record not found in Rate Table: " + riData.RateTable);
            //    errors.Add(error);
            //    riData.SetError(StandardOutputBo.GetPropertyNameByType(StandardOutputBo.TypeRateTable), error);
            //}

            // query from memory
            if (!FinaliseRiDataBatch.CacheService.RateTableCodes.IsNullOrEmpty())
            {
                var rd = riData;

                string rateTableCode = rd.RateTable.Split('_')[0];

                var rtcs = FinaliseRiDataBatch.CacheService.RateTableCodes.Where(q => q == rateTableCode).ToList();
                if (rtcs.IsNullOrEmpty())
                {
                    error = riData.FormatFinaliseError(StandardOutputBo.TypeRateTable, "Record not found in Rate Table: " + rateTableCode);
                    errors.Add(error);
                    riData.SetError(StandardOutputBo.GetPropertyNameByType(StandardOutputBo.TypeRateTable), error);
                }
            }
        }

        /// <summary>
        /// Check MFRS 17 Cell Name is exists in MFRS 17 Cell Mapping Table
        /// </summary>
        /// <param name="riData"></param>
        /// <param name="errors"></param>
        public void ValidateMfrs17CellNames(ref RiDataBo riData, ref List<string> errors)
        {
            if (string.IsNullOrEmpty(riData.Mfrs17CellName))
                return;

            string error;

            // query from database
            //if (Mfrs17CellMappingService.CountByCellName(riData.Mfrs17CellName) == 0)
            //{
            //    error = riData.FormatFinaliseError(StandardOutputBo.TypeMfrs17CellName, "Record not found in MFRS17 Cell Mapping: " + riData.Mfrs17CellName);
            //    errors.Add(error);
            //    riData.SetError(StandardOutputBo.GetPropertyNameByType(StandardOutputBo.TypeMfrs17CellName), error);
            //}

            // query from memory
            if (!FinaliseRiDataBatch.CacheService.Mfrs17CellMappingCellNames.IsNullOrEmpty())
            {
                var rd = riData;
                var cns = FinaliseRiDataBatch.CacheService.Mfrs17CellMappingCellNames.Where(q => q == rd.Mfrs17CellName).ToList();
                if (cns.IsNullOrEmpty())
                {
                    error = riData.FormatFinaliseError(StandardOutputBo.TypeMfrs17CellName, "Record not found in MFRS17 Cell Mapping: " + riData.Mfrs17CellName);
                    errors.Add(error);
                    riData.SetError(StandardOutputBo.GetPropertyNameByType(StandardOutputBo.TypeMfrs17CellName), error);
                }
            }
        }

        /// <summary>
        /// Check MFRS 17 Contract Code is exists in MFRS 17 Cell Mapping Table
        /// </summary>
        /// <param name="riData"></param>
        /// <param name="errors"></param>
        public void ValidateMfrs17ContractCodes(ref RiDataBo riData, ref List<string> errors)
        {
            if (string.IsNullOrEmpty(riData.Mfrs17TreatyCode))
                return;

            string error;

            // query from memory
            if (!FinaliseRiDataBatch.CacheService.Mfrs17CellMappingContractCodes.IsNullOrEmpty())
            {
                var rd = riData;
                var ccs = FinaliseRiDataBatch.CacheService.Mfrs17CellMappingContractCodes.Where(q => q == rd.Mfrs17TreatyCode).ToList();
                if (ccs.IsNullOrEmpty())
                {
                    error = riData.FormatFinaliseError(StandardOutputBo.TypeMfrs17TreatyCode, "Record not found in MFRS17 Contract Code: " + riData.Mfrs17TreatyCode);
                    errors.Add(error);
                    riData.SetError(StandardOutputBo.GetPropertyNameByType(StandardOutputBo.TypeMfrs17TreatyCode), error);
                }
            }
        }

        /// <summary>
        /// Check LOA Code is exists in MFRS 17 Cell Mapping Table
        /// </summary>
        /// <param name="riData"></param>
        /// <param name="errors"></param>
        public void ValidateLoaCodes(ref RiDataBo riData, ref List<string> errors)
        {
            if (string.IsNullOrEmpty(riData.LoaCode))
                return;

            string error;

            // query from memory
            if (!FinaliseRiDataBatch.CacheService.Mfrs17CellMappingLoaCodes.IsNullOrEmpty())
            {
                var rd = riData;
                var lcs = FinaliseRiDataBatch.CacheService.Mfrs17CellMappingLoaCodes.Where(q => q == rd.LoaCode).ToList();
                if (lcs.IsNullOrEmpty())
                {
                    error = riData.FormatFinaliseError(StandardOutputBo.TypeLoaCode, "Record not found in MFRS17 Cell Mapping: " + riData.LoaCode);
                    errors.Add(error);
                    riData.SetError(StandardOutputBo.GetPropertyNameByType(StandardOutputBo.TypeLoaCode), error);
                }
            }
        }

        /// <summary>
        /// Check 
        /// </summary>
        /// <param name="riData"></param>
        /// <param name="errors"></param>
        public void ValidateDropDowns(ref RiDataBo riData, ref List<string> errors)
        {
            string error;

            // query from memory
            if (!FinaliseRiDataBatch.CacheService.RiDataDropDownDetailBos.IsNullOrEmpty())
            {
                var dropDowns = StandardOutputBo.GetDropDownTypes();
                foreach (int type in dropDowns)
                {
                    string property = StandardOutputBo.GetPropertyNameByType(type);
                    object value = riData.GetPropertyValue(property);

                    if (value == null)
                        continue;

                    if (value is string @string && !string.IsNullOrEmpty(@string))
                    {
                        var pickListDetails = FinaliseRiDataBatch.CacheService.RiDataDropDownDetailBos.Where(q => q.PickListBo.StandardOutputId == type && q.Code == @string).ToList();
                        if (pickListDetails.IsNullOrEmpty())
                        {
                            error = riData.FormatFinaliseError(type, "Record not found in Pick List: " + @string);
                            errors.Add(error);
                            riData.SetError(property, error);
                        }
                    }
                }
            }

            // query from database
            //var dropDowns = StandardOutputBo.GetDropDownTypes();
            //foreach (int type in dropDowns)
            //{
            //    string property = StandardOutputBo.GetPropertyNameByType(type);
            //    object value = riData.GetPropertyValue(property);
            //    if (value != null && value is string @string && !string.IsNullOrEmpty(@string))
            //    {
            //        int count = PickListDetailService.CountByStandardOutputIdCode(type, @string);
            //        if (count == 0)
            //        {
            //            error = riData.FormatFinaliseError(type, "Record not found in Pick List: " + @string);
            //            errors.Add(error);
            //            riData.SetError(property, error);
            //        }
            //    }
            //}
        }

        public void Save()
        {
            var riData = RiDataBo;
            RiDataService.Update(ref riData);
        }
    }
}
