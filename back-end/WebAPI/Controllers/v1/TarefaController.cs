using Aplicacao.Interfaces;
using Azure;
using Entidades.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
    [HttpGet("/Tarefa/List")]
    public async Task<Models.Response<List<TarefaModel>>> ListarTarefas()
    {
        var tarefas = await _aplicacaoTarefa.ListarTarefas();
        var tarefasViewModel = new List<TarefaModel>() { };
        for (int i = 0; i < tarefas.Count(); i++)
        {
            var tarefa = tarefas[i];

            var tarefaStatus = "To do";

            switch (tarefa.Status)
            {
                case 2:
                    tarefaStatus = "Doing";
                    break;
                case 3:
                    tarefaStatus = "Done";
                    break;
                default:
                    break;
            }

            var tarefaViewModel = new TarefaModel()
            {
                id = tarefa.Id,
                titulo = tarefa.Titulo,
                descricao = tarefa.Descricao,
                status = tarefaStatus,
                idUsuario = tarefa.UsuarioId
            };

            tarefaViewModel.SetarDataParaTela(tarefa.CriadoEm);

            tarefasViewModel.Add(tarefaViewModel);

        }

        var response = new Models.Response<List<TarefaModel>>();
        response.Result = tarefasViewModel;
        response.Mensagem = "Tarefas recuperadas com sucesso";

        return response;
    }

    [Authorize]
    [Produces("application/json")]
    [HttpGet("/Tarefa/{id}")]
    public async Task<Models.Response<TarefaModel>> ListarTarefaPorId(int id)
    {
        var tarefa = await _aplicacaoTarefa.ObterTarefaPorIdAsync(id);

        

        var tarefaStatus = "To do";

        switch (tarefa.Status)
        {
            case 2:
                tarefaStatus = "Doing";
                break;
            case 3:
                tarefaStatus = "Done";
                break;
            default:
                break;
        }

        var tarefaViewModel = new TarefaModel()
        {
            id = tarefa.Id,
            titulo = tarefa.Titulo,
            descricao = tarefa.Descricao,
            status = tarefaStatus,
            idUsuario = tarefa.UsuarioId
        };

        tarefaViewModel.SetarDataParaTela(tarefa.CriadoEm);


        var response = new Models.Response<TarefaModel>();
        response.Result = tarefaViewModel;
        response.Mensagem = "Tarefa recuperada com sucesso";

        return response;
    }



    [Authorize]
    [Produces("application/json")]
    [HttpPost("/Tarefa/Create")]
    public async Task<List<Entidades.Notificacoes.Notifica>> Create([FromBody] TarefaModel TarefaTarefa)
    {
        var novaTarefa = new Entidades.Entidades.Tarefa
        {
            Titulo = TarefaTarefa.titulo,
            Descricao = TarefaTarefa.descricao
        };

        var idUsuario = JwtSecurityKey.GetIdFromUserSession(User);
        novaTarefa.UsuarioId = idUsuario;

        await _aplicacaoTarefa.AdicionarTarefa(novaTarefa);

        return novaTarefa.Notificacoes;
    }




    [Authorize]
    [Produces("application/json")]
    [HttpPut("/Tarefa")]
    public async Task<List<Entidades.Notificacoes.Notifica>> Update([FromBody] TarefaModel TarefaTarefa)
    {
        var novaTarefa = await _aplicacaoTarefa.BuscarPorId(TarefaTarefa.id);
        novaTarefa.Titulo = TarefaTarefa.titulo;
        novaTarefa.Descricao = TarefaTarefa.descricao;

        var idUsuario = JwtSecurityKey.GetIdFromUserSession(User);

        novaTarefa.UsuarioId = idUsuario;
        await _aplicacaoTarefa.AtualizaTarefa(novaTarefa);
        return novaTarefa.Notificacoes;
    }


    [Authorize]
    [Produces("application/json")]
    [HttpDelete("/Tarefa/{id}")]
    public async Task<List<Entidades.Notificacoes.Notifica>> Delete(int id)
    {
        var tarefa = await _aplicacaoTarefa.ObterTarefaPorIdAsync(id);
        await _aplicacaoTarefa.Excluir(tarefa);
        return tarefa.Notificacoes;
    }

}
