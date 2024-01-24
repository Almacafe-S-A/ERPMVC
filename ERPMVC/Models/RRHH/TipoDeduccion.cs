using Syncfusion.JavaScript.DataVisualization.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class TipoDeduccion
    {
        [Display(Name = "Id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Codigo Tipo Deduccion")]
        public Int64 DeductionTypeId { get; set; }

        [Display(Name = "Tipo Deduccion")]
        public string DeductionType { get; set; }

        [Display(Name = "Quincena a pagar")]
        public double Fortnight { get; set; }

        public long IdEstado { get; set; }

        [ForeignKey("IdEstado")]
        public Estados Estado { get; set; }
        public string NombreEstado { get; set; }
        //public bool ? EsPorcentaje { get; set; }
        public string Cantidad { get; set; }


        public string CuentaContableIngresosNombre { get; set; }
        public Int64? CuentaContableIngresosId { get; set; }
        [ForeignKey("AccountId")]
        public Accounting CuentaContableIngresos { get; set; }

        public string CuentaContablePorCobrarNombre { get; set; }
        public Int64? CuentaContableIdPorCobrar { get; set; }
        [ForeignKey("AccountIdPorCobrar")]
        public Accounting CuentaContablePorCobrar { get; set; }


        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public List<DeductionQty> DeductionQties { get; set; }
    }
}
