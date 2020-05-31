using System;

namespace WebAPITest.Models.DTOs
{
    public class UserDTO
    {
        public int IdUsuario { get; set; }
        public string NomUsuario { get; set; }
        public string NomRol { get; set; }
        public DateTime FechaGeneracion { get; set; }
    }
}
