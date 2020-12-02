using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class EmployeeDocument
    {

        [Display(Name = "Id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 EmployeeDocumentId { get; set; }

        [Display(Name = "Empleado")]
        public Int64 EmployeeId { get; set; }

        [Display(Name = "Documento")]
        public Int64 DocumentTypeId { get; set; }

        [Display(Name = "Documento")]
        public string DocumentTypeName { get; set; }

        [Display(Name = "Nombre de documento")]
        public string DocumentName { get; set; }

        [Display(Name = "Ruta")]
        public string Path { get; set; }

        [Display(Name = "Fecha de ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Display(Name = "Fecha de vencimiento")]
        public DateTime? FechaVencimiento { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime? FechaCreacion { get; set; }
        [Display(Name = "Fecha de Modificación")]
        public DateTime? FechaModificacion { get; set; }
        [Display(Name = "Usuario de Creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Fecha de Creación")]
        public string UsuarioModificacion { get; set; }



    }

}
