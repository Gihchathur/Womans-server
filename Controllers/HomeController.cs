using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("GetProducts")]
        public JsonResult GetProducts()
        {
            string query = "select * from dbo.dbinventory";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("Default");
            SqlDataReader myReader;
            using (SqlConnection sqlConn = new SqlConnection(sqlDatasource))
            {
                sqlConn.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConn))
                {
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    sqlConn.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        [Route("GetProducts")]
        public JsonResult AddProducts()
        {
            string query = "select * from dbo.dbinventory";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("Default");
            SqlDataReader myReader;
            using (SqlConnection sqlConn = new SqlConnection(sqlDatasource))
            {
                sqlConn.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConn))
                {
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    sqlConn.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
