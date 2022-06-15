
namespace Domain.Models.Outputs
{
    public class TestEntityOutDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
    }

    public class TestEntityDetails: TestEntityOutDto
    {
        public bool IsComplete { get; set; }
    }
}