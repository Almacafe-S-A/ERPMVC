using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    //public class PEPS
    //{
    //    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    [Display(Name = "Id")]
    //    public Int64 PEPSId { get; set; }
    //    [Display(Name = "Fecha de documento")]
    //    public DateTime DocumentDate { get; set; }
    //    [Display(Name = "Funcionario")]
    //    public string Funcionario { get; set; }
    //    [Display(Name = "Cargo")]
    //    public string Cargo { get; set; }
    //    [Display(Name = "Departamento")]
    //    public string Departamento { get; set; }
    //    [Display(Name = "Ciudad")]
    //    public string Municipio { get; set; }
    //    public string Pais { get; set; }
    //    [Display(Name = "Observación")]
    //    public string Observacion { get; set; }
    //    [Display(Name = "Oficial")]
    //    public string Official { get; set; }
    //    [Display(Name = "Estado")]
    //    public Int64 IdEstado { get; set; }
    //    [Display(Name = "Estado")]
    //    public string Estado { get; set; }
    //    [Display(Name = "Fecha creación")]
    //    public DateTime FechaCreacion { get; set; }
    //    [Display(Name = "Fecha modificación")]
    //    public DateTime FechaModificacion { get; set; }

    //    [Display(Name = "Usuario creación")]
    //    public string UsuarioCreacion { get; set; }

    //    [Display(Name = "Usuario modificación")]
    //    public string UsuarioModificacion { get; set; }

    //}

    public class PEPS
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 PEPSId { get; set; }
        [Display(Name = "Fecha de documento")]
        public DateTime DocumentDate { get; set; }
        [Display(Name = "Funcionario")]
        public string Funcionario { get; set; }
        [Display(Name = "Cargo")]
        public string Cargo { get; set; }

        [Display(Name = "Departamento")]
        public Int64 StateId { get; set; }

        [Display(Name = "Departamento")]
        public string StateName { get; set; }

        [Display(Name = "Municipio")]
        public Int64 CityId { get; set; }

        [Display(Name = "Municipio")]
        public string CityName { get; set; }

        [Display(Name = "País")]
        public Int64 CountryId { get; set; }

        [Display(Name = "País")]
        public string CountryName { get; set; }

        [Display(Name = "Comentario")]
        public string Comments { get; set; }

        public string Periodo { get; set; }

        [Display(Name = "Oficial")]
        public string Official { get; set; }

        [Display(Name = "Partido Politico")]
        public Int64 PartidoPoliticoId { get; set; }
        [Display(Name = "Partido Politico")]
        public string PartidoPoliticoName { get; set; }

        [Display(Name = "Identidad")]
        public string Identidad { get; set; }
      
        [Display(Name = "Estado")]
        public Int64 IdEstado { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }
        [Display(Name = "Fecha creación")]
        public DateTime FechaCreacion { get; set; }
        [Display(Name = "Fecha modificación")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario creación")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Usuario modificación")]
        public string UsuarioModificacion { get; set; }

    }
}
