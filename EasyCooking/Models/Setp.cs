
namespace EasyCooking.Models
{
    public class Setp
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Content { get; set; }
        public int StepOrder { get; set; }
    }
}
