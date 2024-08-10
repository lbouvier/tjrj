using System.Collections.Generic;

namespace TJRJ.Entities
{
    public class Livro //: EntidadeBase
    {
        public Livro()
        {
            LivroAutores = new List<LivroAutor>();
            LivroAssuntos = new List<LivroAssunto>();
        }

        public int Cod { get; set; }
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public decimal? Preco { get; set; }
        public string AnoPublicacao { get; set; }
        public IList<LivroAutor> LivroAutores { get; set; }
        public  IList<LivroAssunto> LivroAssuntos { get; set; }
    }
}
