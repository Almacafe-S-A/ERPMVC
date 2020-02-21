using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class EndososDTO : EndososCertificados
    {
        public List<EndososCertificados> _EndososCertificados { get; set; }
        public List<Int64> TipoEndosoIdList { get; set; }
        public Int64 editar { get; set; } = 1;
        public string token { get; set; }
    }
}
