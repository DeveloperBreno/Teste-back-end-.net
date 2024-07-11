using Aplicacao.Interfaces;
using Dominio.Interfaces;
using Dominio.Interfaces.InterfaceServicos;
using Entidades.Entidades;
using System.Formats.Tar;

namespace Aplicacao.Aplicacoes;

public class AplicacaoTarefa : IAplicacaoTarefa
{

    ITarefa _ITarefa;
    IServicoTarefa _IServicoTarefa;

    public AplicacaoTarefa(ITarefa ITarefa, IServicoTarefa IServicoTarefa)
    {
        _ITarefa = ITarefa;
        _IServicoTarefa = IServicoTarefa;
    }

    public async Task AdicionarTarefa(Tarefa tarefa)
    {
        await _IServicoTarefa.AdicionarTarefa(tarefa);
    }

    public async Task AtualizaTarefa(Tarefa tarefa)
    {
        await _IServicoTarefa.AtualizaTarefa(tarefa);
    }
    public async Task<List<Tarefa>> ListarTarefasAtivas()
    {
        return await _IServicoTarefa.ListarTarefas();
    }
    public async Task Adicionar(Tarefa obj)
    {
        await _ITarefa.Adicionar(obj);
    }
    public async Task Atualizar(Tarefa obj)
    {
        await _ITarefa.Atualizar(obj);
    }
    public async Task<Tarefa> BuscarPorId(int Id)
    {
        return await _ITarefa.BuscarPorId(Id);
    }
    public async Task Excluir(Tarefa obj)
    {
        await _ITarefa.Excluir(obj);
    }
    public async Task<List<Tarefa>> Listar(Tarefa obj)
    {
        return await _ITarefa.Listar();
    }

    public async Task<List<Tarefa>> ListarTarefas()
    {
        return await _ITarefa.Listar();
    }

    public async Task<Tarefa> ObterTarefaPorIdAsync(int id)
    {
        return await _ITarefa.BuscarPorId(id);

    }

}