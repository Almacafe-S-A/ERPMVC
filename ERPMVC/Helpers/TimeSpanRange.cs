using System;
using System.Collections.Generic;
using System.Linq;


namespace ERPMVC.Helpers
{


    // Definir una clase para representar un rango de horas
    public class TimeSpanRange
    {
        // Definir las propiedades de inicio y fin del rango
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        // Definir un constructor que recibe el inicio y el fin del rango
        public TimeSpanRange(TimeSpan start, TimeSpan end)
        {
            Start = start;
            End = end;
        }

        // Definir un método que devuelve la intersección entre dos rangos de horas
        public TimeSpanRange Intersect(TimeSpanRange other)
        {
            
            // Si los rangos no se solapan, devolver null
            if (other == null || Start > other.End || End < other.Start)
            {
                return null;
            }
            // Si los rangos se solapan, devolver el rango común
            else
            {
                return new TimeSpanRange(
                    Start > other.Start ? Start : other.Start,
                    End < other.End ? End : other.End
                );
            }
        }

        public double DuracionHoras() {
            double duracion =0;
            duracion = (End - Start).TotalHours;
            return duracion; 
        
        }
    }
}
