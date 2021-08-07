using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CustomerContractLines
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Int64 CustomerContractId { get; set; }
        [ForeignKey("CustomerContractId")]
        public CustomerContract CustomerContract { get; set; }
        public Int64 SubProductId { get; set; }
        [ForeignKey("SubProductId")]
        public SubProduct SubProduct { get; set; }
        public string SubProductName { get; set; }
        public Int64 UnitOfMeasureId { get; set; }
        [ForeignKey("UnitOfMeasureId")]
        public string UnitOfMeasureName { get; set; }

        [Display(Name = "Descripcion")]
        public string Description { get; set; }
        [Display(Name = "Cantidad")]
        public decimal Quantity { get; set; }
        [Display(Name = "Precio")]
        public decimal Price { get; set; }
        [Display(Name = "Monto")]

        public decimal? Valor { get; set; }

        public decimal? Porcentaje { get; set; }

        public Int64? TipoCobroId { get; set; }
        [ForeignKey("TipoCobroId")]
        public ElementoConfiguracion TipoCobro { get; set; }

        public string TipoCobroName { get; set; }

        public int PeriodoCobro { get; set; }

    }
}
