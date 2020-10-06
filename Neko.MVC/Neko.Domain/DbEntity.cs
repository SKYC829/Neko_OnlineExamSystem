using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Neko.Domain
{
    public abstract class DbEntity<TPrimaryKey>
    {
        [Required,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TPrimaryKey Id { get; set; }
    }

    public abstract class DbEntity : DbEntity<int>
    {

    }
}
