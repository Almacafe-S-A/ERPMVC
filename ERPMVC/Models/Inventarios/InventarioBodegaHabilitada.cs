using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class InventarioBodegaHabilitada
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? No { get; set; }

        public int InventarioFisicoId { get; set; }
        [ForeignKey("InventarioFisicoId")]
        public InventarioFisico inventarioFisico { get; set; }

        public Int64 ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public SubProduct Product { get; set; }

        public string Descripcion  { get; set; }


        public string ProductoNombre { get; set; }

        public decimal SaldoLibros { get; set; }

        public decimal Cantidad{ get; set; }

        public int? UnitOfMeasureId { get; set; }
        [ForeignKey("UnitOfMeasureId")]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public decimal Factor { get; set; }

        public decimal Valor { get; set; }

        public string Observacion { get; set; }
    }
}
