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
    [Route("api/stavka")]
    [ApiController]
    public class StavkaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public StavkaController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [EnableCors]
        [HttpPost]


        public JsonResult Post(int ocena, int idknjiga, string username)
        {



            string query = @"UPDATE `e-biblioteka`.`stavka` SET `moja_ocena` = " + ocena + " WHERE idknjiga = " + idknjiga + " and idpolica = (select idpolica from polica where korisnik  = '" + username + "' );";
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
           
            
                query = @"update knjiga set brOcena=(brOcena+1) where idknjiga = " + idknjiga;
                dt = new DataTable();

                sqldatasource = _configuration.GetConnectionString("e-bibliotekaCon");
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
                query = @"update knjiga set  ocena = (ocena*(brOcena-1)+" + ocena + ")/(brOcena) where idknjiga = " + idknjiga;
                dt = new DataTable();

                sqldatasource = _configuration.GetConnectionString("e-bibliotekaCon");
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
            
            
            


            return new JsonResult("success");
        }
    }
}
