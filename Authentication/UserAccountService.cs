using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;

namespace TSJ_CRI.Authentication
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class UserAccountService
    {
        private readonly string ConnStrProd;
        public UserAccountService(IConfiguration config)
        {
            ConnStrProd = config.GetConnectionString("103ConnTest");
        }

        public async Task<(IEnumerable<UserAccount>, string, string)> GetUser(string _username, string _password)
        {
            var _WindowsAuth = WindowsAuth(_username, _password);
            if (_WindowsAuth == null)
            {
                try
                {
                    var sql = "SELECT username_window username, price_level role, '3200' cabang " +
                            "FROM PRICE_USERS " +
                            "where username_window = @username";
                    var conn = new SqlConnection(ConnStrProd);
                    var parameters = new { username = _username };
                    var user = await conn.QueryAsync<UserAccount>(sql, parameters);
                    return (user, null, null);
                }
                catch (Exception ex)
                {
                    return (null, null, ex.Message.ToString());
                }
            }
            else
            {
                return (null, _WindowsAuth.ToString(), null);
            }
        }

        private static string WindowsAuth(string _username, string _password)
        {
            string path = "LDAP://inf-addc-01.enseval.com";
            try
            {
                using System.DirectoryServices.DirectoryEntry entry = new(path, _username, _password);
                using DirectorySearcher searcher = new(entry);
                searcher.Filter = "(samaccountname=" + _username + ")";
                var result = searcher.FindOne();

                //ResultPropertyCollection fields = result.Properties;
                //foreach (String ldapField in fields.PropertyNames)
                //{
                //    foreach (Object myCollection in fields[ldapField])
                //    {
                //        //if (ldapField == "department")
                //        //    role = myCollection.ToString().ToLower();
                //    }
                //}
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}