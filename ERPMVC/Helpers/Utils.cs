using System;
using System.Collections.Generic;
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



    }
}
