using MangaApi.Data;
using MangaApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configuración de la cadena de conexión para ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(connectionString));

builder.Services.AddControllers(); // Registra los controladores

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Habilita Swagger para la API

var app = builder.Build();

// Configurar el middleware de Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
