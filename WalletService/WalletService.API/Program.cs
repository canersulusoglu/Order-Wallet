var builder = WebApplication.CreateBuilder(args);
//start up a 6 versiyonunda gerek yok o yüzden program.cs dosyasýnda direkt iþlem yapýyoruz 
// Geliþtirme Ortamý Deðiþkenlerini Yükleme
string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{envName}.json", optional: true)
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
