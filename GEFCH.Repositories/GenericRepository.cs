using GEFCH.Core;
using GEFCH.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GEFCH.Repositories
{
    /// <summary>
    /// A generic repository class that provides basic CRUD operations for any entity type.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TContext">The type of the DbContext.</typeparam>
    public class GenericRepository<T, TContext> : IGenericRepository<T, TContext>
        where T : class
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<T> _dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{T, TContext}"/> class.
        /// </summary>
        /// <param name="context">The DbContext to be used by the repository.</param>
        public GenericRepository(TContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of all entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Gets all entities with an optional filter and order expression.
        /// </summary>
        /// <param name="predicate">The optional predicate to filter entities.</param>
        /// <param name="orderBy">The optional expression to order entities.</param>
        /// <param name="ascending">Indicates whether the ordering should be ascending or descending.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of all entities that match the predicate and are ordered accordingly.</returns>
        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets all entities by a single ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of entities that match the provided ID.</returns>
        public async Task<IEnumerable<T>> GetAllByIdAsync(object id)
        {
            return await _dbSet.Where(entity => EF.Property<object>(entity, "Id").Equals(id)).ToListAsync();
        }

        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found, otherwise null.</returns>
        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Finds entities by a specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of entities that match the predicate.</returns>
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Checks if any entities match a specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether any entities match the predicate.</returns>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the entity was added successfully.</returns>
        public async Task<bool> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        /// <summary>
        /// Adds a new entity and returns its ID.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the added entity.</returns>
        public async Task<int> AddAndReturnIdAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
            {
                throw new InvalidOperationException("Entity does not have an Id property");
            }

            return (int)idProperty.GetValue(entity);
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the entity was updated successfully.</returns>
        public async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                _dbSet.Attach(entity);
                var entry = _context.Entry(entity);
                if (entry.State == EntityState.Unchanged)
                {
                    return false;
                }

                entry.State = EntityState.Modified;
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the entity was deleted successfully.</returns>
        public async Task<bool> DeleteAsync(object id)
        {
            int result = 0;
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                result = await _context.SaveChangesAsync();
            }
            return result > 0;
        }

        /// <summary>
        /// Gets paged data with optional search and order expressions.
        /// </summary>
        /// <param name="request">The DataTableRequest containing pagination and sorting information.</param>
        /// <param name="searchExpression">The optional search expression to filter entities.</param>
        /// <param name="orderExpression">The optional order expression to sort entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DataTableResponse with the paged data.</returns>
        public async Task<DataTableResponse<T>> GetPagedDataAsync(DataTableRequest request, Expression<Func<T, bool>>? searchExpression = null, Expression<Func<T, object>>? orderExpression = null)
        {
            var query = _dbSet.AsQueryable();

            if (searchExpression != null)
            {
                query = query.Where(searchExpression);
            }

            var totalRecords = await query.CountAsync();

            if (orderExpression != null)
            {
                query = request.Order[0].Dir == "asc" ? query.OrderBy(orderExpression) : query.OrderByDescending(orderExpression);
            }

            var data = await query.Skip(request.Start).Take(request.Length).ToListAsync();

            var response = new DataTableResponse<T>
            {
                Draw = request.Draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = totalRecords,
                Data = data.ToArray()
            };

            return response;
        }

        /// <summary>
        /// Retrieves the first entity that matches a specified predicate, including related entities.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="includes">The related entities to include.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches the predicate, or null if no entity matches.</returns>
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Removes a range of entities.
        /// </summary>
        /// <param name="entities">The entities to remove.</param>
        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            _context.SaveChanges();
        }

        /// <summary>
        /// Removes a single entity.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}
