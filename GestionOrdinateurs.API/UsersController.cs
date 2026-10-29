using GestionOrdinateurs.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.API;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ParcInformatiqueContext _context;

    public UsersController(
        ParcInformatiqueContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        List<User> users =
            await _context.Users.ToListAsync();

        return Ok(users);
    }


    [HttpGet("userbylogin/{login}")]
    public async Task<IActionResult> GetUserByLogin(
        string login)
    {
        User? user =
            await _context.Users
                .FirstOrDefaultAsync(
                    u => u.Login == login
                );

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}