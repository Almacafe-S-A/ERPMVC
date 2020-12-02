using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class RecibosCertificado
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 IdReciboCertificado { get; set; }
        [Display(Name = "Recibo")]
        public Int64 IdRecibo { get; set; }
        [Display(Name = "Certificado de deposito")]
        public Int64 IdCD { get; set; }

        [Display(Name = "Unidad de medida")]
        public Int64 UnitMeasureId { get; set; }

        [Display(Name = "Total lempiras")]
        public double productorecibolempiras { get; set; }
        [Display(Name = "Unidad")]
        public double productounidad { get; set; }

        [Display(Name = "Cantidad bultos")]
        public double productocantidadbultos { get; set; }    

      

    }
}
