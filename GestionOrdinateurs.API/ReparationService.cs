using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.API;

public class ReparationService
{
    public record HistoriqueReparationRecord(
    int OrdinateurId,
    string NomOrdinateur,
    string Technicien,
    string TypeReparation,
    DateTime DateIntervention
    );

    private readonly IDbContextFactory<ParcInformatiqueContext> _contextFactory;

    private readonly JournalService _journalService;

    public ReparationService(
        IDbContextFactory<ParcInformatiqueContext> contextFactory, JournalService journalService)
    {
        _contextFactory = contextFactory;
        _journalService = journalService;
    }


    private async Task<Reparation> CreerReparationAsync(
        ParcInformatiqueContext context,
        Ordinateur ordinateur)
    {
        Reparation reparation = new Reparation
        {
            OrdinateurId = ordinateur.Id,
            Technicien = "Technicien 1",
            TypeReparation = "Réparation test",
            DateDebut = DateTime.Now,
            Statut = "En cours"
        };

        await context.Reparations.AddAsync(reparation);
        await context.SaveChangesAsync();

        return reparation;
    }


    public async Task SimulerReparationAsync(
        Ordinateur ordinateur)
    {
        await using ParcInformatiqueContext context =
            await _contextFactory.CreateDbContextAsync();

        Reparation reparation =
            await CreerReparationAsync(
                context,
                ordinateur
            );

        await Task.Delay(2000);

        reparation.Statut = "Terminée";

        await context.SaveChangesAsync();

        _journalService.Ecrire(
            $"{DateTime.Now:dd/MM/yyyy} - {ordinateur.Nom} réparé"
        );

        Console.WriteLine(
            $"{ordinateur.Nom} réparé"
        );
    }


    public async Task<List<Reparation>> ListerReparationsAsync()
    {
        await using ParcInformatiqueContext context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Reparations
            .Include(r => r.UnOrdinateur)
            .ToListAsync();
    }

    public async Task<List<HistoriqueReparationRecord>>
    GetHistoriqueReparationsAsync()
    {
        await using ParcInformatiqueContext context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Reparations
            .Select(r => new HistoriqueReparationRecord(
                r.OrdinateurId,
                r.UnOrdinateur.Nom,
                r.Technicien,
                r.TypeReparation,
                r.DateDebut
            ))
            .ToListAsync();
    }


}