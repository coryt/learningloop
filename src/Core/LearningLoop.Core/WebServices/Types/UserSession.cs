using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Web;

namespace LearningLoop.Core.WebServices.Types
{
    [DataContract]
    public class UserSession : AuthUserSession
    {
        [DataMember]
        public string ProfileUrl64 { get; set; }

        public override void OnAuthenticated(IServiceBase authService, IAuthSession session, IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            base.OnAuthenticated(authService, session, tokens, authInfo);

            this.ProfileUrl64 = session.GetProfileUrl();
        }

        public override void OnCreated(IRequest httpReq)
        {
            base.OnCreated(httpReq);


        }
    }
}