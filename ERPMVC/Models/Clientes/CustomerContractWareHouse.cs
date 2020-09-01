using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CustomerContractWareHouse
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 CustomerContractWareHouseId { get; set; }
        [Display(Name = "Contrato")]
        public Int64 CustomerContractId { get; set; }
        [Display(Name = "Bodega")]
        public Int64 WareHouseId { get; set; }
        [Display(Name = "Bodega")]
        public string WareHouseName { get; set; }

        public string EdificioName { get; set; }


        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de Modificación")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario de Creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de Modificación")]
        public string UsuarioModificacion { get; set; }

    }
}
