using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.Middleware;
using WebApplication1.Repositories;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

//servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

//Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
//AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

//Repositorios
builder.Services.AddScoped<IAutoRepositorio, AutoRepositorio>();
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();

//Servicios
builder.Services.AddScoped<AutoService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtHelper>();

//UsuarioContextoService
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UsuarioContextoService>();

builder.Services.AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetSection("JwtSettings"))
    .ValidateOnStart(); // Valida que la configuración JWT sea correcta al iniciar la app
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

builder.Services.AddAuthentication(options =>
{
    // Esquema por defecto — JWT
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Validaciones del token
        ValidateIssuer = true,  // verifica quién emitió el token
        ValidateAudience = true,  // verifica para quién es el token
        ValidateLifetime = true,  // verifica que no esté expirado
        ValidateIssuerSigningKey = true,  // verifica la firma

        // Valores esperados — vienen de appsettings.json
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
                               Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
    options.Events = JwtEventsHelper.ObtenerEventos();
});

// Autorización por roles
builder.Services.AddAuthorization();


var app = builder.Build();
// Debe ser la PRIMERA línea después de Build()
// para interceptar todos los errores
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())  app.MapOpenApi();


app.UseHttpsRedirection();
// IMPORTANTE — Authentication antes que Authorization
app.UseAuthentication(); // ← verifica el token
app.UseAuthorization();  // ← verifica los permisos
app.MapControllers();
app.Run();
