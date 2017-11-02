using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ServerCheck.Controllers
{
    public class CheckController : ApiController
    {
        private const string MyConnectinString = "Server=183.82.121.94;Port=3306;Database=Test;Uid=root;Pwd=PetsDatebase@1;";

        [HttpGet]
        [Route("Web")]
        public HttpResponseMessage ApplicationStatus()
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent("Web application working fine from VS using Jenkins", Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet]
        [Route("SQL")]
        public HttpResponseMessage SQLStatus()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(MyConnectinString))
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM FirstTable LIMIT 1";
                    string FirstRow = cmd.ExecuteScalar().ToString();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent("SQL Connected successfully now. First row of First table is " + FirstRow, Encoding.UTF8, "application/json");
                    return response;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError);

                response.Content = new StringContent("SQL Connection failed. Exception is " + JsonConvert.SerializeObject(ex), Encoding.UTF8, "application/json");
                return response;
            }
        }
    }
}