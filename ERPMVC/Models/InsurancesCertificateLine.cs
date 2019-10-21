using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class InsurancesCertificateLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id de Seguro Certificado Line")]
        public Int64 InsurancesCertificateLineId { get; set; }

        [Display(Name = "Id de Seguro Certificado")]
        public int InsurancesCertificateId { get; set; }

        [Display(Name = "Total Insurance")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalInsurancesLine { get; set; }

        [Display(Name = "Total Deducible")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotaldeductibleLine { get; set; }

        [Display(Name = "Total Mercaderia")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalofProductLine { get; set; }
        [Display(Name = "Total Insurance Mercaderia")]
        [Column(TypeName = "decimal(18,4)")]

        public decimal TotalInsurancesofProductLine { get; set; }
        [Display(Name = "Diferencia Asegurada")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal DifferenceTotalofProductInsuranceLine { get; set; }


        [Display(Name = "Total Deducible Mercaderia")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotaldeductibleofProduct { get; set; }
        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }

        [Display(Name = "Total Insurance en letras")]
        public string TotalLetras { get; set; }

        [Display(Name = "Id de Sucursal")]
        public int BranchId { get; set; }
        [Display(Name = "Id de Almacén")]
        public int WarehouseId { get; set; }

        [Required]
        [Display(Name = "Nombre del almacén")]
        public string WarehouseName { get; set; }

        [Display(Name = "Grupo económico")]
        public Int64? GrupoEconomicoId { get; set; }

        [Display(Name = "Direccion")]
        public string Address { get; set; }

        [Display(Name = "Lugar de firma")]
        public string LugarFirma { get; set; }



        [Display(Name = "Fecha de firma")]
        public DateTime FechaFirma { get; set; }
        [Required]
        [Display(Name = "Usuario que lo crea")]
        public string CreatedUser { get; set; }

        [Required]
        [Display(Name = "Usuario que lo modifica")]
        public string ModifiedUser { get; set; }

        [Required]
        [Display(Name = "Fecha de creacion")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Fecha de Modificacion")]
        public DateTime ModifiedDate { get; set; }






    }
}
