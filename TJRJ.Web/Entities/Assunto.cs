using System.Collections.Generic;

namespace TJRJ.Entities
{
    public class Assunto //: EntidadeBase
    {
        public Assunto()
        {
            LivroAssuntos = new HashSet<LivroAssunto>();
        }
        public int CodAs { get; set; }
        public string Descricao { get; set; }
        public virtual ICollection<LivroAssunto> LivroAssuntos { get; set; }
    }
}
