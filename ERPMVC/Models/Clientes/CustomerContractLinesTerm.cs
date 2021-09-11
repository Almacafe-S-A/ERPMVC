using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CustomerContractLinesTerms
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Position { get; set; }

        public int ContractTermId { get; set; }
        [ForeignKey("ContractTermId")]
        public CustomerContractTerms CustomerContractTerm { get; set; }

        public string TermTitle { get; set; }

        public string Term { get; set; }




    }
}
