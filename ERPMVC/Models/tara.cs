using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Tara
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Identificación del camión")]
        public string t_placas { get; set; }

        [Display(Name = "Peso tara del camión")]
        public double t_peso { get; set; }

        [Display(Name = "Unidad del peso de la tara")]
        public string t_unidad { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime t_fecha { get; set; }

        [Display(Name = "Operador que capturo")]
        public string t_captura { get; set; }

        [Display(Name = "Observaciones")]
        public string t_observaciones { get; set; }
    }


}
