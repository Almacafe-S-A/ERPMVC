using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ERPMVC.Controllers
{
    public class DownloadController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Download([FromBody]Miarchivo _Miarchivo)
        {
            var path = _Miarchivo.tpath;
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return File(memory, GetMimeTypes()[ext], Path.GetFileName(path));
        }

        private Dictionary<string, string> GetMimeTypes()
        {
                 return new Dictionary<string, string>
             {
                 {".txt", "text/plain"},
                 {".pdf", "application/pdf"},
                 {".doc", "application/vnd.ms-word"},
                 {".docx", "application/vnd.ms-word"},
                 {".png", "image/png"},
                 {".jpg", "image/jpeg"},

             };
        }


        [HttpPost]
        public async Task<IActionResult> DownloadFile([FromBody]Miarchivo _Miarchivo)
        {
            var path = _Miarchivo.tpath;
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return File(memory, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(path));
        }


    }

    

    public class Miarchivo
    {
        public string tpath { get; set; }
    }


}