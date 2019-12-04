using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Alert
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 AlertId { get; set; }
        [Display(Name = "Número de documento")]
        public Int64 DocumentId { get; set; }
        [Display(Name = "Nombre de documento")]
        public string DocumentName { get; set; }
        [Display(Name = "Nombre de alerta")]
        public string AlertName { get; set; }
        [Display(Name = "Código de alerta")]
        public string Code { get; set; } //1.	PERSON001 , 2.	PERSON002 , 1.	PERSON003 // Por producto 1.	PROD001 , 2.	PROD002,3.	PROD003 , 4.	PROD004

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Id Acción tomada")]
        public Int64 ActionTakenId { get; set; }

        [Display(Name = "Acción tomada")]
        public string ActionTakenName { get; set; }

        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Sujeta a pago")]
        public bool SujetaARos { get; set; }

        [Display(Name = "Falso positivo")]
        public bool FalsoPositivo { get; set; }

        [Display(Name = "Fecha de cierre")]
        public DateTime CloseDate { get; set; }

        [Display(Name = "Descripción de la alerta")]
        public string DescriptionAlert { get; set; }
        [Display(Name = "Tipo(s) de alerta")]
        public string Type { get; set; } ///OFAC , ONU ,PEPS si existe en uno o varios
        public string AlertType { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }


    }
}
