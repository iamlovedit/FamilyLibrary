namespace GalaFamilyLibrary.Infrastructure.Security
{
    public class RequestEncryptAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {

            base.OnResultExecuting(context);
        }
    }
}
