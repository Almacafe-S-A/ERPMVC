using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]  
    [Route("api/Tax")]
    [ApiController]
    public class Tax
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 TaxId { get; set; }
        [Display(Name = "Tipo de Impuesto")]
        public string TaxCode { get; set; }
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }
        [Display(Name = "Id Estado")]
        public Int64 IdEstado { get; set; }
        [Display(Name = "Porcentaje impuesto")]
        public double TaxPercentage { get; set; }
        public Int64? CuentaContableIngresosId { get; set; }
        public string CuentaContableIngresosNombre { get; set; }
        [ForeignKey("CuentaImpuestoporCobrarId")]
        public Accounting CuentaContableIngresos { get; set; }
        public Int64? CuentaContablePorCobrarId { get; set; }
        public string CuentaContablePorCobrarNombre { get; set; }
        [ForeignKey("CuentaImpuestoporPagarId")]
        public Accounting CuentaContablePorCobrar { get; set; }

        public Int64? CuentaImpuestoPagadoId { get; set; }
        public string CuentaImpuestoPagadoNombre { get; set; }
        [ForeignKey("CuentaImpuestoPagadoId")]
        public Accounting CuentaImpuestoPagado { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }
        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }


}
