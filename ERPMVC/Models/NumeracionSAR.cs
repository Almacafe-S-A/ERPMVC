using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class NumeracionSAR
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // [Key]
        [Display(Name = "Id")]
        public Int64 IdNumeracion { get; set; }
        [Display(Name = "Cai")]

        public Int64 IdCAI { get; set; }

        [Display(Name = "Número de inicio")]
        public string NoInicio { get; set; }

        [Display(Name = "Número de fin")]
        public string NoFin { get; set; }

        [Display(Name = "Fecha Limite")]

        public DateTime FechaLimite { get; set; }

        [Display(Name = "Cantidad otorgada")]
        public int CantidadOtorgada { get; set; }

        [Display(Name = "Siguiente número")]

        public string SiguienteNumero { get; set; }

        [Display(Name = "Punto de emisión")]
        public Int64 IdPuntoEmision { get; set; }
        [Display(Name = "Punto de emisión")]
        public string PuntoEmision { get; set; }
        [Display(Name = "Tipo de documento")]
        public Int64 DocTypeId { get; set; }
        [Display(Name = "Tipo de documento")]
        public string DocType { get; set; }
        [Display(Name = "Tipo de documento")]
        public Int64 DocSubTypeId { get; set; }
        public string DocSubType { get; set; }

        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }

        public string Estado { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }


    }
}
