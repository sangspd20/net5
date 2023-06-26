using Microsoft.AspNetCore.Builder;

namespace F88.Digital.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "F88.Digital.Api");
                options.RoutePrefix = "swagger";
                options.DisplayRequestDuration();
            });
        }

        public static void UseCorsConfig(this IApplicationBuilder app)
        {
            app.UseCors("CorsPolicy");
        }
    }
}