using System.Web.UI;
using ServiceStack;
using ServiceStack.Caching;

namespace LearningLoop.Web.App_Start
{
    public class PageBase : Page
    {
        /// <summary>
        /// Typed UserSession
        /// </summary>
        private object _userSession;
        /// <summary>
        /// Dynamic SessionBag Bag
        /// </summary>
        private ISession _sessionBag;
        private ISessionFactory _sessionFactory;

        protected virtual TUserSession SessionAs<TUserSession>()
        {
            return (TUserSession)(_userSession ?? (_userSession = Cache.SessionAs<TUserSession>()));
        }

        protected CustomUserSession UserSession
        {
            get
            {
                return SessionAs<CustomUserSession>();
            }
        }

        public new ICacheClient Cache
        {
            get { return HostContext.Resolve<ICacheClient>(); }
        }

        public virtual ISessionFactory SessionFactory
        {
            get { return _sessionFactory ?? (_sessionFactory = HostContext.Resolve<ISessionFactory>()) ?? new SessionFactory(Cache); }
        }
       
        public new ISession SessionBag
        {
            get
            {
                return _sessionBag ?? (_sessionBag = SessionFactory.GetOrCreateSession());
            }
        }

        public void ClearSession()
        {
            _userSession = null;
            this.Cache.Remove(SessionFeature.GetSessionKey());
        }
    }
}
