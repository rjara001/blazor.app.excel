﻿using Amazon.Runtime;
using BlazorAppExcel.Interfaces;
using BlazorAppExcel.Models;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using System.Text;

namespace BlazorAppExcel.Services
{
    public class ExcelService : IExcelService
    {
        private readonly string baseUrl;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStore;
        private readonly IConfiguration config;

        public ExcelService(IHttpClientFactory client, IConfiguration config, ILocalStorageService localStore) {
            this._httpClientFactory = client;
            this._localStore = localStore;
        }

        public async Task<User> GetUser(string nameUser)
        {
            User _user = await getUserFromLocalStorage(nameUser);

            if (_user != null) return _user;

            _user = new User();
            _user.Name = nameUser;

            _user.SetTables(await this.getTables(nameUser));

            await this._localStore.SetItemAsync<User>("__user", _user);

            return _user;

        }

        private async Task<IList<TableExcel>> getTables(string user)
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
                if (responseData != null)
                    return JsonConvert.DeserializeObject<IList<TableExcel>>(responseData);
            }

            return new List<TableExcel>();
        }

        public async Task SetUser(User user, TableExcel table)
        {
            await this._localStore.SetItemAsync<User>("__user", user);

            var httpClient = _httpClientFactory.CreateClient("MyNamedClient");

            var content = new StringContent(JsonConvert.SerializeObject(table), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync("excel", content);

            if (response.IsSuccessStatusCode)
            {
                // Read the content of the response as a string
                string responseData = await response.Content.ReadAsStringAsync();

                // Print the response data
                if (responseData != null){
                    var obj = JsonConvert.DeserializeObject<TableExcel>(responseData);
                    if (obj != null)
                    {
                        table.Id = obj.Id;
                        // We store the id value
                        await this._localStore.SetItemAsync<User>("__user", user);
                    }
      
                }
            }
        }

        public async Task Delete(User user, TableExcel table)
        {
            user.Tables.Remove(table.Name);
            await this._localStore.SetItemAsync<User>("__user", user);

            var httpClient = _httpClientFactory.CreateClient("MyNamedClient");

            await httpClient.DeleteAsync($"excel/{user.Name}/{table.Id}");
        }

        async Task<User> getUserFromLocalStorage(string nameUser)
        {
            if (await this._localStore.ContainKeyAsync("__user"))
            {
                string _json = await this._localStore.GetItemAsync<string>("__user");

                return JsonConvert.DeserializeObject<User>(_json);
            }

            return null;
        }

        public async Task<string> ChangeTableName(User user, TableExcel table, string newName )
        {
            string nameCode = Util.GetName(user.Tables, table, newName);

            user.Tables.Remove(table.Name);
            table.Name = nameCode;
            user.Tables.Add(table.Name, table);

            await SetUser(user, table);

            return nameCode;
        }

      
    }
}
