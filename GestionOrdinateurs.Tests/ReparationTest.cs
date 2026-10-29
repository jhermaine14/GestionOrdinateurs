using GestionOrdinateurs.API;
using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.Tests;

public class ReparationTest
{
    private ParcInformatiqueContext CreerContexte()
    {
        DbContextOptions<ParcInformatiqueContext> options =
            new DbContextOptionsBuilder<ParcInformatiqueContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString()
                )
                .Options;

        return new ParcInformatiqueContext(options);
    }


    [Fact]
    public async Task CreerReparation_AjouteUneReparation()
    {
        using ParcInformatiqueContext context =
            CreerContexte();

        Ordinateur ordinateur = new Ordinateur(
            100,
            "Ordinateur Test",
            EtatOrdinateur.EnService,
            1,
            1000m,
            new Caracteristiques(
                16,
                TypeOrdinateur.Portable,
                512,
                "Dell",
                "Intel Core i5"
            )
        );

        await context.Ordinateurs.AddAsync(
            ordinateur
        );

        await context.SaveChangesAsync();


        Reparation reparation = new Reparation
        {
            OrdinateurId = ordinateur.Id,
            UnOrdinateur = ordinateur,
            Technicien = "Technicien Test",
            TypeReparation = "Réparation test",
            DateDebut = DateTime.Now,
            Statut = "En cours"
        };

        await context.Reparations.AddAsync(
            reparation
        );

        await context.SaveChangesAsync();


        Reparation? reparationEnBase =
            await context.Reparations
                .FirstOrDefaultAsync();

        Assert.NotNull(reparationEnBase);

        Assert.Equal(
            "Technicien Test",
            reparationEnBase.Technicien
        );

        Assert.Equal(
            ordinateur.Id,
            reparationEnBase.OrdinateurId
        );
    }
}