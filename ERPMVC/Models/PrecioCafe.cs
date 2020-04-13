using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class PrecioCafe
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "El Precio Bolsa USD es Requerido.")]
        public decimal PrecioBolsaUSD { get; set; }
        [Required(ErrorMessage = "La Diferencial USD es Requerido.")]
        public decimal DiferencialesUSD { get; set; }

        public decimal TotalUSD { get; set; }

        [UIHint("Tasadecambiodrop")]
        public Int64 ExchangeRateId { get; set; }
        [ForeignKey("ExchangeRateId")]
        public ExchangeRate ExchangeRate { get; set; }

        public decimal BrutoLPSIngreso { get; set; }
        [Required(ErrorMessage = "El Porcentaje Ingreso es Requerido.")]

        public decimal PorcentajeIngreso { get; set; }

        public decimal NetoLPSIngreso { get; set; }

        [Required(ErrorMessage = "El Bruto LPS Consumo Interno es Requerido.")]
        public decimal BrutoLPSConsumoInterno { get; set; }
        [Required(ErrorMessage = "El Porcentaje Consumo Interno es Requerido.")]
        public decimal PorcentajeConsumoInterno { get; set; }

        public decimal NetoLPSConsumoInterno { get; set; }

        public decimal TotalLPSIngreso { get; set; }
        [Required(ErrorMessage = "El Beneficiado USD es Requerido.")]

        public decimal BeneficiadoUSD { get; set; }
        [Required(ErrorMessage = "El Fideicomiso USD es Requerido.")]

        public decimal FideicomisoUSD { get; set; }
        [Required(ErrorMessage = "La Utilidad USD es Requerida.")]
        public decimal UtilidadUSD { get; set; }
        [Required(ErrorMessage = "El Permiso Exportación USD es Requerido.")]
        public decimal PermisoExportacionUSD { get; set; }

        public decimal TotalUSDEgreso { get; set; }

        public decimal TotalLPSEgreso { get; set; }

        public decimal PrecioQQOro { get; set; }

        public decimal PercioQQPergamino { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }


    }
}
