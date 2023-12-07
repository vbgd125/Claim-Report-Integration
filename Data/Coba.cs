using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http.Headers;
using System.ComponentModel.Design;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Hosting.Server;
using System.Web;
using System.Data;

namespace TSJ_CRI.Data
{

    [Route("api/[controller]")]
    [ApiController]
    public class SampleDataController : ControllerBase
    {

        private IHostingEnvironment hostingEnv;
        public SampleDataController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }
        [HttpPost("[action]")]
        public async void Save(IList<IFormFile> UploadFiles)
        {
            try
            {
                foreach (var file in UploadFiles)
                {
                    if (UploadFiles != null)
                    {
                        var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        filename = hostingEnv.WebRootPath + $@"\{filename}";
                        if (!System.IO.File.Exists(filename))
                        {
                            using (FileStream fs = System.IO.File.Create(filename))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                        else
                        {
                            Response.Clear();
                            Response.StatusCode = 204;
                            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File already exists.";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }

        }
        [HttpPost("[action]")]
        public async void Remove(IList<IFormFile> UploadFiles)
        {
            try
            {
                var filename = hostingEnv.ContentRootPath + $@"\{UploadFiles[0].FileName}";
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed successfully";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;

            }
        }
        [HttpGet("[action]")]
        public async void Download(string filename)
        {
            //var url = "D:\"{0}"\test upload\" ;
            var filePath = Path.Combine("D:", "test upload", filename);
            //Directory.GetCurrentDirectory());
            try
            {
                WebClient mywebdownload = new WebClient();
                mywebdownload.DownloadFile(filePath, filename);
                return;
            }
            catch (Exception)
            {
                return;
            }



            //var filePath = Path.Combine(
            //          Directory.GetCurrentDirectory());
            //IFileProvider provider = new PhysicalFileProvider(filePath);
            //IFileInfo fileInfo = provider.GetFileInfo(filename);
            //var readStream = fileInfo.CreateReadStream();
            //var mimeType = "application/pdf";
            //return File(readStream, mimeType, filename);
        }
        [HttpGet("[action]")]
        public FileResult Download1(string filename)
        {
            string[] path = { @"D:\test upload" };
            var filePath = Path.Combine(path);
            IFileProvider provider = new PhysicalFileProvider(filePath);
            IFileInfo fileInfo = provider.GetFileInfo(filename);
            var readStream = fileInfo.CreateReadStream();
            var mimeType = "application/pdf";
            return File(readStream, mimeType, filename);

        }
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    var server = HttpContext.
        //    string url = "https://blackwebsite.s3.us-east-2.amazonaws.com/42431c75-9311-4730-960a-4052f2427ab9.png";
        //    if (!System.IO.Directory.Exists(Server.MapPath("~/Files")))
        //    {
        //        System.IO.Directory.CreateDirectory(Server.MapPath("~/Files"));
        //    }
        //    using (System.Net.WebClient client = new System.Net.WebClient())
        //    {
        //        client.DownloadFile(new Uri(url), System.Web.Hosting.HostingEnvironment.MapPath("~/Files/42431c75-9311-4730-960a-4052f2427ab9.png"));
        //    }
        //}
    }
}