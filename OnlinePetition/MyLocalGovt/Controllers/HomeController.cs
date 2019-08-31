using MyLocalGovt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace MyLocalGovt.Controllers
{
    public class HomeController : Controller
    {


        AlutaEntities Db = new AlutaEntities();

        [HttpGet]
        public ActionResult Petition(PetitionCategory pet, PetitionComplaint comp)
        {
            PetitionModel model = new PetitionModel();
        
             model.CategoryList = Db.PetitionCategories.ToList();
             model.Selected = pet.CategoryNameId;
             model.ComplaintList = Db.PetitionComplaints.ToList();
             model.CompSelected = comp.ComplaintId;

            
             return View(model);
        }


       
       
   
        public ActionResult PetitionDifferent()
        {
            PetitionModel model = new PetitionModel();
            var list = PetitionTypes();
            model.PetitionsList = list;

            return View(model);
        }
        [HttpPost]
        public ActionResult PetitionDifferent(PetitionModel mode)
        {

            List<PetitionModel> list = new List<PetitionModel>();
            var r = Db.PetitionCategories.ToList();
            foreach (var a in r)
            {
                PetitionModel model = new PetitionModel();
                model.CategoryNameId = a.CategoryNameId;
                model.Category = a.Category;
                model.CategoryPic = a.CategoryPic;
                list.Add(model);

            }
            mode.PetitionsList = list;

            return View(mode);
        }
        public ActionResult Index()
        {
            PetitionModel model = new PetitionModel();
            var list = PetitionTypes();
            model.PetitionsList = list;
            var listSign = PetitionSign();
            model.PetitionsSignList = listSign;

            return View(model);
          
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public List<PetitionModel> PetitionTypes()
        {
            List<PetitionModel> list = new List<PetitionModel>();

            List<PetitionCategory> CategoryList = Db.PetitionCategories.OrderBy(x => x.Category).ToList();

            foreach (var a in CategoryList)
            {
                PetitionModel model = new PetitionModel();
                model.CategoryNameId = a.CategoryNameId;
                model.Category = a.Category;
                model.CategoryPic = a.CategoryPic;

              
                list.Add(model);

            }
            return list;
        }
        public List<PetitionModel> PetitionSign()
        {
            List<PetitionModel> listSign = new List<PetitionModel>();

            List<PetitionInfo> SignList = Db.PetitionInfoes.OrderBy(x => x.PetitionId).ToList();

            foreach (var a in SignList)
            {
                PetitionModel model = new PetitionModel();
                model.PetitionId = a.PetitionId;
                model.NameOfFile = a.NameOfFile;
                model.Title = a.Title;
                model.WhySign = a.WhySign;
                model.PetDate = a.PetDate.ToString();
                model.IsApproved = a.Approval.Value;
                listSign.Add(model);

            }
            return listSign;
        }

      


        [HttpPost]
        [Authorize]
        public ActionResult Petition(PetitionModel model, HttpPostedFileBase uploadedFile, RegisterViewModel mode)
        {
            //ToAdminWithCustomerEmail(model);

            if (ModelState.IsValid)
            {

                PetitionInfo petitionInfo = new PetitionInfo();
                petitionInfo.PetitionId = model.PetitionId;
                petitionInfo.Title = model.Title;
              //  petitionInfo.StateId = model.StateId;
                petitionInfo.SchoolId = mode.SchoolId;
                petitionInfo.ToWhom = model.ToWhom;
                petitionInfo.WhySign = model.WhySign;
                petitionInfo.Phone = model.Phone;
                petitionInfo.CategoryId = model.Selected;
                petitionInfo.ComplaintId = model.CompSelected;
                petitionInfo.Approval = false;
                petitionInfo.PetDate = DateTime.Now;
                petitionInfo.UserId = User.Identity.GetUserId().ToString();

                if (uploadedFile != null)
                {
                    string filename = System.IO.Path.GetFileName(uploadedFile.FileName);
                    string physicalPath = Server.MapPath("~/images/Files/" + filename);
                    uploadedFile.SaveAs(physicalPath);
                    petitionInfo.NameOfFile = filename;
                    ViewBag.File = petitionInfo.NameOfFile;

                }
                else
                {

                    petitionInfo.NameOfFile = "white.jpg";
                }
                ViewBag.File = petitionInfo.NameOfFile;
                Db.PetitionInfoes.Add(petitionInfo);
                Db.SaveChanges();
                TempData["Success"] = "Your Petition Has Been Sucessfully Created!";

                return RedirectToAction("Petition");
            }
        
            return View(model);
            }

       
        public void ToAdminWithCustomerEmail(PetitionModel model)
        {


            string MessageFrom = string.Format("Dear Customer, You have sucessfully created your petition with Title: {0} to {1}:<br/>", model.Title, model.ToWhom);

            var myMessage = new MailMessage();
            myMessage.To.Add(new MailAddress("X@x.com"));  // replace with valid value 
           // myMessage.Bcc.Add(new MailAddress("model.email"));
           // myMessage.Bcc.Add(new MailAddress(model.Email));
            myMessage.From = new MailAddress("ee@gmail.com");  // replace with valid value
            myMessage.Subject = "Petition Created!";
            myMessage.Body = MessageFrom;

            myMessage.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "xxxxx@gmail.com",  // replace with valid value
                    Password = "xxxxx"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(myMessage);
            }
        }




        [HttpGet]
        public ActionResult Search()
        {
            PetitionModel model = new PetitionModel();
            var listSigns = PetitionSigns();
            model.PetitionsSignList = listSigns;
            return View(model);
        }
       
        [HttpPost]
        public ActionResult Search(PetitionModel mode)
        {

            List<PetitionModel> list = new List<PetitionModel>();
            var r = Db.PetitionInfoes.Where(emp => emp.Title.Contains(mode.Search)).ToList();
            foreach (var a in r)
            {
                PetitionModel model = new PetitionModel();
                model.PetitionId = a.PetitionId;
                model.Title = a.Title;
                model.Phone = a.Phone;
                model.WhySign = a.WhySign;
                model.ToWhom = a.ToWhom;
                model.PetDate = a.PetDate.ToString();
                model.NameOfFile = a.NameOfFile;
                list.Add(model);

            }
            mode.PetitionsSignList = list;

            return View(mode);
        }
        public List<PetitionModel> PetitionSigns()
        {
            List<PetitionModel> listSigns = new List<PetitionModel>();

            List<PetitionInfo> SignList = Db.PetitionInfoes.OrderBy(x => x.PetitionId).ToList();

            foreach (var a in SignList)
            {
                PetitionModel model = new PetitionModel();
                model.PetitionId = a.PetitionId;
                model.NameOfFile = a.NameOfFile;
                model.Title = a.Title;
                model.WhySign = a.WhySign;
                model.PetDate = a.PetDate.ToString();
               

            }
            return listSigns;
        }
     
        }
    }

