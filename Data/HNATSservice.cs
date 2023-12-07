using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Syncfusion.Pdf.Tables;
using System.Data;
using System.Collections.Generic;
using TSJ_CRI.Model;
using System.Linq;
using System;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Blazor;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Threading;
using Syncfusion.Blazor.Grids;
using MySqlX.XDevAPI.Common;
using static SkiaSharp.HarfBuzz.SKShaper;
using Syncfusion.Blazor.Diagrams;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Syncfusion.Blazor.Diagram;
using Syncfusion.DocIO.DLS;
using Syncfusion.Blazor.PivotView;
using TSJ_CRI.Pages;
using FourRoads.Common.Extensions;
using System.Globalization;

namespace TSJ_CRI.Data
{
    public class HNATSservice
    {
        public MemoryStream CreateHNATS(List<Produk> _produk, List<UserKetidaksesuaian> _userK, string branch, string acronim, List<UserForm2> _userf, string no_pengajuan)
        {
            //Create a new PDF document
            PdfDocument document = new PdfDocument();
            //Add a page
            PdfPage page = document.Pages.Add();
            //Create Pdf graphics for the page
            PdfGraphics g = page.Graphics;
            //Create a solid brush
            PdfBrush brush = new PdfSolidBrush(new PdfColor(0, 0, 0));

            float margin = 40f;
            RectangleF bounds = new RectangleF(0, 0, document.Pages[0].GetClientSize().Width, 90);

            PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);

            var basePath = Path.Combine("D:", "test upload", "header.JPG");
            FileStream jpgImageStream = new FileStream(basePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //Load the JPEG image
            PdfImage jpgImage = new PdfBitmap(jpgImageStream);
            header.Graphics.DrawImage(jpgImage, new PointF(30, 0), new SizeF(450, 70));
            document.Template.Top = header;
            PdfGrid grid = new PdfGrid();
            PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11, PdfFontStyle.Regular);
            PdfFont font1 = new PdfStandardFont(PdfFontFamily.TimesRoman, 14, PdfFontStyle.Regular);

            PdfStringFormat format1 = new PdfStringFormat();
            format1.Alignment = PdfTextAlignment.Center;
            //format1.LineAlignment = PdfVerticalAlignment.Middle;
            PdfStringFormat format2 = new PdfStringFormat();
            format2.Alignment = PdfTextAlignment.Right;
            PdfStringFormat format3 = new PdfStringFormat();
            format3.Alignment = PdfTextAlignment.Left;
            //format2.LineAlignment = PdfVerticalAlignment.Middle;

            g.DrawString("Nama Ekspedisi: " + _userf.FirstOrDefault().PT, font, brush, new PointF(0, 100));
            DataTable dataTable = new DataTable();
            //Add columns to PdfGrid


            var kode = "";
            var namaproduk = "";
            var BAC = "";
            var jumlah = 0;
            var Acronim = "";
            var total = 0;

            //Create a DataTable


            //Add ColumnSpan
            dataTable.Columns.Add("Kode Produk");
            dataTable.Columns.Add("Nama Produk");
            dataTable.Columns.Add("QTY");
            dataTable.Columns.Add("Cabang");
            dataTable.Columns.Add("NO BAC");
            dataTable.Columns.Add("HNA");
            dataTable.Columns.Add("Nilai Klaim");








            int maxvalue = 0;
            int maxnilai = 0;
            int totalAll = 0;

            foreach (var ketidaksesuaian in _userK)
            {

                kode = ketidaksesuaian.kode;
                namaproduk = ketidaksesuaian.Nama;

                jumlah = Math.Abs(ketidaksesuaian.TSdokumen - ketidaksesuaian.TSproduk);
                BAC = no_pengajuan;
                Acronim = acronim;
                var HNA = ketidaksesuaian.HNA;
                var nilai = Math.Abs(HNA * jumlah);
                totalAll = totalAll + nilai;
                
                    dataTable.Rows.Add(new object[] { kode, namaproduk, jumlah, Acronim, BAC,HNA.ToString("C2",
                  CultureInfo.CreateSpecificCulture("id-ID")),nilai.ToString("C2",
                  CultureInfo.CreateSpecificCulture("id-ID")) });
                    maxvalue = maxvalue + 1;
                
                
                



            }




            //for (int a = 0; a < maxvalue; a++)
            //{
            //    var nilai1 = Convert.ToInt32(dataTable.Rows[a][6]);

            //    maxnilai = maxnilai + nilai1;
            //    //int b = int.Parse(nilai);
            //    //maxnilai = maxnilai + nilai;
            //}
            dataTable.Rows.Add(new object[] { "Total", "", "", "", "", "", totalAll.ToString("C2",
                  CultureInfo.CreateSpecificCulture("id-ID")) });
            grid.DataSource = dataTable;
            grid.Columns[0].Width = 55;
            grid.Columns[1].Width = 80;
            grid.Columns[2].Width = 30;
            grid.Columns[2].Format = format1;
            grid.Columns[3].Width = 50;
            grid.Columns[3].Format = format1;
            grid.Columns[4].Format = format1;
            grid.Rows[0].Cells[4].StringFormat = format3;
            grid.Columns[5].Width = 70;
            grid.Columns[5].Format = format1;
            grid.Rows[0].Cells[5].StringFormat = format2;
            grid.Columns[6].Width = 70;
            grid.Rows[0].Cells[6].StringFormat = format2;
            grid.Rows[maxvalue].Cells[0].ColumnSpan = 6;



            PdfStringFormat stringFormat = new PdfStringFormat();

            stringFormat.Alignment = PdfTextAlignment.Center;
            PdfGridCellStyle gridCellStyle = new PdfGridCellStyle();


            //Apply string formatting to a column
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                grid.Rows[i].Cells[5].StringFormat = format2;
                grid.Rows[i].Cells[6].StringFormat = format2;
            }

            PdfGridLayoutFormat gridLayoutFormat = new PdfGridLayoutFormat();
            //Set pagination
            gridLayoutFormat.Layout = PdfLayoutType.Paginate;
            PdfLayoutResult result = grid.Draw(page, new RectangleF(new PointF(0, 120), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
            
            
            //var kode = "";
            //var namaproduk = "";
            //var Qty = "";
            //var Acronim ="";
            //var expiredate = DateTime.Now;
            //var jumlah = "";
            //var ket = "";

            //foreach (var produkandqty in _produk.Zip(_userh, (a, b) => new { A = a, B = b }))
            //{
            //    kode = produkandqty.A.kodeprod;

            //    namaproduk = produkandqty.A.namaprod;
            //    Qty = produkandqty.B.Jumlah;
            //    Acronim = acronim;

            //    dataTable.Rows.Add(new object[] { kode, namaproduk,Qty, expiredate.ToString("dd MMMM yyyy"), jumlah, ket });
            //}
            MemoryStream ms = new MemoryStream();
            document.Save(ms);
            //If the position is not set to '0' then the PDF will be empty.
            ms.Position = 0;
            return ms;
        }
    }
}

 
