using MongoDB.Bson;
using MongoDB.Driver;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(IMongoDatabase database, ILoggerFactory loggerFactory) : base(database, loggerFactory)
    {
        CollectionName = "orders";
    }

    public Task CreateNewDefaultOrderAsync(string mac_address, out Order order)
    {
        Order default_order = Order.LoadDefault(mac_address);
        order = default_order;
        return CreateAsync(default_order);
    }

    public Task CreateOrderAsync(string mac_address, string itemDescr, double itemValue, out Order newOrder)
    {
        var order = Order.CreateNewOrder(mac_address, itemDescr, itemValue);
        newOrder = order;
        return CreateAsync(order);
    }

    public async Task<Order> GetLastOrderAsync(string mac_address)
    {
        return await GetCollection()
            .Find(o => o.mac_address == mac_address)
            .SortByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<Order> GetOrderByExternalReferenceAsync(string externalReference)
    {
        return await GetCollection()
            .Find(o => o.external_reference == externalReference)
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<Order>> GetNotRemovedFilteredByDateAsync(DateTime startDate, DateTime endDate, int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;

        var pipeline = new[]
        {
            new BsonDocument("$match", new BsonDocument
            {
                { "IsRemoved", false },
                { "CreatedAt", new BsonDocument
                    {
                        { "$gte", startDate.ToUniversalTime().Date },
                        { "$lte", endDate.ToUniversalTime().Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99) }
                    }
                }
            }),
            new BsonDocument("$skip", skip),  // Pular os documentos das páginas anteriores
            new BsonDocument("$limit", pageSize)  // Limitar o número de documentos por página
        };

        // Criando a agregação
        return await GetCollection().Aggregate<Order>(pipeline).ToListAsync();
    }

    public async Task<int> CountNotRemovedFilteredByDateAsync(DateTime startDate, DateTime endDate)
    {
        var pipeline = new[]
    {
        // Estágio $match para aplicar o filtro de data e campo IsRemoved
        new BsonDocument("$match", new BsonDocument
        {
            { "IsRemoved", false }, // Filtro para excluir os documentos removidos
            { "CreatedAt", new BsonDocument
                {
                    { "$gte", startDate.ToUniversalTime().Date }, // Filtro de data inicial
                    { "$lte", endDate.ToUniversalTime().Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(99) } // Filtro de data final
                }
            }
        }),

        // Estágio $count para contar os documentos que atendem ao filtro
        new BsonDocument("$count", "documentCount")
    };

        // Executando a agregação no MongoDB e obtendo o resultado
        var result = await GetCollection().Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();

        // Retornando o resultado da contagem, ou 0 se não houver resultados
        return result != null && result.Contains("documentCount") ? result["documentCount"].AsInt32 : 0;
    }

    public async Task<List<Order>> GetNotDeletedAsync() =>
        await GetCollection().Find(o => !o.IsRemoved).ToListAsync();
}