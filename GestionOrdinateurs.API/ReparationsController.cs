using GestionOrdinateurs.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GestionOrdinateurs.API;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReparationsController : ControllerBase
{
    private readonly ReparationService _reparService;
    private readonly IOrdinateurService _ordinateurService;
    private readonly StockService _stockService;
    private readonly ILogger<ReparationsController> _logger;


    public ReparationsController(
        ReparationService reparService,
        IOrdinateurService ordinateurService,
        StockService stockService,
        ILogger<ReparationsController> logger)
    {
        _reparService = reparService;
        _ordinateurService = ordinateurService;
        _stockService = stockService;
        _logger = logger;
    }


    [Authorize(Roles = "Admin")]
    [HttpPost("simuler")]
    public async Task<IActionResult> SimulerReparation()
    {
        // Récupération de la liste des ordinateurs
        List<Ordinateur> listOrdinateurs =
            await _ordinateurService
                .ListerOrdinateursAsync();

        if (listOrdinateurs.Count < 5)
        {
            return BadRequest(
                "Il faut au moins 5 ordinateurs."
            );
        }

        await Task.WhenAll(
            _reparService.SimulerReparationAsync(listOrdinateurs[0]),
            _reparService.SimulerReparationAsync(listOrdinateurs[1]),
            _reparService.SimulerReparationAsync(listOrdinateurs[2]),
            _reparService.SimulerReparationAsync(listOrdinateurs[3]),
            _reparService.SimulerReparationAsync(listOrdinateurs[4])
        );

        return Ok("Les réparations sont terminées.");
    }


    [HttpGet]
    public async Task<IActionResult> GetReparations()
    {
        List<Reparation> reparations =
            await _reparService
                .ListerReparationsAsync();

        return Ok(reparations);
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("stock-ssd")]
    public IActionResult GetStockSSD()
    {
        Parallel.For(
            0,
            10,
            i => _stockService.ConsommerSSD()
        );

        _logger.LogInformation(
            $"Stock SSD restant : {_stockService.StockSSD}"
        );

        return Ok(_stockService.StockSSD);
    }


    [HttpGet("historique")]
    public async Task<IActionResult> GetHistorique()
    {
        var historique =
            await _reparService.GetHistoriqueReparationsAsync();

        return Ok(historique);
    }

    
}