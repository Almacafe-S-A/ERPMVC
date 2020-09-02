using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class MatrizRiesgoCustomers
    {
        [Display(Name = "Id")]
        public Int64 Id { get; set; }
        [Display(Name = "Cliente")]
        public Int64 CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        [Display(Name = "Sucursal")]
        public int BranchId { get; set; }
        [Display(Name = "Nombre Sucursal")]
        public string BranchName { get; set; }
        public Customer Customer { get; set; }
        [Display(Name = "Servicio")]
        public Int64 ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [Display(Name = "Factor Riesgo")]
        public Int64 IdFactorRiesgo { get; set; }
        [Display(Name = "Factor Riesgo")]
        public string FactorRiesgo { get; set; }
        [Display(Name = "Riesgo")]
        public string Riesgo { get; set; }
        [Display(Name = "Efecto")]
        public string Efecto { get; set; }
        [Display(Name = "Contexto")]
        public Int64 IdContextoRiesgo { get; set; }
        [ForeignKey("IdContextoRiesgo")]
        public ContextoRiesgo ContextoRiesgo { get; set; }
        [Display(Name = "Responsable")]
        public string Responsable { get; set; }
        [Display(Name = "Riesgo Inicial Probabilidad")]
        public Int64 RiesgoInicialProbabilidad { get; set; }
        [Display(Name = "Riesgo Inicial Impacto")]
        public Int64 RiesgoInicialImpacto { get; set; }
        [Display(Name = "Riesgo Inicial Calificación")]
        public Int64 RiesgoInicialCalificacion { get; set; }
        [Display(Name = "Riesgo Inicial Valor Severidad")]
        public Int64 RiesgoInicialValorSeveridad { get; set; }
        [Display(Name = "Riesgo Inicial Nivel")]
        public string RiesgoInicialNivel { get; set; }
        [Display(Name = "Riesgo Inicial Color Hexadecimal")]
        public string RiesgoInicialColorHexadecimal { get; set; }
        [Display(Name = "Controles")]
        public string Controles { get; set; }
        public int TipodeAccionderiesgoId { get; set; }
        [ForeignKey("TipodeAccionderiesgoId")]
        public TipodeAccionderiesgo TipodeAccionderiesgo { get; set; }
        [Display(Name = "Fecha Objetivo")]
        public string FechaObjetvo { get; set; }
        [Display(Name = "Riesgo Residual Probabilidad")]
        public Int64 RiesgoResidualProbabilidad { get; set; }
        [Display(Name = "Riesgo Residual Impacto")]
        public Int64 RiesgoResidualImpacto { get; set; }
        [Display(Name = "Riesgo Residual Calificación")]
        public Int64 RiesgoResidualCalificacion { get; set; }
        [Display(Name = "Riesgo Residual Valor Severidad")]
        public Int64 RiesgoResidualValorSeveridad { get; set; }
        [Display(Name = "Riesgo Residual Nivel")]
        public string RiesgoResidualNivel { get; set; }
        [Display(Name = "Riesgo Residual Color Hexadecimal")]
        public string RiesgoResidualColorHexadecimal { get; set; }
        [Display(Name = "Seguimiento")]
        public string Seguimiento { get; set; }
        [Display(Name = "Fecha Revisión")]
        public DateTime FechaRevision { get; set; }
        [Display(Name = "Avance")]
        public double Avance { get; set; }
        [Display(Name = "Eficaz")]
        public bool Eficaz { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
