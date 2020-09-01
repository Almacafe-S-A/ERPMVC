using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class FixedAsset
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id Activo Fijo")]
        public Int64 FixedAssetId { get; set; }

        [Display(Name = "Nombre de activo fijo")]
        public string FixedAssetName { get; set; }

        [Display(Name = "Descripción de activos fijo")]
        public string FixedAssetDescription { get; set; }

        [Display(Name = "Fecha del activo")]
        public DateTime AssetDate { get; set; }

        [Display(Name = "Grupo del activo fijo")]
        public Int64 FixedAssetGroupId { get; set; }

        [Display(Name = "Código de grupo")]
        public string FixedAssetCode { get; set; }

        [Display(Name = "Sucursal")]
        public Int64 BranchId { get; set; }

        [Display(Name = "Sucursal")]
        public string BranchName { get; set; }

        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Bodega")]
        public Int64 WarehouseId { get; set; }

        [Display(Name = "Bodega")]
        public string WarehouseName { get; set; }

        [Display(Name = "Centro de costos")]
        public Int64 CenterCostId { get; set; }

        [Display(Name = "Centro de costos")]
        public string CenterCostName { get; set; }

        [Display(Name = "Vida útil")]
        public double FixedActiveLife { get; set; }

        [Display(Name = "Monto")]
        public double Amount { get; set; }

        [Display(Name = "Costo")]
        public double Cost { get; set; }

        [Display(Name = "Valor residual")]
        public double ResidualValue { get; set; }

        [Display(Name = "A depreciar")]
        public double ToDepreciate { get; set; }

        [Display(Name = "Total Depreciado")]
        public double TotalDepreciated { get; set; }

        [Display(Name = "Costo Total Activo")]
        public double ActiveTotalCost { get; set; }

        [Display(Name = "Valor residual porcentaje")]
        public double ResidualValuePercent { get; set; }

        [Display(Name = "Valor neto")]
        public double NetValue { get; set; }

        [Display(Name = "Depreciacion Acumulada")]
        public double AccumulatedDepreciation { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }
    }



}
