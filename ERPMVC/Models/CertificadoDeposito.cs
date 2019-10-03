using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CertificadoDeposito
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 IdCD { get; set; }
        [Display(Name = "No de certificado")]
        public Int64 NoCD { get; set; }
        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }

        [Display(Name = "Cliente")]
        public string CustomerName { get; set; }

        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }
        public string Estado { get; set; }

        [Display(Name = "Sucursal")]
        public Int64 BranchId { get; set; }
        [Display(Name = "Sucursal")]
        public string BranchName { get; set; }

        [Display(Name = "Bodega")]
        public Int64 WarehouseId { get; set; }
        [Display(Name = "Bodega")]
        public string WarehouseName { get; set; }
        [Display(Name = "Servicio")]
        public Int64 ServicioId { get; set; }

        [Display(Name = "Servicio")]
        public string ServicioName { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        [Display(Name = "Fecha de certificado")]
        public DateTime FechaCertificado { get; set; }

        [Display(Name = "Empresa")]
        public string NombreEmpresa { get; set; }

        [Display(Name = "Empresa Seguro")]
        public string EmpresaSeguro { get; set; }

        [Display(Name = "No. Poliza")]
        public string NoPoliza { get; set; }
        [Display(Name = "Sujetos a pago")]
        public double SujetasAPago { get; set; }
        [Display(Name = "Fecha de vencimiento")]
        public DateTime FechaVencimientoDeposito { get; set; }

        [Display(Name = "Número de traslado")]
        public Int64 NoTraslado { get; set; }

        [Display(Name = "Fecha de vencimiento")]
        public DateTime FechaVencimiento { get; set; }

        [Display(Name = "Aduana")]
        public string Aduana { get; set; }

        [Display(Name = "Carta de porte o manifiesto No.")]
        public string ManifiestoNo { get; set; }

        [Display(Name = "Almacenaje")]
        public string Almacenaje { get; set; }
        [Display(Name = "Seguro")]
        public string Seguro { get; set; }

        [Display(Name = "Otros Cargos")]
        public string OtrosCargos { get; set; }

        [Display(Name = "Banco")]
        public Int64 BankId { get; set; }
        [Display(Name = "Banco")]
        public string BankName { get; set; }
        [Display(Name = "Moneda")]
        public Int64 CurrencyId { get; set; }
        [Display(Name = "Moneda")]
        public string CurrencyName { get; set; }
        [Display(Name = "Monto de garantia")]
        public double MontoGarantia { get; set; }
        [Display(Name = "Fecha pago")]
        public DateTime FechaPagoBanco { get; set; }

        [Display(Name = "Porcentaje intereses")]
        public double PorcentajeInteresesInsolutos { get; set; }

        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicioComputo { get; set; }

        [Display(Name = "Lugar de firma")]
        public string LugarFirma { get; set; }

        [Display(Name = "Fecha de firma")]
        public DateTime FechaFirma { get; set; }

        [Display(Name = "Nombre prestatario")]
        public string NombrePrestatario { get; set; }
        /// <summary>
        /// Totales de Detalle de Linea
        /// </summary>
        [Display(Name = "Suma Cantidad")]
        public double Quantitysum { get; set; }

        [Display(Name = "Total")]
        public double Total { get; set; }

        /// <summary>
        /// Totales de Detalle de Linea
        /// </summary>
        public string Impreso { get; set; }
        public List<CertificadoLine> _CertificadoLine { get; set; } = new List<CertificadoLine>();
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }
}
