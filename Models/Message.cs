namespace MyApiProject.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public Message() { }

        public Message(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public Message(string title)
        {
            Title = title;
        }
    }
}
