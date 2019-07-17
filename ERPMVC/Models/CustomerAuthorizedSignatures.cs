using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CustomerAuthorizedSignature
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 CustomerAuthorizedSignatureId { get; set; }

        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }

        public string Nombre { get; set; }

        public string RTN { get; set; }

        [Display(Name = "Telefono")]
        public string Telefono { get; set; }

        [EmailAddress]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Correo invalido")]
        [DataType(DataType.EmailAddress)]        
        public string Correo { get; set; }

        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificacion")]
        public DateTime FechaModificacion { get; set; }



    }
}
