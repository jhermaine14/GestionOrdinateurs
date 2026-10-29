namespace GestionOrdinateurs.API;

public class JournalService
{
    public void Ecrire(string message)
    {
        string dossier = Path.Combine(
            "Logs",
            "Reparations"
        );

        Directory.CreateDirectory(dossier);

        using var writer = new StreamWriter(
            Path.Combine(
                dossier,
                "reparations.log"
            ),
            append: true
        );

        writer.WriteLine(message);
    }


    public void EcrireOrdinateur(string message)
    {
        string dossier = Path.Combine(
            "Logs",
            "Ordinateurs"
        );

        Directory.CreateDirectory(dossier);

        using var writer = new StreamWriter(
            Path.Combine(
                dossier,
                "ordinateurs.log"
            ),
            append: true
        );

        writer.WriteLine(message);
    }
}