namespace GalaFamilyLibrary.Infrastructure.Domains;

public interface IDateAbility
{
    DateTime CreatedDate { get; set; }

    DateTime? UpdatedDate { get; set; }
}