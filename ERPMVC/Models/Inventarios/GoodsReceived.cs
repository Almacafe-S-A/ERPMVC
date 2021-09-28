using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class GoodsReceived
    {
        [Display(Name = "Id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 GoodsReceivedId { get; set; }

        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }

        [ForeignKey("CustomerId")]

        public Customer Customer { get; set; }

        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }

        [Display(Name = "Control Id")]
        public Int64 ControlId { get; set; }

        [Display(Name = "País de origen")]
        public Int64 CountryId { get; set; }

        [Display(Name = "País de origen")]
        public string CountryName { get; set; }

        [Display(Name = "Fecha")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Fecha de documento")]
        public DateTime DocumentDate { get; set; }


        [Display(Name = "Sucursal")]
        public Int64 BranchId { get; set; }

        [Display(Name = "Sucursal")]
        public string BranchName { get; set; }

        [Display(Name = "Bodega")]
        public int? WarehouseId { get; set; }

        [Display(Name = "Bodega")]
        public string WarehouseName { get; set; }

        [Display(Name = "Producto Cliente")]
        public Int64? SubProductId { get; set; }

        [Display(Name = "Producto Cliente")]
        public string SubProductName { get; set; }

        [Display(Name = "Producto")]
        public Int64 ProductId { get; set; }

        [Display(Name = "Producto")]
        public string ProductName { get; set; }

        [Display(Name = "Vigilante")]
        public Int64 VigilanteId { get; set; }

        [Display(Name = "Vigilante")]
        public string VigilanteName { get; set; }

        [Display(Name = "Recibimos de")]
        public string Motorista { get; set; }

        [Display(Name = "Boleta de salida")]
        public Int64 BoletaSalidaId { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }

        [Display(Name = "Boleta de peso")]
        public Int64 WeightBallot { get; set; }

        [Display(Name = "Peso bruto")]
        public double? PesoBruto { get; set; }

        [Display(Name = "Tara de transporte")]
        public double? TaraTransporte { get; set; }

        [Display(Name = "Peso Neto")]
        public double? PesoNeto { get; set; }

        public int? CustomerUnitOfMeasureId { get; set; }
        [ForeignKey("CustomerUnitOfMeasureId")]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public string CustomerUnitOfMeasure { get; set; }

        [Display(Name = "Tara unidad de medida")]
        public double? TaraUnidadMedida { get; set; }

        [Display(Name = "Peso Neto 2")]
        public double? PesoNeto2 { get; set; }

        [Display(Name = "Comentarios")]
        public string Comments { get; set; }

        [Display(Name = "Tara de camión")]
        public double? TaraCamion { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime? FechaCreacion { get; set; }
        [Display(Name = "Fecha de modificación")]
        public DateTime? FechaModificacion { get; set; }

        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }


        //  public List<GoodsReceivedLine> _GoodsReceivedLine = new List<GoodsReceivedLine>();
        public List<GoodsReceivedLine> _GoodsReceivedLine { get; set; } = new List<GoodsReceivedLine>();

    }


    //public class GoodsReceivedDTO : GoodsReceived
    //{
    //    public Kardex Kardex { get; set; } = new Kardex();
    //}


}
