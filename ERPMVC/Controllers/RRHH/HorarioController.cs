using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using ERPMVC.DTO;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class HorarioController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;

        public HorarioController(IOptions<MyConfig> config, ILogger<HorarioController> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<DataSourceResult> GetHorarios([DataSourceRequest] DataSourceRequest request)
        {
            List<Horario> horario = new List<Horario>();
            try { 
            var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                config.Value.urlbase + "api/Horario/GetHorarios");
            string valorrespuesta = "";
            if (respuesta.IsSuccessStatusCode)
            {
                valorrespuesta = await respuesta.Content.ReadAsStringAsync();
                horario = JsonConvert.DeserializeObject<List<Horario>>(valorrespuesta);
                    horario = horario.OrderByDescending(x => x.Id).ToList();
            }
        }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al cargar los horarios");
                throw ex;
            }
            return horario.ToDataSourceResult(request);
        }

        public async Task<ActionResult> Guardar([DataSourceRequest] DataSourceRequest request, Horario horario)
        {
            try
            {
                    if (horario.Id == 0)
                    {
                        horario.UsuarioCreacion = HttpContext.Session.GetString("user");
                        horario.UsuarioModificacion = horario.UsuarioCreacion;
                        horario.FechaCreacion = DateTime.Now;
                        horario.FechaModificacion = horario.FechaCreacion;
                        horario.IdEstado = horario.activo ? 1 : 2;
                }
                else
                    {
                        horario.UsuarioModificacion = HttpContext.Session.GetString("user");
                        horario.FechaModificacion = DateTime.Now;
                        horario.IdEstado = horario.activo ? 1 : 2;
                }
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                        config.Value.urlbase + "api/Horario/Guardar", horario);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        var contenido = await respuesta.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<Horario>(contenido);
                        horario.Id = resultado.Id;
                        return Json(new[] { resultado }.ToDataSourceResult(request, ModelState));
                    }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al actualizar horario");
                return BadRequest();
            }
            return Ok();
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddHorario([FromBody] HorarioDTO _sarpara)
        {
            HorarioDTO _Horario = new HorarioDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Horario/GetHorarioById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Horario = JsonConvert.DeserializeObject<HorarioDTO>(valorrespuesta);

                }

                if (_Horario == null)
                {
                    _Horario = new HorarioDTO();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }



            return PartialView(_Horario);

        }

        [HttpPost]
        public async Task<ActionResult<Horario>> SaveHorario([FromBody] HorarioDTO horarioDTO)
        {
                Horario _Horario = horarioDTO; // Convierte el DTO en un objeto Horario
            try
            {
                // Verificar si el horarioDTO es nulo
                if (horarioDTO == null)
                {
                    return BadRequest("No se envió correctamente el horario.");
                }

                // Convertir las horas de cadena a objetos TimeSpan
                TimeSpan horaInicio;
                if (!TimeSpan.TryParse(horarioDTO.HoraInicio, out horaInicio))
                {
                    return BadRequest("La hora de inicio no es válida.");
                }

                TimeSpan horaFinal;
                if (!TimeSpan.TryParse(horarioDTO.HoraFinal, out horaFinal))
                {
                    return BadRequest("La hora final no es válida.");
                }

                // Verificar que la hora de inicio sea menor que la hora final
                if (horaInicio >= horaFinal)
                {
                    return BadRequest("La hora de inicio debe ser menor que la hora final.");
                }


                if (horarioDTO.Id == 0)
                {
                    var insertResult = await Insert(horarioDTO); // Define tu método InsertHorario
                }
                else
                {
                    var updateResult = await Update(_Horario.Id, _Horario); // Define tu método UpdateHorario
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrió un error: {ex.ToString()}");
                throw ex;
            }

            return Json(_Horario);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<Horario>> Insert(Horario horario)
        {
            Horario _Horario = horario;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                horario.UsuarioCreacion = HttpContext.Session.GetString("user");
                horario.UsuarioModificacion = HttpContext.Session.GetString("user");
                horario.FechaCreacion = DateTime.Now;
                horario.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Horario/Insert", horario);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    horario = JsonConvert.DeserializeObject<Horario>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return Ok(horario);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 Id, Horario horario)
        {
            Horario _Horario = horario;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Horario.FechaModificacion = DateTime.Now;
                _Horario.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/Horario/Update", _Horario);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Horario = JsonConvert.DeserializeObject<Horario>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Horario }, Total = 1 });
        }

        [HttpPost]
        public async Task<ActionResult<Horario>> Delete(Int64 Id, [FromBody] Horario horario)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Horario/Delete", horario);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    horario = JsonConvert.DeserializeObject<Horario>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrio un error: {ex.ToString()}");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { horario }, Total = 1 });
        }


    }
}