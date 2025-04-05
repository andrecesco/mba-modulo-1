using MLV.Core.Data;
using System.ComponentModel.DataAnnotations;

namespace MLV.Business.Models;

public class Vendedor : Entity
{
    [Required]
    public string NomeFantasia { get; private set; }
    [Required]
    public string Email { get; private set; }
    
    public string RazaoSocial { get; private set; }

    public Vendedor(Guid id, string nomeFantasia, string email, string razaoSocial, string usuarioCriacao)
    {
        Id = id;
        NomeFantasia = nomeFantasia;
        Email = email;
        RazaoSocial = razaoSocial;
        UsuarioCriacao = usuarioCriacao;
    }

    internal void AtualizarDados(string nomeFantasia, string razaoSocial, string usuarioAlteracao)
    {
        NomeFantasia = nomeFantasia;
        RazaoSocial= razaoSocial;
        UsuarioAlteracao = usuarioAlteracao;
    }
}
