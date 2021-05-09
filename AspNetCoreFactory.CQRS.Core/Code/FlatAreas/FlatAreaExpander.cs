using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

// Modified from https://github.com/OdeToCode/AddFeatureFolders

namespace AspNetCoreFactory.CQRS.Core
{
    public class FlatAreaExpander : IViewLocationExpander
    {
        private readonly string _placeholder;

        public FlatAreaExpander(FlatAreaOptions options)
        {
            _placeholder = options.AreaPlaceholder;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {

        }

        public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (viewLocations == null)
            {
                throw new ArgumentNullException(nameof(viewLocations));
            }

            var controllerDescriptor = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;
            var areaName = controllerDescriptor?.Properties["area"] as string;

            foreach (var location in viewLocations)
            {
                yield return location.Replace(_placeholder, areaName);
            }
        }
    }
}
