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

// Add PostgreSQL
string postgreSQLConnectionString = builder.Configuration.GetConnectionString("PostgreSQL");
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(postgreSQLConnectionString);
});

// Add Redis
string RedisURI = builder.Configuration.GetConnectionString("Redis");
var redis = ConnectionMultiplexer.Connect(RedisURI);

// Add RabbitMQ
SetupRabbitMQ(builder);

// Add Repositories
builder.Services.AddScoped<IRoomOrderRepository, RoomOrderRepository>();
builder.Services.AddScoped<IUserOrderRepository, UserOrderRepository>();
builder.Services.AddScoped<IUserOrderItemRepository, UserOrderItemRepository>();
builder.Services.AddSingleton<ICacheOrderRepository>(provider => new CacheOrderRepository(redis));

// Add Services
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddScoped<IPastOrdersService>(service =>
    new PastOrdersService(
        service.GetRequiredService<IRoomOrderRepository>(),
        service.GetRequiredService<IUserOrderItemRepository>()
    )
);
builder.Services.AddSingleton<ICurrentOrdersService>(service =>
    new CurrentOrdersService(
        service.GetRequiredService<ICacheOrderRepository>(),
        service.GetRequiredService<IRoomOrderRepository>()
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

    // EventBus'tan gelen event sonucunu eventhandler'dan alabilmek için servisi autofac'e ekleme iþlemi
    _builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder
        .RegisterAssemblyTypes(typeof(BasketConfirmedIntegrationEventHandling).Assembly)
        .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
    });
}

void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    // Basket.API'den verinin gelmesini dinlemek için kanala katýlma iþlemi
    eventBus.Subscribe<BasketConfirmedIntegrationEvent, BasketConfirmedIntegrationEventHandling>();
}