using GenericEFCoreHelper.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericEFCoreHelper.IRepos
{
    /// <summary>
    /// A generic repository interface that provides basic CRUD operations for any entity type.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TContext">The type of the DbContext.</typeparam>
    public interface IGenericRepository<T, TContext> where T : class where TContext : DbContext
    {
        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of all entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets all entities with an optional filter and order expression.
        /// </summary>
        /// <param name="predicate">The optional predicate to filter entities.</param>
        /// <param name="orderBy">The optional expression to order entities.</param>
        /// <param name="ascending">Indicates whether the ordering should be ascending or descending.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of all entities that match the predicate and are ordered accordingly.</returns>
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true);

        /// <summary>
        /// Gets all entities by a single ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of entities that match the provided ID.</returns>
        Task<IEnumerable<T>> GetAllByIdAsync(object id);

        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found, otherwise null.</returns>
        Task<T?> GetByIdAsync(object id);

        /// <summary>
        /// Finds entities by a specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of entities that match the predicate.</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Checks if any entities match a specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether any entities match the predicate.</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the entity was added successfully.</returns>
        Task<bool> AddAsync(T entity);

        /// <summary>
        /// Adds a new entity and returns its ID.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the added entity.</returns>
        Task<int> AddAndReturnIdAsync(T entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the entity was updated successfully.</returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the entity was deleted successfully.</returns>
        Task<bool> DeleteAsync(object id);

        /// <summary>
        /// Gets paged data with optional search and order expressions.
        /// </summary>
        /// <param name="request">The DataTableRequest containing pagination and sorting information.</param>
        /// <param name="searchExpression">The optional search expression to filter entities.</param>
        /// <param name="orderExpression">The optional order expression to sort entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DataTableResponse with the paged data.</returns>
        Task<DataTableResponse<T>> GetPagedDataAsync(DataTableRequest request, Expression<Func<T, bool>>? searchExpression = null, Expression<Func<T, object>>? orderExpression = null);

        /// <summary>
        /// Retrieves the first entity that matches a specified predicate, including related entities.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="includes">The related entities to include.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches the predicate, or null if no entity matches.</returns>
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Removes a range of entities.
        /// </summary>
        /// <param name="entities">The entities to remove.</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Removes a single entity.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void Remove(T entity);
    }
}
