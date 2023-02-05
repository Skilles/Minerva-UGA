using System.Linq.Expressions;
using System.Reflection;
using Minerva.Config;
using Minerva.Infrastructure.Database;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Minerva.Utility;

public static class MongoExtensions
{
    public static async Task<TResult?> FindOneAsync<TResult>(this IRepository<TResult> repository,
        FilterDefinition<TResult> filter, ProjectionDefinition<TResult>? projection = null,
        CancellationToken cancellationToken = default)
    {
        var options = projection == null
            ? null
            : new FindOptions<TResult>
            {
                Limit = 1,
                Projection = projection
            };

        var cursor = await repository.Collection.FindAsync(filter, options, cancellationToken);

        return await cursor.FirstOrDefaultAsync(cancellationToken);
    }

    public static async Task<TResult?> FindOneAsync<TResult>(this IRepository<TResult> repository,
        Expression<Func<TResult, bool>> filter, ProjectionDefinition<TResult>? projection = null,
        CancellationToken cancellationToken = default)
        => await repository.FindOneAsync(new ExpressionFilterDefinition<TResult>(filter), projection,
            cancellationToken);

    public static TResult DoTransaction<TResult>(this IMongoClient client,
        Func<IClientSessionHandle, CancellationToken, TResult> action, TransactionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var session = client.StartSession();

        options ??= new(
            readPreference: ReadPreference.Primary,
            readConcern: ReadConcern.Local,
            writeConcern: WriteConcern.WMajority);

        try
        {
            return session.WithTransaction(action, options, cancellationToken);
        }
        catch (Exception)
        {
            session.AbortTransaction(cancellationToken);
            throw;
        }
    }

    public static async Task<TResult> DoTransactionAsync<TResult>(this IMongoClient client,
        Func<IClientSessionHandle, CancellationToken, Task<TResult>> action, TransactionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var session = await client.StartSessionAsync(cancellationToken: cancellationToken);

        options ??= new(
            readPreference: ReadPreference.Primary,
            readConcern: ReadConcern.Local,
            writeConcern: WriteConcern.WMajority);
        try
        {
            return await session.WithTransactionAsync(action, options, cancellationToken);
        }
        catch (Exception)
        {
            await session.AbortTransactionAsync(cancellationToken);
            throw;
        }
    }

    public static void AddMongoDb(this IServiceCollection serviceCollection, MinervaConfig config)
    {
        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        serviceCollection.AddSingleton<IMongoClient>(_ => new MongoClient(config.Mongo.ConnectionString));

        var types = Assembly.GetExecutingAssembly().GetTypes().Where(t =>
            t.IsClass && !t.IsAbstract && Util.IsAssignableToGenericType(t, typeof(IRepository<>)));
        foreach (var type in types)
        {
            var interfaceType = Util.GetGenericInterfaceForType(type, typeof(IRepository<>));
            serviceCollection.AddSingleton(interfaceType!, type);
        }
    }
}