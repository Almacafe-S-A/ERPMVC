using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class CertificadoProducto
    {
        [Display(Name = "Certificado Deposito")]
        public Int64 IdCD { get; set; }

        [Display(Name = "Producto")]
        public Int64 ProductId { get; set; }


    }
}
