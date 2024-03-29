﻿using System;
using System.Linq;
using System.Web.Mvc;
using TasksERP.Models;

namespace TasksERP.Controllers
{
    public class HomeController : Controller
    {
        TicketsContainer db = new TicketsContainer();
        // GET: Home
        public ActionResult Index()
        {
            
           

            return View();
        }
         
        public ActionResult Main()
        {
            ViewBag.TicketsTotal = db.Tickets.Count()>0 ? db.Tickets.Count()  : 0;
            ViewBag.TicketsClosed = db.Tickets.Count() > 0 ? db.Tickets.Count(x=>x.Status=="Closed") : 0;
            ViewBag.TicketsInProgress = db.Tickets.Count() > 0 ? db.Tickets.Count(x => x.Status == "In Progress") : 0;
            ViewBag.TicketsNew = db.Tickets.Count() > 0 ? db.Tickets.Count(x => x.Status == "New") : 0;
            return View(db.Tickets);
        }
         
        public ActionResult Exit()
        {

            return View("Index");
        }
        [HttpGet]
        public ActionResult NewTask()
        {

            return View();
        }

        [HttpPost]
        public ActionResult NewTask(Tickets TicketData)
        {
            if (!string.IsNullOrEmpty(TicketData.Comment))
                {
                TicketData.Status = "New";
                TicketData.CreationDate = DateTime.Now.ToString();

                db.Tickets.Add(TicketData);
                db.SaveChanges(); };
            
            return RedirectToAction("Main");
        }
        public ActionResult GetTicket(int? id)
        {
            if (id.HasValue)
            {
            Tickets t = db.Tickets.Find(id);
                t.Status = "In Progress";
                t.AssignedTo = "consultant01@test.ru";
                t.AssignmentDate=DateTime.Now.ToString();
                db.SaveChanges();
            }
             

            return RedirectToAction("Main");
        }
        public ActionResult Close(int? id)
        {
            if (id.HasValue)
            {
                Tickets t = db.Tickets.Find(id);
                t.Status = "Closed";
                
                t.ClosureDate = DateTime.Now.ToString();
                db.SaveChanges();
            }


            return RedirectToAction("Main");
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


            return RedirectToAction("Main");
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


            return RedirectToAction("Main");
        }
        [HttpGet]
        public ActionResult Details(int? id)
        {
            Tickets t = new Tickets();
            if (id.HasValue)
            {
                t = db.Tickets.Find(id);
                ViewBag.TicketsTotal = db.Tickets.Count();
            }
            else
            {
                ViewBag.TicketsTotal = 0;
                return RedirectToAction("Main");
            }

            return View(t);
        }
        public ActionResult Delete(int? id)
        {
            Tickets t = new Tickets();
            if (id.HasValue)
            {
                t = db.Tickets.Find(id);
                db.Tickets.Remove(t);
                db.SaveChanges();
            }
            
          return RedirectToAction("Main");
             
        }
        [HttpPost]
        public ActionResult Details(Tickets TicketData)
        {
            if (TicketData.Notes1 != null)
            {
                Tickets t = new Tickets();

                t = db.Tickets.Find(TicketData.Id);
                var CommentText = TicketData.Notes1.Replace("\n", "</br>");

                t.Comment += $"<div class='row ticket-card mt-3 pb-2 border-bottom pb-3 mb-3'>" +
                             $"<div class='col-md-1'><img class='img-sm rounded-circle mb-4 mb-md-0' src='{Url.Content("~/images/faces/face2.jpg")}' alt='profile image'></div>" +
                             $"<div class='ticket-details col-md-9'><a class='d-flex'><p class='text-primary mr-1 mb-0'>{t.AssignedTo}</p>" +
                             $"<div class='media-body'><p class='card-text'>{CommentText}</p></div></a></div></div>";

                db.SaveChanges();
            }
            return RedirectToAction("Main");
        }
    }
}