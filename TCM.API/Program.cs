var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddJWTAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}
// Attach BasicAuthMiddleware only for login
app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/api/Authentication/Login"),
    appBuilder => appBuilder.UseMiddleware<BasicAuthMiddleware>()
);
app.UseExceptionHandler(options => { });
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
