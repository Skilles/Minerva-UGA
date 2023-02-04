using System.Linq.Expressions;
using Minerva.Config;
using MongoDB.Driver;

namespace Minerva.Infrastructure.Database;

public abstract class MongoRepository<TDocument> : IRepository<TDocument> where TDocument : MongoDocument
{
    public IMongoClient Client { get; }
    
    protected string CollectionName { get; }
    
    protected string DatabaseName { get; }
    
    public IMongoCollection<TDocument> Collection { get; }
    
    public IMongoDatabase Database { get; }
    
    protected MongoRepository(IMongoClient client, MinervaConfig config, string collectionName)
    {
        Client = client;
        CollectionName = collectionName;
        DatabaseName = config.Mongo.Database;
        Database = client.GetDatabase(DatabaseName);
        Collection = Database.GetCollection<TDocument>(CollectionName);
    }

    public Task<BulkWriteResult<TDocument>> UpsertManyAsync<TId>(IEnumerable<TDocument> documents, Expression<Func<TDocument, TId>> idSelector, CancellationToken cancellationToken = default)
    {
        var idFunc = idSelector.Compile();
        var requests = documents.AsParallel().Select(document => new ReplaceOneModel<TDocument>(Builders<TDocument>.Filter.Eq(idSelector, idFunc(document)), document) { IsUpsert = true });
        
        return Collection.BulkWriteAsync(requests, new() { IsOrdered = false }, cancellationToken: cancellationToken);
    }

    public Task<ReplaceOneResult> UpsertAsync<T>(TDocument document, Expression<Func<TDocument, T>> idSelector, CancellationToken cancellationToken = default) 
        => UpsertAsync(document, idSelector.ToFilter(document), cancellationToken);

    public Task<ReplaceOneResult> UpsertAsync(TDocument document, FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default) 
        => Collection.ReplaceOneAsync(filter, document, new ReplaceOptions { IsUpsert = true }, cancellationToken);
    
    public Task<TDocument> UpsertAndReturnAsync<T>(TDocument document, Expression<Func<TDocument, T>> idSelector, CancellationToken cancellationToken = default)
        => UpsertAndReturnAsync(document, idSelector.ToFilter(document), cancellationToken);
    
    public Task<TDocument> UpsertAndReturnAsync(TDocument document, FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default)
        => Collection.FindOneAndReplaceAsync(filter, document, new FindOneAndReplaceOptions<TDocument> { IsUpsert = true }, cancellationToken);
}