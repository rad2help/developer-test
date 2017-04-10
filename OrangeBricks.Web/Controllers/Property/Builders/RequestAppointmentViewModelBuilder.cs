using System;
using OrangeBricks.Web.Controllers.Property.ViewModels;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Property.Builders
{
    public class RequestAppointmentViewModelBuilder
    {
        private readonly IOrangeBricksContext _context;

        public RequestAppointmentViewModelBuilder(IOrangeBricksContext context)
        {
            _context = context;
        }

        public RequestAppointmentViewModel Build(int id,string userId)
        {
            var property = _context.Properties.Find(id);

            var retModel = new RequestAppointmentViewModel
            {
                PropertyId = property.Id,
                PropertyType = property.PropertyType,
                StreetName = property.StreetName                
            };
            return retModel;
        }
    }
}