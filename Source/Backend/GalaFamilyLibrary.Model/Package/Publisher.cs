using GalaFamilyLibrary.Infrastructure.Domains;

namespace GalaFamilyLibrary.Model.Package
{
    public class Publisher : IdentifiableBase<string>
    {
        public string Username { get; set; }
    }
}