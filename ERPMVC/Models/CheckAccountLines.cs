using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CheckAccountLines
    {
        [Key]
        public int Id { get; set; }

        public Int64 CheckAccountId { get; set; }
        [ForeignKey("CheckAccountId")]
        public CheckAccount CheckAccount { get; set; }

        public string CheckNumber { get; set; }

        public DateTime Date { get; set; }

        public string Place { get; set; }

        public string PaytoOrderOf { get; set; }

        public decimal Ammount { get; set; }

        public string AmountWords { get; set; }

        public string Address { get; set; }

        public Int64 IdEstado { get; set; }
        [ForeignKey("IdEstado")]
        public Estados Estados { get; set; }

        public string  Estado { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
