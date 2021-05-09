using System;
using System.Collections.Generic;

namespace AspNetCoreFactory.CQRS.Core.Domain
{
    public partial class Traveler
    {
        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
