using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

public interface IOrdinateurService
{

    Task<List<Ordinateur>> ListerOrdinateursAsync();

    Task<Ordinateur?> GetOrdinateurParIdAsync(int id);

    Task<bool> AjouterOrdinateurAsync(Ordinateur ordinateur);

    Task<bool> SupprimerOrdinateurAsync(int id);

    Task<bool> MettreAJourOrdinateurAsync(
        int id,
        Ordinateur nouvellesInformations);

    Task<List<Ordinateur>> GetOrdinateursDisponiblesAsync();

    Task<List<Ordinateur>> GetOrdinateursParMarqueAsync(
        string marque);

    Task<List<Ordinateur>> GetOrdinateursParPrixAsync();

    
    string Diagnostiquer(Ordinateur ordinateur);

    (
    int TotalOrdinateur,
    int OrdiDisponibles,
    decimal ValeurStock,
    int NbReparations)
    CalculerStatistiques();
}