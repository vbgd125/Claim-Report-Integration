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
    public class IndexService
    {
        private readonly string ConnStrProd;
        public IndexService(IConfiguration config)
        {
            ConnStrProd = config.GetConnectionString("103ConnTest");
        }
        public async Task<int> brpDHR(string userid, DateTime? startvalue, DateTime? endvalue)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @User_id = userid,
                        @startvalue = startvalue,
                        @endvalue = endvalue

                    };
                    var results = connection.QueryFirst<int>("select count (download) as banyak from Header_Hilang_Rusak_CRI where download='1' and User_id=@User_id and create_date between @startvalue and @endvalue", parameter);
                    connection.Close();
                    return results;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<int> brpUHR(string userid, DateTime? startvalue, DateTime? endvalue)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @User_id = userid,
                        @startvalue = startvalue,
                        @endvalue = endvalue
                    };
                    var results = connection.QueryFirst<int>("select count (UpdateHR) as banyak from Header_Hilang_Rusak_CRI where UpdateHR='Sudah' and User_id=@User_id and create_date between @startvalue and @endvalue ", parameter);
                    connection.Close();
                    return results;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> brphilang(string userid, DateTime? startvalue, DateTime? endvalue)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @User_id = userid,
                        @startvalue = startvalue,
                        @endvalue = endvalue
                    };
                    var results = connection.QueryFirst<int>("select count(hilangrusak) from Header_Hilang_Rusak_CRI where hilangrusak='KEHILANGAN' and User_id=@User_id and create_date between @startvalue and @endvalue", parameter);
                    connection.Close();
                    return results;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> brprusak(string userid, DateTime? startvalue, DateTime? endvalue)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @User_id = userid,
                        @startvalue = startvalue,
                        @endvalue = endvalue
                    };
                    var results = connection.QueryFirst<int>("select count(hilangrusak) from Header_Hilang_Rusak_CRI where hilangrusak='KERUSAKAN' and User_id=@User_id and create_date between @startvalue and @endvalue ", parameter);
                    connection.Close();
                    return results;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> brphr(string userid, DateTime? startvalue, DateTime? endvalue)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @User_id = userid,
                        @startvalue = startvalue,
                        @endvalue = endvalue
                    };
                    var results = connection.QueryFirst<int>("select count(*) from Header_Hilang_Rusak_CRI where User_id=@User_id and create_date between @startvalue and @endvalue", parameter);
                    connection.Close();
                    return results;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<string> GetNama()
        {

            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {

                    connection.Open();
                    var users = connection.Query<string>("SELECT Branch_Name from Cabang_CRI").ToList();
                    connection.Close();
                    return users.ToString();
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Summary> GetjumlahTS(DateTime? startvalue, DateTime? endvalue)
        {
            List<Summary> users = new List<Summary>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @startvalue = startvalue,
                        @endvalue = endvalue,
                    };
                    users = connection.Query<Summary>("select Branch_name, count(No_Id) as banyak from Cabang_CRI as c left join User_CRI as U on U.Org_ID=c.Org_ID left join Header_TSDetail_CRI as h on h.User_id=U.User_ID and h.create_date between @startvalue and @endvalue group by Branch_Name order by banyak ASC ", parameters).ToList();
                    connection.Close();
                    return users;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Summary> GetjumlahHR(DateTime? startvalue, DateTime? endvalue)
        {
            List<Summary> users = new List<Summary>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @startvalue = startvalue,
                        @endvalue = endvalue,
                    };
                    users = connection.Query<Summary>("select Branch_name, count(No_Id) as banyak from Cabang_CRI as c left join User_CRI as U on U.Org_ID=c.Org_ID left join Header_Hilang_Rusak_CRI as h on h.User_id=U.User_ID and h.create_date between @startvalue and @endvalue group by Branch_Name order by banyak ASC ", parameters).ToList();
                    connection.Close();
                    return users;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<int> brpTS(string userid, DateTime? startvalue, DateTime? endvalue)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @User_id = userid,
                        @startvalue = startvalue,
                        @endvalue = endvalue
                    };
                    var results = connection.QueryFirst<int>("select count (*) as banyak from Header_TSDetail_CRI where User_id=@User_id and create_date between @startvalue and @endvalue", parameter);
                    connection.Close();
                    return results;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public async Task<int> brpDTS(string userid, DateTime? startvalue, DateTime? endvalue)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @User_id = userid,
                        @startvalue = startvalue,
                        @endvalue = endvalue
                    };
                    var results = connection.QueryFirst<int>("select count (download) as banyak from Header_TSDetail_CRI where download='1' and User_id=@User_id and create_date between @startvalue and @endvalue", parameter);
                    connection.Close();
                    return results;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> brpUTS(string userid, DateTime? startvalue, DateTime? endvalue)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @User_id = userid,
                        @startvalue = startvalue,
                        @endvalue = endvalue
                    };
                    var results = connection.QueryFirst<int>("select count (UpdateTS) as banyak from Header_TSDetail_CRI where UpdateTS='Sudah' and User_id=@User_id and create_date between @startvalue and @endvalue", parameter);
                    connection.Close();
                    return results;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        
    }
}
