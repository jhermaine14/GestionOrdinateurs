using GestionOrdinateurs.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GestionOrdinateurs.API;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdinateursController : ControllerBase
{
    private readonly IOrdinateurService _ordinateurService;
    private readonly ILogger<OrdinateursController> _logger;
    private readonly IConfiguration _configuration;


    public OrdinateursController(IOrdinateurService ordinateurService, ILogger<OrdinateursController> logger, IConfiguration configuration)
    {
        _ordinateurService = ordinateurService;
        _logger = logger;
        _configuration = configuration;
    }


    [HttpGet("info")]
    public IActionResult GetApplicationInfo()
    {
        _logger.LogInformation($"Récupération des informations de l'application.");

        string? name = _configuration["Application:Name"];
        string? version = _configuration["Application:Version"];

        return Ok(new { Name = name, Version = version });
    }


    [HttpGet]
    public async Task<IActionResult> GetOrdinateurs()
    {
        _logger.LogInformation(
            "Récupération de la liste des ordinateurs."
        );

        return Ok(
            await _ordinateurService.ListerOrdinateursAsync()
        );
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetComputerById(int id)
    {
        _logger.LogInformation(
            $"Recherche de l'ordinateur avec l'ID {id}."
        );

        Ordinateur? ordinateur =
            await _ordinateurService
                .GetOrdinateurParIdAsync(id);

        if (ordinateur == null)
        {
            return NotFound();
        }

        return Ok(ordinateur);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AjouterOrdinateur(
    Ordinateur ordinateur)
    {
        _logger.LogInformation(
            $"Ajout de l'ordinateur {ordinateur.Nom}."
        );

        bool ajoute =
            await _ordinateurService
                .AjouterOrdinateurAsync(ordinateur);

        if (!ajoute)
        {
            return BadRequest();
        }

        return CreatedAtAction(
            nameof(GetComputerById),
            new { id = ordinateur.Id },
            ordinateur
        );
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> ModifierOrdinateur(
    int id,
    Ordinateur ordinateur)
    {
        _logger.LogInformation(
            $"Modification de l'ordinateur avec l'ID {id}."
        );

        bool modifie =
            await _ordinateurService
                .MettreAJourOrdinateurAsync(id, ordinateur);

        if (!modifie)
        {
            return NotFound();
        }

        return Ok(
            await _ordinateurService
                .GetOrdinateurParIdAsync(id)
        );
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> SupprimerOrdinateur(int id)
    {
        _logger.LogInformation(
            $"Suppression de l'ordinateur avec l'ID {id}."
        );

        bool supprime =
            await _ordinateurService
                .SupprimerOrdinateurAsync(id);

        if (!supprime)
        {
            return NotFound();
        }

        return NoContent();
    }


    [HttpGet("disponibles")]
    public async Task<IActionResult> GetOrdinateursDisponibles()
    {
        _logger.LogInformation(
            "Récupération des ordinateurs disponibles."
        );

        return Ok(
            await _ordinateurService
                .GetOrdinateursDisponiblesAsync()
        );
    }


    [HttpGet("marque/{marque}")]
    public async Task<IActionResult> GetOrdinateursParMarque(
        string marque)
    {
        _logger.LogInformation(
            $"Recherche des ordinateurs de marque {marque}."
        );

        return Ok(
            await _ordinateurService
                .GetOrdinateursParMarqueAsync(marque)
        );
    }


    [HttpGet("prix")]
    public async Task<IActionResult> GetOrdinateursParPrix()
    {
        _logger.LogInformation(
            "Récupération des ordinateurs du plus cher au moins cher."
        );

        return Ok(
            await _ordinateurService
                .GetOrdinateursParPrixAsync()
        );
    }


    [HttpGet("{id}/diagnostic")]
    public async Task<IActionResult> GetDiagnostic(int id)
    {
        _logger.LogInformation(
            $"Diagnostic de l'ordinateur avec l'ID {id}."
        );

        Ordinateur? ordinateur =
            await _ordinateurService.GetOrdinateurParIdAsync(id);

        if (ordinateur == null)
        {
            return NotFound();
        }

        string diagnostic =
            _ordinateurService.Diagnostiquer(ordinateur);

        return Ok(diagnostic);
    }



    [HttpGet("statistiques")]
    public IActionResult GetStatistiques()
    {
        _logger.LogInformation(
            "Récupération des statistiques."
        );

        var (
            total,
            enStock,
            valeurStock,
            nbReparations
        ) = _ordinateurService.CalculerStatistiques();

        return Ok(new
        {
            TotalOrdinateur = total,
            OrdiDisponibles = enStock,
            ValeurStock = valeurStock,
            NbReparations = nbReparations
        });
    }


    [HttpGet("inventaire/{nom}")]
    public async Task<IActionResult> Chercher(string nom)
    {
        List<Ordinateur> ordinateurs =
            await _ordinateurService.ListerOrdinateursAsync();

        InventaireOrdinateurs inventaire =
            new InventaireOrdinateurs();

        inventaire.Ordinateurs = ordinateurs;

        Ordinateur? ordinateur = inventaire[nom];

        if (ordinateur == null)
        {
            return NotFound();
        }

        return Ok(ordinateur);
    }

    [HttpGet("inventaire/id/{id}")]
    public async Task<IActionResult> ChercherParId(int id)
    {
        List<Ordinateur> ordinateurs =
            await _ordinateurService.ListerOrdinateursAsync();

        InventaireOrdinateurs inventaire =
            new InventaireOrdinateurs();

        inventaire.Ordinateurs = ordinateurs;

        Ordinateur? ordinateur = inventaire[id];

        if (ordinateur == null)
        {
            return NotFound();
        }

        return Ok(ordinateur);
    }
}