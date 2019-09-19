using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IdentityModel.Protocols.WSTrust;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Windows.Media.Imaging;
using TasksERP.Models;

namespace TasksERP.Controllers
{
    public class ITController : Controller
    {
           
        // GET: IT        
        //TicketsContainer db = new TicketsContainer();
        LeavReqEntities db = new LeavReqEntities ();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main(string GetWithStatus)
        {
            ViewBag.TicketsTotal = db.TicketsArchives.Count() > 0 ? db.TicketsArchives.Count() : 0;
            ViewBag.TicketsClosed = db.TicketsArchives.Count() > 0 ? db.TicketsArchives.Count(x => x.Status == "Closed") : 0;
            ViewBag.TicketsInProgress = db.TicketsArchives.Count() > 0 ? db.TicketsArchives.Count(x => x.Status == "In Progress") : 0;
            ViewBag.TicketsNew = db.TicketsArchives.Count() > 0 ? db.TicketsArchives.Count(x => x.Status == "New") : 0;

            var username = "meshkov";
            //depending on your environment, you may need to specify a container along with the domain
            //ex: new PrincipalContext(ContextType.Domain, "yourdomain", "OU=abc,DC=xyz")
            using (var context = new PrincipalContext(ContextType.Domain, "mazars.ru"))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                if (user != null)
                {
                    ViewBag.UserName = user.Name;
                    ViewBag.EmailAddress = user.EmailAddress;
                    DirectoryEntry directoryEntry = user.GetUnderlyingObject() as DirectoryEntry;
                    ViewBag.Department = directoryEntry.Properties["department"].Value.ToString();
                    ViewBag.JobTitle = directoryEntry.Properties["title"].Value.ToString();
                    //ViewBag.img = GetUserPhoto(user.Name);
                    ViewBag.PhotoUrl = "https://outlook.office365.com/owa/service.svc/s/GetPersonaPhoto?email=" + user.EmailAddress + "&UA=0&size=HR64x64";


                    var ticketsDataSet = string.IsNullOrEmpty(GetWithStatus) ? db.TicketsArchives.Take(10).ToList() : db.TicketsArchives.Where(x => x.Status == GetWithStatus).Take(10).ToList();


                    return View(ticketsDataSet);

                } 
            }


                return View();
        }

         
      
       
         
        public ActionResult Exit()
        {

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult NewTaskIT()
        {

            return View();
        }

        [HttpPost]
        public ActionResult NewTaskIT(Tickets TicketData)
        {
            TicketData.Status = "New";
            TicketData.CreationDate = DateTime.Now.ToString();

            //db.TicketsArchives.Add(TicketData);
            //db.SaveChanges();

            return RedirectToAction("Main", "Home");
        }
        public ActionResult GetTicket(int? id)
        {
            if (id.HasValue)
            {
                TicketsArchive t = db.TicketsArchives.Find(id);
                t.Status = "In Progress";
                t.AssignedTo = "consultant01@test.ru";
                t.DateCreated = DateTime.Now;
                db.SaveChanges();
            }


            return RedirectToAction("Main", "IT");
        }
        public ActionResult Close(int? id)
        {
            if (id.HasValue)
            {
                //Tickets t = db.Tickets.Find(id);
                TicketsArchive t = db.TicketsArchives.Find(id);
                t.Status = "Closed";

                //t.ClosureDate = DateTime.Now.ToString();
                t.DateClosed = DateTime.Now;
                db.SaveChanges();
            }


            return RedirectToAction("Main", "IT");
        }
        public ActionResult Reply(int? id)
        {
            //if (id.HasValue)
            //{
            //    Tickets t = db.Tickets.Find(id);
            //    t.Status = "Close";

            //    t.ClosureDate = DateTime.Now.ToString();
            //    db.SaveChanges();
            //}


            return RedirectToAction("Main", "IT");
        }
        public ActionResult Redirect(int? id)
        {
            //if (id.HasValue)
            //{
            //    Tickets t = db.Tickets.Find(id);
            //    t.Status = "Close";

            //    t.ClosureDate = DateTime.Now.ToString();
            //    db.SaveChanges();
            //}


            return RedirectToAction("Main", "IT");
        }
        [HttpGet]
        public ActionResult Details(int? id)
        {
            TicketsArchive t = new TicketsArchive();
            if (id.HasValue)
            {
                int nid = id.Value;
                t = db.TicketsArchives.Where(x=>x.Id==nid).FirstOrDefault();
                ViewBag.TicketsArchivesTotal = db.TicketsArchives.Count();
            }
            else
            {
                ViewBag.TicketsTotal = 0;
                return RedirectToAction("Main", "IT");
            }

            return View(t);
        }
        public ActionResult Delete(int? id)
        {
            Tickets t = new Tickets();
            //if (id.HasValue)
            //{
            //    t = db.Tickets.Find(id);
            //    db.Tickets.Remove(t);
            //    db.SaveChanges();
            //}

            return RedirectToAction("Main", "IT");

        }
        [HttpPost]
        public ActionResult Details(TicketsArchive TicketData)
        {
            if (TicketData.Description != null)
            {
                TicketsArchive t = new TicketsArchive();

                t = db.TicketsArchives.Find(TicketData.Id);
                var CommentText = TicketData.Description.Replace("\n", "</br>");

                t.Comment += $"<div class='row ticket-card mt-3 pb-2 border-bottom pb-3 mb-3'>" +
                             $"<div class='col-md-1'><img class='img-sm rounded-circle mb-4 mb-md-0' src='{Url.Content("~/images/faces/face2.jpg")}' alt='profile image'></div>" +
                             $"<div class='ticket-details col-md-9'><a class='d-flex'><p class='text-primary mr-1 mb-0'>{t.AssignedTo}</p>" +
                             $"<div class='media-body'><p class='card-text'>{CommentText}</p></div></a></div></div>";

                db.SaveChanges();
            }
            return RedirectToAction("Main", "IT");
        }

        //public ActionResult Details(TicketsArchive TicketData)
        //{
        //    if (TicketData.Notes1 != null)
        //    {
        //        TicketsArchive t = new TicketsArchive();

        //        t = db.TicketsArchives.Find(TicketData.Id);
        //        var CommentText = TicketData.Notes1.Replace("\n", "</br>");

        //        t.Comment += $"<div class='row ticket-card mt-3 pb-2 border-bottom pb-3 mb-3'>" +
        //                     $"<div class='col-md-1'><img class='img-sm rounded-circle mb-4 mb-md-0' src='{Url.Content("~/images/faces/face2.jpg")}' alt='profile image'></div>" +
        //                     $"<div class='ticket-details col-md-9'><a class='d-flex'><p class='text-primary mr-1 mb-0'>{t.AssignedTo}</p>" +
        //                     $"<div class='media-body'><p class='card-text'>{CommentText}</p></div></a></div></div>";

        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Main", "IT");
        //}
    }
}