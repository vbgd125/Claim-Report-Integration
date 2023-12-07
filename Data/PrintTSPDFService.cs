
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




namespace TSJ_CRI.Data
{
    public class PrintTSPDFService
    {
        /// <summary>
        /// Create a simple PDF document
        /// </summary>
        /// <returns>Return the created PDF document as stream</returns>
        public MemoryStream CreatePdfDocument1(List<UserForm2> _userf, string branch, List<UserKetidaksesuaian> _uketidaksesuaian,string acronim)
        {
            //Create a new PDF document
            PdfDocument document = new PdfDocument();
            //Add a page
            document.Pages.PageAdded += new PageAddedEventHandler(Pages_PageAdded);
            PdfPage page = document.Pages.Add();
            
            //Create Pdf graphics for the page
            PdfGraphics g = page.Graphics;
            //Create a solid brush
            PdfSolidBrush brush = new PdfSolidBrush(Color.Black);
            PdfPen pen = new PdfPen(new PdfColor(Color.Black), 1f);

            float margin = 40f;
            RectangleF bounds = new RectangleF(0, 0, document.Pages[0].GetClientSize().Width, 90);

            PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);

            var basePath = Path.Combine("D:", "test upload", "header.JPG");
            FileStream jpgImageStream = new FileStream(basePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //Load the JPEG image
            PdfImage jpgImage = new PdfBitmap(jpgImageStream);
            header.Graphics.DrawImage(jpgImage, new PointF(30, 0), new SizeF(450, 70));
            document.Template.Top = header;
            PdfStringFormat format3 = new PdfStringFormat();
            format3.Alignment = PdfTextAlignment.Justify;
            PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
            PdfFont font3 = new PdfStandardFont(PdfFontFamily.TimesRoman, 11);
            PdfFont font1 = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);
            PdfStringFormat format1 = new PdfStringFormat();
            format1.Alignment = PdfTextAlignment.Center;
            format1.LineAlignment = PdfVerticalAlignment.Middle;
            PdfStringFormat format4 = new PdfStringFormat();
            format4.Alignment = PdfTextAlignment.Center;
            
            if (branch == "TSJ Office")
            {
                g.DrawString("Pusat: " + branch, font, brush, new PointF(200, 85));
            }
            else
            {
                g.DrawString("Cabang: " + branch, font, brush, new PointF(200, 85));
            }
            g.DrawString("BERITA ACARA KETIDAKSESUAIAN PENGIRIMAN" , font, brush, new PointF(110, 100));
            g.DrawString( _userf.FirstOrDefault().no_pengajuan , font, brush, new PointF(200, 120));


            g.DrawString("Tanggal : " + _userf.FirstOrDefault().Tanggal.ToString("dd MMMM yyyy"), font3, brush, new Point(190, 150));
            g.DrawString("Pada tanggal " + _userf.FirstOrDefault().create_date.ToString("dd") + " bulan " + _userf.FirstOrDefault().create_date.ToString("MMMM") + " tahun " + _userf.FirstOrDefault().create_date.ToString("yyyy") + " telah dilaksanakan penerimaan barang dari jasa "  , font3, brush, new PointF(30, 190),format3);
            g.DrawString(_userf.FirstOrDefault().PT + " , dan terdapat ketidaksesuaian dengan alasan : " , font3, brush, new PointF(30, 205),format3);
            g.DrawString(_userf.FirstOrDefault().Ketidaksesuaian,font,brush,new PointF(207,225));

            PdfGrid pdfGrid = new PdfGrid();
            

            //Create and customize the string formats


            PdfStringFormat format2 = new PdfStringFormat();
           
            format2.LineAlignment = PdfVerticalAlignment.Middle;
            PdfGridCellStyle gridCellStyle1 = new PdfGridCellStyle();
            PdfPen transparentPen;
            transparentPen = new PdfPen(new PdfColor(Color.FromArgb(Color.Transparent.A, Color.Transparent.R, Color.Transparent.G, Color.Transparent.B)), .3f);
            transparentPen.LineCap = PdfLineCap.Square;

