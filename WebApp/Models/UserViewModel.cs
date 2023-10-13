using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using Services;
using Services.Identity;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public int Status { get; set; }

        [Required]
        [Display(Name = "Username")]
        [StringLength(256)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Employee Name")]
        [StringLength(256)]
        public string FullName { get; set; }

        [RequiredIfPassword]
        //[DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password and Confirm Password does not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        public Department Department { get; set; }

        [Display(Name = "Department")]
        public DepartmentBo DepartmentBo { get; set; }

        public virtual ICollection<UserAccessGroup> UserAccessGroups { get; set; }

        [Display(Name = "User Access Group")]
        public virtual IList<UserAccessGroupBo> UserAccessGroupBos { get; set; }

        [Required]
        [Display(Name = "Access Group")]
        public int AccessGroupId { get; set; }

        [Required(ErrorMessage = "Please select a login method")]
        [Display(Name = "Login Method")]
        public int LoginMethod { get; set; }

        public List<SelectListItem> LoginMethods { get; set; }

        public List<HttpPostedFileBase> Files { get; set; }

        public int ModuleId { get; set; }

        public UserViewModel() 
        {
            Set();
        }

        public UserViewModel(UserBo userBo)
        {
            Set(userBo);
        }

        public void Set(UserBo userBo = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.User.ToString());
            ModuleId = moduleBo.Id;
            Password = "";
            ConfirmPassword = "";
            if (userBo != null)
            {
                Id = userBo.Id;
                Status = userBo.Status;
                UserName = userBo.UserName;
                Email = userBo.Email;
                FullName = userBo.FullName;
                DepartmentId = userBo.DepartmentId;
                DepartmentBo = userBo.DepartmentBo;
                UserAccessGroupBos = userBo.UserAccessGroupBos;
                AccessGroupId = userBo.UserAccessGroupBos[0].AccessGroupId;
                LoginMethod = userBo.LoginMethod;
            }
        }

        public void ProcessDocuments(FormCollection form, int authUserId, ref TrailObject trail)
        {
            int documentMaxIndex = int.Parse(form.Get("documentMaxIndex"));
            for (int i = 0; i <= documentMaxIndex; i++)
            {
                string documentId = form.Get(string.Format("documentId[{0}]", i));
                if (!string.IsNullOrEmpty(documentId))
                    continue;
                
                string tempFilePath = form.Get(string.Format("tempFilePath[{0}]", i));
                if (string.IsNullOrEmpty(tempFilePath))
                    continue;

                int type = int.Parse(form.Get(string.Format("type[{0}]", i)));
                string fileName = form.Get(string.Format("filename[{0}]", i));
                string hashFileName = form.Get(string.Format("hashFileName[{0}]", i));
                string description = form.Get(string.Format("description[{0}]", i));
                string createdAtStr = form.Get(string.Format("createdAtStr[{0}]", i));
                
                DateTime dateTime = DateTime.Parse(createdAtStr, new CultureInfo("fr-FR", false));
                ModuleBo moduleBo = ModuleService.Find(ModuleId);
                DocumentBo documentBo = new DocumentBo
                {
                    Type = type,
                    ModuleId = ModuleId,
                    ModuleBo = moduleBo,
                    ObjectId = Id,
                    FileName = fileName,
                    HashFileName = hashFileName,
                    Description = description,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                    CreatedAt = dateTime,
                    UpdatedAt = dateTime,
                };

                string path = documentBo.GetLocalPath();
                Util.MakeDir(path);
                string tempPath = documentBo.GetTempPath("Uploads");
                File.Move(tempPath, path);

                DocumentService.Save(ref documentBo);
            }
        }

        public void ProcessStatusHistory(int authUserId, ref TrailObject trail)
        {
            StatusHistoryBo statusHistoryBo = new StatusHistoryBo
            {
                ModuleId = ModuleId,
                ObjectId = Id,
                Status = Status,
                CreatedById = authUserId,
                UpdatedById = authUserId,
            };
            StatusHistoryService.Save(ref statusHistoryBo, ref trail);
        }

        public class RequiredIfPassword : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var loginMethod = validationContext.ObjectType.GetProperty("LoginMethod");
                var loginMethodValue = loginMethod.GetValue(validationContext.ObjectInstance, null);
                int minLength = int.Parse(Util.GetConfig("PasswordMinLength"));

                string password = value as string;
                if ((int)loginMethodValue == UserBo.LoginMethodPassword)
                {
                    if (string.IsNullOrEmpty(password))
                    {
                        return new ValidationResult("The Password field is Required");
                    }
                    
                    if (password.Length < minLength)
                    {
                        return new ValidationResult(string.Format("The Password must be at least {0} characters long.", minLength));
                        
                    }
                }
                return null;
            }
        }

        public static Expression<Func<User, UserViewModel>> Expression()
        {
            return entity => new UserViewModel
            {
                Id = entity.Id,
                DepartmentId = entity.DepartmentId,
                Department = entity.Department,
                Status = entity.Status,
                FullName = entity.FullName,
                UserName = entity.UserName,
                Email = entity.Email,

                UserAccessGroups = entity.UserAccessGroups,
            };
        }
    }

    public class UserRequestViewModel : UserViewModel
    {
        [Required]
        [DisplayName("Type Of Request")]
        public int RequestType { get; set; }

        [DisplayName("Name")]
        public int SelectId { get; set; }

        [DisplayName("Request Date")]
        public string RequestDate { get; set; }

        [Required, DisplayName("Requested Date")]
        public string RequestDateStr { get; set; }

        [Required, DisplayName("Requested By")]
        public string RequestUserName { get; set; }

        public bool Download { get; set; } = false;

        public UserRequestViewModel()
        {
            RequestDateStr = DateTime.Now.ToString(Util.GetDateFormat());
        }
    }
}