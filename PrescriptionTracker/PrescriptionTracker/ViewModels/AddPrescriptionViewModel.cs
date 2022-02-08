using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrescriptionDrugTracker.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrescriptionTracker.ViewModels
{
    public class AddPrescriptionViewModel
    {
        public string DrugName { get; set; }
        public int Tier { get; set; }

        [Required(ErrorMessage ="Must have identifier for the prescribed drug.")]
        public int TheDrugPrescribedId { get; set; }

        public List<SelectListItem> Drugs { get; set; }

        public string PatientId { get; set; }
        public IdentityUser Patient { get; set; }

        public AddPrescriptionViewModel() { }

        public AddPrescriptionViewModel(List<Drug> allDrugs)
        {
            Drugs = new List<SelectListItem>();

            foreach (Drug drug in allDrugs)
            {
                Drugs.Add(
                    new SelectListItem
                    {
                        Value = drug.Id.ToString(),
                        Text = drug.DrugName
                    }
                );
            }
        }
    }
}
