using ERPMVC.DTO;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPAPI.Models
{
    public class DeductionQty
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey("DeductionId")]
        public DeduccionDTO Deduction { get; set; }
        public Int64 DeductionId { get; set; }
        [Display(Name = "Quincena a pagar")]
        public double Fortnight { get; set; }
        public double Porcentaje { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
    }
}
