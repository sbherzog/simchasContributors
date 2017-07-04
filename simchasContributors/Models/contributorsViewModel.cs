using simchasContributorsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace simchasContributors.Models
{
    public class contributorsViewModel
    {
        public IEnumerable<PeopleClass> people { get; set; }
        public decimal total { get; set; }
    }
}