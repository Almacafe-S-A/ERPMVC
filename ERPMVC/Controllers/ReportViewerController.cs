using System;
using System.Collections.Generic;
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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ReportViewerController : Controller , IReportController
    {
        private IMemoryCache _cache;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<MyConfig> _config;
        private readonly IMapper mapper;
        private readonly ILogger _logger;

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
            if ((string)(jsonResult["reportAction"]) == "Export")
            {
                jsonResult["reportName"] = "my_custom_report";
            }

            if (jsonResult.ContainsKey("CustomData"))
            {
                DefaultParam = jsonResult["CustomData"].ToString();
            }

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
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {

            //Syncfusion.Report.DataSourceCredentials dsc = new Syncfusion.Report.DataSourceCredentials();
            //dsc.ConnectionString = "Data Source=DESKTOP-RFQ3R0I;Initial Catalog=ERP;";//Configuration.GetConnectionString("DefaultConnection");
            //dsc.IntegratedSecurity = false;
            //dsc.UserId = "sa";
            //dsc.Password = "sql20.15";
            //dsc.Name = "DefaultConnection";
            //if (reportOption.ReportModel.DataSourceCredentials == null)
            //{ reportOption.ReportModel.DataSourceCredentials = new List<Syncfusion.Report.DataSourceCredentials>(); }
            //reportOption.ReportModel.DataSourceCredentials.Add(dsc);
            //var res = reportOption.ReportModel.DataSourceCredentials.Select(q => q.ConnectionString);
            //reportOption.ReportModel.DataSourceCredentials.Add(new DataSourceCredentials("AdventureWorks", "sa", "sql20.14"));

            //reportOption.ReportModel.DataSourceCredentials.Remove(new Syncfusion.Report.DataSourceCredentials { Name="DataSource1" });

//            reportOption.ReportModel.DataSourceCredentials.Add(new Syncfusion.Report.DataSourceCredentials { ConnectionString= "Data Source=DESKTOP-RFQ3R0I;Initial Catalog=ERP;User id=sa;password=sql20.15;", UserId="sa",Password="sql20.14" });

            string basePath = _hostingEnvironment.WebRootPath;
            FileStream inputStream = new FileStream(basePath + reportOption.ReportModel.ReportPath, FileMode.Open, FileAccess.Read);
            reportOption.ReportModel.Stream = inputStream;



            //reportOption.ReportModel.ProcessingMode = Syncfusion.EJ.ReportViewer.ProcessingMode.Local;
            //reportOption.ReportModel.ProcessingMode = Syncfusion.EJ.ReportViewer.ProcessingMode.Remote;
        }

        public  void OnReportLoaded(ReportViewerOptions reportOption)
        {

            //Assembly assembly = typeof(ReportViewerController).GetTypeInfo().Assembly;
            //var resourceName = "ReportSample_core.wwwroot.ReportRDL.ARIALUNI.TTF";

            //if (reportOption.ReportModel.PDFOptions == null)
            //{
            //    reportOption.ReportModel.PDFOptions = new Syncfusion.ReportWriter.PDFOptions();
            //}

            //if (reportOption.ReportModel.PDFOptions.Fonts == null)
            //{
            //    reportOption.ReportModel.PDFOptions.Fonts = new Dictionary<string, Stream>(StringComparer.OrdinalIgnoreCase);
            //}

            //reportOption.ReportModel.PDFOptions.Fonts.Add("Arial", Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName));

            var parameters = new List<ReportParameter>();
            if (DefaultParam != null)
            {
                parameters = JsonConvert.DeserializeObject<List<ReportParameter>>(DefaultParam);
            }

            //reportOption.ReportModel.Parameters = parameters;
            if (parameters != null && parameters.Count > 0)
            {
               
                // reportOption.ReportModel.Parameters = ;

                //reportOption.ReportModel.DataSources.Clear();
                //var salesorder = SalesOrderQ.GetData(parameters[0].Values[0], _config.Value.urlbase, HttpContext.Session.GetString("token"));

                //reportOption.ReportModel.DataSources.Add(new Syncfusion.Report.ReportDataSource
                //{
                //    Name = "SalesOrder",
                //    Value = salesorder,
                //});

                //var salesorderline = SalesOrderQ.GetDataOrderLine(parameters[0].Values[0], _config.Value.urlbase, HttpContext.Session.GetString("token"));
                //reportOption.ReportModel.DataSources.Add(new Syncfusion.Report.ReportDataSource
                //{
                //    Name = "SalesOrderLine",
                //    Value = salesorderline,
                //});

                //CustomerConditions _cc = new CustomerConditions
                //{
                //    IdTipoDocumento = 12,                  
                //    DocumentId = Convert.ToInt64(parameters[0].Values[0]),
                //    ProductId = 0,                     
                //    //FechaCreacion = DateTime.Now,
                //    //FechaModificacion = DateTime.Now
                //    //,
                //    //ConditionId = 0,
                //    //CustomerConditionId = 0,
                //    //CustomerId = 0
                //    // ,
                //    //LogicalCondition = ">",
                //    //UsuarioCreacion = "admin",
                //    //UsuarioModificacion = "admin",
                //    //CustomerConditionName = "asad"
                //    // ,
                //    //ValueDecimal = 0,
                //    //ValueString = "0",
                //    //ValueToEvaluate = "0"

                //};
                //var CustomerConditions = SalesOrderQ.GetDataCustomerConditions(_cc, _config.Value.urlbase, HttpContext.Session.GetString("token"));
                //reportOption.ReportModel.DataSources.Add(new Syncfusion.Report.ReportDataSource
                //{
                //    Name = "Condiciones",
                //    Value = CustomerConditions,
                //});



                //   reportOption.ReportModel.DataSources.Add(new ReportDataSource { Name = "StoreSales", Value = StoreSales.GetData(Convert.ToInt32(parameters[0].Values[0])) });
            }
            //else
            //{
            //    reportOption.ReportModel.DataSources.Clear();
            //    reportOption.ReportModel.DataSources.Add(new ReportDataSource { Name = "StoreSales", Value = StoreSales.GetData(Convert.ToInt32("29825")) });
            //}
        }
       







    }


    public class SalesOrderQ
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
    }
}