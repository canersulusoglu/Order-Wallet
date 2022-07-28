namespace EventBus.RabbitMQ
{
    public static class EventBusConfigurations
    {
        public static string EventBusConnection = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") ? "localhost" : "eventbus";
        public static string EventBusUserName = "";
        public static string EventBusPassword = "";
        public static int EventBusRetryCount = 5;
    }
}
