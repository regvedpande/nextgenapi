using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StoreApi.Filters;
using StoreApi.Middleware;
using StoreApi.Services.Implementations;
using StoreApi.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Read secret key from appsettings.json
var secretKey = builder.Configuration["Jwt:SecretKey"];
// Convert from base64
var key = Convert.FromBase64String(secretKey);

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Add logging
builder.Services.AddLogging();

// Register services with DI
builder.Services.AddScoped<IStoreService>(provider =>
    new StoreService(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAuthService>(provider =>
    new AuthService(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserService>(provider =>
    new UserService(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ValidateModelAttribute>();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Store API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
         new OpenApiSecurityScheme
         {
           Reference = new OpenApiReference
           {
             Type = ReferenceType.SecurityScheme,
             Id = "Bearer"
           }
         },
         new string[] { }
       }
    });
});

var app = builder.Build();

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable Authentication/Authorization
app.UseAuthentication();
app.UseAuthorization();

// Add custom request logging middleware
app.UseMiddleware<RequestLoggingMiddleware>();

// Map controllers
app.MapControllers();

app.Run();
