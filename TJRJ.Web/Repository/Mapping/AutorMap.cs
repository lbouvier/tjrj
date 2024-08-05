using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using TJRJ.Entities;

namespace TJRJ.Web.Repository.Mapping
{
    public class AutorMap : IEntityTypeConfiguration<Autor>
    {
        public void Configure(EntityTypeBuilder<Autor> builder)
        {
            builder.ToTable("Autor");
            builder.Property(x => x.Nome).HasMaxLength(40);
            builder.HasKey(a => a.CodAu);
        }
    }
}
