using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class InventarioFisico
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int InventarioFisicoId { get; set; }

        [ForeignKey("InventarioFisicoId")]
        public InventarioFisico Inventario { get; set; }

        public DateTime Fecha { get; set; }

        public string Sucursal { get; set; }

        public int BranchId { get; set; }

        public int WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }

        public string Bodega { get; set; }

        public Int64? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public string Cliente { get; set; }

        public string Comentarios { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificion { get; set; }

        public string UsuarioCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public List<InventarioFisicoLine> InventarioFisicoLines { get; set; }
    }
}
