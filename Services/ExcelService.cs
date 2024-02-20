using Amazon.Runtime;
using BlazorAppExcel.Interfaces;
using BlazorAppExcel.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorAppExcel.Services
{
    public class ExcelService : IExcelService
    {
        private readonly string baseUrl;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration config;

        public ExcelService(IHttpClientFactory client, IConfiguration config) {
            this._httpClientFactory = client;
        }

        public async Task<IList<TableExcel>> getTableExcels(string user)
        {
            var httpClient = _httpClientFactory.CreateClient("MyNamedClient");

            var resp = await httpClient.GetAsync($"excel/{user}");

            HttpResponseMessage response = await httpClient.GetAsync($"excel/{user}");

            // Check if the request was successful (status code 200)
            if (response.IsSuccessStatusCode)
            {
                // Read the content of the response as a string
                string responseData = await response.Content.ReadAsStringAsync();

                // Print the response data
                if (responseData!=null)
                    return JsonSerializer.Deserialize<IList<TableExcel>>(responseData);
            }

            return new List<TableExcel>();
        }

        public async Task saveAsync(TableExcel model)
        {
            var httpClient = _httpClientFactory.CreateClient("MyNamedClient");

            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("excel", content);
        }

    }
}
