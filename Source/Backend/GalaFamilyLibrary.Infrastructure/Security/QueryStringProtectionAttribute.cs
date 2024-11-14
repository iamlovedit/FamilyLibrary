namespace GalaFamilyLibrary.Infrastructure.Security
{
    public class QueryStringProtectionAttribute(IDataProtectionHelper dataProtectionHelper) : ActionFilterAttribute
    {
        public string[] QueryStringParameters { get; set; } = Array.Empty<string>();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var parameter in QueryStringParameters)
            {
                if (context.ActionArguments.ContainsKey(parameter))
                {
                    var queryStringValue = context.HttpContext.Request.Query[parameter];
                    var decryptedValue = dataProtectionHelper.Decrypt(queryStringValue, nameof(QueryStringProtectionAttribute));
                    context.ActionArguments[parameter] = decryptedValue;
                }
            }
        }
    }
}
