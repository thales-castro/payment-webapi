using MongoDB.Driver;
using PaymentWebApi.Entities;
using PaymentWebApi.Exceptions;

namespace PaymentWebApi.Database.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected string CollectionName = string.Empty;
    private readonly IMongoDatabase _database;
    protected ILogger _logger;

    protected GenericRepository(IMongoDatabase database, ILoggerFactory loggerFactory)
    {
        _database = database;
        _logger = loggerFactory.CreateLogger(GetType().Name);
    }

    protected IMongoCollection<T> GetCollection()
    {
        return _database.GetCollection<T>(CollectionName);
    }

    public T Create(T entity)
    {
        entity.CreatedAt = DateTime.Now; // MongoDB already set the date in UTC Format.
        entity.CreatedBy = "PaymentAPI"; // TODO: Get this from HttpContext.
        GetCollection().InsertOne(entity);
        return entity;
    }

    public Task CreateAsync(T entity)
    {
        entity.CreatedAt = DateTime.Now; // MongoDB already set the date in UTC Format.
        entity.CreatedBy = "PaymentAPI"; // TODO: Get this from HttpContext.
        return GetCollection().InsertOneAsync(entity);        
    }

    public T Read(string id) =>
            GetCollection().Find(e => e.Id == id).FirstOrDefault();

    public List<T> ReadAll()
        => GetCollection().Find(Builders<T>.Filter.Empty).ToList();

    public T Update(T entity)
    {
        var oldEntity = Read(entity.Id);

        if (oldEntity == null)
        {
            _logger.LogError($"Error on updating entity {entity.GetType().Name} with id [{entity.Id}] does not exists.");
            throw new EntityNotFoundException($"Error on updating entity {entity.GetType().Name} with id [{entity.Id}] does not exists.");
        }

        entity.Id = oldEntity.Id;
        entity.UpdatedAt = DateTime.Now; // MongoDB already set the date in UTC Format.
        entity.UpdatedBy = "Tracker";

        var result = GetCollection().ReplaceOne(e => e.Id == oldEntity.Id, entity);

        if (result.MatchedCount == 0)
            throw new Exception($"Error on updating entity {entity.GetType().Name} with id [{entity.Id}]: {result.UpsertedId}");

        return entity;
    }

    public T Delete(string id)
    {
        var entity = Read(id);

        if (entity == null)
        {
            _logger.LogError($"Error on deleting entity {entity.GetType().Name} with id [{entity.Id}] does not exists.");
            throw new EntityNotFoundException($"Error on deleting entity {entity.GetType().Name} with id [{entity.Id}] does not exists.");
        }

        entity.RemovedAt = DateTime.Now;
        entity.RemovedBy = "Tracker";
        entity.IsRemoved = true;

        var result = GetCollection().ReplaceOne(e => e.Id == entity.Id, entity);

        if (result.MatchedCount == 0)
            throw new Exception($"Error on updating entity {entity.GetType().Name} with id [{entity.Id}]: {result.UpsertedId}");

        return entity;
    }
}