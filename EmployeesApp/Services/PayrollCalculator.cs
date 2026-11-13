using System;

namespace EmployeesApp.Services
{
    public static class PayrollCalculator
    {
        // Налог 13 %, округление до 2 знаков. salary < 0 — ArgumentException
        public static decimal CalculateTax(decimal salary)
        {
            if (salary < 0)
                throw new ArgumentException("Зарплата не может быть отрицательной",
                                            nameof(salary));

            return Math.Round(salary * 0.13m, 2);
        }

        // Зарплата минус налог
        public static decimal CalculateNetSalary(decimal salary)
        {
            return salary - CalculateTax(salary);
        }

        // Число полных лет. hireDate > today — ArgumentException
        public static int GetExperienceYears(DateTime hireDate, DateTime today)
        {
            if (hireDate > today)
                throw new ArgumentException("Дата приёма не может быть в будущем",
                                            nameof(hireDate));

            int years = today.Year - hireDate.Year;
            if (today.Date < hireDate.Date.AddYears(years))
                years--;

            return years;
        }

        // Меньше 1 года — 0 %; от 1 до 5 лет включительно — 5 %; больше 5 лет — 10 %
        public static decimal CalculateBonus(decimal salary, int years)
        {
            if (salary < 0)
                throw new ArgumentException("Зарплата не может быть отрицательной",
                                            nameof(salary));
            if (years < 0)
                throw new ArgumentOutOfRangeException(nameof(years));

            decimal percent;
            if (years < 1) percent = 0m;
            else if (years <= 5) percent = 0.05m;
            else percent = 0.10m;

            return Math.Round(salary * percent, 2);
        }
    }
}