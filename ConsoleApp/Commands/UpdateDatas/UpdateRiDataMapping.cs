using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;

namespace ConsoleApp.Commands.UpdateDatas
{
    class UpdateRiDataMapping : Command
    {
        public UpdateRiDataMapping()
        {
            Title = "UpdateRiDataMapping";
            Description = "To update default value to pick list detail id for standard output which are drop down type with fixed value transformation";
            Options = new string[] {
            };
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                var query = db.RiDataMappings.Where(q => q.TransformFormula == RiDataMappingBo.TransformFormulaFixedValue && (q.StandardOutput.DataType == StandardOutputBo.DataTypeDropDown || q.StandardOutput.Type == StandardOutputBo.TypeMlreBenefitCode));
                int count = query.Count();
                int processed = 0;

                while (processed < count)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var riDataMapping = query.OrderBy(q => q.Id).Skip(processed).Take(1).FirstOrDefault();
                    if (riDataMapping != null)
                    {
                        StandardOutput standardOutput = StandardOutput.Find(riDataMapping.StandardOutputId);
                        if (riDataMapping.TransformFormula == RiDataMappingBo.TransformFormulaFixedValue && (standardOutput.DataType == StandardOutputBo.DataTypeDropDown || standardOutput.Type == StandardOutputBo.TypeMlreBenefitCode))
                        {
                            string defaultValue = riDataMapping.DefaultValue;
                            bool isInteger = int.TryParse(defaultValue, out int objectId);
                            int? defaultObjectId = null;
                            if (!isInteger)
                            {
                                if (standardOutput.Type == StandardOutputBo.TypeMlreBenefitCode)
                                {
                                    Benefit benefit = Benefit.FindByCode(defaultValue);
                                    defaultObjectId = benefit?.Id;
                                    defaultValue = benefit?.Code;
                                }
                                else
                                {
                                    PickListDetail pickListDetail = PickListDetail.FindByStandardOutputIdCode(riDataMapping.StandardOutputId, defaultValue);
                                    defaultObjectId = pickListDetail?.Id;
                                }
                            }
                            else 
                            {
                                if (standardOutput.Type == StandardOutputBo.TypeMlreBenefitCode)
                                {
                                    Benefit benefit = Benefit.Find(objectId);
                                    defaultValue = benefit?.Code;
                                    defaultObjectId = benefit?.Id;
                                }
                                else
                                {
                                    PickListDetail pickListDetail = PickListDetail.Find(objectId);
                                    defaultValue = pickListDetail?.Code;
                                    defaultObjectId = pickListDetail?.Id;
                                }
                            }

                            riDataMapping.DefaultObjectId = defaultObjectId;
                            riDataMapping.DefaultValue = defaultValue;
                            db.Entry(riDataMapping).State = EntityState.Modified;

                            SetProcessCount("Updated");
                        }
                    }

                    processed++;
                }
                db.SaveChanges();

                if (processed > 0)
                    PrintProcessCount();
            }

            PrintEnding();
        }
    }
}
