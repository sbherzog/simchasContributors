using simchasContributorsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace simchasContributors.Models
{
    public class simchasCcontributions
    {
        public IEnumerable<ContributorsSimchaClass> ContributorsSimcha { get; set; }
        public string simchaName { get; set; }
        public int simchaID { get; set; }
        public decimal? simchaAmount { get; set; }
    }
}