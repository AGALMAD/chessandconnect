using chess4connect.Enums;

namespace chess4connect.DTOs
{
    public class RequestDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarImageUrl { get; set; }

    }
}
