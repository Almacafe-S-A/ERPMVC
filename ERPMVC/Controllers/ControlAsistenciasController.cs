using System;
using System.Collections.Generic;
using System.Globalization;
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

        [HttpPost]
        public async Task<JsonResult> GetSumControlAsistenciasByEmployeeId(Int64 empleado, DateTime pemes, DateTime ultimafecha, short TipoAsistencia)
        {
            ControlAsistencias _suma = new ControlAsistencias();
            try
            {
                ControlAsistencias prueba = new ControlAsistencias();
                prueba.IdEmpleado = empleado;
                prueba.FechaCreacion = pemes;
                prueba.FechaModificacion = ultimafecha;
                prueba.TipoAsistencia = TipoAsistencia;
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ControlAsistencias/GetSumControlAsistenciasByEmployeeId" + prueba);
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
            return Json(_suma);
        }

        [HttpPost]
        public async Task<JsonResult> CantidadTipoAsietnciaByEmpleado(ControlAsistencias NuevaControlAsistencia, DateTime primero, DateTime actual, Int64 TipoAsistencia)
        {
            Int32 ControlAsistencia = 0;
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                NuevaControlAsistencia.IdEmpleado = NuevaControlAsistencia.Empleado.IdEmpleado;
                NuevaControlAsistencia.TipoAsistencia = TipoAsistencia;
                NuevaControlAsistencia.FechaCreacion = primero;
                NuevaControlAsistencia.FechaModificacion = actual;
                NuevaControlAsistencia.UsuarioCreacion = HttpContext.Session.GetString("user");
                NuevaControlAsistencia.UsuarioModificacion = HttpContext.Session.GetString("user");
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/ControlAsistencias/GetSumControlAsistenciasByEmployeeId", NuevaControlAsistencia);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    ControlAsistencia = JsonConvert.DeserializeObject<Int32>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(ControlAsistencia);
        }


        [HttpPost]
        public async Task<JsonResult> ColorTipoAsistencia(Int32 Id)
        {
            ElementoConfiguracion Ele = new ElementoConfiguracion();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementoConfiguracionById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    Ele = JsonConvert.DeserializeObject<ElementoConfiguracion>(valorrespuesta);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(Ele);
        }


        [HttpGet]
        public async Task<DataSourceResult> GetGetControlAsistencias([DataSourceRequest]DataSourceRequest request, string primero, string actual)
        {
            var uno = primero;
            var diactual = actual;
            var pmes = Convert.ToDateTime(uno);
            var dctual = Convert.ToDateTime(diactual);

            DateTime fechafindmes = DateTime.Now;

            if (uno == null && diactual == null)
            {
                fechafindmes = DateTime.Now;
                pmes = Convert.ToDateTime(fechafindmes.Month + "-" + 1 + "-" + fechafindmes.Year);

            }
            else if (pmes.Month == DateTime.Now.Month)
            {
                fechafindmes = DateTime.Now;

            }
            else
            {
                int me = pmes.Month;
                int ao = pmes.Year;
                int diass = DateTime.DaysInMonth(pmes.Year, pmes.Month);
                string fechavieja =  me + "-" + diass + "-" + ao;
                fechafindmes = Convert.ToDateTime(fechavieja);
            }
            int letras = DateTime.DaysInMonth(fechafindmes.Year, fechafindmes.Month);


            List<ControlAsistenciasDTO> _ControlAsistencias = new List<ControlAsistenciasDTO>();
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
                }
                foreach (var _ListEmpleadosLis in _ListEmpleados)
                {
                    ControlAsistenciasDTO NuevaControlAsistencia = new ControlAsistenciasDTO();
                    NuevaControlAsistencia.Empleado = _ListEmpleadosLis;
                    NuevaControlAsistencia.EmployeesId = _ListEmpleadosLis.IdEmpleado;
                    //-------------------------------------Iniciarl de el dia----------------------------------------------------

                    var l1 = Convert.ToDateTime(fechafindmes.Month + "-" + 1 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD1 = l1.ToString("dddd").Substring(0, 1).ToUpper();

                    var l2 = Convert.ToDateTime(fechafindmes.Month + "-" + 2 + "-" + fechafindmes.Year);
                    NuevaControlAsistencia.LetraD2 = l2.ToString("dddd").Substring(0, 1).ToUpper();

                    var l3 = Convert.ToDateTime(fechafindmes.Month + "-" + 3 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD3 = l3.ToString("dddd").Substring(0, 1).ToUpper();

                    var l4 = Convert.ToDateTime(fechafindmes.Month + "-" + 4 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD4 = l4.ToString("dddd").Substring(0, 1).ToUpper();

                    var l5 = Convert.ToDateTime(fechafindmes.Month + "-" + 5 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD5 = l5.ToString("dddd").Substring(0, 1).ToUpper();

                    var l6 = Convert.ToDateTime(fechafindmes.Month + "-" + 6 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD6 = l6.ToString("dddd").Substring(0, 1).ToUpper();

                    var l7 = Convert.ToDateTime(fechafindmes.Month + "-" + 7 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD7 = l7.ToString("dddd").Substring(0, 1).ToUpper();

                    var l8 = Convert.ToDateTime(fechafindmes.Month + "-" + 8 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD8 = l8.ToString("dddd").Substring(0, 1).ToUpper();

                    var l9 = Convert.ToDateTime(fechafindmes.Month + "-" + 9 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD9 = l9.ToString("dddd").Substring(0, 1).ToUpper();

                    var l10 = Convert.ToDateTime(fechafindmes.Month + "-" + 10 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD10 = l10.ToString("dddd").Substring(0, 1).ToUpper();

                    var l11 = Convert.ToDateTime(fechafindmes.Month + "-" + 11 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD11 = l11.ToString("dddd").Substring(0, 1).ToUpper();

                    var l12 = Convert.ToDateTime(fechafindmes.Month + "-" + 12 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD12 = l12.ToString("dddd").Substring(0, 1).ToUpper();

                    var l13 = Convert.ToDateTime(fechafindmes.Month + "-" + 13 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD13 = l13.ToString("dddd").Substring(0, 1).ToUpper();

                    var l14 = Convert.ToDateTime(fechafindmes.Month + "-" + 14 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD14 = l14.ToString("dddd").Substring(0, 1).ToUpper();

                    var l15 = Convert.ToDateTime(fechafindmes.Month + "-" + 15 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD15 = l15.ToString("dddd").Substring(0, 1).ToUpper();

                    var l16 = Convert.ToDateTime(fechafindmes.Month + "-" + 16 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD16 = l16.ToString("dddd").Substring(0, 1).ToUpper();

                    var l17 = Convert.ToDateTime(fechafindmes.Month + "-" + 17 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD17 = l17.ToString("dddd").Substring(0, 1).ToUpper();

                    var l18 = Convert.ToDateTime(fechafindmes.Month + "-" + 18 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD18 = l18.ToString("dddd").Substring(0, 1).ToUpper();

                    var l19 = Convert.ToDateTime(fechafindmes.Month + "-" + 19 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD19 = l19.ToString("dddd").Substring(0, 1).ToUpper();

                    var l20 = Convert.ToDateTime(fechafindmes.Month + "-" + 20 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD20 = l20.ToString("dddd").Substring(0, 1).ToUpper();

                    var l21 = Convert.ToDateTime(fechafindmes.Month + "-" + 21 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD21 = l21.ToString("dddd").Substring(0, 1).ToUpper();

                    var l22 = Convert.ToDateTime(fechafindmes.Month + "-" + 22 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD22 = l22.ToString("dddd").Substring(0, 1).ToUpper();

                    var l23 = Convert.ToDateTime(fechafindmes.Month + "-" + 23 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD23 = l23.ToString("dddd").Substring(0, 1).ToUpper();

                    var l24 = Convert.ToDateTime(fechafindmes.Month + "-" + 24 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD24 = l24.ToString("dddd").Substring(0, 1).ToUpper();

                    var l25 = Convert.ToDateTime(fechafindmes.Month + "-" + 25 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD25 = l25.ToString("dddd").Substring(0, 1).ToUpper();

                    var l26 = Convert.ToDateTime(fechafindmes.Month + "-" + 26 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD26 = l26.ToString("dddd").Substring(0, 1).ToUpper();

                    var l27 = Convert.ToDateTime(fechafindmes.Month + "-" + 27 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD27 = l27.ToString("dddd").Substring(0, 1).ToUpper();

                    var l28 = Convert.ToDateTime(fechafindmes.Month + "-" + 28 + "-"+ fechafindmes.Year);
                    NuevaControlAsistencia.LetraD28 = l28.ToString("dddd").Substring(0, 1).ToUpper();

                    if (letras == 29)
                    {
                        var l29 = Convert.ToDateTime(fechafindmes.Month + "-" + 29 + "-"+ fechafindmes.Year);
                        NuevaControlAsistencia.LetraD29 = l29.ToString("dddd").Substring(0, 1).ToUpper();
                    }
                    if (letras == 30)
                    {
                        var l29 = Convert.ToDateTime(fechafindmes.Month + "-" + 29 + "-"+ fechafindmes.Year);
                        NuevaControlAsistencia.LetraD29 = l29.ToString("dddd").Substring(0, 1).ToUpper();

                        var l30 = Convert.ToDateTime(fechafindmes.Month + "-" + 30 + "-"+ fechafindmes.Year);
                        NuevaControlAsistencia.LetraD30 = l30.ToString("dddd").Substring(0, 1).ToUpper();
                    }
                    if (letras == 31)
                    {
                        var l29 = Convert.ToDateTime(fechafindmes.Month + "-" + 29 + "-"+ fechafindmes.Year);
                        NuevaControlAsistencia.LetraD29 = l29.ToString("dddd").Substring(0, 1).ToUpper();

                        var l30 = Convert.ToDateTime(fechafindmes.Month + "-" + 30 + "-"+ fechafindmes.Year);
                        NuevaControlAsistencia.LetraD30 = l30.ToString("dddd").Substring(0, 1).ToUpper();

                        var l31 = Convert.ToDateTime(fechafindmes.Month + "-" + 31 + "-"+ fechafindmes.Year);
                        NuevaControlAsistencia.LetraD31 = l31.ToString("dddd").Substring(0, 1).ToUpper();
                    }

                    //-------------------------------------------------------------------------------------------------------------
                    //Calculos
                    //-----------------------------Llegadas Tarde por Empleado--------------------------------------------
                    short LlegadaTarde = 75;
                    var Llegatarde = await CantidadTipoAsietnciaByEmpleado(NuevaControlAsistencia, pmes, fechafindmes, LlegadaTarde);
                    NuevaControlAsistencia.LlegadasTarde = Convert.ToInt32(Llegatarde.Value);
                    int cLLegadasTarde = Convert.ToInt32(Llegatarde.Value);
                    //----------------------------------------------------------------------------------------------------

                    short Domingodiaslibres = 76;
                    var DomigosyLibres = await CantidadTipoAsietnciaByEmpleado(NuevaControlAsistencia, pmes, fechafindmes, Domingodiaslibres);
                    int cDomingosandLibres = Convert.ToInt32(DomigosyLibres.Value);
                    //----------------------------------------------------------------------------------------------------
                    short Permisoshoras = 77;
                    var PermisosH = await CantidadTipoAsietnciaByEmpleado(NuevaControlAsistencia, pmes, fechafindmes, Permisoshoras);
                    int cPermisoshora = Convert.ToInt32(PermisosH.Value);
                    //----------------------------------------------------------------------------------------------------
                    short Incapacidad = 78;
                    var Incapacidades = await CantidadTipoAsietnciaByEmpleado(NuevaControlAsistencia, pmes, fechafindmes, Incapacidad);
                    int cIncapacidades = Convert.ToInt32(Incapacidades.Value);
                    //----------------------------------------------------------------------------------------------------
                    short Vacacion = 79;
                    var Vacaciones = await CantidadTipoAsietnciaByEmpleado(NuevaControlAsistencia, pmes, fechafindmes, Vacacion);
                    int cVacaiones = Convert.ToInt32(Vacaciones.Value);
                    //----------------------------------------------------------------------------------------------------
                    short Permiso = 80;
                    var Permisos = await CantidadTipoAsietnciaByEmpleado(NuevaControlAsistencia, pmes, fechafindmes, Permiso);
                    int cPermisos = Convert.ToInt32(Permisos.Value);
                    //----------------------------------------------------------------------------------------------------
                    short Presente = 81;
                    var Presentes = await CantidadTipoAsietnciaByEmpleado(NuevaControlAsistencia, pmes, fechafindmes, Presente);
                    int cPrecente = Convert.ToInt32(Presentes.Value);

                    int DiasLaborales = (cLLegadasTarde + cDomingosandLibres + cPermisoshora + cPrecente);
                    NuevaControlAsistencia.DiasLaborales = DiasLaborales;

                    double porcentajellegadastarde = ((double)DiasLaborales / (double)cLLegadasTarde);
                    if (double.IsNaN(porcentajellegadastarde))
                    {
                        NuevaControlAsistencia.PorcentajeLlegadasTarde = "0%";
                    }
                    else
                    {
                        NuevaControlAsistencia.PorcentajeLlegadasTarde = Convert.ToString(porcentajellegadastarde + "%");
                    }

                    int hola = 0;
                    for (var d = pmes; d < fechafindmes; d = d.AddDays(1))
                    {
                        switch (d.Day)
                        {
                            case 1:
                                var fechasdia = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia = ((List<ControlAsistencias>)fechasdia.Value);

                                if (LSCAdia.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia1 = d;
                                    NuevaControlAsistencia.Dia1TA = 0;
                                    //var l1 = Convert.ToDateTime(1 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD1 = l1.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {

                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD1 = li.Formula;

                                    NuevaControlAsistencia.Dia1 = LSCAdia[0].Fecha;
                                    NuevaControlAsistencia.Dia1TA = LSCAdia[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD1 = LSCAdia[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper(); 
                                }
                                break;
                            case 2:
                                var fechasdia2 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia2 = ((List<ControlAsistencias>)fechasdia2.Value);

                                if (LSCAdia2.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia2 = d;
                                    NuevaControlAsistencia.Dia2TA = 0;
                                    //var l2 = Convert.ToDateTime(2 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD2 = l2.ToString("dddd").Substring(0, 1).ToUpper();

                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia2[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD2 = li.Formula;

                                    NuevaControlAsistencia.Dia2 = LSCAdia2[0].Fecha;
                                    NuevaControlAsistencia.Dia2TA = LSCAdia2[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD2 = LSCAdia2[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                       
                                }

                                break;
                            case 3:
                                var fechasdia3 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia3 = ((List<ControlAsistencias>)fechasdia3.Value);

                                if (LSCAdia3.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia3 = d;
                                    NuevaControlAsistencia.Dia3TA = 0;
                                    //var l3 = Convert.ToDateTime(3 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD3 = l3.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia3[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD3 = li.Formula;
                                    NuevaControlAsistencia.Dia3 = LSCAdia3[0].Fecha;
                                    NuevaControlAsistencia.Dia3TA = LSCAdia3[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD3 = LSCAdia3[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                   
                                }
                                break;
                            case 4:
                                var fechasdia4 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia4 = ((List<ControlAsistencias>)fechasdia4.Value);

                                if (LSCAdia4.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia4 = d;
                                    NuevaControlAsistencia.Dia4TA = 0;
                                    //var l4 = Convert.ToDateTime(4 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD4 = l4.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia4[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD4 = li.Formula;
                                    NuevaControlAsistencia.Dia4 = LSCAdia4[0].Fecha;
                                    NuevaControlAsistencia.Dia4TA = LSCAdia4[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD4 = LSCAdia4[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                    
                                }
                                break;
                            case 5:
                                var fechasdia5 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia5 = ((List<ControlAsistencias>)fechasdia5.Value);

                                if (LSCAdia5.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia5 = d;
                                    NuevaControlAsistencia.Dia5TA = 0;
                                    //var l5 = Convert.ToDateTime(5 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD5 = l5.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia5[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD5 = li.Formula;
                                    NuevaControlAsistencia.Dia5 = LSCAdia5[0].Fecha;
                                    NuevaControlAsistencia.Dia5TA = LSCAdia5[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD5 = LSCAdia5[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                  
                                }
                                break;
                            case 6:
                                var fechasdia6 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia6 = ((List<ControlAsistencias>)fechasdia6.Value);

                                if (LSCAdia6.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia6 = d;
                                    NuevaControlAsistencia.Dia6TA = 0;
                                    //var l6 = Convert.ToDateTime(6+"-"+fechafindmes.Month +"-"+ fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD6 = l6.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia6[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD6 = li.Formula;
                                    NuevaControlAsistencia.Dia6 = LSCAdia6[0].Fecha;
                                    NuevaControlAsistencia.Dia6TA = LSCAdia6[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD6 = LSCAdia6[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                   
                                }
                                break;
                            case 7:
                                var fechasdia7 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia7 = ((List<ControlAsistencias>)fechasdia7.Value);

                                if (LSCAdia7.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia7 = d;
                                    NuevaControlAsistencia.Dia7TA = 0;
                                    //var l7 = Convert.ToDateTime(7 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD7 = l7.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia7[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD7 = li.Formula;
                                    NuevaControlAsistencia.Dia7 = LSCAdia7[0].Fecha;
                                    NuevaControlAsistencia.Dia7TA = LSCAdia7[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD7 = LSCAdia7[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                    
                                }
                                break;
                            case 8:
                                var fechasdia8 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia8 = ((List<ControlAsistencias>)fechasdia8.Value);

                                if (LSCAdia8.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia8 = d;
                                    NuevaControlAsistencia.Dia8TA = 0;
                                    //var l8 = Convert.ToDateTime(8 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD8 = l8.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia8[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD8 = li.Formula;
                                    NuevaControlAsistencia.Dia8 = LSCAdia8[0].Fecha;
                                    NuevaControlAsistencia.Dia8TA = LSCAdia8[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD8 = LSCAdia8[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                    
                                }
                                break;
                            case 9:
                                var fechasdia9 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia9 = ((List<ControlAsistencias>)fechasdia9.Value);

                                if (LSCAdia9.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia9 = d;
                                    NuevaControlAsistencia.Dia9TA = 0;
                                    //var l9 = Convert.ToDateTime(9 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD9 = l9.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia9[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD9 = li.Formula;
                                    NuevaControlAsistencia.Dia9 = LSCAdia9[0].Fecha;
                                    NuevaControlAsistencia.Dia9TA = LSCAdia9[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD9 = LSCAdia9[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                    
                                }
                                break;
                            case 10:
                                var fechasdia10 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia10 = ((List<ControlAsistencias>)fechasdia10.Value);

                                if (LSCAdia10.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia10 = d;
                                    NuevaControlAsistencia.Dia10TA = 0;
                                    //var l10 = Convert.ToDateTime(10 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD10 = l10.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia10[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD10 = li.Formula;
                                    NuevaControlAsistencia.Dia10 = LSCAdia10[0].Fecha;
                                    NuevaControlAsistencia.Dia10TA = LSCAdia10[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD10 = LSCAdia10[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                    
                                }
                                break;
                            case 11:
                                var fechasdia11 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia11 = ((List<ControlAsistencias>)fechasdia11.Value);

                                if (LSCAdia11.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia11 = d;
                                    NuevaControlAsistencia.Dia11TA = 0;
                                    //var l11 = Convert.ToDateTime(11 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD11 = l11.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia11[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD11 = li.Formula;
                                    NuevaControlAsistencia.Dia11 = LSCAdia11[0].Fecha;
                                    NuevaControlAsistencia.Dia11TA = LSCAdia11[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD11 = LSCAdia11[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                    
                                }
                                break;
                            case 12:
                                var fechasdia12 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia12 = ((List<ControlAsistencias>)fechasdia12.Value);

                                if (LSCAdia12.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia12 = d;
                                    NuevaControlAsistencia.Dia21TA = 0;
                                    //var l12 = Convert.ToDateTime(12 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD12 = l12.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia12[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD12 = li.Formula;
                                    NuevaControlAsistencia.Dia12 = LSCAdia12[0].Fecha;
                                    NuevaControlAsistencia.Dia12TA = LSCAdia12[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD12 = LSCAdia12[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                    
                                }
                                break;
                            case 13:
                                var fechasdia13 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia13 = ((List<ControlAsistencias>)fechasdia13.Value);

                                if (LSCAdia13.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia13 = d;
                                    NuevaControlAsistencia.Dia13TA = 0;
                                    //var l13 = Convert.ToDateTime(13 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD13 = l13.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia13[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD13 = li.Formula;
                                    NuevaControlAsistencia.Dia13 = LSCAdia13[0].Fecha;
                                    NuevaControlAsistencia.Dia13TA = LSCAdia13[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD13 = LSCAdia13[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                    
                                }
                                break;
                            case 14:
                                var fechasdia14 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia14 = ((List<ControlAsistencias>)fechasdia14.Value);

                                if (LSCAdia14.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia14 = d;
                                    NuevaControlAsistencia.Dia14TA = 0;
                                    //var l14 = Convert.ToDateTime(14 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD14 = l14.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia14[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD14 = li.Formula;
                                    NuevaControlAsistencia.Dia14 = LSCAdia14[0].Fecha;
                                    NuevaControlAsistencia.Dia14TA = LSCAdia14[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD14 = LSCAdia14[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                   
                                }
                                break;
                            case 15:
                                var fechasdia15 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia15 = ((List<ControlAsistencias>)fechasdia15.Value);

                                if (LSCAdia15.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia15 = d;
                                    NuevaControlAsistencia.Dia15TA = 0;
                                    //var l15 = Convert.ToDateTime(15 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD15 = l15.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia15[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD15 = li.Formula;
                                    NuevaControlAsistencia.Dia15 = LSCAdia15[0].Fecha;
                                    NuevaControlAsistencia.Dia15TA = LSCAdia15[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD15 = LSCAdia15[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper(); 
                                }
                                break;
                            case 16:
                                var fechasdia16 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia16 = ((List<ControlAsistencias>)fechasdia16.Value);

                                if (LSCAdia16.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia16 = d;
                                    NuevaControlAsistencia.Dia16TA = 0;
                                    //var l16 = Convert.ToDateTime(16 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD16 = l16.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia16[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD16 = li.Formula;
                                    NuevaControlAsistencia.Dia16 = LSCAdia16[0].Fecha;
                                    NuevaControlAsistencia.Dia16TA = LSCAdia16[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD16 = LSCAdia16[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                  
                                }
                                break;
                            case 17:
                                var fechasdia17 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia17 = ((List<ControlAsistencias>)fechasdia17.Value);

                                if (LSCAdia17.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia17 = d;
                                    NuevaControlAsistencia.Dia17TA = 0;
                                    //var l17 = Convert.ToDateTime(17 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD17 = l17.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia17[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD17 = li.Formula;
                                    NuevaControlAsistencia.Dia17 = LSCAdia17[0].Fecha;
                                    NuevaControlAsistencia.Dia17TA = LSCAdia17[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD17 = LSCAdia17[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                    
                                }
                                break;
                            case 18:
                                var fechasdia18 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia18 = ((List<ControlAsistencias>)fechasdia18.Value);

                                if (LSCAdia18.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia18 = d;
                                    NuevaControlAsistencia.Dia18TA = 0;
                                    //var l18 = Convert.ToDateTime(18 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD18 = l18.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia18[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD18 = li.Formula;
                                    NuevaControlAsistencia.Dia18 = LSCAdia18[0].Fecha;
                                    NuevaControlAsistencia.Dia18TA = LSCAdia18[1].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD18= LSCAdia18[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                
                                }
                                break;
                            case 19:
                                var fechasdia19 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia19 = ((List<ControlAsistencias>)fechasdia19.Value);

                                if (LSCAdia19.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia19 = d;
                                    NuevaControlAsistencia.Dia19TA = 0;
                                    //var l19 = Convert.ToDateTime(19 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD19 = l19.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia19[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD19 = li.Formula;
                                    NuevaControlAsistencia.Dia19 = LSCAdia19[0].Fecha;
                                    NuevaControlAsistencia.Dia19TA = LSCAdia19[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD19 = LSCAdia19[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                  
                                }
                                break;
                            case 20:
                                var fechasdia20 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia20 = ((List<ControlAsistencias>)fechasdia20.Value);

                                if (LSCAdia20.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia20 = d;
                                    NuevaControlAsistencia.Dia20TA = 0;
                                    //var l20 = Convert.ToDateTime(20 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD20 = l20.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia20[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD20 = li.Formula;
                                    NuevaControlAsistencia.Dia20 = LSCAdia20[0].Fecha;
                                    NuevaControlAsistencia.Dia20TA = LSCAdia20[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD20 = LSCAdia20[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                    
                                }
                                break;
                            case 21:
                                var fechasdia21 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia21 = ((List<ControlAsistencias>)fechasdia21.Value);

                                if (LSCAdia21.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia21 = d;
                                    NuevaControlAsistencia.Dia21TA = 0;
                                    //var l21 = Convert.ToDateTime(21 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD21 = l21.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia21[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD21 = li.Formula;
                                    NuevaControlAsistencia.Dia21 = LSCAdia21[0].Fecha;
                                    NuevaControlAsistencia.Dia21TA = LSCAdia21[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD21 = LSCAdia21[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper(); 
                                }
                                break;
                            case 22:
                                var fechasdia22 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia22 = ((List<ControlAsistencias>)fechasdia22.Value);

                                if (LSCAdia22.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia22 = d;
                                    NuevaControlAsistencia.Dia22TA = 0;
                                    //var l22 = Convert.ToDateTime(22 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD22 = l22.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia22[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD22 = li.Formula;
                                    NuevaControlAsistencia.Dia22 = LSCAdia22[0].Fecha;
                                    NuevaControlAsistencia.Dia22TA = LSCAdia22[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD22 = LSCAdia22[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                    
                                }
                                break;
                            case 23:
                                var fechasdia23 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia23 = ((List<ControlAsistencias>)fechasdia23.Value);

                                if (LSCAdia23.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia23 = d;
                                    NuevaControlAsistencia.Dia23TA = 0;
                                    //var l23 = Convert.ToDateTime(23 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD23 = l23.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia23[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD23 = li.Formula;
                                    NuevaControlAsistencia.Dia23 = LSCAdia23[0].Fecha;
                                    NuevaControlAsistencia.Dia23TA = LSCAdia23[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD23 = LSCAdia23[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                    
                                }
                                break;
                            case 24:
                                var fechasdia24 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia24 = ((List<ControlAsistencias>)fechasdia24.Value);

                                if (LSCAdia24.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia24 = d;
                                    NuevaControlAsistencia.Dia24TA = 0;
                                    //var l24 = Convert.ToDateTime(24 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD24 = l24.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia24[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD24 = li.Formula;
                                    NuevaControlAsistencia.Dia24 = LSCAdia24[0].Fecha;
                                    NuevaControlAsistencia.Dia24TA = LSCAdia24[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD24 = LSCAdia24[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                    
                                }
                                break;
                            case 25:
                                var fechasdia25 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia25 = ((List<ControlAsistencias>)fechasdia25.Value);

                                if (LSCAdia25.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia25 = d;
                                    NuevaControlAsistencia.Dia25TA = 0;
                                    //var l25 = Convert.ToDateTime(25 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD25 = l25.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia25[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD25 = li.Formula;
                                    NuevaControlAsistencia.Dia25 = LSCAdia25[0].Fecha;
                                    NuevaControlAsistencia.Dia25TA = LSCAdia25[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD25 = LSCAdia25[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                     
                                }
                                break;
                            case 26:
                                var fechasdia26 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia26 = ((List<ControlAsistencias>)fechasdia26.Value);

                                if (LSCAdia26.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia26 = d;
                                    NuevaControlAsistencia.Dia26TA = 0;
                                    //var l26 = Convert.ToDateTime(26 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD26 = l26.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia26[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD26 = li.Formula;
                                    NuevaControlAsistencia.Dia26 = LSCAdia26[0].Fecha;
                                    NuevaControlAsistencia.Dia26TA = LSCAdia26[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD26 = LSCAdia26[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper(); 
                                }
                                break;
                            case 27:
                                var fechasdia27 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia27 = ((List<ControlAsistencias>)fechasdia27.Value);

                                if (LSCAdia27.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia27 = d;
                                    NuevaControlAsistencia.Dia27TA = 0;
                                    //var l27 = Convert.ToDateTime(27 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD27 = l27.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia27[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD27 = li.Formula;
                                    NuevaControlAsistencia.Dia27 = LSCAdia27[0].Fecha;
                                    NuevaControlAsistencia.Dia27TA = LSCAdia27[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD27 = LSCAdia27[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                    
                                }
                                break;
                            case 28:
                                var fechasdia28 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia28 = ((List<ControlAsistencias>)fechasdia28.Value);

                                if (LSCAdia28.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia28 = d;
                                    NuevaControlAsistencia.Dia28TA = 0;
                                    //var l28 = Convert.ToDateTime(28 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD28 = l28.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia28[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD28 = li.Formula;
                                    NuevaControlAsistencia.Dia28 = LSCAdia28[0].Fecha;
                                    NuevaControlAsistencia.Dia28TA = LSCAdia28[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD28 = LSCAdia28[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                 
                                }
                                break;
                            case 29:
                                var fechasdia29 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia29 = ((List<ControlAsistencias>)fechasdia29.Value);

                                if (LSCAdia29.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia29 = d;
                                    NuevaControlAsistencia.Dia29TA = 0;
                                    //var l29 = Convert.ToDateTime(29 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD29 = l29.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia29[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD29 = li.Formula;
                                    NuevaControlAsistencia.Dia29 = LSCAdia29[0].Fecha;
                                    NuevaControlAsistencia.Dia29TA = LSCAdia29[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD29 = LSCAdia29[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                   
                                }
                                break;
                            case 30:
                                var fechasdia30 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia30 = ((List<ControlAsistencias>)fechasdia30.Value);

                                if (LSCAdia30.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia30 = d;
                                    NuevaControlAsistencia.Dia30TA = 0;
                                    //var l30 = Convert.ToDateTime(30 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD30 = l30.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia30[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD30 = li.Formula;
                                    NuevaControlAsistencia.Dia30 = LSCAdia30[0].Fecha;
                                    NuevaControlAsistencia.Dia30TA = LSCAdia30[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD30 = LSCAdia30[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                   
                                }
                                break;
                            case 31:
                                var fechasdia31 = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, d, d);
                                List<ControlAsistencias> LSCAdia31 = ((List<ControlAsistencias>)fechasdia31.Value);

                                if (LSCAdia31.Count <= 0)
                                {
                                    NuevaControlAsistencia.Dia31 = d;
                                    NuevaControlAsistencia.Dia31TA = 0;
                                    //var l31 = Convert.ToDateTime(31 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                                    //NuevaControlAsistencia.LetraD31 = l31.ToString("dddd").Substring(0, 1).ToUpper();
                                }
                                else
                                {
                                    var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(LSCAdia31[0].TipoAsistencia));
                                    var li = ((ElementoConfiguracion)getTipoAsistencia.Value);
                                    NuevaControlAsistencia.ColorD31 = li.Formula;
                                    NuevaControlAsistencia.Dia31 = LSCAdia31[0].Fecha;
                                    NuevaControlAsistencia.Dia31TA = LSCAdia31[0].TipoAsistencia;
                                    //NuevaControlAsistencia.LetraD31 = LSCAdia31[0].Fecha.ToString("dddd").Substring(0, 1).ToUpper();                                  
                                }
                                break;
                            default:

                                break;

                        }
                    }
                    _ControlAsistencias.Add(NuevaControlAsistencia);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return _ControlAsistencias.ToDataSourceResult(request);
        }

        [HttpPost]
        public async Task<JsonResult> GetControlAsistenciasByEmpl(ControlAsistencias NuevaControlAsistencia, DateTime primero, DateTime actual)
        {
            var primerdiames = Convert.ToDateTime(primero);
            var diaactual = Convert.ToDateTime(actual);

            //DateTime Fecha = new DateTime(01-11-2019);
            List<ControlAsistencias> ControlAsistencia = new List<ControlAsistencias>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                NuevaControlAsistencia.FechaCreacion = primerdiames;//new DateTime(2019, 11, 01);
                NuevaControlAsistencia.FechaModificacion = diaactual;//DateTime.Now;
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

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(ControlAsistencia);
        }

        [HttpPost]
        public async Task<ActionResult<ControlAsistencias>> Insert(ControlAsistencias _ControlAsistencia)
        {
            try
            {
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
            ControlAsistencias _ControlAsis1 = new ControlAsistencias();
            ControlAsistencias _ControlAsis2 = new ControlAsistencias();
            ControlAsistencias _ControlAsis3 = new ControlAsistencias();
            ControlAsistencias _ControlAsis4 = new ControlAsistencias();
            ControlAsistencias _ControlAsis5 = new ControlAsistencias();
            ControlAsistencias _ControlAsis6 = new ControlAsistencias();
            ControlAsistencias _ControlAsis7 = new ControlAsistencias();
            ControlAsistencias _ControlAsis8 = new ControlAsistencias();
            ControlAsistencias _ControlAsis9 = new ControlAsistencias();
            ControlAsistencias _ControlAsis10 = new ControlAsistencias();
            ControlAsistencias _ControlAsis11 = new ControlAsistencias();
            ControlAsistencias _ControlAsis12 = new ControlAsistencias();
            ControlAsistencias _ControlAsis13 = new ControlAsistencias();
            ControlAsistencias _ControlAsis14 = new ControlAsistencias();
            ControlAsistencias _ControlAsis15 = new ControlAsistencias();
            ControlAsistencias _ControlAsis16 = new ControlAsistencias();
            ControlAsistencias _ControlAsis17 = new ControlAsistencias();
            ControlAsistencias _ControlAsis18 = new ControlAsistencias();
            ControlAsistencias _ControlAsis19 = new ControlAsistencias();
            ControlAsistencias _ControlAsis20 = new ControlAsistencias();
            ControlAsistencias _ControlAsis21 = new ControlAsistencias();
            ControlAsistencias _ControlAsis22 = new ControlAsistencias();
            ControlAsistencias _ControlAsis23 = new ControlAsistencias();
            ControlAsistencias _ControlAsis24 = new ControlAsistencias();
            ControlAsistencias _ControlAsis25 = new ControlAsistencias();
            ControlAsistencias _ControlAsis26 = new ControlAsistencias();
            ControlAsistencias _ControlAsis27 = new ControlAsistencias();
            ControlAsistencias _ControlAsis28 = new ControlAsistencias();
            ControlAsistencias _ControlAsis29 = new ControlAsistencias();
            ControlAsistencias _ControlAsis30 = new ControlAsistencias();
            ControlAsistencias _ControlAsis31 = new ControlAsistencias();
            DateTime fechafindmes = DateTime.Now;

            if (_CtlAsis.Dia1.Month == DateTime.Now.Month)
            {
                fechafindmes = DateTime.Now;
            }
            else
            {
                int me = _CtlAsis.Dia1.Month;
                int ao = _CtlAsis.Dia1.Year;
                int diass = DateTime.DaysInMonth(_CtlAsis.Dia1.Year, _CtlAsis.Dia1.Month);
                string fechavieja =  me + "-" + diass + "-"  + ao;
                fechafindmes = Convert.ToDateTime(fechavieja);
            }

            for (var d = _CtlAsis.Dia1; d < fechafindmes; d = d.AddDays(1))

            {
                switch (d.Day)
                {
                    case 1:
                        _ControlAsis1.Fecha = _CtlAsis.Dia1;
                        _ControlAsis1.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras1 = _CtlAsis.Dia1.ToString("dddd");
                        int dianum1 = 0;

                        if (dialetras1 == "lunes")
                        {
                            dianum1 = 1;
                        }
                        else if (dialetras1 == "martes")
                        {
                            dianum1 = 2;
                        }
                        else if (dialetras1 == "miércoles")
                        {
                            dianum1 = 3;
                        }
                        else if (dialetras1 == "jueves")
                        {
                            dianum1 = 4;
                        }
                        else if (dialetras1 == "viernes")
                        {
                            dianum1 = 5;
                        }
                        else if (dialetras1 == "sábado")
                        {
                            dianum1 = 6;
                        }
                        else if (dialetras1 == "domingo")
                        {
                            dianum1 = 7;
                        }
                        _ControlAsis1.Dia = dianum1;
                        _ControlAsis1.TipoAsistencia = _CtlAsis.Dia1TA;
                        var insert1 = await SaveControlAsistencia(_ControlAsis1);
                        break;
                    case 2:
                        _ControlAsis2.Fecha = _CtlAsis.Dia2;
                        _ControlAsis2.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras2 = _CtlAsis.Dia2.ToString("dddd");
                        int dianum2 = 0;
                        if (dialetras2 == "lunes")
                        {
                            dianum2 = 1;
                        }
                        else if (dialetras2 == "martes")
                        {
                            dianum2 = 2;
                        }
                        else if (dialetras2 == "miércoles")
                        {
                            dianum2 = 3;
                        }
                        else if (dialetras2 == "jueves")
                        {
                            dianum2 = 4;
                        }
                        else if (dialetras2 == "viernes")
                        {
                            dianum2 = 5;
                        }
                        else if (dialetras2 == "sábado")
                        {
                            dianum2 = 6;
                        }
                        else if (dialetras2 == "domingo")
                        {
                            dianum2 = 7;
                        }
                        _ControlAsis2.Dia = dianum2;
                        _ControlAsis2.TipoAsistencia = _CtlAsis.Dia2TA;
                        var insert2 = await SaveControlAsistencia(_ControlAsis2);
                        break;
                    case 3:
                        _ControlAsis3.Fecha = _CtlAsis.Dia3;
                        _ControlAsis3.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras3 = _CtlAsis.Dia3.ToString("dddd");
                        int dianum3 = 0;
                        if (dialetras3 == "lunes")
                        {
                            dianum3 = 1;
                        }
                        else if (dialetras3 == "martes")
                        {
                            dianum3 = 2;
                        }
                        else if (dialetras3 == "miércoles")
                        {
                            dianum3 = 3;
                        }
                        else if (dialetras3 == "jueves")
                        {
                            dianum3 = 4;
                        }
                        else if (dialetras3 == "viernes")
                        {
                            dianum3 = 5;
                        }
                        else if (dialetras3 == "sábado")
                        {
                            dianum3 = 6;
                        }
                        else if (dialetras3 == "domingo")
                        {
                            dianum3 = 7;
                        }
                        _ControlAsis3.Dia = dianum3;
                        _ControlAsis3.TipoAsistencia = _CtlAsis.Dia3TA;
                        var insert3 = await SaveControlAsistencia(_ControlAsis3);
                        break;
                    case 4:
                        _ControlAsis4.Fecha = _CtlAsis.Dia4;
                        _ControlAsis4.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras4 = _CtlAsis.Dia4.ToString("dddd");
                        int dianum4 = 0;
                        if (dialetras4 == "lunes")
                        {
                            dianum4 = 1;
                        }
                        else if (dialetras4 == "martes")
                        {
                            dianum4 = 2;
                        }
                        else if (dialetras4 == "miércoles")
                        {
                            dianum4 = 3;
                        }
                        else if (dialetras4 == "jueves")
                        {
                            dianum4 = 4;
                        }
                        else if (dialetras4 == "viernes")
                        {
                            dianum4 = 5;
                        }
                        else if (dialetras4 == "sábado")
                        {
                            dianum4 = 6;
                        }
                        else if (dialetras4 == "domingo")
                        {
                            dianum4 = 7;
                        }
                        _ControlAsis4.Dia = dianum4;
                        _ControlAsis4.TipoAsistencia = _CtlAsis.Dia4TA;
                        var insert4 = await SaveControlAsistencia(_ControlAsis4);
                        break;
                    case 5:
                        _ControlAsis5.Fecha = _CtlAsis.Dia5;
                        _ControlAsis5.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras5 = _CtlAsis.Dia5.ToString("dddd");
                        int dianum5 = 0;
                        if (dialetras5 == "lunes")
                        {
                            dianum4 = 1;
                        }
                        else if (dialetras5 == "martes")
                        {
                            dianum5 = 2;
                        }
                        else if (dialetras5 == "miércoles")
                        {
                            dianum5 = 3;
                        }
                        else if (dialetras5 == "jueves")
                        {
                            dianum5 = 4;
                        }
                        else if (dialetras5 == "viernes")
                        {
                            dianum5 = 5;
                        }
                        else if (dialetras5 == "sábado")
                        {
                            dianum5 = 6;
                        }
                        else if (dialetras5 == "domingo")
                        {
                            dianum5 = 7;
                        }
                        _ControlAsis5.Dia = dianum5;
                        _ControlAsis5.TipoAsistencia = _CtlAsis.Dia5TA;
                        var insert5 = await SaveControlAsistencia(_ControlAsis5);
                        break;
                    case 6:
                        _ControlAsis6.Fecha = _CtlAsis.Dia6;
                        _ControlAsis6.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras6 = _CtlAsis.Dia6.ToString("dddd");
                        int dianum6 = 0;
                        if (dialetras6 == "lunes")
                        {
                            dianum6 = 1;
                        }
                        else if (dialetras6 == "martes")
                        {
                            dianum6 = 2;
                        }
                        else if (dialetras6 == "miércoles")
                        {
                            dianum6 = 3;
                        }
                        else if (dialetras6 == "jueves")
                        {
                            dianum6 = 4;
                        }
                        else if (dialetras6 == "viernes")
                        {
                            dianum6 = 5;
                        }
                        else if (dialetras6 == "sábado")
                        {
                            dianum6 = 6;
                        }
                        else if (dialetras6 == "domingo")
                        {
                            dianum6 = 7;
                        }
                        _ControlAsis6.Dia = dianum6;
                        _ControlAsis6.TipoAsistencia = _CtlAsis.Dia6TA;
                        var insert6 = await SaveControlAsistencia(_ControlAsis6);
                        break;
                    case 7:
                        _ControlAsis7.Fecha = _CtlAsis.Dia7;
                        _ControlAsis7.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras7 = _CtlAsis.Dia7.ToString("dddd");
                        int dianum7 = 0;
                        if (dialetras7 == "lunes")
                        {
                            dianum7 = 1;
                        }
                        else if (dialetras7 == "martes")
                        {
                            dianum7 = 2;
                        }
                        else if (dialetras7 == "miércoles")
                        {
                            dianum7 = 3;
                        }
                        else if (dialetras7 == "jueves")
                        {
                            dianum7 = 4;
                        }
                        else if (dialetras7 == "viernes")
                        {
                            dianum7 = 5;
                        }
                        else if (dialetras7 == "sábado")
                        {
                            dianum7 = 6;
                        }
                        else if (dialetras7 == "domingo")
                        {
                            dianum7 = 7;
                        }
                        _ControlAsis7.Dia = dianum7;
                        _ControlAsis7.IdEmpleado = _CtlAsis.EmployeesId;
                        var insert7 = await SaveControlAsistencia(_ControlAsis7);
                        break;
                    case 8:
                        _ControlAsis8.Fecha = _CtlAsis.Dia8;
                        _ControlAsis8.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras8 = _CtlAsis.Dia8.ToString("dddd");
                        int dianum8 = 0;
                        if (dialetras8 == "lunes")
                        {
                            dianum7 = 1;
                        }
                        else if (dialetras8 == "martes")
                        {
                            dianum8 = 2;
                        }
                        else if (dialetras8 == "miércoles")
                        {
                            dianum8 = 3;
                        }
                        else if (dialetras8 == "jueves")
                        {
                            dianum8 = 4;
                        }
                        else if (dialetras8 == "viernes")
                        {
                            dianum8 = 5;
                        }
                        else if (dialetras8 == "sábado")
                        {
                            dianum8 = 6;
                        }
                        else if (dialetras8 == "domingo")
                        {
                            dianum8 = 7;
                        }
                        _ControlAsis8.Dia = dianum8;
                        _ControlAsis8.TipoAsistencia = _CtlAsis.Dia8TA;
                        var insert8 = await SaveControlAsistencia(_ControlAsis8);
                        break;
                    case 9:
                        _ControlAsis9.Fecha = _CtlAsis.Dia9;
                        _ControlAsis9.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras9 = _CtlAsis.Dia9.ToString("dddd");
                        int dianum9 = 0;
                        if (dialetras9 == "lunes")
                        {
                            dianum7 = 1;
                        }
                        else if (dialetras9 == "martes")
                        {
                            dianum9 = 2;
                        }
                        else if (dialetras9 == "miércoles")
                        {
                            dianum9 = 3;
                        }
                        else if (dialetras9 == "jueves")
                        {
                            dianum9 = 4;
                        }
                        else if (dialetras9 == "viernes")
                        {
                            dianum9 = 5;
                        }
                        else if (dialetras9 == "sábado")
                        {
                            dianum9 = 6;
                        }
                        else if (dialetras9 == "domingo")
                        {
                            dianum9 = 7;
                        }
                        _ControlAsis9.Dia = dianum9;
                        _ControlAsis9.TipoAsistencia = _CtlAsis.Dia9TA;
                        var insert9 = await SaveControlAsistencia(_ControlAsis9);
                        break;
                    case 10:
                        _ControlAsis10.Fecha = _CtlAsis.Dia10;
                        _ControlAsis10.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras10 = _CtlAsis.Dia10.ToString("dddd");
                        int dianum10 = 0;
                        if (dialetras10 == "lunes")
                        {
                            dianum10 = 1;
                        }
                        else if (dialetras10 == "martes")
                        {
                            dianum10 = 2;
                        }
                        else if (dialetras10 == "miércoles")
                        {
                            dianum10 = 3;
                        }
                        else if (dialetras10 == "jueves")
                        {
                            dianum10 = 4;
                        }
                        else if (dialetras10 == "viernes")
                        {
                            dianum10 = 5;
                        }
                        else if (dialetras10 == "sábado")
                        {
                            dianum10 = 6;
                        }
                        else if (dialetras10 == "domingo")
                        {
                            dianum10 = 7;
                        }
                        _ControlAsis11.Dia = dianum10;
                        _ControlAsis10.TipoAsistencia = _CtlAsis.Dia10TA;
                        var insert10 = await SaveControlAsistencia(_ControlAsis10);
                        break;
                    case 11:
                        _ControlAsis11.Fecha = _CtlAsis.Dia11;
                        _ControlAsis11.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras11 = _CtlAsis.Dia11.ToString("dddd");
                        int dianum11 = 0;
                        if (dialetras11 == "lunes")
                        {
                            dianum11 = 1;
                        }
                        else if (dialetras11 == "martes")
                        {
                            dianum11 = 2;
                        }
                        else if (dialetras11 == "miércoles")
                        {
                            dianum11 = 3;
                        }
                        else if (dialetras11 == "jueves")
                        {
                            dianum11 = 4;
                        }
                        else if (dialetras11 == "viernes")
                        {
                            dianum11 = 5;
                        }
                        else if (dialetras11 == "sábado")
                        {
                            dianum11 = 6;
                        }
                        else if (dialetras11 == "domingo")
                        {
                            dianum11 = 7;
                        }
                        _ControlAsis11.Dia = dianum11;
                        _ControlAsis11.TipoAsistencia = _CtlAsis.Dia11TA;
                        var insert11 = await SaveControlAsistencia(_ControlAsis11);
                        break;
                    case 12:
                        _ControlAsis12.Fecha = _CtlAsis.Dia12;
                        _ControlAsis12.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras12 = _CtlAsis.Dia12.ToString("dddd");
                        int dianum12 = 0;
                        if (dialetras12 == "lunes")
                        {
                            dianum12 = 1;
                        }
                        else if (dialetras12 == "martes")
                        {
                            dianum12 = 2;
                        }
                        else if (dialetras12 == "miércoles")
                        {
                            dianum12 = 3;
                        }
                        else if (dialetras12 == "jueves")
                        {
                            dianum12 = 4;
                        }
                        else if (dialetras12 == "viernes")
                        {
                            dianum12 = 5;
                        }
                        else if (dialetras12 == "sábado")
                        {
                            dianum12 = 6;
                        }
                        else if (dialetras12 == "domingo")
                        {
                            dianum12 = 7;
                        }
                        _ControlAsis12.Dia = dianum12;
                        _ControlAsis12.TipoAsistencia = _CtlAsis.Dia12TA;
                        var insert12 = await SaveControlAsistencia(_ControlAsis12);
                        break;
                    case 13:
                        _ControlAsis13.Fecha = _CtlAsis.Dia13;
                        _ControlAsis13.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras13 = _CtlAsis.Dia13.ToString("dddd");
                        int dianum13 = 0;
                        if (dialetras13 == "lunes")
                        {
                            dianum13 = 1;
                        }
                        else if (dialetras13 == "martes")
                        {
                            dianum13 = 2;
                        }
                        else if (dialetras13 == "miércoles")
                        {
                            dianum13 = 3;
                        }
                        else if (dialetras13 == "jueves")
                        {
                            dianum13 = 4;
                        }
                        else if (dialetras13 == "viernes")
                        {
                            dianum13 = 5;
                        }
                        else if (dialetras13 == "sábado")
                        {
                            dianum13 = 6;
                        }
                        else if (dialetras13 == "domingo")
                        {
                            dianum13 = 7;
                        }
                        _ControlAsis13.Dia = dianum13;
                        _ControlAsis13.TipoAsistencia = _CtlAsis.Dia13TA;
                        var insert13 = await SaveControlAsistencia(_ControlAsis13);
                        break;
                    case 14:
                        _ControlAsis14.Fecha = _CtlAsis.Dia14;
                        _ControlAsis14.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras14 = _CtlAsis.Dia14.ToString("dddd");
                        int dianum14 = 0;
                        if (dialetras14 == "lunes")
                        {
                            dianum14 = 1;
                        }
                        else if (dialetras14 == "martes")
                        {
                            dianum14 = 2;
                        }
                        else if (dialetras14 == "miércoles")
                        {
                            dianum14 = 3;
                        }
                        else if (dialetras14 == "jueves")
                        {
                            dianum14 = 4;
                        }
                        else if (dialetras14 == "viernes")
                        {
                            dianum14 = 5;
                        }
                        else if (dialetras14 == "sábado")
                        {
                            dianum14 = 6;
                        }
                        else if (dialetras14 == "domingo")
                        {
                            dianum14 = 7;
                        }
                        _ControlAsis14.Dia = dianum14;
                        _ControlAsis14.TipoAsistencia = _CtlAsis.Dia14TA;
                        var insert14 = await SaveControlAsistencia(_ControlAsis14);
                        break;
                    case 15:
                        _ControlAsis15.Fecha = _CtlAsis.Dia15;
                        _ControlAsis15.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras15 = _CtlAsis.Dia15.ToString("dddd");
                        int dianum15 = 0;
                        if (dialetras15 == "lunes")
                        {
                            dianum15 = 1;
                        }
                        else if (dialetras15 == "martes")
                        {
                            dianum15 = 2;
                        }
                        else if (dialetras15 == "miércoles")
                        {
                            dianum15 = 3;
                        }
                        else if (dialetras15 == "jueves")
                        {
                            dianum15 = 4;
                        }
                        else if (dialetras15 == "viernes")
                        {
                            dianum15 = 5;
                        }
                        else if (dialetras15 == "sábado")
                        {
                            dianum15 = 6;
                        }
                        else if (dialetras15 == "domingo")
                        {
                            dianum15 = 7;
                        }
                        _ControlAsis15.Dia = dianum15;
                        _ControlAsis15.TipoAsistencia = _CtlAsis.Dia15TA;
                        var insert15 = await SaveControlAsistencia(_ControlAsis15);
                        break;
                    case 16:
                        _ControlAsis16.Fecha = _CtlAsis.Dia16;
                        _ControlAsis16.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras16 = _CtlAsis.Dia16.ToString("dddd");
                        int dianum16 = 0;
                        if (dialetras16 == "lunes")
                        {
                            dianum16 = 1;
                        }
                        else if (dialetras16 == "martes")
                        {
                            dianum16 = 2;
                        }
                        else if (dialetras16 == "miércoles")
                        {
                            dianum16 = 3;
                        }
                        else if (dialetras16 == "jueves")
                        {
                            dianum16 = 4;
                        }
                        else if (dialetras16 == "viernes")
                        {
                            dianum16 = 5;
                        }
                        else if (dialetras16 == "sábado")
                        {
                            dianum16 = 6;
                        }
                        else if (dialetras16 == "domingo")
                        {
                            dianum16 = 7;
                        }
                        _ControlAsis16.Dia = dianum16;
                        _ControlAsis16.TipoAsistencia = _CtlAsis.Dia16TA;
                        var insert16 = await SaveControlAsistencia(_ControlAsis16);
                        break;
                    case 17:
                        _ControlAsis17.Fecha = _CtlAsis.Dia17;
                        _ControlAsis17.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras17 = _CtlAsis.Dia17.ToString("dddd");
                        int dianum17 = 0;
                        if (dialetras17 == "lunes")
                        {
                            dianum17 = 1;
                        }
                        else if (dialetras17 == "martes")
                        {
                            dianum17 = 2;
                        }
                        else if (dialetras17 == "miércoles")
                        {
                            dianum17 = 3;
                        }
                        else if (dialetras17 == "jueves")
                        {
                            dianum17 = 4;
                        }
                        else if (dialetras17 == "viernes")
                        {
                            dianum17 = 5;
                        }
                        else if (dialetras17 == "sábado")
                        {
                            dianum17 = 6;
                        }
                        else if (dialetras17 == "domingo")
                        {
                            dianum17 = 7;
                        }
                        _ControlAsis17.Dia = dianum17;
                        _ControlAsis17.TipoAsistencia = _CtlAsis.Dia17TA;
                        var insert17 = await SaveControlAsistencia(_ControlAsis17);
                        break;
                    case 18:
                        _ControlAsis18.Fecha = _CtlAsis.Dia18;
                        _ControlAsis18.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras18 = _CtlAsis.Dia18.ToString("dddd");
                        int dianum18 = 0;
                        if (dialetras18 == "lunes")
                        {
                            dianum18 = 1;
                        }
                        else if (dialetras18 == "martes")
                        {
                            dianum18 = 2;
                        }
                        else if (dialetras18 == "miércoles")
                        {
                            dianum18 = 3;
                        }
                        else if (dialetras18 == "jueves")
                        {
                            dianum18 = 4;
                        }
                        else if (dialetras18 == "viernes")
                        {
                            dianum18 = 5;
                        }
                        else if (dialetras18 == "sábado")
                        {
                            dianum18 = 6;
                        }
                        else if (dialetras18 == "domingo")
                        {
                            dianum18 = 7;
                        }
                        _ControlAsis18.Dia = dianum18;
                        _ControlAsis18.TipoAsistencia = _CtlAsis.Dia18TA;
                        var insert18 = await SaveControlAsistencia(_ControlAsis18);
                        break;
                    case 19:
                        _ControlAsis19.Fecha = _CtlAsis.Dia19;
                        _ControlAsis19.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras19 = _CtlAsis.Dia19.ToString("dddd");
                        int dianum19 = 0;
                        if (dialetras19 == "lunes")
                        {
                            dianum19 = 1;
                        }
                        else if (dialetras19 == "martes")
                        {
                            dianum19 = 2;
                        }
                        else if (dialetras19 == "miércoles")
                        {
                            dianum19 = 3;
                        }
                        else if (dialetras19 == "jueves")
                        {
                            dianum19 = 4;
                        }
                        else if (dialetras19 == "viernes")
                        {
                            dianum19 = 5;
                        }
                        else if (dialetras19 == "sábado")
                        {
                            dianum19 = 6;
                        }
                        else if (dialetras19 == "domingo")
                        {
                            dianum19 = 7;
                        }
                        _ControlAsis19.Dia = dianum19;
                        _ControlAsis19.TipoAsistencia = _CtlAsis.Dia19TA;
                        var insert19 = await SaveControlAsistencia(_ControlAsis19);
                        break;
                    case 20:
                        _ControlAsis20.Fecha = _CtlAsis.Dia20;
                        _ControlAsis20.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras20 = _CtlAsis.Dia20.ToString("dddd");
                        int dianum20 = 0;
                        if (dialetras20 == "lunes")
                        {
                            dianum20 = 1;
                        }
                        else if (dialetras20 == "martes")
                        {
                            dianum20 = 2;
                        }
                        else if (dialetras20 == "miércoles")
                        {
                            dianum20 = 3;
                        }
                        else if (dialetras20 == "jueves")
                        {
                            dianum20 = 4;
                        }
                        else if (dialetras20 == "viernes")
                        {
                            dianum20 = 5;
                        }
                        else if (dialetras20 == "sábado")
                        {
                            dianum20 = 6;
                        }
                        else if (dialetras20 == "domingo")
                        {
                            dianum20 = 7;
                        }
                        _ControlAsis20.Dia = dianum20;
                        _ControlAsis20.TipoAsistencia = _CtlAsis.Dia20TA;
                        var insert20 = await SaveControlAsistencia(_ControlAsis20);
                        break;
                    case 21:
                        _ControlAsis21.Fecha = _CtlAsis.Dia21;
                        _ControlAsis21.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras21 = _CtlAsis.Dia21.ToString("dddd");
                        int dianum21 = 0;
                        if (dialetras21 == "lunes")
                        {
                            dianum21 = 1;
                        }
                        else if (dialetras21 == "martes")
                        {
                            dianum21 = 2;
                        }
                        else if (dialetras21 == "miércoles")
                        {
                            dianum21 = 3;
                        }
                        else if (dialetras21 == "jueves")
                        {
                            dianum21 = 4;
                        }
                        else if (dialetras21 == "viernes")
                        {
                            dianum21 = 5;
                        }
                        else if (dialetras21 == "sábado")
                        {
                            dianum21 = 6;
                        }
                        else if (dialetras21 == "domingo")
                        {
                            dianum21 = 7;
                        }
                        _ControlAsis21.Dia = dianum21;
                        _ControlAsis21.TipoAsistencia = _CtlAsis.Dia21TA;
                        var insert21 = await SaveControlAsistencia(_ControlAsis21);
                        break;
                    case 22:
                        _ControlAsis22.Fecha = _CtlAsis.Dia22;
                        _ControlAsis22.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras22 = _CtlAsis.Dia22.ToString("dddd");
                        int dianum22 = 0;
                        if (dialetras22 == "lunes")
                        {
                            dianum22 = 1;
                        }
                        else if (dialetras22 == "martes")
                        {
                            dianum22 = 2;
                        }
                        else if (dialetras22 == "miércoles")
                        {
                            dianum22 = 3;
                        }
                        else if (dialetras22 == "jueves")
                        {
                            dianum22 = 4;
                        }
                        else if (dialetras22 == "viernes")
                        {
                            dianum22 = 5;
                        }
                        else if (dialetras22 == "sábado")
                        {
                            dianum22 = 6;
                        }
                        else if (dialetras22 == "domingo")
                        {
                            dianum22 = 7;
                        }
                        _ControlAsis22.Dia = dianum22;
                        _ControlAsis22.TipoAsistencia = _CtlAsis.Dia22TA;
                        var insert22 = await SaveControlAsistencia(_ControlAsis22);
                        break;
                    case 23:
                        _ControlAsis23.Fecha = _CtlAsis.Dia23;
                        _ControlAsis23.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras23 = _CtlAsis.Dia23.ToString("dddd");
                        int dianum23 = 0;
                        if (dialetras23 == "lunes")
                        {
                            dianum23 = 1;
                        }
                        else if (dialetras23 == "martes")
                        {
                            dianum23 = 2;
                        }
                        else if (dialetras23 == "miércoles")
                        {
                            dianum23 = 3;
                        }
                        else if (dialetras23 == "jueves")
                        {
                            dianum23 = 4;
                        }
                        else if (dialetras23 == "viernes")
                        {
                            dianum23 = 5;
                        }
                        else if (dialetras23 == "sábado")
                        {
                            dianum23 = 6;
                        }
                        else if (dialetras23 == "domingo")
                        {
                            dianum23 = 7;
                        }
                        _ControlAsis23.Dia = dianum23;
                        _ControlAsis23.TipoAsistencia = _CtlAsis.Dia23TA;
                        var insert23 = await SaveControlAsistencia(_ControlAsis23);
                        break;
                    case 24:
                        _ControlAsis24.Fecha = _CtlAsis.Dia24;
                        _ControlAsis24.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras24 = _CtlAsis.Dia24.ToString("dddd");
                        int dianum24 = 0;
                        if (dialetras24 == "lunes")
                        {
                            dianum24 = 1;
                        }
                        else if (dialetras24 == "martes")
                        {
                            dianum24 = 2;
                        }
                        else if (dialetras24 == "miércoles")
                        {
                            dianum24 = 3;
                        }
                        else if (dialetras24 == "jueves")
                        {
                            dianum24 = 4;
                        }
                        else if (dialetras24 == "viernes")
                        {
                            dianum24 = 5;
                        }
                        else if (dialetras24 == "sábado")
                        {
                            dianum24 = 6;
                        }
                        else if (dialetras24 == "domingo")
                        {
                            dianum24 = 7;
                        }
                        _ControlAsis24.Dia = dianum24;
                        _ControlAsis24.TipoAsistencia = _CtlAsis.Dia24TA;
                        var insert24 = await SaveControlAsistencia(_ControlAsis24);
                        break;
                    case 25:
                        _ControlAsis25.Fecha = _CtlAsis.Dia25;
                        _ControlAsis25.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras25 = _CtlAsis.Dia25.ToString("dddd");
                        int dianum25 = 0;
                        if (dialetras25 == "lunes")
                        {
                            dianum25 = 1;
                        }
                        else if (dialetras25 == "martes")
                        {
                            dianum25 = 2;
                        }
                        else if (dialetras25 == "miércoles")
                        {
                            dianum25 = 3;
                        }
                        else if (dialetras25 == "jueves")
                        {
                            dianum25 = 4;
                        }
                        else if (dialetras25 == "viernes")
                        {
                            dianum25 = 5;
                        }
                        else if (dialetras25 == "sábado")
                        {
                            dianum25 = 6;
                        }
                        else if (dialetras25 == "domingo")
                        {
                            dianum25 = 7;
                        }
                        _ControlAsis25.Dia = dianum25;
                        _ControlAsis25.TipoAsistencia = _CtlAsis.Dia25TA;
                        var insert25 = await SaveControlAsistencia(_ControlAsis25);
                        break;
                    case 26:
                        _ControlAsis26.Fecha = _CtlAsis.Dia26;
                        _ControlAsis26.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras26 = _CtlAsis.Dia26.ToString("dddd");
                        int dianum26 = 0;
                        if (dialetras26 == "lunes")
                        {
                            dianum26 = 1;
                        }
                        else if (dialetras26 == "martes")
                        {
                            dianum26 = 2;
                        }
                        else if (dialetras26 == "miércoles")
                        {
                            dianum26 = 3;
                        }
                        else if (dialetras26 == "jueves")
                        {
                            dianum26 = 4;
                        }
                        else if (dialetras26 == "viernes")
                        {
                            dianum26 = 5;
                        }
                        else if (dialetras26 == "sábado")
                        {
                            dianum26 = 6;
                        }
                        else if (dialetras26 == "domingo")
                        {
                            dianum26 = 7;
                        }
                        _ControlAsis26.Dia = dianum26;
                        _ControlAsis26.TipoAsistencia = _CtlAsis.Dia26TA;
                        var insert26 = await SaveControlAsistencia(_ControlAsis26);
                        break;
                    case 27:
                        _ControlAsis27.Fecha = _CtlAsis.Dia27;
                        _ControlAsis27.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras27 = _CtlAsis.Dia27.ToString("dddd");
                        int dianum27 = 0;
                        if (dialetras27 == "lunes")
                        {
                            dianum27 = 1;
                        }
                        else if (dialetras27 == "martes")
                        {
                            dianum27 = 2;
                        }
                        else if (dialetras27 == "miércoles")
                        {
                            dianum27 = 3;
                        }
                        else if (dialetras27 == "jueves")
                        {
                            dianum27 = 4;
                        }
                        else if (dialetras27 == "viernes")
                        {
                            dianum27 = 5;
                        }
                        else if (dialetras27 == "sábado")
                        {
                            dianum27 = 6;
                        }
                        else if (dialetras27 == "domingo")
                        {
                            dianum27 = 7;
                        }
                        _ControlAsis27.Dia = dianum27;
                        _ControlAsis27.TipoAsistencia = _CtlAsis.Dia27TA;
                        var insert27 = await SaveControlAsistencia(_ControlAsis27);
                        break;
                    case 28:
                        _ControlAsis28.Fecha = _CtlAsis.Dia28;
                        _ControlAsis28.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras28 = _CtlAsis.Dia28.ToString("dddd");
                        int dianum28 = 0;
                        if (dialetras28 == "lunes")
                        {
                            dianum28 = 1;
                        }
                        else if (dialetras28 == "martes")
                        {
                            dianum28 = 2;
                        }
                        else if (dialetras28 == "miércoles")
                        {
                            dianum28 = 3;
                        }
                        else if (dialetras28 == "jueves")
                        {
                            dianum28 = 4;
                        }
                        else if (dialetras28 == "viernes")
                        {
                            dianum28 = 5;
                        }
                        else if (dialetras28 == "sábado")
                        {
                            dianum28 = 6;
                        }
                        else if (dialetras28 == "domingo")
                        {
                            dianum28 = 7;
                        }
                        _ControlAsis28.Dia = dianum28;
                        _ControlAsis28.TipoAsistencia = _CtlAsis.Dia28TA;
                        var insert28 = await SaveControlAsistencia(_ControlAsis28);
                        break;
                    case 29:
                        _ControlAsis29.Fecha = _CtlAsis.Dia29;
                        _ControlAsis29.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras29 = _CtlAsis.Dia29.ToString("dddd");
                        int dianum29 = 0;
                        if (dialetras29 == "lunes")
                        {
                            dianum29 = 1;
                        }
                        else if (dialetras29 == "martes")
                        {
                            dianum29 = 2;
                        }
                        else if (dialetras29 == "miércoles")
                        {
                            dianum29 = 3;
                        }
                        else if (dialetras29 == "jueves")
                        {
                            dianum29 = 4;
                        }
                        else if (dialetras29 == "viernes")
                        {
                            dianum29 = 5;
                        }
                        else if (dialetras29 == "sábado")
                        {
                            dianum29 = 6;
                        }
                        else if (dialetras29 == "domingo")
                        {
                            dianum29 = 7;
                        }
                        _ControlAsis29.Dia = dianum29;
                        _ControlAsis29.TipoAsistencia = _CtlAsis.Dia29TA;
                        var insert29 = await SaveControlAsistencia(_ControlAsis29);
                        break;
                    case 30:
                        _ControlAsis30.Fecha = _CtlAsis.Dia30;
                        _ControlAsis30.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras30 = _CtlAsis.Dia30.ToString("dddd");
                        int dianum30 = 0;
                        if (dialetras30 == "lunes")
                        {
                            dianum30 = 1;
                        }
                        else if (dialetras30 == "martes")
                        {
                            dianum30 = 2;
                        }
                        else if (dialetras30 == "miércoles")
                        {
                            dianum30 = 3;
                        }
                        else if (dialetras30 == "jueves")
                        {
                            dianum30 = 4;
                        }
                        else if (dialetras30 == "viernes")
                        {
                            dianum30 = 5;
                        }
                        else if (dialetras30 == "sábado")
                        {
                            dianum30 = 6;
                        }
                        else if (dialetras30 == "domingo")
                        {
                            dianum30 = 7;
                        }
                        _ControlAsis30.Dia = dianum30;
                        _ControlAsis30.TipoAsistencia = _CtlAsis.Dia30TA;
                        var insert30 = await SaveControlAsistencia(_ControlAsis30);
                        break;
                    case 31:
                        _ControlAsis31.Fecha = _CtlAsis.Dia31;
                        _ControlAsis31.IdEmpleado = _CtlAsis.EmployeesId;
                        string dialetras31 = _CtlAsis.Dia31.ToString("dddd");
                        int dianum31 = 0;
                        if (dialetras31 == "lunes")
                        {
                            dianum31 = 1;
                        }
                        else if (dialetras31 == "martes")
                        {
                            dianum31 = 2;
                        }
                        else if (dialetras31 == "miércoles")
                        {
                            dianum31 = 3;
                        }
                        else if (dialetras31 == "jueves")
                        {
                            dianum31 = 4;
                        }
                        else if (dialetras31 == "viernes")
                        {
                            dianum31 = 5;
                        }
                        else if (dialetras31 == "sábado")
                        {
                            dianum31 = 6;
                        }
                        else if (dialetras31 == "domingo")
                        {
                            dianum31 = 7;
                        }
                        _ControlAsis31.Dia = dianum31;
                        _ControlAsis31.TipoAsistencia = _CtlAsis.Dia31TA;
                        var insert31 = await SaveControlAsistencia(_ControlAsis31);
                        break;
                    default:

                        break;
                }

            }

            return Json(_CtlAsis);

        }


        public async Task<ActionResult<ControlAsistencias>> SaveControlAsistencia(ControlAsistencias _Nuevo_Update)
        {
           ControlAsistencias ControlAsistenciainsert = _Nuevo_Update;
            try
            {
                ControlAsistencias _listAccount = new ControlAsistencias();
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                if (_Nuevo_Update.Fecha != null)
                {
                    _Nuevo_Update.UsuarioCreacion = HttpContext.Session.GetString("user");
                    _Nuevo_Update.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var resultdate = await _client.PostAsJsonAsync(baseadress + "api/ControlAsistencias/GetControlAsistenciasByFecha", _Nuevo_Update);
                    string valorrespuesta = "";
                    _Nuevo_Update.FechaModificacion = DateTime.Now;
                    _Nuevo_Update.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (resultdate.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (resultdate.Content.ReadAsStringAsync());
                        _Nuevo_Update = JsonConvert.DeserializeObject<ControlAsistencias>(valorrespuesta);

                        if (_Nuevo_Update == null)
                        {
                            _Nuevo_Update = new Models.ControlAsistencias();
                        }

                        if (_Nuevo_Update.Id > 0)
                        {
                            
                            _Nuevo_Update.UsuarioModificacion = HttpContext.Session.GetString("user");
                            _Nuevo_Update.FechaModificacion = DateTime.Now;
                            _Nuevo_Update.TipoAsistencia = ControlAsistenciainsert.TipoAsistencia;
                            var updateresult = await Update(_Nuevo_Update.Id, _Nuevo_Update);
                        }
                        else if(_Nuevo_Update.Id == 0 && ControlAsistenciainsert.TipoAsistencia > 0 )
                        {
                            _Nuevo_Update.Fecha = ControlAsistenciainsert.Fecha;
                            _Nuevo_Update.IdEmpleado = ControlAsistenciainsert.IdEmpleado;
                            _Nuevo_Update.Dia = ControlAsistenciainsert.Dia;
                            _Nuevo_Update.TipoAsistencia = ControlAsistenciainsert.TipoAsistencia;

                            var insertresult = await Insert(_Nuevo_Update);
                            var value = (insertresult.Result as ObjectResult).Value;
                            ControlAsistencias resultado = ((ControlAsistencias)(value));
                        }

                    }
                }


                //if (ControlAsistenciainsert.Id == 0 && ControlAsistenciainsert.TipoAsistencia > 0)
                //{
                   

                //}
                //else
                //{
                    
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_Nuevo_Update);
        }
    }
}