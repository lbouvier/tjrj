using System;

namespace TJRJ.Web.ViewModels
{
    public class BaseViewModel
    {
        public string Id { get; set; }
        public BaseViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
