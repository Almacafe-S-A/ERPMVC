using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Warehouse
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage ="Campo requerido")]
        [Display(Name = "Nombre del almacén")]
        public string WarehouseName { get; set; }
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        public Int64 IdEstado { get; set; }
        public string Estado { get; set; }
        [Display(Name = "Sucursal")]
        public int BranchId { get; set; }

        [Display(Name = "Capacidad Bodega")]
        public double CapacidadBodega { get; set; }

        [Display(Name = "Unidad de medida")]
        public int UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de medida")]
        public string UnitOfMeasureName { get; set; }
        [Required]
        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }
        [Required]
        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }
        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }
        [Required]
        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }
    }
}
