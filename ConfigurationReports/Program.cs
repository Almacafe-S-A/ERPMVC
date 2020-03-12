using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
//using Syncfusion.RDL.DOM;

namespace ReportSettings
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("El programa va a reemplazar el DataSource(cadena de conexión de todos los reportes\n que se encuentren en la ruta del ejecutable ),"
                +"\n Ejemplo Cadena de conexión: Data Source=localhost;Initial Catalog=ERP;User id=bidss;password=1234;"
                +"\n Esta seguro que desea continuar?: (S/N) ");
            string respuesta = Console.ReadLine();

            if (respuesta == "S")
            {
                Console.Write("Ingrese Connection String: ");
                string conne = Console.ReadLine();

                DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory());
                FileInfo[] Files = d.GetFiles("*.rdl"); //Getting Text files               
                foreach (FileInfo file in Files)
                {
                    //string conne = "Data Source=DESKTOP-RFQ3R0I; Initial Catalog = ERP; User ID = sa;Password =sql20.15;";
                    FileStream stream = new FileStream(file.Name, FileMode.Open, FileAccess.Read, FileShare.Read);
                    // Syncfusion.RDL.DOM.ReportDefinition reportDefinition;//= new Syncfusion.RDL.DOM.ReportDefinition();
                    XElement rdl = XElement.Load(XmlReader.Create(stream));
                    string Namespace = (from attribute in rdl.Attributes() where attribute.Name.LocalName == "xmlns" select attribute.Value).FirstOrDefault();
                    //XmlSerializer xs = new XmlSerializer(typeof(Syncfusion.RDL.DOM.ReportDefinition), Namespace);


                    // string xmlbody = rdl.ToString().Replace("<ConnectString>", $"<ConnectString>{conne}</ConnectString>");
                    //xmlbody = xmlbody.Replace("</ConnectString>", "");
                    //XElement outrdl = XElement.Load(XmlReader.Create(xmlbody));           

                    XmlDocument xdocu = new XmlDocument();
                    xdocu.LoadXml(rdl.ToString());
                    //  var s = XDocument.Parse(rdl.ToString());

                    XmlNodeList xnList = xdocu.SelectNodes("//*");

                    foreach (XmlNode node in xnList)
                    {
                        if (node.Name == "DataSources")
                        {
                            XmlNodeList xnListc = node.ChildNodes;
                            string namevalue = "";
                            foreach (var item in xnListc)
                            {
                                var id = (item as XmlElement);
                                namevalue=id.Attributes["Name"].Value;
                                
                            }

                            node.InnerXml = HttpUtility.HtmlDecode($" <DataSource Name=\"{namevalue}\">  "
                              + "<ConnectionProperties>"
                              + "<DataProvider>SQL</DataProvider>"
                              + $"<ConnectString>{conne}</ConnectString>  "
                              + "</ConnectionProperties>                                                                                "
                              + " </DataSource>"); ;
                        }
                    }
                    stream.Position = 0;
                    stream.Dispose();
                    xdocu.Save(file.Name);
                }
            }

                //xdocu.Load(rdl.ToString());

            //    XNode nodo =null;

            //foreach (XNode item in rdl.Nodes())
            //{

            //    if (item.ToString().Contains("ConnectString"))
            //    {


            //        var id = (item as XElement); //.Attribute("ConnectString").Value;

            //        //id = new XElement("DataSource", " <DataSource Name=\"DataSource1\">  "
            //        // + "<ConnectionProperties>"
            //        // + "<DataProvider>SQL</DataProvider>"
            //        // + $"<ConnectString>{conne}</ConnectString>  "
            //        // + "</ConnectionProperties> "
            //        // + " </DataSource>");

            //        var element = new XElement("DataSource",
            //                new XElement("ConnectionProperties", new XElement("DataProvider", "SQL"),
            //                 new XElement("ConnectString", $"{conne}")
            //                )

            //          );

            //        nodo = element;

            //        //nodo = (element as XNode);
            //       // id.Value = element.ToString();
            //        id.Value = HttpUtility.HtmlDecode(" <DataSource Name=\"DataSource1\">  "
            //     + "<ConnectionProperties>"
            //     + "<DataProvider>SQL</DataProvider>"
            //     + $"<ConnectString>{conne}</ConnectString>  "
            //     + "</ConnectionProperties>                                                                                "
            //     + " </DataSource>");

            //        id.Value =  id.Value.Replace("&lt;", "<")
            //                                       .Replace("&amp;", "&")
            //                                       .Replace("&gt;", ">")
            //                                       .Replace("&quot;", "\"")
            //                                       .Replace("&apos;", "'");

            //    }
            //}




            //rdl.Save("output.rdl");

            //var xl = rdl.Nodes().Where(q=>q.Name=="DataSources").Single();
            //xl.ReplaceWith(nodo);

            //var pinkFloyd = rdl.Elements("Report")
            //                .Where(x => x.Name == "DataSources")
            //                .Single();

            //var pinkFloyd = rdl.Nodes().Where(q=>q. .Elements("Report")
            //              .Where(x => x.Name == "DataSources")
            //              .Single();


            //pinkFloyd.ReplaceWith(nodo);







          


        }




    }
}
