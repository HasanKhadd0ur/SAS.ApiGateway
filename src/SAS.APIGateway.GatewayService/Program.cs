using SAS.Gateway.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevClient",
        builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials() 
    );
});


// Add JWT authentication
builder.Services.AddJwtAuthentication(builder.Configuration);


// Load Reverse Proxy from appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


// Add services to the container.
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseCors("AllowAngularDevClient");

app.UseAuthentication();
app.UseAuthorization();

// Enable routing through proxy
app.MapReverseProxy();

app.Run();