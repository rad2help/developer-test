using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Appointments.ViewModels
{
    public class AppointmentViewModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public AppointmentStatus Status { get; set; }
        public string BuyerUserId { get; set; }
       
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsPending => Status == AppointmentStatus.Pending;
    }

    public class AppointmentsViewModel
    {
        public List<AppointmentViewModel> Appointments { get; set; }
        public Models.Property Property { get; set; }
    }

    public class MyAppointmentsViewModel
    {
        public List<MyAppointmentViewModel> MyAppointments { get; set; }
    }

    public class MyAppointmentViewModel
    {
        public AppointmentViewModel Appointment { get; set; }
        public Models.Property Property { get; set; }
    }
}