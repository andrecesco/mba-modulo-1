using MLV.Business.Data;
using System.ComponentModel.DataAnnotations;

namespace MLV.Business.Models;

public class Categoria : Entity
{
    [Required]
    public string Nome { get; private set; }

    public Categoria(Guid id, string nome, string usuarioCriacao)
    {
        Id = id;
        Nome = nome.Trim();
        UsuarioCriacao = usuarioCriacao;
    }

    public void AtualizarNome(string nome, string usuarioLogado)
    {
        Nome = nome.Trim();
        UsuarioAlteracao = usuarioLogado;
    }
}
