using Microsoft.EntityFrameworkCore;

namespace TJRJ.ViewModels
{
    public class RelatorioViewModel
    {
        
        public RelatorioViewModel() 
        {
            
        }

        public int Id { get; set; }
        public int AutorCodigo { get; set; }
        public string AutorNome { get; set; }
        public int LivrosEscritos { get; set; }
        public string Livros { get; set; }
        public string Assuntos { get; set; }
        public decimal TotalPreco { get; set; }
    }
}
