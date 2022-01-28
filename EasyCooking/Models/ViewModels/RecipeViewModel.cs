using System.Collections.Generic;

namespace EasyCooking.Models.ViewModels
{
    public class RecipeViewModel
    {
        public Recipe Recipe { get; set; }
        public List<Category> CategoryOptions { get; set; }
    }
}
