using System;
using ServiceStack;
using ServiceStack.Auth;

namespace LearningLoop.Web.App_Start
{
    public class CustomCredentialsAuthProvider : CredentialsAuthProvider
    {
        public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
        {
            if (password == "test")
                return true;

            throw HttpError.Unauthorized("Custom Error Message");
        }

        public override object Authenticate(IServiceBase authService, IAuthSession session, Authenticate request)
        {
            try
            {
                return base.Authenticate(authService, session, request);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}