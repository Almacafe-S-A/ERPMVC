using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class JournalEntryConfigurationLine
    {
        public Int64 JournalEntryConfigurationLineId { get; set; }

        [Display(Name = "Fecha de creación")]
        public string JournalEntryConfigurationId { get; set; }

        [Display(Name = "Cuenta Contable")]
        public Int64 AccountId { get; set; }

        [Display(Name = "Cuenta Contable")]
        public string AccountName { get; set; }

        [Display(Name = "Indicador Débito o Crédito")]
        public string DebitCredit { get; set; }      
      

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }
        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }
    }
}
