using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LearningLoop.Web.App_Start;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;

namespace LearningLoop.Web.Services
{
    public class AuthenticationService
    {
    }

    [Route("/profile")]
    public class GetUserProfile { }

    public class UserProfile
    {
        public int Id { get; set; }

        public UserAuth UserAuth { get; set; }
        public AuthUserSession Session { get; set; }
        public List<UserAuthDetails> UserAuthDetails { get; set; }
    }

    public class UserProfileResponse
    {
        public UserProfile Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/lockallusers")]
    public class LockAllUsers { }
    public class LockServices : Service
    {
        public object Any(LockAllUsers request)
        {
            Db.UpdateOnly(new UserAuth { LockedDate = DateTime.UtcNow },
                onlyFields: x => new { x.LockedDate },
                where: x => x.LockedDate == null);

            return HttpResult.Redirect("/");
        }
    }

    [Authenticate]
    public class UserProfileService : Service
    {
        public UserProfile Get(GetUserProfile request)
        {
            var session = base.SessionAs<CustomUserSession>();

            var userAuthId = session.UserAuthId.ToInt();
            var userProfile = new UserProfile
            {
                Id = userAuthId,
                Session = session,
                UserAuth = Db.SingleById<UserAuth>(userAuthId),
                UserAuthDetails = Db.Select<UserAuthDetails>(x => x.UserAuthId == userAuthId),
            };

            return userProfile;
        }
    }

    [Route("/reset-userauth")]
    public class ResetUserAuth { }
    public class ResetUserAuthService : Service
    {
        public object Get(ResetUserAuth request)
        {
            this.Cache.Remove(SessionFeature.GetSessionKey(Request));

            Db.DeleteAll<UserAuth>();
            Db.DeleteAll<UserAuthDetails>();

            var referrer = Request.UrlReferrer != null
                ? Request.UrlReferrer.AbsoluteUri
                : HttpHandlerFactory.GetBaseUrl();

            return HttpResult.Redirect(referrer);
        }
    }
}