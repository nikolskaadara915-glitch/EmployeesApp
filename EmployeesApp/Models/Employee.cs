using System;

namespace EmployeesApp.Models
{
    public class Employee
    {
        public int Id { get; set; }                  // Уникальный идентификатор (генерируется БД)
        public string FullName { get; set; }         // ФИО сотрудника
        public string Position { get; set; }         // Должность
        public decimal Salary { get; set; }          // Зарплата (decimal используется для денег, чтобы избежать ошибок округления)
        public DateTime HireDate { get; set; }       // Дата приема на работу
    }
}