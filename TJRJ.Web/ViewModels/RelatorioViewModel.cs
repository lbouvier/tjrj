using Microsoft.EntityFrameworkCore;

namespace TJRJ.ViewModels
{
    [Keyless]
    public class RelatorioViewModel
    {
        
        public RelatorioViewModel() 
        {
            
        }

        public int AutorCodigo { get; set; }
        public string AutorNome { get; set; }
        public int LivroCodigo { get; set; }
        public string LivroNome { get; set; }
        public string Assuntos { get; set; }
    }
}
