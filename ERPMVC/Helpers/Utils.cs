using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;

namespace ERPMVC.Helpers
{
    public static class Utils
    {
        public static string ConexionReportes { get; set; }

        public static bool Cerrado { get; set; }

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
