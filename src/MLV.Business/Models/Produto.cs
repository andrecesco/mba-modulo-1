using Microsoft.AspNetCore.Http;
using MLV.Core.Data;
using System.ComponentModel.DataAnnotations;

namespace MLV.Business.Models;

public class Produto : Entity
{
    [Required]
    public Guid CategoriaId { get; private set; }
    [Required]
    public Guid VendedorId { get; private set; }
    [Required]
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public decimal Valor {  get; private set; }
    public int Estoque { get; private set; }
    public string CaminhoImagem { get; private set; }

    //EF Relations
    public Categoria Categoria { get; set; }
    public Vendedor Vendedor { get; set; }

    public Produto(Guid id, Guid categoriaId, Guid vendedorId, string nome, string descricao, decimal valor, int estoque, string caminhoImagem, string usuarioCriacao)
    {
        Id = id;
        CategoriaId = categoriaId;
        VendedorId = vendedorId;
        Nome = nome.Trim();
        Descricao = descricao.Trim();
        Valor = valor;
        Estoque = estoque;
        CaminhoImagem = caminhoImagem;
        UsuarioCriacao = usuarioCriacao;
    }

    public void AtualizarProduto(Guid categoriaId, string nome, string descricao, decimal valor, int estoque, string caminhoImagem, string usuarioAlteracao)
    {
        CategoriaId = categoriaId;
        Nome = nome.Trim();
        Descricao = descricao.Trim();
        Valor = valor;
        Estoque = estoque;
        CaminhoImagem = caminhoImagem;
        UsuarioAlteracao = usuarioAlteracao;
    }
}
