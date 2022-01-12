using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Syncfusion.EJ.ReportViewer;
using Syncfusion.JavaScript.Models.ReportViewer;


namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class ReportViewerController : Controller , IReportController
    {
        private IMemoryCache _cache;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<MyConfig> _config;
        private readonly IMapper mapper;
        private readonly ILogger _logger;
        public string ArchivoReporte { get; set; }

        public string DefaultParam = null;
        public ReportViewerController(IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment
            , ILogger<SalesOrderController> logger, IOptions<MyConfig> config, IMapper mapper
            , IConfiguration configuration)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
            this.mapper = mapper;
            this._logger = logger;
            this._config = config;
            Configuration = configuration;
            ArchivoReporte = "";
        }

        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        


        [HttpPost]
        public object PostReportAction([FromBody] Dictionary<string, object> jsonResult)
        {
            //if ((string)(jsonResult["reportAction"]) == "Export")
            //{
            //    string reportname = jsonResult["Description"] != null ? jsonResult["Description"].ToString() : "Reporte";
            //    jsonResult["reportName"] = jsonResult["Description"].ToString() + DateTime.Now.Year + "_" + DateTime.Now.Month + "_"
            //                               + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second;
            //}

            //if (jsonResult.ContainsKey("CustomData"))
            //{
            //    DefaultParam = jsonResult["CustomData"].ToString();
            //}

            return ReportHelper.ProcessReport(jsonResult, this, this._cache);
        }

        [ActionName("GetResource")]
        [AcceptVerbs("GET")]
        public object GetResource(ReportResource resource)
        {
            return ReportHelper.GetResource(resource, this, _cache);
        }



        [HttpPost]
        public object PostFormReportAction()
        {
            return ReportHelper.ProcessReport(null, this, this._cache);
        }
        public IConfiguration Configuration { get; }

        [Authorize]
        public async void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            var urlBase = Configuration.GetSection("AppSettings").GetSection("urlbase").Value;
            Syncfusion.Report.DataSourceCredentials dsc = new Syncfusion.Report.DataSourceCredentials();
            dsc.ConnectionString = Utils.ConexionReportes;
            dsc.Name = "ERP";
            string basePath = _hostingEnvironment.WebRootPath;
            FileStream inputStream = new FileStream(basePath + reportOption.ReportModel.ReportPath, FileMode.Open, FileAccess.Read);
            reportOption.ReportModel.Stream = inputStream;
            reportOption.ReportModel.DataSourceCredentials.Add(dsc);
            reportOption.ReportModel.EmbedImageData = true;
            //var reportviewermodel = new ReportViewerModel();
            //reportviewermodel.ReportDefinition = new Syncfusion.RDL.DOM.ReportDefinition();
            //reportviewermodel.ReportDefinition.Description = reportOption.ReportModel.ReportPath.Substring(0, reportOption.ReportModel.ReportPath.LastIndexOf('.'));
            //reportOption.ReportModel.ReportDefinition = reportviewermodel.ReportDefinition;
           // reportOption.ReportModel.ReportDefinition.Description = reportOption.ReportModel.ReportPath.Substring(0, reportOption.ReportModel.ReportPath.LastIndexOf('.'));
           // IEnumerable<ReportParameter> reportParameters = 
            //reportOption.ReportModel.Parameters.Append(new ReportParameter()
           // {Name="NombreReporte",Values = new List<string>() { reportOption.ReportModel.ReportPath } });
            var defaultDateCulture = "es-HN";
            var ci = new CultureInfo(defaultDateCulture);
            reportOption.Culture = ci;
        }

        public  void OnReportLoaded(ReportViewerOptions reportOption)
        {
        }
    }

    /*public class SalesOrderQ
    {

       
      public static  List<SalesOrder> GetData(string param,string urlbase,string token)
        {
            string baseadress = urlbase;
            HttpClient _client = new HttpClient();
            List<SalesOrder> _SalesOrder = new  List<SalesOrder>();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var result =  _client.GetAsync(baseadress + "api/SalesOrder/GetById/"+ param).Result;
            string valorrespuesta = "";
            if (result.IsSuccessStatusCode)
            {
                valorrespuesta = (result.Content.ReadAsStringAsync()).Result;
                //  var des = (List<SalesOrder>)Newtonsoft.Json.JsonConvert.DeserializeObject(valorrespuesta, typeof(List<SalesOrder>));

                SalesOrder _SalesOrder1 = JsonConvert.DeserializeObject<SalesOrder>(valorrespuesta);
                _SalesOrder1.CustomerName = (_SalesOrder1.CustomerName == "0" || _SalesOrder1.CustomerName == "")
                                               ? _SalesOrder1.SalesOrderName : _SalesOrder1.CustomerName;
                _SalesOrder.Add(_SalesOrder1);
            }
            
            return _SalesOrder;
        }


        public static  List<SalesOrderLine> GetDataOrderLine(string param, string urlbase, string token)
        {
            HttpClient _client = new HttpClient();
            string baseadress = urlbase;
            List<SalesOrderLine> _SalesOrderLine = new List<SalesOrderLine>();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            _client.DefaultRequestHeaders.Add("SalesOrderId", param);
            var result2 =  _client.GetAsync(baseadress + "api/SalesOrderLine/").Result;
            string valorrespuesta = "";
            if (result2.IsSuccessStatusCode)
            {
                valorrespuesta =  (result2.Content.ReadAsStringAsync()).Result;
                _SalesOrderLine = JsonConvert.DeserializeObject<List<SalesOrderLine>>(valorrespuesta);
            }

        //    _SalesOrderLine.Add(new SalesOrderLine { ProductId = 3, SubProductName = "Fch", SalesOrderLineId = 10, SalesOrderId = 3, Price = 0.1, Quantity = 1 });


            return _SalesOrderLine;
        }

        public static List<CustomerConditions> GetDataCustomerConditions(CustomerConditions _Ccq, string urlbase, string token)
        {
            HttpClient _client = new HttpClient();
            string baseadress = urlbase;
            List<CustomerConditions> _SalesOrderLine = new List<CustomerConditions>();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
      
            var result2 = _client.PostAsJsonAsync(baseadress + "api/CustomerConditions/GetCustomerConditionsByClass", _Ccq).Result;
            string valorrespuesta = "";
            if (result2.IsSuccessStatusCode)
            {
                valorrespuesta = (result2.Content.ReadAsStringAsync()).Result;
                _SalesOrderLine = JsonConvert.DeserializeObject<List<CustomerConditions>>(valorrespuesta);
            }


            return _SalesOrderLine;
        }
    }*/
}