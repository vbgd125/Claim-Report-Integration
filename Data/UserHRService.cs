using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSJ_CRI.Model;
using TSJ_CRI.Pages;

namespace TSJ_CRI.Data
{
    public class UserHRService
    {
        private readonly string ConnStrProd;
        public UserHRService(IConfiguration config)
        {
            ConnStrProd = config.GetConnectionString("103ConnTest");
        }

        public List<UserFormHR> GetUser(string userid)
        {
            List<UserFormHR> users = new List<UserFormHR>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @userid=userid


                    };
                    users = connection.Query<UserFormHR>("SELECT No_id,Kode,Namaproduk, Batch, Expiredate, Jumlah, Ket " +
                                    "FROM Detail_HRTEMP_CRI WHERE No_id != 81 and User_id=@userid order by No_id asc ",parameters).ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<UserFormHR> GetUsertest()
        {
            List<UserFormHR> users = new List<UserFormHR>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    users = connection.Query<UserFormHR>("SELECT No_id,Kode,Namaproduk, Batch, Expiredate, Jumlah, Ket,id_temp " +
                                    "FROM Detail_HRDetail_CRI WHERE No_id != 81 order by No_id asc").ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<UserFormHR> GetUser2(string id_temp)
        {
            List<UserFormHR> users = new List<UserFormHR>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @id_temp = id_temp,
                      

                    };
                    users = connection.Query<UserFormHR>("SELECT a.No_id,a.Kode,a.Namaproduk, a.Batch, a.Expiredate, a.Jumlah, a.Ket,a.User_id,c.HNA " +
                                    "FROM Detail_HRDetail_CRI a inner join Header_Hilang_Rusak_CRI b on a.id_temp = @id_temp and a.id_temp= b.id_temp " +
                                    "inner join Master_Item c on a.Kode=c.Kodeprod", parameters ).ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Ekspedisi> GetEkspedisi()
        {
            List<Ekspedisi> users = new List<Ekspedisi>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    users = connection.Query<Ekspedisi>("SELECT NAMA_EKSPEDISI " +
                                    "FROM Ekspedition_CRI WHERE STATUS = 'Y' ").ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Produk> Getkodeproduk()
        {
            List<Produk> users = new List<Produk>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    users = connection.Query<Produk>("SELECT concat(Kodeprod,'-',Namaprod) as pengenal, Kodeprod,Namaprod,HNA " +
                                    "FROM Master_Item ").ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }
        //public List<Cabang> GetCabang()
        //{
        //    List<Cabang> users = new List<Cabang>();
        //    try
        //    {
        //        using (var connection = new SqlConnection(ConnStrProd))
        //        {
        //            connection.Open();
        //            users = connection.Query<Cabang>("SELECT org_id, branch_name" +
        //                            " FROM Cabang_CRI").ToList();
        //            connection.Close();
        //        }
        //        return users;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        public async Task<(int, string)> UpdateUser(UserFormHR _userH)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {
                        No_id = _userH.No_id,
                        Kode = _userH.Kode,
                        Namaproduk = _userH.Namaproduk,
                        Expiredate = _userH.Expiredate,
                        Jumlah = _userH.Jumlah,
                        Ket = _userH.Ket
                        

                    };
                    var sql = await connection.ExecuteAsync("Update Detail_HRTEMP_CRI" +
                                " set Kode = @Kode" +
                                ", Namaproduk = @Namaproduk" +
                                ", Expiredate = @Expiredate" +
                                ", Jumlah = @Jumlah" +
                                ", Ket = @Ket" +
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
        public async Task<(int, string)> InsertUpdate(string seq, string filename, string update)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {

                        @FILENAME = filename,
                        @id_temp = seq
                    };

                    var sql = await connection.ExecuteAsync("update Header_Hilang_Rusak_CRI " +
                        "set FILENAME=@FILENAME " +
                        "where id_temp= @id_temp", parameters);


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
                        @FILENAME = filename,
                        @UpdateTS = update
                    };
                    var sql = await connection.ExecuteAsync("update Header_Hilang_Rusak_CRI " +
                        "set UpdateHR='Sudah' " +
                        "where FILENAME=@FILENAME", parameters);

                    connection.Close();
                    return (sql, "Berhasil");
                }
                catch (Exception ex)
                {
                    return (0, ex.Message.ToString());
                }
            }


        }
        public async Task<(int, string)> InsertUser(UserFormHRbaru _userH,string userid,string seq)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {
                        
                        @Kode = _userH.Kode,
                        @Namaproduk = _userH.Namaproduk,
                        @Batch = _userH.Batch,
                        @Expiredate = _userH.Expiredate,
                        @Jumlah = _userH.Jumlah,
                        @Ket= _userH.Ket,
                        @User_id= userid,
                        @seq= seq
                    };
                    var sql = await connection.ExecuteAsync("insert into Detail_HRTemp_CRI(Kode,Namaproduk,Batch,Expiredate,Jumlah,Ket,User_id,id_temp) " +
                            "values(@Kode," +
                                   "@Namaproduk, " +
                                   "@Batch, " +
                                   "@Expiredate, " +
                                   "@Jumlah, " +
                                   "@Ket, " +
                                   "@User_id," +
                                   "@seq )", parameters);

                    connection.Close();
                    return (sql, "Berhasil");
                }
                catch (Exception ex)
                {
                    return (0, ex.Message.ToString());
                }
            }
        }
        public async Task<(int, string)> DeleteUser(UserFormHR _userH)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {

                        No_id = _userH.No_id,
                        Kode = _userH.Kode,
                        Namaproduk = _userH.Namaproduk,
                        Batch = _userH.Batch,
                        Expiredate = _userH.Expiredate,
                        Jumlah = _userH.Jumlah,
                        Ket= _userH.Ket
                    };
                    var sql = await connection.ExecuteAsync("delete from Detail_HRTEMP_CRI where No_id = @No_id", parameters);


                    connection.Close();
                    return (sql, "Berhasil");
                }
                catch (Exception ex)
                {
                    return (0, ex.Message.ToString());
                }
            }
        }
        public async Task<(int, string)> InsertForm2(string userid)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {
                        @User_id = userid,
                    };
                    var sql = await connection.ExecuteAsync("insert into Detail_HRDetail_CRI(No_id,Kode,Namaproduk,Batch,Expiredate,Jumlah,Ket,User_id,id_temp)" +
                            "SELECT [No_id] " +
                            ",[Kode] " +
                            ",[Namaproduk] " +
                            ",[Batch] " +
                            ",[Expiredate] " +
                            ",[Jumlah] " +
                            ",[Ket] " +
                            ",[User_id] " +
                            ",[id_temp]" +
                           
                            "FROM Detail_HRTEMP_CRI where User_id = @User_id", parameters);

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
                        @User_id = userid
                    };
                    var sql = await connection.ExecuteAsync("Delete from Detail_HRTEMP_CRI where User_id = @User_id ", parameters);

                    connection.Close();
                    return (sql, "Berhasil");
                }
                catch (Exception ex)
                {
                    return (0, ex.Message.ToString());
                }
            }
        }
        //    try
        //    {
        //        connection.Open();
        //        var parameters = new
        //        {
        //            @User_id = userid
        //        };
        //        var sql = await connection.ExecuteAsync(" DBCC CHECKIDENT ('Detail_HRTEMP_CRI', RESEED, 0) ", parameters);

        //        connection.Close();
        //        return (sql, "Berhasil");
        //    }
        //    catch (Exception ex)
        //    {
        //        return (0, ex.Message.ToString());
        //    }
        //}




        public async Task<int> deletetemp(string userid)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @User_id = userid
                    };
                    var sql = await connection.ExecuteAsync("Delete from Detail_HRTEMP_CRI where User_id=@user_id",parameters);
                    connection.Close();
                    return sql;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> hitung()
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                   var results = connection.QueryFirst<int>("select count (No_id) from Detail_HRTEMP_CRI");
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