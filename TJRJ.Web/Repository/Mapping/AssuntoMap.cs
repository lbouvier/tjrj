using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using TJRJ.Entities;

namespace TJRJ.Web.Repository.Mapping
{
    public class AssuntoMap : IEntityTypeConfiguration<Assunto>
    {
        public void Configure(EntityTypeBuilder<Assunto> builder)
        {
            builder.ToTable("Assunto");
            builder.Property(x => x.Descricao).HasMaxLength(20);
            builder.HasKey(a => a.CodAs);
        }
    }
}
