using System.Net.Http.Json;
using Domain.Models.Inputs;
using Domain.Models.Outputs;
using Services.Contracts;

namespace Services.Implementation
{
    public class RestService : IRestService
    {
        private readonly HttpClient http;

        public RestService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<HttpResponseMessage> CreateEntity(TestEntityInDto form)
        {
            return await http.PostAsJsonAsync($"api/testentity", form);
        }

        public async Task<HttpResponseMessage> DeleteEntity(long id)
        {
            return await http.DeleteAsync($"api/testentity/{id}");
        }

        public async Task<IEnumerable<TestEntityDetails>> GetEntities()
        {
            Console.WriteLine(http.BaseAddress);
            return await http.GetFromJsonAsync<List<TestEntityDetails>>("api/testentity");
        }

        public async Task<TestEntityDetails> GetEntityDetails(long id)
        {
            return await http.GetFromJsonAsync<TestEntityDetails>($"api/testentity/{id}");
        }

        public async Task<HttpResponseMessage> UpdateEntity(long id, TestEntityInDto form)
        {
            return await http.PutAsJsonAsync($"api/testentity/{id}", form);
        }
    }
}