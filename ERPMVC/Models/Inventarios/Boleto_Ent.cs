using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class Boleto_Ent
    {
        //[Display(Name = "Clave de entrada(Consecutivo)")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 clave_e { get; set; }
        [Display(Name = "Clave de la compañia(cliente o proveedor)")]
        public string clave_C { get; set; }

        public Int64? CustomerId { get; set; }
        public Customer Customer { get; set; }

        public string CustomerName { get; set; }

        public string SubProductName { get; set; }

        public Int64? SubProductId { get; set; }
        public SubProduct SubProduct { get; set; }

        public bool CapturaAutomatica { get; set; }

        public decimal PesoLBS { get; set; }

        public decimal PesoKG { get; set; }

        public decimal PesoTM { get; set; }
        public decimal PesoQQ { get; set; }


        public decimal PesoLBSI { get; set; }

        public decimal PesoKGI { get; set; }

        public decimal PesoTMI { get; set; }
        public decimal PesoQQI { get; set; }

        public string Destino { get; set; }

        public string MarcaVehiculo { get; set; }

        public string Barco { get; set; }

        public decimal PesoPuerto { get; set; }

        public string Tranportista { get; set; }

        public string RTNTransportista { get; set; }

        public string Orden { get; set; }

        public string PlacaContenedor { get; set; }

        public bool? Ingreso { get; set; }


        [Display(Name = "Clave del producto")]
        [NotMapped]
        public string NombreProducto { get; set; }
        [NotMapped]
        public string Cliente { get; set; }

        [NotMapped]
        public string Unidad { get; set; }
        public string clave_p { get; set; }
        [Display(Name = "Salida")]
        public bool completo { get; set; }
        [Display(Name = "Fecha de captura del peso de entrada")]
        public DateTime fecha_e { get; set; }
        [Display(Name = "Hora de captura del peso de entrada")]
        public string hora_e { get; set; }
        [Display(Name = "Identificador del vehículo")]
        public string placas { get; set; }
        [Display(Name = "Nombre del conductor del vehiculo")]
        public string conductor { get; set; }   

        public string DNIConductor { get; set; }
        [Display(Name = "Valor del peso de entrada")]
        public Int32 peso_e { get; set; }
        [Display(Name = "Observaciones del proceso de captura")]
        public string observa_e { get; set; }
        [Display(Name = "Nombre del operador de entrada")]
        public string nombre_oe { get; set; }
        [Display(Name = "Turno del operador de entrada")]
        public string turno_oe { get; set; }
        [Display(Name = "Unidades de la pesada de entrada")]
        public string unidad_e { get; set; }
        [Display(Name = "Bascula de entrada")]
        public string bascula_e { get; set; }
        [Display(Name = "Tipo de entrada(1.Automatico , 2.Manual, 3. Predeterminado")]
        public int t_entrada { get; set; }
        [Display(Name = "Clave del usuario")]
        public string clave_u { get; set; }

        [NotMapped]
        public decimal PesoUnidadPreferidaEntrada { get; set; }
        [NotMapped]
        public decimal PesoUnidadPreferidaSalida { get; set; }
        [NotMapped]
        public decimal PesoUnidadPreferidaNeto { get; set; }
        [NotMapped]
        public string UnidadPreferida { get; set; }
        [NotMapped]
        public int UnidadPreferidaId { get; set; }

         public decimal Convercion(double pesoLBE, int UOM)
        {

            double pesonetoLBI = (pesoLBE * 460) / 453.59;
            double qq = pesoLBE / 100;
            double tm = pesoLBE / 2204.62;
            
            double kg = pesoLBE / 2.20462;

            double qqi = pesonetoLBI / 100;
            double tmi = pesonetoLBI / 2204.62;
            double kgi = pesonetoLBI / 2.20462;



            switch (UOM)
            {
                case 1:
                    return Decimal.Round(Convert.ToDecimal(pesoLBE), 3);
                    
                case 2:
                    return Decimal.Round(Convert.ToDecimal(kg), 3);
                    
                case 3:
                    return Decimal.Round(Convert.ToDecimal(qq), 3);
                   
                case 4:
                    return Decimal.Round(Convert.ToDecimal(tm), 3);
                    
                case 5:
                    return Decimal.Round(Convert.ToDecimal(pesonetoLBI), 3);
                    
                case 6:

                    return Decimal.Round(Convert.ToDecimal(kgi), 3);
                    
                case 7:
                    return Decimal.Round(Convert.ToDecimal(qqi), 3);
                    
                case 8:
                    return Decimal.Round(Convert.ToDecimal(tmi), 3);
                    
            }

            return Decimal.Round(Convert.ToDecimal(pesoLBE),3);

            


        }

        public virtual Boleto_Sal Boleto_Sal { get; set; }
    }
}
