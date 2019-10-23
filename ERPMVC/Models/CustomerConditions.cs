using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CustomerConditions
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 CustomerConditionId { get; set; }
        [Display(Name = "Condición")]
        public Int64 ConditionId { get; set; }
        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }
        [Display(Name = "Producto")]
        public Int64 ProductId { get; set; }
        [Display(Name = "Sub Servicio")]
        public Int64 SubProductId { get; set; }

        [Display(Name = "Documento")]
        public Int64 DocumentId { get; set; }

        public Int64 IdEstado { get; set; }
        public string Estado { get; set; }

        [Display(Name = "Tipo Documento")]
        public Int64 IdTipoDocumento { get; set; }

        [Display(Name = "Nombre condición")]
        public string CustomerConditionName { get; set; }

        [Display(Name = "Descripción de la condicion")]
        public string Description { get; set; }

        [Display(Name = "Condición logica")]
        public Int64 LogicalConditionId { get; set; }

        [Display(Name = "Condición logica")]
        public string LogicalCondition { get; set; }
        [Display(Name = "Valor a evaluar")]
        public string ValueToEvaluate { get; set; }
        [Display(Name = "% de comisión")]
        public double ValueDecimal { get; set; }
        [Display(Name = "Valor cadena")]
        public string ValueString { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }
}
