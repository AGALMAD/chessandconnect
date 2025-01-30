using chess4connect.Enums;

namespace chess4connect.Models.Database.DTOs
{
    public class RequestDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarImageUrl { get; set; }
        
    }
}
