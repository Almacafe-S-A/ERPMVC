using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class MainContraAccount
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 MainAccountId { get; set; }
        public int RelatedContraAccountId { get; set; }

        [ForeignKey("MainAccountId")]
        [InverseProperty("ContraAccounts")]
        public virtual Accounting MainAccount { get; set; }
        [ForeignKey("RelatedContraAccountId")]
        public virtual Accounting RelatedContraAccount { get; set; }
    }
}
