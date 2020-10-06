using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Domain.Entities
{
    [Flags]
    public enum RoleType
    {
        Admin = 0,
        Teacher = 2,
        Student = 4,
    }
}
