using System.Collections.Generic;
using Domain.Entities;
using Domain.Models.Outputs;

namespace Domain.Mocks
{
    public class TestEntityDetailsMock
    {
        public static IEnumerable<TestEntity> MockTestEntitiesDetails(int count)
        {
            List<TestEntity> mock = new();
            for (var i = 1; i <= count; i++)
            {
                mock.Add(new TestEntity
                {
                    Id = i,
                    Name = $"Entity{i}",
                    IsComplete = true
                });
            }

            return mock;
        }

        public static TestEntity GetOne()
        {
            return new TestEntity
            {
                Id = 1,
                Name = "testing",
                IsComplete = true
            };
        }
    }
}