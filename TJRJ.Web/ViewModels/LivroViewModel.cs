
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TJRJ.Entities;

namespace TJRJ.ViewModels
{
    public class LivroViewModel
    {
        public LivroViewModel()
        {
            LivroAutores = new HashSet<LivroAutorViewModel>();
            LivroAssuntos = new HashSet<LivroAssuntoViewModel>();
        }

        public int Cod { get; set; }

        [Required(ErrorMessage = "Informe o título do livro")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Informe a editora do livro")]
        [Display(Name = "Editora")]
        public string Editora { get; set; }

        [Required(ErrorMessage = "Informe a edição do livro")]
        [Display(Name = "Edição")]
        public int? Edicao { get; set; }

        [Required(ErrorMessage = "Informe o preço do livro")]
        [Display(Name = "Preço")]
        public decimal? Preco { get; set; }

        [Required(ErrorMessage = "Informe o ano de publicação do livro")]
        [Display(Name = "Ano de Publicação")]
        public string AnoPublicacao { get; set; }
        
        [Required]
        public ICollection<LivroAutorViewModel> LivroAutores { get; set; }
        
        [Required]
        public ICollection<LivroAssuntoViewModel> LivroAssuntos { get; set; }
    }
}
