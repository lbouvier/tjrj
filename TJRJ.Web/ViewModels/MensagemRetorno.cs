using System.Text.RegularExpressions;

namespace TJRJ.Web.ViewModels
{
    public class MensagemRetorno
    {
        public string Propriedade { get; set; }
        public string Atributo
        {
            get => Propriedade;
            set => Propriedade = value;
        }
        public string Mensagem { get; set; }
        /// <summary>
        /// Deve conter os dados a serem interpolada com a string de tradução, para montar uma mensagem mais direcionada!
        /// </summary>
        public object Dados { get; set; }
        public string MensagemTraduzida { get; set; }
    }
}
