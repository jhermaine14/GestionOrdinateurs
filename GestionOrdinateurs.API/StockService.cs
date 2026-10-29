namespace GestionOrdinateurs.API;

public class StockService
{
    public int StockSSD { get; private set; } = 10;

    // Verrou utilisé pour sécuriser l'accès concurrent au stock
    private readonly object _stockLock = new object();

    public void ConsommerSSD()
    {
        Task.Delay(10).Wait();

        lock (_stockLock)
        {
            StockSSD--;
        }
    }
}