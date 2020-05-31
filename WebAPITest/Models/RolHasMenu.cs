using System;
using System.Collections.Generic;

namespace WebAPITest.Models
{
    public partial class RolHasMenu
    {
        public int IdRolHasMenu { get; set; }
        public int? IdRol { get; set; }
        public int? IdMenu { get; set; }

        public virtual Menu IdMenuNavigation { get; set; }
        public virtual Rol IdRolNavigation { get; set; }
    }
}
