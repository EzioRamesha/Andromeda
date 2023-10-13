using BusinessObject;
using BusinessObject.RiDatas;
using BusinessObject.Sanctions;
using BusinessObject.SoaDatas;
using Services.RiDatas;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;
using Row = Shared.ProcessFile.Row;

namespace ConsoleApp.Commands.RawFiles.Sanction
{
    public class MappingSanction : Command
    {
        public const int ROW_TYPE_HEADER = 1;
        public const int ROW_TYPE_DATA = 2;

        public ProcessSanctionBatch ProcessSanctionBatch { get; set; }

        // Data
        public SanctionBo SanctionBo { get; set; }

        public List<string> Aliases { get; set; }

        public List<string> IdTypes { get; set; }

        public List<string> IdNumbers { get; set; }

        public List<string> BirthDates { get; set; }

        public List<string> Countries { get; set; }

        public List<string> Addresses { get; set; }

        public List<string> Comments { get; set; }

        // Error
        public bool IsSuccess { get; set; } = true;

        public List<string> Errors { get; set; }

        public Row Row { get; set; }

        public MappingSanction(ProcessSanctionBatch batch)
        {
            ProcessSanctionBatch = batch;
        }

        public MappingSanction(ProcessSanctionBatch batch, Row row) : this(batch)
        {
            Row = row;
        }

        public void MappingRow()
        {
            int rowIndex = Row.RowIndex;
            if (rowIndex == 1)
                return;

            SanctionBo = new SanctionBo();
            Errors = new List<string>();

            foreach (Column col in Row.Columns)
            {
                ProcessData(col);
            }

            if (!string.IsNullOrWhiteSpace(SanctionBo.Name))
            {
                if (Aliases == null)
                    Aliases = new List<string>();

                if (!Aliases.Contains(SanctionBo.Name))
                {
                    Aliases.Add(SanctionBo.Name);
                }
            }

            ValidateData();
        }

        public void ProcessData(Column col)
        {
            int? colIndex = col.ColIndex;
            object value = col.Value;
            object value2 = col.Value2;

            var mappingCol = ProcessSanctionBatch.Columns.Where(q => q.ColIndex == colIndex).FirstOrDefault();
            if (mappingCol == null)
                return;

            switch (mappingCol.Type)
            {
                case SanctionBatchBo.TypeAlias:
                    Aliases = SeparateColumnValue(value);
                    break;
                case SanctionBatchBo.TypeIdType:
                    IdTypes = SeparateColumnValue(value);
                    break;
                case SanctionBatchBo.TypeIdNumber:
                    IdNumbers = SeparateColumnValue(value);
                    break;
                case SanctionBatchBo.TypeDateOfBirth:
                    BirthDates = SeparateColumnValue(value);
                    break;
                case SanctionBatchBo.TypeCountry:
                    Countries = SeparateColumnValue(value);
                    break;
                case SanctionBatchBo.TypeAddress:
                    Addresses = SeparateColumnValue(value);
                    break;
                case SanctionBatchBo.TypeComment:
                    Comments = SeparateColumnValue(value);
                    break;
                default:
                    if (!string.IsNullOrEmpty(mappingCol.Property))
                        SanctionBo.SetPropertyValue(mappingCol.Property, value);
                    break;
            }
        }

        public List<string> SeparateColumnValue(object value)
        {
            if (value == null)
                return null;

            string valueStr = value.ToString();
            if (string.IsNullOrEmpty(valueStr))
                return null;

            return valueStr.Split('|').Select(q => q.Trim()).ToList();
        }

        public void ValidateData()
        {
            if (string.IsNullOrEmpty(SanctionBo.PublicationInformation))
            {
                Errors.Add("Publication Information is empty");
            }

            if (string.IsNullOrEmpty(SanctionBo.Category))
            {
                Errors.Add("Category is empty");
            }
            else
            {
                string category = SanctionBo.Category.ToUpper();
                if (category != SanctionBo.GetCategoryName(SanctionBo.CategoryIndividual, true) && category != SanctionBo.GetCategoryName(SanctionBo.CategoryEntity, true))
                {
                    Errors.Add(string.Format("Invalid Category: {0}", SanctionBo.Category));
                }
            }

            if (string.IsNullOrEmpty(SanctionBo.Name))
            {
                Errors.Add("Name is empty");
            }

            if (!BirthDates.IsNullOrEmpty())
            {
                List<string> validBirthDates = new List<string>();
                foreach (string birthDate in BirthDates)
                {
                    if (birthDate.Length == 4 && int.TryParse(birthDate, out _))
                    {
                        validBirthDates.Add(birthDate);
                        continue;
                    }

                    if (!Util.TryParseDateTime(birthDate, out _, out _))
                    {
                        //Errors.Add("Date of Birth format is incorrect");
                        if (Comments == null)
                            Comments = new List<string>();

                        Comments.Add(string.Format("DOB: {0}", birthDate));
                    }
                    else
                    {
                        validBirthDates.Add(birthDate);
                    }
                }
                BirthDates = validBirthDates;
            }

            //int idTypeCount = (IdTypes.IsNullOrEmpty()) ? 0 : IdTypes.Count();
            //int idNumberCount = (IdNumbers.IsNullOrEmpty()) ? 0 : IdNumbers.Count();

            //if (idTypeCount != idNumberCount)
            //{
            //    Errors.Add("The number of document types and document numbers does not match");
            //}

            if (!Errors.IsNullOrEmpty())
            {
                IsSuccess = false;
                Errors = Errors.Select(q => string.Format("{0} at row {1}", q, Row.RowIndex)).ToList();
            }
        }
    }
}
