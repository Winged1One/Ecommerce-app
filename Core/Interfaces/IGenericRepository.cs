using Core.Entities;

namespace Core.Interfaces;

//This means it only accept classes that inherits from BaseEntity
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec);
    //TResult is not declared by the class, so the method must declare it using <TResult>.
    //The <TResult> after ListAsync declares TResult, and then the return type can use it.
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec);
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<bool> SaveAllAsync();
    bool Exist(int id);
    Task<int> CountAsync(ISpecification<T> spec);
}