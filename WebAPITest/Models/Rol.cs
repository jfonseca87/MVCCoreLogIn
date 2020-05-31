using System;
using System.Collections.Generic;

namespace WebAPITest.Models
{
    public partial class Rol
    {
        public Rol()
        {
            RolHasMenu = new HashSet<RolHasMenu>();
            Usuario = new HashSet<Usuario>();
        }

        public int IdRol { get; set; }
        public string NomRol { get; set; }

        public virtual ICollection<RolHasMenu> RolHasMenu { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
