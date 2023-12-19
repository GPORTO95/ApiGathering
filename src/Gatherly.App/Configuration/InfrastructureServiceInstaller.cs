using Gatherly.Persistence.Interceptors;
using Gatherly.Persistence;
using Microsoft.EntityFrameworkCore;
using Scrutor;
using Gatherly.App.Middlewares;
using Ardalis.GuardClauses;
using Throw;
using Gatherly.Domain.Shared;

namespace Gatherly.App.Configuration;

public class InfrastructureServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");

        Ensure.NotNullOrWithSpace(connectionString);

        Guard.Against.NullOrWhiteSpace(connectionString);

        connectionString.ThrowIfNull();

        services
            .Scan(
                selector => selector
                    .FromAssemblies(
                        Gatherly.Infrastructure.AssemblyReference.Assembly,
                        Gatherly.Persistence.AssemblyReference.Assembly)
                    .AddClasses(false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsMatchingInterface()
                    .WithScopedLifetime());

        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        services.AddDbContext<ApplicationDbContext>(
            (sp, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(connectionString);
            });

        services.AddLogging();
        services.AddTransient<GlobalExceptionHandlingMiddleware>();
    }
}
