using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Boleto_Sal
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public Int64 Boleto_SalId { get; set; }
        [Display(Name = "Clave de la entrada(Boleta correspondiente)")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 clave_e { get; set; }
        [Display(Name = "Fecha de captura de la salida")]
        public DateTime fecha_s { get; set; }
        [Display(Name = "Hora de la captura de la salida")]
        public string hora_s { get; set; }
        [Display(Name = "Peso de salida")]
        public double peso_s { get; set; }
        [Display(Name = "Peso neto")]
        public double peso_n { get; set; }
        [Display(Name = "Turno")]
        public string turno_s { get; set; }
        [Display(Name = "Báscula de salida")]
        public string bascula_s{get;set;}
        [Display(Name = "Peso de salida normal")]
        public bool s_manual { get; set; }

        public virtual Boleto_Ent Boleto_Ent { get; set; }

    }
}
