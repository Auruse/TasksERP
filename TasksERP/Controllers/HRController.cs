using System;
using System.Linq;
using System.Web.Mvc;
using TasksERP.Models;

namespace TasksERP.Controllers
{
    public class HRController : Controller
    {
        // GET: IT        
        TicketsContainer db = new TicketsContainer();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            ViewBag.TicketsTotal = db.TicketsHRs.Count() > 0 ? db.TicketsHRs.Count() : 0;
            ViewBag.TicketsClosed = db.TicketsHRs.Count() > 0 ? db.TicketsHRs.Count(x => x.Status == "Closed") : 0;
            ViewBag.TicketsInProgress = db.TicketsHRs.Count() > 0 ? db.TicketsHRs.Count(x => x.Status == "In Progress") : 0;
            ViewBag.TicketsNew = db.TicketsHRs.Count() > 0 ? db.TicketsHRs.Count(x => x.Status == "New") : 0;
            return View(db.TicketsHRs);
        }
        [HttpGet]
        public ActionResult NewTaskHR()
        {

            return View();
        }

        [HttpPost]
        public ActionResult NewTaskHR(TicketsHR TicketData)
        {
            TicketData.Status = "New";
            TicketData.CreationDate = DateTime.Now.ToString("dd.MM.yyyy");
            
            db.TicketsHRs.Add(TicketData);
            db.SaveChanges();

            return RedirectToAction("Main", "Home");
        }
        [HttpGet]
        public ActionResult NewTaskSickLeave()
        {

            return View();
        }

        [HttpPost]
        public ActionResult NewTaskSickLeave(TicketsHR TicketDataHR)
        {
            TicketDataHR.Status = "New";
            TicketDataHR.CreationDate = DateTime.Now.ToString("dd.MM.yyyy");
            TicketDataHR.Subject = "Больничный";             
            
                db.TicketsHRs.Add(TicketDataHR);
                db.SaveChanges();
             
            return RedirectToAction("Main", "Home");
        }
        public ActionResult Exit()
        {

            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult GetTicket(int? id)
        {
            if (id.HasValue)
            {
                TicketsHR t = db.TicketsHRs.Find(id);
                t.Status = "In Progress";
                t.AssignedTo = "consultant01@test.ru";
                
                db.SaveChanges();
            }


            return RedirectToAction("Main", "HR");
        }
        public ActionResult Close(int? id)
        {
            if (id.HasValue)
            {
                TicketsHR t = db.TicketsHRs.Find(id);
                t.Status = "Closed";

                
                db.SaveChanges();
            }


            return RedirectToAction("Main", "HR");
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


            return RedirectToAction("Main", "HR");
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


            return RedirectToAction("Main", "HR");
        }
        [HttpGet]
        public ActionResult Details(int? id)
        {
            TicketsHR t = new TicketsHR();
            if (id.HasValue)
            {
                t = db.TicketsHRs.Find(id);
                ViewBag.TicketsTotal = db.TicketsHRs.Count();
            }
            else
            {
                ViewBag.TicketsTotal = 0;
                return RedirectToAction("Main", "HR");
            }

            return View(t);
        }
        public ActionResult Delete(int? id)
        {
            TicketsHR t = new TicketsHR();
            if (id.HasValue)
            {
                t = db.TicketsHRs.Find(id);
                db.TicketsHRs.Remove(t);
                db.SaveChanges();
            }

            return RedirectToAction("Main", "HR");

        }
        [HttpPost]
        public ActionResult Details(TicketsHR TicketData)
        {
            if (TicketData.Notes1 != null)
            {
                TicketsHR t = new TicketsHR();

                t = db.TicketsHRs.Find(TicketData.Id);
                var CommentText = TicketData.Notes1.Replace("\n", "</br>");

                t.Comment += $"<div class='row ticket-card mt-3 pb-2 border-bottom pb-3 mb-3'>" +
                             $"<div class='col-md-1'><img class='img-sm rounded-circle mb-4 mb-md-0' src='{Url.Content("~/images/faces/face2.jpg")}' alt='profile image'></div>" +
                             $"<div class='ticket-details col-md-9'><a class='d-flex'><p class='text-primary mr-1 mb-0'>{t.AssignedTo}</p>" +
                             $"<div class='media-body'><p class='card-text'>{CommentText}</p></div></a></div></div>";

                db.SaveChanges();
            }
            return RedirectToAction("Main", "HR");
        }
    }
}