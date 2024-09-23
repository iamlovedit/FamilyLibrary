namespace GalaFamilyLibrary.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class IdempotencyAttribute(string parameter, int seconds = 5, string message = "请求过于频繁") : Attribute
    {
        public string Parameter { get; } = parameter;

        public int Seconds { get; } = seconds;

        public string Message { get; } = message;
    }
}