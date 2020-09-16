using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class FixedAssetGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 FixedAssetGroupId { get; set; }

        [Display(Name = "Nombre del grupo de activos fijos")]
        public string FixedAssetGroupName { get; set; }

        [Display(Name = "Descripción del grupo de activos fijos")]
        public string FixedAssetGroupDescription { get; set; }

        [Display(Name = "Código de grupo")]
        public string FixedGroupCode { get; set; }

        [Display(Name = "Cuenta de Activo")]
        public Int64? FixedAssetAccountingId { get; set; }
        [ForeignKey("FixedAssetAccountingId")]
        public Accounting FixedAssetAccounting { get; set; }

        [Display(Name = "Cuenta de Depreciacionde Activo")]
        public Int64? DepreciationAccountingId { get; set; }
        [ForeignKey("DepreciationAccountingId")]
        public Accounting DepreciationFixedAssetAccounting { get; set; }


        public Int64? ResidualValueFixedAssetAccountingId { get; set; }
        [ForeignKey("ResidualValueFixedAssetAccountingId")]
        public Accounting ResidualValueFixedAssetAccounting { get; set; }

        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

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
