using ServiceStack.Auth;

namespace LearningLoop.Web.App_Start
{
    public class CustomUserAuth : UserAuth
    {
        public string Custom { get; set; }
    }
}