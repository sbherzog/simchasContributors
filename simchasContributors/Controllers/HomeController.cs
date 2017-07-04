using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using simchasContributorsData;
using simchasContributors.Models;

namespace simchasContributors.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            simchasContributorsManager manager = new simchasContributorsManager(Properties.Settings.Default.strCon);
            homeViewModel vm = new homeViewModel();
            vm.simchas = manager.GetAllSimchas();
            vm.count = manager.GetAmountPeoplre();
            return View(vm);
        }

        public ActionResult addNewSimcha(simchaClass simcha)
        {
            simchasContributorsManager manager = new simchasContributorsManager(Properties.Settings.Default.strCon);
            manager.AddNewSimcha(simcha);
            TempData["successful"] = $"The simcha {simcha.simchaName} on {simcha.simchaDate.ToShortDateString()} was successful added to your list";
            return Redirect("/");
        }

        public ActionResult contributors()
        {
            simchasContributorsManager manager = new simchasContributorsManager(Properties.Settings.Default.strCon);
            contributorsViewModel vm = new contributorsViewModel();
            vm.people = manager.GetAllContributors();
            vm.total = manager.Total();
            return View(vm);
        }

        [HttpPost]
        public ActionResult contributorsHistory(int id)
        {
            simchasContributorsManager manager = new simchasContributorsManager(Properties.Settings.Default.strCon);
            contributorsHistory vm = new contributorsHistory();
            vm.deposit = manager.GetHistory(id);
            vm.person = manager.GetPersonByID(id);
            return View(vm);
        }

        public ActionResult addNewContributors(PeopleClass p, DepositClass d)
        {
            simchasContributorsManager manager = new simchasContributorsManager(Properties.Settings.Default.strCon);
            d.DepositpersonId = manager.addNewContributors(p);
            manager.addNewDeposit(d);
            TempData["successful"] = $"{p.firstName} {p.lastName} with the deposit of: {d.DepositAmount} was successful added to your list";
            return Redirect("/Home/contributors/");
        }


        public ActionResult EditPerson(PeopleClass p)
        {
            simchasContributorsManager manager = new simchasContributorsManager(Properties.Settings.Default.strCon);
             manager.UpdateContributors(p);
            TempData["successful"] = $"{p.firstName} {p.lastName} was successful Update";
            return Redirect("/Home/contributors/");
        }

        

        public ActionResult addNewDeposit(DepositClass d)
        {
            simchasContributorsManager manager = new simchasContributorsManager(Properties.Settings.Default.strCon);
            manager.addNewDeposit(d);
            TempData["successful"] = $"The deposit of: {d.DepositAmount} was successful added to your list";
            return Redirect("/Home/contributors/");
        }

        public ActionResult simchasCcontributions(int simchaId)
        {
            simchasContributorsManager manager = new simchasContributorsManager(Properties.Settings.Default.strCon);
            simchasCcontributions vm = new simchasCcontributions();
            vm.ContributorsSimcha = manager.GetContributorsSimcha(simchaId);
            vm.simchaName = manager.GetSimchaName(simchaId);
            vm.simchaAmount = manager.GetSimchaAmount(simchaId);
            vm.simchaID = simchaId;
            return View(vm);
        }

        public ActionResult AddNewContributorsSimcha(IEnumerable<ContributorsSimchaClass> c, int SID, string SName)
        {
            simchasContributorsManager manager = new simchasContributorsManager(Properties.Settings.Default.strCon);
            manager.DeletContributions(SID);
            manager.Contributions(c);
            TempData["successful"] = $"Contributions for the {SName} simcha have been updated successfully ";
            return Redirect("/Home/");
        }


        public ActionResult PrintContributions(int simchaId)
        {
            simchasContributorsManager manager = new simchasContributorsManager(Properties.Settings.Default.strCon);
            simchasCcontributions vm = new simchasCcontributions();
            vm.ContributorsSimcha = manager.GetContributorsSimcha(simchaId);
            vm.simchaName = manager.GetSimchaName(simchaId);
            return View(vm);
        }       

    }
}