using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
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
        private readonly ClaimsPrincipal _principal;

        public ControlAsistenciasController(ILogger<EstadosController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this._config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;

        }

        public IActionResult DiasMes()
        {
            return View();
        }


        public async Task<IActionResult> ControlAsistencia()
        {
            ViewData["ElementoConfiguracion"] = await ObtenerTiposControlAsistencias();
            ViewData["permisos"] = _principal;
            return View();
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
                    List<ElementoConfiguracion> elementoConfiguracions = new List<ElementoConfiguracion>();

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    elementoConfiguracions = JsonConvert.DeserializeObject<List<ElementoConfiguracion>>(valorrespuesta);
                    elementoConfiguracions.Add(new ElementoConfiguracion { Id = 0, Valorstring = " " });
                    TA = elementoConfiguracions.OrderBy(q => q.Id).ToList();
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
        public async Task<DataSourceResult> GetGetControlAsistencias([DataSourceRequest]DataSourceRequest request, string datepikerselect)
        {

            var diactual = datepikerselect;
            var actuals = Convert.ToDateTime(diactual);
            var primero = Convert.ToDateTime(1 + "-" + actuals.Month + "-" + actuals.Year);

            //DateTime actual = DateTime.Now;

            if (diactual == null)
            {
                actuals = DateTime.Now;                
                primero = Convert.ToDateTime(1 + "-" + actuals.Month + "-" + actuals.Year);
            }
            else if (primero.Month == DateTime.Now.Month)
            {
                actuals = DateTime.Now;
            }
            else
            {
                actuals = Convert.ToDateTime(DateTime.DaysInMonth(primero.Year, primero.Month) + "-" + primero.Month + "-" + primero.Year);
            }



            //int letras = DateTime.DaysInMonth(actuals.Year, actuals.Month);
            var l1 = Convert.ToDateTime(actuals.Month + "-" + 1 + "-" + actuals.Year);

            //int LlegadaTarde = 77;
            //int Domingodiaslibres = 78;
            //int Permisoshoras = 79;
            //int Incapacidad = 80;
            //int Vacacion = 81;
            //int Permiso = 82;
            //int Presente = 83;



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
                    _ListEmpleados = _ListEmpleados.OrderBy(q => q.NombreEmpleado).Where(q => q.IdEstado == 1)
                        .OrderBy(o => o.IdEmpleado).ToList();
                }

                List<ControlAsistencias> asistencias = await GetControlAsistenciasByEmplActivos( primero, actuals);


                foreach (var empleado in _ListEmpleados)
                {
                    ControlAsistenciasDTO NuevaControlAsistencia = new ControlAsistenciasDTO();
                    NuevaControlAsistencia.Empleado = empleado;                
                    NuevaControlAsistencia.EmployeesId = empleado.IdEmpleado;

                    List<ControlAsistencias> asistenciaempleado = asistencias.Where(q => q.IdEmpleado == empleado.IdEmpleado).ToList();

                    ControlAsistencias asistenciarevisado = asistenciaempleado.Where(q => q.Revisado).FirstOrDefault();
                    
                    if (asistenciarevisado != null)
                    {
                        NuevaControlAsistencia.Revisado = true;
                    }

                    ControlAsistencias primerasistencia = asistenciaempleado.FirstOrDefault();
                    if (primerasistencia!= null) { 
                        NuevaControlAsistencia.Fecha = primerasistencia.Fecha;
                    }
                    
                    NuevaControlAsistencia.Dia1 = l1;

                    NuevaControlAsistencia.LlegadasTarde = asistenciaempleado.Where(q => q.TipoAsistencia == 77).Count();

                    int DomigosyLibres = asistenciaempleado.Where(q => q.TipoAsistencia == 78).Count();
                    NuevaControlAsistencia.Domingo = DomigosyLibres;
                   
                    int PermisosH = asistenciaempleado.Where(q => q.TipoAsistencia == 79).Count();
                    NuevaControlAsistencia.Feriado = PermisosH;

                    NuevaControlAsistencia.Incapacidad = asistenciaempleado.Where(q => q.TipoAsistencia == 80).Count();
                   
                    NuevaControlAsistencia.Vacaciones = asistenciaempleado.Where(q => q.TipoAsistencia == 81).Count();

                    NuevaControlAsistencia.Permisos = asistenciaempleado.Where(q => q.TipoAsistencia == 82).Count();

                    int Presentes = asistenciaempleado.Where(q => q.TipoAsistencia == 83).Count();
                    NuevaControlAsistencia.DiasLaborales = Presentes;

                    NuevaControlAsistencia.TeleTrabajo = asistenciaempleado.Where(q => q.TipoAsistencia == 210).Count();

                    NuevaControlAsistencia.Inasistencia = asistenciaempleado.Where(q => q.TipoAsistencia == 215).Count();

                    int DiasLaborales = (DomigosyLibres + PermisosH + NuevaControlAsistencia.Incapacidad + NuevaControlAsistencia.Vacaciones 
                        + NuevaControlAsistencia.Permisos + Presentes + NuevaControlAsistencia.TeleTrabajo);

                    NuevaControlAsistencia.DiasLaborales = DiasLaborales;

                    //double porcentajellegadastarde = ((double)cLLegadasTarde / (double)DiasLaborales) * 100;
                    double porcentajellegadastarde = ((double)NuevaControlAsistencia.LlegadasTarde / (double)NuevaControlAsistencia.DiasLaborales) * 100;

                    if (double.IsNaN(porcentajellegadastarde))
                    {
                        NuevaControlAsistencia.PorcentajeLlegadasTarde = "0.0%";
                    }
                    else
                    {
                        NuevaControlAsistencia.PorcentajeLlegadasTarde =   porcentajellegadastarde.ToString("N1") + " %";
                    }


                    List<ControlAsistencias> LSCA = ((List<ControlAsistencias>)asistenciaempleado);
                   
                    foreach (var _Listardias in LSCA)
                    {

                        var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(_Listardias.TipoAsistencia));
                        var li = ((ElementoConfiguracion)getTipoAsistencia.Value);


                        switch (_Listardias.Fecha.Day)
                        {
                            case 1:
                             
                               if (li != null)
                               {
                                    //NuevaControlAsistencia.Dia1 = l1;
                                    NuevaControlAsistencia.Dia1TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD1 = li.Formula;

                                }
                               else
                               {

                                   //NuevaControlAsistencia.Dia1 = _Listardias.Fecha;
                                   NuevaControlAsistencia.Dia1TA = 0;
                               }
                               
                                break;
                            case 2:


                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia2 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia2TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD2 = li.Formula;

                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia2 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia2TA = 0;
                                }

                               
                                break;
                            case 3:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia3 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia3TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD3 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia3 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia3TA = 0;
                                }
                                
                                break;
                            case 4:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia4 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia4TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD4 = li.Formula;

                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia4 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia4TA = 0;
                                }
                                
                                break;
                            case 5:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia5 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia5TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD5 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia5 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia5TA = 0;
                                }
                                
                                break;
                            case 6:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia6 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia6TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD6 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia6 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia6TA = 0;
                                }
                                
                                break;
                            case 7:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia7 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia7TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD7 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia7 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia7TA = 0;
                                }
                               
                                break;
                            case 8:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia8 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia8TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD8 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia8 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia8TA = 0;
                                }
                                
                                break;
                            case 9:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia9 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia9TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD9 = li.Formula;
                                }
                                else
                                {
                                    
                                    //NuevaControlAsistencia.Dia9 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia9TA = 0;
                                }
                                
                                break;
                            case 10:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia10 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia10TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD10 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia10 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia10TA = 0;
                                }                                
                                break;
                            case 11:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia11 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia11TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD11 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia11 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia11TA = 0;
                                }

                                
                                break;
                            case 12:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia12 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia12TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD12 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia12 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia12TA = 0;
                                }
                                
                                break;
                            case 13:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia13 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia13TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD13 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia13 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia13TA = 0;
                                }
                                
                                break;
                            case 14:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia14 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia14TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD14 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia14 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia14TA = 0;
                                }
                                
                                break;
                            case 15:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia15 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia15TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD15 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia15 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia15TA = 0;
                                }
                                
                                break;
                            case 16:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia16 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia16TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD16 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia16 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia16TA = 0;
                                }
                                
                                break;
                            case 17:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia17 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia17TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD17 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia17 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia17TA = 0;
                                }
                               
                                break;
                            case 18:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia18 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia18TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD18 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia18 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia18TA = 0;
                                }

                                
                                break;
                            case 19:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia19 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia19TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD19 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia19 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia19TA = 0;
                                }
                                                               
                                break;
                            case 20:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia20 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia20TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD20 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia20 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia20TA = 0;
                                }
                                
                                break;
                            case 21:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia21 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia21TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD21 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia21 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia21TA = 0;
                                }
                               
                                break;
                            case 22:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia22 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia22TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD22 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia22 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia22TA = 0;
                                }
                                
                                break;
                            case 23:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia23 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia23TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD23 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia23 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia23TA = 0;
                                }
                                
                                break;
                            case 24:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia24 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia24TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD24 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia24 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia24TA = 0;
                                }
                                
                                break;
                            case 25:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia25 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia25TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD25 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia25 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia25TA = 0;
                                }
                                
                                break;
                            case 26:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia26 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia26TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD26 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia26 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia26TA = 0;
                                }
                                
                                break;
                            case 27:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia27 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia27TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD27 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia27 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia27TA = 0;
                                }
                                
                                break;
                            case 28:
                                 if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia28 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia28TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD28 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia28 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia28TA = 0;
                                }
                                
                                break;
                            case 29:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia29 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia29TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD29 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia29 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia29TA = 0;
                                }
                               
                                break;
                            case 30:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia30 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia30TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD30 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia30 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia30TA = 0;
                                }

                                
                                break;
                            case 31:
                                if (li != null)
                                {
                                    //NuevaControlAsistencia.Dia31 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia31TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD31 = li.Formula;
                                }
                                else
                                {

                                    //NuevaControlAsistencia.Dia31 = _Listardias.Fecha;
                                    NuevaControlAsistencia.Dia31TA = 0;
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


        [HttpPost]
        public async Task<List<ControlAsistencias>> GetControlAsistenciasByEmplActivos(DateTime primero, DateTime actual)
        {
            var primerdiames = Convert.ToDateTime(primero);
            var diaactual = Convert.ToDateTime(actual);

            //DateTime Fecha = new DateTime(01-11-2019);
            List<ControlAsistencias> ControlAsistencia = new List<ControlAsistencias>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                ControlAsistenciasDTO NuevaControlAsistencia = new ControlAsistenciasDTO();
                NuevaControlAsistencia.FechaCreacion = primerdiames;//new DateTime(2019, 11, 01);
                NuevaControlAsistencia.FechaModificacion = diaactual;//DateTime.Now;
                NuevaControlAsistencia.UsuarioCreacion = HttpContext.Session.GetString("user");
                NuevaControlAsistencia.UsuarioModificacion = HttpContext.Session.GetString("user");
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/ControlAsistencias/GetControlAsistenciasByEmplActivos", NuevaControlAsistencia);
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


            return ControlAsistencia;
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
            ControlAsistencias _ControlAsis = new ControlAsistencias();

            DateTime fechafindmes = DateTime.Now;

            DateTime c = Convert.ToDateTime(_CtlAsis.Dia1.ToString("MM/dd/yyyy"));

            if (_CtlAsis.Dia1.Month == DateTime.Now.Month)
            {
                fechafindmes = DateTime.Now;
            }

            
            else
            {
                int me = _CtlAsis.Dia1.Month;
                int ao = _CtlAsis.Dia1.Year;
                int diass = DateTime.DaysInMonth(_CtlAsis.Dia1.Year, _CtlAsis.Dia1.Month);
                string fechavieja = diass + "-" + me + "-" + ao;
                fechafindmes = Convert.ToDateTime(fechavieja);
            }

            for (var d = _CtlAsis.Dia1; d < fechafindmes; d = d.AddDays(1))

            {
                switch (d.Day)
                {
                    case 1:                       
                        _ControlAsis.Fecha = Convert.ToDateTime(1 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); 
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia1 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia1.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia1TA;
                        var insert1 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 2:                       
                        _ControlAsis.Fecha = Convert.ToDateTime(2 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia2 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia2.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia2TA;
                        var insert2 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 3:                        
                        _ControlAsis.Fecha = Convert.ToDateTime(3 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia3 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia3.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia3TA;
                        var insert3 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 4:                        
                        _ControlAsis.Fecha = Convert.ToDateTime(4 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); 
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia4 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia4.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia4TA;
                        var insert4 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 5:                        
                        _ControlAsis.Fecha = Convert.ToDateTime(5 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia5 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia5.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia5TA;
                        var insert5 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 6:
                        _ControlAsis.Fecha = Convert.ToDateTime(6 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia6 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia6.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia6TA;
                        var insert6 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 7:                        
                        _ControlAsis.Fecha = Convert.ToDateTime(7 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia7 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia7.Value));
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var insert7 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 8:                        
                        _ControlAsis.Fecha = Convert.ToDateTime(8 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia8 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia8.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia8TA;
                        var insert8 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 9:
                        _ControlAsis.Fecha = Convert.ToDateTime(9 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia9 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia9.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia9TA;
                        var insert9 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 10:
                        _ControlAsis.Fecha = Convert.ToDateTime(10 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia10 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia10.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia10TA;
                        var insert10 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 11:                       
                        _ControlAsis.Fecha = Convert.ToDateTime(11 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia11 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia11.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia11TA;
                        var insert11 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 12:
                        _ControlAsis.Fecha = Convert.ToDateTime(12 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia12 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia12.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia12TA;
                        var insert12 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 13:
                        _ControlAsis.Fecha = Convert.ToDateTime(13 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia13 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia13.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia13TA;
                        var insert13 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 14:
                        _ControlAsis.Fecha = Convert.ToDateTime(14 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia14 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia14.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia14TA;
                        var insert14 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 15:
                        _ControlAsis.Fecha = Convert.ToDateTime(15 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia15 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia15.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia15TA;
                        var insert15 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 16: 
                        _ControlAsis.Fecha = Convert.ToDateTime(16 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia16 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia16.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia16TA;
                        var insert16 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 17:
                        _ControlAsis.Fecha = Convert.ToDateTime(17 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia17 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia17.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia17TA;
                        var insert17 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 18:                        
                        _ControlAsis.Fecha = Convert.ToDateTime(18 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia18 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia18.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia18TA;
                        var insert18 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 19:
                        _ControlAsis.Fecha = Convert.ToDateTime(19 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia19 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia19.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia19TA;
                        var insert19 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 20:
                        _ControlAsis.Fecha = Convert.ToDateTime(20 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia20 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia20.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia20TA;
                        var insert20 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 21:
                        _ControlAsis.Fecha = Convert.ToDateTime(21 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia21 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia21.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia21TA;
                        var insert21 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 22:
                        _ControlAsis.Fecha = Convert.ToDateTime(22 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia22 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia22.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia22TA;
                        var insert22 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 23:
                        _ControlAsis.Fecha = Convert.ToDateTime(23 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia23 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia23.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia23TA;
                        var insert23 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 24:
                        _ControlAsis.Fecha = Convert.ToDateTime(24 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia24 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia24.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia24TA;
                        var insert24 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 25:
                        _ControlAsis.Fecha = Convert.ToDateTime(25 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); 
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia25 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia25.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia25TA;
                        var insert25 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 26:
                        _ControlAsis.Fecha = Convert.ToDateTime(26 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); 
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia26 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia26.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia26TA;
                        var insert26 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 27:
                        _ControlAsis.Fecha = Convert.ToDateTime(27 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); 
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia27 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia27.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia27TA;
                        var insert27 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 28:
                        _ControlAsis.Fecha = Convert.ToDateTime(28 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); 
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia28 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia28.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia28TA;
                        var insert28 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 29:
                        _ControlAsis.Fecha = Convert.ToDateTime(29 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); 
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia29 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia29.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia29TA;
                        var insert29 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 30:
                        _ControlAsis.Fecha = Convert.ToDateTime(30 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); 
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia30 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia30.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia30TA;
                        var insert30 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 31: 
                        _ControlAsis.Fecha = Convert.ToDateTime(31 + "-" + fechafindmes.Month + "-" + fechafindmes.Year); ;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia31 = await ndia(_ControlAsis.Fecha.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia31.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia31TA;
                        var insert31 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    default:

                        break;
                }

            }

            return Json(_CtlAsis);

        }


        [HttpPost]
        public async Task<JsonResult> ndia(string dia)
        {
            int dianum = 0;
            if (dia == "lunes")
            {
                dianum = 1;
            }
            else if (dia == "martes")
            {
                dianum = 2;
            }
            else if (dia == "miércoles")
            {
                dianum = 3;
            }
            else if (dia == "jueves")
            {
                dianum = 4;
            }
            else if (dia == "viernes")
            {
                dianum = 5;
            }
            else if (dia == "sábado")
            {
                dianum = 6;
            }
            else if (dia == "domingo")
            {
                dianum = 7;
            }

            return Json(dianum);
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
                        else if (_Nuevo_Update.Id == 0 && ControlAsistenciainsert.TipoAsistencia > 0)
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

        //[HttpPost("[action]")]
        public async Task<ActionResult> Revisar(ControlAsistenciasDTO asistencias)
        {
            try
            {
                if (asistencias == null)
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/ControlAsistencias/ChangeStatus/{asistencias.IdEmpleado}/{asistencias.Anio}/{asistencias.Mes}");
                string valorrespuesta = "";
                if (!result.IsSuccessStatusCode)
                {
                    return await Task.Run(() => BadRequest("No se Aprobo la Revisión!"));
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }
        }

    }
}