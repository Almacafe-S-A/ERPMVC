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

        public int DocumentoId { get; set; }

        public string DocumentoTipo { get; set; }


        public int? GuiaRemisionId { get; set; }

        [Display(Name = "Sr. Vigilante")]
        public Int64 VigilanteId { get; set; }

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
        public string DNIMotorista { get; set; }

        public string PlacaContenedor { get; set; }

        [Display(Name = "Cargado/Vacio")]
        public Int64 CargadoId { get; set; }
        [Display(Name = "Cargado/Vacio")]
        public string Cargadoname { get; set; }

        [Display(Name = "Producto")]
        public Int64? SubProductId { get; set; }
        [Display(Name = "Producto")]
        public string SubProductName { get; set; }

        [Display(Name = "Unidad de medida")]
        public Int64? UnitOfMeasureId { get; set; }

        [Display(Name = "Unidad de medida")]
        public string UnitOfMeasureName { get; set; }

        [Display(Name = "Boleta de peso")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es requerido")]
        public Int64? WeightBallot { get; set; }

        public Int64? Producto { get; set; }
        [ForeignKey("Producto")]
        public Product Product { get; set; }

        public string ProductName { get; set; }


        [Display(Name = "Cantidad")]
        public double Quantity { get; set; }

        public string GuiRemisionNo { get; set; }

        public string DireccionDestion { get; set; }

        public string Transportista { get; set; }
        
        public string RTNTransportista { get; set; }

        public string Observaciones { get; set; }



        public string Barco { get; set; }

        public string OrdenNo { get; set; }

        public decimal TonPuerto { get; set; }

        public decimal QQPuerto { get; set; }

        public decimal PesoBruto { get; set; }

        public decimal Tara { get; set; }

        public decimal PNInglesas { get; set; }

        public decimal QQInglesas { get; set; }


        [Display(Name = "Fecha de Creación")]
        public DateTime? FechaCreacion { get; set; }
        [Display(Name = "Fecha de Modificación")]
        public DateTime? FechaModificacion { get; set; }
        [Display(Name = "Usuario de Creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Fecha de Creación")]
        public string UsuarioModificacion { get; set; }


        public List<BoletaDeSalidaLine> BoletaDeSalidaLines { get; set; }


    }
}
