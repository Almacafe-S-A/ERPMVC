using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class JournalEntryConfiguration
    {
         [Display(Name = "Id")]
        public Int64 JournalEntryConfigurationId { get; set; }

        [Display(Name = "Transacción/Tipo Documento")]
        public Int64 TransactionId { get; set; }
        [Display(Name = "Transacción/Tipo Documento")]
        public string Transaction { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Servicio")]
        public Int64 ProductId { get; set; }

        [Display(Name = "Servicio")]
        public string ProductName { get; set; }

        [Display(Name = "Estado")]
        public Int64 EstadoId { get; set; }

        [Display(Name = "Estado")]
        public string EstadoName { get; set; }

        [Display(Name = "Sucursal")]
        public Int64 BranchId { get; set; }

        [Display(Name = "Sucursal")]
        public string BranchName { get; set; }


        [Display(Name = "Moneda")]
        public Int64 CurrencyId { get; set; }

        [Display(Name = "Moneda")]
        public string CurrencyName { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }
        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }      


       public List<JournalEntryConfigurationLine> JournalEntryConfigurationLine = new List<JournalEntryConfigurationLine>();

    }
}
