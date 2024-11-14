namespace GalaFamilyLibrary.Infrastructure.Security
{
    public class RequestDecryptAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}
