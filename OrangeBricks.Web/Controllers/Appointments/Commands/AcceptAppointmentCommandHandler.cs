using System;
using OrangeBricks.Web.Controllers.Offers.Commands;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Appointments.Commands
{
    public class StatusAppointmentCommandHandler
    {
        private readonly IOrangeBricksContext _context;

        public StatusAppointmentCommandHandler(IOrangeBricksContext context)
        {
            _context = context;
        }

        public void Handle(StatusAppointmentCommand command)
        {
            var appointment = _context.Appointments.Find(command.AppointmentId);

            appointment.UpdatedAt = DateTime.Now;
            appointment.Status = command.NewAppointmentStatus;

            _context.SaveChanges();
        }
    }

}