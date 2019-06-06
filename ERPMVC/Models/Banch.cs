using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
     public class Branch
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int BranchId { get; set; }
        [Required]
        [Display(Name = "Nombre Sucursal")]

        public string BranchName { get; set; }

        [Display(Name = "Descripcion")]
        public string Description { get; set; }
        [Display(Name = "Moneda")]
        public int CurrencyId { get; set; }
        [Display(Name = "Moneda")]
        public string CurrencyName { get; set; }
        [Display(Name = "Direccion")]
        public string Address { get; set; }
        [Display(Name = "Ciudad")]
        public string City { get; set; }
        [Display(Name = "Departamento")]
        public string State { get; set; }
        [Display(Name = "Code Zip ")]
        public string ZipCode { get; set; }
        [Display(Name = "Telefono ")]
        public string Phone { get; set; }
        [Display(Name = "Correo ")]
        public string Email { get; set; }

        [Display(Name = "Persona de contacto")]
        public string ContactPerson { get; set; }

        public Int64 IdEstado { get; set; }
        public string Estado { get; set; }

        [Required]
        public string UsuarioCreacion { get; set; }
        [Required]
        public string UsuarioModificacion { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
        [Required]
        public DateTime FechaModificacion { get; set; }
    }
}
