using BusinessObject;
using BusinessObject.Identity;
using PagedList;
using Services;
using Services.Identity;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Auth]
    public class UserController : BaseController
    {
        public const string Controller = "User";

        // GET: User
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Index(
            string FullName,
            string UserName,
            int? DepartmentId,
            int? AccessGroupId,
            string Email,
            int? Status,
            string SortOrder,
            int? Page
        )
        {
            ViewBag.RouteValue = new RouteValueDictionary
            {
                ["FullName"] = FullName,
                ["UserName"] = UserName,
                ["DepartmentId"] = DepartmentId,
                ["AccessGroupId"] = AccessGroupId,
                ["Email"] = Email,
                ["Status"] = Status,
                ["SortOrder"] = SortOrder,
            };
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortFullName = GetSortParam("FullName");
            ViewBag.SortUserName = GetSortParam("UserName");
            ViewBag.SortDepartmentId = GetSortParam("DepartmentId");
            ViewBag.SortAccessGroupId = GetSortParam("AccessGroupId");
            ViewBag.SortEmail = GetSortParam("Email");

            var query = _db.GetUsers().Select(UserViewModel.Expression());
            if (!string.IsNullOrEmpty(FullName)) query = query.Where(q => q.FullName.ToLower().Contains(FullName.ToLower()));
            if (!string.IsNullOrEmpty(UserName)) query = query.Where(q => q.UserName.ToLower().Contains(UserName.ToLower()));
            if (DepartmentId != null) query = query.Where(q => q.DepartmentId == DepartmentId);
            if (AccessGroupId != null) query = query.Where(q => q.UserAccessGroups.Any(uag => uag.AccessGroupId == AccessGroupId));
            if (!string.IsNullOrEmpty(Email)) query = query.Where(q => q.Email.ToLower().Contains(Email.ToLower()));
            if (Status != null) query = query.Where(q => q.Status == Status);

            if (SortOrder == Html.GetSortAsc("FullName")) query = query.OrderBy(q => q.FullName);
            else if (SortOrder == Html.GetSortDsc("FullName")) query = query.OrderByDescending(q => q.FullName);
            else if (SortOrder == Html.GetSortAsc("UserName")) query = query.OrderBy(q => q.UserName);
            else if (SortOrder == Html.GetSortDsc("UserName")) query = query.OrderByDescending(q => q.UserName);
            else if (SortOrder == Html.GetSortAsc("DepartmentId")) query = query.OrderBy(q => q.Department.Name);
            else if (SortOrder == Html.GetSortDsc("DepartmentId")) query = query.OrderByDescending(q => q.Department.Name);
            else if (SortOrder == Html.GetSortAsc("AccessGroupId")) query = query.OrderBy(q => q.UserAccessGroups.OrderBy(uag => uag.AccessGroup.Name).FirstOrDefault().AccessGroup.Name);
            else if (SortOrder == Html.GetSortDsc("AccessGroupId")) query = query.OrderByDescending(q => q.UserAccessGroups.OrderByDescending(uag => uag.AccessGroup.Name).FirstOrDefault().AccessGroup.Name);
            else if (SortOrder == Html.GetSortAsc("Email")) query = query.OrderBy(q => q.Email);
            else if (SortOrder == Html.GetSortDsc("Email")) query = query.OrderByDescending(q => q.Email);
            else query = query.OrderBy(q => q.UserName);

            ViewBag.Total = query.Count();
            IndexPage();
            return View(query.ToPagedList(Page ?? 1, PageSize));
        }

        // GET: User/Details/5
        [Auth(Controller = Controller, Power = "R")]
        public ActionResult Details(int id)
        {
            UserBo userBo = UserService.Find(id);
            if (userBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new UserViewModel(userBo));
        }

        // GET: User/Create
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create()
        {
            UserViewModel model = new UserViewModel();
            LoadPage(model);
            return View(model);
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "C")]
        public ActionResult Create(FormCollection form, UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userBo = new UserBo
                {
                    Status = model.Status,
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    DepartmentId = model.DepartmentId,
                    FullName = model.FullName,
                    LoginMethod = model.LoginMethod,
                    CreatedById = AuthUserId,
                    UpdatedById = AuthUserId,
                };

                TrailObject trail = GetNewTrailObject();
                Result = UserService.Create(ref userBo, ref trail);
                if (Result.Valid)
                {
                    model.Id = userBo.Id;
                    model.ProcessStatusHistory(AuthUserId, ref trail);
                    if (model.AccessGroupId != 0)
                    {
                        UserService.AddToAccessGroup(userBo, model.AccessGroupId, ref trail);
                    }


                    int emailType = userBo.LoginMethod == UserBo.LoginMethodPassword ? EmailBo.TypeNewPasswordUser : EmailBo.TypeNewActiveDirectoryUser;
                    GetNewEmail(emailType, userBo.Email, userBo.Id);
                    EmailBo.AddData(userBo.UserName);
                    if (userBo.LoginMethod == UserBo.LoginMethodPassword)
                    {
                        var userPasswordBo = new UserPasswordBo
                        {
                            UserId = userBo.Id,
                            PasswordHash = userBo.PasswordHash,
                            CreatedById = AuthUserId,
                        };
                        UserPasswordService.Create(ref userPasswordBo, ref trail);

                        EmailBo.AddData(model.Password);
                    }

                    List<DocumentBo> documentBos = DocumentController.GetDocuments(form);
                    DocumentController.SaveDocuments(documentBos, model.ModuleId, userBo.Id, AuthUserId, ref trail);

                    GenerateMail(warningMessage: MessageBag.NewUserNotSent);
                    SaveEmail(ref trail, 1);

                    CreateTrail(
                        userBo.Id,
                        "Create User"
                    );

                    SetCreateSuccessMessage(Controller);
                    return RedirectToAction("Edit", new { id = userBo.Id });
                }
                AddResult(Result);
            }

            LoadPage(model, form);
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            if (!CheckEditPageReadOnly(Controller))
                return RedirectDashboard();

            UserBo userBo = UserService.Find(id);
            if (userBo == null)
            {
                return RedirectToAction("Index");
            }

            UserViewModel model = new UserViewModel(userBo);
            LoadPage(model);
            return View(model);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Edit(int id, FormCollection form, UserViewModel model)
        {
            UserBo userBo = UserService.Find(id);
            if (userBo == null)
            {
                return RedirectToAction("Index");
            }

            if (userBo.Id == AuthUserId)
            {
                ModelState.AddModelError("", MessageBag.UnableUpdateSelf);
            }

            if (userBo.LoginMethod == model.LoginMethod || model.LoginMethod == UserBo.LoginMethodAD)
            {
                ModelState.Remove("Password");
            }

            if (ModelState.IsValid)
            {
                userBo.UserName = model.UserName;
                userBo.LoginMethod = model.LoginMethod;
                userBo.Email = model.Email;
                userBo.DepartmentId = model.DepartmentId;
                userBo.FullName = model.FullName;
                userBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = UserService.Update(ref userBo, ref trail);
                if (Result.Valid)
                {
                    if (userBo.LoginMethod == UserBo.LoginMethodPassword && !string.IsNullOrEmpty(model.Password))
                    {
                        Result = UserService.UpdatePassword(ref userBo, model.Password, ref trail);
                    }

                    if (Result.Valid)
                    {
                        UserService.AddToAccessGroup(userBo, model.AccessGroupId, ref trail);

                        CreateTrail(
                            userBo.Id,
                            "Update User"
                        );

                        SetUpdateSuccessMessage(Controller);
                        return RedirectToAction("Edit", new { id = userBo.Id });
                    }
                }
                AddResult(Result);
            }

            object selectedAccessGroupId = null;
            if (userBo.UserAccessGroupBos != null && userBo.UserAccessGroupBos.Count > 0)
            {
                selectedAccessGroupId = userBo.UserAccessGroupBos[0].AccessGroupId;
            }

            LoadPage(model, form);
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Suspend(int id, FormCollection form, UserViewModel model)
        {
            UserBo userBo = UserService.Find(id);
            if (userBo == null)
            {
                return RedirectToAction("Index");
            }

            if (userBo.Id == AuthUserId)
            {
                ModelState.AddModelError("", MessageBag.UnableUpdateSelf);
            }

            userBo.Status = UserBo.StatusSuspend;
            userBo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = UserService.Update(ref userBo, ref trail);
            if (Result.Valid)
            {
                model.Status = userBo.Status;
                model.ProcessStatusHistory(AuthUserId, ref trail);

                CreateTrail(
                    userBo.Id,
                    "Suspend User"
                );

                SetSuccessSessionMsg(MessageBag.UserSuccessfullySuspended);
                return RedirectToAction("Edit", new { id = userBo.Id });
            }
            AddResult(Result);

            LoadPage(model, form);
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult Activate(int id, FormCollection form, UserViewModel model)
        {
            UserBo userBo = UserService.Find(id);
            if (userBo == null)
            {
                return RedirectToAction("Index");
            }

            if (userBo.Id == AuthUserId)
            {
                ModelState.AddModelError("", MessageBag.UnableUpdateSelf);
            }

            userBo.Status = UserBo.StatusActive;
            userBo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = UserService.Update(ref userBo, ref trail);
            if (Result.Valid)
            {
                model.Status = userBo.Status;
                model.ProcessStatusHistory(AuthUserId, ref trail);

                CreateTrail(
                    userBo.Id,
                    "Activate User"
                );

                SetSuccessSessionMsg(MessageBag.UserSuccessfullyActivated);
                return RedirectToAction("Edit", new { id = userBo.Id });
            }
            AddResult(Result);

            LoadPage(model, form);
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "U")]
        public ActionResult SendNewPassword(int id, FormCollection form, UserViewModel model)
        {
            UserBo userBo = UserService.Find(id);
            if (userBo == null)
            {
                return RedirectToAction("Index");
            }

            if (userBo.Id == AuthUserId)
            {
                ModelState.AddModelError("", MessageBag.UnableUpdateSelf);
            }

            if (ModelState.IsValid)
            {
                userBo.UserName = model.UserName;
                userBo.LoginMethod = model.LoginMethod;
                userBo.Email = model.Email;
                userBo.DepartmentId = model.DepartmentId;
                userBo.FullName = model.FullName;
                userBo.UpdatedById = AuthUserId;

                TrailObject trail = GetNewTrailObject();
                Result = UserService.Update(ref userBo, ref trail);
                if (Result.Valid)
                {
                    Result = UserService.UpdatePassword(ref userBo, model.Password, ref trail);
                    if (Result.Valid)
                    {
                        GetNewEmail(EmailBo.TypeChangePassword, userBo.Email, userBo.Id);
                        EmailBo.AddData(model.Password);
                        bool success = GenerateMail(warningMessage: MessageBag.NewPasswordNotSent);
                        SaveEmail(ref trail, 0);

                        CreateTrail(
                            userBo.Id,
                            "Update User"
                        );

                        if (success)
                            SetSuccessSessionMsg(MessageBag.PasswordEmailedSuccesfully);
                        return RedirectToAction("Edit", new { id = userBo.Id });
                    }
                }

                AddResult(Result);
            }

            LoadPage(model, form);
            return View("Edit", model);
        }

        // GET: User/Delete/5
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult Delete(int id)
        {
            SetViewBagMessage();
            UserBo userBo = UserService.Find(id);
            if (userBo == null)
            {
                return RedirectToAction("Index");
            }
            return View(new UserViewModel(userBo));
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Controller = Controller, Power = "D")]
        public ActionResult DeleteConfirmed(int id, UserViewModel model)
        {
            UserBo userBo = UserService.Find(id);
            if (userBo == null)
            {
                return RedirectToAction("Index");
            }

            if (userBo.Id == AuthUserId)
            {
                ModelState.AddModelError("", MessageBag.UnableDeleteSelf);
            }

            TrailObject trail = GetNewTrailObject();

            userBo.Status = UserBo.StatusDelete;
            userBo.UpdatedById = AuthUserId;
            userBo.PasswordExpiresAt = null;
            Result = UserService.Delete(ref userBo, ref trail);
            if (Result.Valid)
            {
                CreateTrail(
                    userBo.Id,
                    "Delete User"
                );

                SetDeleteSuccessMessage(Controller);
                return RedirectToAction("Index");
            }

            if (Result.MessageBag.Errors.Count > 1)
                SetErrorSessionMsgArr(Result.MessageBag.Errors.ToList());
            else
                SetErrorSessionMsg(Result.MessageBag.Errors[0]);

            return RedirectToAction("Delete", new { id = userBo.Id });
        }

        // GET: AccessGroup/RequestUser
        public ActionResult RequestUser()
        {
            UserRequestViewModel model = new UserRequestViewModel();
            LoadPage(model);
            return View(model);
        }

        // POST: AccessGroup/RequestAccessGroup
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestUser(FormCollection form, UserRequestViewModel model)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                if (!model.Download)
                {
                    model.Download = true;
                    ViewBag.AuthUserName = AuthUser.FullName;
                    LoadPage(model);

                    ViewBag.SuccessMessageBottom = "Your request form will be downloaded shortly.";

                    return View(model);
                }

                MemoryStream s = new MemoryStream();
                Pdf document = new Pdf(Server.MapPath("~/Document/User_Request_Form_Template.pdf"));

                document.CreateRequestedDate(model.RequestDateStr);
                document.AddVerticalSpace(10);

                document.SectionHeader("EMPLOYEE DETAILS");
                document.AddTextBox("Employee Name", model.FullName);
                document.AddTextBox("Username", model.UserName);
                document.AddTextBox("Email", model.Email);
                document.AddTextBox("Existing User", (model.RequestType == UserBo.RequestTypeNew) ? "No" : "Yes");
                document.AddTextBox("Login Method", UserBo.GetLoginMethodName(model.LoginMethod));
                document.AddVerticalSpace(20);

                DepartmentBo departmentBo = DepartmentService.Find(model.DepartmentId);
                string departmentName = departmentBo != null ? departmentBo.Name : "";

                AccessGroupBo accessGroupBo = AccessGroupService.Find(model.AccessGroupId);
                string accessGroupName = accessGroupBo != null ? accessGroupBo.Name : "";

                document.SectionHeader("REQUEST TYPE, DEPARTMENT & ACCESS GROUP");
                document.AddTextBox("Request Type", UserBo.GetRequestTypeName(model.RequestType));
                document.AddTextBox("Department", departmentName);
                document.AddTextBox("Access Group", accessGroupName);
                document.AddTextBox("Requested By", AuthUser.FullName);
                document.AddVerticalSpace(20);

                document.SectionHeader("APPROVAL");
                document.SignatureSection("HOD Signature");
                document.AddVerticalSpace(20);

                document.DrawITSection();

                document.Document.Save(s);
                var fileName = "User Application.pdf";
                var contentType = "application/pdf";

                return File(s, contentType, fileName);
            }

            model.Download = false;
            LoadPage(model);
            return View(model);
        }

        public void IndexPage()
        {
            DropDownDepartment(true);
            DropDownAccessGroup();
            DropDownStatus();
            AuthUserName();
            SetViewBagMessage();
        }

        public void LoadPage(UserViewModel model, FormCollection form = null)
        {
            DropDownLoginMethod();
            DropDownDepartment(true);
            DropDownAccessGroup(true, model.DepartmentId);
            GetStatusHistory(model.ModuleId, model.Id);
            DropDownRequestType();
            DropDownDocumentType();
            string authUserName = AuthUserName();

            string downloadDocumentUrl = Url.Action("Download", "Document");
            if (form != null)
                ViewBag.DocumentBos = DocumentController.GetDocuments(form, downloadDocumentUrl);
            else
                GetDocuments(model.ModuleId, model.Id, downloadDocumentUrl);

            model.Files = null;
            ViewBag.PasswordRequirement = UserBo.GetPasswordRequirements();
            SetViewBagMessage();
        }

        protected List<SelectListItem> DropDownLoginMethod()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= UserBo.MaxLoginMethod; i++)
            {
                items.Add(new SelectListItem { Text = UserBo.GetLoginMethodName(i), Value = i.ToString() });
            }
            ViewBag.DropDownLoginMethods = items;
            return items;
        }

        protected List<SelectListItem> DropDownStatus()
        {
            var items = GetEmptyDropDownList();
            for (int i = 1; i <= UserBo.MaxStatus; i++)
            {
                if (i == UserBo.StatusDelete)// Do not include deleted
                    continue;

                items.Add(new SelectListItem { Text = UserBo.GetStatusName(i), Value = i.ToString() });
            }
            ViewBag.DropDownStatus = items;
            return items;
        }

        // POST: User/GetActiveDirectoryEmail
        public JsonResult GetActiveDirectoryDetails(string userName)
        {
            string error = null;
            string email = null;
            string name = null;

            if (string.IsNullOrEmpty(userName))
            {
                error = MessageBag.UsernameRequired;
            }
            else
            {
                try
                {
                    UserBo userBo = UserService.FindInActiveDirectory(userName);
                    if (userBo != null)
                    {
                        email = userBo.Email;
                        name = userBo.FullName;
                    }
                    else
                    {
                        error = MessageBag.UserNotFoundInAd;
                    }
                }
                catch (Exception e)
                {
                    error = e.Message;
                }
            }

            return Json(new { error, email, name });
        }

        public JsonResult GetUser(string userName)
        {
            string error = null;
            UserBo userBo = null;

            if (string.IsNullOrEmpty(userName))
            {
                error = MessageBag.UsernameRequired;
            }
            else
            {
                userBo = UserService.FindByUsername(userName);
                if (userBo == null)
                {
                    error = MessageBag.NoRecordFound;
                }
            }

            return Json(new { error, userBo });
        }

        public JsonResult GeneratePassword()
        {
            int passwordLength = Int32.Parse(Util.GetConfig("RandomPasswordLength"));
            string password = Util.GenerateRandomString(passwordLength, passwordLength);

            return Json(new { password });
        }
    }
}