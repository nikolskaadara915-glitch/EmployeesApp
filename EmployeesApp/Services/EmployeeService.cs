using System;
using EmployeesApp.Models;

namespace EmployeesApp.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeService(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        // Прием на работу
        public int Hire(Employee e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (string.IsNullOrWhiteSpace(e.FullName))
                throw new ArgumentException("ФИО не может быть пустым", nameof(e));
            if (e.Salary < 0)
                throw new ArgumentException("Зарплата не может быть отрицательной", nameof(e));

            return _repo.Add(e); // Если проверки пройдены, сохраняем в БД
        }

        // Увольнение
        public void Fire(int id)
        {
            var e = _repo.GetById(id);
            if (e == null) // Если сотрудника нет, бросаем ошибку
                throw new InvalidOperationException("Сотрудник не найден");

            _repo.Delete(id);
        }
    }
}