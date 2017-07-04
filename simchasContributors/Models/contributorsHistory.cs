using simchasContributorsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace simchasContributors.Models
{
    public class contributorsHistory
    {
        public PeopleClass person { get; set; }
        public IEnumerable<DepositClass> deposit { get; set; }
    }
}