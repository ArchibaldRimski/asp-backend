using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace e_biblioteka.Controllers
{
    [Route("api/signup")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public SignupController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        // POST api/<ValuesController>
        [EnableCors]
        [HttpPost]


        public JsonResult Post(Korisnik korisnik)
        {

            if (korisnik is null)
            {
                throw new ArgumentNullException(nameof(korisnik));
            }

            string query = String.Format("INSERT INTO `e-biblioteka`.`korisnik` (`username`, `password`, `ime`, `prezime`, `admin`) VALUES ('{0}', '{1}', '{2}', '{3}', {4});",korisnik.Username,korisnik.Password,korisnik.Ime,korisnik.Prezime,korisnik.Status);
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
                    
                    return new JsonResult("username taken!");
                }
            }

            
            return new JsonResult("success");
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
