using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class CertificadoProducto
    {
        [Display(Name = "Certificado Depósito")]
        public Int64 IdCD { get; set; }

        [Display(Name = "Producto")]
        public Int64 ProductId { get; set; }

        [Display(Name = "Id")]
        public Int64 Id { get; set; }


    }
}
