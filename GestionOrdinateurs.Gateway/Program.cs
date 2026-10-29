var builder = WebApplication.CreateBuilder(args);


// YARP
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(
        builder.Configuration
            .GetSection("ReverseProxy")
    );


// CORS pour le client Blazor
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowBlazorClient",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:5112"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        }
    );
});


var app = builder.Build();


// CORS
app.UseCors("AllowBlazorClient");


// YARP
app.MapReverseProxy();


app.MapGet(
    "/",
    () => "Gateway is running"
);


app.Run();