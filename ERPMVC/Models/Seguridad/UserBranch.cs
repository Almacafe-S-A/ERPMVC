using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class UserBranch
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }

        public string BranchName { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Fecha de Modificación")]
        public DateTime? ModifiedDate { get; set; }
        [Display(Name = "Usuario de Creación")]
        public string CreatedUser { get; set; }

        [Display(Name = "Usuario de Modificacion")]
        public string ModifiedUser { get; set; }

    }
}
