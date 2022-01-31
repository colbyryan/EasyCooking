
using System.Collections.Generic;

namespace EasyCooking.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int UserProfileId { get; set; }
        public int? CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public string Creator { get; set; }
        public string Description {get;set;}
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public string ServingAmount { get; set; }
        public Category Category { get; set; }
        public List<Category> CategoryOptions { get; set; }
    }
}
