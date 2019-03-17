namespace Runpath.Models
{
    public class Album
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public Photo[] Photos { get; set; }
    }
}
