namespace GalaFamilyLibrary.Infrastructure.OAuth.QQ
{
    public class QQAuthenticationOptions : AuthenticationOAuthOptions
    {
        public string AvatarFullUrl { get; } = "urn:qq:avatar_full";

        public string AvatarUrl { get; } = "urn:qq:avatar";

        public string PictureFullUrl { get; } = "urn:qq:picture_full";

        public string PictureMediumUrl { get; } = "urn:qq:picture_medium";

        public string PictureUrl { get; } = "urn:qq:picture";

        public string UnionId { get; } = "urn:qq:unionid";

        public override string RedirectUri { get; set; }
    }
}