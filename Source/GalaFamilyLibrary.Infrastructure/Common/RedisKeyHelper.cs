namespace GalaFamilyLibrary.Infrastructure.Common
{
    public static class RedisKeyHelper
    {
        public static string GetUserKey(long id)
        {
            return $"user/{id}";
        }

        public static string GetUserDetailsKey(long id)
        {
            return $"user/details/{id}";
        }
    }
}