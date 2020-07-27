using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class CompanyInfo
    {
      
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public Int64 CompanyInfoId { get; set; }

        [Display(Name = "Empresa")]
        public string Company_Name{get;set;}

        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Código Postal")]
        public string PostalCode { get; set; }

        [Display(Name = "Teléfono ")]
        public string Phone { get; set; }

        [Display(Name = "Telefono Movil")]
        public string MobilePhone { get; set; }

        [Display(Name = "Fax")]
        public string Fax { get; set; }

        //[EmailAddress(ErrorMessage ="Agregue una direccion de correo valida")]
        [EmailAddress]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Correo invalido")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Display(Name = "RTN")]
        public string Tax_Id { get; set; }

        [Display(Name = "Imagen")]
        public string image { get; set; }
        [Display(Name = "Direccion de la imagen")]
        public string imageDir { get; set; }


        [Display(Name = "Director General")]
        public string Manager { get; set; }

        [Display(Name = "Dirección")]
        public string RTNMANAGER { get; set; }

        [Display(Name = "Encabezado de impresión")]
        public string PrintHeader { get; set; }

        [Display(Name = "Pais")]
        public Int64 CountryId { get; set; }

        [Display(Name = "Pais")]
        public string CountryName { get; set; }

        [Display(Name = "Hacienda")]
        public string RevOffice { get; set; }

        [Display(Name = "Id de estado")]
        public Int64? IdEstado { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }
        [Display(Name = "Redes Sociales")]
        public string SocialNetworks { get; set; }
        [Display(Name = "Página Web")]
        public string WebPage { get; set; }

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
