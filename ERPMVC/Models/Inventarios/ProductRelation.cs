using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ProductRelation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 RelationProductId { get; set; }

        [Display(Name = "Servicio")]
        public Int64 ProductId { get; set; }
        [Display(Name = "Subservicio")]
        public Int64 SubProductId { get; set; }

        public string CuentaContableIngresosNombre { get; set; }
        public Int64? CuentaContableIngresosId { get; set; }
        [ForeignKey("AccountId")]
        public Accounting CuentaContableIngresos { get; set; }

        public string CuentaContablePorCobrarNombre { get; set; }
        public Int64? CuentaContableIdPorCobrar { get; set; }
        [ForeignKey("AccountIdPorCobrar")]
        public Accounting CuentaContablePorCobrar { get; set; }

        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }

        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public Product Product { get; set; }
        public SubProduct SubProduct { get; set; }

    }



}
