
namespace JwtAuth.API.Dependency
{
    public class WriteTo
    {
        public string Name { set; get; } = string.Empty;
        public List<string> Args { set; get; } = new List<string>();
    }

    public class SerilogOptions
    {
        public List<string> Using { set; get; } = new List<string>();
        public string MinimumLevel { set; get; } = string.Empty;
        public string Enrich { set; get; } = string.Empty;
        public List<WriteTo> WriteTo { set; get; } = new List<WriteTo>();

    }

    public static class SerilogSettings
    {
        public static void AddApplicationInsightConfiguration(ConfigurationManager configuration)
        {
            var keyVaultAppInsightConnectionString = configuration.GetSection("dabf-insights-connectionString").Value ?? string.Empty;

            if (!string.IsNullOrEmpty(keyVaultAppInsightConnectionString))
            {
                var serilogSettings = configuration.GetSection("Serilog").Get<SerilogOptions>() ?? new SerilogOptions();
                var appSettingConnectionStringForAI = serilogSettings.WriteTo.SingleOrDefault(el => el.Name == "ApplicationInsights")?.Args.FirstOrDefault();

                //#TODO update value
            }
        }
    }
}