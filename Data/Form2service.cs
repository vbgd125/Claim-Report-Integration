using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSJ_CRI.Model;
using TSJ_CRI.Pages;
using Google.Protobuf.WellKnownTypes;

namespace TSJ_CRI.Data
{
    public class Form2service
    {
        private readonly string ConnStrProd;
        public Form2service(IConfiguration config)
        {
            ConnStrProd = config.GetConnectionString("103ConnTest");
        }

        public List<UserForm2> GetUser(DateTime? startvalue, DateTime? endvalue)
        {
            List<UserForm2> users = new List<UserForm2>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {

                        @startvalue = startvalue,
                        @endvalue = endvalue

                    };
                    users = connection.Query<UserForm2>("SELECT No_id, convert(date, getdate(), 3) as Tanggal  , PT, Ketidaksesuaian,Notes,no_pengajuan,create_date,id_temp,approve,FA " +
                                    "FROM Header_TSDetail_CRI WHERE create_date between @startvalue and @endvalue",parameter).ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<UserForm2> Getapprovehistory()
        {
            List<UserForm2> users = new List<UserForm2>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    
                    users = connection.Query<UserForm2>("SELECT approve,FA " +
                                    "FROM Header_TSDetail_CRI ").ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<UserForm2> GetUser1(string userid, string roles)
        {
            List<UserForm2> users = new List<UserForm2>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @User_id = userid,
                        @roles = roles,
                        
                    };
                    var query = @"  SELECT a.No_id, convert(date, Tanggal) as Tanggal , a.PT, a.Ketidaksesuaian,a.Notes,a.User_id,a.UpdateTS,a.id_temp,a.no_pengajuan, c.Branch_Name,a.approve,a.FA 
                        FROM Cabang_CRI as c   
                        inner join User_CRI as u on u.Org_ID=c.Org_ID   
                        inner join Header_TSDetail_CRI as a on a.User_id=u.User_ID 
						where create_date >= cast(dateadd(month, -2, getdate()) as date) and (a.approve='Approved by Logistik' and a.FA='Approved by FA')
						";
                    if (roles == "2")
                    {
                        query += $" and a.User_id =@User_id";
                    }
                    query += @" union all
						  SELECT a.No_id, convert(date, Tanggal) as Tanggal , a.PT, a.Ketidaksesuaian,a.Notes,a.User_id,a.UpdateTS,a.id_temp,a.no_pengajuan, c.Branch_Name,a.approve,a.FA 
                        FROM Cabang_CRI as c   
                        inner join User_CRI as u on u.Org_ID=c.Org_ID   
                        inner join Header_TSDetail_CRI as a on a.User_id=u.User_ID 
						where (a.approve !='Approved by Logistik' or a.FA !='Approved by FA')";
                    if (roles == "2")
                    {
                        query += $" and a.User_id =@User_id";
                    }
                    users = connection.Query<UserForm2>(query, parameters).ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public List<UserForm2> GetUser2(string id_temp)
        {
            List<UserForm2> users = new List<UserForm2>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @id_temp = id_temp
                    };
                    users = connection.Query<UserForm2>("SELECT No_id, convert(date, getdate(), 3) as Tanggal  , PT, Ketidaksesuaian,Notes,no_pengajuan,create_date " +
                                    "FROM Header_TSDetail_CRI WHERE id_temp= @id_temp",parameters).ToList();
                    connection.Close();
                    return users;
                }
                
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<UserForm2> GetNo(string seq)
        {
            List<UserForm2> users = new List<UserForm2>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    var parameters = new
                    {
                        @id_temp = seq
                    };
                    connection.Open();
                    users = connection.Query<UserForm2>("SELECT no_pengajuan " +
                                    "FROM Header_TSDetail_CRI WHERE id_temp= @id_temp", parameters).ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }
       
        public async Task<string> Getapprove(string seq)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @id_temp = seq
                    };

