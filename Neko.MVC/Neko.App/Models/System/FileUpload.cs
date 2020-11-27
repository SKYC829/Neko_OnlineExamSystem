using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Neko.App.Models.System
{
    public class FileUpload
    {
        [Required]
        [Display(Name = "附件")]
        //[FileExtensions(Extensions = ".xls", ErrorMessage ="目前只支持Excel文件(.xls)哦")]
        public IFormFile UploadFile { get; set; }
    }
}
