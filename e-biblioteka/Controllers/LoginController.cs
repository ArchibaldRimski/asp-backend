using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors;

namespace e_biblioteka.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public LoginController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet] 
        public JsonResult Get()
        {
           

            string query = @"select username, password, ime, prezime, admin from korisnik ;";
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
                    Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    return new JsonResult("invalid credentials");
                }
            }
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return new JsonResult(dt);
        }
        [EnableCors]
        [HttpPost]
        

        public JsonResult Post(Korisnik korisnik)
        {

            if (korisnik is null)
            {
                throw new ArgumentNullException(nameof(korisnik));
            }

            string query = @"select username, password, ime, prezime, admin from korisnik where username = '" + korisnik.Username + "' and password = '" + korisnik.Password + "' ;";
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
                    Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    Request.Headers.Add("Access-Control-Allow-Origin", "*");
                    return new JsonResult("invalid credentials");
                }
            }
            
            if (dt.Rows.Count == 0)
            {
                Response.Headers.Add("Access-Control-Allow-Origin", "*");
                Request.Headers.Add("Access-Control-Allow-Origin", "*");
                return new JsonResult("invalid credentials");
            }
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Request.Headers.Add("Access-Control-Allow-Origin", "*");
            return new JsonResult(dt);
        }
    }
}
