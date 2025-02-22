using Demo.Core.Entities;
using Demo.Services.DBContext;
using Demo.Services.IServices;
using GenericEFCoreHelper.IRepos;
using GenericEFCoreHelper.Models;
using System.Linq.Expressions;

namespace Demo.Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IGenericRepository<Employee, ApplicationDbContext> _employeeRepository;

        public EmployeeService(IGenericRepository<Employee, ApplicationDbContext> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(
            Expression<Func<Employee, bool>>? predicate = null,
            Expression<Func<Employee, object>>? orderBy = null,
            bool ascending = true)
        {
            return await _employeeRepository.GetAllAsync(predicate, orderBy, ascending);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesByIdAsync(int id)
        {
            return await _employeeRepository.GetAllByIdAsync(id);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Employee>> FindEmployeesAsync(Employee employee)
        {
            return await _employeeRepository.FindAsync(x => x == employee);
        }

        public async Task<bool> EmployeeExistsAsync(Employee employee)
        {
            return await _employeeRepository.ExistsAsync(x => x == employee);
        }

        public async Task<bool> AddEmployeeAsync(Employee employee)
        {
            return await _employeeRepository.AddAsync(employee);
        }

        public async Task<int> AddEmployeeAndReturnIdAsync(Employee employee)
        {
            return await _employeeRepository.AddAndReturnIdAsync(employee);
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            return await _employeeRepository.UpdateAsync(employee);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            return await _employeeRepository.DeleteAsync(id);
        }

        public async Task<DataTableResponse<Employee>> GetPagedEmployeesAsync(DataTableRequest request)
        {
            Expression<Func<Employee, bool>>? searchExpression = null;
            Expression<Func<Employee, object>>? orderExpression = null;

            // Apply search filter
            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                searchExpression = u => u.Name.Contains(request.Search.Value);
            }

            // Apply ordering
            if (request.Order != null && request.Order.Length > 0)
            {
                var order = request.Order[0];
                var column = request.Columns[order.Column]?.Data;

                if (!string.IsNullOrEmpty(column))
                {
                    var parameter = Expression.Parameter(typeof(Employee), "u");
                    var property = Expression.Property(parameter, column);
                    orderExpression = Expression.Lambda<Func<Employee, object>>(Expression.Convert(property, typeof(object)), parameter);
                }
            }
            return await _employeeRepository.GetPagedDataAsync(request, searchExpression, orderExpression);
        }

        public async Task<Employee?> GetFirstOrDefaultEmployeeAsync(Expression<Func<Employee, bool>> predicate, params Expression<Func<Employee, object>>[] includes)
        {
            return await _employeeRepository.FirstOrDefaultAsync(predicate, includes);
        }

        public void RemoveEmployeesRange(IEnumerable<Employee> employees)
        {
            _employeeRepository.RemoveRange(employees);
        }

        public void RemoveEmployee(Employee employee)
        {
            _employeeRepository.Remove(employee);
        }
    }
}

