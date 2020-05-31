using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPITest2.Models;
using WebAPITest2.Models.DTOs;
using WebAPITest2.Utils;

namespace WebAPITest2.Services
{
    public class Method1
    {
        private readonly SecurityApplicationContext db;

        public Method1(SecurityApplicationContext _db)
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

            string token = JWTTokenGenerator.GenereteJWTToken(userDB, Global.Key);

            return token;
        }

    }
}
