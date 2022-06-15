using AutoMapper;
using DataAccess.Contracts.UnitOfWork;
using Domain.Entities;
using Domain.Models.Inputs;
using Services.Contracts;

namespace Services.Implementation
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            uow = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<TestEntity> CreateTestEntity(TestEntityInDto form)
        {
            var new_entity = mapper.Map<TestEntity>(form);
            // Unit of Work allows to run insert/update/delete operations
            // inside a business transaction
            await uow.TestEntities.AddAsync(new_entity);
            await uow.Commit();

            return new_entity;
        }

        public async Task<TestEntity> DeleteTestEntity(long id)
        {
            var entity = await uow.TestEntities.GetByIdAsync(id);
            if (entity is null)
                return null;

            uow.TestEntities.Delete(entity);
            await uow.Commit();
            return entity;
        }

        public async Task<IEnumerable<TestEntity>> GetAllTestEntities()
        {
            return await uow.TestEntities.GetAllAsync();
        }

        public async Task<TestEntity> GetTestEntityById(long id)
        {
            return await uow.TestEntities.GetByIdAsync(id);
        }

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