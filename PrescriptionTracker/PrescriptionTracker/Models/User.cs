using System;
using System.Collections.Generic;
using System.Linq;

namespace PrescriptionDrugTrackerImplemented.Models
{
    public class User
    {
        public Dictionary<string, DateTime> Expirations { get; set; }

        public User()
        {
            Expirations = new Dictionary<string, DateTime>();
        }

        public List<string> GetUserMedsWithExpirations()
        {
            return Expirations.Keys.ToList();
        }

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
    }
}
