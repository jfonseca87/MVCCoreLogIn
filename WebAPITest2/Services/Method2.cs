using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebAPITest2.Models;
using WebAPITest2.Models.DTOs;
using WebAPITest2.Utils;

namespace WebAPITest2.Services
{
    public class Method2
    {
        private readonly SecurityApplicationContext db;

        public Method2(SecurityApplicationContext _db)
        {
            db = _db;
        }

        public string Authtenticate(string user, string password)
        {
            string formatUser = user.ToLower().Trim();

            var userDB = db.Usuario.Where(x =>
                                          x.NickName.ToLower().Trim().Equals(formatUser) &&
                                          x.Password.Equals(password))
                                   .Include(x => x.IdRolNavigation)
                                   .Select(x => new UserDTO
                                   {
                                       Id = x.IdUsuario,
                                       NomUser = x.NomUsuario,
                                       NomRol = x.IdRolNavigation.NomRol
                                   })
                                   .FirstOrDefault();

            if (userDB == null)
            {
                return "NotAuthorized";
            }

            userDB.FechaGeneracion = DateTime.UtcNow;

            var jsonUser = JsonConvert.SerializeObject(userDB);
            TokenGenerator tokenGenerator = new TokenGenerator();

            string token = tokenGenerator.Encrypt(Global.Key, jsonUser);

            return token;
        }
    }
}
