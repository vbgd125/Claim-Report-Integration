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
    public class UserKetidaksesuaianService
    {
        private readonly string ConnStrProd;
        public UserKetidaksesuaianService(IConfiguration config)
        {
            ConnStrProd = config.GetConnectionString("103ConnTest");
        }

        public List<UserKetidaksesuaian> GetUser(string userid)
        {
            List<UserKetidaksesuaian> users = new List<UserKetidaksesuaian>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @userid=userid


                    };
                    users = connection.Query<UserKetidaksesuaian>("SELECT No_id,Dokumen ,PO, kode,Nama, TSdokumen, TSproduk,User_id " +
                                    "FROM DetailTSTEMP_CRI WHERE No_id != 81 and User_id=@userid order by No_id asc",parameters).ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<UserKetidaksesuaian> GetUser3()
        {
            List<UserKetidaksesuaian> users = new List<UserKetidaksesuaian>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    users = connection.Query<UserKetidaksesuaian>("SELECT No_id,Dokumen ,PO, Nama, TSdokumen, TSproduk,User_id,id_temp " +
                                    "FROM DetailTSDetail_CRI WHERE No_id != 81 order by No_id asc").ToList();
                    connection.Close();
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<UserKetidaksesuaian> GetUser1()
        {
            List<UserKetidaksesuaian> users = new List<UserKetidaksesuaian>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    users = connection.Query<UserKetidaksesuaian>("SELECT No_id,Tanggal , PT, Ketidaksesuaian,Notes " +
                                    "FROM HeaderTSDetail_CRI WHERE No_id != 81 order by No_id asc").ToList();
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
        public List<UserKetidaksesuaian> GetUser2(string id_temp)
        {
            List<UserKetidaksesuaian> users = new List<UserKetidaksesuaian>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @id_temp = id_temp


                    };
                    users = connection.Query<UserKetidaksesuaian>("SELECT a.No_id,Dokumen,a.PO,a.kode, a.Nama, a.TSdokumen, a.TSproduk,a.User_id,c.HNA " +
                                    "FROM DetailTSDetail_CRI a inner join Header_TSDetail_CRI b on a.id_temp= @id_temp and a.id_temp= b.id_temp" +
                                    " inner join Master_item c on a.kode=c.Kodeprod", parameters).ToList();
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

        public async Task<(int, string)> UpdateUser(UserKetidaksesuaian _userK)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {
                        No_id = _userK.No_id,
                        Dokumen = _userK.Dokumen,
                        PO = _userK.PO,
                        Nama = _userK.Nama,
                        TSdokumen = _userK.TSdokumen,
                        TSproduk = _userK.TSproduk
                        

                    };
                    var sql = await connection.ExecuteAsync("Update DetailTSTEMP_CRI" +
                                " set Dokumen = @Dokumen" +
                                ", PO = @PO" +
                                ", Nama = @Nama" +
                                ", TSdokumen = @TSdokumen" +
                                ", TSproduk = @TSproduk" +
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

        public List<Produk> Getkodeproduk()
        {
            List<Produk> users = new List<Produk>();
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    users = connection.Query<Produk>("SELECT concat(Kodeprod,'-',Namaprod) as pengenal, Kodeprod,Namaprod " +
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
        public async Task<(int, string)> InsertUser(UserBaru _userb, string userid, string seq)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {
                        @Dokumen = _userb.Dokumen,
                        @PO = _userb.PO,
                        @Nama = _userb.Nama,
                        @kode = _userb.kode,
                        @TSdokumen = _userb.TSdokumen,
                        @TSproduk = _userb.TSproduk,
                        @User_id = userid,
                        @Seq = seq
                    };
                    var sql = await connection.ExecuteAsync("insert into DetailTSTEMP_CRI(Dokumen,PO,kode,Nama,TSdokumen,TSproduk, User_id,id_temp )" +
                            "values(@Dokumen," +
                                   "@PO, " +
                                   "@kode,"+
                                   "@Nama, " +
                                   "@TSdokumen, " +
                                   "@TSproduk, " +
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
        public async Task<(int, string)> DeleteUser(UserKetidaksesuaian _userK)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {

                        No_id = _userK.No_id,
                        Dokumen = _userK.Dokumen,
                        PO = _userK.PO,
                        Nama = _userK.Nama,
                        TSdokumen = _userK.TSdokumen,
                        TSproduk = _userK.TSproduk
                        
                    };
                    var sql = await connection.ExecuteAsync("delete from DetailTSTEMP_CRI where No_id = @No_id", parameters);


                    connection.Close();
                    return (sql, "Berhasil");
                }
                catch (Exception ex)
                {
                    return (0, ex.Message.ToString());
                }
            }
        }

        public async Task<(int, string)> InsertUser2(string userid)
        {
            using (var connection = new SqlConnection(ConnStrProd))
            {
                try
                {
                    connection.Open();
                    var parameters = new
                    {
                        @User_id = userid
                    };

                    var sql = await connection.ExecuteAsync("insert into DetailTSDetail_CRI(No_id,Dokumen,PO,kode,Nama,TSdokumen,TSproduk,User_id, id_temp)" +
                            "SELECT [No_id] " +
                            ",[Dokumen] " +
                            ",[PO] " +
                            ",[kode]"+
                            ",[Nama] " +
                            ",[TSdokumen] " +
                            ",[TSproduk] " +
                            ",[User_id] " +
                            ",[id_temp] " +
                            "FROM DetailTSTEMP_CRI where User_id = @User_id", parameters);

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
                    var sql = await connection.ExecuteAsync("Delete from DetailTSTEMP_CRI where User_id = @User_id ", parameters);

                    connection.Close();
                    return (sql, "Berhasil");
                }
                catch (Exception ex)
                {
                    return (0, ex.Message.ToString());
                }
            }
        }
        public async Task<(int, string)> InsertUpdate(string seq,string filename, string update)
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

                    var sql = await connection.ExecuteAsync("update Header_TSDetail_CRI " +
                        "set FILENAME=@FILENAME " +
                        "where id_temp= @id_temp", parameters) ;


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
                    var sql = await connection.ExecuteAsync("update Header_TSDetail_CRI " +
                        "set UpdateTS='Sudah' " +
                        "where FILENAME=@FILENAME", parameters) ;

                    connection.Close();
                    return (sql, "Berhasil");
                }
                catch (Exception ex)
                {
                    return (0, ex.Message.ToString());
                }
            }


        }
        public async Task<int> deletetemp(string userid)
        {
            try
            {
                using (var connection = new SqlConnection(ConnStrProd))
                {
                    connection.Open();
                    var parameters = new
                    {
                        @userid=userid
                    };
                    var sql = await connection.ExecuteAsync("Delete from DetailTSTEMP_CRI where @User_id=userid",parameters);
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
                    var results = connection.QueryFirst<int>("select count (No_id) from DetailTSTEMP_CRI");
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
