using ServiceStack.Auth;

namespace LearningLoop.Web.App_Start
{
    public class CustomUserAuthDetails : UserAuthDetails
    {
        public string Custom { get; set; }
    }
}