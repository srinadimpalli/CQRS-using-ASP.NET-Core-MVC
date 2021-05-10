using System;
using System.Collections.Generic;

namespace AspNetCoreFactory.Domain.Entities
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
