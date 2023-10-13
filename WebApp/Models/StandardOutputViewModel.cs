using BusinessObject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class StandardOutputViewModel
    {
        public int Id { get; set; }

        public int Type { get; set; }

        [DisplayName("Data Type")]
        public int DataType { get; set; }

        [Required, DisplayName("Name")]
        public string Name { get; set; }

        [Required, DisplayName("Code")]
        public string Code { get; set; }

        public StandardOutputViewModel() { }

        public StandardOutputViewModel(StandardOutputBo standardOutputBo)
        {
            Set(standardOutputBo);
        }

        public void Set(StandardOutputBo standardOutputBo)
        {
            if (standardOutputBo != null)
            {
                Id = standardOutputBo.Id;
                Type = standardOutputBo.Type;
                DataType = standardOutputBo.DataType;
                Name = standardOutputBo.Name;
                Code = standardOutputBo.Code;
            }
            else
            {
                Type = StandardOutputBo.TypeCustomField;
                DataType = 0;
                Name = null;
                Code = "";
            }
        }
    }
}