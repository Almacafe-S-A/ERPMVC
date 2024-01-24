using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class TipoBonificacion
    {
        public long Id { get; set; }

        public string Nombre { get; set; }

        public bool EnPlanilla { get; set; }
        public double Valor { get; set; }

        public long EstadoId { get; set; }


        public Estados Estado { get; set; }

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
    }
}
