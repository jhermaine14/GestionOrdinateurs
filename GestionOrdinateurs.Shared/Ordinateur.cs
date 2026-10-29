using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestionOrdinateurs.Shared;

public enum EtatOrdinateur
{
    EnService,
    EnPanne,
    EnMaintenance,
    HorsService,
    Lent,
    Reparable
}

public enum TypeOrdinateur
{
    Portable,
    Fixe,
    Server
}

[Table("Ordinateur")]
public class Ordinateur
{
    public int Id { get; set; }

    public string Nom { get; set; } = "";

    public EtatOrdinateur Etat { get; set; }

    [DefaultValue(0)]
    public int Stock { get; set; }

    [DefaultValue(0)]
    public decimal Prix { get; set; }

    public bool Vendu { get; set; } = false;

    public Caracteristiques Caracteristiques { get; set; } = new();

    [JsonIgnore]
    public List<Reparation> Reparations { get; set; } = new();


    public Ordinateur()
    {
    }


    public Ordinateur(
        int id,
        string nom,
        EtatOrdinateur etat,
        int stock,
        decimal prix,
        Caracteristiques caracteristiques)
    {
        Id = id;
        Nom = nom;
        Etat = etat;
        Stock = stock;
        Prix = prix;
        Caracteristiques = caracteristiques;
    }


    public void ChangerEtat(EtatOrdinateur nouvelEtat)
    {
        Etat = nouvelEtat;
    }


    public string AfficherInformations()
    {
        return $"{Id} - {Caracteristiques.Marque} {Nom} - " +
               $"{Caracteristiques.RamGo} Go RAM - " +
               $"{Caracteristiques.Processeur} - {Etat}";
    }


    public void ModifierInformations(
        string nom,
        EtatOrdinateur etat,
        int stock,
        decimal prix,
        Caracteristiques caracteristiques)
    {
        Nom = nom;
        Etat = etat;
        Stock = stock;
        Prix = prix;
        Caracteristiques = caracteristiques;
    }
}