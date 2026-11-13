using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeesApp.Models;
using EmployeesApp.Services;

namespace EmployeesApp.Tests
{
    public class FakeEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _items = new List<Employee>();

        public int Add(Employee e)
        {
            e.Id = _items.Count + 1;
            _items.Add(e);
            return e.Id;
        }

        public Employee GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public bool Delete(int id)
        {
            var e = GetById(id);
            if (e == null) return false;
            _items.Remove(e);
            return true;
        }

        public int Count()
        {
            return _items.Count;
        }
    }

    [TestClass]
    public class EmployeeServiceTests
    {
        private FakeEmployeeRepository _fake;
        private EmployeeService _service;

        [TestInitialize]
        public void Init()
        {
            _fake = new FakeEmployeeRepository();
            _service = new EmployeeService(_fake);
        }

        [TestMethod]
        public void Hire_ValidEmployee_SavedInRepository()
        {
            var e = new Employee
            {
                FullName = "Сидоров Сидор",
                Position = "Бухгалтер",
                Salary = 60000m,
                HireDate = new DateTime(2023, 1, 10)
            };

            int id = _service.Hire(e);

            Assert.AreEqual(1, _fake.Count());
            Assert.IsNotNull(_fake.GetById(id));
        }

        [TestMethod]
        public void Hire_EmptyName_ThrowsArgumentException()
        {
            var e = new Employee
            {
                FullName = "  ",
                Position = "Бухгалтер",
                Salary = 60000m,
                HireDate = DateTime.Today
            };

            Assert.ThrowsException<ArgumentException>(() => _service.Hire(e));
        }

        [TestMethod]
        public void Hire_NegativeSalary_ThrowsArgumentException()
        {
            var e = new Employee
            {
                FullName = "Петров Пётр",
                Position = "Бухгалтер",
                Salary = -100m,
                HireDate = DateTime.Today
            };

            Assert.ThrowsException<ArgumentException>(() => _service.Hire(e));
        }

        [TestMethod]
        public void Fire_ExistingEmployee_RemovedFromRepository()
        {
            int id = _fake.Add(new Employee
            {
                FullName = "Уволенный Уволен",
                Position = "Стажёр",
                Salary = 30000m,
                HireDate = new DateTime(2024, 1, 1)
            });

            _service.Fire(id);

            Assert.IsNull(_fake.GetById(id));
            Assert.AreEqual(0, _fake.Count());
        }

        [TestMethod]
        public void Fire_NotExistingEmployee_ThrowsInvalidOperationException()
        {
            Assert.ThrowsException<InvalidOperationException>(
                () => _service.Fire(999));
        }
    }
}