                    var users = connection.Query<UserForm2>("SELECT approve " +
                                     "FROM Header_TSDetail_CRI WHERE id_temp= @id_temp", parameter).ToList();
                    connection.Close();
                    return users.FirstOrDefault().Approve;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> GetFA(string id_temp)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @id_temp = id_temp
                    };

                    var users = connection.Query<UserForm2>("SELECT FA " +
                                     "FROM Header_TSDetail_CRI WHERE id_temp= @id_temp", parameter).ToList();
                    connection.Close();
                    return users.FirstOrDefault().FA;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<UserForm2> Updateapprove(string id_temp)
        {
            List<UserForm2> users = new List<UserForm2>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @id_temp = id_temp
                    };
                    users = connection.Query<UserForm2>("update Header_TSDetail_CRI set approve ='Approved by Logistik' where id_temp=@id_temp", parameters).ToList();
                    connection.Close();
                    return users;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<UserForm2> UpdateapproveFA(string id_temp)
        {
            List<UserForm2> users = new List<UserForm2>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @id_temp = id_temp
                    };
                    users = connection.Query<UserForm2>("update Header_TSDetail_CRI set FA ='Approved by FA' where id_temp=@id_temp", parameters).ToList();
                    connection.Close();
                    return users;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<(int, string)> UpdateUser(UserForm2 _userK)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {
                        No_id = _userK.No_id,
                        Tanggal = _userK.Tanggal,
                        
                        PT = _userK.PT,
                        Ketidaksesuaian = _userK.Ketidaksesuaian,
                        Notes = _userK.Notes


                    };
                    var sql = await connection.ExecuteAsync("Update HeaderTSDetail_CRI" +
                                " set Tanggal = @Tanggal" +
                                
                                ", PT = @PT" +
                                ", Ketidaksesuaian = @Ketidaksesuaian" +
                                ", Notes = @Notes" +
                                " where No_id = @No_id", parameters);
                    connection.Close();
                    return (sql, "");
                }
                catch (Exception ex)
                {
                    return (0, ex.Message.ToString());
                }
            }
        }

        public async Task<(int, string)> InsertUser(UserFormbaru2 _userF, string userid, string seq,string cabang)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {
                        @Tanggal = _userF.Tanggal,
                        @PT = _userF.PT,
                        
                        @ketidaksesuaian = _userF.Ketidaksesuaian,
                        @Notes = _userF.Notes,
                        @User_id = userid,
                        @id_temp=seq
                    };
                    var sql = await connection.ExecuteAsync("insert into Header_TSDetail_CRI(Tanggal,PT,Ketidaksesuaian,Notes , User_id,id_temp,create_date,UpdateTS,approve,FA) " +
                            "values(@Tanggal," +
                                   
                                   "@PT, " +
                                   "@Ketidaksesuaian, " +
                                   "@Notes, " +
                                   "@User_id, " +
                                   "@id_temp," +
                                   "getdate()," +
                                   "'BELUM'," +
                                   "'Belum di Approve'," +
                                   "'Belum di Approve' )", parameters);

                    connection.Close();
                    
                }
                catch (Exception)
                {
                    return (0, null);

                }
                try
                {
                    connection.Open();
                    var parameters = new
                    {

                        @id_temp = seq,
                        @org_id=cabang
                    };
                    var query = "update Header_TSDetail_CRI " +
                        "set no_pengajuan=(select CONCAT((select namaseq from seqTS_temp ),(select no_id from Header_TSDetail_CRI where id_temp=@id_temp),'-',(select Acronim from Cabang_CRI where Org_ID=@org_id),'-',FORMAT (getdate(),'MM') ,'-',FORMAT (getdate(),'yyyy') ))where id_temp=@id_temp ";
                    var sql = await connection.ExecuteAsync(query, parameters);

                    connection.Close();
                    return (sql, "Berhasil");
                }
                catch (Exception ex)
                {
                    return (0, ex.Message.ToString());
                }
            }
        }
        //public async Task<(int, string)> InsertForm2(string userid)
        //{
        //    using (var connection = new SqlConnection(ConnStrProd))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            var parameters = new
        //            {
        //                @User_id = userid
        //            };
        //            var sql = await connection.ExecuteAsync("insert into HeaderTSDetail_CRI(No_id,Tanggal,Nodok,PT,Ketidaksesuaian,Notes,User_id)" +
        //                    "SELECT [No_id]" +
        //                    ",[Tanggal] " +
        //                    ",[Nodok] " +
        //                    ",[PT] " +
        //                    ",[Ketidaksesuaian] " +
        //                    ",[Notes] " +
        //                    ",[User_id] " +
        //                    "FROM HeaderTSTEMP_CRI where User_id = @User_id", parameters);

        //            connection.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            return (0, ex.Message.ToString());
        //        }
        //        try
        //        {
        //            connection.Open();
        //            var parameters = new
        //            {
        //                @User_id = userid
        //            };
        //            var sql = await connection.ExecuteAsync("Delete from HeaderTSTEMP_CRI where User_id = @User_id ", parameters);

        //            connection.Close();
        //            return (sql, "Berhasil");
        //        }
        //        catch (Exception ex)
        //        {
        //            return (0, ex.Message.ToString());
        //        }
        //    }
        //}

        //public async Task<string> checktemp(string userid)
        //{

        //    try
        //    {
        //        using (var connection = new SqlConnection(ConnStrProd))
        //        {
        //            connection.Open();
        //            var parameters = new
        //            {
        //                @User_id = userid
        //            };
        //            var check = connection.Query<UserForm2>("SELECT No_id,Tanggal ,Nodok, PT, Ketidaksesuaian,Notes " +
        //                            "FROM HeaderTSTEMP_CRI WHERE user_id = @user_id", parameters).ToList();
        //            connection.Close();
        //            if (check.Count > 0)
        //            {
        //                return "0";
        //            }
        //            else
        //            {
        //                return "1";
        //            }
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        //public async Task<string> seq(seq _seq)
        //{
        //    using (var connection = new SqlConnection(ConnStrProd))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            var parameters = new
        //            {
        //                @no_seq = _seq
        //            };
        //            var sq = await connection.ExecuteAsync("insert into seq (no_seq)" +
        //                     "values (@no_seq)", parameters);

        //            connection.Close();

        //            if (sq.Count > 0)
        //            {
        //                return "0";
        //            }
        //            else
        //            {
        //                return "1";
        //            }
        //        }


        //        catch (Exception)
        //        {
        //            return null;
        //        }
        //    }


        public async Task<string> count()
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var sql = connection.Query<seq>("SELECT concat(namaseq ,no_seq) as namaseq " +
                                     "FROM seqTS_temp ").ToList();
                    connection.Close();
                    return sql.FirstOrDefault().namaseq.ToString();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> cabang(string org_id)
        {
            List<Cabang> cabangk = new List<Cabang>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @Org_ID = org_id
                    };
                        
                     
                    
                    cabangk  = connection.Query<Cabang>("SELECT Branch_Name " +
                                     "FROM Cabang_CRI where Org_ID = @Org_ID ", parameter).ToList();
                    connection.Close();
                    
                }
                return cabangk.FirstOrDefault().branch_name;
            }
            catch (Exception)
            {
                return null;           
            }
        }
        public async Task<int> updatecount()
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();

                    var sql = connection.Query<seq>("SELECT no_seq " +
                                    "FROM seqTS_temp " +
                                    "update seqTS_temp " +
                                     "set no_seq = no_seq + 1 ").ToList();
                    connection.Close();
                    return sql.FirstOrDefault().no_seq;
                }
            }
            catch (Exception)
            {
                return 0 ;
            }
        }
        public async Task<string> acronim(string org_id)
        {
            List<Cabang> cabangk = new List<Cabang>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @Org_ID = org_id
                    };



                    cabangk = connection.Query<Cabang>("SELECT Acronim " +
                                     "FROM Cabang_CRI where Org_ID = @Org_ID ", parameter).ToList();
                    connection.Close();

                }
                return cabangk.FirstOrDefault().Acronim;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> updownload(string id_temp)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @id_temp = id_temp
                    };

                    var sql = await connection.ExecuteAsync("update Header_TSDetail_CRI " +
                                     "set download = '1' " +
                                     "where id_temp = @id_temp", parameter);
                    connection.Close();
                    return sql.ToString();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        
        
        public async Task<string> getdownload(string id_temp)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @id_temp = id_temp
                    };

                    var sql = connection.Query<UserForm>("select download " +
                        "from Header_TSDetail_CRI where id_temp=@id_temp", parameter).ToList();
                    connection.Close();
                    return sql.FirstOrDefault().download;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> getupdate(string id_temp)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @id_temp = id_temp
                    };

                    var sql = connection.Query<UserForm2>("select UpdateTS " +
                        "from Header_TSDetail_CRI where id_temp=@id_temp", parameter).ToList();
                    connection.Close();
                    return sql.FirstOrDefault().UpdateTS;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> getfile(string id_temp)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @id_temp = id_temp
                    };

                    var sql = connection.Query<UserForm>("select FILENAME " +
                        "from Header_TSDetail_CRI where id_temp=@id_temp", parameter).ToList();
                    connection.Close();
                    return sql.FirstOrDefault().FILENAME;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> getpengajuan(string id_temp)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @id_temp = id_temp
                    };

                    var sql = connection.Query<UserForm>("select no_pengajuan " +
                        "from Header_TSDetail_CRI where id_temp=@id_temp", parameter).ToList();
                    connection.Close();
                    return sql.FirstOrDefault().no_pengajuan;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> getemail(string userid)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @User_id = userid
                    };

                    var sql = connection.Query<string>("select concat(Email,',',Email_2,',',Email_3) " +
                        "from User_CRI where User_ID=@User_id", parameter).ToList();
                    connection.Close();
                    return sql.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<UserManage>> getemailpusat()
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();


                    var sql = connection.Query<UserManage>("select concat(Email,',',Email_2,',',Email_3) as email " +
                        "from User_CRI where Roles ='3' and Status ='1'").ToList();
                    connection.Close();
                    return sql;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<UserManage>> getemailFA()
        {

            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();


                    var sql = connection.Query<UserManage>("select concat(Email,',',Email_2,',',Email_3) as email " +
                        "from User_CRI where Roles ='5' and Status ='1'").ToList();
                    connection.Close();
                    return sql;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<UserManage>> getemailLogistik()
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();

                    var sql = connection.Query<UserManage>("select concat(Email,',',Email_2,',',Email_3) as email " +
                        "from User_CRI where Roles ='4' and Status='1'").ToList();
                    connection.Close();
                    return sql;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<int> gethna(string id_temp)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @id_temp = id_temp
                    };

                    var sql = connection.Query<UserForm>("select hna " +
                        " from Master_item as c" +
                        " inner join DetailTSDetail_CRI as h on c.Namaprod=h.Nama" +
                        " where id_temp=@id_temp", parameter).ToList();
                    connection.Close();
                    return sql.FirstOrDefault().HNA;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<string> getekspedisi(string id_temp)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @id_temp = id_temp
                    };

                    var sql = connection.Query<UserForm2>("select PT " +
                        "from Header_TSDetail_CRI where id_temp=@id_temp", parameter).ToList();
                    connection.Close();
                    return sql.FirstOrDefault().PT;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> getemailekspedisi(string ekspedisi)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var paramater = new
                    {
                        @ekspedisi = ekspedisi
                    };
                    var sql = connection.Query<string>("select concat(Email,',',Email_2,',',Email_3) from Ekspedition_CRI where NAMA_EKSPEDISI=@ekspedisi", paramater).ToList();
                    connection.Close();
                    return sql.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}