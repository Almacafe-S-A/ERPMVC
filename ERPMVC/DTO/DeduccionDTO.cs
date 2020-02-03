using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class DeduccionDTO
    {
        public Int64 DeductionId { get; set; }
        public string Description { get; set; }
        public Int64 DeductionTypeId { get; set; }
        public string DeductionType { get; set; }
        public double Formula { get; set; }
        public double Fortnight { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
    }
}
