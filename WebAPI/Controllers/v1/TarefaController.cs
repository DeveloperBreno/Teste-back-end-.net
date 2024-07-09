using Aplicacao.Interfaces;
using Entidades.Entidades;
using Entidades.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Token;

namespace WebAPI.Controllers.v1;

[Route("api/[controller]")]
[ApiController]
public class TarefaController : ControllerBase
{
    private readonly IAplicacaoTarefa _aplicacaoTarefa;
    private readonly IAplicacaoUsuario _IAplicacaoUsuario;

    public TarefaController(IAplicacaoTarefa aplicacaoTarefa, IAplicacaoUsuario iAplicacaoUsuario)
    {
        _aplicacaoTarefa = aplicacaoTarefa;
        _IAplicacaoUsuario = iAplicacaoUsuario;
    }


    [Authorize]
    [Produces("application/json")]
    [HttpPost("/Tarefa/List")]
    public async Task<List<Tarefa>> ListarTarefas()
    {
        return await _aplicacaoTarefa.ListarTarefas();
    }


    [Authorize]
    [Produces("application/json")]
    [HttpPost("/Tarefa/Create")]
    public async Task<List<Entidades.Notificacoes.Notifica>> Create([FromBody] TarefaModel TarefaTarefa)
    {
        var novaTarefa = new Entidades.Entidades.Tarefa
        {
            Titulo = TarefaTarefa.Titulo,
            Descricao = TarefaTarefa.Descricao
        };

        var email = JwtSecurityKey.GetEmailFromUserSession(User);
        var idUsuario = await _IAplicacaoUsuario.RetornaIdUsuario(email);
        novaTarefa.UsuarioId = idUsuario;

        await _aplicacaoTarefa.AdicionarTarefa(novaTarefa);

        return novaTarefa.Notificacoes;
    }


    [Authorize]
    [Produces("application/json")]
    [HttpPost("/Update")]
    public async Task<List<Entidades.Notificacoes.Notifica>> Update([FromBody] TarefaModel TarefaTarefa)
    {
        var novaTarefa = await _aplicacaoTarefa.BuscarPorId(TarefaTarefa.id);
        novaTarefa.Titulo = TarefaTarefa.Titulo;
        novaTarefa.Descricao = TarefaTarefa.Descricao;

        var email = JwtSecurityKey.GetEmailFromUserSession(User);
        var idUsuario = await _IAplicacaoUsuario.RetornaIdUsuario(email);

        novaTarefa.UsuarioId = idUsuario;
        await _aplicacaoTarefa.AtualizaTarefa(novaTarefa);
        return novaTarefa.Notificacoes;
    }


    [Authorize]
    [Produces("application/json")]
    [HttpPost("/Delete")]
    public async Task<List<Entidades.Notificacoes.Notifica>> Delete([FromBody] TarefaModel TarefaTarefa)
    {
        var novaTarefa = await _aplicacaoTarefa.BuscarPorId(TarefaTarefa.id);
        await _aplicacaoTarefa.Excluir(novaTarefa);
        return novaTarefa.Notificacoes;
    }

}
