using GrapeCity.ActiveReports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Text;
using GrapeCity.ActiveReports.Data;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ERPAPI.Models;
using System.Configuration;

namespace AR_WebService
{
    /// <summary>
    /// Summary description for CustomReportService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CustomReportService : GrapeCity.ActiveReports.Web.ReportService
    {
        [WebMethod]
        protected override object OnCreateReportHandler(string reportPath)
        {
            string rptPath = "";
            var path = new string[0];
            if (reportPath.Contains(","))
            {
                path = reportPath.Split(',');
                rptPath = path[0];
            }
            else
            {
                rptPath = reportPath;
            }

           // SectionReport rptOrders;

            switch (rptPath)
            {
                case "Reports/BillingInvoice.rdlx":
                case "Reports/Orders.rdlx":
                    PageReport rptPageRDL = new PageReport();
                    rptPageRDL.Load(new FileInfo(Server.MapPath(reportPath)));
                    rptPageRDL.Report.DataSources[0].ConnectionProperties.ConnectString = "data source=" + Server.MapPath("~/App_Data/NWind.mdb") + ";provider=Microsoft.Jet.OLEDB.4.0;";
                    return rptPageRDL;

                case "Reports/cotizacion.rdlx":
                    var str = path[1].ToString().Split(':');
                    var param = str[0];
                     paramValue = str[1];
                     token = "";
                    var fortoken = path[2].ToString().Split('=');
                    token = fortoken[1];

                    PageReport rptOrders;
                    rptOrders = new GrapeCity.ActiveReports.PageReport(new System.IO.FileInfo(Server.MapPath(rptPath)));
                    rptOrders.Document.LocateDataSource += new LocateDataSourceEventHandler(runt_LocateDataSource);

                    //rptOrders = new SectionReport();
                    //XmlTextReader xtr = new XmlTextReader(Server.MapPath(rptPath));
                    //rptOrders.LoadLayout(xtr);                   

                   // (rptOrders.DataSource as OleDBDataSource).ConnectionString = "data source=" + Server.MapPath("~/App_Data/NWind.mdb") + ";provider=Microsoft.Jet.OLEDB.4.0;";
                   // (rptOrders.DataSource as OleDBDataSource).SQL = @"SELECT * FROM Invoices ";

                   // GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport p = new GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport();
                 //   p.Export(rptOrders.Document, "C:\\files\\p.pdf");
                    //(rptOrders.DataSource as OleDBDataSource).SQL = @"SELECT * FROM Invoices where Country='" + paramValue + "'";
                    return rptOrders;
                default:
                    return base.OnCreateReportHandler(reportPath);
            }


        }

        string token = "", paramValue = "";
        void runt_LocateDataSource(object sender, GrapeCity.ActiveReports.LocateDataSourceEventArgs args)
        {

            string baseadress = ConfigurationManager.AppSettings["baseaddress"];
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var result = _client.GetAsync(baseadress + "api/SalesOrder/GetById/" + paramValue).Result;
            SalesOrder _salesorder = new SalesOrder();
            string valorrespuesta = "";
            if (result.IsSuccessStatusCode)
            {
                valorrespuesta = (result.Content.ReadAsStringAsync()).Result;
                _salesorder = JsonConvert.DeserializeObject<SalesOrder>(valorrespuesta);

            }

            // int id = (int)args.Report.Parameters[0].CurrentValue;
            //    args.Report.Parameters["CustomerId"].CurrentValue = _salesorder.CustomerId;
            //args.Report.Parameters["CustomerId"].CurrentValue = _salesorder.CustomerId;
            // var collect = _salesorder.SalesOrderLines;
            if (args.DataSetName == "DataSet1")
            {
                args.Data = _salesorder.SalesOrderLines;
            }

           
           
            if(args.DataSetName== "DataSet2")
            {
                List<SalesOrder> _list = new List<SalesOrder>();
                _list.Add(_salesorder);
                args.Data = _list;
            }
            
          

        }




    }
}
