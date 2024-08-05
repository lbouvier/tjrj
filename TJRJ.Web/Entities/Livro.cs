using System.Collections.Generic;

namespace TJRJ.Entities
{
    public class Livro //: EntidadeBase
    {
        public Livro()
        {
            LivroAutores = new HashSet<LivroAutor>();
            LivroAssuntos = new HashSet<LivroAssunto>();
        }

        public int Cod { get; set; }
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public decimal? Preco { get; set; }
        public string AnoPublicacao { get; set; }
        public ICollection<LivroAutor> LivroAutores { get; set; }
        public ICollection<LivroAssunto> LivroAssuntos { get; set; }
        //public List<string> Test { get; set; }
    }
}
