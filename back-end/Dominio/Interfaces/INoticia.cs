using Dominio.Interfaces.Genericos;
using Entidades.Entidades;
using System.Linq.Expressions;

namespace Dominio.Interfaces;

public interface ITarefa : IGenericos<Tarefa>
{
    Task<bool> ExcluirTarefasPorUsuarioId(string id);
    Task<List<Tarefa>> ListarTarefas(Expression<Func<Tarefa, bool>> exTarefa);
}
