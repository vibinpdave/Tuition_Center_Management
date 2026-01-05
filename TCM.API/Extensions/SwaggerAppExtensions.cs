namespace TCM.API.Extensions
{
    public static class SwaggerAppExtensions
    {
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TCM API v1");
            });

            return app;
        }
    }
}
