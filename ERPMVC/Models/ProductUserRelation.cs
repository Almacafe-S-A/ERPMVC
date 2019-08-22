using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ProductUserRelation
    {
        [Display(Name = "Id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ProductUserRelationId { get; set; }
        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }
        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }
        [Display(Name = "Producto")]
        public Int64 ProductId { get; set; }
        [Display(Name = "Producto")]
        public string ProductName { get; set; }

        [Display(Name = "Tipo de documento")]
        public string DocType { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }
        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }
        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }
        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }


    }
}
