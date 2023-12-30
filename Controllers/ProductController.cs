using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Product()
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
        public JsonResult GetProducts([FromQuery] string? search)
        {
            string query = "select * from dbo.dbinventory";

            // Check if a search parameter was provided and modify the query accordingly
            if (!string.IsNullOrEmpty(search))
            {
                // Modify the query to include the search filter
                query = "SELECT * FROM dbo.dbinventory WHERE ProductID = @Search OR ProductName LIKE '%' + @Search + '%'";
                DataTable table1 = new DataTable();
                string sqlDatasource1 = _configuration.GetConnectionString("Default");
                SqlDataReader myReader1;
                using (SqlConnection sqlConn = new SqlConnection(sqlDatasource1))
                {
                    sqlConn.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConn))
                    {
                        sqlCommand.Parameters.AddWithValue("@Search", search);
                        sqlCommand.Parameters.AddWithValue("@SearchPattern", "%" + search + "%");
                        myReader1 = sqlCommand.ExecuteReader();
                        table1.Load(myReader1);
                        myReader1.Close();
                        sqlConn.Close();
                    }
                }
                return new JsonResult(table1);
                // Replace YourColumnName with the actual column name you want to filter on.
            }
            else {
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
        [HttpPost]
        [Route("InsertProduct")]
        public IActionResult InsertProduct(Productdto product)
        {
            try
            {
                string query = @"INSERT INTO dbo.dbinventory (ProductID, ProductName, NetPrice, SellingPrice, EnteredTime, LastupdatedTime, Quantity) 
                                VALUES (@ProductID, @ProductName, @NetPrice, @SellingPrice, @EnteredTime, @LastupdatedTime, @Quantity)";

                string sqlDatasource = _configuration.GetConnectionString("Default");
                using (SqlConnection sqlConn = new SqlConnection(sqlDatasource))
                {
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConn);
                    sqlCommand.Parameters.AddWithValue("@ProductID", product.ProductID);
                    sqlCommand.Parameters.AddWithValue("@ProductName", product.ProductName);
                    sqlCommand.Parameters.AddWithValue("@NetPrice", product.NetPrice);
                    sqlCommand.Parameters.AddWithValue("@SellingPrice", product.SellingPrice);
                    sqlCommand.Parameters.AddWithValue("@EnteredTime", product.EnteredTime);
                    sqlCommand.Parameters.AddWithValue("@LastupdatedTime", product.LastupdatedTime);
                    sqlCommand.Parameters.AddWithValue("@Quantity", product.Quantity);

                    sqlConn.Open();
                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    sqlConn.Close();

                    if (rowsAffected > 0)
                    {
                        return Ok("Product inserted successfully");
                    }
                    else
                    {
                        return BadRequest("Failed to insert product");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("AddProduct")]
        public JsonResult AddProduct()
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
