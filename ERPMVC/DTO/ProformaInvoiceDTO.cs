using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class ProformaInvoiceDTO : ProformaInvoice
    {
        public Kardex Kardex { get; set; } = new Kardex();

        public int editar { get; set; } = 1;

        //public Guid Identificador { get; set; }
    }


}
 