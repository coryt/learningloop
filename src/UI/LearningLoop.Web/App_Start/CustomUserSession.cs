using ServiceStack;

namespace LearningLoop.Web.App_Start
{
    public class CustomUserSession : AuthUserSession
    {
        public string CustomProperty { get; set; }
    }
}