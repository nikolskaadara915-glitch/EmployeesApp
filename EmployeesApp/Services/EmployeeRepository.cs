using System.Data.SqlClient;
using EmployeesApp.Models;

namespace EmployeesApp.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString; // Строка подключения к БД

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Add(Employee e)
        {
            // SQL-запрос (чтобы сразу получить созданный Id)
            const string sql = "INSERT INTO Employees (FullName, Position, Salary, HireDate) " +
                               "OUTPUT INSERTED.Id VALUES (@name, @pos, @salary, @date)";

            // using гарантирует, что соединение закроется после выполнения
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                // Передаем параметры
                cmd.Parameters.AddWithValue("@name", e.FullName);
                cmd.Parameters.AddWithValue("@pos", e.Position);
                cmd.Parameters.AddWithValue("@salary", e.Salary);
                cmd.Parameters.AddWithValue("@date", e.HireDate);

                con.Open(); // Открываем соединение
                return (int)cmd.ExecuteScalar(); // Выполняем и возвращаем Id
            }
        }

        public Employee GetById(int id)
        {
            const string sql = "SELECT Id, FullName, Position, Salary, HireDate " +
                               "FROM Employees WHERE Id = @id";

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                using (var r = cmd.ExecuteReader()) // ExecuteReader для чтения строк
                {
                    if (!r.Read()) return null; // Если строк нет, возвращаем null

                    // Создаем объект Employee и заполняем его данными из БД
                    return new Employee
                    {
                        Id = r.GetInt32(0),
                        FullName = r.GetString(1),
                        Position = r.GetString(2),
                        Salary = r.GetDecimal(3),
                        HireDate = r.GetDateTime(4)
                    };
                }
            }
        }

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Employees WHERE Id = @id";
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                // ExecuteNonQuery возвращает количество затронутых строк. Если > 0, значит удалили.
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public int Count()
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Employees", con))
            {
                con.Open();
                return (int)cmd.ExecuteScalar(); // Возвращает одно число (количество)
            }
        }
    }
}