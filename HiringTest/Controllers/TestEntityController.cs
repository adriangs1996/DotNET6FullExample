using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models.Inputs;
using Domain.Models.Outputs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace HiringTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestEntityController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IApplicationService services;
        private readonly INotificationService notificationService;

        public TestEntityController(
            IMapper mapper, 
            IApplicationService services,
            INotificationService notificationService
        )
        {
            this.mapper = mapper;
            this.services = services;
            this.notificationService = notificationService;
        }

        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<TestEntityDetails>))]
        public async Task<IActionResult> GetAllTestEntities()
        {
            var entities = await services.GetAllTestEntities();
            var dtos = mapper.Map<List<TestEntityDetails>>(entities);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(TestEntityDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTestEntityDetails(long id)
        {
            var entity = await services.GetTestEntityById(id);
            if (entity is null)
                return NotFound();

            var dto = mapper.Map<TestEntityDetails>(entity);

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
            var new_entity = await services.CreateTestEntity(model);

            var dto = mapper.Map<TestEntityDetails>(new_entity);

            // Send a notification that a new TestEntity has been created
            await notificationService.TestEntityAdded(dto);
            
            return CreatedAtAction(nameof(GetTestEntityDetails), new { id = new_entity.Id }, dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status202Accepted)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTestEntity(long id, TestEntityInDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await services.UpdateEntityIfPresent(id, model);
            if (entity is null)
                return NotFound();

            // Get the output dto and provide the location of the updated resource
            var dto = mapper.Map<TestEntityDetails>(entity);

            // Send a notification that a test entity has been edited
            await notificationService.TestEntityEdited(dto);

            return AcceptedAtAction(nameof(GetTestEntityDetails), new { id }, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> DeleteTestEntity(long id)
        {
            var entity = await services.DeleteTestEntity(id);
            if (entity is null)
                return NotFound();

            var dto = mapper.Map<TestEntityDetails>(entity);
            await notificationService.TestEntityRemoved(dto);

            return Accepted();
        }
    }
}