using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using TSJ_CRI.Model;

namespace TSJ_CRI.Data
{
    public class EmailServiceHR
    {


        public async Task KirimemailAsync(string sender, string Receiver, string subject, string contains, string ccEmail, List<UserForm> _userf)
        {

            SmtpClient Smtp_Server = new SmtpClient();
            MailMessage e_mail = new MailMessage();
            Smtp_Server.UseDefaultCredentials = false;
            Smtp_Server.Port = 25;
            Smtp_Server.EnableSsl = false;
            // Smtp_Server.Host = "imss.enseval.com"
            Smtp_Server.Host = "mail.enseval.com";

            e_mail = new MailMessage();
            e_mail.From = new MailAddress(sender, subject);
            e_mail.To.Add(Receiver);
            string[] cc_email = ccEmail.Split(',');
            foreach (string cc_email_split in cc_email)
            {
                if (cc_email_split != "")
                    e_mail.CC.Add(new MailAddress(cc_email_split));
            }

            e_mail.Subject = "TSJCRI: ajuan klaim kepada " +_userf.FirstOrDefault().namaekspedisi + " mengenai " + _userf.FirstOrDefault().hilangrusak ;
            e_mail.IsBodyHtml = true;
            e_mail.Body = contains;

            try
            {
                Smtp_Server.Send(e_mail);

                Console.WriteLine("Berhasil");
            }
            catch (Exception ex)
            {
                Console.WriteLine("gagal karena " + ex.Message.ToString());
            }

        }
    }
}
