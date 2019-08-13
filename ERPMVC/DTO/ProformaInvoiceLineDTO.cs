using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class ProformaInvoiceLineDTO : ProformaInvoiceLine
    {
        public Int64 IdCD { get; set; }
    }
}
