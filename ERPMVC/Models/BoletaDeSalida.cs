using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class BoletaDeSalida
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Boleta de Salida")]
        public Int64 BoletaDeSalidaId { get; set; }

        [Display(Name = "C/E No.")]
        public Int64 GoodsDeliveredId { get; set; }

        [Display(Name = "A/R No.")]
        public Int64 GoodsDeliveryAuthorizationId { get; set; }

        [Display(Name = "Sr. Vigilante")]
        public string Vigilante { get; set; }

        [Display(Name = "Fecha y Hora")]
        public DateTime DocumentDate { get; set; }

        [Display(Name = "Sucursal")]
        public Int64 BranchId { get; set; }

        [Display(Name = "Sucursal")]
        public string BranchName { get; set; }

        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }
        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }

        public string Placa { get; set; }

        public string Marca { get; set; }

        public string Motorista { get; set; }

        [Display(Name = "Cargado/Vacio")]
        public Int64 CargadoId { get; set; }
        [Display(Name = "Cargado/Vacio")]
        public string Cargadoname { get; set; }

        [Display(Name = "Producto")]
        public Int64 SubProductId { get; set; }
        [Display(Name = "Producto")]
        public string SubProductName { get; set; }

        [Display(Name = "Unidad de medida")]
        public Int64 UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de medida")]
        public string UnitOfMeasureName { get; set; }

        [Display(Name = "Cantidad")]
        public double Quantity { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime? FechaCreacion { get; set; }
        [Display(Name = "Fecha de Modificación")]
        public DateTime? FechaModificacion { get; set; }
        [Display(Name = "Usuario de Creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Fecha de Creación")]
        public string UsuarioModificacion { get; set; }


    }
}
