using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Domain.Entities
{
    public enum QuestionType
    {
        /// <summary>
        /// 单选
        /// </summary>
        Radio,
        /// <summary>
        /// 多选
        /// </summary>
        Multiple,
        /// <summary>
        /// 填空
        /// </summary>
        Fill_In
    }
}
