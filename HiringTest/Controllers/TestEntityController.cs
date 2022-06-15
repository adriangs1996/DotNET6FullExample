using DataAccess.Contracts.UnitOfWork;
using Domain.Models.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace HiringTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestEntityController : ControllerBase
    {
        private readonly IUnitOfWork UoW;

        public TestEntityController(
            IUnitOfWork unitOfWork
        )
        {
            UoW = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<TestEntityOutDto>))]
        public async Task<IActionResult> GetAllTestEntities()
        {
            var entities = await UoW.TestEntities.GetAllAsync();
            return Ok(await UoW.TestEntities.GetAllAsync());
        }
    }
}