using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TJRJ.Entities;

namespace TJRJ.ViewModels
{
    public class AssuntoViewModel
    {
        public AssuntoViewModel()
        {
            LivroAssuntos = new HashSet<LivroAssuntoViewModel>();
        }
        public int CodAs { get; set; }
        
        [Required(ErrorMessage = "Informe uma descrição")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
       
        [Required]
        public ICollection<LivroAssuntoViewModel> LivroAssuntos { get; set; }
    }
}
