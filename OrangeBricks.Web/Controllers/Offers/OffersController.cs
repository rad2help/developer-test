using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OrangeBricks.Web.Attributes;
using OrangeBricks.Web.Controllers.Offers.Builders;
using OrangeBricks.Web.Controllers.Offers.Commands;
using OrangeBricks.Web.Controllers.Property.ViewModels;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Offers
{

    public class OffersController : Controller
    {
        private readonly IOrangeBricksContext _context;

        public OffersController(IOrangeBricksContext context)
        {
            _context = context;
        }

        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult OnProperty(int id)
        {
            var builder = new OffersOnPropertyViewModelBuilder(_context);
            var viewModel = builder.Build(id);

            return View(viewModel);
        }

        [HttpPost]
        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult Accept(AcceptOfferCommand command)
        {
            var handler = new AcceptOfferCommandHandler(_context);

            handler.Handle(command);

            return RedirectToAction("OnProperty", new {id = command.PropertyId});
        }

        [HttpPost]
        [OrangeBricksAuthorize(Roles = "Seller")]
        public ActionResult Reject(RejectOfferCommand command)
        {
            var handler = new RejectOfferCommandHandler(_context);

            handler.Handle(command);

            return RedirectToAction("OnProperty", new {id = command.PropertyId});
        }


        [HttpGet]
        [OrangeBricksAuthorize(Roles = "Buyer")]
        public ActionResult MyOffers()
        {
            var builder = new MyOffersViewModelBuilder(_context);
            var viewModel = builder.Build(User.Identity.GetUserId());

            return View(viewModel);
        }

    }

    public class MyOffersViewModelBuilder
    {
        private IOrangeBricksContext _context;

        public MyOffersViewModelBuilder(IOrangeBricksContext context)
        {
            _context = context;
        }

        public MyOffersViewModel Build(string userId)
        {

            var myOffersViewModel = new MyOffersViewModel
            {
                Offers = _context.Offers
                    .Where(p => p.BuyerUserId == userId)
                    .Select(p => new MyOfferViewModel
                    {
                        OfferId = p.Id,
                        OfferAmount = p.Amount,
                        OfferStatus = p.Status,
                        OfferCreatedAt = p.CreatedAt,
                        Property = new PropertyViewModel()
                        {
                           PropertyType = p.Property.PropertyType,
                           Description = p.Property.Description,
                           NumberOfBedrooms = p.Property.NumberOfBedrooms,
                           StreetName = p.Property.StreetName
                        }
                    })
                    .ToList()
            };
            return myOffersViewModel;

        }

      
    }

    public class MyOffersViewModel
    {
        public List<MyOfferViewModel> Offers { get; set; }
    }

    public class MyOfferViewModel
    {
        public int OfferId { get; set; }

        public int OfferAmount { get; set; }

        public OfferStatus OfferStatus { get; set; }

        public PropertyViewModel Property { get; set; }
        public DateTime OfferCreatedAt { get; set; }
    }
}
