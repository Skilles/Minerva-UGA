using System.Linq.Expressions;
using MongoDB.Driver;

namespace Minerva.Infrastructure.Database;

public interface IRepository<TDocument>
{
    public IMongoClient Client { get; }
    
    public IMongoCollection<TDocument> Collection { get; }
    
    public IMongoDatabase Database { get; }
    
    public Task<BulkWriteResult<TDocument>> UpsertManyAsync<T>(IEnumerable<TDocument> documents, Expression<Func<TDocument, T>> idSelector, CancellationToken cancellationToken = default);
    
    public Task<ReplaceOneResult> UpsertAsync<T>(TDocument document, Expression<Func<TDocument, T>> idSelector, CancellationToken cancellationToken = default);
    
    public Task<ReplaceOneResult> UpsertAsync(TDocument document, FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default);

    public Task<TDocument> UpsertAndReturnAsync<T>(TDocument document, Expression<Func<TDocument, T>> idSelector, CancellationToken cancellationToken = default);

    public Task<TDocument> UpsertAndReturnAsync(TDocument document, FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default);
}