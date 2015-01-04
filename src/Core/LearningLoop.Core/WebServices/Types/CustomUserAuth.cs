using ServiceStack.Auth;

namespace LearningLoop.Core.WebServices.Types
{
    public class CustomUserAuth : UserAuth
    {
        public string Custom { get; set; }
    }
}