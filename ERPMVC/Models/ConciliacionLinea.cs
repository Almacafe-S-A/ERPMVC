using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    
    public class ConciliacionLinea
    {
        public int ConciliacionLineaId { get; set; }
        public int? MotivoId { get; set; }
        public int ConciliacionId { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
        public Double Monto { get; set; }
        public string ReferenciaBancaria { get; set; }
        public int CurrencyId { get; set; }
        public DateTime TransDate { get; set; }
        public string ReferenceTrans { get; set; }
        public Int64? JournalEntryId { get; set; }
        public Int64? JournalEntryLineId { get; set; }

        public string PartyName { get; set; }
        public Int64? VoucherTypeId { get; set; }
        public bool Reconciled { get; set; }
        public Int64 CheknumberId { get; set; }
        public string MonedaName { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
