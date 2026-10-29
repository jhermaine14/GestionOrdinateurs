using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.API;

public class OrdinateurService : IOrdinateurService
{
    private readonly ParcInformatiqueContext _context;

    private readonly JournalService _journalService;

    public event EventHandler<OrdinateurAjouteEventArgs>? OrdinateurAjoute;
    public event EventHandler<OrdinateurSupprimeEventArgs>? OrdinateurSupprime;
    public event EventHandler<OrdinateurMisAJourEventArgs>? OrdinateurMisAJour;

    public OrdinateurService(ParcInformatiqueContext context, JournalService journalService)
    {
        _context = context;
        _journalService = journalService;
        OrdinateurAjoute += AfficherMessageAjout;
        OrdinateurSupprime += AfficherMessageSupprime;
        OrdinateurMisAJour += AfficherMessageMisAJour;
    }

    private void AfficherMessageAjout(object? sender, OrdinateurAjouteEventArgs e)
    {
        Console.WriteLine($"Ordinateur ajouté : {e.Id} - {e.Nom}");
    }

    private void AfficherMessageSupprime(object? sender, OrdinateurSupprimeEventArgs e)
    {
        Console.WriteLine($"Ordinateur supprimé : {e.Id} - {e.Nom}");
    }

    private void AfficherMessageMisAJour(object? sender, OrdinateurMisAJourEventArgs e)
    {
        Console.WriteLine($"Ordinateur mis à jour : {e.Id} - {e.Nom}");
    }


    public async Task<List<Ordinateur>> ListerOrdinateursAsync()
    {
        return await _context.Ordinateurs.ToListAsync();
    }


    public async Task<Ordinateur?> GetOrdinateurParIdAsync(int id)
    {
        return await _context.Ordinateurs
            .FirstOrDefaultAsync(o => o.Id == id);
    }


    public async Task<bool> AjouterOrdinateurAsync(
    Ordinateur ordinateur)
    {
        bool existe = await _context.Ordinateurs
            .AnyAsync(o => o.Id == ordinateur.Id);

        if (existe)
        {
            return false;
        }

        await _context.Ordinateurs.AddAsync(ordinateur);
        await _context.SaveChangesAsync();

        _journalService.EcrireOrdinateur(
            $"{DateTime.Now:dd/MM/yyyy} - Ordinateur ajouté : {ordinateur.Nom}"
        );

        OrdinateurAjoute?.Invoke(
            this,
            new OrdinateurAjouteEventArgs
            {
                Id = ordinateur.Id,
                Nom = ordinateur.Nom
            }
        );

        return true;
    }


    public async Task<bool> SupprimerOrdinateurAsync(int id)
    {
        Ordinateur? ordinateur =
            await _context.Ordinateurs
                .FirstOrDefaultAsync(o => o.Id == id);

        if (ordinateur == null)
        {
            return false;
        }

        _context.Ordinateurs.Remove(ordinateur);
        await _context.SaveChangesAsync();

        _journalService.EcrireOrdinateur(
            $"{DateTime.Now:dd/MM/yyyy} - Ordinateur supprimé : {ordinateur.Nom}"
        );

        OrdinateurSupprime?.Invoke(
            this,
            new OrdinateurSupprimeEventArgs
            {
                Id = ordinateur.Id,
                Nom = ordinateur.Nom
            }
        );

        return true;
    }

    public async Task<bool> MettreAJourOrdinateurAsync(
    int id,
    Ordinateur nouvellesInformations)
    {
        Ordinateur? ordinateur =
            await _context.Ordinateurs
                .FirstOrDefaultAsync(o => o.Id == id);

        if (ordinateur == null)
        {
            return false;
        }

        ordinateur.ModifierInformations(
        nouvellesInformations.Nom,
        nouvellesInformations.Etat,
        nouvellesInformations.Stock,
        nouvellesInformations.Prix,
        nouvellesInformations.Caracteristiques
        );

        await _context.SaveChangesAsync();

        _journalService.EcrireOrdinateur(
            $"{DateTime.Now:dd/MM/yyyy} - Ordinateur modifié : {ordinateur.Nom}"
        );

        OrdinateurMisAJour?.Invoke(
            this,
            new OrdinateurMisAJourEventArgs
            {
                Id = ordinateur.Id,
                Nom = ordinateur.Nom
            }
        );

        return true;
    }

    public async Task<List<Ordinateur>> GetOrdinateursDisponiblesAsync()
    {
        return await _context.Ordinateurs
            .Where(o => o.Stock > 0)
            .ToListAsync();
    }

    public async Task<List<Ordinateur>> GetOrdinateursParMarqueAsync(
        string marque)
    {
        return await _context.Ordinateurs
            .Where(o => o.Caracteristiques.Marque == marque)
            .ToListAsync();
    }

    public async Task<List<Ordinateur>> GetOrdinateursParPrixAsync()
    {
        List<Ordinateur> ordinateurs =
            await _context.Ordinateurs.ToListAsync();

        return ordinateurs
            .OrderByDescending(o => o.Prix)
            .ToList();
    }


    public string Diagnostiquer(Ordinateur ordinateur)
    {
        return ordinateur switch
        {
            {
                Etat: EtatOrdinateur.HorsService
            }
                => "Remplacement",

            {
                Etat: EtatOrdinateur.Lent,
                Caracteristiques:
                {
                    RamGo: < 8
                }
            }
                => "Ajouter RAM",

            _
                => "Aucune action"
        };
    }


    public (
    int TotalOrdinateur,
    int OrdiDisponibles,
    decimal ValeurStock,
    int NbReparations)
    CalculerStatistiques()
    {
        int totalOrdinateur =
            _context.Ordinateurs.Count();

        int ordiDisponibles =
            _context.Ordinateurs.Count(
                o => o.Stock > 0
            );

        decimal valeurStock =
            _context.Ordinateurs
                .AsEnumerable()
                .Sum(o => o.Prix * o.Stock);

        int nbReparations =
            _context.Reparations.Count();

        return (
            totalOrdinateur,
            ordiDisponibles,
            valeurStock,
            nbReparations
        );
    }
}