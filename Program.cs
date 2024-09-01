using AutoMapper;
using quasitekWeb.Models;
using quasitekWeb.Data;
using quasitekWeb.Interface;
using quasitekWeb.Repository;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using quasitekWeb.Helper.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Register services for controllers with views
builder.Services.AddControllersWithViews();

// Register AutoMapper with the MappingProfile
builder.Services.AddAutoMapper(typeof(Mappers));

// Register the repository with the dependency injection container
builder.Services.AddScoped<IClassesRepository, ClassesRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();

// Register the database context with the dependency injection container 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quasitek API",
        Version = "v1",
        Description = "API documentation for Quasitek Web Application"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Enable middleware to serve generated Swagger as a JSON endpoint
app.UseSwagger();

// Enable middleware to serve Swagger UI
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quasitek API V1");
    c.RoutePrefix = string.Empty; // To serve Swagger UI at the app's root (http://localhost:<port>/)
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();  // Ensure that API controllers are mapped

app.Run();
