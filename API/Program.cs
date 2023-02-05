using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<StoreContext>(options=>options.UseSqlite(
    builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();    
    app.UseSwaggerUI();
}

app.UseCors("corsapp");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

//var host = WebApplication.CreateBuilder(args).Build();

//using var scope = app.Services.CreateScope();
//var services = scope.ServiceProvider;
//var context = services.GetRequiredService<StoreContext>();
//var logger = services.GetRequiredService<ILogger<Program>>();
//try
//{
//    await context.Database.MigrateAsync();
//    await StoreContextSeed.SeedAsync(context);
//} 
//catch(Exception ex)
//{
//    logger.LogError(ex, "An error occured during migration");

//}
//host.Run();

app.Run();
