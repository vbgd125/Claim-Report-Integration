using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSJ_CRI.Authentication
{
    public class UserAccountService
    {
        private string ConnStrProd;
        public UserAccountService(IConfiguration config)
        {
            ConnStrProd = config.GetConnectionString("68ConnProd");
        }

        public async Task<IEnumerable<UserAccount>> GetUser(string _username, string _password)
        {
            try
            {
                var sql = "SELECT id, username, password, cabang, role level " +
                        "FROM user_login " +
                        "where username = @username and password = @password";
                var conn = new MySqlConnection(ConnStrProd);
                var parameters = new
                {
                    username = _username,
                    password = _password
                };
                var user = await conn.QueryAsync<UserAccount>(sql, parameters);
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}