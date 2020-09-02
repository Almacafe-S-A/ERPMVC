using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CostListItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id de Lista de Costo")]
        public Int64 CostListItemId { get; set; }

        [Display(Name = "Id de Subproducto")]
        public Int64 SubproductId { get; set; }
        [Display(Name = "Id de Tasa de cambio")]
        public Int64 ExchangeRateId { get; set; }
        [Display(Name = "Dia de Calculo")]
        public DateTime DayofCalcule { get; set; }

        [Display(Name = "Precio de la Bolsa")]
        public double PriceBagValue { get; set; }
        [Display(Name = "Monto de Diferencia")]
        public double DifferencyPriceBagValue { get; set; }
        [Display(Name = "Monto de Diferencia")]
        public double TotalPriceBagValue { get; set; }
        [Display(Name = "Ingreso de Precio Lps")]
        public double PriceBagValueCurrency { get; set; }

        [Display(Name = "Porcentaje  de Precio Lps")]
        public double PorcentagePriceBagValue { get; set; }
        [Display(Name = "Precio Neto Lps")]
        public double RealBagValueCurrency { get; set; }
        [Display(Name = "Consumo Interno Neto Lps")]
        public double ConRealBagValueInside { get; set; }
        [Display(Name = "Porcentaje Consumo Interno Neto Lps")]
        public double PCRealBagValueInside { get; set; }

        [Display(Name = "Real Consumo Interno Neto Lps")]
        public double RealBagValueInside { get; set; }
        [Display(Name = "Total  Ingresos Neto Lps")]
        public double TotalIncomes { get; set; }
        [Display(Name = "Egresos Beneficiario ")]
        public double RecipientExpenses { get; set; }
        [Display(Name = "Egresos Fidelcomiso ")]
        public double EscrowExpenses { get; set; }
        [Display(Name = "Egresos Utilidad ")]
        public double UtilityExpenses { get; set; }
        [Display(Name = "Egresos Permiso de Exportacion ")]
        public double PermiseExportExpenses { get; set; }
        [Display(Name = "Egresos Inpuestos ")]
        public double TaxesExpenses { get; set; }
        [Display(Name = "Total de Egresos Dolares")]
        public double TotalExpenses { get; set; }
        [Display(Name = "Total de Egresos Lempiras")]
        public double TotalExpensesCurrency { get; set; }


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

    }
}
