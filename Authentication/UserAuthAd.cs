using System;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.DirectoryServices.Protocols;
using Microsoft.AspNetCore.Authentication;
using Dapper;
using MySql.Data.MySqlClient;

namespace TSJ_CRI.Authentication
{
    public class UserAuthAd
    {
        private string ConnStrProd;
        public UserAuthAd(IConfiguration config)
        {
            ConnStrProd = config.GetConnectionString("68ConnProd");
        }

        public async Task<IEnumerable<UserAccount>> IsAuthenticated(string domainName, string userName, string password)
        {

            try
            {
                LdapConnection connection = new(domainName);
                NetworkCredential credential = new(userName, password);
                connection.Credential = credential;
                connection.Bind();

                try
                {
                    var sql = "SELECT username, cabang, role " +
                            "FROM user_all " +
                            "where username = @username";
                    var conn = new MySqlConnection(ConnStrProd);
                    var parameters = new
                    {
                        username = userName
                    };
                    var user = await conn.QueryAsync<UserAccount>(sql, parameters);
                    return user;
                }
                catch
                {
                    return null;
                }
                

            }
            catch (LdapException lexc)
            {
                String ServerMessage = lexc.ServerErrorMessage;
                return null;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                return null;
            }

        }
    }
}   
