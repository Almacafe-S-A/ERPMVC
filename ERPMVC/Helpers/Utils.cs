using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
    }
}
