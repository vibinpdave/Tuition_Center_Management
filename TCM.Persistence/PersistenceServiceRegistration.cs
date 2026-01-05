namespace TCM.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DatabaseConnectionString");
            services.AddDbContext<DatabaseContext>(
                options => options.UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
