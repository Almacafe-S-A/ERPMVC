using System;
using System.Collections.Generic;
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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ControlAsistenciasController : Controller
    {
        private readonly IOptions<MyConfig> _config;
        private readonly ILogger _logger;

        public ControlAsistenciasController(ILogger<EstadosController> logger, IOptions<MyConfig> config)
        {
            this._config = config;
            this._logger = logger;

        }

        //public IActionResult ControlAsistencia()
        //{
        //    return View();
        //}



        public IActionResult DiasMes()
        {
            return View();
        }


        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> ControlAsistencia()
        {
            ViewData["ElementoConfiguracion"] = await ObtenerTiposControlAsistencias();

            return View();
        }

        /// <summary>
        /// Obitiene el listado de los estados!
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<DataSourceResult> GetControlAsistencias([DataSourceRequest]DataSourceRequest request)
        {
            List<ControlAsistencias> _ControlAsistencias = new List<ControlAsistencias>();
            try
            {

                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ControlAsistencias/GetControlAsistencias");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlAsistencias = JsonConvert.DeserializeObject<List<ControlAsistencias>>(valorrespuesta);
                    _ControlAsistencias = _ControlAsistencias.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ControlAsistencias.ToDataSourceResult(request);

        }

        async Task<IEnumerable<ElementoConfiguracion>> ObtenerTiposControlAsistencias()
        {
            IEnumerable<ElementoConfiguracion> TA = null;
            Int64 Elemento = 31;
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementoConfiguracionByIdConfiguracion/" + Elemento);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    TA = JsonConvert.DeserializeObject<IEnumerable<ElementoConfiguracion>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            ViewData["defaultElementoConfiguracion"] = TA.FirstOrDefault();
            return TA;

        }





        [HttpGet]
        public async Task<DataSourceResult> GetGetControlAsistencias([DataSourceRequest]DataSourceRequest request/*, string primero, string actual*/)
        {

            //var uno = primero;
            //var dia = actual;
            // var actualfecha = dia.ToString("yyyy-MM-dd");

            //String FechaTexto = "14/04/2014 9:10:45";
            //DateTime fecha = DateTime.Parse(FechaTexto);
            //FechaTexto = fecha.ToString("dd/MM/yyyy");


            List<ControlAsistenciasDTO> _ControlAsistencias = new List<ControlAsistenciasDTO>();


            //Cargar de lista de empleados
            List<Employees> _ListEmpleados = new List<Employees>();


            try
            {

                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployees");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ListEmpleados = JsonConvert.DeserializeObject<List<Employees>>(valorrespuesta);
                    //  _ListEmpleados = _ListEmpleados.OrderByDescending(q => q.IdEmpleado).ToList();
                }

                // Fin de lista de empleados

                int c = 1;

                foreach (var _ListEmpleadosLis in _ListEmpleados)
                {
                    ControlAsistenciasDTO NuevaControlAsistencia = new ControlAsistenciasDTO();
                    NuevaControlAsistencia.Empleado = _ListEmpleadosLis;
                    NuevaControlAsistencia.Contador = c++;

                    var fechas = await GetControlAsistenciasByEmpl(NuevaControlAsistencia/*, primero, actual*/);


                    List<ControlAsistencias> LSCA = ((List<ControlAsistencias>)fechas.Value);
                    int hola = 0;
                    //Hacer un foreach para recorer fechas luego hacer el swicht
                    //-----------------------------------------------------------------
                    foreach (var _Listardias in LSCA)
                    {
                        // ControlAsistenciasDTO NuevaFila = new ControlAsistenciasDTO();
                        //NuevaFila.Fecha = _Listardias;
                        //_ControlAsistencias.Add(NuevaFila);

                        // var custData = (_Listardias);
                        //var Iditems = _Listardias.Empleado.IdEmpleado;
                        //var dias = (ControlAsistenciasDTO); 



                        switch (_Listardias.Fecha.Day)
                        {
                            case 1:
                                NuevaControlAsistencia.Dia1 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia1TA = _Listardias.TipoAsistencia;
                                //  NuevaControlAsistencia.Dia=
                                break;
                            case 2:
                                NuevaControlAsistencia.Dia2 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia2TA = _Listardias.TipoAsistencia;
                                break;
                            case 3:
                                NuevaControlAsistencia.Dia3 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia3TA = _Listardias.TipoAsistencia;
                                break;
                            case 4:
                                NuevaControlAsistencia.Dia4 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia4TA = _Listardias.TipoAsistencia;
                                break;
                            case 5:
                                NuevaControlAsistencia.Dia5 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia5TA = _Listardias.TipoAsistencia;
                                break;
                            case 6:
                                NuevaControlAsistencia.Dia6 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia6TA = _Listardias.TipoAsistencia;
                                break;
                            case 7:
                                NuevaControlAsistencia.Dia7 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia7TA = _Listardias.TipoAsistencia;
                                break;
                            case 8:
                                NuevaControlAsistencia.Dia8 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia8TA = _Listardias.TipoAsistencia;
                                break;
                            case 9:
                                NuevaControlAsistencia.Dia9 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia9TA = _Listardias.TipoAsistencia;
                                break;
                            case 10:
                                NuevaControlAsistencia.Dia10 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia10TA = _Listardias.TipoAsistencia;
                                break;
                            case 11:
                                NuevaControlAsistencia.Dia11 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia11TA = _Listardias.TipoAsistencia;
                                break;
                            case 12:
                                NuevaControlAsistencia.Dia12 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia12TA = _Listardias.TipoAsistencia;
                                break;
                            case 13:
                                NuevaControlAsistencia.Dia13 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia13TA = _Listardias.TipoAsistencia;
                                break;
                            case 14:
                                NuevaControlAsistencia.Dia14 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia14TA = _Listardias.TipoAsistencia;
                                break;
                            case 15:
                                NuevaControlAsistencia.Dia15 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia15TA = _Listardias.TipoAsistencia;
                                break;
                            case 16:
                                NuevaControlAsistencia.Dia16 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia16TA = _Listardias.TipoAsistencia;
                                break;
                            case 17:
                                NuevaControlAsistencia.Dia17 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia17TA = _Listardias.TipoAsistencia;
                                break;
                            case 18:
                                NuevaControlAsistencia.Dia18 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia18TA = _Listardias.TipoAsistencia;
                                break;
                            case 19:
                                NuevaControlAsistencia.Dia19 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia19TA = _Listardias.TipoAsistencia;
                                break;
                            case 20:
                                NuevaControlAsistencia.Dia20 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia20TA = _Listardias.TipoAsistencia;
                                break;
                            case 21:
                                NuevaControlAsistencia.Dia21 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia1TA = _Listardias.TipoAsistencia;
                                break;
                            case 22:
                                NuevaControlAsistencia.Dia22 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia21TA = _Listardias.TipoAsistencia;
                                break;
                            case 23:
                                NuevaControlAsistencia.Dia23 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia22TA = _Listardias.TipoAsistencia;
                                break;
                            case 24:
                                NuevaControlAsistencia.Dia24 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia24TA = _Listardias.TipoAsistencia;
                                break;
                            case 25:
                                NuevaControlAsistencia.Dia25 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia25TA = _Listardias.TipoAsistencia;
                                break;
                            case 26:
                                NuevaControlAsistencia.Dia26 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia26TA = _Listardias.TipoAsistencia;
                                break;
                            case 27:
                                NuevaControlAsistencia.Dia27 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia27TA = _Listardias.TipoAsistencia;
                                break;
                            case 28:
                                NuevaControlAsistencia.Dia28 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia28TA = _Listardias.TipoAsistencia;
                                break;
                            case 29:
                                NuevaControlAsistencia.Dia29 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia29TA = _Listardias.TipoAsistencia;
                                break;
                            case 30:
                                NuevaControlAsistencia.Dia30 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia30TA = _Listardias.TipoAsistencia;
                                break;
                            case 31:
                                NuevaControlAsistencia.Dia31 = _Listardias.Fecha;
                                NuevaControlAsistencia.Dia31TA = _Listardias.TipoAsistencia;
                                break;
                            default:

                                break;

                        }


                        //    //return null;
                    }

                    _ControlAsistencias.Add(NuevaControlAsistencia);
                    //NuevaControlAsistencia.Empleado.IdEmpleado = _ListEmpleadosLis.IdEmpleado;
                    //DateTime crea = DateTime.Now;
                    //DateTime modi = DateTime.Now;   

                    //-------------------------------------------------------------------------
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }








            //try
            //{

            //    string baseadress = _config.Value.urlbase;
            //    HttpClient _client = new HttpClient();
            //    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            //    var result = await _client.GetAsync(baseadress + "api/ControlAsistencias/GetControlAsistenciasByEmployeeId");
            //    string valorrespuesta = "";
            //    if (result.IsSuccessStatusCode)
            //    {
            //        valorrespuesta = await (result.Content.ReadAsStringAsync());
            //        _ListEmpleados = JsonConvert.DeserializeObject<List<Employees>>(valorrespuesta);
            //        _ListEmpleados = _ListEmpleados.OrderByDescending(q => q.IdEmpleado).ToList();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
            //    throw ex;
            //}

            //----------------------------------------------------------------------------------------


            return _ControlAsistencias.ToDataSourceResult(request);

        }

        [HttpPost]
        public async Task<JsonResult> GetControlAsistenciasByEmpl(ControlAsistencias NuevaControlAsistencia/*, string primero, string actual*/)
        {
            //var primerdiames = Convert.ToDateTime(primero);
            //var diaactual = Convert.ToDateTime(actual);

            //DateTime Fecha = new DateTime(01-11-2019);
            List<ControlAsistencias> ControlAsistencia = new List<ControlAsistencias>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                NuevaControlAsistencia.FechaCreacion = new DateTime(2019, 11, 01);
                NuevaControlAsistencia.FechaModificacion = DateTime.Now;
                NuevaControlAsistencia.UsuarioCreacion = HttpContext.Session.GetString("user");
                NuevaControlAsistencia.UsuarioModificacion = HttpContext.Session.GetString("user");
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/ControlAsistencias/GetControlAsistenciasByEmployeeId", NuevaControlAsistencia);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    ControlAsistencia = JsonConvert.DeserializeObject<List<ControlAsistencias>>(valorrespuesta);

                }

                //foreach (var data in _ListEmpleados)
                //{

                //    ControlAsistencias NuevaControlAsistencias = new ControlAsistencias();
                //    NuevaControlAsistencias.Empleado = data;
                //    _ControlAsistencias.Add(NuevaControlAsistencias);

                //}
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(ControlAsistencia);
        }

        [HttpGet]
        public async Task<ActionResult> GetSumControlAsistenciasByEmployeeId(Int64 EmpleadoId)
        {
            ControlAsistencias _suma = new ControlAsistencias();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ControlAsistencias/GetSumControlAsistenciasByEmployeeId/" + EmpleadoId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _suma = JsonConvert.DeserializeObject<ControlAsistencias>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return View(_suma);
        }

        [HttpPost]
        public async Task<ActionResult<ControlAsistencias>> Insert(ControlAsistencias _ControlAsistencia)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ControlAsistencia.UsuarioCreacion = HttpContext.Session.GetString("user");
                _ControlAsistencia.FechaCreacion = DateTime.Now;
                _ControlAsistencia.UsuarioModificacion = HttpContext.Session.GetString("user");
                _ControlAsistencia.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/ControlAsistencias/Insert", _ControlAsistencia);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlAsistencia = JsonConvert.DeserializeObject<ControlAsistencias>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }


            return Ok(_ControlAsistencia);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _ExchangeRate }, Total = 1 });
        }

        [HttpPut("Id")]
        public async Task<IActionResult> Update(Int64 Id, ControlAsistencias _ControlAsistencia)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/ControlAsistencias/Update", _ControlAsistencia);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlAsistencia = JsonConvert.DeserializeObject<ControlAsistencias>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ControlAsistencia }, Total = 1 });
        }

        [HttpPost("PostControlAsistencias")]
        public async Task<ActionResult<ControlAsistencias>> PostControlAsistencias(ControlAsistenciasDTO _CtlAsis)
        {

            //foreach
            //swith



            ControlAsistencias _ControlAsistencia = _CtlAsis;
            try
            {
                ControlAsistencias _listAccount = new ControlAsistencias();
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                // var result = await _client.GetAsync(baseadress + "api/ExchangeRate/GetExchangeRateById/" + _ExchangeRate.ExchangeRateId);
                if (_ControlAsistencia.Fecha == null)
                {
                    _ControlAsistencia.UsuarioCreacion = HttpContext.Session.GetString("user");
                    _ControlAsistencia.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var resultdate = await _client.PostAsJsonAsync(baseadress + "api/ControlAsistencias/GetControlAsistenciasByFecha", _ControlAsistencia);
                    string valorrespuesta = "";
                    //PRUEBA
                    //decimal valor = Convert.ToDecimal(_ExchangeRate.ExchangeRateValue);
                    //decimal temp = decimal.Round(valor, 4, MidpointRounding.AwayFromZero);
                    // temp = decimal.Parse(temp.ToString("N4"));
                    //string a = String.Format("{0:F2}", temp);                  
                    //temp = decimal.Parse(a); 
                    //decimal variable1 = Convert.ToDecimal(temp, System.Globalization.CultureInfo.InvariantCulture);
                    //_ExchangeRate.ExchangeRateValue = Convert.ToDecimal(variable1);
                    _ControlAsistencia.FechaModificacion = DateTime.Now;
                    _ControlAsistencia.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (resultdate.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (resultdate.Content.ReadAsStringAsync());
                        _ControlAsistencia = JsonConvert.DeserializeObject<ControlAsistencias>(valorrespuesta);

                        if (_ControlAsistencia == null)
                        {
                            _ControlAsistencia = new Models.ControlAsistencias();
                        }

                        //if (_ExchangeRate.Id > 0)
                        //{
                        //    return await Task.Run(() => BadRequest($"Ya existe."));
                        //}
                    }

                }

                ///
                if (_ControlAsistencia.Id == 0)
                {
                    _ControlAsistencia.Fecha = DateTime.Now;
                    _ControlAsistencia.Empleado.IdEmpleado = 14;
                    _ControlAsistencia.Dia = 1;
                    _ControlAsistencia.TipoAsistencia = 80;

                    var insertresult = await Insert(_CtlAsis);
                    var value = (insertresult.Result as ObjectResult).Value;
                    ControlAsistencias resultado = ((ControlAsistencias)(value));
                    if (resultado.Id <= 0)
                    {
                        return await Task.Run(() => BadRequest($"No se guardo la asistencia."));
                    }
                }
                else
                {
                    var result = await _client.GetAsync(baseadress + "api/ControlAsistencias/GetControlAsistenciasById/" + _ControlAsistencia.Id);
                    string valorrespuesta = "";

                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _ControlAsistencia = JsonConvert.DeserializeObject<ControlAsistencias>(valorrespuesta);

                        if (_ControlAsistencia == null)
                        {
                            _ControlAsistencia = new Models.ControlAsistencias();
                        }


                    }

                    _ControlAsistencia.UsuarioCreacion = _ControlAsistencia.UsuarioCreacion;
                    _ControlAsistencia.FechaCreacion = _ControlAsistencia.FechaCreacion;
                    _ControlAsistencia.UsuarioModificacion = HttpContext.Session.GetString("user");
                    _ControlAsistencia.FechaModificacion = DateTime.Now;

                    var updateresult = await Update(_ControlAsistencia.Id, _CtlAsis);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CtlAsis);

        }


        [HttpGet]
        public async Task<DataSourceResult> GetListEmployees([DataSourceRequest]DataSourceRequest request)
        {
            List<Employees> _ListEmpleados = new List<Employees>();
            try
            {

                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployees");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ListEmpleados = JsonConvert.DeserializeObject<List<Employees>>(valorrespuesta);
                    _ListEmpleados = _ListEmpleados.OrderByDescending(q => q.IdEmpleado).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ListEmpleados.ToDataSourceResult(request);

        }


        [HttpGet]
        public async Task<DataSourceResult> GetDias([DataSourceRequest]DataSourceRequest request)
        {
            List<Employees> _ListEmpleados = new List<Employees>();
            try
            {

                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployees");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ListEmpleados = JsonConvert.DeserializeObject<List<Employees>>(valorrespuesta);
                    _ListEmpleados = _ListEmpleados.OrderByDescending(q => q.IdEmpleado).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ListEmpleados.ToDataSourceResult(request);

        }




    }
}