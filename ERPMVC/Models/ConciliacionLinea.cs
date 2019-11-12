using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    //public class ConciliacionLinea
    //{
    //    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    [Display(Name = "Id")]
    //    public int ConciliacionLineaId { get; set; }

    //    [ForeignKey("ElementoConfiguracion")]
    //    public ElementoConfiguracion TipoTransaccion { get; set; }

    //    [Display(Name = "Cuenta")]
    //    public Int64 AccountId { get; set; }
    //    [Display(Name = "Cuenta")]
    //    public string AccountName { get; set; }

    //    [Display(Name = "Crédito")]
    //    public double Credit { get; set; }
    //    [Display(Name = "Débito")]

    //    public double Debit { get; set; }
    //    [Required]
    //    [Display(Name = "Monto")]
    //    public Double Monto { get; set; }


    //    [Required]
    //    [Display(Name = "ReferenciaBancaria")]
    //    public string ReferenciaBancaria { get; set; }

    //    [ForeignKey("IdMoneda")]
    //    public Currency Moneda { get; set; }

    //    [Required]
    //    [Display(Name = "MonedaName")]
    //    public string MonedaName { get; set; }


    //    [Required]
    //    [Display(Name = "FechaCreacion")]
    //    public DateTime FechaCreacion { get; set; }

    //    [Required]
    //    [Display(Name = "FechaModificacion")]
    //    public DateTime FechaModificacion { get; set; }

    //    [Required]
    //    [Display(Name = "UsuarioCreacion")]
    //    public string UsuarioCreacion { get; set; }

    //    [Required]
    //    [Display(Name = "UsuarioModificacion")]
    //    public string UsuarioModificacion { get; set; }
    //}

    public class ConciliacionLinea
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int ConciliacionLineaId { get; set; }

        [ForeignKey("ElementoConfiguracion")]
        public ElementoConfiguracion TipoTransaccion { get; set; }

        [Display(Name = "Id Conciliacion")]
        public int ConciliacionId { get; set; }

        [Display(Name = "Cuenta")]
        public Int64 AccountId { get; set; }
        [Display(Name = "Cuenta")]
        public string AccountName { get; set; }

        [Display(Name = "Crédito")]
        public double Credit { get; set; }
        [Display(Name = "Debito")]

        public double Debit { get; set; }
        [Required]
        [Display(Name = "Balance")]
        public Double Monto { get; set; }


        [Required]
        [Display(Name = "Referencia Bancaria")]
        public string ReferenciaBancaria { get; set; }

        [ForeignKey("CurrencyId")]
        public int CurrencyId { get; set; }

        [Display(Name = "Fecha Transaccion")]
        public DateTime TransDate { get; set; }
        [Display(Name = "Referencia Transacción")]
        public string ReferenceTrans { get; set; }
        [Display(Name = "Id de Journal")]
        public Int64 JournalEntryId { get; set; }
        [Display(Name = "Id de Journal Linea")]
        public Int64 JournalEntryLineId { get; set; }
        [Display(Name = "Tipos de Voucher/Documento")]
        public Int64 VoucherTypeId { get; set; }
        [Display(Name = "Conciliado")]
        public bool Reconciled { get; set; }
        [Display(Name = "Numero de cheque")]
        public Int64 CheknumberId { get; set; }



        [Required]
        [Display(Name = "MonedaName")]
        public string MonedaName { get; set; }


        [Required]
        [Display(Name = "FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [Display(Name = "FechaModificacion")]
        public DateTime FechaModificacion { get; set; }

        [Required]
        [Display(Name = "UsuarioCreacion")]
        public string UsuarioCreacion { get; set; }

        [Required]
        [Display(Name = "UsuarioModificacion")]
        public string UsuarioModificacion { get; set; }
    }
}
