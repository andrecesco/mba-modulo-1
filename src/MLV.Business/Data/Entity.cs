using System.ComponentModel.DataAnnotations;

namespace MLV.Business.Data;
public abstract class Entity
{
    [Required]
    public Guid Id { get; protected set; }
    [Required]
    public DateTime CriadoEm { get; protected set; }
    [Required]
    public string UsuarioCriacao { get; protected set; }
    public DateTime? AtualizadoEm { get; protected set; }
    public string UsuarioAlteracao { get; protected set; }
}
