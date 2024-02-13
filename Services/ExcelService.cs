using BlazorAppExcel.Interfaces;
using BlazorAppExcel.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BlazorAppExcel.Services
{
    public class ExcelService : IExcelService
    {
        private readonly string baseUrl;
        private readonly HttpClient httpClient;
        private readonly IConfiguration config;

        public ExcelService(HttpClient client, IConfiguration config) {
            this.httpClient = client;
            this.config = config;
            this.httpClient.BaseAddress = new Uri(config["ServerUrl:BaseAddress"]??"".ToString());
        }
        public async Task saveAsync(TableExcel model)
        {
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            await this.httpClient.PostAsync("excel", content);
        }
    }
}
