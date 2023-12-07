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
    public class ExportExcelTS
    {
        public MemoryStream CreateExcel(List<UserKetidaksesuaian> _userK, List<UserForm2> _userf,string branch)
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];
                worksheet.Range["A1:Q1"].ColumnWidth = 40;
                //Initialize DataTable and data get from SampleDataTable method
                DataTable table = SampleDataTable(_userK, _userf,branch);

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

        private DataTable SampleDataTable(List<UserKetidaksesuaian> _userK, List<UserForm2> _userf,string branch)
        {
            //Here we create three columns
            DataTable table = new DataTable();

            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Tanggal", typeof(string));
            table.Columns.Add("Jasa Ekspedisi PT", typeof(string));
            table.Columns.Add("Ketidaksesuaian", typeof(string));
            table.Columns.Add("Approval Logistik", typeof(string));
            table.Columns.Add("Approval FA", typeof(string));
            table.Columns.Add("Cabang", typeof(string));
            table.Columns.Add("No Dokumen", typeof(string));
            table.Columns.Add("No.PO TSJ", typeof(string));
            table.Columns.Add("Nama Produk", typeof(string));
            table.Columns.Add("Ketidaksesuaian Pada Dokumen", typeof(string));
            table.Columns.Add("Ketidaksesuaian Pada Fisik", typeof(int));
            

            var query = from header in _userf
                        join detail in _userK
                             on header.id_temp equals detail.id_temp

                        select new
                        {
                            header.no_pengajuan,
                            header.Tanggal,
                            header.PT,
                            header.Ketidaksesuaian,
                            header.Approve,
                            header.FA,
                            branch,
                            detail.Dokumen,
                            detail.PO,
                            detail.Nama,
                            detail.TSdokumen,
                            detail.TSproduk
                            

                        };

            foreach (var export in query)
            {
                //Here we add four DataRows
                table.Rows.Add(export.no_pengajuan, export.Tanggal, export.PT, export.Ketidaksesuaian,export.Approve,export.FA, export.branch, export.Dokumen,
                    export.PO, export.Nama, export.TSdokumen, export.TSproduk);
            }

            return table;
        }
    }
}
