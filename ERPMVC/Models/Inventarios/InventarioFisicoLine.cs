using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class InventarioFisicoLine
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int InventarioFisicoId { get; set; }
        [ForeignKey("InventarioFisicoId")]
        public InventarioFisico inventarioFisico { get; set; }

        public Int64 ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public SubProduct Product { get; set; }

        public string ProductoNombre { get; set; }

        public decimal SaldoLibros { get; set; }

        public decimal InventarioFisico { get; set; }

        public decimal Diferencia { get; set; }

        public string Observacion { get; set; }
    }
}
