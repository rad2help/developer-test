using System;

namespace OrangeBricks.Web.Controllers.Property.Commands
{
    public class RequestAppointmentCommand
    {
        public int PropertyId { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string BuyerUserId { get; set; }
    }
}