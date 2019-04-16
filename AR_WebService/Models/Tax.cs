using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPAPI.Models
{
   

    public class Tax
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 TaxId { get; set; }
        public string TaxCode { get; set; }
        public string Description { get; set; }
        public double TaxPercentage { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }


}
