using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Conciliacion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int ConciliacionId { get; set; }

        [ForeignKey("IdBanco")]
        public Bank Banco { get; set; }

        [Required]
        [Display(Name = "BankName")]
        public string BankName { get; set; }

        [Required]
        [Display(Name = "FechaConciliacion")]
        public DateTime FechaConciliacion { get; set; }

        [Required]
        [Display(Name = "SaldoConciliado")]
        public Double SaldoConciliado { get; set; }

        [Required]
        [Display(Name = "NombreArchivo")]
        public string NombreArchivo { get; set; }


        [Required]
        [Display(Name = "FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [Display(Name = "FechaModificacion")]
        public DateTime FechaModificacion { get; set; }

        [Required]
        [Display(Name = "UsuarioCreacion")]
        public string UsuarioCreacion { get; set; }

        [Required]
        [Display(Name = "UsuarioModificacion")]
        public string UsuarioModificacion { get; set; }

    }
}