            DataTable dataTable = new DataTable();
            //Add columns to PdfGrid
            pdfGrid.Columns.Add(5);
            

                PdfGridRow row = pdfGrid.Rows.Add();
                row.Height = 20;
            
             row = pdfGrid.Rows.Add();
            row.Height = 20;
            
            



            //Add ColumnSpan


            PdfGridCell gridCell = pdfGrid.Rows[0].Cells[0];
            gridCell.RowSpan = 2;
            gridCell.StringFormat = format1;
            
            
            gridCell.Value = "No.Dokumen Pengiriman";
            gridCell = pdfGrid.Rows[0].Cells[1];
            gridCell.RowSpan = 2;
            gridCell.StringFormat = format1;
            gridCell.Value = "No.PO TSJ";
            gridCell = pdfGrid.Rows[0].Cells[2];
            gridCell.RowSpan = 2;
            gridCell.StringFormat = format1;
            pdfGrid.Columns[2].Width = 150;
            gridCell.Value = "Nama Produk";
            gridCell = pdfGrid.Rows[0].Cells[3];
            gridCell.ColumnSpan = 2;
            gridCell.RowSpan = 1;
            gridCell.StringFormat = format1;
            gridCell.Value = "Ketidaksesuaian";
            gridCell = pdfGrid.Rows[1].Cells[3];
            gridCell.RowSpan = 1;

            gridCell.StringFormat = format1;
            gridCell.Value = "Pada Dokumen";
            gridCell = pdfGrid.Rows[1].Cells[4];
            gridCell.RowSpan = 1;
            gridCell.StringFormat = format1;
            gridCell.Value = "Fisik Produk";
            

            var Name = "";
            var no_id = "";
            var dokumen = "";
            var Po = "";
            var dokum = 0;
            var produk = 0;
            foreach (var ketidaksesuaian in _uketidaksesuaian)
            {
                
                Name = ketidaksesuaian.Nama;
                dokumen = ketidaksesuaian.Dokumen;
                Po = ketidaksesuaian.PO;
                 dokum = ketidaksesuaian.TSdokumen;
                 produk = ketidaksesuaian.TSproduk;

                PdfGridRow row2 = pdfGrid.Rows.Add();
                row2 = pdfGrid.Rows.Add();
                
                row2.Cells[2].Value = Name;
                row2.Cells[2].StringFormat = format2;
                row2.Cells[0].Value = dokumen;
                row2.Cells[0].StringFormat = format1;
                row2.Cells[1].Value = Po;
                row2.Cells[1].StringFormat = format1;
                row2.Cells[3].Value = dokum.ToString();
                row2.Cells[3].StringFormat = format1;
                row2.Cells[4].Value = produk.ToString();
                row2.Cells[4].StringFormat = format1;

                

                
            }
            

            //Draw the PdfGrid
            PdfGridLayoutFormat gridLayoutFormat = new PdfGridLayoutFormat();
            //Set pagination
            gridLayoutFormat.Layout = PdfLayoutType.Paginate;
            gridLayoutFormat.Break = PdfLayoutBreakType.FitPage;
           
            PdfLayoutResult result = pdfGrid.Draw(page, new RectangleF(new PointF(35, 260), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)),gridLayoutFormat);

            PdfLayoutFormat layoutFormat = new PdfLayoutFormat();
            

            

            PdfGridCellStyle gridCellStyle = new PdfGridCellStyle();


            PdfStringFormat format = new PdfStringFormat();

            //Set the line alignment

            PdfPageTemplateElement footer = new PdfPageTemplateElement(bounds);
            PdfPageNumberField pageNumber = new PdfPageNumberField(font, brush);

            //Create page count field.

            PdfPageCountField count = new PdfPageCountField(font, brush);
            PdfCompositeField compositeField = new PdfCompositeField(font1, brush, "Halaman {0} dari {1}", pageNumber, count);

            compositeField.Bounds = footer.Bounds;

