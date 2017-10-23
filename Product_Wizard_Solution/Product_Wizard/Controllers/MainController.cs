using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Product.Models;
using Product_Wizard.Models;
using System.Data.Entity;
using System.IO;
using OfficeOpenXml;
using System.Drawing;

namespace Product_Wizard.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        //[HandleError]
        public ActionResult Index()
        {
            ProductWizardEntities db = new ProductWizardEntities();
            return View("Data", from Product_Details in db.Product_Details.Take(10) select Product_Details);
            //return View("ProductDetails");

        }

        public ActionResult ProductDetails()
        {
            return View();
        }

        private Product_Details GetProduct()
        {
            if (Session["Product_Details"] == null)
            {
                Session["Product_Details"] = new Product_Details();
            }
            return (Product_Details)Session["Product_Details"];
        }

        private void RemoveProduct()
        {
            Session.Remove("Product_Details");
        }


        [HttpPost]
        public ActionResult ProductDetails(ProductDetails data, string BtnNext)
        {

            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    Product_Details obj = GetProduct();
                    obj.Product_Name = data.Pname;
                    obj.Type = data.Ptype;
                    obj.Amount = data.Pamount;
                    return View("ManufactureDetails");
                }
            }
            return View();
        }

        public ActionResult ManufactureDetails(ManufactureDetails mdata, string BtnPrev, string BtnNext)
        {
            Product_Details obj = GetProduct();
            if (BtnPrev != null)
            {
                ProductDetails bd = new ProductDetails();
                bd.Pname = obj.Product_Name;
                bd.Ptype = obj.Type;
                bd.Pamount = obj.Amount;
                return View("ProductDetails", bd);
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    obj.Address = mdata.Address;
                    obj.Manufacture_Name = mdata.Mname;
                    obj.No_Of_Retailer = mdata.No_Of_Retailer;
                    obj.Online_Supplier = Convert.ToString(mdata.OnlineSupplier);
                    obj.Contact_No = mdata.Contact_No;
                    return View("ReatilerDetails");
                }
            }
            return View();
        }

        public ActionResult Upload(HttpPostedFileBase file, string BtnPrev, string BtnNext)
        {
            Product_Details obj = GetProduct();
            if (BtnPrev != null)
            {
                ManufactureDetails md = new ManufactureDetails();
                md.Address = obj.Address;
                md.Mname = obj.Manufacture_Name;
                md.No_Of_Retailer = obj.No_Of_Retailer;
                //md.OnlineSupplier = obj.Online_Supplier;
                md.Contact_No = (Int64)obj.Contact_No;
                return View("ManufactureDetails", md);
            }

            if (BtnNext != null)
            {
                string extension = System.IO.Path.GetExtension(file.FileName);
                //string name = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                int size = file.ContentLength;
                if (file != null && size > 0)
                {
                    if (extension == ".pdf" || extension == ".txt" || extension == ".docx" || extension == ".png")
                    {
                        try
                        {
                            string path = Path.Combine(
                                Server.MapPath(System.Web.Configuration.WebConfigurationManager.AppSettings.Get("UploadFilePath")),
                                Path.GetFileName(file.FileName));
                            file.SaveAs(path);
                            ViewBag.Message = "File uploaded successfully";



                            ProductWizardEntities db = new ProductWizardEntities();
                            Models.Image imageobj = new Models.Image();
                            imageobj.Name = file.FileName;
                            imageobj.size = size;
                            imageobj.link = path;

                            db.Images.Add(imageobj);
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
            return View();
        }

        public void ExportToExcel()
        {
            ProductWizardEntities db = new ProductWizardEntities();
            //List<Product_Details> excelData = db.Product_Details.Select(x =>  {

            //    Product_Name = x.Product_Name,
            //    Amount = x.Amount,
            //    Type = x.Type,
            //    Manufacture_Name = x.Manufacture_Name

            //}).ToList();


           var excelData = db.Product_Details.Select(x => x).ToList();

                ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "Reatiler_Info";
            ws.Cells["B1"].Value = "Com1";

            ws.Cells["A2"].Value = "Report";
            ws.Cells["B2"].Value = "Report1";

            ws.Cells["A3"].Value = "Date";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "ProductId";
            ws.Cells["B6"].Value = "Product_Name";
            ws.Cells["C6"].Value = "Amount";
            ws.Cells["D6"].Value = "Type";
            ws.Cells["E6"].Value = "Manufacture_Name";

            int rowStart = 7;
            foreach (var item in excelData)
            {
                if (item.No_Of_Retailer > 10)
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));

                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.Product_Id;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = item.Product_Name;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = item.Amount;
                    ws.Cells[string.Format("E{0}", rowStart)].Value = item.Type;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = item.Manufacture_Name;
                    rowStart++;
             }

                ws.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();

        }
    }
}

//        public ActionResult FinalDetail(ProductDetails bd, ManufactureDetails Md, string BtnPrev, string Cancel, string Save)
//        {
//            Product_Details obj = GetProduct();
//            if (BtnPrev != null)
//            {
//                Image image = new Image();
//                md.Address = obj.Address;
//                md.Mname = obj.Manufacture_Name;
//                md.No_Of_Retailer = obj.No_Of_Retailer;
//                //md.OnlineSupplier = obj.Online_Supplier;
//                md.Contact_No = obj.Contact_No;
//                return View("ManufactureDetails", md);

//            }
//            if (Cancel != null)
//            {
//                return RedirectToAction("Index");
//            }

//            if (Save != null)
//            {

//                ProductWizardEntities db = new ProductWizardEntities();
//                db.Product_Details.Add(obj);
//                db.SaveChanges();
//                RemoveProduct();
//                return View("Success");


//            }
//            return View();
//        }

//        public ActionResult Delete(int id)
//        {
//            ProductWizardEntities db = new ProductWizardEntities();
//            Product_Details data = db.Product_Details.Find(id);
//            if (data != null)
//            {
//                db.Product_Details.Remove(data);
//                //base.Seed(db);
//              //("DBCC CHECKIDENT('Product_Details',RESEED,1);");
//                db.SaveChanges();
//            }
//            return RedirectToAction("Index");
//        }
//    }
//}




 //Product_Details obj = GetProduct();
            ////if (BtnPrev != null)
            ////{
            ////    ManufactureDetails md = new ManufactureDetails();
            ////    md.Mname = obj.Manufacture_Name;
            ////    md.Address = obj.Address;
            ////    md.No_Of_Retailer = obj.No_Of_Retailer;
            ////    md.Contact_No = obj.Contact_No;
            ////    return View("ProductDetails", md);
            ////}

            //if (BtnNext != null)
            //{
            //    if (ModelState.IsValid)
            //    {
            //        //  obj.Address = rd.De
            //        //obj.Manufacture_Name = mdata.Mname;
            //        //obj.No_Of_Retailer = mdata.No_Of_Retailer;
            //        //obj.Online_Supplier = Convert.ToString(mdata.OnlineSupplier);
            //        //obj.Contact_No = mdata.Contact_No;
            //        //    return View("FinalDetails");
            //        //}

            //        List<ReatailerDetails> ListRD = new List<ReatailerDetails>();
            //        List<Retailer_Info> Lstrinfo = new List<Retailer_Info>();
            //        foreach (var item in rd)
            //        {
            //            foreach (var item2 in item.Details)
            //            {
            //                Retailer_Info rinfo = new Retailer_Info();

            //                //rinfo.address = item2.address;

            //                Lstrinfo.Add(rinfo);
            //            }

            //            //  ListRD

            //        }
            //    }
            //}

            //    return View();
            //}}