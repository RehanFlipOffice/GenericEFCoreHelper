using Demo.Core.Entities;
using GenericEFCoreHelper.Models;
using System.Linq.Expressions;

namespace Demo.Services.IServices
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<IEnumerable<Employee>> GetAllEmployeesAsync(Expression<Func<Employee, bool>>? predicate, Expression<Func<Employee, object>>? orderBy, bool ascending);
        Task<IEnumerable<Employee>> GetAllEmployeesByIdAsync(int id);
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<IEnumerable<Employee>> FindEmployeesAsync(Employee employee);
        Task<bool> EmployeeExistsAsync(Employee employee);
        Task<bool> AddEmployeeAsync(Employee employee);
        Task<int> AddEmployeeAndReturnIdAsync(Employee employee);
        Task<bool> UpdateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<DataTableResponse<Employee>> GetPagedEmployeesAsync(DataTableRequest request);
        Task<Employee?> GetFirstOrDefaultEmployeeAsync(Expression<Func<Employee, bool>> predicate, params Expression<Func<Employee, object>>[] includes);
        void RemoveEmployeesRange(IEnumerable<Employee> employees);
        void RemoveEmployee(Employee employee);
    }
}

