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

        public decimal PrecioBolsaUSD { get; set; }

        public decimal DiferencialesUSD { get; set; }

        public decimal TotalUSD { get; set; }

        public Int64 ExchangeRateId { get; set; }
        [ForeignKey("ExchangeRateId")]
        public ExchangeRate ExchangeRate { get; set; }

        public decimal BrutoLPSIngreso { get; set; }

        public decimal PorcentajeIngreso { get; set; }

        public decimal NetoLPSIngreso { get; set; }


        public decimal BrutoLPSConsumoInterno { get; set; }

        public decimal PorcentajeConsumoInterno { get; set; }

        public decimal NetoLPSConsumoInterno { get; set; }

        public decimal TotalLPSIngreso { get; set; }

        public decimal BeneficiadoUSD { get; set; }

        public decimal FideicomisoUSD { get; set; }

        public decimal UtilidadUSD { get; set; }

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
