using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class GarantiaBancaria
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Descripción")]
        public string strign { get; set; }
        [Display(Name = "Fecha Inicio Vigencia")]
        public DateTime FechaInicioVigencia { get; set; }
        [Display(Name = "Fecha Final Vigencia")]
        public DateTime FechaFianlVigencia { get; set; }
        [Display(Name = "Número Certificado")]
        public string NumeroCertificado { get; set; }
        [Required]
        [Display(Name = "Centro de Costo")]
        [ForeignKey("CostCenterId")]
        public CostCenter CostCenter { get; set; }
        [Display(Name = "Monto")]
        public double Monto { get; set; }
        [Display(Name = "Centro de Costos")]
        public Int64 CostCenterId { get; set; }
        [Display(Name = "Moneda")]
        public int CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }
        [Display(Name = "Ajuste")]
        public double Ajuste { get; set; }
        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }
        public Estados Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
