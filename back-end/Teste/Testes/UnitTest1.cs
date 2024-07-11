using Aplicacao.Aplicacoes;
using Aplicacao.Interfaces;
using Dominio.Interfaces;
using Dominio.Interfaces.Filas;
using Dominio.Interfaces.InterfaceServicos;
using Dominio.Servicos;
using Entidades.Entidades;
using Insfraestrutura.Filas;
using Insfraestrutura.Repositorio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;


namespace Testes;

// Configuração de injeção de dependência para os testes
public static class TestDependencyInjection
{
    public static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        // Carregar configurações do appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        // Serviço de notícia
        services.AddScoped<IServicoTarefa, ServicoTarefa>();
        services.AddScoped<ITarefa, RepositorioTarefa>();

        // Configuração RabbitMQ
        var rabbitMQSettings = configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
        services.AddSingleton<IConnection>(sp =>
        {
            var factory = new ConnectionFactory
            {
                HostName = rabbitMQSettings.HostName,
                UserName = rabbitMQSettings.UserName,
                Password = rabbitMQSettings.Password
            };
            return factory.CreateConnection();
        });

        services.AddScoped<IInsereNaFila, InserirNaFila>();

        // Interface aplicação de usuário
        services.AddScoped<IAplicacaoUsuario, AplicacaoUsuario>();
        services.AddScoped<IUsuario, RepositorioUsuario>();
        services.AddScoped(typeof(ITarefa), typeof(RepositorioTarefa));

        services.AddScoped<IAplicacaoTarefa, AplicacaoTarefa>();
        services.AddScoped<IAplicacaoUsuario, AplicacaoUsuario>();

        return services.BuildServiceProvider();
    }
}

public class RabbitMQSettings
{
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}

// Testes unitários usando MSTest
[TestClass]
public class ServicoNoticiaTests
{
    private readonly IServicoTarefa _servicoTarefa;
    private readonly IAplicacaoUsuario _aplicacaoUsuario;
    private readonly IUsuario _usuario;

    private readonly string emailUserTest = "userteste@gmail.com";

    public ServicoNoticiaTests()
    {
        var serviceProvider = TestDependencyInjection.BuildServiceProvider();
        _servicoTarefa = serviceProvider.GetRequiredService<IServicoTarefa>();
        _aplicacaoUsuario = serviceProvider.GetRequiredService<IAplicacaoUsuario>();
        _usuario = serviceProvider.GetRequiredService<IUsuario>();
    }

    [TestMethod]
    public async Task ExecutarTestesEmSequenciaAsync()
    {
        await CriaUsuarioAsync();
        await ListarUsuariosAsync();
        await CriarTarefaAsync();
        await ExcluirNoticiaAsync();
        await RemoveUsuarioAsync();
    }

    private async Task CriaUsuarioAsync()
    {
        var result = await _aplicacaoUsuario.AdicionarUsuario(emailUserTest, "123456", DateTime.UtcNow, "11977300114", "Teste");

        // Assert
        Assert.IsTrue(result);
    }

    private async Task ListarUsuariosAsync()
    {
        var result = await _aplicacaoUsuario.RetornaIdUsuario(emailUserTest);

        // Assert
        Assert.IsTrue(!string.IsNullOrWhiteSpace(result));
    }



    private async Task CriarTarefaAsync()
    {
        var id = await _aplicacaoUsuario.RetornaIdUsuario(emailUserTest);

        var noticia = new Tarefa()
        {
            Titulo = "Noticia Teste",
            Status = 1,
            CriadoEm = DateTime.UtcNow,
            CriadoPor = "Teste",
            Descricao = "Teste",
            Mensagem = "Teste",
            UsuarioId = id,
            AlteradoEm = DateTime.UtcNow,
        };

        await _servicoTarefa.AdicionarTarefa(noticia);

        // Assert
        Assert.IsTrue(true);
    }

    private async Task ExcluirNoticiaAsync()
    {
        var id = await _aplicacaoUsuario.RetornaIdUsuario(emailUserTest);
        var result = await _servicoTarefa.ExcluirTarefasPorUsuarioId(id);

        // Assert
        Assert.IsTrue(result);
    }


    private async Task RemoveUsuarioAsync()
    {
        var id = await _aplicacaoUsuario.RetornaIdUsuario(emailUserTest);

        Assert.IsTrue(!string.IsNullOrWhiteSpace(id));

        var result = await _aplicacaoUsuario.RemoverUsuarioPorId(id);

        // Assert
        Assert.IsTrue(result);
    }

}
