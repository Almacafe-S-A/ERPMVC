using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class PasswordHistory
    {

        public PasswordHistory()
        {
            CreatedDate = DateTime.Now;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 PasswordHistoryId { get; set; }

        public DateTime CreatedDate { get; set; }

        //[Key, Column(Order = 1)]
        public string PasswordHash { get; set; }

        //[Key, Column(Order = 0)]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }



    }



}
