using Aplicacao.Interfaces.Genericos;
using Entidades.Entidades;
namespace Aplicacao.Interfaces;

public interface IAplicacaoTarefa : IGenericaAplicacoes<Tarefa>
{
    Task AdicionarTarefa(Tarefa tarefa);
    Task AtualizaTarefa(Tarefa tarefa);
    Task<List<Tarefa>> ListarTarefas();
    Task<Tarefa> ObterTarefaPorIdAsync(int id);
}
