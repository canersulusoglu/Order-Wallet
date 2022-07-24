var builder = WebApplication.CreateBuilder(args);
//start up a 6 versiyonunda gerek yok o y�zden program.cs dosyas�nda direkt i�lem yap�yoruz 
// Geli�tirme Ortam� De�i�kenlerini Y�kleme
string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
string appSettingsFile = (envName == "Development") ? "appsettings.Development.json" : "appsettings.json";
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(appSettingsFile, optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
