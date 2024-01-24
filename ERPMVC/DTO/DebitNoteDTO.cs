using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class DebitNoteDTO : DebitNote
    {
        [Display(Name = "Número SAR")]
        
        public int editar { get; set; } = 1;

        public int interna { get; set; }
    }
}
