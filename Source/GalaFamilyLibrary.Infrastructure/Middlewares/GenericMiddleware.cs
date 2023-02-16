using GalaFamilyLibrary.Infrastructure.Consul;
using GalaFamilyLibrary.Infrastructure.Cors;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace GalaFamilyLibrary.Infrastructure.Middlewares
{
    public static class GenericMiddleware
    {
        public static void UseGeneric(this WebApplication app)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseVersionedSwaggerUI();
            }

            //app.UseConsul(app.Configuration);

            app.UseCorsService();

            //app.UseAuthentication(); //ocelot已验证 微服务无需验证

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}