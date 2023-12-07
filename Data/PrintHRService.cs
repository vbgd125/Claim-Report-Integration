
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
using Syncfusion.Blazor.Maps.Internal;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Diagrams;
using System.Collections.Specialized;
using FourRoads.Common.Extensions;



namespace TSJ_CRI.Data
{
    public class PrintHRService
    {
        /// <summary>
        /// Create a simple PDF document
        /// </summary>
        /// <returns>Return the created PDF document as stream</returns>
        public MemoryStream CreatePdfDocument(List<UserForm> _userf, string branch, List<UserFormHR> _userh,string acronim)
        {
            //Create a new PDF document
            PdfDocument document = new PdfDocument();
            //Add a page
            document.Pages.PageAdded += new PageAddedEventHandler(Pages_PageAdded);
            PdfPage page = document.Pages.Add();
            //Create Pdf graphics for the page
            PdfGraphics g = page.Graphics;
            //Create a solid brush
            PdfBrush brush = new PdfSolidBrush(new PdfColor(0, 0, 0));

            RectangleF bounds = new RectangleF(0, 0, document.Pages[0].GetClientSize().Width, 90);
            PdfFont font5 = new PdfStandardFont(PdfFontFamily.TimesRoman, 9);
            PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);
            PdfStringFormat format4 = new PdfStringFormat();
            format4.Alignment = PdfTextAlignment.Center;
            PdfPageCountField count = new PdfPageCountField(font5, brush);

            PdfPageNumberField pageNumber = new PdfPageNumberField(font5, brush);

            //Create page count field.


            PdfCompositeField compositeField = new PdfCompositeField(font5, brush, " {0}/{1}", pageNumber, count);
            //ini di difoto bagian header baru di untuk create date dan halaman di hardcode jadi kalo ubah no revisi di screenshot lgi
            var basePath = Path.Combine("D:", "test upload", "header1.JPG");
            FileStream jpgImageStream = new FileStream(basePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //Load the JPEG image
            PdfImage jpgImage = new PdfBitmap(jpgImageStream);
            header.Graphics.DrawImage(jpgImage, new PointF(20, 0), new SizeF(490, 55));
            header.Graphics.DrawString(_userf.FirstOrDefault().create_date.ToString("dd MMMM yyyy"), font5, brush, new PointF(423, 27));
            compositeField.Draw(header.Graphics, new PointF(422, 39));
            document.Template.Top = header;
            float margin = 0;
            PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
            g.DrawString("BERITA ACARA " + _userf.FirstOrDefault().hilangrusak + " PRODUK ", font, brush, new PointF(150, 80));
            g.DrawString(_userf.FirstOrDefault().no_pengajuan, font, brush, new PointF(200, 95));
            if(branch == "TSJ Office")
            {
                g.DrawString("Pusat: " + branch, font, brush, new PointF(200, 125));
            }
            else
            {
                g.DrawString("Cabang: " + branch, font, brush, new PointF(200, 125));
            }
            

            PdfFont font1 = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);
            g.DrawString("Pada tanggal " + _userf.FirstOrDefault().tglkirim?.ToString("dd") + " bulan "+  _userf.FirstOrDefault().tglkirim?.ToString("MMMM") + " tahun "+ _userf.FirstOrDefault().tglkirim?.ToString("yyyy") + " dinyatakan telah terjadi " + _userf.FirstOrDefault().hilangrusak + " barang PT. Tri Sapta Jaya, yang dikirim ", font1, brush, new PointF(30, 160));
            g.DrawString("oleh pihak ekspedisi: ", font1, brush, new PointF(0, 180));
            g.DrawString("Nama Ekspedisi                        : " + _userf.FirstOrDefault().namaekspedisi, font1, brush, new PointF(0, 200));
            g.DrawString("No Polisi Kendaraan                 : " + _userf.FirstOrDefault().nokendaraan, font1, brush, new PointF(0, 220));
            g.DrawString("Nama Sopir                               : " + _userf.FirstOrDefault().namasupir, font1, brush, new PointF(0, 240));

