using e_biblioteka.Models;
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
    [Route("api/[controller]")]
    [ApiController]
    public class PretragaController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public PretragaController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // GET: api/<KnjigaController>
        [EnableCors]
        [HttpGet("{uslov}")]
        public JsonResult Get(int uslov)
        {
            string query;
            if (uslov == 0)
            {
                query = @"SELECT k.idknjiga, k.naslov, truncate(k.ocena,2) as ocena, k.brOcena, p.ime, p.prezime from knjiga k join pisac p on k.idpisac=p.idpisac";
            }
            else
            {
                query = @"SELECT k.idknjiga, k.naslov, truncate(k.ocena,2) as ocena, k.brOcena, p.ime, p.prezime from knjiga k join pisac p on k.idpisac=p.idpisac order by k.ocena desc";
            }
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
        [EnableCors]
        [HttpPost]


        public JsonResult Post(Rezervacija rezervacija)
        {

            if (rezervacija is null)
            {
                throw new ArgumentNullException(nameof(rezervacija));
            }

            string query = @"insert into stavka (idknjiga, idpolica) values ("+rezervacija.IdKnjiga+",(select idpolica from polica where korisnik = '"+rezervacija.Username+"'))";
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
                catch (Exception ex)
                {
                    Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    Request.Headers.Add("Access-Control-Allow-Origin", "*");
                    return new JsonResult(ex.Message);
                }
            }

            
            return new JsonResult("succ");
        }
    }
}
