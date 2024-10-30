using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PaymentSystem.WebApi;
using PaymentSystem.WebApi.Database;
using PaymentSystem.WebApi.Mappers;
using PaymentSystem.WebApi.Utils;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Repository
builder.Services.AddRepositories();

//Services
builder.Services.AddServices();

builder.Services.AddHttpContextAccessor();
builder.Services.AddDatabase();
builder.Services.AddControllers();

var policyName = "_MyAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
        policy =>
        {
            policy.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

//Mappers
builder.Services.AddAutoMapper(typeof(OrderProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger Documentation (Display the locker icon on Authorize endpoints and Authorize button with explanation)
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentSystem.WebApi", Version = "v1" });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization Header - Used with Bearer Authentication.\r\n\r\n" +
            "Type 'Bearer' [space] then your token, as field bellow.\r\n\r\n" +
            "Example (type without quotes): 'ey8357fhasf203t8ik0...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });

    opt.OperationFilter<SwaggerOperationFilter>();

    var filePath = Path.Combine(AppContext.BaseDirectory, "PaymentSystem.WebApi.xml");
    opt.IncludeXmlComments(filePath);
});

// Add authentication to validate jwt token
var secretToken = Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Security:Secret").Value ??
    throw new InvalidOperationException("Secret token was not found in appsettings"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretToken),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

builder.Services.AddLogging(builder => { builder.AddConsole(); });

//builder.WebHost.UseUrls("http://127.0.0.1:5228");

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.AddDefaultMongoData(builder.Services);

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(policyName);

app.MapControllers();

app.Run();
