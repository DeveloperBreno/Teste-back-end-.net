using Insfraestrutura.Configuracoes;
using Microsoft.EntityFrameworkCore;
using Dominio.Interfaces.Genericos;
using Insfraestrutura.Repositorio.Genericos;
using Dominio.Interfaces;
using Insfraestrutura.Repositorio;
using Dominio.Interfaces.InterfaceServicos;
using Dominio.Servicos;
using Entidades.Entidades;
using Aplicacao.Interfaces;
using Aplicacao.Aplicacoes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Token;
using Dominio.Interfaces.Filas;
using Insfraestrutura.Filas;
using RabbitMQ.Client;
using WebAPI.Controllers.v1;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});

// Adiciona a pol�tica CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Recupera a string de conex�o do appsettings.json
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adiciona o DbContext ao cont�iner de servi�os
builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlServer(connectionString));

// Adiciona os servi�os de Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<Contexto>()
    .AddDefaultTokenProviders();

// Interface e reposit�rio
builder.Services.AddScoped(typeof(IGenericos<>), typeof(RepositorioGenerico<>));
builder.Services.AddScoped(typeof(ITarefa), typeof(RepositorioTarefa));
builder.Services.AddScoped(typeof(IUsuario), typeof(RepositorioUsuario));

// Servi�o dom�nio
builder.Services.AddScoped<IServicoTarefa, ServicoTarefa>();

// Interface aplica��o
builder.Services.AddScoped<IAplicacaoTarefa, AplicacaoTarefa>();
builder.Services.AddScoped<IAplicacaoUsuario, AplicacaoUsuario>();


// logs
builder.Services.AddSingleton(typeof(ILogger), typeof(Logger<UsuarioController>));

builder.Services.AddSignalR();

// Configurar RabbitMQ
builder.Services.AddSingleton<IConnection>(sp =>
{
    var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
    var factory = new ConnectionFactory
    {
        HostName = rabbitMQSettings.HostName,
        Port = rabbitMQSettings.Port,
        UserName = rabbitMQSettings.UserName,
        Password = rabbitMQSettings.Password
    };
    return factory.CreateConnection();
});

builder.Services.AddScoped<IInsereNaFila, InserirNaFila>();

// JWT
// muda no usuarioController tambem
var key = "Secret_Key-12345678_Secret_Key-12345678";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Teste.Securiry.Bearer",
            ValidAudience = "Teste.Securiry.Bearer",
            IssuerSigningKey = JwtSecurityKey.Create(key)
        };

        option.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddLogging();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAll");

app.UseRequestTimeout(TimeSpan.FromSeconds(30));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


public class RabbitMQSettings
{
    public string HostName { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}