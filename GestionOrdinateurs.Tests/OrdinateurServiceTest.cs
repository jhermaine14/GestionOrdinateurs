using GestionOrdinateurs.API;
using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.Tests;

public class OrdinateurServiceTest
{

    private ParcInformatiqueContext CreerContexte()
    {
        var options =
            new DbContextOptionsBuilder<ParcInformatiqueContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        return new ParcInformatiqueContext(options);
    }


    [Fact]
    public async Task AjouterOrdinateur_AjouteUnOrdinateur()
    {

        ParcInformatiqueContext context = CreerContexte();
        JournalService journalService =
            new JournalService();

        OrdinateurService service =
            new OrdinateurService(
                context,
                journalService
            );

        Ordinateur ordinateur = new Ordinateur(
            100,
            "Ordinateur Test",
            EtatOrdinateur.EnService,
            5,
            999m,
            new Caracteristiques(
                16,
                TypeOrdinateur.Fixe,
                512,
                "Dell",
                "Intel Core i5"
            )
        );


        bool resultat =
            await service.AjouterOrdinateurAsync(ordinateur);


        Assert.True(resultat);

        Assert.NotNull(
            await service.GetOrdinateurParIdAsync(100)
        );
    }


    [Fact]
    public async Task SupprimerOrdinateur_Existant_RetourneTrue()
    {

        ParcInformatiqueContext context = CreerContexte();

        JournalService journalService =
            new JournalService();

        OrdinateurService service =
            new OrdinateurService(
                context,
                journalService
            );

        Ordinateur ordinateur = new Ordinateur(
            101,
            "PC Suppression",
            EtatOrdinateur.EnService,
            3,
            500m,
            new Caracteristiques(
                8,
                TypeOrdinateur.Fixe,
                256,
                "HP",
                "Intel Core i3"
            )
        );

        await service.AjouterOrdinateurAsync(ordinateur);


        bool resultat =
            await service.SupprimerOrdinateurAsync(101);


        Assert.True(resultat);

        Assert.Null(
            await service.GetOrdinateurParIdAsync(101)
        );
    }


    [Fact]
    public async Task SupprimerOrdinateur_Inexistant_RetourneFalse()
    {

        ParcInformatiqueContext context = CreerContexte();

        JournalService journalService =
            new JournalService();

        OrdinateurService service =
            new OrdinateurService(
                context,
                journalService
            );


        bool resultat =
            await service.SupprimerOrdinateurAsync(9999);


        Assert.False(resultat);
    }
}