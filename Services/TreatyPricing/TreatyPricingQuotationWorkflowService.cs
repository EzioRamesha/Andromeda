using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingQuotationWorkflowService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingQuotationWorkflow)),
                Controller = ModuleBo.ModuleController.TreatyPricingQuotationWorkflow.ToString()
            };
        }

        public static Expression<Func<TreatyPricingQuotationWorkflow, TreatyPricingQuotationWorkflowBo>> Expression()
        {
            return entity => new TreatyPricingQuotationWorkflowBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                QuotationId = entity.QuotationId,
                ReinsuranceTypePickListDetailId = entity.ReinsuranceTypePickListDetailId,
                Name = entity.Name,
                Summary = entity.Summary,
                Status = entity.Status,
                StatusRemarks = entity.StatusRemarks,
                TargetSendDate = entity.TargetSendDate,
                LatestRevisionDate = entity.LatestRevisionDate,
                PricingTeamPickListDetailId = entity.PricingTeamPickListDetailId,
                PricingStatus = entity.PricingStatus,
                TargetClientReleaseDate = entity.TargetClientReleaseDate,
                TargetRateCompletionDate = entity.TargetRateCompletionDate,
                FinaliseDate = entity.FinaliseDate,
                LatestVersion = entity.LatestVersion,
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                CreatedAt = entity.CreatedAt,
                //TargetSendDateStr = entity.TargetSendDate.HasValue ? entity.TargetSendDate.Value.ToString(Util.GetDateFormat()) : "",
                //LatestRevisionDateStr = entity.LatestRevisionDate.HasValue ? entity.LatestRevisionDate.Value.ToString(Util.GetDateFormat()) : "",
                //TargetClientReleaseDateStr = entity.TargetClientReleaseDate.HasValue ? entity.TargetClientReleaseDate.Value.ToString(Util.GetDateFormat()) : "",
                //TargetRateCompletionDateStr = entity.TargetRateCompletionDate.HasValue ? entity.TargetRateCompletionDate.Value.ToString(Util.GetDateFormat()) : "",
                //FinaliseDateStr = entity.FinaliseDate.HasValue ? entity.FinaliseDate.Value.ToString(Util.GetDateFormat()) : "",
            };
        }

        public static TreatyPricingQuotationWorkflowBo FormBo(TreatyPricingQuotationWorkflow entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingQuotationWorkflowBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),
                QuotationId = entity.QuotationId,
                ReinsuranceTypePickListDetailId = entity.ReinsuranceTypePickListDetailId,
                Name = entity.Name,
                Summary = entity.Summary,
                Status = entity.Status,
                StatusRemarks = entity.StatusRemarks,
                TargetSendDate = entity.TargetSendDate,
                LatestRevisionDate = entity.LatestRevisionDate,
                PricingTeamPickListDetailId = entity.PricingTeamPickListDetailId,
                PricingStatus = entity.PricingStatus,
                TargetClientReleaseDate = entity.TargetClientReleaseDate,
                TargetRateCompletionDate = entity.TargetRateCompletionDate,
                FinaliseDate = entity.FinaliseDate,
                Description = entity.Description,
                LatestVersion = entity.LatestVersion,
                CreatedAt = entity.CreatedAt,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                TargetSendDateStr = entity.TargetSendDate.HasValue ? entity.TargetSendDate.Value.ToString(Util.GetDateFormat()) : "",
                LatestRevisionDateStr = entity.LatestRevisionDate.HasValue ? entity.LatestRevisionDate.Value.ToString(Util.GetDateFormat()) : "",
                TargetClientReleaseDateStr = entity.TargetClientReleaseDate.HasValue ? entity.TargetClientReleaseDate.Value.ToString(Util.GetDateFormat()) : "",
                TargetRateCompletionDateStr = entity.TargetRateCompletionDate.HasValue ? entity.TargetRateCompletionDate.Value.ToString(Util.GetDateFormat()) : "",
                FinaliseDateStr = entity.FinaliseDate.HasValue ? entity.FinaliseDate.Value.ToString(Util.GetDateFormat()) : "",
                TreatyPricingQuotationWorkflowVersionBos = TreatyPricingQuotationWorkflowVersionService.GetByTreatyPricingQuotationWorkflowId(entity.Id),
                TreatyPricingWorkflowObjectBos = TreatyPricingWorkflowObjectService.GetByTypeWorkflowId(TreatyPricingWorkflowObjectBo.TypeQuotation, entity.Id),

                StatusName = TreatyPricingQuotationWorkflowBo.GetStatusName(entity.Status),

                ReinsuranceType = entity.GetReinsuranceType(entity.ReinsuranceTypePickListDetailId),
                PricingTeam = entity.GetPricingTeam(entity.PricingTeamPickListDetailId),

                BDPersonInChargeId = entity.BDPersonInChargeId,
                PersonInChargeId = entity.PersonInChargeId,
                CEOPending = entity.CEOPending,
                PricingPending = entity.PricingPending,
                UnderwritingPending = entity.UnderwritingPending,
                HealthPending = entity.HealthPending,
                ClaimsPending = entity.ClaimsPending,
                BDPending = entity.BDPending,
                TGPending = entity.TGPending,
                PricingDueDate = entity.PricingDueDate,
            };
        }

        public static IList<TreatyPricingQuotationWorkflowBo> FormBos(IList<TreatyPricingQuotationWorkflow> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingQuotationWorkflowBo> bos = new List<TreatyPricingQuotationWorkflowBo>() { };
            foreach (TreatyPricingQuotationWorkflow entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingQuotationWorkflow FormEntity(TreatyPricingQuotationWorkflowBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingQuotationWorkflow
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                QuotationId = bo.QuotationId,
                ReinsuranceTypePickListDetailId = bo.ReinsuranceTypePickListDetailId,
                Name = bo.Name,
                Summary = bo.Summary,
                Status = bo.Status,
                StatusRemarks = bo.StatusRemarks,
                TargetSendDate = bo.TargetSendDate,
                LatestRevisionDate = bo.LatestRevisionDate,
                PricingTeamPickListDetailId = bo.PricingTeamPickListDetailId,
                PricingStatus = bo.PricingStatus,
                TargetClientReleaseDate = bo.TargetClientReleaseDate,
                TargetRateCompletionDate = bo.TargetRateCompletionDate,
                FinaliseDate = bo.FinaliseDate,
                Description = bo.Description,
                LatestVersion = bo.LatestVersion,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,

                CEOPending = 0,
                PricingPending = 0,
                UnderwritingPending = 0,
                HealthPending = 0,
                ClaimsPending = 0,
                BDPending = 0,
                TGPending = 0,
                PricingDueDate = null,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingQuotationWorkflow.IsExists(id);
        }

        public static TreatyPricingQuotationWorkflowBo Find(int? id)
        {
            return FormBo(TreatyPricingQuotationWorkflow.Find(id));
        }

        public static TreatyPricingQuotationWorkflowBo FindByQuotationId(string quotationId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingQuotationWorkflows.Where(q => q.QuotationId == quotationId).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingQuotationWorkflowBo> FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingQuotationWorkflows.Where(q => q.Status == status).ToList());
            }
        }

        public static IList<TreatyPricingQuotationWorkflowBo> GetAll()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingQuotationWorkflows.ToList());
            }
        }

        public static IList<TreatyPricingWorkflowObjectBo> GetWorkflowObjects(TreatyPricingWorkflowObjectBo bo)
        {
            using (var db = new AppDbContext())
            {
                List<int> workflowIds = db.TreatyPricingWorkflowObjects
                    .Where(q => q.Type == TreatyPricingWorkflowObjectBo.TypeQuotation)
                    .Where(q => q.ObjectType == bo.ObjectType)
                    .Select(q => q.WorkflowId)
                    .ToList();

                var query = db.TreatyPricingQuotationWorkflows.Where(q => !workflowIds.Contains(q.Id));
                if (!string.IsNullOrEmpty(bo.WorkflowCode))
                {
                    query = query.Where(q => q.QuotationId.Contains(bo.WorkflowCode));
                }

                int type = TreatyPricingWorkflowObjectBo.TypeQuotation;
                string typeName = TreatyPricingWorkflowObjectBo.GetTypeName(type);

                return query.Select(
                    q => new TreatyPricingWorkflowObjectBo()
                    {
                        Type = type,
                        TypeName = typeName,
                        ObjectType = bo.ObjectType,
                        ObjectId = bo.ObjectId,
                        ObjectVersionId = bo.ObjectVersionId,
                        WorkflowId = q.Id,
                        WorkflowCode = q.QuotationId,
                    }
                ).ToList();
            }
        }

        //public static IList<TreatyPricingQuotationWorkflowBo> GetByTreatyPricingCedantId(int treatyPricingCedantId)
        //{
        //    using (var db = new AppDbContext())
        //    {
        //        return FormBos(db.TreatyPricingQuotationWorkflows
        //            .Where(q => q.TreatyPricingCedantId == treatyPricingCedantId)
        //            .ToList());
        //    }
        //}

        public static int GetLatestVersion(int workflowId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingQuotationWorkflowVersions.Where(q => q.TreatyPricingQuotationWorkflowId == workflowId).Select(q => q.Version).Max();
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingQuotationWorkflows.Where(q => q.Status == status).Count();
            }
        }

        public static string GetSharePointPath(TreatyPricingQuotationWorkflowBo bo)
        {
            string reinsuranceType = PickListDetailService.Find(bo.ReinsuranceTypePickListDetailId).Code;

            string sharePointPath = reinsuranceType + "/" + bo.CedantBo.Code + "/" + bo.QuotationId;

            return sharePointPath;
        }

        #region Quotation Dashboard
        public static int CalculateDateRange(string startDateStr, string endDateStr, bool addDayAfterWorkingHour = false)
        {
            DateTime startDate;
            DateTime endDate;

            bool parseStartDate = DateTime.TryParse(startDateStr, out startDate);
            bool parseEndDate = DateTime.TryParse(endDateStr, out endDate);

            string error = null;
            if (!parseStartDate || !parseEndDate)
            {
                error = "Invalid Date Format";
            }
            else if (endDate <= startDate)
            {
                error = "End Date cannot be before Start Date";
            }

            if (error != null)
                return -1;

            DateTime startWorkingTime;
            if (addDayAfterWorkingHour)
            {
                startWorkingTime = DateTime.Parse(startDate.ToShortDateString() + " " + Util.GetConfig("StartWorkingTime"));
                DateTime endWorkingTime = DateTime.Parse(startDate.ToShortDateString() + " " + Util.GetConfig("EndWorkingTime"));

                if (startDate >= endWorkingTime)
                {
                    startDate = startDate.AddDays(1);
                    TimeSpan ts = new TimeSpan(startWorkingTime.Hour, startWorkingTime.Minute, 0);

                    startDate = startDate.Date + ts;
                }

                if (startDate < startWorkingTime)
                {
                    TimeSpan ts = new TimeSpan(startWorkingTime.Hour, startWorkingTime.Minute, 0);

                    startDate = startDate.Date + ts;
                }
            }

            int days = 0;

            DateTime currentDate = startDate;

            while (currentDate.Date < endDate.Date)
            {
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    currentDate = currentDate.AddDays(1);
                    continue;
                }

                if (PublicHolidayDetailService.IsExists(currentDate))
                {
                    currentDate = currentDate.AddDays(1);
                    continue;
                }

                currentDate = currentDate.AddDays(1);
                days++;
            }

            startWorkingTime = DateTime.Parse(endDate.ToShortDateString() + " " + Util.GetConfig("StartWorkingTime"));
            if (endDate < startWorkingTime)
                endDate = startWorkingTime;

            TimeSpan timeRemaining = endDate - startWorkingTime;

            int hours = (days * 24) + timeRemaining.Hours;
            int minutes = timeRemaining.Minutes;

            long turnAroundTime = (new TimeSpan(hours, minutes, 0)).Ticks;

            return days;
        }

        public static IList<TreatyPricingQuotationWorkflowBo> GetQuotationCaseByReinsuranceType()
        {
            var reinsuranceTypes = PickListDetailService.GetByPickListId(PickListBo.ReinsuranceType);
            List<TreatyPricingQuotationWorkflowBo> bos = new List<TreatyPricingQuotationWorkflowBo> { };
            int statusQuoting = TreatyPricingQuotationWorkflowBo.StatusQuoting;

            using (var db = new AppDbContext(false))
            {
                foreach (var reinsuranceType in reinsuranceTypes)
                {
                    var noOfCase = db.TreatyPricingQuotationWorkflows
                        .Where(q => q.ReinsuranceTypePickListDetailId == reinsuranceType.Id && q.Status == statusQuoting)
                        .Count();

                    bos.Add(new TreatyPricingQuotationWorkflowBo
                    {
                        ReinsuranceTypePickListDetailId = reinsuranceType.Id,
                        ReinsuranceType = reinsuranceType.Code,
                        NoOfCase = noOfCase,
                    });
                }
            }

            return bos;
        }

        public static IList<TreatyPricingQuotationDashboardBo> GetQuotationCaseByPic()
        {
            var reinsuranceTypes = PickListDetailService.GetByPickListId(PickListBo.ReinsuranceType);
            List<TreatyPricingQuotationDashboardBo> bos = new List<TreatyPricingQuotationDashboardBo> { };
            int statusQuoting = TreatyPricingQuotationWorkflowBo.StatusQuoting;

            using (var db = new AppDbContext(false))
            {
                var personInChargeList = db.TreatyPricingQuotationWorkflows
                    .Where(q => q.Status == statusQuoting)
                    .Select(q => q.BDPersonInChargeId).Distinct().ToList();

                foreach(var personInCharge in personInChargeList)
                {
                    string userName = db.Users.Where(q => q.Id == personInCharge)
                        .Select(q => q.FullName).FirstOrDefault();

                    var bo = new TreatyPricingQuotationDashboardBo
                    {
                        UserId = personInCharge,
                        UserName = userName,
                        TreatyPricingQuotationWorkflowBos = new List<TreatyPricingQuotationWorkflowBo> { },
                        TotalCase = 0
                    };

                    foreach (var reinsuranceType in reinsuranceTypes)
                    {
                        int totalCaseByPersonInChargeReinsuranceType = 
                            db.TreatyPricingQuotationWorkflows
                            .Where(q => q.BDPersonInChargeId == personInCharge && q.ReinsuranceTypePickListDetailId == reinsuranceType.Id && q.Status == statusQuoting)
                            .Count();

                        bo.TreatyPricingQuotationWorkflowBos.Add(new TreatyPricingQuotationWorkflowBo
                        {
                            ReinsuranceTypePickListDetailId = reinsuranceType.Id,
                            ReinsuranceType = reinsuranceType.Code,
                            NoOfCase = totalCaseByPersonInChargeReinsuranceType
                        });

                        bo.TotalCase += totalCaseByPersonInChargeReinsuranceType;
                    }

                    bos.Add(bo);
                }
            }

            #region Commented
            //using (var db = new AppDbContext(false))
            //{
            //    var Versions = db.TreatyPricingQuotationWorkflowVersions
            //        .GroupBy(q => q.TreatyPricingQuotationWorkflowId)
            //        .Select(q => q.OrderByDescending(g => g.Version).FirstOrDefault())
            //        .ToList();

            //    foreach (var Version in Versions)
            //    {
            //        if (Version.BDPersonInChargeId.HasValue && !bos.Where(q => q.UserId == Version.BDPersonInChargeId).Any())
            //        {
            //            var bo = new TreatyPricingQuotationDashboardBo
            //            {
            //                UserId = Version.BDPersonInChargeId,
            //                UserName = Version.BDPersonInCharge?.FullName,
            //                TreatyPricingQuotationWorkflowBos = new List<TreatyPricingQuotationWorkflowBo> { },
            //            };

            //            foreach (var reinsuranceType in reinsuranceTypes)
            //            {
            //                bo.TreatyPricingQuotationWorkflowBos.Add(new TreatyPricingQuotationWorkflowBo
            //                {
            //                    ReinsuranceTypePickListDetailId = reinsuranceType.Id,
            //                    ReinsuranceType = reinsuranceType.Code,
            //                });
            //            }

            //            var currentReinsuranceTypeBo = bo.TreatyPricingQuotationWorkflowBos.Where(q => q.ReinsuranceTypePickListDetailId == Version.TreatyPricingQuotationWorkflow.ReinsuranceTypePickListDetailId).FirstOrDefault();
            //            currentReinsuranceTypeBo.NoOfCase += 1;
            //            bo.TotalCase += 1;

            //            bos.Add(bo);
            //        }
            //        else
            //        {
            //            var bo = bos.Where(q => q.UserId == Version.BDPersonInChargeId).FirstOrDefault();
            //            if (bo != null)
            //            {
            //                var currentReinsuranceTypeBo = bo.TreatyPricingQuotationWorkflowBos.Where(q => q.ReinsuranceTypePickListDetailId == Version.TreatyPricingQuotationWorkflow.ReinsuranceTypePickListDetailId).FirstOrDefault();
            //                currentReinsuranceTypeBo.NoOfCase += 1;
            //                bo.TotalCase += 1;
            //            }
            //        }
            //    }
            //}
            #endregion

            return bos;
        }

        public static IList<TreatyPricingQuotationDashboardBo> GetQuotationCaseByDepartment()
        {
            IList<TreatyPricingQuotationDashboardBo> bos = new List<TreatyPricingQuotationDashboardBo>();
            int statusQuoting = TreatyPricingQuotationWorkflowBo.StatusQuoting;
            var statuses = TreatyPricingQuotationWorkflowVersionQuotationChecklistService.GetChecklistStatusBosForDashboard();
            List<string> departments = new List<string>();

            for (int i = 0; i < 7; i++)
            {
                departments.Add(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(i+1));
            }

            using (var db = new AppDbContext(false))
            {
                foreach (var department in departments)
                {
                    List<int> caseCountByDepartmentStatus = new List<int>();
                    int totalCaseByDepartment = 0;

                    foreach (var status in statuses)
                    {
                        int count = 0;

                        if (department == "CEO")
                            count = db.TreatyPricingQuotationWorkflows.Where(q => q.CEOPending == status.Status && q.Status == statusQuoting).Count();

                        if (department == "Business Development & Group")
                            count = db.TreatyPricingQuotationWorkflows.Where(q => q.BDPending == status.Status && q.Status == statusQuoting).Count();

                        if (department == "Product Pricing")
                            count = db.TreatyPricingQuotationWorkflows.Where(q => q.PricingPending == status.Status && q.Status == statusQuoting).Count();

                        if (department == "Underwriting")
                            count = db.TreatyPricingQuotationWorkflows.Where(q => q.UnderwritingPending == status.Status && q.Status == statusQuoting).Count();

                        if (department == "Health")
                            count = db.TreatyPricingQuotationWorkflows.Where(q => q.HealthPending == status.Status && q.Status == statusQuoting).Count();

                        if (department == "Claims")
                            count = db.TreatyPricingQuotationWorkflows.Where(q => q.ClaimsPending == status.Status && q.Status == statusQuoting).Count();

                        if (department == "Compliance & Risk")
                            count = db.TreatyPricingQuotationWorkflows.Where(q => q.TGPending == status.Status && q.Status == statusQuoting).Count();

                        caseCountByDepartmentStatus.Add(count);
                        totalCaseByDepartment += count;
                    }

                    if (totalCaseByDepartment > 0)
                    {
                        var bo = new TreatyPricingQuotationDashboardBo
                        {
                            DepartmentName = department,
                            DepartmentId = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamId(department),
                            CaseCountByDepartmentStatus = caseCountByDepartmentStatus,
                            TotalCase = totalCaseByDepartment,
                        };
                        bos.Add(bo);
                    }
                }
            }

            return bos;
        }

        #region GetQuotationCaseByDepartment (backup)
        //public static IList<TreatyPricingQuotationDashboardBo> GetQuotationCaseByDepartment()
        //{
        //    IList<TreatyPricingQuotationDashboardBo> bos = new List<TreatyPricingQuotationDashboardBo>();
        //    var reinsuranceTypes = PickListDetailService.GetByPickListId(PickListBo.ReinsuranceType);
        //    List<string> departments = new List<string>();

        //    for (int i = 0; i < 7; i++)
        //    {
        //        departments.Add(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamName(i + 1));
        //    }

        //    using (var db = new AppDbContext(false))
        //    {
        //        foreach (var department in departments)
        //        {
        //            List<int> caseCountByDepartmentReinsuranceType = new List<int>();
        //            int totalCaseByDepartment = 0;

        //            var quotationBos = GetForQuotationDashboardPendingDepartmentReview(department);

        //            foreach (var reinsuranceType in reinsuranceTypes)
        //            {
        //                int count = 0;

        //                foreach (var quotationBo in quotationBos)
        //                {
        //                    if (quotationBo.ReinsuranceTypePickListDetailId == reinsuranceType.Id)
        //                    {
        //                        count++;
        //                        totalCaseByDepartment++;
        //                    }
        //                }

        //                caseCountByDepartmentReinsuranceType.Add(count);
        //            }

        //            if (totalCaseByDepartment > 0)
        //            {
        //                var bo = new TreatyPricingQuotationDashboardBo
        //                {
        //                    DepartmentName = department,
        //                    DepartmentId = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetInternalTeamId(department),
        //                    CaseCountByDepartmentReinsuranceType = caseCountByDepartmentReinsuranceType,
        //                    TotalCase = totalCaseByDepartment,
        //                };
        //                bos.Add(bo);
        //            }
        //        }
        //    }

        //    return bos;
        //}

        //public static IList<TreatyPricingQuotationWorkflowBo> GetForQuotationDashboardPendingDepartmentReview(string department)
        //{
        //    IList<TreatyPricingQuotationWorkflowBo> bos = new List<TreatyPricingQuotationWorkflowBo>();
        //    var versionBos = TreatyPricingQuotationWorkflowVersionService.GetLatestVersionBos();

        //    int statusRequested = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusRequested;
        //    int statusPendingSignOff = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusPendingSignOff;

        //    foreach (var versionBo in versionBos)
        //    {
        //        var checklistBo = TreatyPricingQuotationWorkflowVersionQuotationChecklistService.GetByDepartmentAndVersion(department, versionBo.Id);

        //        if (checklistBo != null && (checklistBo.Status == statusRequested || checklistBo.Status == statusPendingSignOff))
        //        {
        //            var bo = TreatyPricingQuotationWorkflowService.Find(versionBo.TreatyPricingQuotationWorkflowId);
        //            bos.Add(bo);
        //        }
        //    }

        //    return bos;
        //}
        #endregion

        public static IList<TreatyPricingQuotationWorkflowBo> GetByCedantReinsuranceType(int cedantId, int reinsuranceTypePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingQuotationWorkflows
                    .Where(q => q.CedantId == cedantId && q.ReinsuranceTypePickListDetailId == reinsuranceTypePickListDetailId)
                    .ToList());
            }
        }

        public static IList<TreatyPricingQuotationDashboardBo> GetCompanyActiveCases()
        {
            IList<TreatyPricingQuotationDashboardBo> bos = new List<TreatyPricingQuotationDashboardBo>();

            using (var db = new AppDbContext(false))
            {
                var viewList = db.QuotationDashboardActiveCasesByCompany.ToList();

                foreach (var viewItem in viewList)
                {
                    int expectedRIPremiumReceivable = 0;

                    if (Int32.TryParse(viewItem.ExpectedRiPremium.ToString(), out int expectedRIPremium))
                        expectedRIPremiumReceivable = expectedRIPremium;

                    var bo = new TreatyPricingQuotationDashboardBo
                    {
                        CedantId = viewItem.CedantId,
                        CedantName = viewItem.CedantName,
                        ReinsuranceTypePickListDetailId = viewItem.ReinsuranceTypeId,
                        ReinsuranceType = viewItem.ReinsuranceTypeName,
                        TotalCase = viewItem.QuotingCaseCount.HasValue ? viewItem.QuotingCaseCount.Value : 0,
                        NoOfQuotedCaseExceed14 = viewItem.QuotedExceeded14Days,
                        ExpectedRIPremiumReceivable = expectedRIPremiumReceivable,
                    };
                    bos.Add(bo);
                }
            }

            #region Commented
            //var cedantBos = TreatyPricingCedantService.Get();
            //string today = DateTime.Now.ToString(Util.GetDateFormat());
            //int workflowType = TreatyPricingWorkflowObjectBo.TypeQuotation;
            //int objectType = TreatyPricingWorkflowObjectBo.ObjectTypeProduct;

            //foreach (var cedantBo in cedantBos)
            //{
            //    var bosByCedant = GetByCedantReinsuranceType(cedantBo.CedantId, cedantBo.ReinsuranceTypePickListDetailId);
            //    int totalQuotingCase = 0;
            //    int totalQuotedExceed14 = 0;
            //    int expectedRIPremiumReceivable = 0;

            //    foreach (var boByCedant in bosByCedant)
            //    {
            //        if (boByCedant.Status == TreatyPricingQuotationWorkflowBo.StatusQuoting)
            //            totalQuotingCase++;

            //        if (boByCedant.Status == TreatyPricingQuotationWorkflowBo.StatusQuoted && CalculateDateRange(boByCedant.FinaliseDateStr, today, true) >= 14)
            //            totalQuotedExceed14++;

            //        var objectBo = TreatyPricingWorkflowObjectService.GetByTypeWorkflowIdObjectType(workflowType, boByCedant.Id, objectType);
            //        if (objectBo != null)
            //        {
            //            var productVersionBo = TreatyPricingProductVersionService.Find(objectBo.ObjectVersionId);
            //            if (productVersionBo != null && productVersionBo.ExpectedRiPremium != null)
            //            {
            //                if (Int32.TryParse(productVersionBo.ExpectedRiPremium.Replace(",", ""), out int expectedRIPremium))
            //                {
            //                    if (boByCedant.Status == TreatyPricingQuotationWorkflowBo.StatusQuoting)
            //                        expectedRIPremiumReceivable += expectedRIPremium;
            //                }
            //            }
            //        }
            //    }

            //    var bo = new TreatyPricingQuotationDashboardBo
            //    {
            //        CedantId = cedantBo.CedantId,
            //        CedantName = cedantBo.CedantBo.Name,
            //        ReinsuranceTypePickListDetailId = cedantBo.ReinsuranceTypePickListDetailId,
            //        ReinsuranceType = cedantBo.ReinsuranceTypePickListDetailBo.Description,
            //        TotalCase = totalQuotingCase,
            //        NoOfQuotedCaseExceed14 = totalQuotedExceed14,
            //        ExpectedRIPremiumReceivable = expectedRIPremiumReceivable,
            //    };
            //    bos.Add(bo);
            //}
            #endregion

            return bos;
        }
        #endregion

        #region Pricing Dashboard
        public static IList<TreatyPricingQuotationWorkflowBo> GetQuotationCaseByPricingStatus(int pricingTeamPickListDetailId)
        {
            List<int> pricingStatuses = new List<int>();

            for (int i = 0; i < TreatyPricingQuotationWorkflowBo.PricingStatusMax; i++)
            {
                pricingStatuses.Add(i + 1);
            }

            List<TreatyPricingQuotationWorkflowBo> bos = new List<TreatyPricingQuotationWorkflowBo> { };

            using (var db = new AppDbContext(false))
            {
                foreach (int pricingStatus in pricingStatuses)
                {
                    var noOfCase = db.TreatyPricingQuotationWorkflows
                        .Where(q => q.PricingStatus == pricingStatus && q.PricingTeamPickListDetailId == pricingTeamPickListDetailId)
                        .Count();

                    bos.Add(new TreatyPricingQuotationWorkflowBo
                    {
                        PricingStatus = pricingStatus,
                        PricingStatusName = TreatyPricingQuotationWorkflowBo.GetPricingStatusName(pricingStatus),
                        NoOfCase = noOfCase,
                    });
                }
            }

            return bos;
        }

        public static IList<TreatyPricingQuotationDashboardBo> GetQuotationCaseByPricingPic(int pricingTeamPickListDetailId)
        {
            List<TreatyPricingQuotationDashboardBo> bos = new List<TreatyPricingQuotationDashboardBo> { };

            using (var db = new AppDbContext(false))
            {
                var viewResultSet = db.PricingDashboardOutstandingPricingOverviewPIC
                    .Where(q => q.PricingTeamPickListDetailId == pricingTeamPickListDetailId)
                    .OrderBy(g => g.UserName)
                    .ToList();

                foreach (var viewResult in viewResultSet)
                {
                    var bo = new TreatyPricingQuotationDashboardBo
                    {
                        UserId = viewResult.UserId,
                        UserName = viewResult.UserName,
                        TreatyPricingQuotationWorkflowBos = new List<TreatyPricingQuotationWorkflowBo> { },
                    };

                    #region Pricing Status handling
                    int statusUnassigned = TreatyPricingQuotationWorkflowBo.PricingStatusUnassigned;
                    int statusAssessmentInProgress = TreatyPricingQuotationWorkflowBo.PricingStatusAssessmentInProgress;
                    int statusPendingTechReview = TreatyPricingQuotationWorkflowBo.PricingStatusPendingTechReview;
                    int statusPendingPeerReview = TreatyPricingQuotationWorkflowBo.PricingStatusPendingPeerReview;
                    int statusPendingPricingAuthorityReview = TreatyPricingQuotationWorkflowBo.PricingStatusPendingPricingAuthorityReview;
                    int statusToUpdateRepo = TreatyPricingQuotationWorkflowBo.PricingStatusToUpdateRepo;
                    int statusUpdatedRepo = TreatyPricingQuotationWorkflowBo.PricingStatusUpdatedRepo;

                    bo.TreatyPricingQuotationWorkflowBos.Add(new TreatyPricingQuotationWorkflowBo
                    {
                        PricingStatus = statusUnassigned,
                        PricingStatusName = TreatyPricingQuotationWorkflowBo.GetPricingStatusName(statusUnassigned),
                        NoOfCase = viewResult.Unassigned.HasValue ? viewResult.Unassigned.Value : 0,
                    });

                    bo.TreatyPricingQuotationWorkflowBos.Add(new TreatyPricingQuotationWorkflowBo
                    {
                        PricingStatus = statusAssessmentInProgress,
                        PricingStatusName = TreatyPricingQuotationWorkflowBo.GetPricingStatusName(statusAssessmentInProgress),
                        NoOfCase = viewResult.AssessmentInProgress.HasValue ? viewResult.AssessmentInProgress.Value : 0,
                    });

                    bo.TreatyPricingQuotationWorkflowBos.Add(new TreatyPricingQuotationWorkflowBo
                    {
                        PricingStatus = statusPendingTechReview,
                        PricingStatusName = TreatyPricingQuotationWorkflowBo.GetPricingStatusName(statusPendingTechReview),
                        NoOfCase = viewResult.PendingTechReview.HasValue ? viewResult.PendingTechReview.Value : 0,
                    });

                    bo.TreatyPricingQuotationWorkflowBos.Add(new TreatyPricingQuotationWorkflowBo
                    {
                        PricingStatus = statusPendingPeerReview,
                        PricingStatusName = TreatyPricingQuotationWorkflowBo.GetPricingStatusName(statusPendingPeerReview),
                        NoOfCase = viewResult.PendingPeerReview.HasValue ? viewResult.PendingPeerReview.Value : 0,
                    });

                    bo.TreatyPricingQuotationWorkflowBos.Add(new TreatyPricingQuotationWorkflowBo
                    {
                        PricingStatus = statusPendingPricingAuthorityReview,
                        PricingStatusName = TreatyPricingQuotationWorkflowBo.GetPricingStatusName(statusPendingPricingAuthorityReview),
                        NoOfCase = viewResult.PendingPricingAuthorityReview.HasValue ? viewResult.PendingPricingAuthorityReview.Value : 0,
                    });

                    bo.TreatyPricingQuotationWorkflowBos.Add(new TreatyPricingQuotationWorkflowBo
                    {
                        PricingStatus = statusToUpdateRepo,
                        PricingStatusName = TreatyPricingQuotationWorkflowBo.GetPricingStatusName(statusToUpdateRepo),
                        NoOfCase = viewResult.ToUpdateRepo.HasValue ? viewResult.ToUpdateRepo.Value : 0,
                    });

                    bo.TreatyPricingQuotationWorkflowBos.Add(new TreatyPricingQuotationWorkflowBo
                    {
                        PricingStatus = statusUpdatedRepo,
                        PricingStatusName = TreatyPricingQuotationWorkflowBo.GetPricingStatusName(statusUpdatedRepo),
                        NoOfCase = viewResult.UpdatedRepo.HasValue ? viewResult.UpdatedRepo.Value : 0,
                    });
                    #endregion

                    bos.Add(bo);
                }
            }

            return bos;

            #region Commented
            //List<int> pricingStatuses = new List<int>();

            //for (int i = 0; i < TreatyPricingQuotationWorkflowBo.PricingStatusMax; i++)
            //{
            //    pricingStatuses.Add(i + 1);
            //}

            //List<TreatyPricingQuotationDashboardBo> bos = new List<TreatyPricingQuotationDashboardBo> { };

            //using (var db = new AppDbContext(false))
            //{
            //    var Versions = db.TreatyPricingQuotationWorkflowVersions
            //        .Where(q => q.TreatyPricingQuotationWorkflow.PricingTeamPickListDetailId == pricingTeamPickListDetailId)
            //        .GroupBy(q => q.TreatyPricingQuotationWorkflowId)
            //        .Select(q => q.OrderByDescending(g => g.Version).FirstOrDefault())
            //        .ToList();

            //    foreach (var Version in Versions)
            //    {
            //        if (Version.PersonInChargeId.HasValue && !bos.Where(q => q.UserId == Version.PersonInChargeId).Any())
            //        {
            //            var bo = new TreatyPricingQuotationDashboardBo
            //            {
            //                UserId = Version.PersonInChargeId,
            //                UserName = Version.PersonInCharge?.FullName,
            //                TreatyPricingQuotationWorkflowBos = new List<TreatyPricingQuotationWorkflowBo> { },
            //            };

            //            foreach (var pricingStatus in pricingStatuses)
            //            {
            //                bo.TreatyPricingQuotationWorkflowBos.Add(new TreatyPricingQuotationWorkflowBo
            //                {
            //                    PricingStatus = pricingStatus,
            //                    PricingStatusName = TreatyPricingQuotationWorkflowBo.GetPricingStatusName(pricingStatus),
            //                });
            //            }

            //            var currentPricingStatusBo = bo.TreatyPricingQuotationWorkflowBos.Where(q => q.PricingStatus == Version.TreatyPricingQuotationWorkflow.PricingStatus).FirstOrDefault();
            //            currentPricingStatusBo.NoOfCase += 1;
            //            bo.TotalCase += 1;

            //            bos.Add(bo);
            //        }
            //        else
            //        {
            //            var bo = bos.Where(q => q.UserId == Version.PersonInChargeId).FirstOrDefault();
            //            if (bo != null)
            //            {
            //                var currentPricingStatusBo = bo.TreatyPricingQuotationWorkflowBos.Where(q => q.PricingStatus == Version.TreatyPricingQuotationWorkflow.PricingStatus).FirstOrDefault();
            //                currentPricingStatusBo.NoOfCase += 1;
            //                bo.TotalCase += 1;
            //            }
            //        }
            //    }
            //}

            //return bos;
            #endregion
        }

        public static IList<TreatyPricingQuotationWorkflowBo> GetQuotationCaseForDueDateOverview(int pricingTeamPickListDetailId)
        {
            List<TreatyPricingQuotationWorkflowBo> bos = new List<TreatyPricingQuotationWorkflowBo> { };
            
            using (var db = new AppDbContext(false))
            {
                var viewResultSet = db.PricingDashboardDueDateOverview
                    .Where(q => q.PricingTeamPickListDetailId == pricingTeamPickListDetailId)
                    .OrderBy(q => q.DueDateOverviewType)
                    .ToList();

                foreach (var viewResult in viewResultSet)
                {
                    bos.Add(new TreatyPricingQuotationWorkflowBo
                    {
                        DueDateOverviewType = viewResult.DueDateOverviewType,
                        NoOfCase = viewResult.NoOfCase.HasValue ? viewResult.NoOfCase.Value : 0,
                    });
                }
            }

            return bos;

            #region Commented
            //List<TreatyPricingQuotationWorkflowBo> bos = new List<TreatyPricingQuotationWorkflowBo> { };
            //string today = DateTime.Now.ToString(Util.GetDateFormat());

            //using (var db = new AppDbContext(false))
            //{
            //    var quotations = db.TreatyPricingQuotationWorkflows
            //        .Where(q => q.PricingTeamPickListDetailId == pricingTeamPickListDetailId).ToList();

            //    for (int i = 0; i < 3; i++)
            //    {
            //        int noOfCase = 0;

            //        foreach (var quotation in quotations)
            //        {
            //            string dueDateString = quotation.PricingDueDate.HasValue ? quotation.PricingDueDate.Value.ToString(Util.GetDateFormat()) : today;
            //            int dateRange = CalculateDateRange(dueDateString, today, true);

            //            if ((i == 0 && dateRange <= 5)
            //                || (i == 1 && dateRange > 5 && dateRange <= 10)
            //                || (i == 2 && dateRange > 10))
            //                noOfCase++;
            //        }

            //        bos.Add(new TreatyPricingQuotationWorkflowBo
            //        {
            //            DueDateOverviewType = i + 1,
            //            NoOfCase = noOfCase,
            //        });
            //    }
            //}

            //return bos;
            #endregion
        }

        public static List<int> GetIdsFilteredByDueDateOverview(int pricingTeamPickListDetailId, int dueDateOverviewType)
        {
            List<int> idList = new List<int>();

            using (var db = new AppDbContext(false))
            {
                var viewResultSet = db.PricingDashboardDueDateOverviewIdList
                    .Where(q => q.PricingTeamPickListDetailId == pricingTeamPickListDetailId
                    && q.DueDateOverviewType == dueDateOverviewType).ToList();

                foreach (var viewResult in viewResultSet)
                {
                    if (viewResult.QuotationId.HasValue)
                        idList.Add(viewResult.QuotationId.Value);
                }
            }

            return idList;

            #region Commented
            //List<int> idList = new List<int>();
            //string today = DateTime.Now.ToString(Util.GetDateFormat());

            //using (var db = new AppDbContext(false))
            //{
            //    var bos = FormBos(db.TreatyPricingQuotationWorkflows
            //        .Where(q => q.PricingTeamPickListDetailId == pricingTeamPickListDetailId).ToList());

            //    foreach (var bo in bos)
            //    {
            //        string dueDateString = bo.PricingDueDate.HasValue ? bo.PricingDueDate.Value.ToString(Util.GetDateFormat()) : today;
            //        int dateRange = CalculateDateRange(dueDateString, today, true);

            //        if ((dueDateOverviewType == 1 && dateRange <= 5)
            //            || (dueDateOverviewType == 2 && dateRange > 5 && dateRange <= 10)
            //            || (dueDateOverviewType == 3 && dateRange > 10))
            //            idList.Add(bo.Id);
            //    }
            //}

            //return idList;
            #endregion
        }

        public static IList<TreatyPricingQuotationDashboardBo> GetQuotationCaseByPricingPicForDueDateOverview(int pricingTeamPickListDetailId)
        {
            List<TreatyPricingQuotationDashboardBo> bos = new List<TreatyPricingQuotationDashboardBo> { };

            using (var db = new AppDbContext(false))
            {
                var viewResultSet = db.PricingDashboardDueDateOverviewPIC
                    .Where(q => q.PricingTeamPickListDetailId == pricingTeamPickListDetailId && q.UserId != null)
                    .OrderBy(q => q.UserName)
                    .ToList();

                foreach (var viewResult in viewResultSet)
                {
                    bos.Add(new TreatyPricingQuotationDashboardBo
                    {
                        UserId = viewResult.UserId,
                        UserName = viewResult.UserName,
                        NoOfCaseDueDateBelow5 = viewResult.NoOfCaseDueDateBelow5,
                        NoOfCaseDueDate6To10 = viewResult.NoOfCaseDueDate6To10,
                        NoOfCaseDueDateExceed10 = viewResult.NoOfCaseDueDateExceed10,
                    });
                }
            }

            return bos;

            #region Commented
            //List<TreatyPricingQuotationDashboardBo> bos = new List<TreatyPricingQuotationDashboardBo> { };
            //string today = DateTime.Now.ToString(Util.GetDateFormat());

            //using (var db = new AppDbContext(false))
            //{
            //    var personInChargeIds = db.TreatyPricingQuotationWorkflows
            //        .Where(q => q.PricingTeamPickListDetailId == pricingTeamPickListDetailId && q.PersonInChargeId != null)
            //        .Select(q => q.PersonInChargeId)
            //        .Distinct()
            //        .ToList();

            //    var quotations = db.TreatyPricingQuotationWorkflows
            //        .Where(q => q.PricingTeamPickListDetailId == pricingTeamPickListDetailId).ToList();

            //    if (personInChargeIds.Count > 0)
            //    {
            //        foreach (int personInChargeId in personInChargeIds)
            //        {
            //            int value1 = 0;
            //            int value2 = 0;
            //            int value3 = 0;

            //            foreach (var quotation in quotations)
            //            {
            //                if (quotation.PersonInChargeId == personInChargeId)
            //                {
            //                    string dueDateString = quotation.PricingDueDate.HasValue ? quotation.PricingDueDate.Value.ToString(Util.GetDateFormat()) : today;
            //                    int dateRange = CalculateDateRange(dueDateString, today, true);

            //                    for (int i = 0; i < 3; i++)
            //                    {
            //                        if (i == 0 && dateRange <= 5)
            //                            value1++;

            //                        if (i == 1 && dateRange > 5 && dateRange <= 10)
            //                            value2++;

            //                        if (i == 2 && dateRange > 10)
            //                            value3++;
            //                    }
            //                }
            //            }

            //            if (value1 > 0 || value2 > 0 || value3 > 0)
            //            {
            //                bos.Add(new TreatyPricingQuotationDashboardBo
            //                {
            //                    UserId = personInChargeId,
            //                    UserName = UserService.Find(personInChargeId).FullName,
            //                    NoOfCaseDueDateBelow5 = value1,
            //                    NoOfCaseDueDate6To10 = value2,
            //                    NoOfCaseDueDateExceed10 = value3,
            //                });
            //            }
            //        }
            //    }
            //}

            //return bos;
            #endregion
        }

        public static IList<TreatyPricingQuotationDashboardBo> GetPricingCompanyActiveCases(int pricingTeamPickListDetailId)
        {
            IList<TreatyPricingQuotationDashboardBo> bos = new List<TreatyPricingQuotationDashboardBo>();
            int statusQuoting = TreatyPricingQuotationWorkflowBo.StatusQuoting;

            using (var db = new AppDbContext(false))
            {
                var viewResultSet = db.PricingDashboardQuotingCasesByCompany
                    .Where(q => q.PricingTeamPickListDetailId == pricingTeamPickListDetailId && q.CedantId != null)
                    .OrderBy(q => q.CedantName)
                    .ToList();

                foreach (var viewResult in viewResultSet)
                {
                    int? expectedRIPremiumReceivable = 0;
                    if (Int32.TryParse(viewResult.ExpectedRiPremium.ToString(), out int expectedRIPremium))
                        expectedRIPremiumReceivable = expectedRIPremium;

                    var bo = new TreatyPricingQuotationDashboardBo
                    {
                        Status = statusQuoting,
                        CedantId = viewResult.CedantId,
                        CedantName = viewResult.CedantName,
                        TotalCase = viewResult.QuotingCaseCount.HasValue ? viewResult.QuotingCaseCount.Value : 0,
                        ExpectedRIPremiumReceivable = expectedRIPremiumReceivable,
                    };
                    bos.Add(bo);
                }
            }

            return bos;

            #region Commented
            //IList<TreatyPricingQuotationDashboardBo> bos = new List<TreatyPricingQuotationDashboardBo>();
            //int quoting = TreatyPricingQuotationWorkflowBo.StatusQuoting;
            //int workflowType = TreatyPricingWorkflowObjectBo.TypeQuotation;
            //int objectType = TreatyPricingWorkflowObjectBo.ObjectTypeProduct;

            //using (var db = new AppDbContext(false))
            //{
            //    var cedants = db.Cedants.OrderBy(q => q.Name).ToList();

            //    foreach (var cedant in cedants)
            //    {
            //        var quotations = db.TreatyPricingQuotationWorkflows
            //            .Where(q => q.Status == quoting && q.CedantId == cedant.Id
            //            && q.PricingTeamPickListDetailId == pricingTeamPickListDetailId)
            //            .ToList();

            //        int totalQuotingCase = quotations.Count;
            //        int expectedRIPremiumReceivable = 0;

            //        if (totalQuotingCase > 0)
            //        {
            //            foreach (var quotation in quotations)
            //            {
            //                var objectBo = TreatyPricingWorkflowObjectService.GetByTypeWorkflowIdObjectType(workflowType, quotation.Id, objectType);
            //                if (objectBo != null)
            //                {
            //                    var productVersionBo = TreatyPricingProductVersionService.Find(objectBo.ObjectVersionId);
            //                    if (productVersionBo != null && productVersionBo.ExpectedRiPremium != null)
            //                    {
            //                        if (Int32.TryParse(productVersionBo.ExpectedRiPremium.Replace(",", ""), out int expectedRIPremium))
            //                        {
            //                            expectedRIPremiumReceivable += expectedRIPremium;
            //                        }
            //                    }
            //                }
            //            }

            //            var bo = new TreatyPricingQuotationDashboardBo
            //            {
            //                Status = quoting,
            //                CedantId = cedant.Id,
            //                CedantName = cedant.Name,
            //                TotalCase = totalQuotingCase,
            //                ExpectedRIPremiumReceivable = expectedRIPremiumReceivable,// / totalQuotingCase,
            //            };
            //            bos.Add(bo);
            //        }
            //    }
            //}

            //return bos;
            #endregion
        }
        #endregion

        public static Result Save(ref TreatyPricingQuotationWorkflowBo bo)
        {
            if (!TreatyPricingQuotationWorkflow.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingQuotationWorkflowBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingQuotationWorkflow.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingQuotationWorkflowBo bo)
        {
            TreatyPricingQuotationWorkflow entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingQuotationWorkflowBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingQuotationWorkflowBo bo)
        {
            Result result = Result();

            TreatyPricingQuotationWorkflow entity = TreatyPricingQuotationWorkflow.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.CedantId = bo.CedantId;
                entity.QuotationId = bo.QuotationId;
                entity.ReinsuranceTypePickListDetailId = bo.ReinsuranceTypePickListDetailId;
                entity.Name = bo.Name;
                entity.Summary = bo.Summary;
                entity.Status = bo.Status;
                entity.StatusRemarks = bo.StatusRemarks;
                entity.TargetSendDate = bo.TargetSendDate;
                entity.LatestRevisionDate = bo.LatestRevisionDate;
                entity.PricingTeamPickListDetailId = bo.PricingTeamPickListDetailId;
                entity.PricingStatus = bo.PricingStatus;
                entity.TargetClientReleaseDate = bo.TargetClientReleaseDate;
                entity.TargetRateCompletionDate = bo.TargetRateCompletionDate;
                entity.FinaliseDate = bo.FinaliseDate;
                entity.Description = bo.Description;
                entity.LatestVersion = bo.LatestVersion;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                //Quotation & Pricing dashboard
                entity.BDPersonInChargeId = bo.BDPersonInChargeId;
                entity.PersonInChargeId = bo.PersonInChargeId;
                entity.CEOPending = bo.CEOPending;
                entity.PricingPending = bo.PricingPending;
                entity.UnderwritingPending = bo.UnderwritingPending;
                entity.HealthPending = bo.HealthPending;
                entity.ClaimsPending = bo.ClaimsPending;
                entity.BDPending = bo.BDPending;
                entity.TGPending = bo.TGPending;
                entity.PricingDueDate = bo.PricingDueDate;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingQuotationWorkflowBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingQuotationWorkflowBo bo)
        {
            TreatyPricingQuotationWorkflow.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingQuotationWorkflowBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingQuotationWorkflow.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static int GetCurrentQuotationIdCount(string prefix)
        {
            using (var db = new AppDbContext())
            {
                TreatyPricingQuotationWorkflow quotationWorkflow = db.TreatyPricingQuotationWorkflows.Where(q => !string.IsNullOrEmpty(q.QuotationId) && q.QuotationId.StartsWith(prefix)).OrderByDescending(q => q.QuotationId).FirstOrDefault();

                int count = 0;
                if (quotationWorkflow != null)
                {
                    string quotationId = quotationWorkflow.QuotationId;
                    string[] quotationIdInfo = quotationId.Split('_');

                    string countStr;
                    if (quotationIdInfo.Length == 3)
                    {
                        countStr = quotationIdInfo[2];
                        int.TryParse(countStr, out count);
                    }
                }

                return count;
            }
        }
    }
}
