using System.Collections.Generic;

namespace EasyCooking.Models.ViewModels
{
    public class RecipeViewModel
    {
        public Recipe Recipe { get; set; }
        public Category category { get; set; }
        public List<Category> CategoryOptions { get; set; }
        public List<Ingredient> ingredients { get; set; }
        public List<Step> steps { get; set; }
    }
}
