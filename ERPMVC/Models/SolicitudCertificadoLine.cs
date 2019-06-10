using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class SolicitudCertificadoLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 CertificadoLineId { get; set; }
        [Display(Name = "Certificado")]
        public Int64 IdCD { get; set; }
        [Display(Name = "Producto")]
        public Int64 SubProductId { get; set; }
        [Display(Name = "Nombre producto")]
        public string SubProductName { get; set; }

        [Display(Name = "Unidad de medida")]
        public Int64 UnitMeasureId { get; set; }
        [Display(Name = "Unidad de medida")]
        public string UnitMeasurName { get; set; }

        [Display(Name = "Descripcion")]
        public string Description { get; set; }
        [Display(Name = "Cantidad")]
        public double Quantity { get; set; }
        [Display(Name = "Precio")]
        public double Price { get; set; }    

        [Display(Name = "Total")]
        public double Amount { get; set; }

        [Display(Name = "Total Cantidad")]
        public double TotalCantidad { get; set; }
    }
}
