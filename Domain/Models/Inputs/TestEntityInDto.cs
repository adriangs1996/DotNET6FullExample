using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Inputs
{
    public class TestEntityInDto
    {
        [Required]
        public string? Name { get; set; }
        public bool IsComplete { get; set; } = false;
    }
}