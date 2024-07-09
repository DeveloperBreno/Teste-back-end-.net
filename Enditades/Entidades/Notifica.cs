using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entidades.Notificacoes;

namespace Entidades.Entidades;

[Table("TB_TAREFA")]
public class Tarefa : Notificacoes.Notifica
{

    [Column("Titulo")]
    public string Titulo { get; set; }

    [Column("Descricao")]
    public string Descricao { get; set; }

    [Column("Status")]
    public int Status { get; set; }

    [Column("UsuarioId")]
    public string UsuarioId { get; set; }

    // Propriedade de navegação para ApplicationUser
    [ForeignKey("UsuarioId")]
    public ApplicationUser Usuario { get; set; }



}
