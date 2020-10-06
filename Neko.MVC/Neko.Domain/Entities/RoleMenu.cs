using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Neko.Domain.Entities
{
    [Table("Tb_RoleMenu")]
    public class RoleMenu:DbEntity
    {
        public int RoleId { get; set; }

        public int MenuId { get; set; }

        public virtual RoleInfo Role { get; set; }

        public virtual MenuInfo Menu { get; set; }
    }
}
