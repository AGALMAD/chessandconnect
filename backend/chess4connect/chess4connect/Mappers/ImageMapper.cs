using chess4connect.Extensions;
using chess4connect.Models.Database.Entities;

namespace chess4connect.Models.Mappers
{
    public class ImageMapper
    {
        public IList<User> AddCorrectPath(IList<User> users, HttpRequest httpRequest = null)
        {
            foreach (User user in users)
            {
                user.AvatarImageUrl = httpRequest is null ? user.AvatarImageUrl : httpRequest.GetAbsoluteUrl("bicis/" + bici.UrlImg);
            }
            return users;
        }

        public User AddCorrectPath(User users, HttpRequest httpRequest = null)
        {
            string test = "bicis/" + users.AvatarImageUrl;
            users.AvatarImageUrl = httpRequest.GetAbsoluteUrl(test);
            return users;
        }

    }
}