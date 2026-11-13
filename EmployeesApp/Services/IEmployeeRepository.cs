using EmployeesApp.Models;

namespace EmployeesApp.Services
{
    public interface IEmployeeRepository
    {
        int Add(Employee employee);
        Employee GetById(int id);
        bool Delete(int id);
        int Count();
    }
}