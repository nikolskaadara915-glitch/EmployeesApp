using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeesApp.Models;
using EmployeesApp.Services;

namespace EmployeesApp.Tests
{
    [TestClass]
    public class EmployeeRepositoryTests
    {
        private TransactionScope _scope;
        private EmployeeRepository _repo;

        [TestInitialize]
        public void Init()
        {
            // Всё, что тест запишет в базу, будет отменено
            _scope = new TransactionScope();
            string cs = ConfigurationManager
                .ConnectionStrings["EmployeesDb"].ConnectionString;
            _repo = new EmployeeRepository(cs);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _scope.Dispose();
        }

        private static Employee NewEmployee(decimal salary = 50000m)
        {
            return new Employee
            {
                FullName = "Иванов Иван",
                Position = "Инженер",
                Salary = salary,
                HireDate = new DateTime(2022, 3, 15)
            };
        }

        [TestMethod]
        public void Add_ValidEmployee_ReturnsPositiveId()
        {
            int id = _repo.Add(NewEmployee());
            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void GetById_AfterAdd_ReturnsSameData()
        {
            int id = _repo.Add(NewEmployee(75000m));

            Employee e = _repo.GetById(id);

            Assert.IsNotNull(e);
            Assert.AreEqual("Иванов Иван", e.FullName);
            Assert.AreEqual("Инженер", e.Position);
            Assert.AreEqual(75000m, e.Salary);
            Assert.AreEqual(new DateTime(2022, 3, 15), e.HireDate);
        }

        [TestMethod]
        public void Delete_ExistingEmployee_GetByIdReturnsNull()
        {
            int id = _repo.Add(NewEmployee());

            bool deleted = _repo.Delete(id);

            Assert.IsTrue(deleted);
            Assert.IsNull(_repo.GetById(id));
        }

        [TestMethod]
        public void Add_IncreasesCountByOne()
        {
            int before = _repo.Count();
            _repo.Add(NewEmployee());
            Assert.AreEqual(before + 1, _repo.Count());
        }

        [TestMethod]
        public void Add_NegativeSalary_ThrowsSqlException()
        {
            Assert.ThrowsException<SqlException>(
                () => _repo.Add(NewEmployee(-1m)));
        }
    }
}