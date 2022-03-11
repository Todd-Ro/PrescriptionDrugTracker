using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrescriptionDrugTracker.Models;
using PrescriptionTracker.Data;
using PrescriptionTracker.Models;
using PrescriptionTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PrescriptionDrugTracker.Controllers
{
    public class PrescriptionController : Controller
    {
        static List<Drug> AllDrugs;
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



        //This List ultimately needs to be replaced with the prescriptions for the current user
        //static List<Prescription> SelectedDrugs = new List<Prescription>();
        //static List<int> SelectedDrugIds = new List<int>();

        [Authorize]
        [HttpGet]
        [HttpPost]
        public IActionResult MySelected(string drugname, string[] drugnames)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            //TODO: Ensure that only the current user's drugs are included
            ViewBag.mydruglist = pContext.PrescriptionSet
                .Where(p => p.PatientId.Equals(currentUserId))
                .ToList();
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult SelectMeds()
        {
            pContext.InitializeDrugLibrary();
            AddPrescriptionViewModel addPrescriptionViewModel =
                new AddPrescriptionViewModel(pContext.DrugSet.ToList());
            ViewBag.mydruglist = pContext.PrescriptionSet.ToList();
            return View(addPrescriptionViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult SelectMeds(Prescription newPrescription, 
            AddPrescriptionViewModel addPrescriptionViewModel)
        {
            pContext.InitializeDrugLibrary();

            if (ModelState.IsValid)
            {
                Drug prescribedDrug = pContext.DrugSet.Find(newPrescription.TheDrugPrescribedId);
                newPrescription.DrugName = prescribedDrug.DrugName;
                newPrescription.Tier = prescribedDrug.Tier;
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                newPrescription.PatientId = currentUserId;

                List<Prescription> alreadyPrescribedSameDrug = pContext.PrescriptionSet
                    .Where(presc => presc.TheDrugPrescribedId == newPrescription.TheDrugPrescribedId)
                    .Where(presc => presc.PatientId.Equals(newPrescription.PatientId))
                    .ToList();

                if (alreadyPrescribedSameDrug.Count == 0)
                {
                    //SelectedDrugs.Add(newPrescription);
                    //SelectedDrugIds.Add(newPrescription.TheDrugPrescribedId);
                    
                    pContext.Add(newPrescription);
                    pContext.SaveChanges();
                }
                Prescription.NextId = newPrescription.Id + 1;
                return Redirect("/Prescription/MySelected");
            }
            else
            {
                return View(addPrescriptionViewModel);
            }
        }



        //static User DefaultUser = new User();
        //static User CurrentUser = DefaultUser;

        [Authorize]
        [HttpGet]
        [Route("/prescription/setexpiration/{drugname?}")]
        public IActionResult SetExpiration(string drugname)
        {
            ViewBag.drugname = drugname;
            return View();
        }

        //TODO: Check what happens if we try to set duration when already set: DONE
        [Authorize]
        [HttpPost]
        [Route("/prescription/processsetexpiration/{drugname?}")]
        public IActionResult ProcessSetExpiration(string drugname, string lastfilled, int daysoffill)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            DateTime LastFillDate = ParseDateString(lastfilled);
            DateTime RefillDue = LastFillDate.AddDays(daysoffill);
            Expiration newExpiration = new Expiration(drugname, RefillDue, currentUserId); 
            pContext.ExpirationSet.Add(newExpiration);
            pContext.SaveChanges();

            return Redirect("/Prescription/MySelected");
        }

        [Authorize]
        public IActionResult ViewUserProfile(string sortpreference="True")
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            //TODO: create method for getting currentUserId
            //TODO: Make less janky
            List<Expiration> thisUserPrescriptionsWithExpirations =
                pContext.ExpirationSet
                .Where(exp => exp.PatientId.Equals(currentUserId))
                .ToList();
            List<String> drugNames = new List<String>();
            List<String> expiryStrings = new List<String>();
            List<DateTime> expiryDates = new List<DateTime>();
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "yyyy-MM(MMM)-dd";
            foreach (Expiration e in thisUserPrescriptionsWithExpirations)
            {
                drugNames.Add(e.DrugName);
                //The Expiration class has a DateTime field called RefillDueDate.
                expiryStrings.Add(e.RefillDueDate.ToString(dtfi));
                expiryDates.Add(e.RefillDueDate);
            }

            Dictionary<String, String> userDrugInfo = new Dictionary<string, string>();
            if (sortpreference.Equals("True"))
            {
                for(int i = 0; i < drugNames.Count; i++)
                {
                    userDrugInfo[expiryStrings[i]] = drugNames[i];
                }
                expiryStrings.Sort();
                expiryDates.Sort();
                for (int i = 0; i < drugNames.Count; i++)
                {
                    drugNames[i] = userDrugInfo[expiryStrings[i]];
                }
            }
            else
            {
                for (int i = 0; i < drugNames.Count; i++)
                {
                    userDrugInfo[drugNames[i]] = expiryStrings[i];
                }
                drugNames.Sort();
                for (int i = 0; i < drugNames.Count; i++)
                {
                    expiryStrings[i] = userDrugInfo[drugNames[i]];
                    expiryDates[i] = DateTime.Parse(
                        RemoveParentheticals(userDrugInfo[drugNames[i]]) );
                }
            }
            ViewBag.usermeds = drugNames;
            ViewBag.userexpiries = expiryStrings;
            ViewBag.userexpirydates = expiryDates;

            return View();
        }

        [Authorize]
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
            pContext.InitializeDrugLibrary();
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


        public static string RemoveParentheticals(string s)
        {
            StringBuilder sb = new StringBuilder();
            int openParens = 0;
            for(int i=0; i<s.Length; i++)
            {
                char c = s[i];
                if (c == '(')
                {
                    openParens++;
                }
                else if (c == ')')
                {
                    openParens--;
                }
                else if (openParens == 0)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }


    }
}
