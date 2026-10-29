namespace GestionOrdinateurs.Shared;

public class InventaireOrdinateurs
{
    public List<Ordinateur> Ordinateurs { get; set; } = new();


    public Ordinateur? this[int ordinateurId]
    {
        get
        {
            return Ordinateurs
                .FirstOrDefault(o => o.Id == ordinateurId);
        }
    }


    public Ordinateur? this[string nom]
    {
        get
        {
            return Ordinateurs
                .FirstOrDefault(o => o.Nom == nom);
        }
    }
}