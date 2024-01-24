using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERPMVC.Models
{
    public class HorasExtrasDistribucion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public long HorasExtrasBiometricoId { get; set; }

        [ForeignKey("HorasExtrasBiometricoId")]

        //public HorasExtraBiometrico horasExtraBiometrico { get; set; }

        public decimal CantidadHoras { get; set; }

        public decimal FactorHora { get; set; }

        public decimal ValorHoraExtra { get; set; }

        public decimal TotalaAPagar { get; set; }

        public bool Pagado { get; set; }

        public long? PlanillaId { get; set; }
        [ForeignKey("PlanillaId")]
        public Planilla Planilla { get; set; }
    }
}
