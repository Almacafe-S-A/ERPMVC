using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class VendorInvoiceDTO : VendorInvoice
    {

        [Display(Name = "Número SAR")]
        public string NumeroDEIString { get; set; }
        public int editar { get; set; } = 1;
    }
}
