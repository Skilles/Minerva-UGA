using System.Linq.Expressions;
using MongoDB.Driver;

namespace Minerva.Infrastructure.Database;

public static class MongoExtensions
{
    public static FilterDefinition<TDocument> ToFilter<TDocument, TId>(this Expression<Func<TDocument, TId>> idSelector, TDocument document) 
        => Builders<TDocument>.Filter.Eq(idSelector, idSelector.Compile()(document));
    
    public static FilterDefinition<TDocument> ToFilter<TDocument, TId>(this Expression<Func<TDocument, TId>> idSelector, TId id) 
        => Builders<TDocument>.Filter.Eq(idSelector, id);
}