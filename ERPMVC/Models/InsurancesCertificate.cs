using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class InsurancesCertificate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id de Certificado")]
        public int InsurancesCertificateId { get; set; }
        [Display(Name = "Id de Aseguradora")]
        public int InsurancesId { get; set; }

        [Display(Name = "Fecha de Inicio Poliza")]
        public DateTime BeginDateofInsurance { get; set; }
        [Display(Name = "Fecha de Fin de Poliza")]
        public DateTime EndDateofInsurance { get; set; }

        [Display(Name = "Fecha de Actual")]
        public DateTime DateofInsurance { get; set; }
        [Display(Name = "No. Poliza")]
        public string NoPoliza { get; set; }
        [Display(Name = "Tipo de Poliza")]
        public Int64 IdCertificated { get; set; }
        [Display(Name = "Tasa deducible")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Ratedeductible { get; set; }
        [Display(Name = "Tasa de Aseguradora")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal RateInsurance { get; set; }
        [Display(Name = "Tasa de Mercadería")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal RateofProduct { get; set; }
        [Display(Name = "Tasa de Mercadería")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal QtyofMonths { get; set; }



        [Display(Name = "Total Insurance")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalInsurances { get; set; }

        [Display(Name = "Total Deducible")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Totaldeductible { get; set; }

        [Display(Name = "Total Mercaderia")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalofProduct { get; set; }
        [Display(Name = "Total Insurance Mercaderia")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalInsurancesofProduct { get; set; }
        [Display(Name = "Diferencia Asegurada")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal DifferenceTotalofProductInsurance { get; set; }


        [Display(Name = "Total Deducible Mercaderia")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotaldeductibleofProduct { get; set; }
        [UIHint("Insurances")]
        public virtual Insurances Insurances { get; set; }


        /*[Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }

        [Display(Name = "Total Insurance en letras")]
        public string TotalLetras { get; set; }
        
        [Display(Name = "Id")]
        public int BranchId { get; set; }

        [Display(Name = "Grupo económico")]
        public Int64? GrupoEconomicoId { get; set; }
        
        [Display(Name = "Direccion")]
        public string Address { get; set; }

         [Display(Name = "Lugar de firma")]
        public string LugarFirma { get; set; }
        


        [Display(Name = "Fecha de firma")]
        public DateTime FechaFirma { get; set; }
        */
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
