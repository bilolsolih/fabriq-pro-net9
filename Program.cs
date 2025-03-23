using System.Text.Json.Serialization;
using FabriqPro;
using FabriqPro.Core;
using FabriqPro.Features.Authentication;
using FabriqPro.Features.Products;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers(options => options.Filters.Add<CoreExceptionFilters>())
  .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddNpgsql<FabriqDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.RegisterAuthenticationFeature();
builder.Services.RegisterProductsFeature();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

var uploadsDir = Path.Combine(builder.Environment.ContentRootPath, "uploads");

if (!Directory.Exists(uploadsDir))
{
  Directory.CreateDirectory(uploadsDir);
}

app.UseStaticFiles(
  new StaticFileOptions
  {
    FileProvider = new PhysicalFileProvider(uploadsDir),
    RequestPath = "/uploads"
  }
);

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.Run();