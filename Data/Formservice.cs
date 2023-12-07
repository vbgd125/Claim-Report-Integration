using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSJ_CRI.Model;
using Syncfusion.OfficeChart.Implementation;
using TSJ_CRI.Pages;

namespace TSJ_CRI.Data
{
    public class Formservice
    {
        private readonly string ConnStrProd;
        public Formservice(IConfiguration config)
        {
            ConnStrProd = config.GetConnectionString("103ConnTest");
        }

        public List<UserForm> GetUser(DateTime? startvalue, DateTime? endvalue)
        {
            List<UserForm> users = new List<UserForm>();
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
                    users = connection.Query<UserForm>("SELECT no_id,hilangrusak,namaekspedisi ,nokendaraan, namasupir, nopengiriman,pengirimandari,pengirimanke,nodokument,koli ,convert(date,tglkirim) as tglkirim ,User_id,id_temp,no_pengajuan,approve,FA " +
                                    "FROM Header_Hilang_Rusak_CRI WHERE create_date between @startvalue and @endvalue",parameter).ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<UserForm> GetUser2(string userid, string roles)
        {
            List<UserForm> users = new List<UserForm>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @User_id=userid,
                        @roles=roles
                    };
                    var query = @"
                        SELECT a.no_id,a.hilangrusak,a.namaekspedisi,a.nokendaraan, a.namasupir, a.nopengiriman,a.pengirimandari,a.pengirimanke,a.nodokument,a.koli,convert(date,tglkirim) as tglkirim ,a.User_id,a.id_temp,a.no_pengajuan,a.download,a.UpdateHR ,c.Branch_Name,a.approve,a.FA
                        FROM Cabang_CRI as c
                        inner join User_CRI as u on u.Org_ID=c.Org_ID
                        inner join Header_Hilang_Rusak_CRI as a on a.User_id=u.User_ID
                         WHERE create_date >= cast(dateadd(month, -2, getdate()) as date) and (a.approve='Approved by Logistik' and a.FA='Approved by FA')
						 ";
                    if (roles == "2")
                    {
                        query += $" and a.User_id =@User_id";
                    }
                    query += @" union all
						   SELECT a.no_id,a.hilangrusak,a.namaekspedisi,a.nokendaraan, a.namasupir, a.nopengiriman,a.pengirimandari,a.pengirimanke,a.nodokument,a.koli,convert(date,tglkirim) as tglkirim ,a.User_id,a.id_temp,a.no_pengajuan,a.download,a.UpdateHR ,c.Branch_Name,a.approve,a.FA
                        FROM Cabang_CRI as c
                        inner join User_CRI as u on u.Org_ID=c.Org_ID
                        inner join Header_Hilang_Rusak_CRI as a on a.User_id=u.User_ID
                         WHERE (a.approve !='Approved by Logistik' or a.FA !='Approved by FA')";
                    if (roles == "2")
                    {
                        query += $" and a.User_id =@User_id";
                    }
                    users = connection.Query<UserForm>(query, parameters).ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<UserForm> GetNo(string seq)
        {
            List<UserForm> users = new List<UserForm>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    var parameters = new
                    {
                        @id_temp = seq
                    };
                    connection.Open();
                    users = connection.Query<UserForm>("SELECT no_pengajuan " +
                                    "FROM Header_Hilang_Rusak_CRI WHERE id_temp= @id_temp", parameters).ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<UserForm> GetUser3(string id_temp)
        {
            List<UserForm> users = new List<UserForm>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @id_temp = id_temp
                    };
                    users = connection.Query<UserForm>("SELECT no_id,hilangrusak,namaekspedisi ,nokendaraan, namasupir, nopengiriman,pengirimandari,pengirimanke,nodokument,koli ,convert(date,tglkirim) as tglkirim ,User_id,id_temp,no_pengajuan,approve,FA " +
                                     "FROM Header_Hilang_Rusak_CRI WHERE id_temp= @id_temp", parameters).ToList();
                    connection.Close();
                    return users;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public async Task<string> Getapprove(string id_temp)
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

                    var users = connection.Query<UserForm>("SELECT approve " +
                                     "FROM Header_Hilang_Rusak_CRI WHERE id_temp= @id_temp", parameter).ToList();
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

                    var users = connection.Query<UserForm>("SELECT FA " +
                                     "FROM Header_Hilang_Rusak_CRI WHERE id_temp= @id_temp", parameter).ToList();
                    connection.Close();
                    return users.FirstOrDefault().FA;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<UserForm> Updateapprove(string id_temp)
        {
            List<UserForm> users = new List<UserForm>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @id_temp = id_temp
                    };
                    users = connection.Query<UserForm>("update Header_Hilang_Rusak_CRI set approve ='Approved by Logistik' where id_temp=@id_temp", parameters).ToList();
                    connection.Close();
                    return users;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<UserForm> UpdateapproveFA(string id_temp)
        {
            List<UserForm> users = new List<UserForm>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @id_temp = id_temp
                    };
                    users = connection.Query<UserForm>("update Header_Hilang_Rusak_CRI set FA ='Approved by FA' where id_temp=@id_temp", parameters).ToList();
                    connection.Close();
                    return users;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }
        //public async Task<(int, string)> InsertUser(UserFormbaru _userC, string userid)
        //{
        //    using (var connection = new SqlConnection(ConnStrProd))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            var parameters = new
        //            {
        //                @namaekspedisi = _userC.namaekspedisi,
        //                @emailekspedisi = _userC.emailekspedisi,
        //                @nokendaraan = _userC.nokendaraan,
        //                @namasupir = _userC.namasupir,
        //                @nopengiriman = _userC.nopengiriman,
        //                @pengirimandari = _userC.pengirimandari,
        //                @pengirimanke = _userC.pengirimanke,
        //                @nodokument= _userC.nodokument,
        //                @koli=_userC.koli,
        //                @tglrusak = _userC.tglrusak,
        //                @tglkirim = _userC.tglkirim,
        //                @User_id = userid

        //            };
        //            var sql = await connection.ExecuteAsync("insert into Header_Hilang_Rusak_CRI(namaekspedisi ,emailekspedisi ,nokendaraan ,namasupir ,nopengiriman ,pengirimandari ,pengirimanke ,nodokument, koli,tglrusak , tglkirim, User_Id) " +
        //                    "values(@namaekspedisi," +
        //                           "@emailekspedisi, " +
        //                           "@nokendaraan, " +
        //                           "@namasupir, " +
        //                           "@nopengiriman, " +
        //                           "@pengirimandari, " +
        //                           "@pengirimanke, " +
        //                           "@nodokument, " +
        //                           "@koli, " +
        //                           "convert(date,@tglrusak) , " +
        //                           "@tglkirim, " +
        //                           "@User_id) ", parameters);

        //            connection.Close();
        //            return (sql, "Berhasil");
        //        }
        //        catch (Exception)
        //        {
        //            return (0, null);

        //        }
        //    }
        //}

        public async Task<(int, string)> InsertForm2(UserFormbaru _userC, string userid,string seq,string cabang)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {
                        @hilangrusak = _userC.hilangrusak,
                        @namaekspedisi = _userC.namaekspedisi,

                        @nokendaraan = _userC.nokendaraan,
                        @namasupir = _userC.namasupir,
                        @nopengiriman = _userC.nopengiriman,
                        @pengirimandari = _userC.pengirimandari,
                        @pengirimanke = _userC.pengirimanke,
                        @nodokument = _userC.nodokument,
                        @koli = _userC.koli,
                        
                        @tglkirim = _userC.tglkirim,
                        @User_id = userid,
                        @id_temp=seq

                    };
                    var sql = await connection.ExecuteAsync("insert into Header_Hilang_Rusak_CRI(hilangrusak,namaekspedisi  ,nokendaraan ,namasupir ,nopengiriman ,pengirimandari ,pengirimanke ,nodokument, koli, tglkirim, User_Id,id_temp,create_date,download,approve,FA) " +
                            "values(@hilangrusak,"  +
                                   "@namaekspedisi," +
                                   "@nokendaraan, " +
                                   "@namasupir, " +
                                   "@nopengiriman, " +
                                   "@pengirimandari, " +
                                   "@pengirimanke, " +
                                   "@nodokument, " +
                                   "@koli, " +
                                   
                                   "convert(date,@tglkirim), " +
                                   "@User_id, " +
                                   "@id_temp, " +
                                   "getdate()," +
                                   "'0', " +
                                   "'Belum di Approve'," +
                                   "'Belum di Approve')", parameters);

                    connection.Close();
                   
                }
                catch (Exception ex)
                {
                    return (0, ex.Message.ToString());
                }
                try
                {
                    connection.Open();
                    var parameters = new
                    {
                       
                        @id_temp=seq,
                        @org_id=cabang
                    };
                    var query = "update Header_Hilang_Rusak_CRI " +
                        "set no_pengajuan=(select CONCAT((select namaseq from seqHR_temp ),(select no_id from Header_Hilang_Rusak_CRI where id_temp=@id_temp),'-',(select Acronim from Cabang_CRI where Org_ID=@org_id),'-',FORMAT (getdate(),'MM') ,'-',FORMAT (getdate(),'yyyy') ))where id_temp=@id_temp ";
                    var sql = await connection.ExecuteAsync(query, parameters);

                    connection.Close();
                    return (sql, "Berhasil");
                }
                catch (Exception ex)
                {
                    return (0, ex.Message.ToString());
                }


