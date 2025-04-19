using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLV.Business.Models;

namespace MLV.Business.Data.Mappings;

public sealed class VendedorMapping : IEntityTypeConfiguration<Vendedor>
{
    public void Configure(EntityTypeBuilder<Vendedor> builder)
    {
        builder.HasKey(c => c.Id);

        builder.ToTable("Vendedores");
    }
}
