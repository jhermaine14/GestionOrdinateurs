using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.API;

public class ParcInformatiqueContext : DbContext
{
    public DbSet<Ordinateur> Ordinateurs { get; set; }

    public DbSet<Reparation> Reparations { get; set; }

    public DbSet<User> Users { get; set; }


    public ParcInformatiqueContext()
    {
    }


    public ParcInformatiqueContext(
        DbContextOptions<ParcInformatiqueContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data source=parcinfo.db");
        }
    }

    protected override void OnModelCreating(
    ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ordinateur>()
            .HasKey(o => o.Id);

        modelBuilder.Entity<Ordinateur>()
            .OwnsOne(o => o.Caracteristiques);

        modelBuilder.Entity<Ordinateur>()
            .HasMany(o => o.Reparations)
            .WithOne(r => r.UnOrdinateur)
            .HasForeignKey(r => r.OrdinateurId);
            
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();
    }


}