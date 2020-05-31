using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebAPITest.Models;
using WebAPITest.Models.DTOs;
using WebAPITest.Utils;

namespace WebAPITest.Services
{
    public class AuthenticateMethod2
    {
        private readonly SecurityApplicationContext db;
        private const string key = "this_is_a_key_of_32_characters.."; //Must be 32 characters lenght minimun 

        public AuthenticateMethod2()
        {
        }

        public AuthenticateMethod2(SecurityApplicationContext _db)
        {
            db = _db;
        }

        public string Auth(Usuario usuario)
        {
            string nomUser = usuario.NickName.ToLower().Trim();

            var user = db.Usuario.Where(x =>
                                        x.NickName.ToLower().Trim().Equals(nomUser) &&
                                        x.Password.Equals(usuario.Password))
                                 .Include(x => x.IdRolNavigation)
                                 .FirstOrDefault();

            if (user == null)
            {
                return "NotAuthorized";
            }

            string token = GenerateToken(user);

            return token;
        }

        private string GenerateToken(Usuario usuario)
        {
            var userData = new UserDTO
            {
                IdUsuario = usuario.IdUsuario,
                NomUsuario = usuario.NomUsuario,
                NomRol = usuario.IdRolNavigation.NomRol,
                FechaGeneracion = DateTime.Now
            };

            var jsonObject = JsonConvert.SerializeObject(userData);

            TokenGenerator tokenGenerator = new TokenGenerator();

            var token = tokenGenerator.Encrypt(key, jsonObject);

            return token;
        }

        public bool ValidateToken(string strToken)
        {
            bool isValid = true;
            TokenGenerator tokenGenerator = new TokenGenerator();
            string decryptToken = tokenGenerator.Decrypt(key, strToken);

            var objToken = JsonConvert.DeserializeObject<UserDTO>(decryptToken);

            TimeSpan timeElapsed = DateTime.Now - objToken.FechaGeneracion;

            if (timeElapsed.TotalMinutes > 15)
            {
                isValid = false;
            }

            return isValid;
        }

        public string GetRol(string strToken)
        {
            TokenGenerator tokenGenerator = new TokenGenerator();
            string decryptToken = tokenGenerator.Decrypt(key, strToken);
            var objToken = JsonConvert.DeserializeObject<UserDTO>(decryptToken);

            return objToken.NomRol;
        }

        public string RefreshToken(string strToken)
        {
            TokenGenerator tokenGenerator = new TokenGenerator();
            string decryptToken = tokenGenerator.Decrypt(key, strToken);

            var objToken = JsonConvert.DeserializeObject<UserDTO>(decryptToken);
            objToken.FechaGeneracion = DateTime.Now;

            var jsonObject = JsonConvert.SerializeObject(objToken);

            string newToken = tokenGenerator.Encrypt(key, jsonObject);

            return newToken;
        }
    }
}
