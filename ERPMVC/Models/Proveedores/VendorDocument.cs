using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class VendorDocument
    {
        [Display(Name = "Id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 VendorDocumentId { get; set; }

        [Display(Name = "Proveedor")]
        public Int64 VendorId { get; set; }

        [Display(Name = "Documento")]
        public Int64 DocumentTypeId { get; set; }

        [Display(Name = "Documento")]
        public string DocumentTypeName { get; set; }

        [Display(Name = "Nombre de documento")]
        public string DocumentName { get; set; }

        [Display(Name = "Ruta")]
        public string Path { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Fecha de Modificación")]
        public DateTime? ModifiedDate { get; set; }
        [Display(Name = "Usuario de Creación")]
        public string CreatedUser { get; set; }

        [Display(Name = "Usuario de Modificacion")]
        public string ModifiedUser { get; set; }

    }
}
