using Api.Infrastructure.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Infrastructure
{
    public static class Registration
    {
        public static void AddInfratsructure(this IServiceCollection service, IConfiguration configuration)
        {
            service.Configure<TokenSettings>(configuration.GetSection("JWT"));
        }
    }
}
