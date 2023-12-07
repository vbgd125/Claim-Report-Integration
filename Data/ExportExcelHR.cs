using static System.Net.Mime.MediaTypeNames;
using Syncfusion.ExcelExport;
using System.Data;
using Syncfusion.XlsIO;
using System.IO;

using System.Collections.Generic;
using TSJ_CRI.Model;
using Ubiety.Dns.Core;
using System.Linq;
using TSJ_CRI.Pages;

namespace TSJ_CRI.Data
{
    public class ExportExcelHR
    {
        public MemoryStream CreateExcel(List<UserFormHR> _userh, List<UserForm> _userf,string branch)
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];
                worksheet.Range["A1:Q1"].ColumnWidth = 40;
                //Initialize DataTable and data get from SampleDataTable method
                DataTable table = SampleDataTable(_userh,_userf,branch);

                //Import data from DataTable
                worksheet.ImportDataTable(table, true, 1, 1, true);

                //Save the document as a stream and retrun the stream.
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream;
                }
            }
        }
        
        private DataTable SampleDataTable(List<UserFormHR> _userh, List<UserForm> _userf, string branch)
        {
            //Here we create three columns
            DataTable table = new DataTable();
            
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Barang", typeof(string));
            table.Columns.Add("Nama Ekspedisi", typeof(string));
            table.Columns.Add("Tanggal kirim", typeof(string));
            table.Columns.Add("Nama Supir", typeof(string));
            table.Columns.Add("No Pengiriman", typeof(string));
            table.Columns.Add("Pengiriman dari", typeof(string));
            table.Columns.Add("Pengiriman ke", typeof(string));
            table.Columns.Add("No dokument", typeof(string));
            table.Columns.Add("Koli", typeof(int));
            table.Columns.Add("Approval Logistik", typeof(string));
            table.Columns.Add("Approval FA", typeof(string));
            table.Columns.Add("Cabang", typeof(string));
            table.Columns.Add("Kode Produk", typeof(string));
            table.Columns.Add("Nama Produk", typeof(string));
            table.Columns.Add("Batch Produk", typeof(string));
            table.Columns.Add("Expire Date", typeof(string));
            table.Columns.Add("Jumlah Produk", typeof(int));
            table.Columns.Add("Keterangan", typeof(string));

            var query = from header in _userf
                        join detail in _userh
                             on header.id_temp equals detail.id_temp
                        
                             
                        select new
                        {
                            header.no_pengajuan,
                            header.hilangrusak,
                            header.namaekspedisi,
                            header.tglkirim,
                            header.namasupir,
                            header.nopengiriman,
                            header.pengirimandari,
                            header.pengirimanke,
                            header.nodokument,
                            header.koli,
                            header.Approve,
                            header.FA,
                            branch,
                            detail.Kode,
                            detail.Namaproduk,
                            detail.Batch,
                            detail.Expiredate,
                            detail.Jumlah,
                            detail.Ket

                        };

            foreach (var export in query)
            {
                //Here we add four DataRows
                table.Rows.Add(export.no_pengajuan, export.hilangrusak, export.namaekspedisi, export.tglkirim?.ToString("dd MMMM yyyy"), export.namasupir, export.nopengiriman,
                    export.pengirimandari, export.pengirimanke, export.nodokument, export.koli,export.Approve,export.FA, export.branch,export.Kode,export.Namaproduk, export.Batch, export.Expiredate?.ToString("dd MMMM yyyy"),
                    export.Jumlah, export.Ket);
            }

            return table;
        }
    }
}