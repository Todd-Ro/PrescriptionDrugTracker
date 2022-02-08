using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrescriptionDrugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrescriptionTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Prescription> PrescriptionSet { get; set; }
        public DbSet<Drug> DrugSet { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }





        private bool previouslyFoundHighDrugIndex = false;
        private bool existsHighDrugIndex()
        {
            List<Drug> listToSearch = DrugSet.ToList();
            foreach (Drug d in listToSearch)
            {
                if (d.Id >= (Drug.LibraryCount))
                {
                    previouslyFoundHighDrugIndex = true;
                    return true;
                }
            }
            return false;
        }

        static private List<Drug> Drugs;
        static List<Drug> StarterDrugs = Drug.InitializeDrugs();


        public void InitializeDrugLibrary()
        {
            if (Drugs is null)
            {
                if (!previouslyFoundHighDrugIndex)
                {
                    if (!existsHighDrugIndex())
                    {
                        Drugs = new List<Drug>();
                        for (int i = 0; i < Drug.LibraryCount; i++)
                        {
                            Drug anInitialDrug = new Drug
                            {
                                DrugName = StarterDrugs[i].DrugName,
                                Tier = StarterDrugs[i].Tier
                            };
                            /*if (aStarterEvent.Id == (2+Event.StarterIndex))
                            {
                                aStarterEvent = new Event(StarterEvents[i],
                                StarterDescriptions[i], "somewhere", "tifferthecool@aol.com",
                                32, e, true);
                            }*/
                            Drugs.Add(anInitialDrug);
                            DrugSet.Add(anInitialDrug);
                        }
                        this.SaveChanges();
                    }
                }

            }
        }




    }
}
