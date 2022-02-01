
using System.Collections.Generic;

namespace EasyCooking.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public List<Ingredient> ingredients { get; set; }
    }
}
