using System;
using System.Collections.Generic;
using LearningLoop.Core.WebServices.Types;
using ServiceStack;
using ServiceStack.Auth;

namespace LearningLoop.Core.WebServices
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileUrl64 { get; set; }

        public IUserAuth UserAuth { get; set; }
        public IAuthSession Session { get; set; }
        public List<IUserAuthDetails> UserAuthDetails { get; set; }
    }

    [Route("/profile")]
    public class GetUserProfile { }

    [Authenticate]
    public class UserProfileService : Service
    {
        public IUserAuthRepository UserAuthRepository { get; set; }

        public UserProfile Get(GetUserProfile request)
        {
            var session = base.GetSession();
            if (session == null)
                throw new UnauthorizedAccessException("Must be signed in to use this service");

            var userAuth = UserAuthRepository.GetUserAuth(session.UserAuthId);
            if (userAuth == null)
                throw new UnauthorizedAccessException("Must be signed in to use this service");

            var userAuthDetails = UserAuthRepository.GetUserAuthDetails(session.UserAuthId);
            userAuth.PasswordHash = "[Redacted]";
            userAuth.Salt = "[Redacted]";
            userAuth.DigestHa1Hash = "[Redacted]";
            userAuthDetails.ForEach(x =>
                x.AccessToken = x.AccessTokenSecret = x.RequestTokenSecret = "[Redacted]");

            //session.ProviderOAuthAccess.ForEach(x =>
            //x.AccessToken = x.AccessTokenSecret = x.RequestTokenSecret = "[Redacted]");

            var userProfile = new UserProfile
            {
                Id = session.UserAuthId,
                Session = session,
                UserAuth = userAuth,
                UserAuthDetails = userAuthDetails,
            };

            if (userProfile.DisplayName == null)
                userProfile.DisplayName = userAuth.UserName
                    ?? userAuth.FullName
                    ?? "{0} {1}".Fmt(userAuth.FirstName, userAuth.LastName);

            if (userProfile.ProfileUrl64 == null)
                userProfile.ProfileUrl64 = this.SessionAs<UserSession>().ProfileUrl64
                    ?? "/img/no-profile-pic-64.png";

            return userProfile;
        }
    }
}