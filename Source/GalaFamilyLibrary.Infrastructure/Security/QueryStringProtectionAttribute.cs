using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GalaFamilyLibrary.Infrastructure.Security
{
    public class QueryStringProtectionAttribute : ActionFilterAttribute
    {
        private readonly IDataProtectionHelper _dataProtectionHelper;
        public string[] QueryStringParameters { get; set; }
        public QueryStringProtectionAttribute(IDataProtectionHelper dataProtectionHelper)
        {
            _dataProtectionHelper = dataProtectionHelper;
            QueryStringParameters = Array.Empty<string>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var parameter in QueryStringParameters)
            {
                if (context.ActionArguments.ContainsKey(parameter))
                {
                    var queryStringValue = context.HttpContext.Request.Query[parameter];
                    var decryptedValue = _dataProtectionHelper.Decrypt(queryStringValue, nameof(QueryStringProtectionAttribute));
                    context.ActionArguments[parameter] = decryptedValue;
                }
            }
        }
    }
}
