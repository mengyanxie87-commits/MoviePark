using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApiProject.Pages
{
    public class IndexModel : PageModel
    {
        public string Message { get; set; }
        public void OnGet()
        {
            Message = "歡迎使用 Razor Pages!";
        }
    }
}
