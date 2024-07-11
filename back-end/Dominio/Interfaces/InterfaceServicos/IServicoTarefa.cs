using Entidades.Entidades;

namespace Dominio.Interfaces.InterfaceServicos;

public interface IServicoTarefa
{
    Task AdicionarTarefa(Tarefa tarefa);
    Task AtualizaTarefa(Tarefa tarefa);
    Task<bool> ExcluirTarefasPorUsuarioId(string id);
    Task<List<Tarefa>> ListarTarefas();
}
