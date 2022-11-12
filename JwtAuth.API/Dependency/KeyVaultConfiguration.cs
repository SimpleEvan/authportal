using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace JwtAuth.API.Dependency
{
    public class KeyVaultSettings
    {
        public string KeyVaultName { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }

    public static class KeyVaultConfiguration
    {
        public static void AddKeyVaultDependency(this WebApplicationBuilder builder)
        {
            var settings = builder.Configuration.GetSection("KeyVaultSettings").Get<KeyVaultSettings>() ?? new KeyVaultSettings();
            var credentials = new ClientSecretCredential(settings.TenantId, settings.ClientId, settings.ClientSecret) ;
            var client = new SecretClient(
                    new Uri($"https://{settings.KeyVaultName}.vault.azure.net/"),
                    credentials);
            // TODO Add retry policy Polly
            builder.Configuration.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
        }
    }
}