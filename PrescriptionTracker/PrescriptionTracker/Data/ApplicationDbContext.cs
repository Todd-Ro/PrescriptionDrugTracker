using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrescriptionDrugTracker.Models;
using System;
using System.Collections.Generic;
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
    }
}
