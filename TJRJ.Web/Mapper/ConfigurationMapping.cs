using AutoMapper;
using TJRJ.Entities;
using TJRJ.ViewModels;

namespace TJRJ.Mapper
{
    public class ConfigurationMapping : Profile
    {
        public ConfigurationMapping()
        {

            var config = new MapperConfiguration(cfg =>
            {
                CreateMap<Livro, LivroViewModel>();
                CreateMap<LivroViewModel, Livro>();

                CreateMap<Assunto, AssuntoViewModel>();
                CreateMap<AssuntoViewModel, Assunto>();

                CreateMap<Autor, AutorViewModel>();
                CreateMap<AutorViewModel, Autor>();

                CreateMap<LivroAssunto, LivroAssuntoViewModel>();
                CreateMap<LivroAssuntoViewModel, LivroAssunto>();

                CreateMap<LivroAutor, LivroAutorViewModel>();
                CreateMap<LivroAutorViewModel, LivroAutor>();

            });
            IMapper mapper = config.CreateMapper();
        }
    }
}
