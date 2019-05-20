using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class SubProduct
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 SubproductId { get; set; }
        public string ProductName { get; set; }
        public Int64 ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }

        public string ProductCode { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }      
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }


}
