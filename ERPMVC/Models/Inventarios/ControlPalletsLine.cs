using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ControlPalletsLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id Linea")]
        public Int64 ControlPalletsLineId { get; set; }
        [Display(Name = "Id Control")]
        public Int64 ControlPalletsId { get; set; }
        public int Alto { get; set; }
        public int Ancho { get; set; }
        public int Otros { get; set; }


        public Int64? SubProductId { get; set; }

        public string SubProductName { get; set; }

        public int? UnitofMeasureId { get; set; }

        public string UnitofMeasureName { get; set; }

        public decimal? Qty { get; set; }


        public int? WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public string Observacion { get; set; }

        [Display(Name = "Total de Línea")]
        public double Totallinea { get; set; }

        [Display(Name = "Cantidad de Sacos Yute")]
        public int cantidadYute { get; set; }

        [Display(Name = "Cantidad de Sacos de Polietileno")]
        public int cantidadPoliEtileno { get; set; }
        [Display(Name = "Fecha de creación")]
        public DateTime? FechaCreacion { get; set; }
        [Display(Name = "Fecha de modificación")]
        public DateTime? FechaModificacion { get; set; }
        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }

    }
}
