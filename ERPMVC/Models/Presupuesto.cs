using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Presupuesto
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Int64 CostCenterId { get; set; }
        [ForeignKey("CostCenterId")]
        public CostCenter CostCenter { get; set; }
        [Display(Name = "Año")]
        public int PeriodoId { get; set; }
        [ForeignKey("PeriodoId")]
        public Periodo Periodo { get; set; }
        [UIHint("Presupuesto")]
        public decimal PresupuestoEnero { get; set; }
        [UIHint("Presupuesto")]
        public decimal PresupuestoFebrero { get; set; }
        [UIHint("Presupuesto")]
        public decimal PresupuestoMarzo { get; set; }
        [UIHint("Presupuesto")]
        public decimal PresupuestoAbril { get; set; }
        [UIHint("Presupuesto")]
        public decimal PresupuestoMayo { get; set; }
        [UIHint("Presupuesto")]
        public decimal PresupuestoJunio { get; set; }
        [UIHint("Presupuesto")]
        public decimal PresupuestoJulio { get; set; }
        [UIHint("Presupuesto")]
        public decimal PresupuestoAgosto { get; set; }
        [UIHint("Presupuesto")]
        public decimal PresupuestoSeptiembre { get; set; }
        [UIHint("Presupuesto")]
        public decimal PresupuestoOctubre { get; set; }
        [UIHint("Presupuesto")]
        public decimal PresupuestoNoviembre { get; set; }
        [UIHint("Presupuesto")]
        public decimal PresupuestoDiciembre { get; set; }
        [UIHint("Presupuesto")]
        public decimal EjecucionEnero { get; set; }
        [UIHint("Presupuesto")]
        public decimal EjecucionFebrero { get; set; }
        [UIHint("Presupuesto")]
        public decimal EjecucionMarzo { get; set; }
        [UIHint("Presupuesto")]
        public decimal EjecucionAbril { get; set; }
        [UIHint("Presupuesto")]
        public decimal EjecucionMayo { get; set; }
        [UIHint("Presupuesto")]
        public decimal EjecucionJunio { get; set; }
        [UIHint("Presupuesto")]
        public decimal EjecucionJulio { get; set; }
        [UIHint("Presupuesto")]
        public decimal EjecucionAgosto { get; set; }
        [UIHint("Presupuesto")]
        public decimal EjecucionSeptiembre { get; set; }
        [UIHint("Presupuesto")]
        public decimal EjecucionOctubre { get; set; }
        [UIHint("Presupuesto")]
        public decimal EjecucionNoviembre { get; set; }
        [UIHint("Presupuesto")]
        public decimal EjecucionDiciembre { get; set; }
        [UIHint("Cuentas")]
        public Int64 AccountigId { get; set; }
        [ForeignKey("AccountigId")]
        public Accounting Accounting { get; set; }

        public string AccountName { get; set; }

        public decimal TotalMontoPresupuesto { get; set; }

        public decimal TotalMontoEjecucion { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

        //[UIHint("Cuentas")]
        //public virtual Accounting Accounting { get; set; }
    }
}
