namespace PaymentSystem.WebApi.Database.Repositories;

public interface IGenericRepository<T> where T : class
{
    T Create(T entity);
    T Read(string id);
    List<T> ReadAll();
    T Update(T entity);
    T Delete(string id);
}
