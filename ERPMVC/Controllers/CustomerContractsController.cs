using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.DocIO;
using Syncfusion.DocIO.Utilities;
using Syncfusion.DocIO.DLS;
using Microsoft.AspNetCore.Hosting;

namespace ERPMVC.Controllers
{
    public class CustomerContractsController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        public CustomerContractsController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult ReplaceTest()
        {


            string basePath = _hostingEnvironment.WebRootPath;
            //FileStream fs = System.IO.File.OpenRead(basePath+ "/ContratosTemplate/Test.docx");
            FileStream fs = new FileStream(basePath + "/ContratosTemplate/Test.docx", FileMode.Open, FileAccess.Read);

            WordDocument document = new WordDocument(fs, FormatType.Docx);
            string[] fieldNames = new string[] { "EmployeeId", "Name", "Phone", "City" };
            string[] fieldValues = new string[] { "1001", "Freddy", "+122-97242717", "Tegucigalpa" };

            //Performs the mail merge.

            document.MailMerge.Execute(fieldNames, fieldValues);

            //Saves and closes the WordDocument instance
            // document.Save(document.)
            MemoryStream stream = new MemoryStream();

            
            document.Save(stream, FormatType.Docx);

            using (FileStream file = new FileStream(basePath+ "/ContratosTemplate/file.docx", FileMode.Create, System.IO.FileAccess.Write))
                stream.WriteTo(file);

            document.Close();

            return View();
        }


    }
}