            //Draw the composite field in footer.

            compositeField.Draw(footer.Graphics, new PointF(440, 40));

            //Add the footer template at the bottom.

            document.Template.Bottom = footer;

            format.Alignment = PdfTextAlignment.Center;

            if ((result.Bounds.Bottom <= 392) && (document.PageCount == 1))
            {

                if (_userf.FirstOrDefault().Notes != null)
                {
                    PdfTextElement textElement1 = new PdfTextElement("Kesimpulan: ", font1);
                    textElement1.Draw(page, new RectangleF(new PointF(30, result.Bounds.Bottom + 10), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), layoutFormat);

                    textElement1 = new PdfTextElement(_userf.FirstOrDefault().Notes, font1);
                    textElement1.Draw(page, new RectangleF(new PointF(30, result.Bounds.Bottom + 25), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), layoutFormat);
                }
                PdfTextElement textElement = new PdfTextElement("Demikian berita acara ini dibuat dengan sebenarnya untuk digunakan sebagaimana mestinya ", font1);

                textElement.Draw(page, new RectangleF(new PointF(30, result.Bounds.Bottom + 55), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                PdfFont font2 = new PdfStandardFont(PdfFontFamily.TimesRoman, 8);
                textElement = new PdfTextElement("Dibuat Oleh, ", font1);
                textElement.Draw(page, new RectangleF(new PointF(30, result.Bounds.Bottom + 75), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("Mengetahui, ", font1);
                textElement.Draw(page, new RectangleF(new PointF(260, result.Bounds.Bottom + 75), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page, new RectangleF(new PointF(20, result.Bounds.Bottom + 155), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page, new RectangleF(new PointF(130, result.Bounds.Bottom + 155), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page, new RectangleF(new PointF(250, result.Bounds.Bottom + 155), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page, new RectangleF(new PointF(370, result.Bounds.Bottom + 155), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);

                textElement = new PdfTextElement("Kepala Gudang", font2);
                textElement.Draw(page, new RectangleF(new PointF(37, result.Bounds.Bottom + 167), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("APJ/PJT", font2);
                textElement.Draw(page, new RectangleF(new PointF(162, result.Bounds.Bottom + 167), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("Pimpinan/BM*", font2);
                textElement.Draw(page, new RectangleF(new PointF(279, result.Bounds.Bottom + 167), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement(_userf.FirstOrDefault().PT, font2);
                textElement.StringFormat = format4;
                textElement.Draw(page, new RectangleF(new PointF(210, result.Bounds.Bottom + 167), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);


            }
            else {

                PdfPage page1 = document.Pages.Add();
                layoutFormat.Layout = PdfLayoutType.Paginate;

                if (_userf.FirstOrDefault().Notes != null)
                {
                    PdfTextElement textElement1 = new PdfTextElement("Kesimpulan: ", font1);
                    textElement1.Draw(page1, new RectangleF(new PointF(30, 10), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), layoutFormat);
                    textElement1 = new PdfTextElement(_userf.FirstOrDefault().Notes, font1);
                    textElement1.Draw(page1, new RectangleF(new PointF(30, 25), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), layoutFormat);
                }
                PdfTextElement textElement = new PdfTextElement("Demikian berita acara ini dibuat dengan sebenarnya untuk digunakan sebagaimana mestinya ", font1);

                textElement.Draw(page1, new RectangleF(new PointF(30, 45), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                PdfFont font2 = new PdfStandardFont(PdfFontFamily.TimesRoman, 8);
                textElement = new PdfTextElement("Dibuat Oleh, ", font1);
                textElement.Draw(page1, new RectangleF(new PointF(30, 75), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("Mengetahui, ", font1);
                textElement.Draw(page1, new RectangleF(new PointF(260, 75), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page1, new RectangleF(new PointF(20, 155), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page1, new RectangleF(new PointF(130, 155), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page1, new RectangleF(new PointF(250, 155), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page1, new RectangleF(new PointF(370, 155), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                
                textElement = new PdfTextElement("Kepala Gudang", font2);
                textElement.Draw(page1, new RectangleF(new PointF(37, 167), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("APJ/PJT", font2);
                textElement.Draw(page1, new RectangleF(new PointF(162, 167), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("Pimpinan/BM*", font2);
                
                textElement.Draw(page1, new RectangleF(new PointF(279, 167), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement(_userf.FirstOrDefault().PT, font2);
                textElement.StringFormat = format4;
                textElement.Draw(page1, new RectangleF(new PointF(210, 167), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                
            }





            



            //PdfGrid grid2 = new PdfGrid();

            //grid2.Columns.Add(6);

            //PdfGridRow head = grid2.Headers.Add(2)[0];
            //head.Cells[0].Value = "NO";
            //head.Cells[0].RowSpan = 2;
            //head.Style= 
            //head.Cells[1].Value = "No.Dokumen Pengiriman";
            //head.Cells[1].RowSpan = 2;
            //head.Cells[2].Value = "No.PO TSJ";
            //head.Cells[2].RowSpan = 2;
            //head.Cells[3].Value = "Nama Produk";
            //head.Cells[3].RowSpan = 2;
            //head.Cells[4].Value = "Ketidaksesuaian";

            ////head.Cells[4].ColumnSpan = 2;

            //PdfGridRow row2 = grid2.Rows.Add();
            //row2 = grid2.Rows.Add();
            //row2.Cells[0].Value = "Job/Extra1:";
            //row2.Cells[1].Value = "Job/Extra2:";
            //row2.Cells[2].Value = "Job/Extra3:";
            //row2.Cells[3].Value = "Job/Extra4:";
            //row2.Cells[4].Value = "Job/Extra5:";

            ////Draws a grid
            //grid2.Draw(page.Graphics, new PointF(0, 500));


            //PdfLightTable pdfLightTable = new PdfLightTable();

            //    DataTable table = new DataTable();
            //table.Columns.Add("Name");

            //table.Columns.Add("Age");

            //table.Columns.Add("Sex");

            //Include rows to the DataTable.

            //table.Rows.Add(new string[] { "abc", "21", "Male" });

            ////Assign data source.

            //pdfLightTable.DataSource = table;

            ////Draw PdfLightTable.

            //pdfLightTable.Draw(page, new PointF(0, 0));

            ////Set the font
            //PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 36);
            //    //Draw the text
            //    g.DrawString("Hello world!", font, brush, new PointF(20, 20));
            //Saving the PDF to the MemoryStream
            MemoryStream ms = new MemoryStream();
                document.Save(ms);
                //If the position is not set to '0' then the PDF will be empty.
                ms.Position = 0;
                return ms;
            }
        void Pages_PageAdded(object sender, PageAddedEventArgs args)
        {
            PdfPage page = args.Page;
        }
        private void EndPageLayout(object sender, EndPageLayoutEventArgs e)
        {
            EndTextPageLayoutEventArgs args = (EndTextPageLayoutEventArgs)e;
            PdfPage page = args.Result.Page;
            PdfPen pen = PdfPens.Black;
            page.Graphics.DrawRectangle(pen, new RectangleF(PointF.Empty, page.Graphics.ClientSize));
        }
        private void BeginPageLayout(object sender, BeginPageLayoutEventArgs e)
        {
            int index = e.Page.Section.Pages.IndexOf(e.Page);
            float offset = 50;
            PdfSolidBrush brush = new PdfSolidBrush(Color.LightBlue);
            if (index % 2 == 0)
            {
                RectangleF bounds = e.Bounds;
                e.Page.Graphics.DrawEllipse(brush, bounds.Width / 2 - offset, bounds.Height / 2 - offset, 2 * offset, 2 * offset);
            }
            else
            {
                RectangleF bounds = e.Bounds;
                e.Page.Graphics.DrawRectangle(brush, bounds.Width / 2 - offset, bounds.Height / 2 - offset, 2 * offset, 2 * offset);
            }
        }
    }
    }

