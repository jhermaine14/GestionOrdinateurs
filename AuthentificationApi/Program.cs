global using System.Text;
global using Microsoft.IdentityModel.Tokens;
global using GestionOrdinateurs.Shared;

var builder = WebApplication.CreateBuilder(args);


// CORS pour le client Blazor
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5112",
            "http://localhost:5000"
            )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// HttpClientFactory pour appeler GestionOrdinateurs.API
builder.Services.AddHttpClient(
    "GestionOrdinateursApi",
    client =>
    {
        client.BaseAddress =
            new Uri("http://gestionordinateurs-api:5187/");
    }
);


var app = builder.Build();


// Activer CORS
app.UseCors("AllowBlazorClient");


// Swagger
app.UseSwagger();
app.UseSwaggerUI();


// Controllers
app.MapControllers();

app.Run();