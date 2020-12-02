using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class BlackListCustomers
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 BlackListId { get; set; }
        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }
        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }
        
        [Display(Name = "Cliente referencia")]
        public string CustomerReference { get; set; }
        [Display(Name = "Fecha")]
        public DateTime DocumentDate { get; set; }
        [Display(Name = "Alias")]
        public string Alias { get; set; }
        [Display(Name = "Identidad")]
        public string Identidad { get; set; }
        [Display(Name = "RTN")]
        public string RTN { get; set;  }
        [Display(Name = "Referencia")]
        public string Referencia { get; set; }
        [Display(Name = "Origen")]
        public string Origen { get; set; }

        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "País")]
        public Int64 CountryId { get; set; }

        [Display(Name = "País")]
        public string CountryName { get; set; }

        [Display(Name = "Departamento")]
        public Int64 StateId { get; set; }

        [Display(Name = "Departamento")]
        public string StateName { get; set; }

        [Display(Name = "Municipio")]
        public Int64 CityId { get; set; }

        [Display(Name = "Municipio")]
        public string CityName { get; set; }

        [Display(Name = "Observación")]
        public string Comments { get; set; }


        [Display(Name = "Fecha creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha modificación")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario modificación")]
        public string UsuarioModificacion { get; set; }

    }
}
