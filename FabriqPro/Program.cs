using System.Net;
using System.Text.Json.Serialization;
using FabriqPro;
using FabriqPro.Core;
using FabriqPro.Features.Authentication;
using FabriqPro.Features.Products;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
  {
    options.Limits.MaxRequestBodySize = int.MaxValue;
    options.Listen(IPAddress.Parse("0.0.0.0"), 8888);
  }
);

builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers(options => options.Filters.Add<CoreExceptionFilters>())
  .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSwaggerGen(options =>
  {
    options.AddSecurityDefinition(
      "Bearer",
      new OpenApiSecurityScheme
      {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
      }
    );
    options.AddSecurityRequirement(
      new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme()
          {
            Reference = new OpenApiReference()
            {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer",
            }
          },
          Array.Empty<string>()
        }
      }
    );
  }
);
builder.Services.AddHttpContextAccessor();

builder.Services.AddNpgsql<FabriqDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.RegisterAuthenticationFeature(builder.Configuration);
builder.Services.RegisterProductsFeature();

builder.Services.AddCors(options =>
  {
    options.AddPolicy(
      "AllowAll",
      policy => policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
  }
);

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

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.Run();
public partial class Program{}