            g.DrawString("Atas kiriman dengan dokumen pengiriman sebagai berikut: ", font1, brush, new PointF(0, 270));
            g.DrawString("SPK/Faktur/Shipment/DO/TT   : " + _userf.FirstOrDefault().nopengiriman, font1, brush, new PointF(0, 290));
            g.DrawString("Tanggal Pengiriman                   : " + _userf.FirstOrDefault().tglkirim?.ToString("dd MMMM yyyy"), font1, brush, new PointF(0, 310));
            g.DrawString("Pengiriman Dari                         : " + _userf.FirstOrDefault().pengirimandari, font1, brush, new PointF(0, 330));
            g.DrawString("Tujuan Pengiriman                     : " + _userf.FirstOrDefault().pengirimanke, font1, brush, new PointF(0, 350));
            g.DrawString("No.Dokumen Surat Jalan/AWB : " + _userf.FirstOrDefault().nodokument, font1, brush, new PointF(0, 370));
            g.DrawString("Jumlah Koli                                : " + _userf.FirstOrDefault().koli, font1, brush, new PointF(0, 390));

            g.DrawString("Adapun deskripsi produk adalah sebagai berikut: ", font1, brush, new PointF(0, 425));

            PdfGrid grid = new PdfGrid();
            PdfStringFormat format1 = new PdfStringFormat();
            format1.Alignment = PdfTextAlignment.Center;
            //Create a DataTable
            DataTable dataTable = new DataTable();

            //Add columns to the DataTable
            dataTable.Columns.Add("Kode Produk");
            dataTable.Columns.Add("Nama Produk");
            dataTable.Columns.Add("Batch Number");
            dataTable.Columns.Add("Expire Date");
            dataTable.Columns.Add("Jumlah");
            dataTable.Columns.Add("KET");

            //Add rows to the DataTable
            var no_id = "";
            var kode = "";
            var namaproduk = "";
            var batch = "";
            var expiredate = DateTime.Now;
            
            var ket = "";
            foreach (var hilangrusak in _userh)
            {
                no_id = hilangrusak.No_id.ToString();
                kode = hilangrusak.Kode;
                namaproduk = hilangrusak.Namaproduk;
                batch = hilangrusak.Batch;
                expiredate = (DateTime)hilangrusak.Expiredate;
                var jumlah = hilangrusak.Jumlah;
                ket = hilangrusak.Ket;
                dataTable.Rows.Add(new object[] { kode, namaproduk, batch,expiredate.ToString("dd MMM yyyy"),jumlah,ket });
            }

            //Assign data source
            grid.DataSource = dataTable;
            grid.Columns[0].Width = 70;
            grid.Columns[0].Format = format1;
            grid.Columns[1].Width = 150;
            grid.Columns[1].Format = format1;
            grid.Columns[2].Format = format1;
            grid.Columns[3].Format = format1;
            grid.Columns[3].Width = 70;
            grid.Columns[4].Width = 30;
            grid.Columns[4].Format = format1;
            grid.Columns[5].Format = format1;
            grid.Columns[5].Width = 150;
            //create and customize the string formats
            PdfStringFormat stringFormat = new PdfStringFormat();
            
            stringFormat.Alignment = PdfTextAlignment.Center;
            PdfStringFormat stringFormat1 = new PdfStringFormat();
                stringFormat1.Alignment = PdfTextAlignment.Center;

            //Apply string formatting to a column
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                grid.Rows[i].Cells[4].StringFormat = stringFormat1;
            }

