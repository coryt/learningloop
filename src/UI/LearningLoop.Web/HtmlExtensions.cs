using LearningLoop.Core.WebServices.Types;
using ServiceStack.Razor;

namespace LearningLoop.Web
{
    public static class HtmlExtensions
    {
        public static string ProfileUrl(this ViewPage view)
        {
            var session = view.SessionAs<UserSession>();
            return session == null || session.ProfileUrl64 == null
                ? "/img/no-profile-pic-64.png"
                : session.ProfileUrl64.Replace("http:", "");
        }
    }
}