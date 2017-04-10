using System;
using System.Collections.Generic;
using OrangeBricks.Web.Models;
using Microsoft.AspNet.Identity;

namespace OrangeBricks.Web.Controllers.Property.Commands
{
    public class RequestAppointmentCommandHandler
    {
        private readonly IOrangeBricksContext _context;

        public RequestAppointmentCommandHandler(IOrangeBricksContext context)
        {
            _context = context;
        }

        public void Handle(RequestAppointmentCommand command)
        {
            var property = _context.Properties.Find(command.PropertyId);

            var appt = new Appointment
            {
                Date = System.DateTime.Parse(command.AppointmentDate + " " + command.AppointmentTime),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                BuyerUserId = command.BuyerUserId
            };

            if (property.Appointments == null)
            {
                property.Appointments = new List<Appointment>();
            }
            
            property.Appointments.Add(appt);  
            _context.SaveChanges();
        }
    }
}