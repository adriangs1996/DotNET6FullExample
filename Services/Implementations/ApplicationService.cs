using AutoMapper;
using DataAccess.Contracts.UnitOfWork;
using Domain.Entities;
using Domain.Models.Inputs;
using Services.Contracts;

namespace Services.Implementation
{
    public class ApplicationService : IApplicationService
    {
        /// <summary>
        /// Entrypoint to access all Repositories. Also handles transactional
        /// operations.
        /// </summary>
        private readonly IUnitOfWork uow;

        /// <summary>
        /// Utility to easily update and map objects.
        /// </summary>
        private readonly IMapper mapper;
        
        /// <summary>
        /// Constructor based Dependency Injection.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            uow = unitOfWork;
            this.mapper = mapper;
        }

        /// <summary>
        /// Creates a new <see cref="TestEntity"/> with
        /// name and IsComplete populated from <paramref name="form"/>.
        /// </summary>
        /// <param name="form">
        /// Data for the new <see cref="TestEntity"/>. Service
        /// layer assumes this form is has already being validated.
        /// </param>
        /// <returns>
        /// A new <see cref="TestEntity"/> on success, or null
        /// </returns>
        public async Task<TestEntity> CreateTestEntity(TestEntityInDto form)
        {
            var new_entity = mapper.Map<TestEntity>(form);
            // Unit of Work allows to run insert/update/delete operations
            // inside a business transaction
            await uow.TestEntities.AddAsync(new_entity);
            await uow.Commit();

            return new_entity;
        }

        /// <summary>
        /// Delete the entity given by <paramref name="id"/> if present.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// The deleted <see cref="TestEntity"/> on success, null otherwise 
        /// </returns>
        public async Task<TestEntity> DeleteTestEntity(long id)
        {
            var entity = await uow.TestEntities.GetByIdAsync(id);
            if (entity is null)
                return null;

            uow.TestEntities.Delete(entity);
            await uow.Commit();
            return entity;
        }

        /// <summary>
        /// Returns the collection of <see cref="TestEntity"/>
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TestEntity>> GetAllTestEntities()
        {
            return await uow.TestEntities.GetAllAsync();
        }

        /// <summary>
        /// Find a <see cref="TestEntity"/> by <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// A <see cref="TestEntity"/> on success, null if not found.
        /// </returns>
        public async Task<TestEntity> GetTestEntityById(long id)
        {
            return await uow.TestEntities.GetByIdAsync(id);
        }

        /// <summary>
        /// Updates the <see cref="TestEntity"/> given by <paramref name="id"/>
        /// if present, using the data from <paramref name="form"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="form"></param>
        /// <returns>
        /// The updated <see cref="TestEntity"/> on success, null if not found.
        /// </returns>
        public async Task<TestEntity> UpdateEntityIfPresent(long id, TestEntityInDto form)
        {
            var entity = await uow.TestEntities.GetByIdAsync(id);

            if (entity is null)
                return null;

            // At this point, entity is not null and it is already being tracked
            // by the UoW session. Now we can modify it and commit the transaction
            mapper.Map(form, entity);
            await uow.Commit();

            return entity;
        }
    }
}