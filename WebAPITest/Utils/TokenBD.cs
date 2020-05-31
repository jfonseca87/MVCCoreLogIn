using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;

namespace WebAPITest.Utils
{
    public class TokenBD
    {
        public bool ValidateTokenInDb(string strToken)
        {
            using var conn = new SqlConnection(Global.ConnectionString);
            using var cmd = new SqlCommand("Select FechaGeneracion from TokenUsuario where Token = @p1", conn);
            cmd.Parameters.Add(new SqlParameter("@p1", strToken));

            conn.Open();
            var result = cmd.ExecuteScalar();

            bool isValid = true;

            if (result == null)
            {
                return isValid = false;
            }

            var timeResult = DateTime.Now - (DateTime)result;

            if (timeResult.TotalMinutes > 15)
            {
                isValid = false;
            }

            return isValid;
        }

        public void RefreshTokenInDb(string strToken)
        {
            using var conn = new SqlConnection(Global.ConnectionString);
            using var cmd = new SqlCommand("update tokenusuario set fechageneracion = @p1 where token = @p2", conn);
            cmd.Parameters.Add(new SqlParameter("@p1", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@p2", strToken));

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public string FindToken(int userId)
        {
            using var conn = new SqlConnection(Global.ConnectionString);
            using var cmd = new SqlCommand("select top 1 token from tokenusuario where idusuario = @p1", conn);
            cmd.Parameters.Add(new SqlParameter("@p1", userId));

            conn.Open();
            var result = cmd.ExecuteScalar();

            return result?.ToString();
        }

        public string GetRol(string strToken)
        {
            using var conn = new SqlConnection(Global.ConnectionString);
            using var cmd = new SqlCommand("select a.nomrol from Rol a inner join usuario b on a.idrol = b.idrol inner join tokenusuario c on c.idusuario = b.idusuario where c.token = @p1", conn);
            cmd.Parameters.Add(new SqlParameter("@p1", strToken));

            conn.Open();
            var result = cmd.ExecuteScalar();

            return result.ToString();
        }
    }
}
