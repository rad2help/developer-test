using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OrangeBricks.Web.Attributes;
using OrangeBricks.Web.Controllers.Appointments.Builders.OrangeBricks.Web.Controllers.Offers.Builders;
using OrangeBricks.Web.Controllers.Appointments.Commands;
using OrangeBricks.Web.Controllers.Appointments.ViewModels;
using OrangeBricks.Web.Controllers.Offers;
using OrangeBricks.Web.Controllers.Offers.Builders;
using OrangeBricks.Web.Controllers.Offers.Commands;
using OrangeBricks.Web.Controllers.Property.ViewModels;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Appointments
{
    public class AppointmentsController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Appointments
        public ActionResult Index()
        {
            return View(_context.Appointments.ToList());
        }

        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = _context.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Status,BuyerUserId,CreatedAt,UpdatedAt")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Appointments.Add(appointment);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = _context.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Status,BuyerUserId,CreatedAt,UpdatedAt")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(appointment).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = _context.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = _context.Appointments.Find(id);
            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult OnProperty(int id)
        {
            var builder = new AppointmentsOnPropertyViewModelBuilder(_context);
            var viewModel = builder.Build(id);

            return View(viewModel);
        }

        [HttpPost]
        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult Accept(AcceptAppointmentCommand command)
        {
            var handler = new StatusAppointmentCommandHandler(_context);

            handler.Handle(command);

            return RedirectToAction("OnProperty", new { id = command.PropertyId });
        }

        [HttpPost]
        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult Reject(RejectAppointmentCommand command)
        {
            var handler = new StatusAppointmentCommandHandler(_context);

            handler.Handle(command);

            return RedirectToAction("OnProperty", new { id = command.PropertyId });
        }

        [HttpGet]
        [OrangeBricksAuthorize(Roles = "Buyer")]
        public ActionResult MyAppointments()
        {
            var builder = new MyAppointmentsViewModelBuilder(_context);
            var viewModel = builder.Build(User.Identity.GetUserId());

            return View(viewModel);
        }

       
    }

    public class MyAppointmentsViewModelBuilder
    {
        private IOrangeBricksContext _context;

        public MyAppointmentsViewModelBuilder(IOrangeBricksContext context)
        {
            _context = context;
        }

        public MyAppointmentsViewModel Build(string userId)
        {
            var appts = _context.Appointments
                .Where(p => p.BuyerUserId == userId)
                .Include(x => x.Property);


            var appointments = appts
                .Select(app => new MyAppointmentViewModel()
                {
                    Appointment =  new AppointmentViewModel() { 
                    BuyerUserId = app.BuyerUserId,
                    CreatedAt = app.CreatedAt,
                    Date = app.Date,
                    Id = app.Id,
                    Status = app.Status,
                    UpdatedAt = app.UpdatedAt
                    },
                    Property = app.Property
                }).ToList();

            var apptsVM = new MyAppointmentsViewModel() {MyAppointments = appointments};


            return apptsVM;

        }


    }
}
