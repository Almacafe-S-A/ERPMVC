using ERPMVC.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ERPMVC.DTO
{
    public class ConciliacionDTO : Conciliacion
    {
        public List<Conciliacion> _Conciliacion { get; set; }

        public int editar { get; set; } = 1;

        public string token { get; set; }

        public IEnumerable<IFormFile> files { get; set; }
    }
}
