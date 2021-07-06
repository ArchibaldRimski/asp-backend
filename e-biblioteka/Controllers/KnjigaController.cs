using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using e_biblioteka.Models;
using Microsoft.AspNetCore.Cors;
using System;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace e_biblioteka.Controllers
{
    [Route("api/popular")]
    [ApiController]
    public class KnjigaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public KnjigaController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [EnableCors]
        [HttpPost]


        public JsonResult Post(Knjiga knjiga)
        {

            if (knjiga is null)
            {
                throw new ArgumentNullException(nameof(knjiga));
            }

            string query = String.Format("INSERT INTO `e-biblioteka`.`knjiga` (`naslov`, `ocena`, `brOcena`, `idpisac`) VALUES ('{0}', '0', '0', '{1}');", knjiga.Naslov, knjiga.Pisac.IdPisac);
            DataTable dt = new DataTable();
            MySqlDataReader reader;
            string sqldatasource = _configuration.GetConnectionString("e-bibliotekaCon");
            using (MySqlConnection sqlConnection = new MySqlConnection(sqldatasource))
            {
                sqlConnection.Open();
                try
                {
                    using (MySqlCommand sqlCommand = new MySqlCommand(query, sqlConnection))
                    {
                        reader = sqlCommand.ExecuteReader();
                        dt.Load(reader);
                        sqlConnection.Close();
                    }
                }
                catch (Exception)
                {

                    return new JsonResult("fail!");
                }
            }


            return new JsonResult("success");
        }
        // GET: api/<KnjigaController>
        [EnableCors]
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT k.idknjiga, k.naslov, truncate(k.ocena,2) as ocena, k.brOcena, p.ime, p.prezime from knjiga k join pisac p on k.idpisac=p.idpisac order by k.ocena desc limit 6";
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

        // GET api/<KnjigaController>/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {

            string query = @"SELECT k.naslov, k.ocena, k.brOcena, p.ime, p.prezime from knjiga k join pisac p on k.idpisac=p.idpisac where k.idknjiga= "+id;
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
            return new JsonResult(dt);
        }

        
        // PUT api/<KnjigaController>/5
        [HttpPut("{ocena}")]
        public JsonResult Put(double ocena)
        {
            string sql = @"update knjiga set ocena = " + ocena;
            DataTable dt = new DataTable();
            MySqlDataReader reader;
            string sqldatasource = _configuration.GetConnectionString("e-bibliotekaCon");
            using (MySqlConnection sqlConnection = new MySqlConnection(sqldatasource))
            {
                sqlConnection.Open();
                using (MySqlCommand sqlCommand = new MySqlCommand(sql, sqlConnection))
                {
                    reader = sqlCommand.ExecuteReader();
                    dt.Load(reader);
                    sqlConnection.Close();
                }
            }
            return new JsonResult(dt);
        }

        
    }
}
