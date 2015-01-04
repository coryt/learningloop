using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Text;
using ServiceStack.Web;

namespace LearningLoop.Core.WebServices.Types
{
    public class CustomUserSession : AuthUserSession
    {
        public override void OnAuthenticated(IServiceBase authService, IAuthSession session, IAuthTokens tokens,
            Dictionary<string, string> authInfo)
        {
            var jsv = authService.Request.Dto.Dump();
            "OnAuthenticated(): {0}".Print(jsv);
        }

        public override void OnRegistered(IRequest httpReq, IAuthSession session, IServiceBase service)
        {
            "OnRegistered()".Print();
        }
    }
}