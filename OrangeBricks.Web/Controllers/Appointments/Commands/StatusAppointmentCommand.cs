using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Appointments.Commands
{
    public abstract class StatusAppointmentCommand
    {
        public int AppointmentId { get; set; }
        public int PropertyId { get; set; }

        public abstract AppointmentStatus NewAppointmentStatus { get; }
    }

    public class AcceptAppointmentCommand : StatusAppointmentCommand
    {       
        public override AppointmentStatus NewAppointmentStatus => AppointmentStatus.Accepted;
    }

    public class RejectAppointmentCommand: StatusAppointmentCommand {
        public override AppointmentStatus NewAppointmentStatus => AppointmentStatus.Rejected;
    }
}