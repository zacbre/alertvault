using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AlertVault.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // // Remove the existing registration
            // var descriptor = services.SingleOrDefault(
            //     d => d.ServiceType == typeof(IEmailRepository));
            //
            // if (descriptor != null)
            // {
            //     services.Remove(descriptor);
            // }
            //
            // // Add the mock
            // services.AddSingleton(EmailRepositoryMock.Object);
        });
    }
}