using ServiceStack.Auth;

namespace LearningLoop.Core.WebServices.Types
{
    public class CustomUserAuthDetails : UserAuthDetails
    {
        public string Custom { get; set; }
    }
}