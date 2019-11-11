using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Insurances
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int InsurancesId { get; set; }
        [Required]
        [Display(Name = "Nombre Aseguradora")]
        public string InsurancesName { get; set; }

        [Display(Name = "Documento")]
        public Int64 DocumentTypeId { get; set; }

        [Display(Name = "Documento")]
        public string DocumentTypeName { get; set; }

        [Display(Name = "Nombre de documento")]
        public string DocumentName { get; set; }

        [Display(Name = "Ruta")]
        public string Path { get; set; }

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
