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
    [Route("api/shelf")]
    [ApiController]
    public class PolicaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public PolicaController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [EnableCors]
        [HttpPost]


        public JsonResult Post(Vracanje vracanje)
        {

            

            string query = @"delete from stavka where idknjiga = "+ vracanje.IdKnjiga+" and idpolica = ( select idpolica from polica where korisnik= '"+vracanje.Username+"');";
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
                        
                        sqlConnection.Close();
                    }
                }
                catch (Exception ex)
                {

                    return new JsonResult(ex.Message);
                }
            }


            return new JsonResult("Uspešno vraćena knjiga");
        }
        
        // GET api/<KnjigaController>/5
        [HttpGet("{id}")]
        public JsonResult Get(string id)
        {

            string query = @"SELECT k.idknjiga,k.naslov, truncate(k.ocena,2) as ocena, k.brOcena, s.moja_ocena, pis.ime, pis.prezime from pisac pis join knjiga k on pis.idpisac=k.idpisac join stavka s on k.idknjiga=s.idknjiga join polica p on s.idpolica=p.idpolica where p.korisnik = '" + id+"';";
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
        [HttpPut("{username}")]
        public JsonResult Put(string username)
        {
            string query = @"insert into polica (korisnik) values ('"+username+"');";
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
        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
            string query = @"delete from stavka where idpolica = ( select idpolica from polica where korisnik = '"+id+"')";
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
    }
}
