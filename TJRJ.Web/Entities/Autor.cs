using System.Collections.Generic;

namespace TJRJ.Entities
{
    public class Autor //: EntidadeBase
    {
        public Autor()
        {
            LivroAutores = new HashSet<LivroAutor>();
        }
        public int CodAu { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<LivroAutor> LivroAutores { get; set; }
    }
}
