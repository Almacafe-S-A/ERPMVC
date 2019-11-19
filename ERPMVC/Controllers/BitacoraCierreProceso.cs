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

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class BitacoraCierreProcesoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public BitacoraCierreProcesoController(ILogger<BitacoraCierreProcesos> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetCierreProcesosByCierreContableId([DataSourceRequest]DataSourceRequest request, int IdProceso)
        {
            List<BitacoraCierreProcesos> _roles = new List<BitacoraCierreProcesos>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/BitacoraCierreProcesos/GetBitacoraCierreProceso");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _roles = JsonConvert.DeserializeObject<List<BitacoraCierreProcesos>>(valorrespuesta);
                    _roles = _roles.Where(q => q.IdProceso == IdProceso).ToList();

                    //foreach (var item in _roles)
                    //{
                    //   var resultclient = await _client.GetAsync(baseadress + "api/BitacoraCierreContable/GetBitacoraCierreContableById/" + item.IdBitacoraCierre).Result.Content.ReadAsStringAsync();
                    //   item.Estatus = (JsonConvert.DeserializeObject<BitacoraCierreContable>(resultclient)).Estatus;
                    //}
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return _roles.ToDataSourceResult(request);
        }
    }
}