using Microsoft.AspNetCore.Mvc.Rendering;
using MVCProekt.Models;

namespace MVCProekt.ViewModels
{
    public class ProductCategoriesEditViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<int>? SelectedCategories { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
    }
}
