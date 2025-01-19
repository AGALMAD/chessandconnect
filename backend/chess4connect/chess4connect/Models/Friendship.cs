namespace chess4connect.Models
{
    public class Friendship
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string friendId { get; set; }
        public bool isFriend { get; set; }
    }
}
