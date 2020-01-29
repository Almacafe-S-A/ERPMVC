using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class City
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public long Id { get; set; }
        [Display(Name = "Ciudad")]
        public string Name { get; set; }
        [Display(Name = "País")]
        public long? CountryId { get; set; }

        [Display(Name = "Departamento")]
        public long? StateId { get; set; }
        [ForeignKey("StateId")]
        public State State { get; set; }
    }
}
