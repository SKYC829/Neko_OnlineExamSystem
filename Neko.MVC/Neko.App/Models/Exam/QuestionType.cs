using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Neko.App.Models.Exam
{
    public enum QuestionType
    {
        /// <summary>
        /// 单选
        /// </summary>
        [Display(Name = "单选题")]
        Radio,
        /// <summary>
        /// 多选
        /// </summary>
        [Display(Name = "多选题")]
        Multiple,
        ///// <summary>
        ///// 填空
        ///// </summary>
        //[Display(Name = "填空题")]
        //Fill_In
    }
}
