using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using TJRJ.Entities;

namespace TJRJ.Web.Repository.Mapping
{
    public class LivroAutorMap : IEntityTypeConfiguration<LivroAutor>
    {
        public void Configure(EntityTypeBuilder<LivroAutor> builder)
        {
            builder.ToTable("LivroAutor");
            builder.HasKey(la => new { la.LivroCod, la.AutorCodAu });

            builder.HasOne(la => la.Livro)
            .WithMany(l => l.LivroAutores)
            .HasForeignKey(la => la.LivroCod);

            builder.HasOne(la => la.Autor)
                .WithMany(a => a.LivroAutores)
                .HasForeignKey(la => la.AutorCodAu);
        }
    }
}
