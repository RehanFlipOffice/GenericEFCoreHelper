using GEFCH.Core;
using GEFCH.Repositories;
using GenericEFCoreHelper.Tests.DBContext;
using GenericEFCoreHelper.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace GenericEFCoreHelper.Tests
{
    public class GenericRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly GenericRepository<Employee, ApplicationDbContext> _repository;

        public GenericRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new GenericRepository<Employee, ApplicationDbContext>(_context);
        }

        private ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllEntities()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("John Doe", result.First().Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenNoEntities()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsFilteredEntities()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            Expression<Func<Employee, bool>> predicate = e => e.Name.Contains("John");

            // Act
            var result = await repository.GetAllAsync(predicate);

            // Assert
            Assert.Single(result);
            Assert.Equal("John Doe", result.First().Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEntitiesOrderedAscending()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" },
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            Expression<Func<Employee, object>> orderBy = e => e.Name;

            // Act
            var result = await repository.GetAllAsync(orderBy: orderBy, ascending: true);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Jane Doe", result.First().Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEntitiesOrderedDescending()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            Expression<Func<Employee, object>> orderBy = e => e.Name;

            // Act
            var result = await repository.GetAllAsync(orderBy: orderBy, ascending: false);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("John Doe", result.First().Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsEntity_WhenValidId()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employee = new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenInvalidId()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employee = new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(2);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task FindAsync_ReturnsMatchingEntities()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            Expression<Func<Employee, bool>> predicate = e => e.Name.Contains("John");

            // Act
            var result = await repository.FindAsync(predicate);

            // Assert
            Assert.Single(result);
            Assert.Equal("John Doe", result.First().Name);
        }

        [Fact]
        public async Task FindAsync_ReturnsEmptyList_WhenNoMatchingEntities()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            Expression<Func<Employee, bool>> predicate = e => e.Name.Contains("Smith");

            // Act
            var result = await repository.FindAsync(predicate);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task ExistsAsync_ReturnsTrue_WhenEntityMatchesPredicate()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            Expression<Func<Employee, bool>> predicate = e => e.Name.Contains("John");

            // Act
            var result = await repository.ExistsAsync(predicate);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_ReturnsFalse_WhenNoEntityMatchesPredicate()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            Expression<Func<Employee, bool>> predicate = e => e.Name.Contains("Smith");

            // Act
            var result = await repository.ExistsAsync(predicate);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue_WhenEntityAddedSuccessfully()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employee = new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };

            // Act
            var result = await repository.AddAsync(employee);

            // Assert
            Assert.True(result);
            Assert.Equal(1, context.Employees.Count());
        }

        [Fact]
        public async Task AddAsync_ReturnsFalse_WhenEntityAdditionFails()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockContext.Setup(m => m.Set<Employee>()).Returns(mockDbSet.Object);
            mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(0); // Simulate failure

            var repository = new GenericRepository<Employee, ApplicationDbContext>(mockContext.Object);
            var employee = new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };

            // Act
            var result = await repository.AddAsync(employee);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddAndReturnIdAsync_ReturnsId_WhenEntityAddedSuccessfully()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockContext.Setup(m => m.Set<Employee>()).Returns(mockDbSet.Object);
            mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1); // Simulate success

            var repository = new GenericRepository<Employee, ApplicationDbContext>(mockContext.Object);
            var employee = new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };

            // Act
            var result = await repository.AddAndReturnIdAsync(employee);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task AddAndReturnIdAsync_ThrowsException_WhenEntityDoesNotHaveIdProperty()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            var mockDbSet = new Mock<DbSet<object>>();
            mockContext.Setup(m => m.Set<object>()).Returns(mockDbSet.Object);
            mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1); // Simulate success

            var repository = new GenericRepository<object, ApplicationDbContext>(mockContext.Object);
            var entityWithoutId = new { Name = "EntityWithoutId" };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => repository.AddAndReturnIdAsync(entityWithoutId));
        }

        [Fact]
        public async Task AddAndReturnIdAsync_ReturnsId_WhenSaveChangesAsyncIsCalledSuccessfully()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockContext.Setup(m => m.Set<Employee>()).Returns(mockDbSet.Object);
            mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1); // Simulate success

            var repository = new GenericRepository<Employee, ApplicationDbContext>(mockContext.Object);
            var employee = new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" };

            // Act
            var result = await repository.AddAndReturnIdAsync(employee);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenEntityUpdatedSuccessfully()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Act
            employee.Name = "John Smith";
            var result = await _repository.UpdateAsync(employee);

            // Assert
            Assert.True(result);
            var updatedEmployee = await _context.Employees.FindAsync(employee.Id);
            Assert.Equal("John Smith", updatedEmployee.Name);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenEntityDoesNotExist()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "Non-Existent Employee", Email = "non.existent@example.com" };

            // Act
            var result = await _repository.UpdateAsync(employee);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsArgumentNullException_WhenEntityIsNull()
        {
            // Arrange
            Employee employee = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.UpdateAsync(employee));
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenEntityDeletedSuccessfully()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);
            var employee = new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.DeleteAsync(employee.Id);

            // Assert
            Assert.True(result);
            var deletedEmployee = await context.Employees.FindAsync(employee.Id);
            Assert.Null(deletedEmployee);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenEntityDoesNotExist()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);
            var nonExistentId = 999;

            // Act
            var result = await repository.DeleteAsync(nonExistentId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenEntityIsNull()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);
            var nullId = (object)null;

            // Act
            var result = await repository.DeleteAsync(nullId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetPagedDataAsync_ReturnsPagedData_WithoutSearchAndOrderExpressions()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" },
                new Employee { Id = 3, Name = "Jim Doe", Email = "jim.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            var request = new DataTableRequest
            {
                Draw = 1,
                Start = 0,
                Length = 2,
                Order = new[] { new DataTableOrder { Column = 0, Dir = "asc" } }
            };

            // Act
            var result = await repository.GetPagedDataAsync(request);

            // Assert
            Assert.Equal(1, result.Draw);
            Assert.Equal(3, result.RecordsTotal);
            Assert.Equal(3, result.RecordsFiltered);
            Assert.Equal(2, result.Data.Length);
            Assert.Equal("John Doe", result.Data[0].Name);
            Assert.Equal("Jane Doe", result.Data[1].Name);
        }

        [Fact]
        public async Task GetPagedDataAsync_ReturnsPagedData_WithSearchExpression()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" },
                new Employee { Id = 3, Name = "Jim Doe", Email = "jim.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            var request = new DataTableRequest
            {
                Draw = 1,
                Start = 0,
                Length = 2,
                Order = new[] { new DataTableOrder { Column = 0, Dir = "asc" } }
            };

            Expression<Func<Employee, bool>> searchExpression = e => e.Name.Contains("John");

            // Act
            var result = await repository.GetPagedDataAsync(request, searchExpression);

            // Assert
            Assert.Equal(1, result.Draw);
            Assert.Equal(1, result.RecordsTotal);
            Assert.Equal(1, result.RecordsFiltered);
            Assert.Single(result.Data);
            Assert.Equal("John Doe", result.Data[0].Name);
        }

        [Fact]
        public async Task GetPagedDataAsync_ReturnsPagedData_WithOrderExpression()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" },
                new Employee { Id = 3, Name = "Jim Doe", Email = "jim.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            var request = new DataTableRequest
            {
                Draw = 1,
                Start = 0,
                Length = 2,
                Order = new[] { new DataTableOrder { Column = 0, Dir = "desc" } }
            };

            Expression<Func<Employee, object>> orderExpression = e => e.Name;

            // Act
            var result = await repository.GetPagedDataAsync(request, orderExpression: orderExpression);

            // Assert
            Assert.Equal(1, result.Draw);
            Assert.Equal(3, result.RecordsTotal);
            Assert.Equal(3, result.RecordsFiltered);
            Assert.Equal(2, result.Data.Length);
            Assert.Equal("John Doe", result.Data[0].Name);
            Assert.Equal("Jim Doe", result.Data[1].Name);
        }

        [Fact]
        public async Task GetPagedDataAsync_ReturnsPagedData_WithSearchAndOrderExpressions()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" },
                new Employee { Id = 3, Name = "Jim Doe", Email = "jim.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            var request = new DataTableRequest
            {
                Draw = 1,
                Start = 0,
                Length = 2,
                Order = new[] { new DataTableOrder { Column = 0, Dir = "desc" } }
            };

            Expression<Func<Employee, bool>> searchExpression = e => e.Name.Contains("Jane");
            Expression<Func<Employee, object>> orderExpression = e => e.Name;

            // Act
            var result = await repository.GetPagedDataAsync(request, searchExpression, orderExpression);

            // Assert
            Assert.Equal(1, result.Draw);
            Assert.Equal(1, result.RecordsTotal);
            Assert.Equal(1, result.RecordsFiltered);
            Assert.Single(result.Data);
            Assert.Equal("Jane Doe", result.Data[0].Name);
        }

        [Fact]
        public async Task FirstOrDefaultAsync_ReturnsEntity_WhenPredicateMatches()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            Expression<Func<Employee, bool>> predicate = e => e.Name.Contains("John");

            // Act
            var result = await repository.FirstOrDefaultAsync(predicate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public async Task FirstOrDefaultAsync_ReturnsNull_WhenNoEntityMatchesPredicate()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            Expression<Func<Employee, bool>> predicate = e => e.Name.Contains("Smith");

            // Act
            var result = await repository.FirstOrDefaultAsync(predicate);

            // Assert
            Assert.Null(result);
        }

        //[Fact]
        //public async Task FirstOrDefaultAsync_ReturnsEntityWithIncludes_WhenPredicateMatches()
        //{
        //    // Arrange
        //    var context = CreateContext();
        //    var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

        //    var manager = new Employee { Id = 1, Name = "Manager", Email = "manager@example.com" };
        //    var employee = new Employee { Id = 2, Name = "Employee", Email = "employee@example.com", Manager = manager };

        //    context.Employees.AddRange(manager, employee);
        //    await context.SaveChangesAsync();

        //    Expression<Func<Employee, bool>> predicate = e => e.Name.Contains("Employee");
        //    Expression<Func<Employee, object>> include = e => e.Manager;

        //    // Act
        //    var result = await repository.FirstOrDefaultAsync(predicate, include);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal("Employee", result.Name);
        //    Assert.NotNull(result.Manager);
        //    Assert.Equal("Manager", result.Manager.Name);
        //}

        [Fact]
        public async Task FirstOrDefaultAsync_ThrowsArgumentNullException_WhenPredicateIsNull()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            Expression<Func<Employee, bool>> predicate = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => repository.FirstOrDefaultAsync(predicate));
        }

        [Fact]
        public void RemoveRange_RemovesEntitiesSuccessfully()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" }
            };

            context.Employees.AddRange(employees);
            context.SaveChanges();

            // Act
            repository.RemoveRange(employees);

            // Assert
            Assert.Empty(context.Employees);
        }

        [Fact]
        public void RemoveRange_DoesNotThrow_WhenRangeIsEmpty()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employees = new List<Employee>();

            // Act & Assert
            var exception = Record.Exception(() => repository.RemoveRange(employees));
            Assert.Null(exception);
        }

        [Fact]
        public void RemoveRange_DoesNotRemoveEntitiesNotInContext()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var existingEmployee = new Employee { Id = 1, Name = "ExistingEmployee", Email = "existing@example.com" };
            context.Employees.Add(existingEmployee);
            context.SaveChanges();

            var nonExistentEmployees = new List<Employee>
            {
                new Employee { Id = 2, Name = "NonExistentEmployee1", Email = "nonexistent1@example.com" },
                new Employee { Id = 3, Name = "NonExistentEmployee2", Email = "nonexistent2@example.com" }
            };

            // Act
            var exception = Record.Exception(() => repository.RemoveRange(nonExistentEmployees));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<DbUpdateConcurrencyException>(exception);
            Assert.Single(context.Employees);
            Assert.Equal("ExistingEmployee", context.Employees.First().Name);
        }

        [Fact]
        public void Remove_RemovesEntitySuccessfully()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employee = new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };
            context.Employees.Add(employee);
            context.SaveChanges();

            // Act
            repository.Remove(employee);

            // Assert
            Assert.Empty(context.Employees);
        }

        [Fact]
        public void Remove_ThrowsArgumentNullException_WhenEntityIsNull()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            Employee employee = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => repository.Remove(employee));
        }

        [Fact]
        public void Remove_ThrowsDbUpdateConcurrencyException_WhenEntityDoesNotExistInContext()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employee = new Employee { Id = 1, Name = "NonExistentEmployee", Email = "nonexistent@example.com" };

            // Act
            var exception = Record.Exception(() => repository.Remove(employee));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<DbUpdateConcurrencyException>(exception);
        }

        [Fact]
        public async Task GetAllByIdAsync_ReturnsEntity_WhenValidId()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employee = new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("John Doe", result.First().Name);
        }

        [Fact]
        public async Task GetAllByIdAsync_ReturnsEmptyList_WhenInvalidId()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            var employee = new Employee { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllByIdAsync(2);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllByIdAsync_ReturnsEmptyList_WhenNoEntities()
        {
            // Arrange
            var context = CreateContext();
            var repository = new GenericRepository<Employee, ApplicationDbContext>(context);

            // Act
            var result = await repository.GetAllByIdAsync(1);

            // Assert
            Assert.Empty(result);
        }
    }
}
