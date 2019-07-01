using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class EndososCertificadosLine
    {
        [Display(Name = "Id Linea")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 EndososCertificadosLineId { get; set; }

        [Display(Name = "Id Endoso")]
        public Int64 EndososCertificadosId { get; set; }

        [Display(Name = "Unidad de medida")]
        public Int64 UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de medida")]
        public string UnitOfMeasureName { get; set; }


        [Display(Name = "Producto")]
        public Int64 SubProductId { get; set; }

        [Display(Name = "Producto")]
        public string SubProductName { get; set; }

        [Display(Name = "Precio")]
        public double Price { get; set; }

        [Display(Name = "Valor endoso")]
        public double ValorEndoso { get; set; }


    }
}
