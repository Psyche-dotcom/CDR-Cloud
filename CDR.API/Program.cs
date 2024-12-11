using AutoMapper;
using CDR.API.Api.Service.Implementation;
using CDR.API.Api.Service.Interface;
using CDR.API.AutoMapper.Profiles;
using CDR.API.Resources;
using CDR.Data.Concrete.EntityFramework.Contexts;
using CDR.Entities.Concrete;
using CDR.Services.AutoMapper;
using CDR.Services.Extensions;
using CDR.Shared.Utilities.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
}).AddDataAnnotationsLocalization(o =>
{
    var type = typeof(DtoResource);
    var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
    var factory = builder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
    var localizer = factory.Create("DtoResource", assemblyName.Name);
    o.DataAnnotationLocalizerProvider = (t, f) => localizer;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new UserProfile());
    cfg.AddProfile(new ReportFavoriteFilterProfile());
})
.CreateMapper());


builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Rest Authentication Schema",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },new string[]{ }
        }
     });
});
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(option =>
    {
        option.SaveToken = true;
        option.RequireHttpsMetadata = false;
        option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });
builder.Services.ConfigureWritable<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.ConfigureWritable<IyzicoSettings>(builder.Configuration.GetSection("IyzicoSettings"));
builder.Services.ConfigureWritable<CDR.Entities.Concrete.GlobalSettings>(builder.Configuration.GetSection("GlobalSettings"));

builder.Services.LoadServices(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
    });

});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGenerateJwt, GenerateJwt>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
var optionBuilder = new DbContextOptionsBuilder<CdrContext>();
optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{*/
    app.UseCors();
    app.UseSwagger();
    app.UseSwaggerUI();
/*}*/


IList<CultureInfo> supportedCultures = new List<CultureInfo>
                            {
                                new CultureInfo("en-US"),
                                new CultureInfo("tr-TR"),
                                new CultureInfo("nl-NL")
                            };

app.UseForwardedHeaders();
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
