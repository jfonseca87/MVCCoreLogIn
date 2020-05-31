using System;

namespace WebAPITest2.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string NomUser { get; set; }
        public string NomRol { get; set; }
        public string Password { get; set; }
        public DateTime FechaGeneracion { get; set; }
    }
}
