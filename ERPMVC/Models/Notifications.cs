using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Notifications
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Description { get; set; }
		public DateTime FechaNotificacion { get; set; }
		public DateTime? FechaLectura { get; set; }
		public bool Leido { get; set; }
		public string ModuloInvocacion { get; set; }
		public string Link { get; set; }
		public string PermisoLectura { get; set; }
		public string UsuarioCreacion { get; set; }
		public string UsuarioLectura { get; set; }
        public string Titulo { get; set; }
        public string Icono { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }
}