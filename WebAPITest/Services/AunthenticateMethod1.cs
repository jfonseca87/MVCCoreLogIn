using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Models;
using WebAPITest.Utils;

namespace WebAPITest.Services
{
    public class AunthenticateMethod1
    {
        private readonly SecurityApplicationContext db;

        public AunthenticateMethod1()
        {
        }

        public AunthenticateMethod1(SecurityApplicationContext _db)
        {
            db = _db;
        }

        public string Auth (Usuario usuario)
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

            string strToken = ValidateTokenExistence(user.IdUsuario);

            if (strToken != null)
            {
                return strToken;
            }

            TokenUsuario token = new TokenUsuario
            {
                IdUsuario = user.IdUsuario,
                Token = Guid.NewGuid().ToString(),
                FechaGeneracion = DateTime.Now
            };

            db.TokenUsuario.Add(token);
            db.SaveChanges();

            return token.Token;
        }

        public bool ValidateToken(string strToken)
        {
            TokenBD tokenBD = new TokenBD();

            return tokenBD.ValidateTokenInDb(strToken);
        }

        public void RefreshTokenTime(string strToken)
        {
            TokenBD tokenBD = new TokenBD();

            tokenBD.RefreshTokenInDb(strToken);
        }

        public string GetRol(string strToken)
        {
            TokenBD tokenBD = new TokenBD();
            var result = tokenBD.GetRol(strToken);

            return result;
        }

        private string ValidateTokenExistence(int userId)
        {
            TokenBD tokenBD = new TokenBD();

            return tokenBD.FindToken(userId);
        }
    }
}
