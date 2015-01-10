using LearningLoop.Core.DomainServices;
using LearningLoop.Core.WebServices.Types;
using LearningLoop.Infrastructure.Persistence;
using Raven.Client;
using Raven.Client.Document;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Authentication.RavenDb;
using ServiceStack.Caching;
using SimpleInjector;

namespace LearningLoop.Web
{
    public static class DependencyRegistrar
    {
        public static Container RegisterServices()
        {
            var container = new Container();
            container.Register<IClassroomRepository, ClassroomRepository>();

            container.Register<IDocumentStore>(() => new DocumentStore { ConnectionStringName = "RavenHQ" }.Initialize());
            container.RegisterPerWebRequest<IDocumentSession>(() => RavenDBBootstrap.DocumentStore.OpenSession());

            //FYI: both repositories are required by the SS internals. found out the hard way...
            container.Register<IAuthRepository>(() => new RavenDbUserAuthRepository<UserAuth, UserAuthDetails>(RavenDBBootstrap.DocumentStore));
            container.Register<IUserAuthRepository>(() => new RavenDbUserAuthRepository<UserAuth, UserAuthDetails>(RavenDBBootstrap.DocumentStore));

            container.Verify();
            return container;
        }
    }
}
