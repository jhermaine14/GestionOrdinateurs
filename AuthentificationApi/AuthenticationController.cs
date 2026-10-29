namespace AuthentificationApi;

using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("auth")]
public class AuthenticationController(
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory)
    : ControllerBase
{
    private readonly IConfiguration _configuration =
        configuration;

    private readonly IHttpClientFactory _httpClientFactory =
        httpClientFactory;


    /// <summary>
    /// Génère le token JWT à la connexion
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login(string login)
    {
        var client =
            _httpClientFactory
                .CreateClient("GestionOrdinateursApi");

        var response =
            await client.GetAsync(
                $"api/Users/userbylogin/{login}"
            );


        if (!response.IsSuccessStatusCode)
        {
            return NotFound("Il n'existe pas");
        }


        User? user =
            await response.Content
                .ReadFromJsonAsync<User>();


        if (user == null)
        {
            return NotFound("Il n'existe pas");
        }


        var jwtKey =
            _configuration["Jwt:Key"] ?? "";

        var key =
            Encoding.ASCII.GetBytes(jwtKey);


        return Ok(
            GenTokenJWT(
                key,
                user.Role,
                user.Login
            )
        );
    }


    /// <summary>
    /// Permet de générer un token JWT
    /// </summary>
    private string GenTokenJWT(
        byte[] keyP,
        RoleEnum role,
        string login)
    {
        var tokenHandler =
            new JwtSecurityTokenHandler();


        var claims = new[]
        {
            new Claim(
                ClaimTypes.Name,
                login
            ),

            new Claim(
                ClaimTypes.Role,
                role.ToString()
            )
        };


        var tokenDescriptor =
            new SecurityTokenDescriptor
            {
                Subject =
                    new ClaimsIdentity(claims),

                Expires =
                    DateTime.UtcNow
                        .AddMinutes(15),

                SigningCredentials =
                    new SigningCredentials(
                        new SymmetricSecurityKey(keyP),
                        SecurityAlgorithms
                            .HmacSha256Signature
                    )
            };


        var token =
            tokenHandler.CreateToken(
                tokenDescriptor
            );


        var jwt =
            tokenHandler.WriteToken(token);


        return jwt;
    }
}