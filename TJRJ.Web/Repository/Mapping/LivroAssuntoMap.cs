using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using TJRJ.Entities;

namespace TJRJ.Web.Repository.Mapping
{
    public class LivroAssuntoMap : IEntityTypeConfiguration<LivroAssunto>
    {
        public void Configure(EntityTypeBuilder<LivroAssunto> builder)
        {
            builder.ToTable("LivroAssunto");
            builder.HasKey(la => new { la.LivroCod, la.AssuntoCodAs });

            builder.HasOne(la => la.Livro)
               .WithMany(l => l.LivroAssuntos)
               .HasForeignKey(la => la.LivroCod);

            builder.HasOne(la => la.Assunto)
                .WithMany(a => a.LivroAssuntos)
                .HasForeignKey(la => la.AssuntoCodAs);
        }
    }
}
