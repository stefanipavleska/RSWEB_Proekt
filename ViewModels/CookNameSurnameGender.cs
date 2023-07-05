using Microsoft.AspNetCore.Mvc.Rendering;
using MVCProekt.Models;

namespace MVCProekt.ViewModels
{
    public class CookNameSurnameGender
    {
        public IList<Cook> Cooks { get; set; }
        public SelectList Gender { get; set; }
        public string CookGender { get; set; }
        public string SearchStringName { get; set; }

        public string SearchStringSurname { get; set; }
    }
}
