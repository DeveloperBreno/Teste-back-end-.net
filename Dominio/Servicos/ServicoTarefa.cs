using Dominio.Interfaces;
using Dominio.Interfaces.InterfaceServicos;

using Entidades.Entidades;

namespace Dominio.Servicos;

public class ServicoTarefa : IServicoTarefa
{
    private readonly ITarefa _Itarefa;

    public ServicoTarefa(ITarefa itarefa)
    {
        _Itarefa = itarefa;
    }

    private bool Validar(Tarefa tarefa)
    {
        var validarTitulo = tarefa.ValidarPropriedadesString(tarefa.Titulo, "Titulo");
        var validarInformacao = tarefa.ValidarPropriedadesString(tarefa.Descricao, "Informacao");

        if (validarTitulo && validarInformacao)
        {
            return true;
        }
        return false;
    }

    public async Task AdicionarTarefa(Tarefa tarefa)
    {
        if (Validar(tarefa))
        {
            tarefa.CriadoPor = "mudar!";
            tarefa.CriadoEm = DateTime.Now;
            tarefa.Status = 1;
            await _Itarefa.Adicionar(tarefa);
        }
    }

    public async Task AtualizaTarefa(Tarefa tarefa)
    {
        if (Validar(tarefa))
        {
            tarefa.AlteradoPor = "mudar!";
            tarefa.AlteradoEm = DateTime.Now;
            await _Itarefa.Atualizar(tarefa);
        }
    }

    public async Task<List<Tarefa>> ListarTarefas()
    {
        return await _Itarefa.ListarTarefas(n => n.ExcluidoEm.Equals(null));
    }


    public async Task<bool> ExcluirTarefasPorUsuarioId(string id)
    {
        return await _Itarefa.ExcluirTarefasPorUsuarioId(id);
    }
}