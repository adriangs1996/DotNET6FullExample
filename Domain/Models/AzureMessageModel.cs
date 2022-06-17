using Domain.Models.Outputs;

namespace Domain.Models
{
    public enum Topic
    {
        DeletedTestEntity,
        EditedTestEntity,
        CreatedTestEntity
    }

    public record AzureMessageModel(
        Topic Topic,
        TestEntityDetails Entity
    );
}