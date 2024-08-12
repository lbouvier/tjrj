using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TJRJ.Entities;
using TJRJ.ViewModels;

namespace TJRJ.Repository.Mapping
{
    public class RelatorioMap : IEntityTypeConfiguration<RelatorioViewModel>
    {
        public void Configure(EntityTypeBuilder<RelatorioViewModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToView("vwrelatorioautores");
        }
    }
}
