using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CustomerContractTerms
    {
        public int Id { get; set; }

        public int Position { get; set; }

        public string TermTitle { get; set; }

        public string Term { get; set; }


        public Int64 TypeInvoiceId { get; set; }
        [ForeignKey("TypeInvoiceId")]

        public string TypeInvoiceName { get; set; }


        public int? CustomerContractType { get; set; }

        public string CustomerContractTypeName { get; set; }

        public Int64 ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public string Servicio { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }


    }
}
