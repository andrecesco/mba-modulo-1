using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLV.Business.Models;

namespace MLV.Infra.Data.Mappings;

public sealed class ProdutoMapping : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(e => e.Valor)
            .HasColumnType("decimal(38,2)")
            .IsRequired();

        builder.Property(e => e.Descricao)
            .HasColumnType("varchar(2048)");

        builder.ToTable("Produtos");
    }
}
