using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GBW.Controllers
{
    public class HelperController : Controller
    {
        // GET: Helper
        public string SaveImageFromApi(SaveImageViewModel model)
        {
            try
            {
                try
                {
                    byte[] bytes = Convert.FromBase64String(model.Image);
                    HttpPostedFileBase objFile = (HttpPostedFileBase)new MemoryPostedFile(bytes);
                    if (objFile != null)
                    {
                        string FName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_UsersImg.jpg";
                        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(model.Image)))
                        {
                            try
                            {
                                using (Bitmap bm1 = new Bitmap(ms))
                                {
                                    //ImagePhysicalPath
                                    string @_Path = Server.MapPath("/UploadedImages/UsersImages/" + FName);
                                    bm1.Save(_Path, ImageFormat.Jpeg);
                                    return "/UploadedImages/UsersImages/" + FName;
                                }
                            }
                            catch (Exception ex)
                            {

                                throw ex;
                            }
                        }
                    }
                    return null;
                }
                catch (Exception)
                {

                    return null;
                }
            }
            catch
            {
                return null;
            }

        }
    }

    public class SaveImageViewModel
    {
        public string UserId { get; set; }
        public string Image { get; set; }
    }

    public class MemoryPostedFile : HttpPostedFileBase
    {
        private readonly byte[] fileBytes;

        public MemoryPostedFile(byte[] fileBytes, string fileName = null)
        {
            this.fileBytes = fileBytes;
            this.FileName = fileName;
            this.InputStream = new MemoryStream(fileBytes);
        }

        public override int ContentLength => fileBytes.Length;

        public override string FileName { get; }

        public override Stream InputStream { get; }
    }
}