using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Reports.Infrastructure.Seeders
{
    public static class OpenIddictSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("teachers-reports-generator") is null)
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = "teachers-reports-generator",
                    ClientSecret = "P8yZ9Ln#UqFw5*Kz^Tb4X6!mRpHd3Yj@VcQ2Ga&B", // Replace with a strong secret
                    DisplayName = "Teachers Reports Generator",
                    Permissions =
                    {
                        Permissions.GrantTypes.ClientCredentials,
                        Permissions.GrantTypes.Password,
                        Permissions.Endpoints.Token,
                        Permissions.Scopes.Profile,      // Optional for future use
                    },
                    RedirectUris = { new Uri("https://localhost:44399/callback") }, // Update based on your app's URL
                    PostLogoutRedirectUris = { new Uri("https://localhost:44399/signout-callback") }, // Optional
                    ApplicationType = ClientTypes.Public, // Public app
                    ClientType = ClientTypes.Confidential
                };

                await manager.CreateAsync(descriptor);
            }
        }
    }
}
