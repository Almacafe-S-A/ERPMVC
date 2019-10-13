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
        [Display(Name = "Id")]
        public Int64 SubproductId { get; set; }
        [Display(Name = "SubServicio")]
        public string ProductName { get; set; }
        [Display(Name = "Tipo de Subservicio")]
        public Int64 ProductTypeId { get; set; }

        [Display(Name = "Tipo de producto")]
        public string ProductTypeName { get; set; }

        [Display(Name = "Tipo de prohibición")]
        public Int64 TipoProhibidoId { get; set; }

        [Display(Name = "Tipo prohibición")]
        public string TipoProhibidoName { get; set; }

     
        [Display(Name = "Saldo Quintales")]
        public double Balance { get; set; }
        [Display(Name = "Saldo Sacos")]
        public Int64 BagBalance { get; set; }
        [Display(Name = "Código de producto")]
        public string ProductCode { get; set; }
        [Display(Name = "Código de barra")]
        public string Barcode { get; set; }
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Display(Name = "Unidad de medida")]
        public int? UnitOfMeasureId { get; set; }
        [Display(Name = "Unidad de medida")]
        public string UnitOfMeasureName { get; set; }
        [Display(Name = "Merma")]
        public double Merma { get; set; }
        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }
        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }


}
