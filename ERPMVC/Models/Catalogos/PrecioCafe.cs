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
        public double PrecioBolsaUSD { get; set; }
        [Required(ErrorMessage = "La Diferencial USD es Requerido.")]
        //public double? DiferencialesUSD { get; set; }

        //public double? TotalUSD { get; set; }

        [UIHint("Tasadecambiodrop")]
        public Int64 ExchangeRateId { get; set; }
        [ForeignKey("ExchangeRateId")]
        public ExchangeRate ExchangeRate { get; set; }

        [NotMapped]
        public string Descripcion { get; set; }

        public double? ExchangeRateValue { get; set; }

        public string Cosecha { get; set; }
        [UIHint("Customer")]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public decimal BrutoLPSIngreso { get; set; }
        [Required(ErrorMessage = "El Porcentaje Ingreso es Requerido.")]

        public double? PorcentajeIngreso { get; set; }

        public string ImgPrecioCafe { get; set; }
        public string ImgName { get; set; }

        public decimal NetoLPSIngreso { get; set; }

        [Required(ErrorMessage = "El Bruto LPS Consumo Interno es Requerido.")]
        public decimal BrutoLPSConsumoInterno { get; set; }
        [Required(ErrorMessage = "El Porcentaje Consumo Interno es Requerido.")]
        public double? PorcentajeConsumoInterno { get; set; }

        public decimal NetoLPSConsumoInterno { get; set; }

        public decimal TotalLPSIngreso { get; set; }
        [Required(ErrorMessage = "El Beneficiado USD es Requerido.")]

        public double? BeneficiadoUSD { get; set; }
        [Required(ErrorMessage = "El Fideicomiso USD es Requerido.")]

        public double? FideicomisoUSD { get; set; }
        [Required(ErrorMessage = "La Utilidad USD es Requerida.")]
        public double? UtilidadUSD { get; set; }
        [Required(ErrorMessage = "El Permiso Exportación USD es Requerido.")]
        public double? PermisoExportacionUSD { get; set; }

        public decimal TotalUSDEgreso { get; set; }

        public decimal TotalLPSEgreso { get; set; }

        public decimal PrecioQQOro { get; set; }

        public decimal PercioQQPergamino { get; set; }

        public decimal PrecioQQCalidadesInferiores { get; set; }

        public decimal Otros { get; set; }

        public bool? UtilizadaCertificado { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }


    }
}
