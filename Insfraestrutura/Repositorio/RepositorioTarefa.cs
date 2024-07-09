﻿using Dominio.Interfaces;
using Entidades.Entidades;
using Insfraestrutura.Configuracoes;
using Insfraestrutura.Repositorio.Genericos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Insfraestrutura.Repositorio;

public class RepositorioTarefa : RepositorioGenerico<Tarefa>, ITarefa
{
    private readonly Contexto _context;

    public RepositorioTarefa()
    {
        _context = new Contexto(new DbContextOptionsBuilder<Contexto>().Options);
    }

    public async Task<bool> ExcluirTarefasPorUsuarioId(string id)
    {
        var tarefas = await _context.Tarefas.Where(o => o.UsuarioId == id).ToListAsync();

        foreach (var tarefa in tarefas)
        {
            _context.Tarefas.Remove(tarefa);
        }

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Tarefa>> ListarTarefas(Expression<Func<Tarefa, bool>> exTarefa)
    {
        return await _context.Tarefas.Where(exTarefa).ToListAsync();
    }
}