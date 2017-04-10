using System;

namespace OrangeBricks.Web.Controllers.Property.ViewModels
{
    public class RequestAppointmentViewModel
    {
        public string PropertyType { get; set; }
        public string StreetName { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime{ get; set; }
        public int PropertyId { get; set; }
    }
}