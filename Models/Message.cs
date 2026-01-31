using MyApiProject.Models;
namespace MyApiProject.Models
{
    public class Message
    {
        public string Title { get; set; }
        public int code { get; set; }

        public Message() { }

        public Message(string title)
        {
            Title = title;
        }
        public string Status { get; set; }  // 狀態 (例如 Success, Error)
        public string Info { get; set; }    // 附加訊息
    }

}
