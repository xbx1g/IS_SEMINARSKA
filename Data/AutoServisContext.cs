using AutoServis.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AutoServis.Data
{
    public class AutoServisContext : IdentityDbContext<ApplicationUser>
    {
        public AutoServisContext(DbContextOptions<AutoServisContext> options) : base(options) { }

        public DbSet<Vozilo> Vozila { get; set; }
        public DbSet<Rezervacija> Rezervacije { get; set; }
        public DbSet<Stranka> Stranke { get; set; }
        public DbSet<Mehanik> Mehaniki { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Rezervacija>()
                .Property(r => r.Cena)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<Vozilo>().ToTable("Vozilo");
            modelBuilder.Entity<Rezervacija>().ToTable("Rezervacija");
            modelBuilder.Entity<Stranka>().ToTable("Stranka");
            modelBuilder.Entity<Mehanik>().ToTable("Mehanik");
        }
    }
}