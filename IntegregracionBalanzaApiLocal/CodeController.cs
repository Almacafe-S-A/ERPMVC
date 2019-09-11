using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IntegregracionBalanzaApiLocal
{

    [EnableCors("*", "*", "*")]
    public class CodeController : ApiController
    {
        // POST api/code 
        //public HttpResponseMessage Get([FromBody]Boleto_Ent _Boleto_Entp)
     //   [EnableCors(origins: "http://localhost:1371", headers: "*", methods: "*")]
        public HttpResponseMessage Get(Int64 id)
        {

            List<Boleto_Ent> _boleta_entq = new List<Boleto_Ent>();
            try
            {
                string connetionString = null;
                OdbcConnection cnn = new OdbcConnection();
                connetionString =ConfigurationManager.AppSettings["AccessConnectionString"];
               
                try
                {
                    if (cnn.State == System.Data.ConnectionState.Closed)
                    {
                        cnn = new OdbcConnection(connetionString);
                        cnn.Open();
                    }
                    // Console.WriteLine("Connection Open ! ");
                    // OdbcConnection connection = new OdbcConnection("");
                    OdbcCommand command = new OdbcCommand("SELECT * FROM boleto_ent WHERE clave_e = ? ", cnn);
                    command.Parameters.Add("@clave_e", OdbcType.Int).Value = id;

                    Boleto_Ent _boleta_ent = new Boleto_Ent();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _boleta_ent.clave_e = Convert.ToInt64(reader.GetValue(0).ToString());
                            _boleta_ent.clave_C = reader.GetValue(1).ToString();
                            _boleta_ent.clave_o = reader.GetValue(2).ToString();
                            _boleta_ent.clave_p = reader.GetValue(3).ToString();
                            _boleta_ent.completo = Convert.ToBoolean(reader.GetValue(4).ToString());
                            _boleta_ent.fecha_e = Convert.ToDateTime(reader.GetValue(5).ToString());
                            _boleta_ent.hora_e = reader.GetValue(6).ToString();
                            _boleta_ent.placas = reader.GetValue(7).ToString();

                            _boleta_ent.conductor = reader.GetValue(8).ToString();
                            _boleta_ent.peso_e = Convert.ToInt32(reader.GetValue(9).ToString());
                            _boleta_ent.observa_e = reader.GetValue(10).ToString();
                            _boleta_ent.nombre_oe = reader.GetValue(11).ToString();
                            _boleta_ent.turno_oe = reader.GetValue(12).ToString();
                            _boleta_ent.unidad_e = reader.GetValue(13).ToString();

                            _boleta_ent.bascula_e = reader.GetValue(14).ToString();
                            _boleta_ent.t_entrada = Convert.ToInt32(reader.GetValue(15).ToString());
                            _boleta_ent.clave_u = reader.GetValue(16).ToString();
                            _boleta_entq.Add(_boleta_ent);
                            // Word is from the database. Do something with it.
                        }
                    }

                    command = new OdbcCommand("SELECT * FROM boleto_sal WHERE clave_e = ? ", cnn);
                    command.Parameters.Add("@clave_e", OdbcType.Int).Value =id;
                    Boleto_Sal _boleta_sal = new Boleto_Sal();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _boleta_sal = new Boleto_Sal();
                            _boleta_sal.clave_e = Convert.ToInt64(reader.GetValue(0).ToString());
                            _boleta_sal.clave_o = reader.GetValue(1).ToString();
                            _boleta_sal.fecha_s = Convert.ToDateTime(reader.GetValue(2).ToString());
                            _boleta_sal.hora_s = reader.GetValue(3).ToString();
                            _boleta_sal.peso_s = Convert.ToDouble(reader.GetValue(4));
                            _boleta_sal.peso_n = Convert.ToDouble(reader.GetValue(5));
                            _boleta_sal.observa_s = reader.GetValue(6).ToString();
                            _boleta_sal.turno_s = reader.GetValue(7).ToString();
                            _boleta_sal.bascula_s = reader.GetValue(8).ToString();
                            _boleta_sal.s_manual = Convert.ToBoolean(reader.GetValue(9));

                           _boleta_entq[0].Boleto_Sal = _boleta_sal;

                            //_salidasnoexistentes.Add(_boleta_ent);


                        }
                    }

                   // _boleta_ent.Boleto_Sal = _boleta_sal;

                    var formatter = new JsonMediaTypeFormatter();
                    var json = formatter.SerializerSettings;

                    json.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                    json.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                    json.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    json.Formatting = Newtonsoft.Json.Formatting.Indented;
                    json.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    //  json.Culture = new CultureInfo("it-IT");


                    //_boleta_ent.Boleto_Sal = _boleta_sal;
                    var serializado = Newtonsoft.Json.JsonConvert.SerializeObject(_boleta_ent);
                    var resp = new HttpResponseMessage()
                    {
                        Content = new StringContent(serializado),
                        StatusCode = HttpStatusCode.OK,
                         
                         
                    };
                   // resp.Headers.Add("Access-Control-Allow-Origin", ConfigurationManager.AppSettings["clientepermitido"]);
                    resp.Headers.Add("Access-Control-Allow-Origin", ConfigurationManager.AppSettings["clientepermitido2"]);

                    return resp;
                   // Request.Headers.Add("Access-Control-Allow-Origin", "http://localhost:1371");
                    //return Request.CreateResponse(HttpStatusCode.OK, _boleta_ent, formatter);


                    //return Request.CreateResponse(HttpStatusCode.OK, Newtonsoft.Json.JsonConvert.SerializeObject(_boleta_ent));
                    //return Request.CreateResponse(HttpStatusCode.OK, _boleta_ent, JsonMediaTypeFormatter.DefaultMediaType);
                }
                catch (Exception ex)
                {

                    throw ex;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }


}
