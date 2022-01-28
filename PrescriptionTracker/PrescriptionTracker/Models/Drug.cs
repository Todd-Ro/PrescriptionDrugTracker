using PrescriptionDrugTracker.Models;
using System.Collections.Generic;

namespace PrescriptionDrugTracker.Models
{
    public class Drug
    {
        static Dictionary<string, int> DrugLibrary =
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
        };

        public static Dictionary<string, int> GetDrugLibrary()
        {
            return DrugLibrary;
        }

        private static Dictionary<string, int> DrugIdLibrary =
            new Dictionary<string, int>()
        {
                {"Amoxicillin", 1 },
                {"Penicillin", 2 },
                {"Econazole", 3 },
                {"Clonidine", 4 },
                {"Azithromycin", 5 },
                {"Lisinopril", 6 },
                {"Propranolol", 7 },
                {"Lovastatin", 8 },
                {"Niacin", 9 },
                {"Omega-3", 10 },
                {"Provastatin", 11 }
        };

        private static Dictionary<int, string> DrugByIdLibrary =
            new Dictionary<int, string>()
        {
                {1, "Amoxicillin" },
                {2, "Penicillin"},
                {3, "Econazole"},
                {4, "Clonidine"},
                {5, "Azithromycin"},
                {6, "Lisinopril"},
                {7, "Propranolol"},
                {8, "Lovastatin"},
                {9, "Niacin"},
                {10, "Omega-3"},
                {11, "Provastatin"}
        };

        public string DrugName { get; set; }
        public int Tier { get; set; }
        public int Id { get; set; }
        public static List<Drug> AllDrugs { get; set; }
        public static string GetDrugNameById(int drugId)
        {
            return DrugByIdLibrary[drugId];
        }

        public static int GetDrugIdByName(string drugName)
        {
            return DrugIdLibrary[drugName];
        }

        //TODO: PrescriptionController's List<Prescription> AllDrugs should be moved here,
        // as should the functionality of this method:
        /* 
        public List<Prescription> GenerateAllDrugs()
        {
            Dictionary<string, int> DrugsDict = Prescription.GetDrugLibrary();
            List<Prescription> ret = new List<Prescription>();
            foreach(string s in DrugsDict.Keys)
            {
                ret.Add(new Prescription(s, DrugsDict[s]));
            }
            AllDrugs = ret;
            return ret;
        }
        */

        public Drug(string drugName, int tier)
        {
            DrugName = drugName;
            Tier = tier;
            Id = DrugIdLibrary[drugName];
        }

        public static List<Drug> InitializeDrugs()
        {
            List<Drug> ret;
            if(AllDrugs is null)
            {
                ret = new List<Drug>();
                foreach (string s in DrugLibrary.Keys)
                {
                    ret.Add(new Drug(s, DrugLibrary[s]));
                }
                AllDrugs = ret;
            }
            else
            {
                ret = AllDrugs;
            }
            Prescription.DrugsInitialized = true;
            return ret;
        }
    }
}
