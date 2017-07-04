using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simchasContributorsData
{
    public class ContributorsSimchaClass : PeopleClass
    {
       public int simchaID { get; set; }

        public bool Contribute { get; set; }

        public decimal amount { get; set; }
        public string ContributeBool { get; set; }
    }
}
