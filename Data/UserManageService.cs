using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSJ_CRI.Model;

namespace TSJ_CRI.Data
{

    public class UserManageService
    {

        private readonly string ConnStrProd;
        public UserManageService(IConfiguration config)
        {
            ConnStrProd = config.GetConnectionString("103ConnTest");
        }

        public List<UserManage> GetUser()
        {
            List<UserManage> users = new List<UserManage>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    users = connection.Query<UserManage>("SELECT user_id id ,username, email,email_2,email_3, status, roles, USERS.Org_ID org_id, CABANG.BRANCH_NAME CABANG " +
                                    "FROM User_CRI USERS, CABANG_CRI CABANG " +
                                    "WHERE USERS.ORG_ID = CABANG.ORG_ID  order by id asc").ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Cabang> GetCabang()
        {
            List<Cabang> users = new List<Cabang>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    users = connection.Query<Cabang>("SELECT org_id, branch_name" +
                                    " FROM Cabang_CRI").ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<(int, string)> UpdateUser(UserManage _user)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {
                        id = _user.id,
                        org_id = _user.org_id,
                        status = _user.status,
                        roles = _user.roles,
                        email = _user.email,
                        email_2= _user.email_2,
                        email_3= _user.email_3,
                        username = _user.username

                    };
                    var sql = await connection.ExecuteAsync("update User_CRI" +
                                " set username = @username" +
                                ", email = @email" +
                                ", email_2=@email_2" +
                                ", email_3=@email_3"+
                                ", org_id = @org_id" +
                                ", roles = @roles" +
                                ", status = @status" +
                                " where user_id = @id", parameters);
                    connection.Close();
                    return (sql, "");
                }
                catch (Exception ex)
                {
                    return (0, ex.Message.ToString());
                }
            }
        }

        public async Task<(int, string)> InsertUser(UserNew _user)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {
                        @username = _user.username,
                        @email = _user.email,
                        @email_2=_user.email_2,
                        @email_3= _user.email_3,
                        @roles = _user.roles,
                        @org_id = _user.org_id,
                        @status = '1'
                    };
                    var sql = await connection.ExecuteAsync("insert into User_CRI(username,email,Email_2,Email_3,roles,org_id,status) " +
                            "values(@username," +
                                   "@email, " +
                                   "@email_2," +
                                   "@email_3," +
                                   "@roles, " +
                                   "@org_id, " +
                                   "@status)",parameters);
                    connection.Close();
                    return (sql, "Berhasil");
                }
                catch (Exception ex)
                {
                    return (0, ex.Message.ToString());
                }
            }
        }
    }
}