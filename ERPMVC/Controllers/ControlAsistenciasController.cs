using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
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

        //Llenar Grid
        [HttpGet("[controller]/GetGetControlAsistencias")]
        public async Task<DataSourceResult> GetGetControlAsistencias([DataSourceRequest]DataSourceRequest request, string datepikerselect)
        {
            //Fechas Comparaciones
            var diactual = datepikerselect;
            var dctual = Convert.ToDateTime(diactual);
            var pmes = Convert.ToDateTime(1 + "-" + dctual.Month + "-" + dctual.Year);

            DateTime fechafindmes = DateTime.Now;

            if (diactual == null)
            {
                fechafindmes = DateTime.Now;
                pmes = Convert.ToDateTime(1 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                 
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
                string fechavieja = diass + "-" + me + "-" + ao;
                fechafindmes = Convert.ToDateTime(fechavieja);
            }
            int letras = DateTime.DaysInMonth(fechafindmes.Year, fechafindmes.Month);
            //Finde de  Comparaciones de fechas
            List<ControlAsistenciasDTO> _ControlAsistencias = new List<ControlAsistenciasDTO>();          
            

            try
            {
                var _ListadoEmpleados = await ListadoEmpleados();
                List<Employees> _ListEmpleados = ((List<Employees>)_ListadoEmpleados.Value);

                var _ListadoAsistenciasporEmpleadp = await ListaAsistenciasporempleado(_ListadoEmpleados, pmes, fechafindmes);

                List<ControlAsistenciasDTO> _Lista = ((List<ControlAsistenciasDTO>)_ListadoAsistenciasporEmpleadp.Value);
                _ControlAsistencias = _Lista.OrderBy(order => order.Empleado.NombreEmpleado).ToList();


              


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ControlAsistencias.ToDataSourceResult(request);
        }



        [HttpPost]
        public async Task<JsonResult> ListaAsistenciasporempleado(JsonResult _ListadoEmpleados,DateTime pmes, DateTime fechafindmes)
        {
            List<ControlAsistenciasDTO> _ControlAsistencias = new List<ControlAsistenciasDTO>();

            List<Employees> _ListEmpleados = ((List<Employees>)_ListadoEmpleados.Value);
                






            
            try
            {
                foreach (var _ListEmpleadosAsistencias in _ListEmpleados)
                {
                    ControlAsistenciasDTO NuevaControlAsistencia = new ControlAsistenciasDTO();
                    NuevaControlAsistencia.Empleado = _ListEmpleadosAsistencias;
                    NuevaControlAsistencia.EmployeesId = _ListEmpleadosAsistencias.IdEmpleado;

                    var fechas = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, pmes, fechafindmes);
                    List<ControlAsistencias> LSCA = ((List<ControlAsistencias>)fechas.Value);


                   
                   short LlegadaTarde = 77;
                   var Llegatarde = await CantidadTipoAsietnciaByEmpleado(NuevaControlAsistencia, pmes, fechafindmes, LlegadaTarde);
                   NuevaControlAsistencia.LlegadasTarde = Convert.ToInt32(Llegatarde.Value);
                   int cLLegadasTarde = Convert.ToInt32(Llegatarde.Value);
                   //----------------------------------------------------------------------------------------------------

                   short Domingodiaslibres = 78;
                   var DomigosyLibres = await CantidadTipoAsietnciaByEmpleado(NuevaControlAsistencia, pmes, fechafindmes, Domingodiaslibres);
                   int cDomingosandLibres = Convert.ToInt32(DomigosyLibres.Value);
                   //----------------------------------------------------------------------------------------------------
                   short Permisoshoras = 79;
                   var PermisosH = await CantidadTipoAsietnciaByEmpleado(NuevaControlAsistencia, pmes, fechafindmes, Permisoshoras);
                   int cPermisoshora = Convert.ToInt32(PermisosH.Value);
                   //----------------------------------------------------------------------------------------------------
                   short Incapacidad = 80;
                   var Incapacidades = await CantidadTipoAsietnciaByEmpleado(NuevaControlAsistencia, pmes, fechafindmes, Incapacidad);
                   int cIncapacidades = Convert.ToInt32(Incapacidades.Value);
                   //----------------------------------------------------------------------------------------------------
                   short Vacacion = 81;
                   var Vacaciones = await CantidadTipoAsietnciaByEmpleado(NuevaControlAsistencia, pmes, fechafindmes, Vacacion);
                   int cVacaiones = Convert.ToInt32(Vacaciones.Value);
                   //----------------------------------------------------------------------------------------------------
                   short Permiso = 82;
                   var Permisos = await CantidadTipoAsietnciaByEmpleado(NuevaControlAsistencia, pmes, fechafindmes, Permiso);
                   int cPermisos = Convert.ToInt32(Permisos.Value);
                   //----------------------------------------------------------------------------------------------------
                   short Presente = 83;
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
                       NuevaControlAsistencia.PorcentajeLlegadasTarde = Convert.ToString(porcentajellegadastarde + " %");
                   }



                    foreach (var _Listardias in LSCA)
                    {


                        var getTipoAsistencia = await ColorTipoAsistencia(Convert.ToInt32(_Listardias.TipoAsistencia));
                        var li = ((ElementoConfiguracion)getTipoAsistencia.Value);

                        var fechasdia = await GetControlAsistenciasByEmpl(NuevaControlAsistencia, pmes, pmes);
                        List<ControlAsistencias> LSCAdia = ((List<ControlAsistencias>)fechasdia.Value);





                        int letras = DateTime.DaysInMonth(fechafindmes.Year, fechafindmes.Month);

                        var l1 = Convert.ToDateTime(1 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD1 = l1.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia1 = l1;

                        var l2 = Convert.ToDateTime(2 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD2 = l2.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia2 = l2;

                        var l3 = Convert.ToDateTime(3 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD3 = l3.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia3 = l3;

                        var l4 = Convert.ToDateTime(4 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD4 = l4.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia4 = l4;

                        var l5 = Convert.ToDateTime(5 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD5 = l5.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia5 = l5;

                        var l6 = Convert.ToDateTime(6 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD6 = l6.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia6 = l6;

                        var l7 = Convert.ToDateTime(7 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD7 = l7.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia7 = l7;

                        var l8 = Convert.ToDateTime(8 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD8 = l8.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia8 = l8;

                        var l9 = Convert.ToDateTime(9 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD9 = l9.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia9 = l9;

                        var l10 = Convert.ToDateTime(10 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD10 = l10.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia11 = l10;

                        var l11 = Convert.ToDateTime(11 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD11 = l11.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia11 = l11;

                        var l12 = Convert.ToDateTime(12 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD12 = l12.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia12 = l12;

                        var l13 = Convert.ToDateTime(13 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD13 = l13.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia13 = l13;

                        var l14 = Convert.ToDateTime(14 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD14 = l14.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia14 = l14;

                        var l15 = Convert.ToDateTime(15 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD15 = l15.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia15 = l15;

                        var l16 = Convert.ToDateTime(16 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD16 = l16.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia16 = l16;

                        var l17 = Convert.ToDateTime(17 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD17 = l17.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia17 = l17;

                        var l18 = Convert.ToDateTime(18 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD18 = l18.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia18 = l18;

                        var l19 = Convert.ToDateTime(19 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD19 = l19.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia19 = l19;

                        var l20 = Convert.ToDateTime(20 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD20 = l20.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia20 = l20;

                        var l21 = Convert.ToDateTime(21 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD21 = l21.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia21 = l21;

                        var l22 = Convert.ToDateTime(22 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD22 = l22.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia22 = l22;

                        var l23 = Convert.ToDateTime(23 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD23 = l23.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia23 = l23;

                        var l24 = Convert.ToDateTime(24 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD24 = l24.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia24 = l24;

                        var l25 = Convert.ToDateTime(25 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD25 = l25.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia25 = l25;

                        var l26 = Convert.ToDateTime(26 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD26 = l26.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia26 = l26;

                        var l27 = Convert.ToDateTime(27 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD27 = l27.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia27 = l27;

                        var l28 = Convert.ToDateTime(28 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        NuevaControlAsistencia.LetraD28 = l28.ToString("dddd").Substring(0, 1).ToUpper();
                        NuevaControlAsistencia.Dia28 = l28;

                        if (letras == 29)
                        {
                            var l29 = Convert.ToDateTime(29 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                            NuevaControlAsistencia.LetraD29 = l29.ToString("dddd").Substring(0, 1).ToUpper();
                            NuevaControlAsistencia.Dia29 = l29;
                        }
                        if (letras == 30)
                        {
                            var l29 = Convert.ToDateTime(29 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                            NuevaControlAsistencia.LetraD29 = l29.ToString("dddd").Substring(0, 1).ToUpper();
                            NuevaControlAsistencia.Dia29 = l29;

                            var l30 = Convert.ToDateTime(30 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                            NuevaControlAsistencia.LetraD30 = l30.ToString("dddd").Substring(0, 1).ToUpper();
                            NuevaControlAsistencia.Dia30 = l30;
                        }
                        if (letras == 31)
                        {
                            var l29 = Convert.ToDateTime(29 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                            NuevaControlAsistencia.LetraD29 = l29.ToString("dddd").Substring(0, 1).ToUpper();
                            NuevaControlAsistencia.Dia29 = l29;

                            var l30 = Convert.ToDateTime(30 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                            NuevaControlAsistencia.LetraD30 = l30.ToString("dddd").Substring(0, 1).ToUpper();
                            NuevaControlAsistencia.Dia30 = l30;

                            var l31 = Convert.ToDateTime(31 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                            NuevaControlAsistencia.LetraD31 = l31.ToString("dddd").Substring(0, 1).ToUpper();
                            NuevaControlAsistencia.Dia31 = l31;
                        }


                        switch (_Listardias.Fecha.Day)
                        {
                            case 1:                                                            
                                    
                                    NuevaControlAsistencia.Dia1TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD1 = li.Formula;                                
                                break;
                            case 2:
                                   
                                    NuevaControlAsistencia.Dia2TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD2 = li.Formula;
                                break;
                            case 3:
                                    NuevaControlAsistencia.Dia3TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD3 = li.Formula;                                
                                break;
                            case 4:
                                    NuevaControlAsistencia.Dia4TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD4 = li.Formula;
                               
                                break;
                            case 5:
                               
                                    NuevaControlAsistencia.Dia5TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD5 = li.Formula;
                               
                                break;
                            case 6:
                               
                                    NuevaControlAsistencia.Dia6TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD6 = li.Formula;
                               
                                break;
                            case 7:
                                
                                    NuevaControlAsistencia.Dia7TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD7 = li.Formula;
                               
                                break;
                            case 8:
                               
                                    NuevaControlAsistencia.Dia8TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD8 = li.Formula;
                               
                                break;
                            case 9:
                               
                                    NuevaControlAsistencia.Dia9TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD9 = li.Formula;
                                
                                break;
                            case 10:
                                
                                    NuevaControlAsistencia.Dia10TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD10 = li.Formula;
                                
                                break;
                            case 11:

                                NuevaControlAsistencia.Dia11TA = _Listardias.TipoAsistencia;
                                NuevaControlAsistencia.ColorD11 = li.Formula;

                                break;

                            case 12:
                                
                                    NuevaControlAsistencia.Dia12TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD12 = li.Formula;

                                break;
                            case 13:
                                
                                    NuevaControlAsistencia.Dia13TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD13 = li.Formula;
                               
                                break;
                            case 14:
                               
                                    NuevaControlAsistencia.Dia14TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD14 = li.Formula;
                               
                                break;
                            case 15:
                                
                                    NuevaControlAsistencia.Dia15TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD15 = li.Formula;
                               
                                break;
                            case 16:
                                
                                    NuevaControlAsistencia.Dia16TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD16 = li.Formula;
                               
                                break;
                            case 17:
                                
                                    NuevaControlAsistencia.Dia17TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD17 = li.Formula;
                               
                                break;
                            case 18:
                               
                                    NuevaControlAsistencia.Dia18TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD18 = li.Formula;
                                
                                break;
                            case 19:
                               
                                    NuevaControlAsistencia.Dia19TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD19 = li.Formula;
                                
                                break;
                            case 20:
                               
                                    NuevaControlAsistencia.Dia20TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD20 = li.Formula;
                               
                                break;
                            case 21:
                                
                                    NuevaControlAsistencia.Dia21TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD21 = li.Formula;
                                
                                break;
                            case 22:
                               
                                    NuevaControlAsistencia.Dia22TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD22 = li.Formula;
                               
                                break;
                            case 23:
                               
                                    NuevaControlAsistencia.Dia23TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD23 = li.Formula;
                               
                                break;
                            case 24:
                               
                                    NuevaControlAsistencia.Dia24TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD24 = li.Formula;
                               
                                break;
                            case 25:
                               
                                    NuevaControlAsistencia.Dia25TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD25 = li.Formula;
                               
                                break;
                            case 26:
                               
                                    NuevaControlAsistencia.Dia26TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD26 = li.Formula;
                               
                                break;
                            case 27:
                               
                                    NuevaControlAsistencia.Dia27TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD27 = li.Formula;
                               
                                break;
                            case 28:
                               
                                    NuevaControlAsistencia.Dia28TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD28 = li.Formula;
                               
                                break;
                            case 29:
                                
                                    NuevaControlAsistencia.Dia29TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD29 = li.Formula;
                               
                                break;
                            case 30:
                               
                                    NuevaControlAsistencia.Dia30TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD30 = li.Formula;
                                
                                break;
                            case 31:
                               
                                    NuevaControlAsistencia.Dia31TA = _Listardias.TipoAsistencia;
                                    NuevaControlAsistencia.ColorD31 = li.Formula;
                               
                                break;
                            default:

                                break;

                        }                        
                    }

                    _ControlAsistencias.Add(NuevaControlAsistencia);

                }


            }
            catch (Exception Ex)
            {
                _logger.LogError($"Ocurrio un error: { Ex.ToString() }");
                throw Ex;
            }
            return Json(_ControlAsistencias);
        }



        [HttpPost]
        public async Task<JsonResult> ListadoEmpleados()
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
                  
                }
            }
            catch(Exception Ex)
            {
                _logger.LogError($"Ocurrio un error: { Ex.ToString() }");
                throw Ex;
            }
            return Json(_ListEmpleados);
        }

        [HttpPost]
        public async Task<JsonResult> GetControlAsistenciasByEmpl(ControlAsistencias NuevaControlAsistencia, DateTime primero, DateTime actual)
        {
            var primerdiames = Convert.ToDateTime(primero);
            var diaactual = Convert.ToDateTime(actual);
                        
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

        //---------------------------

        [HttpPost("PostControlAsistencias")]
        public async Task<ActionResult<ControlAsistencias>> PostControlAsistencias(ControlAsistenciasDTO _CtlAsis)
        {
            ControlAsistencias _ControlAsis = new ControlAsistencias();

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
                string fechavieja = diass + "-" + me + "-" + ao;
                fechafindmes = Convert.ToDateTime(fechavieja);
            }

            for (var d = _CtlAsis.Dia1; d < fechafindmes; d = d.AddDays(1))

            {
                switch (d.Day)
                {
                    case 1:

                        var f1 =  Convert.ToDateTime(1 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f1;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia1 = await ndia(f1.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia1.Value));                        
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia1TA;
                        var insert1 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 2:
                        var f2 = Convert.ToDateTime(2 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f2;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia2 = await ndia(f2.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia2.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia2TA;
                        var insert2 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 3:
                        var f3 = Convert.ToDateTime(3 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f3;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia3 = await ndia(_CtlAsis.Dia3.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia3.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia3TA;
                        var insert3 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 4:
                        var f4 = Convert.ToDateTime(4 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f4;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia4 = await ndia(_CtlAsis.Dia4.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia4.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia4TA;
                        var insert4 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 5:
                        var f5 = Convert.ToDateTime(5 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f5;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia5 = await ndia(_CtlAsis.Dia5.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia5.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia5TA;
                        var insert5 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 6:
                        var f6 = Convert.ToDateTime(6 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f6;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia6 = await ndia(_CtlAsis.Dia6.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia6.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia6TA;
                        var insert6 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 7:
                        var f7 = Convert.ToDateTime(7 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = _CtlAsis.Dia7;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia7 = await ndia(_CtlAsis.Dia7.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia7.Value));
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var insert7 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 8:
                        var f8 = Convert.ToDateTime(8 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = _CtlAsis.Dia8;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia8 = await ndia(_CtlAsis.Dia8.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia8.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia8TA;
                        var insert8 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 9:
                        var f9 = Convert.ToDateTime(9 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f9;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia9 = await ndia(_CtlAsis.Dia9.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia9.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia9TA;
                        var insert9 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 10:
                        var f10 = Convert.ToDateTime(10 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f10;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia10 = await ndia(_CtlAsis.Dia10.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia10.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia10TA;
                        var insert10 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 11:
                        var f11 = Convert.ToDateTime(11 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f11;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia11 = await ndia(_CtlAsis.Dia11.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia11.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia11TA;
                        var insert11 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 12:
                        var f12 = Convert.ToDateTime(12 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f12;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia12 = await ndia(_CtlAsis.Dia12.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia12.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia12TA;
                        var insert12 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 13:
                        var f13 = Convert.ToDateTime(13 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f13;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia13 = await ndia(_CtlAsis.Dia13.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia13.Value));                       
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia13TA;
                        var insert13 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 14:
                        var f14 = Convert.ToDateTime(14 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f14;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia14 = await ndia(_CtlAsis.Dia14.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia14.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia14TA;
                        var insert14 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 15:
                        var f15 = Convert.ToDateTime(15 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f15;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia15 = await ndia(_CtlAsis.Dia5.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia15.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia15TA;
                        var insert15 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 16:
                        var f16 = Convert.ToDateTime(16 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f16;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia16 = await ndia(_CtlAsis.Dia16.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia16.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia16TA;
                        var insert16 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 17:
                        var f17 = Convert.ToDateTime(17 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f17;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia17 = await ndia(_CtlAsis.Dia17.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia17.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia17TA;
                        var insert17 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 18:
                        var f18 = Convert.ToDateTime(18 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f18;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia18 = await ndia(_CtlAsis.Dia18.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia18.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia18TA;
                        var insert18 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 19:
                        var f19 = Convert.ToDateTime(19 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f19;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia19 = await ndia(_CtlAsis.Dia19.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia19.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia19TA;
                        var insert19 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 20:
                        var f20 = Convert.ToDateTime(20 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f20;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia20 = await ndia(_CtlAsis.Dia20.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia20.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia20TA;
                        var insert20 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 21:
                        var f21 = Convert.ToDateTime(21 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f21;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia21 = await ndia(_CtlAsis.Dia21.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia21.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia21TA;
                        var insert21 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 22:
                        var f22 = Convert.ToDateTime(22 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f22;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia22 = await ndia(_CtlAsis.Dia22.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia22.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia22TA;
                        var insert22 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 23:
                        var f23 = Convert.ToDateTime(23 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f23;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia23 = await ndia(_CtlAsis.Dia23.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia23.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia23TA;
                        var insert23 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 24:
                        var f24 = Convert.ToDateTime(24 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f24;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia24 = await ndia(_CtlAsis.Dia24.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia24.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia24TA;
                        var insert24 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 25:
                        var f25 = Convert.ToDateTime(25 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f25;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia25 = await ndia(_CtlAsis.Dia25.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia25.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia25TA;
                        var insert25 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 26:
                        var f26 = Convert.ToDateTime(26 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f26;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia26 = await ndia(_CtlAsis.Dia26.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia26.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia26TA;
                        var insert26 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 27:
                        var f27 = Convert.ToDateTime(27 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f27;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia27 = await ndia(_CtlAsis.Dia27.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia27.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia27TA;
                        var insert27 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 28:
                        var f28 = Convert.ToDateTime(28 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f28;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia28 = await ndia(_CtlAsis.Dia28.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia28.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia28TA;
                        var insert28 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 29:
                        var f29 = Convert.ToDateTime(29 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f29;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia29 = await ndia(_CtlAsis.Dia29.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia29.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia29TA;
                        var insert29 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 30:
                        var f30 = Convert.ToDateTime(30 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f30;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia30 = await ndia(_CtlAsis.Dia30.ToString("dddd"));
                        _ControlAsis.Dia = (Convert.ToInt32(dia30.Value));
                        _ControlAsis.TipoAsistencia = _CtlAsis.Dia30TA;
                        var insert30 = await SaveControlAsistencia(_ControlAsis);
                        break;
                    case 31:
                        var f31 = Convert.ToDateTime(31 + "-" + fechafindmes.Month + "-" + fechafindmes.Year);
                        _ControlAsis.Fecha = f31;
                        _ControlAsis.IdEmpleado = _CtlAsis.EmployeesId;
                        var dia31 = await ndia(_CtlAsis.Dia31.ToString("dddd"));
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


    }
}