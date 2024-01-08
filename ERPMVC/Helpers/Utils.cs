using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Models;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;

namespace ERPMVC.Helpers
{
    public static class Utils
    {
        public static string ConexionReportes { get; set; }

        public static bool Cerrado { get; set; }

        public static int PeriodoActualId { get; set; }

        public static string PeriodoActual { get; set; }

        public  static Periodo Periodo { get; set; }

        public static bool InicioPagodeCuotasISR { get; set; }

        public static async Task<HttpResponseMessage> HttpGetAsync(string token, string url)
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            return await cliente.GetAsync(url);
        }

        public static async Task<HttpResponseMessage> HttpPostAsync(string token, string url, object valor)
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            return await cliente.PostAsJsonAsync(url, valor);
        }

        public static async Task<HttpResponseMessage> HttpPutAsync(string token, string url, object valor)
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            return await cliente.PutAsJsonAsync(url, valor);
        }

        public static DateTime GetFechaXLS(ICell celda)
        {
            if (celda.CellType == CellType.Blank || 
                celda.CellType == CellType.Unknown ||
                celda.CellType == CellType.Error || 
                celda.CellType == CellType.Boolean || 
                celda.CellType == CellType.Formula)
            {
                return DateTime.MinValue;
            }
            if(celda.CellType == CellType.String)
            {
                string[] formatos = {"d/M/yyyy H:mm:ss tt", "d/M/yyyy H:mm tt",
                         "dd/MM/yyyy HH:mm:ss", "d/M/yyyy H:mm:ss",
                         "d/M/yyyy HH:mm tt", "d/M/yyyy HH tt",
                         "d/M/yyyy H:mm", "d/M/yyyy H:mm",
                         "dd/MM/yyyy HH:mm", "dd/M/yyyy HH:mm",
                         "d/MM/yyyy HH:mm:ss.ffffff" };
               // string[] formatos = {"d/M/yyyy H:mm:ss" };
                string fecha = celda.StringCellValue;
                //string formato = celda.CellStyle.GetDataFormatString();
                //return DateTime.Parse(fecha);
                return DateTime.ParseExact(fecha, formatos, null);
            }


            try
            {
                return celda.DateCellValue;
            }
            catch (Exception)
            {
                var cadena = celda.StringCellValue;
                try
                {
                    string formato = celda.CellStyle.GetDataFormatString();
                    return DateTime.ParseExact(cadena, formato, null);
                }
                catch (Exception)
                {
                    return DateTime.MinValue;
                }
            }
        }

        public static DateTime GetHoraXLS(ICell celda)
        {
            if (celda.CellType == CellType.Blank ||
                celda.CellType == CellType.Unknown ||
                celda.CellType == CellType.Error ||
                celda.CellType == CellType.Boolean ||
                celda.CellType == CellType.Formula)
            {
                return DateTime.MinValue;
            }

            try
            {
                return celda.DateCellValue;
            }
            catch (Exception)
            {
                var cadena = celda.StringCellValue;
                try
                {
                    cadena = cadena.ToLower().Replace("a.m.", "AM");
                    cadena = cadena.ToLower().Replace("p.m.", "PM");
                    cadena = cadena.ToLower().Replace("am.", "AM");
                    cadena = cadena.ToLower().Replace("pm.", "PM");
                    cadena = cadena.ToLower().Replace("a.m", "AM");
                    cadena = cadena.ToLower().Replace("p.m", "PM");
                    cadena = cadena.ToLower().Replace("am", "AM");
                    cadena = cadena.ToLower().Replace("pm", "PM");
                    cadena = cadena.ToUpper();
                    if (cadena.Contains("AM") || cadena.Contains("PM"))
                    {
                        return DateTime.ParseExact(cadena, "hh:mm tt", CultureInfo.InvariantCulture);
                    }
                    return DateTime.ParseExact(cadena, "hh:mm", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    return DateTime.MinValue;
                }
            }
        }

        public static double? GetNumeroXLS(ICell celda)
        {
            if (celda.CellType == CellType.Blank ||
                celda.CellType == CellType.Unknown ||
                celda.CellType == CellType.Error ||
                celda.CellType == CellType.Boolean ||
                celda.CellType == CellType.Formula)
            {
                return null;
            }

            try
            {
                return celda.NumericCellValue;
            }
            catch (Exception)
            {
                var cadena = celda.StringCellValue;
                try
                {
                    return double.Parse(cadena);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

    }
}
