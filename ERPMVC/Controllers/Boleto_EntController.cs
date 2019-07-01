using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.DTO;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class Boleto_EntController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public Boleto_EntController(ILogger<Boleto_EntController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> pvwBoleto_Ent(Int64 Id = 0)
        {
            Boleto_Ent _Boleto_Ent = new Boleto_Ent();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Boleto_Ent/GetBoleto_EntById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Boleto_Ent = JsonConvert.DeserializeObject<Boleto_Ent>(valorrespuesta);

                }

                if (_Boleto_Ent == null)
                {
                    _Boleto_Ent = new Boleto_Ent();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Boleto_Ent);

        }

        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Boleto_Ent> _Boleto_Ent = new List<Boleto_Ent>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Boleto_Ent/GetBoleto_Ent");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Boleto_Ent = JsonConvert.DeserializeObject<List<Boleto_Ent>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Boleto_Ent.ToDataSourceResult(request);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetBoleto_EntById([DataSourceRequest]DataSourceRequest request, Boleto_Ent _Boleto_Entp)
        {
          PesajeDTO _Pesaje = new PesajeDTO();
            try
            {

                string connetionString = null;
                OdbcConnection cnn;
                connetionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\tomaturnos\Desktop\Old\Projects\BI\Documentos\ALMACAFE\Integracion balanza\rev_2000.accdb";
                cnn = new OdbcConnection(connetionString);
                try
                {
                    cnn.Open();
                    // Console.WriteLine("Connection Open ! ");
                   // OdbcConnection connection = new OdbcConnection("");
                    OdbcCommand command = new OdbcCommand("SELECT * FROM boleto_ent WHERE clave_e = ? ", cnn);
                    command.Parameters.Add("@clave_e", OdbcType.Int).Value = _Boleto_Entp.clave_e;

                    Boleto_Ent _boleta_ent = new Boleto_Ent();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _boleta_ent.clave_e =   Convert.ToInt64(reader.GetValue(0).ToString());
                            _boleta_ent.clave_C =   reader.GetValue(1).ToString();
                            _boleta_ent.clave_o =   reader.GetValue(2).ToString();
                            _boleta_ent.clave_p =   reader.GetValue(3).ToString();
                            _boleta_ent.completo =  Convert.ToBoolean(reader.GetValue(4).ToString());
                            _boleta_ent.fecha_e =   Convert.ToDateTime(reader.GetValue(5).ToString());
                            _boleta_ent.hora_e =    reader.GetValue(6).ToString();
                            _boleta_ent.placas =    reader.GetValue(7).ToString();

                            _boleta_ent.conductor = reader.GetValue(8).ToString();
                            _boleta_ent.peso_e =    Convert.ToInt32(reader.GetValue(9).ToString());
                            _boleta_ent.observa_e = reader.GetValue(10).ToString();
                            _boleta_ent.nombre_oe = reader.GetValue(11).ToString();
                            _boleta_ent.turno_oe =  reader.GetValue(12).ToString();
                            _boleta_ent.unidad_e =  reader.GetValue(13).ToString();

                            _boleta_ent.bascula_e = reader.GetValue(14).ToString();
                            _boleta_ent.t_entrada = Convert.ToInt32(reader.GetValue(15).ToString());
                            _boleta_ent.clave_u = reader.GetValue(16).ToString();
                            // Word is from the database. Do something with it.
                        }
                    }

                    OdbcCommand commandtara = new OdbcCommand("SELECT * FROM tara WHERE t_placas = ? ", cnn);
                    commandtara.Parameters.Add("@t_placas", OdbcType.VarChar).Value = _boleta_ent.placas;
                    Tara _Tara = new Tara();
                    using (OdbcDataReader reader2 = commandtara.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            _Tara.t_placas = reader2.GetValue(0).ToString();

                            _Tara.t_peso = Convert.ToDouble(reader2.GetValue(1).ToString());
                            _Tara.t_unidad = reader2.GetValue(2).ToString();
                            _Tara.t_fecha = Convert.ToDateTime(reader2.GetValue(3).ToString());
                            _Tara.t_captura = reader2.GetValue(4).ToString();
                            _Tara.t_observaciones = reader2.GetValue(5).ToString();
                         
                        }
                    }

                     _Pesaje.Boleto_Ent = _boleta_ent;
                    _Pesaje.Tara = _Tara;
                    cnn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Can not open connection ! " + ex.Message);
                }

                
                //string baseadress = config.Value.urlbase;
                //HttpClient _client = new HttpClient();
                //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //var result = await _client.GetAsync(baseadress + "api/Boleto_Ent/GetBoleto_Ent");
                //string valorrespuesta = "";
                //if (result.IsSuccessStatusCode)
                //{
                //    valorrespuesta = await (result.Content.ReadAsStringAsync());
                //    _Pesaje = JsonConvert.DeserializeObject<List<Boleto_Ent>>(valorrespuesta);

                //}


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Pesaje); //_Pesaje.ToDataSourceResult(request);

        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<Boleto_Ent>> SaveBoleto_Ent([FromBody]Boleto_Ent _Boleto_Ent)
        {

            try
            {
                Boleto_Ent _listBoleto_Ent = new Boleto_Ent();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Boleto_Ent/GetBoleto_EntById/" + _Boleto_Ent.clave_e);
                string valorrespuesta = "";
               // _Boleto_Ent.FechaModificacion = DateTime.Now;
                //_Boleto_Ent.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listBoleto_Ent = JsonConvert.DeserializeObject<Boleto_Ent>(valorrespuesta);
                }

                if (_listBoleto_Ent.clave_e == 0)
                {
                    //_Boleto_Ent.FechaCreacion = DateTime.Now;
                   // _Boleto_Ent.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_Boleto_Ent);
                }
                else
                {
                    var updateresult = await Update(_Boleto_Ent.clave_e, _Boleto_Ent);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Boleto_Ent);
        }

        // POST: Boleto_Ent/Insert
        [HttpPost("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Boleto_Ent>> Insert(Boleto_Ent _Boleto_Ent)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
               // _Boleto_Ent.UsuarioCreacion = HttpContext.Session.GetString("user");
                //_Boleto_Ent.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Boleto_Ent/Insert", _Boleto_Ent);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Boleto_Ent = JsonConvert.DeserializeObject<Boleto_Ent>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_Boleto_Ent);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _Boleto_Ent }, Total = 1 });
        }

        [HttpPut("[controller]/[action]/{id}")]
        public async Task<ActionResult<Boleto_Ent>> Update(Int64 id, Boleto_Ent _Boleto_Ent)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/Boleto_Ent/Update", _Boleto_Ent);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Boleto_Ent = JsonConvert.DeserializeObject<Boleto_Ent>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Boleto_Ent }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<Boleto_Ent>> Delete([FromBody]Boleto_Ent _Boleto_Ent)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Boleto_Ent/Delete", _Boleto_Ent);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Boleto_Ent = JsonConvert.DeserializeObject<Boleto_Ent>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _Boleto_Ent }, Total = 1 });
        }





    }
}