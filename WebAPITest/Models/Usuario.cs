using System;
using System.Collections.Generic;

namespace WebAPITest.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            TokenUsuario = new HashSet<TokenUsuario>();
        }

        public int IdUsuario { get; set; }
        public string NomUsuario { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public int? IdRol { get; set; }

        public virtual Rol IdRolNavigation { get; set; }
        public virtual ICollection<TokenUsuario> TokenUsuario { get; set; }
    }
}
