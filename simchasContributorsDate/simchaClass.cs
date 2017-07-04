using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simchasContributorsData
{
    public class simchaClass
    {
        public int simchaId { get; set; }
        public string simchaName { get; set; }
        public DateTime simchaDate { get; set; }

        public int simchaContributionsCount { get; set; }
        public decimal simchaTotalMoney { get; set; }
    }
}
