using AutoMapper;
using DataAccess.Contracts.UnitOfWork;
using Domain.Entities;
using Domain.Models.Inputs;
using Domain.Models.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace HiringTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestEntityController : ControllerBase
    {
        private readonly IUnitOfWork UoW;
        private readonly IMapper Mapper;

        public TestEntityController(
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            UoW = unitOfWork;
            Mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<TestEntityOutDto>))]
        public async Task<IActionResult> GetAllTestEntities()
        {
            var entities = await UoW.TestEntities.GetAllAsync();
            var dtos = Mapper.Map<List<TestEntityOutDto>>(entities);
            return Ok(dtos);
        }

        [HttpGet("/{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(TestEntityDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTestEntityDetails(long id)
        {
            var entity = await UoW.TestEntities.GetByIdAsync(id);
            if (entity is null)
                return NotFound();

            var dto = Mapper.Map<TestEntityDetails>(entity);

            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNewTestEntity(TestEntityInDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // ModelState ensures that Name is present, thanks to
            // the [Required] attribute
            var new_entity = Mapper.Map<TestEntity>(model);

            // Unit of Work allows to run insert/update/delete operations
            // inside a business transaction
            await UoW.TestEntities.AddAsync(new_entity);
            await UoW.Commit();

            var dto = Mapper.Map<TestEntityDetails>(new_entity);
            return CreatedAtAction(nameof(GetTestEntityDetails), new { id = new_entity.Id }, dto);
        }

        [HttpPut("/{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status202Accepted)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTestEntity(long id, TestEntityInDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Try to find the test entity
            var entity = await UoW.TestEntities.GetByIdAsync(id);
            if (entity is null)
                return NotFound();

            // At this point, entity is not null and it is already being tracked
            // by the UoW session. Now we can modify it and commit the transaction
            Mapper.Map(model, entity);
            await UoW.Commit();

            // Get the output dto and provide the location of the updated resource
            var dto = Mapper.Map<TestEntityDetails>(entity);
            return AcceptedAtAction(nameof(GetTestEntityDetails), new { id = id }, dto);
        }

        [HttpDelete("/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> DeleteTestEntity(long id)
        {
            var entity = await UoW.TestEntities.GetByIdAsync(id);
            if (entity is null)
                return NotFound();

            UoW.TestEntities.Delete(entity);
            await UoW.Commit();

            return Accepted();
        }
    }
}