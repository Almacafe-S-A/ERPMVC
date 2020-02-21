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
        //public Periodo Periodo { get; set; }

        public decimal PresupuestoEnero { get; set; }
        public decimal PresupuestoFebrero { get; set; }
        public decimal PresupuestoMarzo { get; set; }
        public decimal PresupuestoAbril { get; set; }
        public decimal PresupuestoMayo { get; set; }
        public decimal PresupuestoJunio { get; set; }
        public decimal PresupuestoJulio { get; set; }
        public decimal PresupuestoAgosto { get; set; }
        public decimal PresupuestoSeptiembre { get; set; }
        public decimal PresupuestoOctubre { get; set; }
        public decimal PresupuestoNoviembre { get; set; }
        public decimal PresupuestoDiciembre { get; set; }


        public decimal EjecucionEnero { get; set; }
        public decimal EjecucionFebrero { get; set; }
        public decimal EjecucionMarzo { get; set; }
        public decimal EjecucionAbril { get; set; }
        public decimal EjecucionMayo { get; set; }
        public decimal EjecucionJunio { get; set; }
        public decimal EjecucionJulio { get; set; }
        public decimal EjecucionAgosto { get; set; }
        public decimal EjecucionSeptiembre { get; set; }
        public decimal EjecucionOctubre { get; set; }
        public decimal EjecucionNoviembre { get; set; }
        public decimal EjecucionDiciembre { get; set; }

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
    }
}
