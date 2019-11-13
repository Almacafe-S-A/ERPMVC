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

        public IActionResult ControlAsistencia()
        {
            return View();
        }



        public IActionResult DiasMes()
        {
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


        [HttpGet]
        public async Task<DataSourceResult> GetGetControlAsistencias([DataSourceRequest]DataSourceRequest request)
        {
            List<ControlAsistencias> _ControlAsistencias = new List<ControlAsistencias>();

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
                    _ListEmpleados = _ListEmpleados.OrderByDescending(q => q.IdEmpleado).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            // Fin de lista de empleados

            ControlAsistencias NuevaControlAsistencia = new ControlAsistencias();
            NuevaControlAsistencia.Empleado = new Employees();

            //var lista = _ListEmpleados
            // .Select(a => new { a.IdEmpleado, a.NombreEmpleado })
            // .Concat(_ControlAsistencias.
            //  Select(p => new { p.Empleado.IdEmpleado,p.Id }))
            // .OrderBy(x => x.Id);




            //----------------------------------------------------------------------------------------


            return _ControlAsistencias.ToDataSourceResult(request);

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