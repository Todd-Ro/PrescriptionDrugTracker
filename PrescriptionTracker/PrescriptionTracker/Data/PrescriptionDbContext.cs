using Microsoft.EntityFrameworkCore;
using PrescriptionDrugTracker.Models;

namespace PrescriptionTracker.Data
{
    public class PrescriptionDbContext: DbContext
    {
        public DbSet<Prescription> PrescriptionSet { get; set; }

        public PrescriptionDbContext(DbContextOptions<PrescriptionDbContext> options) : base(options)
        {

        }
    }
}
