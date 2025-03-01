using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnlineDBController : ControllerBase
    {
        private readonly string _connectionString;

        public OnlineDBController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AppConn");
        }

        [HttpGet]
        public IActionResult GetData()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM abc", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(reader);

                        var list = new List<Dictionary<string, object>>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            var dict = new Dictionary<string, object>();
                            foreach (DataColumn col in dataTable.Columns)
                            {
                                dict[col.ColumnName] = row[col]; // Store column data
                            }
                            list.Add(dict);
                        }

                        return Ok(list); // ✅ Now it's serializable
                    }
                }
            }
        }
    }
}
