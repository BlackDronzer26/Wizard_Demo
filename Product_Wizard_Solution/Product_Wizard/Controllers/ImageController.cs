using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Product_Wizard.Models;

namespace Product_Wizard.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            string extension = System.IO.Path.GetExtension(file.FileName);
            //string name = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
            int size = file.ContentLength;
            if (file != null && size > 0) { 
                if (extension == ".pdf" || extension == ".txt" || extension == ".docx" || extension == ".png" )
                {
                    try
                    {
                        string path = Path.Combine(
                            Server.MapPath(System.Web.Configuration.WebConfigurationManager.AppSettings.Get("UploadFilePath")), 
                            Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        ViewBag.Message = "File uploaded successfully";



                        ProductWizardEntities db = new ProductWizardEntities();
                        Image obj = new Image();
                        obj.Name = file.FileName;
                        obj.size = size;
                        obj.link = path;

                        db.Images.Add(obj);
                        db.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }
                else
                {
                    ViewBag.Message = "Please select pdf,txt,docx file only.";
                }
            }

            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View();
        }
        public ViewResult Downloadpage()
        {
            ProductWizardEntities db = new ProductWizardEntities();
            return View("Downloadpage", from Image in db.Images.Take(10) select Image);
        }
        public ActionResult Download(int id)
        {
            //int id = 5;
            string link = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("UploadFilePath");
            ProductWizardEntities db = new ProductWizardEntities();

            //Image obj = new Image();
             String filename = db.Images.Where(p=> p.Id ==id).Select(p2=>p2.Name).FirstOrDefault();
            //string url = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("UploadFilePath") + filename;
            //return View("Download", url);
            string extension = Path.GetExtension(filename).Replace(".","");
            string FileType = "application/" + extension;
            string fullLink = link + filename;
            return  File(Server.MapPath(fullLink),FileType,filename);
        }
    }
}