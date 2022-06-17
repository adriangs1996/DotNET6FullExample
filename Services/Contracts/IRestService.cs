using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.Models.Inputs;
using Domain.Models.Outputs;

namespace Services.Contracts
{
    public interface IRestService
    {
        Task<IEnumerable<TestEntityDetails>> GetEntities();
        Task<TestEntityDetails> GetEntityDetails(long id);
        Task<HttpResponseMessage> CreateEntity(TestEntityInDto form);
        Task<HttpResponseMessage> UpdateEntity(long id, TestEntityInDto form);
        Task<HttpResponseMessage> DeleteEntity(long id);
    }
}