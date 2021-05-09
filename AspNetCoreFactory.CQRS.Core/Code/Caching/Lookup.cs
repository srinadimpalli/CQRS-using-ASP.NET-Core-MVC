using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AspNetCoreFactory.CQRS.Core
{
    #region Interface

    public interface ILookup
    {
        List<SelectListItem> PlaneItems { get; }
        List<SelectListItem> TravelerItems { get; }
        List<SelectListItem> LocationItems { get; }
        List<SelectListItem> FlightItems { get; }
    }

    #endregion

    public class Lookup : ILookup
    {
        #region Dependency Injection

        private readonly ICache _cache;

        public Lookup(ICache cache)
        {
            _cache = cache;
        }

        #endregion

        #region Items

        // Dropdown selection list for plane items

        public List<SelectListItem> PlaneItems
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem { Value = "", Text = "-- Select --", Selected = true });
                foreach (var plane in _cache.Planes.Values)
                    list.Add(new SelectListItem { Value = plane.Id.ToString(), Text = plane.Model + ": " + plane.Name });

                return list;
            }
        }

        // Dropdown selection list for traveler items

        public List<SelectListItem> TravelerItems
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem { Value = "", Text = "-- Select --", Selected = true });
                foreach (var traveler in _cache.Travelers.Values)
                    list.Add(new SelectListItem { Value = traveler.Id.ToString(), Text = traveler.Name });

                return list;
            }
        }

        // Dropdown selection list for flight items


        public List<SelectListItem> FlightItems
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem { Value = "", Text = "-- Select --", Selected = true });
                foreach (var flight in _cache.Flights.Values)
                    list.Add(new SelectListItem { Value = flight.Id.ToString(), Text = flight.FlightNumber + ": " + flight.From +  " - " + flight.To });

                return list;
            }
        }

        // Dropdown selection list for Location

        public List<SelectListItem> LocationItems
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- Select --", Selected = true },
                    new SelectListItem { Value = "Window", Text = "Window" },
                    new SelectListItem { Value = "Isle", Text = "Isle" }
                };
            }
        }

        #endregion
    }
}
