using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TJRJ.Entities;

namespace TJRJ.ViewModels
{
    public class AutorViewModel
    {
        public AutorViewModel()
        {
            LivroAutores = new HashSet<LivroAutorViewModel>();
        }
        public int CodAu { get; set; }

        [Required(ErrorMessage = "Informe o nome do Autor")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
       
        [Required]
        public ICollection<LivroAutorViewModel> LivroAutores { get; set; }
    }
}
