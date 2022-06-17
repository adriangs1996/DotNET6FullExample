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
        private readonly IMapper _mapper;
        private readonly IApplicationService _services;
        private readonly INotificationService _notificationService;

        public TestEntityController(IMapper mapper, IApplicationService services,
            INotificationService notificationService)
        {
            _mapper = mapper;
            _services = services;
            _notificationService = notificationService;
        }

        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<TestEntityDetails>))]
        public async Task<IActionResult> GetAllTestEntities()
        {
            var entities = await _services.GetAllTestEntities();
            var dtos = _mapper.Map<List<TestEntityDetails>>(entities);
            return Ok(dtos);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(TestEntityDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTestEntityDetails(long id)
        {
            var entity = await _services.GetTestEntityById(id);
            if (entity is null) 
                return NotFound();
            var dto = _mapper.Map<TestEntityDetails>(entity);
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNewTestEntity(TestEntityInDto model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            // ModelState ensures that Name is present, thanks to
            // the [Required] attribute
            var newEntity = await _services.CreateTestEntity(model);
            var dto = _mapper.Map<TestEntityDetails>(newEntity);

            // Send a notification that a new TestEntity has been created
            await _notificationService.TestEntityAdded(dto);
            return CreatedAtAction(nameof(GetTestEntityDetails), new {id = newEntity.Id}, dto);
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTestEntity(long id, TestEntityInDto model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            
            var entity = await _services.UpdateEntityIfPresent(id, model);
            if (entity is null) 
                return NotFound();

            // Get the output dto and provide the location of the updated resource
            var dto = _mapper.Map<TestEntityDetails>(entity);
            // Send a notification that a test entity has been edited
            await _notificationService.TestEntityEdited(dto);
            return AcceptedAtAction(nameof(GetTestEntityDetails), new {id}, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> DeleteTestEntity(long id)
        {
            var entity = await _services.DeleteTestEntity(id);
            if (entity is null) 
                return NotFound();
            
            var dto = _mapper.Map<TestEntityDetails>(entity);
            await _notificationService.TestEntityRemoved(dto);
            return Accepted();
        }
    }
}