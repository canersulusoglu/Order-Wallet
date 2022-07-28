var builder = WebApplication.CreateBuilder(args);

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

// Add Redis
string RedisURI = builder.Configuration.GetConnectionString("Redis");
var redis = ConnectionMultiplexer.Connect(RedisURI);

// Add RabbitMQ
SetupRabbitMQ(builder);

// Add Repositories
builder.Services.AddSingleton<IBasketRepository>(provider => new BasketRepository(redis));

// Add Services
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddSingleton<IBasketService>(service =>
    new BasketService.API.Services.Basket.BasketService(
        service.GetRequiredService<IBasketRepository>()
    )
);
builder.Services.AddSingleton<ICheckoutService>(service =>
    new CheckoutService(
        service.GetRequiredService<IBasketRepository>(),
        service.GetRequiredService<IEventBus>()
    )
);

var app = builder.Build();

// Add RabbitMQ Events
ConfigureEventBus(app);

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


void SetupRabbitMQ(WebApplicationBuilder _builder)
{
    _builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    _builder.Services.AddSingleton<IRabbitMQPersistentConnection>(options =>
    {
        string EventBusConnection = EventBusConfigurations.EventBusConnection;
        string EventBusUserName = EventBusConfigurations.EventBusUserName;
        string EventBusPassword = EventBusConfigurations.EventBusPassword;
        int EventBusRetryCount = EventBusConfigurations.EventBusRetryCount;

        var factory = new ConnectionFactory()
        {
            HostName = EventBusConnection,
            DispatchConsumersAsync = true
        };

        if (!string.IsNullOrEmpty(EventBusUserName))
        {
            factory.UserName = EventBusUserName;
        }

        if (!string.IsNullOrEmpty(EventBusPassword))
        {
            factory.Password = EventBusPassword;
        }

        return new RabbitMQPersistentConnection(factory, EventBusRetryCount);
    });

    _builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

    _builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
    {
        var subscriptionClientName = _builder.Configuration.GetConnectionString("SubscriptionClientName");
        var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
        var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
        var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

        return new EventBusRabbitMQ(
            rabbitMQPersistentConnection,
            eventBusSubcriptionsManager,
            iLifetimeScope,
            subscriptionClientName,
            EventBusConfigurations.EventBusRetryCount
        );
    });
}

void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    // Add Subscribes Here
}
