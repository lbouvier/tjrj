
namespace TJRJ.Entities
{
    public class LivroAssunto //: EntidadeBase
    {
        public LivroAssunto()
        {

        }

        public int LivroCod { get; set; }
        public Livro Livro { get; set; }

        public int AssuntoCodAs { get; set; }
        public Assunto Assunto { get; set; }
    }
}