            PdfGridLayoutFormat gridLayoutFormat = new PdfGridLayoutFormat();
            //Set pagination
            gridLayoutFormat.Layout = PdfLayoutType.Paginate;
            PdfLayoutResult result = grid.Draw(page, new RectangleF(new PointF(0, 440), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)),gridLayoutFormat);
           
            PdfGridCellStyle gridCellStyle = new PdfGridCellStyle();
            gridCellStyle.StringFormat = stringFormat;

            PdfStringFormat format = new PdfStringFormat();
            PdfLayoutFormat layoutFormat = new PdfLayoutFormat();
            layoutFormat.Break = PdfLayoutBreakType.FitPage;
            layoutFormat.Layout = PdfLayoutType.Paginate;

            PdfPageTemplateElement footer = new PdfPageTemplateElement(bounds);


            compositeField.Bounds = footer.Bounds;

            //Draw the composite field in footer.

            compositeField.Draw(footer.Graphics, new PointF(440, 40));

            //Add the footer template at the bottom.

            document.Template.Bottom = footer;
            //Set the line alignment
            if ((result.Bounds.Bottom <= 510) && (document.PageCount == 1) )
            {
                PdfTextElement textElement = new PdfTextElement("Demikian berita acara ini dibuat dengan sebenarnya untuk digunakan sebagaimana mestinya ", font1);

                textElement.Draw(page, new RectangleF(new PointF(30, result.Bounds.Bottom + 20), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                PdfFont font2 = new PdfStandardFont(PdfFontFamily.TimesRoman, 8);
                textElement = new PdfTextElement("Dibuat Oleh, ", font1);
                textElement.Draw(page, new RectangleF(new PointF(20, result.Bounds.Bottom + 50), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("Mengetahui, ", font1);
                textElement.Draw(page, new RectangleF(new PointF(250, result.Bounds.Bottom + 50), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page, new RectangleF(new PointF(10, result.Bounds.Bottom + 130), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page, new RectangleF(new PointF(130, result.Bounds.Bottom + 130), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page, new RectangleF(new PointF(250, result.Bounds.Bottom + 130), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page, new RectangleF(new PointF(370, result.Bounds.Bottom + 130), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);

                textElement = new PdfTextElement("Checker", font2);
                textElement.Draw(page, new RectangleF(new PointF(47, result.Bounds.Bottom + 142), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("Warehouse Cord", font2);
                textElement.Draw(page, new RectangleF(new PointF(152, result.Bounds.Bottom + 142), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("APJ/PJT*", font2);
                textElement.Draw(page, new RectangleF(new PointF(284, result.Bounds.Bottom + 142), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement(_userf.FirstOrDefault().namaekspedisi, font2);
                textElement.StringFormat = format4;
                textElement.Draw(page, new RectangleF(new PointF(162, result.Bounds.Bottom + 142), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
            }
            else
            {
                PdfPage page1 = document.Pages.Add();
                format.Alignment = PdfTextAlignment.Center;
                PdfTextElement textElement = new PdfTextElement("Demikian berita acara ini dibuat dengan sebenarnya untuk digunakan sebagaimana mestinya ", font1);

                textElement.Draw(page1, new RectangleF(new PointF(30, 20), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                PdfFont font2 = new PdfStandardFont(PdfFontFamily.TimesRoman, 8);
                textElement = new PdfTextElement("Dibuat Oleh, ", font1);
                textElement.Draw(page1, new RectangleF(new PointF(20, 50), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("Mengetahui, ", font1);
                textElement.Draw(page1, new RectangleF(new PointF(250, 50), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page1, new RectangleF(new PointF(10, 130), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page1, new RectangleF(new PointF(130, 130), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page1, new RectangleF(new PointF(250, 130), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("(.....................................)", font1);
                textElement.Draw(page1, new RectangleF(new PointF(370, 130), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);

                textElement = new PdfTextElement("Checker", font2);
                textElement.Draw(page1, new RectangleF(new PointF(47, 142), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("Warehouse Cord", font2);
                textElement.Draw(page1, new RectangleF(new PointF(152, 142), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement("APJ/PJT*", font2);
                textElement.Draw(page1, new RectangleF(new PointF(284, 142), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                textElement = new PdfTextElement(_userf.FirstOrDefault().namaekspedisi, font2);
                textElement.StringFormat = format4;
                textElement.Draw(page1, new RectangleF(new PointF(162, 142), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
            }
            

            //PdfTextElement element = new PdfTextElement("", new PdfStandardFont(PdfFontFamily.TimesRoman, 14));

            //PdfTextLayoutResult result1 = element.Draw(page, bounds, layoutFormat);

            ////PdfLayoutResult result = null;

            //for (int i = 0; i < 600; i++)

            //{
            //    bounds = new RectangleF(new PointF(0, result1.LastLineBounds.Y + 20), new SizeF(page.Graphics.ClientSize.Width - 20, page.Graphics.ClientSize.Height - 10));
            //    element.Text = "Some First text";
            //    result = element.Draw(page, bounds.Location, bounds.Width, layoutFormat);

            //    if ((result1.Remainder != null) && (result1.Remainder.Length > 0))

            //    {
            //        bounds = new RectangleF(new PointF(0, 10), new SizeF(page.Graphics.ClientSize.Width - 20, page.Graphics.ClientSize.Height - 10));
            //        element.Text = result1.Remainder;
            //        result = element.Draw(page, bounds.Location, bounds.Width, layoutFormat);

            //    }

            //    else

            //    {
            //        page = result1.Page;
            //        bounds = new RectangleF(new PointF(0, result1.LastLineBounds.Y + 20), new SizeF(page.Graphics.ClientSize.Width - 20, page.Graphics.ClientSize.Height - 10));
            //    }

            //}



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

    }
    
}


