using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class DepreciationFixedAsset
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 DepreciationFixedAssetId { get; set; }

        [Display(Name = "Id")]
        public Int64 FixedAssetId { get; set; }

        [Display(Name = "Año")]
        public Int32 Year { get; set; }

        [Display(Name = "Enero")]
        public double January { get; set; }

        [Display(Name = "Febrero")]
        public double February { get; set; }

        [Display(Name = "Marzo")]
        public double March { get; set; }

        [Display(Name = "Abril")]
        public double April { get; set; }

        [Display(Name = "Mayo")]
        public double May { get; set; }

        [Display(Name = "Junio")]
        public double June { get; set; }

        [Display(Name = "Julio")]
        public double July { get; set; }

        [Display(Name = "Agosto")]
        public double August { get; set; }

        [Display(Name = "Septiembre")]
        public double September { get; set; }

        [Display(Name = "Octubre")]
        public double October { get; set; }

        [Display(Name = "Noviembre")]
        public double November { get; set; }

        [Display(Name = "Diciembre")]
        public double December { get; set; }

        [Display(Name = "Total Depreciado")]
        public double TotalDepreciated { get; set; }


        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario de creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario de modificación")]
        public string UsuarioModificacion { get; set; }

    }


}
