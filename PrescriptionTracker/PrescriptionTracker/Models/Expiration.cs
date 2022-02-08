using Microsoft.AspNetCore.Identity;
using PrescriptionDrugTracker.Models;
using System;

namespace PrescriptionTracker.Models
{
    public class Expiration
    {
        public int Id { get; set; }
        public string DrugName { get; set; }
        public DateTime RefillDueDate { get; set; }
        public string PatientId { get; set; }
        public IdentityUser Patient { get; set; }
        //public int PrescriptionId { get; set; }
        //public Prescription ThisDrug { get; set; }

        public Expiration() { }
        public Expiration(string drugName, DateTime refillDueDate, string patientId) { 
            DrugName = drugName;
            RefillDueDate = refillDueDate;
            PatientId = patientId;
        }

        /* 
         *public Dictionary<string, DateTime> Expirations { get; set; }

        public User()
        {
            Expirations = new Dictionary<string, DateTime>();
        }

        // Needs to be replaced with use of context and .where
        public List<string> GetUserMedsWithExpirations()
        {
            return Expirations.Keys.ToList();
        }

        // Needs to be replaced with use of context and .where
        public List<string> GetUserMedExpirations()
        {
            List<string> meds = GetUserMedsWithExpirations();
            List<string> ret = new List<string>();
            foreach(string med in meds)
            {
                ret.Add(Expirations[med].ToString());
            }
            return ret;
        }  
         */
    }
}
