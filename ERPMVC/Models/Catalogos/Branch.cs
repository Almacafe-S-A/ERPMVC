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
       // [Required(ErrorMessage ="Valor requerido")]
        [Display(Name = "Nombre Sucursal")]

        public string BranchName { get; set; }

        [Display(Name = "Código de Sucursal")]
        public string BranchCode { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Display(Name = "Moneda")]
        public int CurrencyId { get; set; }
        [Display(Name = "Moneda")]
        public string CurrencyName { get; set; }
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Cliente")]
        public Int64? CustomerId { get; set; }

        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }

        [Display(Name = "Ciudad")]
        public int CityId { get; set; }

        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Display(Name = "País")]
        public int CountryId { get; set; }

        [Display(Name = "País")]
        public string CountryName { get; set; }
        [Display(Name = "Límite de CNBS")]
        public decimal? LimitCNBS { get; set; }
        [Display(Name = "Departamento")]
        public int StateId { get; set; }
        public string State { get; set; }
        [Display(Name = "Code Zip ")]
        public string ZipCode { get; set; }
        [Display(Name = "Teléfono ")]
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
