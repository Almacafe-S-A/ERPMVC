using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class TipoContratoDTO : TipoContrato
    {
        public List<TipoContrato> _TipoContrato { get; set; }

        public int editar { get; set; } = 1;

        public string token { get; set; }
    }
}
