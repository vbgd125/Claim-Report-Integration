using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.Drawing;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using TSJ_CRI.Model;
using System.Linq;
using System.Reflection.Emit;
using Syncfusion.DocIORenderer;
using Syncfusion.Blazor.Kanban.Internal;
using Syncfusion.Pdf.Tables;
using Syncfusion.Pdf;
using System;



namespace TSJ_CRI.Data
{
    public class PrintTSService
    {

        public MemoryStream CreateWord(List<UserForm2> _userf, string branch, List<UserKetidaksesuaian> _uketidaksesuaian)
            {

            var Name ="";
            var no_id="" ;
            var dokumen = "";
            var Po = "";
            
            

            //Creating a new document.
            using (WordDocument document = new WordDocument())
                {
                    //Adding a new section to the document.
                    WSection section = document.AddSection() as WSection;
                    //Set Margin of the section.
                    section.PageSetup.Margins.All = 72;
                    //Set page size of the section.


                    //Create Paragraph styles.
                    WParagraphStyle style = document.AddParagraphStyle("Normal") as WParagraphStyle;
                    style.CharacterFormat.FontName = "Calibri";
                    style.CharacterFormat.FontSize = 11f;
                    style.ParagraphFormat.BeforeSpacing = 0;
                    style.ParagraphFormat.AfterSpacing = 0;
                    //style.ParagraphFormat.LineSpacing = 13.8f;

                    style = document.AddParagraphStyle("Heading 1") as WParagraphStyle;
                    style.ApplyBaseStyle("Normal");
                    style.CharacterFormat.FontName = "Calibri Light";
                    style.CharacterFormat.FontSize = 16f;
                    style.CharacterFormat.TextColor = Syncfusion.Drawing.Color.FromArgb(46, 116, 181);
                    style.ParagraphFormat.BeforeSpacing = 12;
                    style.ParagraphFormat.AfterSpacing = 0;
                    style.ParagraphFormat.Keep = true;
                    style.ParagraphFormat.KeepFollow = true;
                    style.ParagraphFormat.OutlineLevel = OutlineLevel.Level1;
                    IWParagraph paragraph = section.HeadersFooters.Header.AddParagraph();

                WTextRange textRange = paragraph.AppendText(" ") as WTextRange;

               

                //Appends paragraph.

                    //paragraph = section.AddParagraph();
                    
                    //paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                    //textRange = paragraph.AppendText("PT.TRI SAPTA JAYA") as WTextRange;
                    //textRange.CharacterFormat.FontSize = 18;
                    //textRange.CharacterFormat.FontName = "Times New Roman";
                    //textRange.CharacterFormat.TextColor = Syncfusion.Drawing.Color.Black;
                    //textRange.CharacterFormat.Bold = true;

                IWTable table = section.Body.AddTable();
                table.ResetCells(1, 2);
                WTableRow row = table.Rows[0];
                row.Cells[0].Width = 78;
                row.Height = 63;
                table.TableFormat.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.Double;
                table.TableFormat.LeftIndent = 0;
                table.TableFormat.IsAutoResized = true;
                IWParagraph cellPara = row.Cells[0].AddParagraph();
                var basePath = Path.Combine("D:", "test upload", "Picture1.png");
                FileStream imageStream = new FileStream(basePath, FileMode.Open, FileAccess.Read);
                IWPicture pic = cellPara.AppendPicture(imageStream);
                pic.Height = 55;
                pic.Width = 65;
                pic.HorizontalPosition = 0;
                pic.VerticalPosition = 3;
                pic.TextWrappingStyle = TextWrappingStyle.InFrontOfText;
                cellPara = row.Cells[1].AddParagraph();
                row.Cells[1].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                row.Cells[1].Width = 370f;
                IWTextRange txt = cellPara.AppendText("PT.TRI SAPTA JAYA");
                cellPara.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                txt.CharacterFormat.FontSize = 24;
                txt.CharacterFormat.FontName = "Times New Roman";
                txt.CharacterFormat.TextColor = Syncfusion.Drawing.Color.Black;
                txt.CharacterFormat.Bold = true;


                //Appends paragraph.
                paragraph = section.AddParagraph();
                    paragraph.ParagraphFormat.FirstLineIndent = 36;
                    paragraph.BreakCharacterFormat.FontSize = 12f;
                    textRange = paragraph.AppendText(" ") as WTextRange;
                    textRange.CharacterFormat.FontSize = 12f;

                //Appends paragraph.
                paragraph = section.AddParagraph();
                
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText("Pusat / Cabang * " + branch) as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange.CharacterFormat.Bold = true;
                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText(" ") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                //Appends paragraph.
                paragraph = section.AddParagraph();
                    
                    paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                    paragraph.BreakCharacterFormat.FontSize = 12f;
                    textRange = paragraph.AppendText("BERITA ACARA KETIDAKSESUAIAN PENGIRIMAN") as WTextRange;
                    textRange.CharacterFormat.Bold = true;
                textRange.CharacterFormat.UnderlineStyle= UnderlineStyle.Single ;
                textRange.CharacterFormat.FontName = "Times New Roman";
                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText(" ") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 168;
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
                paragraph.BreakCharacterFormat.FontSize = 12f;

                
                textRange.CharacterFormat.FontName = "Times New Roman";

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 168;
                paragraph.ParagraphFormat.LineSpacing = 20;
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText("Tanggal : "+ _userf.First().Tanggal.ToString("dd/MM/yyyy")) as WTextRange;
                
                textRange.CharacterFormat.FontName = "Times New Roman";

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText(" ") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;

                paragraph = section.AddParagraph();
                
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Justify;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText("Pada Tanggal "+ _userf.First().Tanggal.ToString("dd")+ " bulan " + _userf.First().Tanggal.ToString("MM") + " tahun " +_userf.First().Tanggal.ToString("yyyy") + " telah dilaksanakan penerimaan barang dari jasa "  +_userf.First().PT + ", dan terdapat ketidaksesuaian") as WTextRange;

                textRange.CharacterFormat.FontName = "Times New Roman";
                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText(" ") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 0;
                paragraph.ParagraphFormat.LineSpacing = 20;

                paragraph.BreakCharacterFormat.FontSize = 12f;
                if (_userf.First().Ketidaksesuaian == "Jumlah Lebih")
                {
                    WCheckBox checkBox = paragraph.AppendCheckBox("Jumlah Lebih", true);
                    paragraph.AppendText("  Jumlah Lebih      ");
                    checkBox = paragraph.AppendCheckBox("Tidak Sesuai Pesanan", false);
                    paragraph.AppendText("  Tidak Sesuai Pesanan      ");
                    checkBox = paragraph.AppendCheckBox("Selisih Batch Number", false);
                    paragraph.AppendText("  Selisih Batch Number     ");
                    checkBox = paragraph.AppendCheckBox("ED Dekat", false);
                    paragraph.AppendText("  ED Dekat");
                }
                else if (_userf.First().Ketidaksesuaian == "Tidak Sesuai Pesanan")
                {
                    WCheckBox checkBox = paragraph.AppendCheckBox("Jumlah Lebih", false);
                    paragraph.AppendText("  Jumlah Lebih      ");
                    checkBox = paragraph.AppendCheckBox("Tidak Sesuai Pesanan", true);
                    paragraph.AppendText("  Tidak Sesuai Pesanan      ");
                    checkBox = paragraph.AppendCheckBox("Selisih Batch Number", false);
                    paragraph.AppendText("  Selisih Batch Number     ");
                    checkBox = paragraph.AppendCheckBox("ED Dekat", false);
                    paragraph.AppendText("  ED Dekat");
                }
                else if (_userf.First().Ketidaksesuaian == "Selisih Batch Number")
                {
                    WCheckBox checkBox = paragraph.AppendCheckBox("Jumlah Lebih", false);
                    paragraph.AppendText("  Jumlah Lebih      ");
                    checkBox = paragraph.AppendCheckBox("Tidak Sesuai Pesanan", false);
                    paragraph.AppendText("  Tidak Sesuai Pesanan      ");
                    checkBox = paragraph.AppendCheckBox("Selisih Batch Number", true);
                    paragraph.AppendText("  Selisih Batch Number     ");
                    checkBox = paragraph.AppendCheckBox("ED Dekat", false);
                    paragraph.AppendText("  ED Dekat");
                }
                else if (_userf.First().Ketidaksesuaian == "ED Dekat")
                {
                    WCheckBox checkBox = paragraph.AppendCheckBox("Jumlah Lebih", false);
                    paragraph.AppendText("  Jumlah Lebih      ");
                    checkBox = paragraph.AppendCheckBox("Tidak Sesuai Pesanan", false);
                    paragraph.AppendText("  Tidak Sesuai Pesanan      ");
                    checkBox = paragraph.AppendCheckBox("Selisih Batch Number", false);
                    paragraph.AppendText("  Selisih Batch Number     ");
                    checkBox = paragraph.AppendCheckBox("ED Dekat", true);
                    paragraph.AppendText("  ED Dekat");
                }
                //WCheckBox checkBox = paragraph.AppendCheckBox("Jumlah Lebih", true) ;
                //paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                //paragraph.AppendText("  Jumlah Lebih      ");
                //checkBox = paragraph.AppendCheckBox("Tidak Sesuai Pesanan", false);
                //paragraph.AppendText("  Tidak Sesuai Pesanan      ");


                //checkBox = paragraph.AppendCheckBox("Selisih Batch Number", false);
                //paragraph.AppendText("  Selisih Batch Number     ");
                //checkBox = paragraph.AppendCheckBox("ED Dekat", true);
                //paragraph.AppendText("  ED Dekat");
                //textRange.CharacterFormat.FontName = "Times New Roman";
                //paragraph = section.AddParagraph();
                //paragraph.ParagraphFormat.FirstLineIndent = 36;
                //paragraph.BreakCharacterFormat.FontSize = 12f;
                //textRange = paragraph.AppendText(" ") as WTextRange;
                //textRange.CharacterFormat.FontSize = 12f;

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText(" ") as WTextRange;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange.CharacterFormat.FontSize = 10;

                table = section.Body.AddTable();
                table.ResetCells(3, 6);
                table.TableFormat.IsAutoResized = true;
                WTableRow row1 = table.Rows[0];
                txt = table[0, 0].AddParagraph().AppendText("NO");
                txt.CharacterFormat.Bold = true;
                txt.CharacterFormat.FontSize = 9;
                table.ApplyVerticalMerge(0, 0, 1);
                table.GetStyle().ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                row1.Cells[0].CellFormat.SamePaddingsAsTable = true;

                txt.CharacterFormat.FontName = "Times New Roman";
                row1.Cells[0].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                row1.Height = 20;
                table.GetStyle().ParagraphFormat.SpaceAfterAuto = false;
                table[0, 0].Width = 30;

                WTableRow row2 = table.Rows[1];
                row2.Height = 15;
                txt = table[0, 1].AddParagraph().AppendText("No.Dokumen Pengiriman");
                txt.CharacterFormat.Bold = true;
                table.ApplyVerticalMerge(1, 0, 1);
                row1.Cells[1].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                txt.CharacterFormat.FontSize = 9;
                table[0, 1].Width = 70;
                txt.CharacterFormat.FontName = "Times New Roman";

                txt = table[0, 2].AddParagraph().AppendText("No.PO TSJ");
                txt.CharacterFormat.Bold = true;
                row1.Cells[2].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                txt.CharacterFormat.FontSize = 9;
                table.ApplyVerticalMerge(2, 0, 1);
                table[0, 2].Width = 70;
                txt.CharacterFormat.FontName = "Times New Roman";

                txt = table[0, 3].AddParagraph().AppendText("Nama Produk");
                txt.CharacterFormat.Bold = true;
                row1.Cells[3].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                table.ApplyVerticalMerge(3, 0, 1);
                txt.CharacterFormat.FontSize = 9;
                table[0, 3].Width = 110;
                txt.CharacterFormat.FontName = "Times New Roman";

                txt = table[0, 4].AddParagraph().AppendText("Ketidaksesuaian");
                txt.CharacterFormat.Bold = true;
                table.ApplyHorizontalMerge(0, 4, 5);
                row1.Cells[4].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                txt.CharacterFormat.FontSize = 9;
                txt.CharacterFormat.FontName = "Times New Roman";

                txt = table[1, 4].AddParagraph().AppendText("Pada Dokumen");
                row1.Cells[4].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                txt.CharacterFormat.FontSize = 9;
                row2.Cells[4].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                txt.CharacterFormat.FontName= "Times New Roman";

                txt = table[1, 5].AddParagraph().AppendText("Fisik Produk");
                row1.Cells[4].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                txt.CharacterFormat.FontSize = 9;
                row2.Cells[5].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                txt.CharacterFormat.FontName = "Times New Roman";

                foreach (var ketidaksesuaian in _uketidaksesuaian)
                {
                    no_id = ketidaksesuaian.No_id.ToString();
                    Name = ketidaksesuaian.Nama;
                    dokumen = ketidaksesuaian.Dokumen;
                    Po = ketidaksesuaian.PO;
                    var dokum = ketidaksesuaian.TSdokumen;
                    var produk = ketidaksesuaian.TSproduk;

                    txt = table[2, 0].AddParagraph().AppendText(ketidaksesuaian.No_id.ToString());
                    txt.CharacterFormat.FontSize = 9;
                    txt.CharacterFormat.FontName = "Times New Roman";

                    txt = table[2, 1].AddParagraph().AppendText(dokumen);
                    txt.CharacterFormat.FontSize = 9;
                    txt.CharacterFormat.FontName = "Times New Roman";

                    txt = table[2, 2].AddParagraph().AppendText(Po);
                    txt.CharacterFormat.FontSize = 9;
                    txt.CharacterFormat.FontName = "Times New Roman";

                    txt = table[2, 3].AddParagraph().AppendText(Name);
                    txt.CharacterFormat.FontSize = 9;
                    txt.CharacterFormat.FontName = "Times New Roman";

                    

                }

                table[0, 4].Width = 80;
                table[0, 5].Width = 110;

                table[1, 0].Width = 30;
                table[1, 1].Width = 70;
                table[1, 2].Width = 70;
                table[1, 3].Width = 110;
                table[1, 4].Width = 80;

                table[2, 0].Width = 30;
                table[2, 1].Width = 70;
                table[2, 2].Width = 70;
                table[2, 3].Width = 110;
                table[2, 4].Width = 80;
                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText(" ") as WTextRange;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange.CharacterFormat.FontSize = 10;

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText("Kesimpulan: ") as WTextRange;
                textRange.CharacterFormat.FontName= "Times New Roman";
                textRange.CharacterFormat.FontSize = 10;

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText(" ") as WTextRange;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange.CharacterFormat.FontSize = 10;

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText(_userf.First().Notes ) as WTextRange;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange.CharacterFormat.FontSize = 10;

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText(" ") as WTextRange;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange.CharacterFormat.FontSize = 10;

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText(" ") as WTextRange;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange.CharacterFormat.FontSize = 10;

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 18;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText("Demikian berita acara ini kami buat berdasarkan kenyataan yang sebenarnya.") as WTextRange;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange.CharacterFormat.FontSize = 12;

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.LeftIndent = -40;
                paragraph.ParagraphFormat.BeforeSpacing = 40;
                var basePath1 = Path.Combine("D:", "test upload", "tanda tangan.JPG");
                FileStream imageStream1 = new FileStream(basePath1, FileMode.Open, FileAccess.Read);
                IWPicture pic1 = paragraph.AppendPicture(imageStream1);
                //pic1.Height = 43;
                //pic1.Width = 50;
                //pic1.HorizontalPosition = 0;
                //pic1.VerticalPosition = 3;
                //Saves the Word document to MemoryStream.
                //DocToPDFConverter converter = new DocToPDFConverter();
                //PdfDocument pdfDocument = converter.ConvertToPDF(document);
                MemoryStream stream = new MemoryStream();
                    document.Save(stream, FormatType.Docx);
                    stream.Position = 0;
                    return stream;
                }
            }
        }
    
}
        
 
    
