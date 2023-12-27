using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace ERPMVC.Models
{
    public class PlanillaDeduccion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public int? DetallePlanillaAplicado { get; set; }

        public int? DetallePlanillaAplicar { get; set; }

        public int Periodo { get; set; }

        public int Mes { get; set; }

        public int QuincenaAplicar { get; set; }

        public long? DeduccionId { get; set; }
        [ForeignKey("DeduccionId")]
        public TipoDeduccion Deduction { get; set; }

        public string NombreDeduccion { get; set; }

        public long EmpleadoId { get; set; }

        [ForeignKey("EmpleadoId")]
        public Employees Employee { get; set; }

        public string EmpleadoNombre { get; set; }

        public decimal Cantidad { get; set; }

        public decimal Monto { get; set; }

        public decimal Valor { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public string UsuarioCreacion { get; set; }
    }
}
