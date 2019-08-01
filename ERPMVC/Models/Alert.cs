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
        [Display(Name = "Descripción")]
        public string DescriptionAlert { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
