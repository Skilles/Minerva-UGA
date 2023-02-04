using Minerva.Config;
using Minerva.Features.Athena.Services;
using Minerva.Features.Authentication.Services;
using Minerva.Features.CoursePlanner.Assemblers;
using Minerva.Features.CoursePlanner.Services;
using Minerva.Infrastructure.Email.Services;
using Minerva.Utility;

namespace Minerva.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMinervaServices(this IServiceCollection serviceCollection, MinervaConfig config)
    {
        serviceCollection.AddSingleton(config);
        serviceCollection.AddMongoDb(config);
        serviceCollection.AddHttpClient<CourseoffCapacityService>();
        serviceCollection.AddHttpClient<EmailService>();
        serviceCollection.AddSingleton<EmailService>();
        serviceCollection.AddSingleton<JWTService>();
        serviceCollection.AddSingleton<AuthenticationService>();
        serviceCollection.AddSingleton<CourseoffCapacityService>();
        serviceCollection.AddSingleton<PlannerDataAssembler>();
        serviceCollection.AddSingleton<PlannerService>();
        serviceCollection.AddSingleton<PlannerFetchService>();

        return serviceCollection;
    }
}