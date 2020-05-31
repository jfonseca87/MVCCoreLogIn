using System;
using System.Collections.Generic;

namespace WebAPITest.Models
{
    public partial class Menu
    {
        public Menu()
        {
            RolHasMenu = new HashSet<RolHasMenu>();
        }

        public int IdMenu { get; set; }
        public string NomMenu { get; set; }
        public string Urlmenu { get; set; }

        public virtual ICollection<RolHasMenu> RolHasMenu { get; set; }
    }
}
