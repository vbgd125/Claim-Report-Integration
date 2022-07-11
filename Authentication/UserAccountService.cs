using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Threading.Tasks;
using TSJ_CRI.Model;

namespace TSJ_CRI.Authentication
{
    public class UserAccountService
    {
        private readonly string ConnStrProd;
        public UserAccountService(IConfiguration config)
        {
            ConnStrProd = config.GetConnectionString("103ConnTest");
        }

        public async Task<(IEnumerable<UserSession>, string)> GetUser(string _username)
        {
            try
            {
                var sql = @"
                SELECT USER_ID Userid
                      ,USERNAME
                      ,EMAIL
                      ,ROLES
                      ,ORG_ID OrgId
                      ,STATUS
                  FROM USER_CRI
                 WHERE USERNAME = @USERNAME
                ";
                var conn = new SqlConnection(ConnStrProd);
                var parameters = new { username = _username };
                var user = await conn.QueryAsync<UserSession>(sql, parameters);
                return (user, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message.ToString());
            }
        }

        public string WindowsAuth(string _username, string _password)
        {
            string path = "inf-addc-01.enseval.com";
            try
            {
                LdapConnection connection = new(path);
                NetworkCredential credential = new(_username, _password);
                connection.Credential = credential;
                connection.Bind();
                return null;
            }
            catch (LdapException lexc)
            {
                String ServerMessage = lexc.ServerErrorMessage;

                if (ServerMessage != null)
                {
                    string errorHex = ServerMessage.Substring(ServerMessage.IndexOf(", data") + 7, 3);
                    var errorCode = Convert.ToInt64(errorHex, 16);
                    var errorDesc = new Win32Exception((int)errorCode).Message;
                    return errorDesc.ToString();
                }
                else
                {
                    return lexc.Message.ToString();
                }
            }
            catch (Exception exc)
            {
                return exc.Message.ToString();
            }

            //try
            //{
            //    using System.DirectoryServices.DirectoryEntry entry = new(path, _username, _password);
            //    using DirectorySearcher searcher = new(entry);
            //    searcher.Filter = "(samaccountname=" + _username + ")";
            //    var result = searcher.FindOne();

            //    //ResultPropertyCollection fields = result.Properties;
            //    //foreach (String ldapField in fields.PropertyNames)
            //    //{
            //    //    foreach (Object myCollection in fields[ldapField])
            //    //    {
            //    //        //if (ldapField == "department")
            //    //        //    role = myCollection.ToString().ToLower();
            //    //    }
            //    //}
            //    return null;
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message.ToString();
            //}
        }
    }
}