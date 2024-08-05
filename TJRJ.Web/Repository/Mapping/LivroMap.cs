using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using TJRJ.Entities;

namespace TJRJ.Web.Repository.Mapping
{
    public class LivroMap : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder.ToTable("Livro");
            builder.HasKey(a => a.Cod);
            builder.Property(x => x.Titulo).HasMaxLength(40);
            builder.Property(x => x.Editora).HasMaxLength(40);
            builder.Property(x => x.Preco).HasPrecision(18,2);
        }
    }
}
