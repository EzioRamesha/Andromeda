using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using Services;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessPickList : Command
    {
        public List<Column> Columns { get; set; }

        public bool UpdateNonEditable { get; set; }

        public bool FillEmptyCode { get; set; }

        public ProcessPickList()
        {
            Title = "ProcessPickList";
            Description = "To read Pick List Detail excel file and insert into database";
            Arguments = new string[]
            {
                "filepath",
            };
            Options = new string[] {
                "--u|updateNonEditable : Update Code of Non-Editable Pick Lists",
                "--f|fillEmptyCode : Fill up codes of Pick List Details which are empty",
            };
            Hide = true;
            GetColumns();
        }

        public override void Initial()
        {
            base.Initial();

            UpdateNonEditable = IsOption("updateNonEditable");
            FillEmptyCode = IsOption("fillEmptyCode");
        }

        public class PickListDetailExcelColumn
        {
            public int PickListId { get; set; }
            public string PickListFieldName { get; set; }
            public string PickListDetailCode { get; set; }
            public string PickListDetailDescription { get; set; }

            public PickListDetailExcelColumn()
            {
                PickListId = 0;
                PickListFieldName = null;
                PickListDetailCode = null;
                PickListDetailDescription = null;
            }
        }

        public override void Run()
        {
            PrintStarting();

            string filepath = CommandInput.Arguments[0];
            int sortIndex = 0;
            int pickListId = 0;
            bool isEditable = true;

            Result result = PickListService.Result();
            TrailObject trail = new TrailObject();
            Excel excel = new Excel(filepath);
            while (excel.GetNextRow() != null)
            {
                if (excel.RowIndex == 1)
                {
                    continue;
                }
                PickListDetail pickListDetail = new PickListDetail()
                {
                    Description = "",
                };

                while (excel.GetNextCol() != null)
                {
                    string value2 = Convert.ToString(excel.GetValue2());
                    value2 = value2?.Trim();

                    switch (excel.ColIndex)
                    {
                        case 1:
                            pickListDetail.PickListId = Convert.ToInt32(value2);

                            sortIndex++;
                            if (pickListId != pickListDetail.PickListId)
                            {
                                if (pickListId != 0)
                                {
                                    Trail(result, pickListId, trail);
                                }

                                sortIndex = 1;
                                trail = new TrailObject();
                                isEditable = PickListService.Find(pickListDetail.PickListId).IsEditable;
                            }
                            pickListId = pickListDetail.PickListId;

                            pickListDetail.SortIndex = sortIndex;

                            break;
                        case 2:
                            break;
                        case 3:
                            if (!string.IsNullOrWhiteSpace(value2))
                            {
                                pickListDetail.Code = value2;
                            }
                            break;
                        case 4:
                            if (!string.IsNullOrWhiteSpace(value2))
                            {
                                pickListDetail.Description = value2;
                            }
                            break;
                        default:
                            break;
                    }
                }

                pickListDetail.CreatedById = User.DefaultSuperUserId;
                pickListDetail.UpdatedById = User.DefaultSuperUserId;

                if (!string.IsNullOrWhiteSpace(pickListDetail.Description))
                {
                    int count = PickListDetailService.CountByPickListIdSortIndex(pickListDetail.PickListId, pickListDetail.SortIndex);
                    if (count == 0)
                    {
                        DataTrail dataTrail = pickListDetail.Create();
                        dataTrail.Merge(ref trail, result.Table);

                        PrintMessage(string.Format("Created: {0}, {1}", pickListDetail.PickListId.ToString(), pickListDetail.Description));
                    }
                    else if (UpdateNonEditable && !isEditable)
                    {
                        PickListDetail entity = PickListDetail.FindByPickListIdSortIndex(pickListDetail.PickListId, pickListDetail.SortIndex);
                        if (entity.Code != pickListDetail.Code)
                        {
                            entity.Code = pickListDetail.Code;
                            entity.UpdatedById = User.DefaultSuperUserId;

                            DataTrail dataTrail = entity.Update();
                            dataTrail.Merge(ref trail, result.Table);

                            PrintMessage(string.Format("Updated Code: {0}, {1}", entity.Id.ToString(), entity.Code));
                        }
                    }
                    else if (FillEmptyCode)
                    {
                        PickListDetail entity = PickListDetail.FindByPickListIdSortIndex(pickListDetail.PickListId, pickListDetail.SortIndex);
                        if (string.IsNullOrEmpty(entity.Code))
                        {
                            entity.Code = pickListDetail.Code;
                            entity.UpdatedById = User.DefaultSuperUserId;

                            DataTrail dataTrail = entity.Update();
                            dataTrail.Merge(ref trail, result.Table);

                            PrintMessage(string.Format("Filled Code: {0}, {1}", entity.Id.ToString(), entity.Code));
                        }
                    }
                }
            }

            // For the last bulk record
            Trail(result, pickListId, trail);

            excel.Close();

            PrintEnding();
        }

        public override bool Validate()
        {
            string filepath = CommandInput.Arguments[0];
            if (!File.Exists(filepath))
            {
                PrintError(string.Format(MessageBag.FileNotExists, filepath));
                return false;
            }

            return base.Validate();
        }

        public void Trail(Result result, int pickListId, TrailObject trail)
        {
            if (trail.IsValid())
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    pickListId,
                    "Create Pick List Details",
                    result,
                    trail,
                    User.DefaultSuperUserId
                );
                UserTrailService.Create(ref userTrailBo);
            }
        }

        public List<Column> GetColumns()
        {
            Columns = PickListBo.GetColumns();
            return Columns;
        }
    }
}
