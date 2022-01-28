using Microsoft.AspNetCore.Mvc;
using PrescriptionDrugTracker.Models;
using PrescriptionTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrescriptionDrugTracker.Controllers
{
    public class PrescriptionController : Controller
    {
        List<Drug> AllDrugs;
        private ApplicationDbContext pContext;

        public PrescriptionController(ApplicationDbContext pDbContext)
        {
            pContext = pDbContext;
        }

        public IActionResult Index()
        {
            if(AllDrugs is null)
            {
                GenerateAllDrugs();
            }
            /*Since this is based on a backend list in drugs and does not need to persist user 
             * input, it does not need to use the DbContext.*/
            ViewBag.druglist = AllDrugs;
            return View();
        }

        [HttpGet]
        public IActionResult SelectMeds()
        {
            if (AllDrugs is null)
            {
                GenerateAllDrugs();
            }
            ViewBag.druglist = AllDrugs;
            return View();
        }

        static List<Prescription> SelectedDrugs = new List<Prescription>();
        static List<int> SelectedDrugIds = new List<int>();

        [HttpGet]
        [HttpPost]
        public IActionResult MySelected(string drugname, string[] drugnames)
        {
            if(!(drugname is null))
            {
                Prescription SelectedDrug = new Prescription(drugname,
                Prescription.GetDrugLibrary()[drugname]);
                int selectedDrugId = Drug.GetDrugIdByName(drugname);

                //This logic depends on how equals method works
                if (!(SelectedDrugIds.Contains(selectedDrugId)))
                {
                    SelectedDrugs.Add(SelectedDrug);
                    SelectedDrugIds.Add(selectedDrugId);
                    pContext.Add(SelectedDrug);
                    pContext.SaveChanges();
                }
            }
            if(!(drugnames is null))
            {

                foreach(string drug in drugnames)
                {
                    //TODO: Ensure this works if equality is based on Id
                    Prescription SelectedDrug = new Prescription(drug,
                    Prescription.GetDrugLibrary()[drug]);
                    int selectedDrugId = Drug.GetDrugIdByName(drugname);
                    if (!(SelectedDrugIds.Contains(selectedDrugId)))
                    {
                        SelectedDrugs.Add(SelectedDrug);
                        SelectedDrugIds.Add(selectedDrugId);
                        pContext.Add(SelectedDrug);
                    }
                }
                pContext.SaveChanges();
            }
            ViewBag.mydruglist = pContext.PrescriptionSet.ToList();
            return View();
        }

        static User DefaultUser = new User();
        static User CurrentUser = DefaultUser;

        [HttpGet]
        [Route("/prescription/setexpiration/{drugname?}")]
        public IActionResult SetExpiration(string drugname)
        {
            ViewBag.drugname = drugname;
            return View();
        }

        //TODO: Check what happens if we try to set duration when already set
        [HttpPost]
        [Route("/prescription/processsetexpiration/{drugname?}")]
        public IActionResult ProcessSetExpiration(string drugname, string lastfilled, int daysoffill)
        {
            DateTime LastFillDate = ParseDateString(lastfilled);
            DateTime RefillDue = LastFillDate.AddDays(daysoffill);
            if(!(CurrentUser.Expirations.ContainsKey(drugname)))
            {
                CurrentUser.Expirations.Add(drugname, RefillDue);
            }
            
            return Redirect("/Prescription/MySelected");
        }

        public IActionResult ViewUserProfile()
        {
            ViewBag.usermeds = CurrentUser.GetUserMedsWithExpirations();
            ViewBag.userexpiries = CurrentUser.GetUserMedExpirations();

            return View();
        }

        public IActionResult SelectMultiple()
        {
            if (AllDrugs is null)
            {
                GenerateAllDrugs();
            }
            //Simply a list of medications, regardless of whether they have been selected
            ViewBag.druglist = AllDrugs;
            return View();
        }

        public List<Drug> GenerateAllDrugs()
        {
            List<Drug> ret = Drug.InitializeDrugs();
            AllDrugs = ret;
            return ret;
        }

        public DateTime ParseDateString(string iso)
        {
            int year = int.Parse(iso.Substring(0, 4));
            int month = int.Parse(iso.Substring(5, 2));
            int day = int.Parse(iso.Substring(8));
            DateTime LastFillDate = new DateTime(year, month, day);
            return LastFillDate;
        }





    }
}
