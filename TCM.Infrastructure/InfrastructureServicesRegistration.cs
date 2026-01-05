using TCM.Infrastructure.Hasher;

namespace TCM.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenService, JWTTokenService>();
            services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
            return services;
        }
    }
}
