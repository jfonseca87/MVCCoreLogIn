using System;
using System.Collections.Generic;

namespace WebAPITest.Models
{
    public partial class TokenUsuario
    {
        public int IdTokenUsuario { get; set; }
        public string Token { get; set; }
        public DateTime? FechaGeneracion { get; set; }
        public int? IdUsuario { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