                //try
                //{
                //    connection.Open();
                //    var parameters = new
                //    {
                //        @User_id = userid
                //    };
                //    var sql = await connection.ExecuteAsync("Delete from Header_Hilang_RusakTEMP_CRI where User_id = @User_id ", parameters);

                //    connection.Close();
                //    return (sql, "Berhasil");
                //}
                //catch (Exception ex)
                //{
                //    return (0, ex.Message.ToString());
                //}
            }
        }

        
        public async Task <string> count ()
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                   var sql = connection.Query<seq>("SELECT concat(namaseq ,no_seq_temp) as namaseq " +
                                    "FROM seqHR_temp ").ToList();
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



                    cabangk = connection.Query<Cabang>("SELECT Branch_Name " +
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
        public List<Cabang> GetAlamat()
        {
            List<Cabang> users = new List<Cabang>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    users = connection.Query<Cabang>("SELECT concat(Branch_Name,' - ',Address1) as alamat ,Address1 " +
                                    "FROM Cabang_CRI  ").ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
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

        public async Task<int> updatecount()
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();

                    var sql = connection.Query<seq>("SELECT no_seq_temp " +
                                    "FROM seqHR_temp "+
                                    "update seqHR_temp " +
                                     "set no_seq_temp = no_seq_temp + 1 ").ToList();
                    connection.Close();
                    return sql.LastOrDefault().no_seq;
                }
            }
            catch (Exception)
            {
                return 0;
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

                    var sql = await connection.ExecuteAsync( "update Header_Hilang_Rusak_CRI " +
                                     "set download = '1' " +
                                     "where id_temp = @id_temp",parameter);
                    connection.Close();
                    return sql.ToString();
                }
            }
            catch (Exception )
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
                        "from Header_Hilang_Rusak_CRI where id_temp=@id_temp", parameter).ToList();
                    connection.Close();
                    return sql.FirstOrDefault().download;
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
                        "from Header_Hilang_Rusak_CRI where id_temp=@id_temp", parameter).ToList();
                    connection.Close();
                    return sql.FirstOrDefault().FILENAME;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
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
        //            var check = connection.Query<UserForm2>("SELECT  no_id,namaekspedisi,emailekspedisi ,nokendaraan, namasupir, nopengiriman,pengirimandari,pengirimanke,nodokument,koli,tglrusak,tglkirim " +
        //                            "FROM Header_Hilang_RusakTEMP_CRI WHERE user_id = @user_id", parameters).ToList();
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


        public async Task<(int, string)> UpdateUser(UserForm _userF)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {
                        no_id = _userF.no_id,
                        namaekspedisi = _userF.namaekspedisi,
                        namasupir = _userF.namasupir,
                        nopengiriman = _userF.nopengiriman,
                        pengirimandari = _userF.pengirimandari,
                        pengirimanke = _userF.pengirimanke,
                        nodokument= _userF.nodokument,
                        koli= _userF.koli,
                        
                        tglkirim= _userF.tglkirim,




                    };
                    var sql = await connection.ExecuteAsync("Update Header_Hilang_Rusak_CRI" +
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

                    var sql = connection.Query<UserForm>("select UpdateHR " +
                        "from Header_Hilang_Rusak_CRI where id_temp=@id_temp", parameter).ToList();
                    connection.Close();
                    return sql.FirstOrDefault().UpdateHR;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> getroles(string userid)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameter = new
                    {
                        @User_id=userid
                    };

                    var sql = connection.Query<Roleses>("select Roles " +
                        "from User_CRI where User_ID=@User_id", parameter).ToList();
                    connection.Close();
                    return sql.FirstOrDefault().Roles;
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

                    var sql = connection.Query<UserForm>("select namaekspedisi " +
                        "from Header_Hilang_Rusak_CRI where id_temp=@id_temp", parameter).ToList();
                    connection.Close();
                    return sql.FirstOrDefault().namaekspedisi;
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
                    var sql = connection.Query<string>("select concat(Email,',',Email_2,',',Email_3) from Ekspedition_CRI where NAMA_EKSPEDISI=@ekspedisi",paramater).ToList();
                    connection.Close();
                    return sql.FirstOrDefault();
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
                        "from Header_Hilang_Rusak_CRI where id_temp=@id_temp", parameter).ToList();
                    connection.Close();
                    return sql.FirstOrDefault().no_pengajuan;
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
                        " inner join Detail_HRDetail_CRI as h on c.Kodeprod=h.Kode" +
                        " where id_temp=@id_temp", parameter).ToList();
                    connection.Close();
                    return sql.FirstOrDefault().HNA ;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}
