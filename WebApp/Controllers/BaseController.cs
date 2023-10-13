using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using DataAccess.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Services;
using Services.Identity;
using Services.Retrocession;
using Services.Sanctions;
using Services.TreatyPricing;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected readonly AppDbContext _db = new AppDbContext();
        protected AppUserManager _userManager;
        protected AppSignInManager _signInManager;
        protected int _authUserId;
        protected User _authUser;

        public const int ErrorTypeAccessDenied = 1;

        public ReportViewer RptViewer { get; set; }

        [Obsolete]
        public void Report(string reportPath, bool hideParameter = false, ReportParameter[] param = null, bool noPdf = false)
        {
            RptViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Remote,
                SizeToReportContent = true,
                ZoomMode = ZoomMode.FullPage,
                //Width = Unit.Percentage(100),
                //Height = Unit.Percentage(100),
                AsyncRendering = true
            };

            string url = Util.GetConfig("ReportServerUrl");
            string username = Util.GetConfig("ReportServerUsername");
            string password = Util.GetConfig("ReportServerPassword");

            //set report server credential here
            ReportServerCredentials cred = new ReportServerCredentials(username, password, "");
            RptViewer.ServerReport.ReportServerCredentials = cred;

            // server url
            RptViewer.ServerReport.ReportServerUrl = new Uri(url);
            RptViewer.ServerReport.ReportPath = reportPath;
            RptViewer.LinkActiveColor = Color.FromArgb(1, 80, 159);
            RptViewer.LinkDisabledColor = Color.FromArgb(1, 80, 159);
            RptViewer.SplitterBackColor = Color.FromArgb(243, 248, 254);
            RptViewer.ToolbarHoverBackgroundColor = Color.FromArgb(243, 248, 254);
            RptViewer.ToolBarItemPressedBorderColor = Color.FromArgb(243, 248, 254);
            RptViewer.ToolbarDividerColor = Color.FromArgb(243, 248, 254);
            RptViewer.ToolBarItemBorderColor = Color.FromArgb(243, 248, 254);
            RptViewer.Style.Add("overflow", "scroll");

            if (hideParameter)
            {
                RptViewer.ShowParameterPrompts = false;

                if (param.Length > 0)
                {
                    RptViewer.ServerReport.SetParameters(param);
                }
            }

            //Way to pass multi value param
            //ReportParameter[] param = new ReportParameter[1];
            //string[] values = new string[] { "0", "1", "2" };
            //param[0] = new ReportParameter("Status", values);
            //RptViewer.ServerReport.SetParameters(param);

            RptViewer.ServerReport.Refresh();

            if (noPdf == true)
            {
                ViewBag.NoPDF = "true";
            }

            ViewBag.RptViewer = RptViewer;
        }

        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().Get<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public AppSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AppSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return User.Identity.IsAuthenticated;
            }
        }

        public int AuthUserId
        {
            get
            {
                return _authUserId == 0 ? User.Identity.GetUserId<int>() : _authUserId;
            }
            private set
            {
                _authUserId = value;
            }
        }

        public User AuthUser
        {
            get
            {
                return _authUser ?? _db.Users.Find(User.Identity.GetUserId<int>());
            }
            private set
            {
                _authUser = value;
            }
        }

        public int PageSize
        {
            get
            {
                int pageSize;
                try
                {
                    pageSize = int.Parse(Util.GetConfig("PageSize"));
                }
                catch (Exception)
                {
                    pageSize = 10;
                }
                ViewBag.PageSize = pageSize;
                return pageSize;
            }
        }

        public int SummaryPageSize
        {
            get
            {
                int summaryPageSize;
                try
                {
                    summaryPageSize = int.Parse(Util.GetConfig("SummaryPageSize"));
                }
                catch (Exception)
                {
                    summaryPageSize = 10;
                }
                ViewBag.SummaryPageSize = summaryPageSize;
                return summaryPageSize;
            }
        }

        public bool ReadPower { get; set; } = false;
        public bool UpdatePower { get; set; } = false;
        public bool ReadOnly { get; set; } = false;
        public Result Result { get; set; }
        public TrailObject TrailObject { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string WarningMessage { get; set; }
        public List<string> SuccessMessages { get; set; } = new List<string> { };
        public List<string> ErrorMessages { get; set; } = new List<string> { };
        public List<string> WarningMessages { get; set; } = new List<string> { };
        public EmailBo EmailBo { get; set; }

        public ModuleBo ModuleBo { get; set; }

        protected string GetIpAddress()
        {
            string ip;
            ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (ip == "" || ip == null)
                ip = Request.ServerVariables["REMOTE_ADDR"];

            return ip;
        }

        protected string GetSortParam(string field)
        {
            string asc = Html.GetSortAsc(field);
            string desc = Html.GetSortDsc(field);
            return ViewBag.SortOrder == asc ? desc : asc;
        }

        protected string GetSortParamTab1(string field)
        {
            string asc = Html.GetSortAsc(field);
            string desc = Html.GetSortDsc(field);
            return ViewBag.SortOrderTab1 == asc ? desc : asc;
        }

        protected bool CheckPower(string controller, string power)
        {
            return UserService.CheckPowerFlag(AuthUser, controller, power);
        }

        protected bool CheckEditPageReadOnly(string controller)
        {
            ViewBag.ReadOnly = false;
            ReadPower = UserService.CheckPowerFlag(AuthUser, controller, AccessMatrixBo.AccessMatrixCRUD.R.ToString());
            UpdatePower = UserService.CheckPowerFlag(AuthUser, controller, AccessMatrixBo.AccessMatrixCRUD.U.ToString());
            if (ReadPower == false && UpdatePower == false)
            {
                return false;
            }
            if (ReadPower == true && UpdatePower == false)
            {
                ReadOnly = true;
                ViewBag.ReadOnly = true;
            }
            return true;
        }

        protected bool CheckObjectLockReadOnly(string controller, int objectId, bool isEditMode)
        {
            ViewBag.ReadOnly = true;
            ReadPower = UserService.CheckPowerFlag(AuthUser, controller, AccessMatrixBo.AccessMatrixCRUD.R.ToString());
            UpdatePower = UserService.CheckPowerFlag(AuthUser, controller, AccessMatrixBo.AccessMatrixCRUD.U.ToString());

            if (!ReadPower && !UpdatePower)
            {
                return false;
            }

            ViewBag.CanEnterEditMode = false;
            ViewBag.IsOtherUserEditing = false;
            if (ReadPower && !UpdatePower)
            {
                ReadOnly = true;
            }
            else
            {
                if (isEditMode)
                {
                    int moduleId = ModuleService.FindByController(controller).Id;

                    var objectLockBo = ObjectLockService.Find(moduleId, objectId);
                    if (objectLockBo != null)
                    {
                        if (objectLockBo.LockedById != AuthUserId && objectLockBo.ExpiresAt > DateTime.Now)
                        {
                            ViewBag.CanEnterEditMode = true;
                            ViewBag.IsOtherUserEditing = true;
                            ViewBag.LockedErrorMessage = string.Format(MessageBag.ObjectLockedBy, objectLockBo.LockedByBo.UserName, objectLockBo.CreatedAt.ToString(Util.GetDateTimeFormat()));

                            return true;
                        }

                        if (objectLockBo.LockedById == AuthUserId)
                        {
                            objectLockBo.RefreshExpiry(AuthUserId);
                        }
                        else if (objectLockBo.ExpiresAt <= DateTime.Now)
                        {
                            ObjectLockService.Delete(objectLockBo);
                            objectLockBo = null;
                        }
                    }

                    if (objectLockBo == null)
                    {
                        // create object lock record;
                        objectLockBo = new ObjectLockBo()
                        {
                            ModuleId = moduleId,
                            ObjectId = objectId,
                            LockedById = AuthUserId,
                            CreatedById = AuthUserId
                        };
                        objectLockBo.RefreshExpiry(AuthUserId);
                    }

                    ObjectLockService.Save(ref objectLockBo);

                    ViewBag.IsEditMode = true;
                    ViewBag.ReadOnly = false;
                }
                else
                    ViewBag.CanEnterEditMode = true;
            }

            return true;
        }

        protected bool CheckObjectLock(string controller, int objectId)
        {
            var objectLockBo = ObjectLockService.Find(controller, objectId, AuthUserId);

            if (objectLockBo != null)
            {
                ViewBag.IsEditMode = true;
                return true;
            }

            SetErrorSessionMsg(MessageBag.ObjectLockLostError);
            return false;
        }

        protected bool CheckWorkgroupPower(int? cedantId = null)
        {
            if (ReadOnly)
                return false;

            if (ViewBag.ReadOnly == null)
                ViewBag.ReadOnly = false;

            bool hasEditPower = false;
            if (cedantId == null)
                hasEditPower = CedantWorkgroupUserService.IsUserExists(AuthUserId);
            else
                hasEditPower = CedantWorkgroupCedantService.CheckWorkgroupPower(AuthUserId, cedantId.Value);

            if (!hasEditPower)
            {
                ReadOnly = true;
                ViewBag.ReadOnly = true;
            }
            return hasEditPower;
        }

        protected bool CheckCutOffReadOnly(string controller, int? id = null)
        {
            if (ReadOnly)
                return false;

            if (ViewBag.ReadOnly == null)
                ViewBag.ReadOnly = false;

            bool exist = CutOffService.GetInCutOffStatusProcessing(controller, id);
            if (exist)
            {
                ReadOnly = true;
                ViewBag.ReadOnly = true;
                SetWarningSessionMsg("Some functions are currently disabled as cut off process is running");
            }
            return exist;
        }

        protected TrailObject GetNewTrailObject()
        {
            TrailObject = new TrailObject();
            return TrailObject;
        }

        protected EmailBo GetNewEmail(int type, string emailAddress, int? recipientUserId = null)
        {
            EmailBo = new EmailBo(type, emailAddress)
            {
                RecipientUserId = recipientUserId,
                CreatedById = AuthUserId,
                UpdatedById = AuthUserId,
            };
            return EmailBo;
        }

        protected bool GenerateMail(bool send = true, bool showWarning = true, string warningMessage = MessageBag.EmailError)
        {
            if (EmailBo == null)
                return false;

            Mail mail = EmailBo.GenerateMail();
            if (EmailBo.EmailAddress.IsValidEmail())
            {
                try
                {
                    if (send)
                    {
                        mail.Send();
                        EmailBo.Status = EmailBo.StatusSent;
                    }
                }
                catch
                {
                    EmailBo.Status = EmailBo.StatusFailed;
                    SetWarningSessionMsg(warningMessage);
                }
            }
            else
            {
                EmailBo.Status = EmailBo.StatusFailed;
                SetWarningSessionMsg(string.Format("Invalid email address format: {0}", EmailBo.EmailAddress));
            }

            return EmailBo.Status == EmailBo.StatusSent;
        }

        protected void SaveEmail(ref TrailObject trail, int? maskDataIndex = null)
        {
            EmailBo bo = EmailBo;
            if (maskDataIndex.HasValue)
                bo.MaskData(maskDataIndex.Value);
            Services.EmailService.Create(ref bo, ref trail);
        }

        protected ModuleBo GetModuleByController(string controller)
        {
            ModuleBo = ModuleService.FindByController(controller);
            return ModuleBo;
        }

        protected string GetModuleName(string name)
        {
            GetModuleByController(name);
            return ModuleBo == null ? name : ModuleBo.Name;
        }

        protected UserTrailBo CreateTrail(
            int objectId,
            string description,
            Result result = null,
            TrailObject trail = null,
            int? createdById = null,
            int type = UserTrailBo.TypeTrail,
            bool ignoreNull = false
        )
        {
            if (result == null)
                result = Result;
            if (trail == null)
                trail = TrailObject;
            if (createdById == null)
                createdById = AuthUserId;

            UserTrailBo userTrailBo = new UserTrailBo(
                objectId,
                description,
                result,
                trail,
                createdById.Value,
                ignoreNull
            )
            {
                Type = type,
                IpAddress = GetIpAddress(),
            };
            UserTrailService.Create(ref userTrailBo);

            return userTrailBo;
        }

        protected RedirectToRouteResult RedirectDashboard(int errorType = 1)
        {
            switch (errorType)
            {
                case ErrorTypeAccessDenied:
                    SetErrorSessionMsg(MessageBag.AccessDenied);
                    break;
            }
            return RedirectToAction("Index", "Home");
        }

        protected void DisableSessionTimeout()
        {
            Session["EnableSessionTimeout"] = false;
        }

        protected void EnableSessionTimeout()
        {
            Session["EnableSessionTimeout"] = true;
        }

        protected bool IsSessionTimeoutEnabled
        {
            get
            {
                return Session["EnableSessionTimeout"] != null ? (bool)Session["EnableSessionTimeout"] : true;
            }
        }

        protected void SetViewBagMessage()
        {
            if (Session["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = Session["SuccessMessage"];
                Session.Remove("SuccessMessage");
            }
            if (Session["SuccessMessageArr"] != null)
            {
                ViewBag.SuccessMessageArr = Session["SuccessMessageArr"];
                Session.Remove("SuccessMessageArr");
            }
            if (Session["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = Session["ErrorMessage"];
                Session.Remove("ErrorMessage");
            }
            if (Session["ErrorMessageArr"] != null)
            {
                ViewBag.ErrorMessageArr = Session["ErrorMessageArr"];
                Session.Remove("ErrorMessageArr");
            }
            if (Session["WarningMessage"] != null)
            {
                ViewBag.WarningMessage = Session["WarningMessage"];
                Session.Remove("WarningMessage");
            }
            if (Session["WarningMessageArr"] != null)
            {
                ViewBag.WarningMessageArr = Session["WarningMessageArr"];
                Session.Remove("WarningMessageArr");
            }

            if (!string.IsNullOrEmpty(SuccessMessage))
                ViewBag.SuccessMessage = SuccessMessage;

            if (!string.IsNullOrEmpty(ErrorMessage))
                ViewBag.ErrorMessage = ErrorMessage;

            if (!string.IsNullOrEmpty(WarningMessage))
                ViewBag.WarningMessage = WarningMessage;

            if (SuccessMessages.Count > 0)
                ViewBag.SuccessMessageArr = SuccessMessages;

            if (ErrorMessages.Count > 0)
                ViewBag.ErrorMessageArr = ErrorMessages;

            if (WarningMessages.Count > 0)
                ViewBag.WarningMessageArr = WarningMessages;
        }

        protected void SetSuccessSessionMsg(string msg)
        {
            Session["SuccessMessage"] = msg;
        }

        protected void SetSuccessSessionMsgArr(List<string> msgArr)
        {
            Session["SuccessMessageArr"] = msgArr;
        }

        protected void SetSuccessMsg(string msg)
        {
            SuccessMessage = msg;
        }

        protected void AddSuccessMsg(string msg)
        {
            SuccessMessages.Add(msg);
        }

        protected void SetErrorSessionMsg(string msg)
        {
            Session["ErrorMessage"] = msg;
        }

        protected void SetErrorSessionMsgArr(List<string> msgArr)
        {
            Session["ErrorMessageArr"] = msgArr;
        }

        protected void SetErrorMsg(string msg)
        {
            ErrorMessage = msg;
        }

        protected void AddErrorMsg(string msg)
        {
            ErrorMessages.Add(msg);
        }

        protected void SetWarningSessionMsg(string msg)
        {
            Session["WarningMessage"] = msg;
        }

        protected void SetWarningSessionMsgArr(List<string> msgArr)
        {
            Session["WarningMessageArr"] = msgArr;
        }

        protected void SetWarningMsg(string msg)
        {
            WarningMessage = msg;
        }

        protected void AddWarningMsg(string msg)
        {
            WarningMessages.Add(msg);
        }

        protected void SetSuccessMessage(string name = null, bool getModuleName = true, string type = "create")
        {
            string message;
            string messageWithName;
            switch (type.ToLower())
            {
                case "create":
                    message = MessageBag.CreateSuccessfully;
                    messageWithName = MessageBag.CreateSuccessfullyWithName;
                    break;
                case "update":
                    message = MessageBag.UpdateSuccessfully;
                    messageWithName = MessageBag.UpdateSuccessfullyWithName;
                    break;
                case "delete":
                    message = MessageBag.DeleteSuccessfully;
                    messageWithName = MessageBag.DeleteSuccessfullyWithName;
                    break;
                default:
                    message = MessageBag.CreateSuccessfully;
                    messageWithName = MessageBag.CreateSuccessfullyWithName;
                    break;
            }

            if (string.IsNullOrEmpty(name))
            {
                SetSuccessSessionMsg(message);
            }
            else if (getModuleName)
            {
                SetSuccessSessionMsg(string.Format(messageWithName, GetModuleName(name)));
            }
            else
            {
                SetSuccessSessionMsg(string.Format(messageWithName, name));
            }
        }

        protected void SetCreateSuccessMessage(string name = null, bool getModuleName = true)
        {
            SetSuccessMessage(name, getModuleName);
        }

        protected void SetUpdateSuccessMessage(string name = null, bool getModuleName = true)
        {
            SetSuccessMessage(name, getModuleName, "update");
        }

        protected void SetDeleteSuccessMessage(string name = null, bool getModuleName = true)
        {
            SetSuccessMessage(name, getModuleName, "delete");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }

                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected void AddResult(Result result)
        {
            if (!result.Valid)
            {
                foreach (string e in result.MessageBag.Errors)
                {
                    ModelState.AddModelError("", e);
                }
            }
        }

        protected int GetPaseSize()
        {
            int pageSize;
            try
            {
                pageSize = Int32.Parse(Util.GetConfig("PageSize"));
            }
            catch (Exception)
            {
                pageSize = 10;
            }
            return pageSize;
        }

        protected int GetSummaryPaseSize()
        {
            int summaryPageSize;
            try
            {
                summaryPageSize = Int32.Parse(Util.GetConfig("SummaryPageSize"));
            }
            catch (Exception)
            {
                summaryPageSize = 10;
            }
            return summaryPageSize;
        }

        protected string AuthUserName()
        {
            string authUserName = AuthUser.UserName;
            ViewBag.AuthUserName = authUserName;
            return authUserName;
        }

        protected List<SelectListItem> GetEmptyDropDownList(bool displaySelect = true, string emptyValue = "")
        {
            var items = new List<SelectListItem> { };
            if (displaySelect) items.Add(new SelectListItem { Text = "Please select", Value = emptyValue });
            return items;
        }

        protected void DropDownEmpty()
        {
            ViewBag.DropDownEmpty = GetEmptyDropDownList();
        }

        protected List<SelectListItem> DropDownDepartment(bool exceptShared = false, int? selectedId = null)
        {
            var items = GetEmptyDropDownList();

            IList<DepartmentBo> bos;
            if (exceptShared)
            {
                bos = DepartmentService.GetExceptShared();
            }
            else
            {
                bos = DepartmentService.Get();
            }

            foreach (var department in bos)
            {
                var selected = department.Id == selectedId;
                items.Add(new SelectListItem { Text = department.Name, Value = department.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownDepartments = items;
            return items;
        }

        protected List<SelectListItem> DropDownCedant(int? status = null, int? selectedId = null, string emptyValue = null, bool checkWorkgroup = false)
        {
            var items = GetEmptyDropDownList(emptyValue: emptyValue);

            IList<CedantBo> cedantBos;
            if (checkWorkgroup)
            {
                cedantBos = CedantService.GetByStatusWorkgroup(AuthUserId, status, selectedId: selectedId);
            }
            else
            {
                cedantBos = CedantService.GetByStatus(status, selectedId: selectedId);
            }

            foreach (var cedant in cedantBos)
            {
                var selected = cedant.Id == selectedId;
                items.Add(new SelectListItem { Text = cedant.ToString(), Value = cedant.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownCedants = items;
            return items;
        }

        protected List<SelectListItem> DropDownNewCedant(int? status = null, bool checkWorkgroup = false)
        {
            var items = GetEmptyDropDownList();

            IList<CedantBo> cedantBos;
            if (checkWorkgroup)
            {
                cedantBos = CedantService.GetByStatusWorkgroup(AuthUserId, status);
            }
            else
            {
                cedantBos = CedantService.GetByStatus(status);
            }

            foreach (var cedant in cedantBos)
            {
                items.Add(new SelectListItem { Text = cedant.ToString(), Value = cedant.Id.ToString() });
            }
            ViewBag.DropDownNewCedants = items;
            return items;
        }

        protected List<SelectListItem> DropDownCedantCode(int? status = null, string cedantCode = null, bool checkWorkgroup = false)
        {
            var items = GetEmptyDropDownList();

            IList<CedantBo> cedantBos;
            if (checkWorkgroup)
            {
                cedantBos = CedantService.GetByStatusWorkgroup(AuthUserId, status, selectedCode: cedantCode);
            }
            else
            {
                cedantBos = CedantService.GetByStatus(status, selectedCode: cedantCode);
            }

            foreach (var cedant in cedantBos)
            {
                var selected = cedant.Code == cedantCode;
                items.Add(new SelectListItem { Text = cedant.ToString(), Value = cedant.Code, Selected = selected });
            }
            ViewBag.DropDownCedants = items;
            return items;
        }

        protected List<SelectListItem> DropDownTreatyPricingCedant()
        {
            var items = GetEmptyDropDownList();
            foreach (var cedant in TreatyPricingCedantService.Get())
            {
                items.Add(new SelectListItem { Text = cedant.ToString(), Value = cedant.Id.ToString() });
            }

            ViewBag.DropDownTreatyPricingCedants = items;
            return items;
        }

        protected List<SelectListItem> DropDownUser(int? status = null, int? selectedId = null, bool exceptSuper = false, int? departmentId = null)
        {
            var items = GetEmptyDropDownList();

            List<int> selectedIds = null;
            if (selectedId.HasValue)
            {
                selectedIds = new List<int>() { selectedId ?? 0 };
            }

            foreach (var user in UserService.GetByStatus(status, selectedIds, exceptSuper, departmentId))
            {
                var selected = user.Id == selectedId;
                items.Add(new SelectListItem { Text = user.FullName, Value = user.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownUsers = items;
            return items;
        }

        protected List<SelectListItem> DropDownUserByDept(int? status = null, int? selectedId = null, List<int> departmentIds = null)
        {
            var items = GetEmptyDropDownList();
            foreach (var user in UserService.GetByDepartments(status, selectedId, departmentIds))
            {
                var selected = user.Id == selectedId;
                items.Add(new SelectListItem { Text = user.FullName, Value = user.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownUsers = items;
            return items;
        }

        protected List<SelectListItem> DropDownUserByAccessPower(int moduleId, string power = null, int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            foreach (var user in UserService.GetUserBosByModulePower(moduleId, power))
            {
                var selected = user.Id == selectedId;
                items.Add(new SelectListItem { Text = user.FullName, Value = user.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownUsers = items;
            return items;
        }

        public List<SelectListItem> DropDownPicClaim(int? status = null, int? selectedId = null, bool hasUnassignedOption = false)
        {
            List<SelectListItem> items = DropDownUser(status, selectedId, true, DepartmentBo.DepartmentClaim);
            if (hasUnassignedOption)
            {
                items.Add(new SelectListItem { Text = "Unassigned", Value = "0" });
            }
            ViewBag.DropDownPicClaims = items;

            return items;
        }

        public List<SelectListItem> DropDownPicDaa(int? status = null, int? selectedId = null, bool hasUnassignedOption = false)
        {
            List<SelectListItem> items = DropDownUser(status, selectedId, true, DepartmentBo.DepartmentDataAnalyticsAdministration);
            if (hasUnassignedOption)
            {
                items.Add(new SelectListItem { Text = "Unassigned", Value = "0" });
            }
            ViewBag.DropDownPicDaas = items;

            return items;
        }

        public void DropDownClaimAssessor(int? status = null, int? selectedId = null)
        {
            ViewBag.DropDownClaimAssessors = DropDownUser(status, selectedId, true, DepartmentBo.DepartmentClaim);
        }

        protected List<SelectListItem> DropDownBenefit(int? status = null, int? selectedId = null, string selectedCode = null, bool codeAsValue = false)
        {
            var items = GetEmptyDropDownList();

            foreach (var benefit in BenefitService.GetByStatusForDropDownBenefit(status, selectedId))
            {
                var selected = codeAsValue ? (benefit.Code == selectedCode) : (benefit.Id == selectedId);
                string value = codeAsValue ? benefit.Code : benefit.Id.ToString();
                items.Add(new SelectListItem { Text = benefit.ToString(), Value = value, Selected = selected });
            }
            ViewBag.DropDownBenefits = items;
            return items;
        }

        protected List<SelectListItem> DropDownClaimCode(int? status = null, int? selectedId = null, string selectedCode = null, bool codeAsValue = false)
        {
            var items = GetEmptyDropDownList();
            foreach (var claimCode in ClaimCodeService.GetByStatus(status, selectedId))
            {
                var selected = codeAsValue ? (claimCode.Code == selectedCode) : (claimCode.Id == selectedId);
                string value = codeAsValue ? claimCode.Code : claimCode.Id.ToString();
                items.Add(new SelectListItem { Text = claimCode.Code, Value = value, Selected = selected });
            }
            ViewBag.DropDownClaimCodes = items;
            return items;
        }

        protected List<SelectListItem> DropDownClaimCategory(int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            foreach (var claimCategory in ClaimCategoryService.Get())
            {
                var selected = claimCategory.Id == selectedId;
                items.Add(new SelectListItem { Text = claimCategory.Category, Value = claimCategory.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownClaimCategories = items;
            return items;
        }

        protected List<SelectListItem> DropDownTreatyCode(int? status = null, int? selectedId = null, int? cedantId = null, bool withDescription = true, string selectedCode = null, bool codeAsValue = false, bool isUniqueCode = false, bool foreign = true)
        {
            IList<TreatyCodeBo> treatyCodeBos;

            if (cedantId != null)
            {
                treatyCodeBos = TreatyCodeService.GetByCedantId(cedantId.Value, status, selectedId, isUniqueCode, foreign);
            }
            else
            {
                treatyCodeBos = TreatyCodeService.GetByStatus(status, selectedId, isUniqueCode, foreign);
            }

            var items = GetEmptyDropDownList();
            foreach (var treatyCode in treatyCodeBos)
            {
                var selected = codeAsValue ? (treatyCode.Code == selectedCode) : (treatyCode.Id == selectedId);
                string value = codeAsValue ? treatyCode.Code : treatyCode.Id.ToString();
                items.Add(new SelectListItem { Text = treatyCode.ToString(withDescription), Value = value, Selected = selected });
            }
            ViewBag.DropDownTreatyCodes = items;
            return items;
        }

        protected List<SelectListItem> DropDownTreaty(int? selectedId = null, int? cedantId = null, bool withDescription = true)
        {
            IList<TreatyBo> treatyBos;
            if (cedantId != null)
            {
                treatyBos = TreatyService.GetByCedantId(cedantId.Value);
            }
            else
            {
                treatyBos = TreatyService.Get();
            }

            var items = GetEmptyDropDownList();

            foreach (var treaty in treatyBos)
            {
                var selected = treaty.Id == selectedId;
                items.Add(new SelectListItem { Text = treaty.ToString(withDescription), Value = treaty.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownTreaties = items;
            return items;
        }

        protected List<SelectListItem> DropDownTreatyPricingObjectModules(int? selectedId = null)
        {
            var items = GetEmptyDropDownList();

            for (int i = 1; i <= TreatyPricingWorkflowObjectBo.ObjectTypeMax; i++)
            {
                string text = TreatyPricingWorkflowObjectBo.GetObjectTypeName(i);
                var selected = i == selectedId;

                items.Add(new SelectListItem { Text = text, Value = i.ToString(), Selected = selected });
            }

            ViewBag.DropDownTreatyPricingObjectModules = items;
            return items;
        }

        protected List<SelectListItem> DropDownMonth(bool isName = true)
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= 12; i++)
            {
                string text = isName ? CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i) : i.ToString();
                items.Add(new SelectListItem { Text = text, Value = i.ToString() });
            }
            ViewBag.DropDownMonths = items;
            return items;
        }

        protected List<SelectListItem> DropDownYear(bool isSelect = false, int count = 50)
        {
            var items = GetEmptyDropDownList(isSelect);
            int currentYear = DateTime.Now.Year;
            items.Add(new SelectListItem { Text = currentYear.ToString(), Value = currentYear.ToString() });
            for (int i = 1; i <= count; i++)
            {
                int year = currentYear - i;
                items.Add(new SelectListItem { Text = year.ToString(), Value = year.ToString() });
            }
            ViewBag.DropDownYears = items;
            return items;
        }

        protected List<SelectListItem> DropDownQuarter(bool isSelect = false, int count = 50)
        {
            var items = GetEmptyDropDownList(isSelect);
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;

            if (currentMonth > 9)
                items.Add(new SelectListItem { Text = currentYear.ToString().Substring(2) + "Q4", Value = currentYear.ToString() + "Q4" });

            if (currentMonth > 6)
                items.Add(new SelectListItem { Text = currentYear.ToString().Substring(2) + "Q3", Value = currentYear.ToString() + "Q3" });

            if (currentMonth > 3)
                items.Add(new SelectListItem { Text = currentYear.ToString().Substring(2) + "Q2", Value = currentYear.ToString() + "Q2" });

            items.Add(new SelectListItem { Text = currentYear.ToString().Substring(2) + "Q1", Value = currentYear.ToString() + "Q1" });

            for (int i = 1; i <= count; i++)
            {
                int year = currentYear - i;

                for (int j = 1; j <= 4; j++)
                {
                    string quarter = year.ToString() + "Q" + (5 - j).ToString();

                    items.Add(new SelectListItem { Text = quarter.Substring(2), Value = quarter });
                }
            }
            ViewBag.DropDownQuarters = items;
            return items;
        }

        protected List<SelectListItem> DropDownEventCode(int? status = null, int? selectedId = null, string selectedCode = null, bool codeAsValue = false)
        {
            var items = GetEmptyDropDownList();
            foreach (var eventCode in EventCodeService.GetByStatus(status, selectedId, selectedCode))
            {
                var selected = eventCode.Id == selectedId;
                string value = codeAsValue ? eventCode.Code : eventCode.Id.ToString();
                items.Add(new SelectListItem { Text = eventCode.Code, Value = value, Selected = selected });
            }
            ViewBag.DropDownEventCodes = items;
            return items;
        }

        protected List<SelectListItem> DropDownClaimReason(int? type = null)
        {
            var items = GetEmptyDropDownList();
            IList<ClaimReasonBo> bos = new List<ClaimReasonBo>();
            if (type.HasValue)
                bos = ClaimReasonService.GetByType(type.Value);
            else
                bos = ClaimReasonService.Get();

            foreach (var claimReason in bos)
            {
                items.Add(new SelectListItem { Text = claimReason.Reason, Value = claimReason.Id.ToString() });
            }

            ViewBag.DropDownClaimReasons = items;
            return items;
        }

        protected List<SelectListItem> DropDownCutOff(int? cutOffId = null)
        {
            var items = GetEmptyDropDownList();

            foreach (var cutOffQuarter in CutOffService.Get())
            {
                bool selected = cutOffId.HasValue && cutOffId.Value == cutOffQuarter.Id;
                var dateStr = cutOffQuarter.CutOffDateTime?.ToString(Util.GetDateFormat());
                var text = string.Format("{0} - {1}", cutOffQuarter.Quarter, dateStr);
                items.Add(new SelectListItem { Text = text, Value = cutOffQuarter.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownCutOffs = items;
            return items;
        }

        protected List<SelectListItem> DropDownCutOffQuarter(int? cutOffId = null)
        {
            var items = GetEmptyDropDownList();

            foreach (var cutOffQuarter in CutOffService.GetCutOffQuarter())
            {
                bool selected = cutOffId.HasValue && cutOffId.Value == cutOffQuarter.Id;
                items.Add(new SelectListItem { Text = cutOffQuarter.Quarter, Value = cutOffQuarter.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownCutOffQuarters = items;
            return items;
        }

        protected List<SelectListItem> DropDownCutOffQuarterWithDate(int? cutOffId = null)
        {
            var items = GetEmptyDropDownList();

            foreach (var cutOffQuarter in CutOffService.GetCutOffQuarter())
            {
                bool selected = cutOffId.HasValue && cutOffId.Value == cutOffQuarter.Id;
                if (cutOffQuarter.CutOffDateTime.HasValue)
                    items.Add(new SelectListItem { Text = cutOffQuarter.Quarter + " - " + cutOffQuarter.CutOffDateTime.Value.ToString("dd MMM yyyy"), Value = cutOffQuarter.Id.ToString(), Selected = selected });
                else
                    items.Add(new SelectListItem { Text = cutOffQuarter.Quarter, Value = cutOffQuarter.Id.ToString(), Selected = selected });

            }
            ViewBag.DropDownCutOffQuartersWithDate = items;
            return items;
        }

        protected List<SelectListItem> DropDownTreatyPricingReportGenerationReportType()
        {
            var items = GetEmptyDropDownList();

            foreach (var reportName in TreatyPricingReportGenerationService.GetDistinctReportName())
            {
                items.Add(new SelectListItem { Text = reportName, Value = reportName, Selected = false });
            }

            ViewBag.DropDownTreatyPricingReportGenerationReportType = items;
            return items;
        }

        protected List<SelectListItem> GetPickListDetailIdDropDown(int? standardOutputType = null, bool displaySelect = true, bool codeAsValue = false, int? standardClaimDataOutputType = null)
        {
            var items = GetEmptyDropDownList(displaySelect);
            IList<PickListDetailBo> pickListDetails = new List<PickListDetailBo>();
            if (standardOutputType.HasValue)
            {
                pickListDetails = PickListDetailService.GetByStandardOutputId(standardOutputType.Value, false);
            }
            else if (standardClaimDataOutputType.HasValue)
            {
                pickListDetails = PickListDetailService.GetByStandardClaimDataOutputId(standardClaimDataOutputType.Value);
            }

            foreach (PickListDetailBo pickListDetail in pickListDetails)
            {
                string value = codeAsValue ? pickListDetail.Code : pickListDetail.Id.ToString();
                items.Add(new SelectListItem { Text = pickListDetail.ToString(), Value = value });
            }
            return items;
        }

        protected List<SelectListItem> GetPickListDetailIdDropDownByPickListId(int type, bool displaySelect = true, bool codeAsValue = false)
        {
            var items = GetEmptyDropDownList(displaySelect);
            foreach (PickListDetailBo pickListDetail in PickListDetailService.GetByPickListId(type))
            {
                string value = codeAsValue ? pickListDetail.Code : pickListDetail.Id.ToString();
                items.Add(new SelectListItem { Text = pickListDetail.ToString(), Value = value });
            }
            return items;
        }

        protected Dictionary<string, string> GetPickListDetailCode(int type, bool codeOnly = false)
        {
            var items = new Dictionary<string, string>();
            foreach (PickListDetailBo pickListDetail in PickListDetailService.GetByStandardOutputId(type))
            {
                string text = codeOnly ? pickListDetail.Code : pickListDetail.ToString();
                items[pickListDetail.Id.ToString()] = text;
            }
            return items;
        }

        protected List<string> GetPickListDetailCodes(int type)
        {
            return PickListDetailService.GetCodeByType(type).ToList();
        }

        protected List<string> GetPickListDetailCodeDescription(int type)
        {
            IList<PickListDetailBo> bos = PickListDetailService.GetByPickListId(type);
            return bos.Select(bo => bo.ToString()).ToList();
        }

        protected List<string> GetTreatyPricingPerLifeRetroCodes()
        {
            return TreatyPricingPerLifeRetroService.GetCodes().ToList();
        }

        protected List<string> GetTreatyPricingCedant()
        {
            List<string> items = new List<string>();
            foreach (var cedant in TreatyPricingCedantService.Get())
            {
                items.Add(cedant.Code);
            }

            ViewBag.TreatyPricingCedants = items;
            return items;
        }

        protected List<string> GetUsers(int? status = null, int? departmentId = null)
        {
            List<string> users = new List<string>();
            foreach (var userBo in UserService.GetByStatus(status: status, departmentId: departmentId))
            {
                users.Add(userBo.UserName);
            }
            return users;
        }

        protected List<SelectListItem> DropDownReinsBasisCode(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDown(StandardOutputBo.TypeReinsBasisCode, codeAsValue: codeAsValue);
            ViewBag.DropDownReinsBasisCodes = items;
            return items;
        }

        protected List<SelectListItem> DropDownInsuredGenderCode(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDown(StandardOutputBo.TypeInsuredGenderCode, codeAsValue: codeAsValue);
            ViewBag.DropDownInsuredGenderCodes = items;
            return items;
        }

        protected List<SelectListItem> DropDownInsuredTobaccoUse(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDown(StandardOutputBo.TypeInsuredTobaccoUse, codeAsValue: codeAsValue);
            ViewBag.DropDownInsuredTobaccoUses = items;
            return items;
        }

        protected List<SelectListItem> DropDownPremiumFrequencyCode(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDown(StandardOutputBo.TypePremiumFrequencyCode, codeAsValue: codeAsValue);
            ViewBag.DropDownPremiumFrequencyCodes = items;
            return items;
        }

        protected List<SelectListItem> DropDownPolicyStatusCode(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDown(StandardOutputBo.TypePolicyStatusCode, codeAsValue: codeAsValue);
            ViewBag.DropDownPolicyStatusCodes = items;
            return items;
        }

        protected List<SelectListItem> DropDownInsuredOccupationCode(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDown(StandardOutputBo.TypeInsuredOccupationCode, codeAsValue: codeAsValue);
            ViewBag.DropDownInsuredOccupationCodes = items;
            return items;
        }

        protected List<SelectListItem> DropDownMfrs17BasicRider(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDown(StandardOutputBo.TypeMfrs17BasicRider, codeAsValue: codeAsValue);
            ViewBag.DropDownMfrs17BasicRiders = items;
            return items;
        }

        protected List<SelectListItem> DropDownCurrencyCode(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDown(StandardOutputBo.TypeCurrencyCode, codeAsValue: codeAsValue);
            ViewBag.DropDownCurrencyCodes = items;
            return items;
        }

        protected List<SelectListItem> DropDownTreatyType(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDown(StandardOutputBo.TypeTreatyType, codeAsValue: codeAsValue);
            ViewBag.DropDownTreatyTypes = items;
            return items;
        }

        protected List<SelectListItem> DropDownCedingBenefitTypeCode(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDown(StandardOutputBo.TypeCedingBenefitTypeCode, codeAsValue: codeAsValue);
            ViewBag.DropDownCedingBenefitTypeCodes = items;
            return items;
        }

        protected List<SelectListItem> DropDownCountryCode()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.CountryCode);
            ViewBag.DropDownCountryCodes = items;
            return items;
        }

        protected List<SelectListItem> DropDownFundsAccountingTypeCode(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDown(StandardOutputBo.TypeFundsAccountingTypeCode, codeAsValue: codeAsValue);
            ViewBag.DropDownFundsAccountingTypeCodes = items;
            return items;
        }

        protected List<SelectListItem> DropDownTransactionTypeCode(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDown(StandardOutputBo.TypeTransactionTypeCode, codeAsValue: codeAsValue);
            ViewBag.DropDownTransactionTypeCodes = items;
            return items;
        }

        protected List<SelectListItem> DropDownRecordType(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDown(standardClaimDataOutputType: StandardClaimDataOutputBo.TypeRecordType, codeAsValue: codeAsValue);
            ViewBag.DropDownRecordTypes = items;
            return items;
        }

        protected List<SelectListItem> DropDownRiDataRecordType(bool codeAsValue = false)
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= RiDataBo.RecordTypeMax; i++)
            {
                string text = RiDataBo.GetRecordTypeName(i);
                string value = codeAsValue ? text : i.ToString();
                items.Add(new SelectListItem { Text = text, Value = value });
            }
            ViewBag.DropDownRiDataRecordTypes = items;
            return items;
        }

        protected List<SelectListItem> DropDownProfitComm(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDown(StandardOutputBo.TypeProfitComm, codeAsValue: codeAsValue);
            ViewBag.DropDownProfitComms = items;
            return items;
        }

        protected List<SelectListItem> DropDownBusinessOrigin(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.BusinessOrigin, codeAsValue: codeAsValue);
            ViewBag.DropDownBusinessOrigins = items;
            return items;
        }

        protected List<SelectListItem> DropDownReinsuranceType()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.ReinsuranceType);
            ViewBag.DropDownReinsuranceTypes = items;
            return items;
        }

        protected List<SelectListItem> DropDownAgeBasis()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.AgeBasis);
            ViewBag.DropDownAgeBasis = items;
            return items;
        }

        protected List<SelectListItem> DropDownRateGuarantee()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.RateGuarantee);
            ViewBag.DropDownRateGuarantees = items;
            return items;
        }

        protected List<SelectListItem> DropDownRetroPremiumFrequencyMode(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.RetroPremiumFrequencyMode, codeAsValue: codeAsValue);
            ViewBag.DropDownRetroPremiumFrequencyModes = items;
            return items;
        }

        protected List<SelectListItem> DropDownAccessGroup(bool linkDepartment = false, int? departmentId = null)
        {
            var items = GetEmptyDropDownList();

            List<AccessGroupBo> accessGroupBos = new List<AccessGroupBo>();
            if (linkDepartment)
            {
                accessGroupBos = AccessGroupService.GetByDepartmentId(departmentId).ToList();
            }
            else
            {
                accessGroupBos = AccessGroupService.Get().ToList();
            }

            foreach (AccessGroupBo accessGroupBo in accessGroupBos)
            {
                items.Add(new SelectListItem { Text = accessGroupBo.Name, Value = accessGroupBo.Id.ToString() });
            }
            ViewBag.DropDownAccessGroups = items;
            return items;
        }

        protected List<string> DropDownDocumentType(bool isOther = false)
        {
            var items = new List<string>();
            if (isOther)
            {
                for (int i = DocumentBo.TypeOthers; i <= DocumentBo.MaxType; i++)
                {
                    items.Add(DocumentBo.GetTypeName(i));
                }
            }
            else
            {
                items.Add("Please select");
                for (int i = 1; i <= DocumentBo.MaxType; i++)
                {
                    items.Add(DocumentBo.GetTypeName(i));
                }
            }
            ViewBag.DropDownDocumentTypes = items;
            return items;
        }

        protected List<DocumentBo> GetDocuments(int moduleId, int modelId, string downloadUrl = "", bool checkPrivacy = false, int? departmentId = null)
        {
            List<DocumentBo> items = DocumentService.GetByModuleIdObjectId(moduleId, modelId, checkPrivacy, departmentId).ToList();
            foreach (var d in items)
            {
                d.IsFileExists();
                d.GetDownloadLink(downloadUrl);
            }
            ViewBag.DocumentBos = items;
            return items;
        }

        protected List<SelectListItem> DropDownRequestType()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= UserBo.MaxRequestType; i++)
            {
                items.Add(new SelectListItem { Text = UserBo.GetRequestTypeName(i), Value = i.ToString() });
            }
            ViewBag.DropDownRequestTypes = items;
            return items;
        }

        protected List<SelectListItem> DropDownYesNo(bool displaySelect = false)
        {
            var items = GetEmptyDropDownList(displaySelect);
            items.Add(new SelectListItem { Text = "No", Value = "false" });
            items.Add(new SelectListItem { Text = "Yes", Value = "true" });
            ViewBag.DropDownYesNo = items;
            return items;
        }

        protected List<SelectListItem> DropDownYesNoWithSelect(bool codeAsValue = false)
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = "No", Value = codeAsValue ? "No" : "false" });
            items.Add(new SelectListItem { Text = "Yes", Value = codeAsValue ? "Yes" : "true" });
            ViewBag.DropDownYesNoWithSelect = items;
            return items;
        }

        protected List<SelectListItem> DropDownYN()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = "N", Value = "N" });
            items.Add(new SelectListItem { Text = "Y", Value = "Y" });
            ViewBag.DropDownYN = items;
            return items;
        }

        public void GetCedingBenefitTypeCode()
        {
            List<string> cedingBenefitTypeCodes = new List<string>();
            foreach (PickListDetailBo pickListDetailBo in PickListDetailService.GetByPickListId(PickListBo.CedingBenefitTypeCode))
            {
                cedingBenefitTypeCodes.Add(pickListDetailBo.Code);
            }
            ViewBag.CedingBenefitTypeCodes = cedingBenefitTypeCodes;
        }

        public void GetMlreEventCode()
        {
            List<string> mlreEventCodes = new List<string>();
            foreach (EventCodeBo eventCodeBo in EventCodeService.GetByStatus(EventCodeBo.StatusActive))
            {
                mlreEventCodes.Add(eventCodeBo.Code);
            }
            ViewBag.MlreEventCodes = mlreEventCodes;
        }

        public void GetTreatyCodes(bool isUniqueCode = false, bool foreign = true)
        {
            List<string> treatyCodes = new List<string>();
            foreach (var treatyCodeBo in TreatyCodeService.GetByStatus(TreatyCodeBo.StatusActive, isUniqueCode: isUniqueCode, foreign: foreign))
            {
                treatyCodes.Add(treatyCodeBo.Code);
            }
            ViewBag.TreatyCodes = treatyCodes;
        }

        protected List<BenefitBo> GetBenefits(bool foreign = true)
        {
            List<BenefitBo> benefitBos = BenefitService.Get(foreign).ToList();

            ViewBag.BenefitBos = benefitBos;
            return benefitBos;
        }

        protected void GetBenefitCodes()
        {
            List<string> benefitCodes = new List<string>();
            foreach (var benefitCodeBo in BenefitService.GetByStatus(BenefitBo.StatusActive))
            {
                benefitCodes.Add(benefitCodeBo.Code);
            }
            ViewBag.BenefitCodes = benefitCodes;
        }

        protected void GetDistributionChannels()
        {
            List<string> distributionChannels = new List<string>();
            foreach (var pickListDetailBo in PickListDetailService.GetByPickListId(PickListBo.DistributionChannel))
            {
                distributionChannels.Add(pickListDetailBo.Description);
            }
            ViewBag.DistributionChannels = distributionChannels;
        }

        protected List<StatusHistoryBo> GetStatusHistory(int moduleId, int modelId)
        {
            List<StatusHistoryBo> items = StatusHistoryService.GetStatusHistoriesByModuleIdObjectId(moduleId, modelId).ToList();
            ViewBag.StatusHistoryBos = items;
            return items;
        }

        protected List<RemarkBo> GetRemark(int moduleId, int modelId)
        {
            List<RemarkBo> items = RemarkService.GetByModuleIdObjectId(moduleId, modelId).ToList();
            ViewBag.RemarkBos = items;
            return items;
        }

        protected List<RemarkBo> GetRemarkDocument(int moduleId, int objectId, string downloadUrl, string subModuleController = null, int? subObjectId = null)
        {
            List<DocumentBo> documents = DocumentService.GetBySubModule(moduleId, objectId, subModuleController, subObjectId).ToList();
            List<RemarkBo> remarks = RemarkService.GetBySubModule(moduleId, objectId, subModuleController, subObjectId).ToList();

            foreach (var remark in remarks)
            {
                remark.DocumentBos = new List<DocumentBo>();
                foreach (var document in documents.Where(q => q.RemarkId == remark.Id).ToList())
                {
                    document.IsFileExists();
                    document.GetDownloadLink(downloadUrl);
                    remark.DocumentBos.Add(document);
                }
            }

            ViewBag.RemarkBos = remarks;
            return remarks;
        }

        protected List<StatusHistoryBo> GetStatusHistories(int moduleId, int objectId, string downloadUrl, string subModuleController = null, int? subObjectId = null)
        {
            List<StatusHistoryBo> statusHistories = StatusHistoryService.GetBySubModule(moduleId, objectId, subModuleController, subObjectId).ToList();
            List<DocumentBo> documents = DocumentService.GetBySubModule(moduleId, objectId, subModuleController, subObjectId).ToList();

            foreach (var statusHistory in statusHistories)
            {
                if (statusHistory.SubModuleController == ModuleBo.ModuleController.TreatyPricingQuotationWorkflowPricing.ToString())
                    statusHistory.StatusName = TreatyPricingQuotationWorkflowBo.GetPricingStatusName(statusHistory.Status);

                if (statusHistory.SubModuleController == ModuleBo.ModuleController.TreatyPricingQuotationWorkflowVersionQuotationChecklist.ToString())
                {
                    statusHistory.StatusName = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetStatusName(statusHistory.Status);

                    var checklistSearchId = statusHistory.SubObjectId ?? default(int);
                    var quotationWorkflowChecklistBo = TreatyPricingQuotationWorkflowVersionQuotationChecklistService.Find(checklistSearchId);

                    if (quotationWorkflowChecklistBo != null)
                    {
                        statusHistory.Department = quotationWorkflowChecklistBo.InternalTeam;
                        statusHistory.PersonInCharge = quotationWorkflowChecklistBo.InternalTeamPersonInCharge;
                    }
                }

                if (statusHistory.SubModuleController == ModuleBo.ModuleController.TreatyPricingGroupReferralChecklist.ToString())
                {
                    statusHistory.StatusName = TreatyPricingGroupReferralChecklistBo.GetStatusName(statusHistory.Status);

                    var checklistSearchId = statusHistory.SubObjectId ?? default(int);
                    var groupReferralChecklistBo = TreatyPricingGroupReferralChecklistService.Find(checklistSearchId);

                    if (groupReferralChecklistBo != null)
                    {
                        statusHistory.Department = TreatyPricingGroupReferralChecklistBo.GetDefaultInternalTeamName(groupReferralChecklistBo.InternalTeam);
                        statusHistory.PersonInCharge = UserService.Find(statusHistory.CreatedById)?.UserName;
                    }
                }

                if (statusHistory.ModuleBo.Controller == ModuleBo.ModuleController.TreatyPricingTreatyWorkflow.ToString() ||
                    statusHistory.ModuleBo.Controller == ModuleBo.ModuleController.TreatyPricingQuotationWorkflow.ToString() ||
                    statusHistory.ModuleBo.Controller == ModuleBo.ModuleController.TreatyPricingQuotationWorkflowPricing.ToString() ||
                    statusHistory.ModuleBo.Controller == ModuleBo.ModuleController.TreatyPricingGroupReferral.ToString())
                {
                    statusHistory.RecipientNames = string.Join(", ", Services.EmailService.GetUserNameByModuleObject(ModuleBo.ModuleController.StatusHistory.ToString(), statusHistory.Id));
                }

                if (statusHistory.RemarkBo != null)
                {
                    statusHistory.RemarkBo.DocumentBos = new List<DocumentBo>();
                    foreach (var document in documents.Where(q => q.RemarkId == statusHistory.RemarkBo.Id).ToList())
                    {
                        document.IsFileExists();
                        document.GetDownloadLink(downloadUrl);
                        statusHistory.RemarkBo.DocumentBos.Add(document);
                    }
                }

            }

            ViewBag.StatusHistoryBos = statusHistories;
            return statusHistories;
        }

        protected void GetRetroPartyParties()
        {
            List<string> retroPartyParties = new List<string>();
            foreach (var retroPartyBo in RetroPartyService.GetByStatus(RetroPartyBo.StatusActive))
            {
                retroPartyParties.Add(retroPartyBo.Party);
            }
            ViewBag.RetroPartyParties = retroPartyParties;
        }

        protected List<SelectListItem> DropDownRetroParty(int? status = null, int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            foreach (var retroParty in RetroPartyService.GetByStatus(status, selectedId))
            {
                var selected = retroParty.Id == selectedId;
                items.Add(new SelectListItem { Text = retroParty.ToString(), Value = retroParty.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownRetroParties = items;
            return items;
        }

        protected List<SelectListItem> DropDownRetroPartyByRetroConfig(int treatyCodeId, int? selectedId = null)
        {
            var items = GetEmptyDropDownList();

            DirectRetroConfigurationBo directRetroConfigurationBo = DirectRetroConfigurationService.FindByTreatyCodeId(treatyCodeId);

            if (directRetroConfigurationBo != null)
            {
                string[] retroParties = Util.ToArraySplitTrim(directRetroConfigurationBo.RetroParty);

                foreach (var retroParty in retroParties)
                {
                    RetroPartyBo retroPartyBo = RetroPartyService.FindByParty(retroParty);
                    if (retroPartyBo != null)
                        items.Add(new SelectListItem { Text = retroPartyBo.ToString(), Value = retroPartyBo.Id.ToString() });
                }
            }

            if (selectedId.HasValue && selectedId != 0 && items.Where(q => q.Value == selectedId.ToString()).Count() == 0)
            {
                RetroPartyBo retroPartyBo = RetroPartyService.Find(selectedId);
                if (retroPartyBo != null)
                    items.Add(new SelectListItem { Text = retroPartyBo.ToString(), Value = retroPartyBo.Id.ToString(), Selected = true });
            }

            ViewBag.DropDownRetroPartiesByRetroConfig = items;
            return items;
        }

        protected List<SelectListItem> DropDownPremiumSpreadTable(int type, bool codeAsValue = false)
        {
            var items = GetEmptyDropDownList();
            foreach (var premiumSpreadTable in PremiumSpreadTableService.GetByType(type))
            {
                string text = premiumSpreadTable.Rule;
                string value = codeAsValue ? text : premiumSpreadTable.Id.ToString();
                items.Add(new SelectListItem { Text = text, Value = value });
            }
            ViewBag.DropDownPremiumSpreadTables = items;
            return items;
        }

        protected List<SelectListItem> DropDownTreatyDiscountTable(int type, bool codeAsValue = false)
        {
            var items = GetEmptyDropDownList();
            foreach (var treatyDiscountTable in TreatyDiscountTableService.GetByType(type))
            {
                string text = treatyDiscountTable.Rule;
                string value = codeAsValue ? text : treatyDiscountTable.Id.ToString();
                items.Add(new SelectListItem { Text = text, Value = value });
            }
            ViewBag.DropDownTreatyDiscountTables = items;
            return items;
        }

        // For Claim Register 
        public List<SelectListItem> DropDownClaimTransactionType(bool codeAsValue = false, bool removeNew = false, bool removeBulk = false, bool displaySelect = true)
        {
            var items = GetPickListDetailIdDropDown(standardClaimDataOutputType: StandardClaimDataOutputBo.TypeClaimTransactionType, displaySelect: displaySelect, codeAsValue: codeAsValue);
            if (removeNew)
                items = items.Where(q => q.Value != PickListDetailBo.ClaimTransactionTypeNew).ToList();
            if (removeBulk)
                items = items.Where(q => q.Value != PickListDetailBo.ClaimTransactionTypeBulk).ToList();
            ViewBag.DropDownClaimTransactionTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownClaimStatus(bool claimOnly = false)
        {
            var items = GetEmptyDropDownList();
            if (claimOnly)
            {
                foreach (int i in ClaimRegisterBo.GetClaimDepartmentStatus())
                {
                    items.Add(new SelectListItem { Text = ClaimRegisterBo.GetStatusName(i), Value = i.ToString() });
                }
            }
            else
            {
                for (int i = 1; i <= ClaimRegisterBo.StatusMax; i++)
                {
                    items.Add(new SelectListItem { Text = ClaimRegisterBo.GetStatusName(i), Value = i.ToString() });
                }
            }
            ViewBag.DropDownClaimStatuses = items;
            return items;
        }

        public List<SelectListItem> DropDownProvisionStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimRegisterBo.ProvisionStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = ClaimRegisterBo.GetProvisionStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownProvisionStatuses = items;
            return items;
        }

        public List<SelectListItem> DropDownOffsetStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= ClaimRegisterBo.OffsetStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = ClaimRegisterBo.GetOffsetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownOffsetStatuses = items;
            return items;
        }

        public void DropDownChecklistStatus()
        {
            var items = GetEmptyDropDownList(false);
            for (int i = 1; i <= ClaimRegisterBo.ChecklistStatusMax; i++)
            {
                items.Add(new SelectListItem { Text = ClaimRegisterBo.GetChecklistStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownChecklistStatuses = items;
        }

        public List<SelectListItem> DropDownSource()
        {
            var items = GetEmptyDropDownList();
            foreach (var source in SourceService.Get())
            {
                items.Add(new SelectListItem { Text = source.Name, Value = source.Id.ToString() });
            }
            ViewBag.DropDownSources = items;
            return items;
        }

        public List<SelectListItem> DropDownAccountCode(int? type, int? reportType = null)
        {
            var items = GetEmptyDropDownList();
            var bos = new List<AccountCodeBo>();
            if (type.HasValue && !reportType.HasValue)
                bos = AccountCodeService.GetByType(type.Value).ToList();
            else if (reportType.HasValue && !type.HasValue)
                bos = AccountCodeService.GetByReportType(reportType.Value).ToList();
            else if (reportType.HasValue && type.HasValue)
                bos = AccountCodeService.Get().Where(a => a.Type == type && a.ReportingType == reportType).ToList();
            else
                bos = AccountCodeService.Get().ToList();

            foreach (var accountCode in bos)
            {
                items.Add(new SelectListItem { Text = accountCode.ToString(), Value = accountCode.Id.ToString() });
            }
            ViewBag.DropDownAccountCodes = items;
            return items;
        }

        protected List<SelectListItem> DropDownTreatyPricingCedant(int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            foreach (var cedant in TreatyPricingCedantService.Get())
            {
                var selected = cedant.Id == selectedId;
                items.Add(new SelectListItem { Text = cedant.ToString(), Value = cedant.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownTreatyPricingCedants = items;
            return items;
        }

        protected List<SelectListItem> DropDownVersion(ObjectVersion model)
        {
            var items = GetEmptyDropDownList(false);
            foreach (var version in model.GetVersions())
            {
                var selected = version == model.CurrentVersion;
                items.Add(new SelectListItem { Text = string.Format("{0}.0", version), Value = version.ToString(), Selected = selected });
            }
            ViewBag.DropDownVersions = items;
            return items;
        }

        public List<SelectListItem> DropDownProductType()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.ProductType);
            ViewBag.DropDownProductTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownBusinessType()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.BusinessType);
            ViewBag.DropDownBusinessTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownRiArrangement()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.RiArrangement);
            ViewBag.DropDownRiArrangements = items;
            return items;
        }

        public List<SelectListItem> DropDownUnearnedPremiumRefund()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.UnearnedPremiumRefund);
            ViewBag.DropDownUnearnedPremiumRefunds = items;
            return items;
        }

        public List<SelectListItem> DropDownAgeDefinition()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.AgeDefinition);
            ViewBag.DropDownAgeDefinitions = items;
            return items;
        }

        public List<SelectListItem> DropDownDistributionTier()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.DistributionTier);
            ViewBag.DropDownDistributionTier = items;
            return items;
        }

        public List<SelectListItem> DropDownPricingTeam()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.PricingTeam);
            ViewBag.DropDownPricingTeam = items;
            return items;
        }

        public List<SelectListItem> DropDownPayoutType()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.PayoutType);
            ViewBag.DropDownPayoutTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownArrangementReinsuranceType()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.ArrangementReinsuranceType);
            ViewBag.DropDownArrangementReinsuranceTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownRiskPatternSum()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.RiskPatternSum);
            ViewBag.DropDownRiskPatternSums = items;
            return items;
        }

        public List<SelectListItem> DropDownIndustryName()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.IndustryName);
            ViewBag.DropDownIndustryNames = items;
            return items;
        }

        public List<SelectListItem> DropDownReferredType()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.ReferredType);
            ViewBag.DropDownReferredTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownPremiumType()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.PremiumType);
            ViewBag.DropDownPremiumTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownUnderwritingMethod()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.UnderwritingMethod);
            ViewBag.DropDownUnderwritingMethods = items;
            return items;
        }

        public List<SelectListItem> DropDownTreatyPricingUnderwritingLimit(int treatyPricingCedantId, bool withVersion = true)
        {
            var items = GetEmptyDropDownList();
            foreach (var uwLimitVersion in TreatyPricingUwLimitVersionService.GetByTreatyPricingCedantId(treatyPricingCedantId, true).OrderBy(a => a.TreatyPricingUwLimitBo.LimitId).ThenBy(a => a.Version))
            {
                string value = uwLimitVersion.Id.ToString();
                string text = string.Format("{0} - {1}", uwLimitVersion.TreatyPricingUwLimitBo.LimitId, uwLimitVersion.TreatyPricingUwLimitBo.Name);
                if (withVersion)
                {
                    value = string.Format("{0}|{1}", value, uwLimitVersion.TreatyPricingUwLimitId.ToString());
                    text = string.Format("{0} v{1}.0", text, uwLimitVersion.Version.ToString());
                }

                items.Add(new SelectListItem { Text = text, Value = value });
            }
            ViewBag.DropDownTreatyPricingUnderwritingLimits = items;
            return items;
        }

        public List<SelectListItem> DropDownTreatyPricingClaimApprovalLimit(int treatyPricingCedantId, bool withVersion = true)
        {
            var items = GetEmptyDropDownList();
            foreach (var claimApprovalLimitVersion in TreatyPricingClaimApprovalLimitVersionService.GetByTreatyPricingCedantId(treatyPricingCedantId, true).OrderBy(a => a.TreatyPricingClaimApprovalLimitBo.Code).ThenBy(a => a.Version))
            {
                string value = claimApprovalLimitVersion.Id.ToString();
                string text = string.Format("{0} - {1}", claimApprovalLimitVersion.TreatyPricingClaimApprovalLimitBo.Code, claimApprovalLimitVersion.TreatyPricingClaimApprovalLimitBo.Name);
                if (withVersion)
                {
                    value = string.Format("{0}|{1}", value, claimApprovalLimitVersion.TreatyPricingClaimApprovalLimitId.ToString());
                    text = string.Format("{0} v{1}.0", text, claimApprovalLimitVersion.Version.ToString());
                }

                items.Add(new SelectListItem { Text = text, Value = value });
            }
            ViewBag.DropDownTreatyPricingClaimApprovalLimits = items;
            return items;
        }

        public List<SelectListItem> DropDownTreatyPricingRateTable(int treatyPricingCedantId, bool withVersion = true)
        {
            var items = GetEmptyDropDownList();
            foreach (var rateTableVersion in TreatyPricingRateTableVersionService.GetByTreatyPricingCedantId(treatyPricingCedantId, true).OrderBy(a => a.TreatyPricingRateTableBo.Code).ThenBy(a => a.Version))
            {
                string value = rateTableVersion.Id.ToString();
                string text = string.Format("{0} - {1}", rateTableVersion.TreatyPricingRateTableBo.Code, rateTableVersion.TreatyPricingRateTableBo.Name);
                if (withVersion)
                {
                    value = string.Format("{0}|{1}", value, rateTableVersion.TreatyPricingRateTableId.ToString());
                    text = string.Format("{0} v{1}.0", text, rateTableVersion.Version.ToString());
                }

                items.Add(new SelectListItem { Text = text, Value = value });
            }
            ViewBag.DropDownTreatyPricingRateTables = items;
            return items;
        }

        public List<SelectListItem> DropDownTreatyPricingDefinitionExclusion(int treatyPricingCedantId, bool withVersion = true)
        {
            var items = GetEmptyDropDownList();
            foreach (var definitionAndExclusion in TreatyPricingDefinitionAndExclusionVersionService.GetByTreatyPricingCedantId(treatyPricingCedantId, true).OrderBy(a => a.TreatyPricingDefinitionAndExclusionBo.Code).ThenBy(a => a.Version))
            {
                string value = definitionAndExclusion.Id.ToString();
                string text = string.Format("{0} - {1}", definitionAndExclusion.TreatyPricingDefinitionAndExclusionBo.Code, definitionAndExclusion.TreatyPricingDefinitionAndExclusionBo.Name);
                if (withVersion)
                {
                    value = string.Format("{0}|{1}", value, definitionAndExclusion.TreatyPricingDefinitionAndExclusionId.ToString());
                    text = string.Format("{0} v{1}.0", text, definitionAndExclusion.Version.ToString());
                }

                items.Add(new SelectListItem { Text = text, Value = value });
            }
            ViewBag.DropDownTreatyPricingDefinitionExclusions = items;
            return items;
        }

        public List<SelectListItem> DropDownTreatyPricingMedicalTable(int treatyPricingCedantId, bool withVersion = true)
        {
            var items = GetEmptyDropDownList();
            foreach (var medicalTable in TreatyPricingMedicalTableVersionService.GetByTreatyPricingCedantId(treatyPricingCedantId, true).OrderBy(a => a.TreatyPricingMedicalTableBo.MedicalTableId).ThenBy(a => a.Version))
            {
                string value = medicalTable.Id.ToString();
                string text = string.Format("{0} - {1}", medicalTable.TreatyPricingMedicalTableBo.MedicalTableId, medicalTable.TreatyPricingMedicalTableBo.Name);
                if (withVersion)
                {
                    value = string.Format("{0}|{1}", value, medicalTable.TreatyPricingMedicalTableId.ToString());
                    text = string.Format("{0} v{1}.0", text, medicalTable.Version.ToString());
                }

                items.Add(new SelectListItem { Text = text, Value = value });
            }
            ViewBag.DropDownTreatyPricingMedicalTables = items;
            return items;
        }

        public List<SelectListItem> DropDownTreatyPricingFinancialTable(int treatyPricingCedantId, bool withVersion = true)
        {
            var items = GetEmptyDropDownList();
            foreach (var financialTable in TreatyPricingFinancialTableVersionService.GetByTreatyPricingCedantId(treatyPricingCedantId, true).OrderBy(a => a.TreatyPricingFinancialTableBo.FinancialTableId).ThenBy(a => a.Version))
            {
                string value = financialTable.Id.ToString();
                string text = string.Format("{0} - {1}", financialTable.TreatyPricingFinancialTableBo.FinancialTableId, financialTable.TreatyPricingFinancialTableBo.Name);
                if (withVersion)
                {
                    value = string.Format("{0}|{1}", value, financialTable.TreatyPricingFinancialTableId.ToString());
                    text = string.Format("{0} v{1}.0", text, financialTable.Version.ToString());
                }

                items.Add(new SelectListItem { Text = text, Value = value });
            }
            ViewBag.DropDownTreatyPricingFinancialTables = items;
            return items;
        }

        public List<SelectListItem> DropDownTreatyPricingUwQuestionnaire(int treatyPricingCedantId, bool withVersion = true)
        {
            var items = GetEmptyDropDownList();
            foreach (var wwQuestionnaire in TreatyPricingUwQuestionnaireVersionService.GetByTreatyPricingCedantId(treatyPricingCedantId, true).OrderBy(a => a.TreatyPricingUwQuestionnaireBo.Code).ThenBy(a => a.Version))
            {
                string value = wwQuestionnaire.Id.ToString();
                string text = string.Format("{0} - {1}", wwQuestionnaire.TreatyPricingUwQuestionnaireBo.Code, wwQuestionnaire.TreatyPricingUwQuestionnaireBo.Name);
                if (withVersion)
                {
                    value = string.Format("{0}|{1}", value, wwQuestionnaire.TreatyPricingUwQuestionnaireId.ToString());
                    text = string.Format("{0} v{1}.0", text, wwQuestionnaire.Version.ToString());
                }

                items.Add(new SelectListItem { Text = text, Value = value });
            }
            ViewBag.DropDownTreatyPricingUwQuestionnaires = items;
            return items;
        }

        public List<SelectListItem> DropDownTreatyPricingAdvantageProgram(int treatyPricingCedantId, bool withVersion = true)
        {
            var items = GetEmptyDropDownList();
            foreach (var advantageProgram in TreatyPricingAdvantageProgramVersionService.GetByTreatyPricingCedantId(treatyPricingCedantId, true).OrderBy(a => a.TreatyPricingAdvantageProgramBo.Code).ThenBy(a => a.Version))
            {
                string value = advantageProgram.Id.ToString();
                string text = string.Format("{0} - {1}", advantageProgram.TreatyPricingAdvantageProgramBo.Code, advantageProgram.TreatyPricingAdvantageProgramBo.Name);
                if (withVersion)
                {
                    value = string.Format("{0}|{1}", value, advantageProgram.TreatyPricingAdvantageProgramId.ToString());
                    text = string.Format("{0} v{1}.0", text, advantageProgram.Version.ToString());
                }

                items.Add(new SelectListItem { Text = text, Value = value });
            }
            ViewBag.DropDownTreatyPricingAdvantagePrograms = items;
            return items;
        }

        public List<SelectListItem> DropDownTreatyPricingProfitCommission(int treatyPricingCedantId, bool withVersion = true)
        {
            var items = GetEmptyDropDownList();
            foreach (var profitCommission in TreatyPricingProfitCommissionVersionService.GetByTreatyPricingCedantId(treatyPricingCedantId, true).OrderBy(a => a.TreatyPricingProfitCommissionBo.Code).ThenBy(a => a.Version))
            {
                string value = profitCommission.Id.ToString();
                string text = string.Format("{0} - {1}", profitCommission.TreatyPricingProfitCommissionBo.Code, profitCommission.TreatyPricingProfitCommissionBo.Name);
                if (withVersion)
                {
                    value = string.Format("{0}|{1}", value, profitCommission.TreatyPricingProfitCommissionId.ToString());
                    text = string.Format("{0} v{1}.0", text, profitCommission.Version.ToString());
                }

                items.Add(new SelectListItem { Text = text, Value = value });
            }
            ViewBag.DropDownTreatyPricingProfitCommissions = items;
            return items;
        }

        public List<string> GetRemarkSubjects()
        {
            var subjects = RemarkService.GetRemarkSubjects().ToList();
            ViewBag.RemarkSubjects = subjects;
            return subjects;
        }

        public List<ObjectVersionChangelog> GetObjectVersionChangelog(string controller, int objectId, ObjectVersion model)
        {
            var items = new List<ObjectVersionChangelog>();
            string versionController = string.Format("{0}Version", controller);
            string versionTable = string.Format("{0}s", versionController);

            int objectType = TreatyPricingWorkflowObjectBo.GetObjectTypeFromController(controller);
            ViewBag.WorkflowObjectType = objectType;

            foreach (var userTrail in UserTrailService.GetByControllerObject(controller, objectId))
            {
                TrailObject trail = JsonConvert.DeserializeObject<TrailObject>(userTrail.Data);
                if (trail.Insert != null)
                {
                    if (TrailObject.FindFirstTable(trail.Insert, versionTable) != null)
                    {
                        // First version (Created when creating object) 
                        object versionObject = model.FindVersionObjectByVersion(1);
                        int firstVersionId = int.Parse(versionObject.GetPropertyValue("Id").ToString());

                        TreatyPricingWorkflowObjectBo workflowObjectBo = TreatyPricingWorkflowObjectService.FindByObject(objectType, objectId, firstVersionId);
                        items.Add(new ObjectVersionChangelog(versionObject, userTrail, workflowObjectBo));

                        foreach (int versionId in model.GetVersionIds())
                        {
                            foreach (var versionUserTrail in UserTrailService.GetByControllerObject(versionController, versionId))
                            {
                                TrailObject versionTrail = JsonConvert.DeserializeObject<TrailObject>(versionUserTrail.Data);
                                var versionTrailTable = TrailObject.FindFirstTable(versionTrail.Insert, versionTable);
                                if (versionTrailTable == null)
                                    continue;

                                int id = int.Parse(versionTrailTable.Name);

                                object tempVersionObject = model.FindVersionObject(id);
                                workflowObjectBo = TreatyPricingWorkflowObjectService.FindByObject(objectType, objectId, id);
                                ObjectVersionChangelog changelog = new ObjectVersionChangelog(tempVersionObject, versionUserTrail, workflowObjectBo);
                                changelog.FormatBetweenVersionTrail(versionObject);

                                items.Insert(0, changelog);

                                versionObject = tempVersionObject;
                            }
                        }
                        continue;
                    }
                }

                if (trail.Update != null)
                {
                    var versionTrailTable = TrailObject.FindFirstTable(trail.Update, versionTable);
                    if (versionTrailTable == null)
                        continue;

                    int versionId = int.Parse(versionTrailTable.Name);

                    ObjectVersionChangelog changelog = items.Where(q => q.ObjectVersionId == versionId).FirstOrDefault();
                    if (changelog == null)
                        continue;

                    changelog.AddTrail(userTrail);
                }
            }

            ViewBag.ObjectVersionChangelogs = items;

            return items;
        }

        protected List<SelectListItem> DropDownInsuredGroupName(int? status = null, int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            foreach (var insuredGroupName in InsuredGroupNameService.GetByStatus(status, selectedId))
            {
                var selected = insuredGroupName.Id == selectedId;
                items.Add(new SelectListItem { Text = insuredGroupName.Name.ToString(), Value = insuredGroupName.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownInsuredGroupNames = items;
            return items;
        }

        protected List<SelectListItem> DropDownTemplate(int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            foreach (var template in TemplateService.Get())
            {
                var selected = template.Id == selectedId;
                items.Add(new SelectListItem { Text = template.Code, Value = template.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownTemplates = items;
            return items;
        }

        protected List<SelectListItem> DropDownTemplateDocumentTypes()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = "Quotation Workflow - Quote Spec", Value = "Quotation Workflow - Quote Spec" });
            items.Add(new SelectListItem { Text = "Quotation Workflow - Campaign Spec", Value = "Quotation Workflow - Campaign Spec" });
            items.Add(new SelectListItem { Text = "RI Group Slip", Value = "Ri Group Slip" });
            items.Add(new SelectListItem { Text = "Reply Template", Value = "Reply Template" });
            ViewBag.DropDownTemplateDocumentTypes = items;
            return items;
        }

        protected List<SelectListItem> DropDownQuoteSpecTemplate(int? cedantId = null, int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            IList<TemplateBo> templateList = new List<TemplateBo>();
            IList<TemplateBo> campaignSpecTemplateList = new List<TemplateBo>();

            //if (cedantId.HasValue && cedantId > 0)
            //{
            //    templateList = TemplateService.GetByCedantAndDocumentType(cedantId ?? 0, "Quotation Workflow - Quote Spec");
            //    campaignSpecTemplateList = TemplateService.GetByCedantAndDocumentType(cedantId ?? 0, "Quotation Workflow - Campaign Spec");

            //    ((List<TemplateBo>)templateList).AddRange(campaignSpecTemplateList);
            //}
            //else
            //{
            //    templateList = TemplateService.Get();
            //}
            templateList = TemplateService.GetByDocumentType("Quotation Workflow - Quote Spec");
            campaignSpecTemplateList = TemplateService.GetByDocumentType("Quotation Workflow - Campaign Spec");

            ((List<TemplateBo>)templateList).AddRange(campaignSpecTemplateList);

            foreach (var template in templateList)
            {
                var selected = template.Id == selectedId;
                items.Add(new SelectListItem { Text = template.Code, Value = template.Code.ToString(), Selected = selected });
            }
            ViewBag.DropDownQuoteSpecTemplates = items;
            return items;
        }

        protected List<SelectListItem> DropDownRateTableTemplate(int? cedantId = null, int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            //IList<TemplateBo> templateList = new List<TemplateBo>();

            //templateList = (cedantId.HasValue && cedantId > 0)
            //    ? TemplateService.GetByCedantAndDocumentType(cedantId ?? 0, "Quotation Workflow - Rate Table")
            //    : TemplateService.Get();

            //foreach (var template in templateList)
            //{
            //    var selected = template.Id == selectedId;
            //    items.Add(new SelectListItem { Text = template.Code, Value = template.Code.ToString(), Selected = selected });
            //}
            items.Add(new SelectListItem { Text = "Reinsurance", Value = "Reinsurance", Selected = false });
            items.Add(new SelectListItem { Text = "Retakaful", Value = "Retakaful", Selected = false });
            ViewBag.DropDownRateTableTemplates = items;
            return items;
        }

        protected void GetTreatyTypes()
        {
            List<string> treatyTypes = new List<string>();
            foreach (var pickListDetailBo in PickListDetailService.GetByPickListId(PickListBo.TreatyType))
            {
                treatyTypes.Add(pickListDetailBo.Code);
            }
            ViewBag.TreatyTypes = treatyTypes;
        }

        protected void GetClaimCodes()
        {
            List<string> claimCodes = new List<string>();
            foreach (var claimCodeBo in ClaimCodeService.GetByStatus(ClaimCodeBo.StatusActive))
            {
                claimCodes.Add(claimCodeBo.Code);
            }
            ViewBag.ClaimCodes = claimCodes;
        }

        protected List<SelectListItem> DropDownRetroBenefitCode(int? status = null, int? selectedId = null, string selectedCode = null, bool codeAsValue = false, bool isDetail = false, List<int> selectedIds = null)
        {
            var items = GetEmptyDropDownList();

            if (!isDetail)
            {
                foreach (var retroBenefitCode in RetroBenefitCodeService.GetByStatus(status, selectedId))
                {
                    var selected = codeAsValue ? (retroBenefitCode.Code == selectedCode) : (retroBenefitCode.Id == selectedId);
                    string value = codeAsValue ? retroBenefitCode.Code : retroBenefitCode.Id.ToString();
                    items.Add(new SelectListItem { Text = retroBenefitCode.ToString(), Value = value, Selected = selected });
                }
            }
            else
            {
                foreach (var retroBenefitCode in RetroBenefitCodeService.GetByStatusByIds(status, selectedIds))
                {
                    string value = retroBenefitCode.Id.ToString();
                    items.Add(new SelectListItem { Text = retroBenefitCode.ToString(), Value = value });
                }
            }

            ViewBag.DropDownRetroBenefitCodes = items;
            return items;
        }

        protected void GetBusinessOrigins()
        {
            List<string> businessOrigins = new List<string>();
            foreach (var pickListDetailBo in PickListDetailService.GetByPickListId(PickListBo.BusinessOrigin))
            {
                businessOrigins.Add(pickListDetailBo.Code);
            }
            ViewBag.BusinessOrigins = businessOrigins;
        }

        protected void GetInvoiceFields()
        {
            List<string> invoiceFields = new List<string>();
            foreach (var pickListDetailBo in PickListDetailService.GetByPickListId(PickListBo.InvoiceField))
            {
                invoiceFields.Add(pickListDetailBo.Code);
            }
            ViewBag.InvoiceFields = invoiceFields;
        }

        public List<SelectListItem> DropDownInvoiceField()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.InvoiceField);
            ViewBag.DropDownInvoiceFields = items;
            return items;
        }

        public List<SelectListItem> DropDownRetroRegisterField()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.RetroRegisterField);
            ViewBag.DropDownRetroRegisterFields = items;
            return items;
        }

        protected void GetEntitlements()
        {
            List<string> entitlements = new List<string>();
            foreach (var pickListDetailBo in PickListDetailService.GetByPickListId(PickListBo.Entitlement))
            {
                entitlements.Add(pickListDetailBo.ToString());
            }
            ViewBag.Entitlements = entitlements;
        }

        public List<SelectListItem> DropDownMfrs17ContractCode()
        {
            var items = GetEmptyDropDownList();
            var mfrs17ContractCodes = Mfrs17ContractCodeService.Get();
            foreach (var mfrs17ContractCode in mfrs17ContractCodes)
            {
                var text = mfrs17ContractCode.CedingCompanyBo.ToString() + " - " + mfrs17ContractCode.ModifiedContractCode;
                items.Add(new SelectListItem { Text = text, Value = mfrs17ContractCode.Id.ToString() });
            }
            ViewBag.DropDownMfrs17ContractCodes = items;
            return items;
        }

        public List<SelectListItem> DropDownMfrs17ContractCodeDetail()
        {
            var items = GetEmptyDropDownList();
            var mfrs17ContractCodeDetails = Mfrs17ContractCodeDetailService.Get();
            foreach (var mfrs17ContractCodeDetail in mfrs17ContractCodeDetails)
            {
                var text = mfrs17ContractCodeDetail.ContractCode;
                items.Add(new SelectListItem { Text = text, Value = mfrs17ContractCodeDetail.Id.ToString() });
            }
            ViewBag.DropDownMfrs17ContractCodeDetails = items;
            return items;
        }

        public List<SelectListItem> DropDownTerritoryOfIssueCode(bool codeAsValue = false)
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.TerritoryOfIssueCode, codeAsValue: codeAsValue);
            ViewBag.DropDownTerritoryOfIssueCodes = items;
            return items;
        }

        public List<SelectListItem> DropDownExceptionStatus()
        {
            var items = GetPickListDetailIdDropDownByPickListId(PickListBo.ExceptionStatus);
            ViewBag.DropDownExceptionStatuses = items;
            return items;
        }

        protected List<SelectListItem> DropDownPerLifeRetroGender(int? selectedId = null)
        {
            var items = GetEmptyDropDownList();

            foreach (var perLifeRetroGenderBo in PerLifeRetroGenderService.Get())
            {
                var selected = perLifeRetroGenderBo.Id == selectedId;
                items.Add(new SelectListItem { Text = perLifeRetroGenderBo.ToString(), Value = perLifeRetroGenderBo.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownPerLifeRetroGenders = items;
            return items;
        }

        protected List<SelectListItem> DropDownActiveInactive()
        {
            var items = GetEmptyDropDownList();
            items.Add(new SelectListItem { Text = "Active", Value = "1" });
            items.Add(new SelectListItem { Text = "Inactive", Value = "2" });
            ViewBag.DropDownActiveInactive = items;
            return items;
        }

        protected List<SelectListItem> DropDownPerLifeRetroCountry(int? selectedId = null)
        {
            var items = GetEmptyDropDownList();

            foreach (var perLifeRetroCountryBo in PerLifeRetroCountryService.Get())
            {
                var selected = perLifeRetroCountryBo.Id == selectedId;
                items.Add(new SelectListItem { Text = perLifeRetroCountryBo.ToString(), Value = perLifeRetroCountryBo.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownPerLifeRetroCountries = items;
            return items;
        }

        public List<SelectListItem> DropDownRetroTreaty(int? status = null, int? selectedId = null)
        {
            var items = GetEmptyDropDownList();
            foreach (var retroTreaty in RetroTreatyService.GetByStatus(status, selectedId))
            {
                var selected = retroTreaty.Id == selectedId;
                items.Add(new SelectListItem { Text = retroTreaty.Code, Value = retroTreaty.Id.ToString(), Selected = selected });
            }
            ViewBag.DropDownRetroTreaties = items;
            return items;
        }

        public List<SelectListItem> DropDownTreatyWorkflowDocumentType()
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingTreatyWorkflowBo.DocumentTypeLetter))
            {
                items.Add(new SelectListItem { Text = TreatyPricingTreatyWorkflowBo.GetDocumentTypeName(i), Value = i.ToString() });
            }
            ViewBag.TreatyWorkflowDocumentTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownWorkflowObjectTypes(bool codeAsValue = false)
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingWorkflowObjectBo.MaxType))
            {
                string code = TreatyPricingWorkflowObjectBo.GetTypeName(i);
                string value = codeAsValue ? code : i.ToString();
                items.Add(new SelectListItem { Text = code, Value = value });
            }
            ViewBag.DropDownWorkflowObjectTypes = items;
            return items;
        }

        public List<SelectListItem> DropDownWorkflowStatus(bool codeAsValue = false)
        {
            var items = GetEmptyDropDownList();
            foreach (var i in Enumerable.Range(1, TreatyPricingQuotationWorkflowBo.StatusMax))
            {
                string code = TreatyPricingQuotationWorkflowBo.GetStatusName(i);
                string value = codeAsValue ? code : i.ToString();
                items.Add(new SelectListItem { Text = code, Value = value });
            }
            foreach (var i in Enumerable.Range(1, TreatyPricingTreatyWorkflowBo.DocumentStatusMax))
            {
                string code = TreatyPricingTreatyWorkflowBo.GetDocumentStatusName(i);
                string value = codeAsValue ? code : i.ToString();
                items.Add(new SelectListItem { Text = code, Value = value });
            }
            ViewBag.DropDownWorkflowStatus = items;
            return items;
        }

        public string GetFullBaseUrl()
        {
            string baseUrl = "";
            if (Util.GetConfigBoolean("RepoObjectUseCookiePath"))
                baseUrl += Util.GetConfig("CookiePath");

            ViewBag.BaseUrl = baseUrl;

            return baseUrl;
        }
    }
}