using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System;


namespace TJRJ.Web.Dtos
{
    public class ReturnMessage
    {
        public string Property { get; set; }
        public string Attribute
        {
            get => Property;
            set => Property = value;
        }
        public string Message { get; set; }
        /// <summary>
        /// Deve conter os dados a serem interpolada com a string de tradução, para montar uma mensagem mais direcionada!
        /// </summary>
        [NotMapped]
        public object Data { get; set; }
        public string TranslatedMessage { get; set; }
    }
}
