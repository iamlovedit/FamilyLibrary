namespace GalaFamilyLibrary.DataTransferObject.Identity
{
    public class UserDTO
    {
        public long Id { get; set; }

        public string? Username { get; set; }

        public string? Nickname { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string? Email { get; set; }
    }
}
