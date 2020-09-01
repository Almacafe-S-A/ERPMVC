using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ERPMVC.Models
{
    public class Biometrico
    {
        
        public long Id { get; set; }
        
        public DateTime Fecha { get; set; }

        public string Observacion { get; set; }

        public long IdEstado { get; set; }

        public List<DetalleBiometrico> Detalle { get; set; }

        public Estados Estado { get; set; }
    }

    public class BiometricoPost
    {
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public IEnumerable<IFormFile> Archivo { get; set; }
    }
}
