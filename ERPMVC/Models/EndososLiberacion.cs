using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class EndososLiberacion
    {
        [Display(Name = "Id")]
        public Int64 EndososLiberacionId { get; set; }

        [Display(Name = "Endoso Id")]
        public Int64 EndososId { get; set; }

        [Display(Name = "Linea Endoso Id")]
        public Int64 EndososLineId { get; set; }

        [Display(Name = "Tipo de Endoso")]
        public string TipoEndoso { get; set; }
        [Display(Name = "Producto")]
        public Int64 SubProductId { get; set; }

        [Display(Name = "Producto")]
        public string SubProductName { get; set; }

        [Display(Name = "Unidad de medida")]
        public Int64 UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de medida")]
        public string UnitOfMeasureName { get; set; }
        [Display(Name = "Fecha de liberación")]
        public DateTime FechaLiberacion { get; set; }

        [Display(Name = "Cantidad")]
        public double Quantity { get; set; }

        [Display(Name = "Saldo")]
        public double Saldo { get; set; }

        public string Impreso { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
