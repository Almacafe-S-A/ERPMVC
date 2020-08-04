using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class JournalEntryLineDTO: JournalEntryLine
    {
        public List<JournalEntryLine> _JournalEntryLine { get; set; }

        public int editar { get; set; } = 1;

        public string token { get; set; }
    }

    public class JournalEntryLineConciliacionDTO : JournalEntryLine
    {
        public DateTime FechaTransaccion { get; set; }
        public string TipoDocumento { get; set; }

        public int NumeroDocumento { get; set; }
    }
}
