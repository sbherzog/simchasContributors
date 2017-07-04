using simchasContributorsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace simchasContributors.Models
{
    public class homeViewModel
    {
        public IEnumerable<simchaClass> simchas { get; set; }
        public int count { get; set; }
    }
}