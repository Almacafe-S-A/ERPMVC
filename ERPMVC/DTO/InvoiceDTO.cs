using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class InvoiceDTO : Invoice
    {

        public int editar { get; set; } = 1;
    }
}
