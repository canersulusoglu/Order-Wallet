var builder = WebApplication.CreateBuilder(args);
//start up a 6 versiyonunda gerek yok o yüzden program.cs dosyasýnda direkt iþlem yapýyoruz 
// Geliþtirme Ortamý Deðiþkenlerini Yükleme
string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
string appSettingsFile = (envName == "Development") ? "appsettings.Development.json" : "appsettings.json";
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(appSettingsFile, optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add Controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

// Add CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        string[] allowedMethods = { "POST", "DELETE", "PUT" };
        policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .WithMethods(methods: allowedMethods);
    });
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Using CORS Policy
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
