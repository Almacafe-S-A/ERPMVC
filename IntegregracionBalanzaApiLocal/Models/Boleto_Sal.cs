using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IntegregracionBalanzaApiLocal
{
    public class Boleto_Sal
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public Int64 Boleto_SalId { get; set; }
     
        public Int64 clave_e { get; set; }
      
        public string clave_o { get; set; }
    
        public DateTime fecha_s { get; set; }
        
        public string hora_s { get; set; }
      
        public double peso_s { get; set; }
      
        public double peso_n { get; set; }
       
        public string observa_s { get; set; }
      
        public string turno_s { get; set; }
      
        public string bascula_s{get;set;}
      
        public bool s_manual { get; set; }

        public virtual Boleto_Ent Boleto_Ent { get; set; }

    }
}
