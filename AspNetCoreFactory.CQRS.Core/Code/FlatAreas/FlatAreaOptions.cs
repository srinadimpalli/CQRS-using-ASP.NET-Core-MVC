using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

// Modified from https://github.com/OdeToCode/AddFeatureFolders

namespace AspNetCoreFactory.CQRS.Core
{
    /// <summary>
    /// Options to control the behavior of feature folders
    /// </summary>
    public class FlatAreaOptions
    {
        public FlatAreaOptions()
        {
            RootFolderName = "Areas";
            DeriveFolderName = null;
            AreaPlaceholder = "{Areas}";
        }

        /// <summary>
        /// The name of the root area folder on disk (default: 'Areas')
        /// </summary>
        public string RootFolderName { get; set; }

        /// <summary>
        /// Given a ControllerModel object, returns the path to the feature folder.
        /// Only set this property if you want to override the default logic.
        /// The default logic takes the namespace of a Controller and assumes the 
        /// namespace maps to a folder. Examples:
        ///     Project.Name.Features.Admin.ManageUsers -> Features\Admin\ManageUsers
        ///     Project.Name.Features.Admin -> Features\Admin
        /// Note the name "Features" is set by the FeatureFolderName property. 
        /// </summary>
        public Func<ControllerModel, string> DeriveFolderName { get; set; }


        /// <summary>
        /// Used internally in RazorOptions.ViewLocationFormats strings. The Default is {Feature},
        /// so the first format string in Razor options will be {Feature}\{0}.cshtml. Razor places 
        /// the view name into the {0} placeholder, the FeatureViewLocationExander class in this project
        /// replaces {feature} with the feature path derived from the ControllerModel
        /// </summary>
        public string AreaPlaceholder { get; set; }
    }
}
