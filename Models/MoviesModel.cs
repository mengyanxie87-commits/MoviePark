using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApiProject.Pages
{
    public class MoviesModel : PageModel
    {
        public List<string> MovieList { get; set; }

        public void OnGet()
        {
            MovieList = new List<string> { "電影1", "電影2", "電影3" };
        }
    }
}