using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace e_biblioteka.Controllers
{
    [Route("api/pisac")]
    [ApiController]
    public class PisacController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public PisacController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // GET: api/<KnjigaController>
        [EnableCors]
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from pisac";
            DataTable dt = new DataTable();
            MySqlDataReader reader;
            string sqldatasource = _configuration.GetConnectionString("e-bibliotekaCon");
            using (MySqlConnection sqlConnection = new MySqlConnection(sqldatasource))
            {
                sqlConnection.Open();
                using (MySqlCommand sqlCommand = new MySqlCommand(query, sqlConnection))
                {
                    reader = sqlCommand.ExecuteReader();
                    dt.Load(reader);
                    sqlConnection.Close();
                }
            }
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return new JsonResult(dt);
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string query = @"delete from stavka where idknjiga = "+id;
            MySqlDataReader reader;
            string sqldatasource = _configuration.GetConnectionString("e-bibliotekaCon");
            using (MySqlConnection sqlConnection = new MySqlConnection(sqldatasource))
            {
                sqlConnection.Open();
                using (MySqlCommand sqlCommand = new MySqlCommand(query, sqlConnection))
                {
                    reader = sqlCommand.ExecuteReader();

                    sqlConnection.Close();
                }
            }
           query = @"delete from knjiga where idknjiga=  " + id;
            
            
            using (MySqlConnection sqlConnection = new MySqlConnection(sqldatasource))
            {
                sqlConnection.Open();
                using (MySqlCommand sqlCommand = new MySqlCommand(query, sqlConnection))
                {
                    reader = sqlCommand.ExecuteReader();
                    
                    sqlConnection.Close();
                }
            }


        }
    }
}
