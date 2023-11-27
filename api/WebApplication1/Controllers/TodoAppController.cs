using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoAppController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TodoAppController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetNotes")]
        public JsonResult GetNotes()
        {
            string query = "select * from dbo.notes";
            DataTable table = new();
            string sqlDatasource = _configuration.GetConnectionString("todoAppDBCon");
            using (SqlConnection myCon = new(sqlDatasource))
            { 
                myCon.Open();
                using SqlCommand myCommand = new(query, myCon);
                SqlDataReader myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();
            }
            return new JsonResult(table);
        }

        [HttpPost]
        [Route("AddNotes")]
        public JsonResult AddNotes([FromForm] string newNotes)
        {
            string query = "insert into dbo.Notes values(@newNotes)";
            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("todoAppDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new(sqlDataSource))
            {
                myCon.Open();
                using SqlCommand MyCommand = new(query, myCon);
                MyCommand.Parameters.AddWithValue("@newNotes", newNotes);
                myReader = MyCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();
            }
            return new JsonResult("Added Successfully");
        }


        [HttpDelete]
        [Route("DeleteNotes")]
        public JsonResult DeleteNotes(int id)
        {
            string query = "delete from dbo.Notes where id = @id";
            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("todoAppDBCon");
            SqlDataReader MyReader;
            using (SqlConnection MyCon = new(sqlDataSource))
            {
                MyCon.Open();
                using SqlCommand myCommand = new(query, MyCon);
                myCommand.Parameters.AddWithValue("@id", id);
                MyReader = myCommand.ExecuteReader();
                table.Load(MyReader);
                MyReader.Close();
                MyCon.Close();
            }
            return new JsonResult("Deleted Successfully!");
        }
    }
}
