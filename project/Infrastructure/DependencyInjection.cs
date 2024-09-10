using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using TaskList.Application.Common.Interfaces;
using TaskList.Infrastructure.Data;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbConnectionString = configuration.GetSection("MongoDbSettings:ConnectionString").Value;
        var mongoDbDatabaseName = configuration.GetSection("MongoDbSettings:DatabaseName").Value;

        Guard.Against.Null(mongoDbConnectionString, message: "Connection string not found.");
        Guard.Against.Null(mongoDbDatabaseName, message: "Database name not found.");

        services.AddSingleton<IMongoClient>(new MongoClient(mongoDbConnectionString));
        services.AddScoped(serviceProvider =>
        {
            var client = serviceProvider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoDbDatabaseName);
        });

        services.AddScoped<ITaskListRepository>(serviceProvider =>
        {
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();
            var collectionName = "TaskLists"; //Todo: get from settings 
            return new TaskListRepository(database, collectionName);
        });

        services.AddScoped<ITaskListUserRepository>(serviceProvider => 
            serviceProvider.GetRequiredService<ITaskListRepository>());

        return services;
    }
}
