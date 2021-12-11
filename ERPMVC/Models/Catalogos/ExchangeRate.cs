using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ExchangeRate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 ExchangeRateId { get; set; }
        
        /////Campo para mostrar la fecha y el valor de compra no mapeado 
        ///
        [NotMapped]
        public string Descripcion { get; set; }
        [Display(Name = "Fecha")]
        public DateTime DayofRate { get; set; }
        [Display(Name = "Tasa de Venta")]
        public decimal ExchangeRateValue { get; set; }

        [Display(Name = "Tasa de Compra")]
        public decimal ExchangeRateValueCompra { get; set; }
        [Display(Name = "Id Moneda")]
        public Int64 CurrencyId { get; set; }

        [Display(Name = "Moneda")]
        public string CurrencyName { get; set; }

        [Required]
        [Display(Name = "Usuario que lo crea")]
        public string CreatedUser { get; set; }

        [Required]
        [Display(Name = "Usuario que lo modifica")]
        public string ModifiedUser { get; set; }

        [Required]
        [Display(Name = "Fecha de creacion")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Fecha de Modificacion")]
        public DateTime ModifiedDate { get; set; }

        public static List<ExchangeRate> PreprocesarTasasCambio(List<ExchangeRate> tasascambio)
        {
            tasascambio = (from tasa in tasascambio
                           select new ExchangeRate
                           {
                               ExchangeRateId = tasa.ExchangeRateId,
                               Descripcion = tasa.DayofRate.Date.ToString("dd/MM/yyyy") + " -> " + tasa.ExchangeRateValueCompra,
                               ExchangeRateValueCompra = tasa.ExchangeRateValueCompra,
                               CurrencyId = tasa.CurrencyId,
                               CurrencyName = tasa.CurrencyName,
                               ExchangeRateValue = tasa.ExchangeRateValue,
                               DayofRate = tasa.DayofRate

                           }
                            ).ToList();


            ;

            return tasascambio;

        }

    }


   
}
