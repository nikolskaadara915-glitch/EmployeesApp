using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeesApp.Services;

namespace EmployeesApp.Tests
{
    [TestClass]
    public class PayrollCalculatorTests
    {
        [TestMethod]
        public void CalculateTax_Salary50000_Returns6500()
        {
            // Arrange
            decimal salary = 50000m;

            // Act
            decimal tax = PayrollCalculator.CalculateTax(salary);

            // Assert
            Assert.AreEqual(6500m, tax);
        }

        [TestMethod]
        public void CalculateTax_ZeroSalary_ReturnsZero()
        {
            Assert.AreEqual(0m, PayrollCalculator.CalculateTax(0m));
        }

        [TestMethod]
        public void CalculateTax_NegativeSalary_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(
                () => PayrollCalculator.CalculateTax(-1m));
        }

        [TestMethod]
        public void CalculateNetSalary_Salary50000_Returns43500()
        {
            Assert.AreEqual(43500m, PayrollCalculator.CalculateNetSalary(50000m));
        }

        [TestMethod]
        public void GetExperienceYears_ExactlyOneYearAgo_ReturnsOne()
        {
            var today = new DateTime(2024, 6, 1);
            var hire = new DateTime(2023, 6, 1);

            Assert.AreEqual(1, PayrollCalculator.GetExperienceYears(hire, today));
        }

        [TestMethod]
        public void GetExperienceYears_OneDayLessThanYear_ReturnsZero()
        {
            var today = new DateTime(2024, 6, 1);
            var hire = new DateTime(2023, 6, 2);

            Assert.AreEqual(0, PayrollCalculator.GetExperienceYears(hire, today));
        }

        [TestMethod]
        public void GetExperienceYears_FutureDate_ThrowsArgumentException()
        {
            var today = new DateTime(2024, 6, 1);
            var hire = new DateTime(2025, 1, 1);

            Assert.ThrowsException<ArgumentException>(
                () => PayrollCalculator.GetExperienceYears(hire, today));
        }

        [DataTestMethod]
        [DataRow(0, 0.0)]      // меньше года — 0 %
        [DataRow(1, 500.0)]    // 1 год — 5 %
        [DataRow(5, 500.0)]    // 5 лет — 5 %
        [DataRow(6, 1000.0)]   // 6 лет — 10 %
        public void CalculateBonus_VariousYears_ReturnsExpected(int years, double expected)
        {
            // Arrange
            decimal salary = 10000m;

            // Act
            decimal bonus = PayrollCalculator.CalculateBonus(salary, years);

            // Assert
            Assert.AreEqual((decimal)expected, bonus);
        }

        [TestMethod]
        public void CalculateBonus_NegativeSalary_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(
                () => PayrollCalculator.CalculateBonus(-1m, 3));
        }

        [TestMethod]
        public void CalculateBonus_NegativeYears_ThrowsArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => PayrollCalculator.CalculateBonus(10000m, -1));
        }
    }
}