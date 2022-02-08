using Microsoft.AspNetCore.Identity;
using PrescriptionDrugTracker.Models;
using System;
using System.Collections.Generic;

namespace PrescriptionDrugTracker.Models
{
    public class Prescription
    {
        /*static Dictionary<string, int> DrugLibrary =
            new Dictionary<string, int>()
            {
                {"Amoxicillin", 1 },
                {"Penicillin", 1 },
                {"Econazole", 3 },
                {"Clonidine", 1 },
                {"Azithromycin", 1 },
                {"Lisinopril", 1 },
                {"Propranolol", 1 },
                {"Lovastatin", 1 },
                {"Niacin", 3 },
                {"Omega-3", 3 },
                {"Provastatin", 1 }
            };*/

        public string DrugName { get; set; }
        public int Tier { get; set; }
        public int Id { get; set; }
        public Guid ExtendedIdent { get; set; }

        public int TheDrugPrescribedId { get; set; }
        public Drug TheDrugPrescribed { get; set; }

        public string PatientId { get; set; }
        public IdentityUser Patient { get; set; }

        public static int StarterIndex { get; } = 1;
        internal static int NextId { get; set; } = StarterIndex;


        //TODO: Make Expiration information that is set after construction
        

        public static bool DrugsInitialized { get; set; } = false;


        static public Dictionary<string, int> GetDrugLibrary()
        {
            return Drug.GetDrugLibrary();
        }

        public override bool Equals(object obj)
        {
            return obj is Prescription prescription &&
                   Id == prescription.Id &&
                   ExtendedIdent.Equals(prescription.ExtendedIdent);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, ExtendedIdent);
        }

        public Prescription()
        {
            if (Id == 0)
            {
                ExtendedIdent = GuidMethods.makeId(NextId);
            }
            else
            {
                ExtendedIdent = GuidMethods.makeId(Id);
            }
        }

        public Prescription(string drugName, int tier): this()
        {
            DrugName = drugName;
            Tier = tier;
            
            TheDrugPrescribedId = Drug.GetDrugIdByName(drugName);
            NextId++;
        }







    }
}
