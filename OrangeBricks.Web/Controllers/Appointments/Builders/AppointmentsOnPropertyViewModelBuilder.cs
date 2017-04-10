using System.Data.Entity;
using System.Linq;
using OrangeBricks.Web.Controllers.Appointments.ViewModels;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Appointments.Builders
{


    namespace OrangeBricks.Web.Controllers.Offers.Builders
    {
        public class AppointmentsOnPropertyViewModelBuilder
        {
            private readonly IOrangeBricksContext _context;

            public AppointmentsOnPropertyViewModelBuilder(IOrangeBricksContext context)
            {
                _context = context;
            }

            public AppointmentsViewModel Build(int id)
            {
                var property = _context.Properties
                    .Where(p => p.Id == id)
                    .Include(x => x.Appointments)
                    .SingleOrDefault();

                var appointments = property?.Appointments
                    .Select(app => new AppointmentViewModel()
                        {
                            BuyerUserId = app.BuyerUserId,
                            CreatedAt = app.CreatedAt,
                            Date = app.Date,
                            Id = app.Id,
                            Status = app.Status,
                            UpdatedAt = app.UpdatedAt
                        }).ToList();
               
                return new AppointmentsViewModel
                {
                   Appointments = appointments,
                   Property = property
                };
            }
        }
    }
}