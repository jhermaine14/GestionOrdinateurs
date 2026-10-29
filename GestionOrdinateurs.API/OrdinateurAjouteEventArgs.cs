namespace GestionOrdinateurs.API;


public class OrdinateurAjouteEventArgs : EventArgs
{
    public int Id { get; set;}
    public string Nom { get; set;} ="";
}