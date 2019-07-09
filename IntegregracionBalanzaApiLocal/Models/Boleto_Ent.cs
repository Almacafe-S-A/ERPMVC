using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IntegregracionBalanzaApiLocal
{
    public class Boleto_Ent
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public Int64 Boleto_EntId { get; set; }
      
        public Int64 clave_e { get; set; }
      
        public string clave_C { get; set; }
       
        public string clave_o { get; set; }
       
        public string clave_p { get; set; }
       
        public bool completo { get; set; }
       
        public DateTime fecha_e { get; set; }
      
        public string hora_e { get; set; }
      
        public string placas { get; set; }
      
        public string conductor { get; set; }
      
        public Int32 peso_e { get; set; }
       
        public string observa_e { get; set; }
       
        public string nombre_oe { get; set; }
     
        public string turno_oe { get; set; }
        
        public string unidad_e { get; set; }
      
        public string bascula_e { get; set; }
       
        public int t_entrada { get; set; }
       
        public string clave_u { get; set; }

        public virtual Boleto_Sal Boleto_Sal { get; set; }
    }
}
