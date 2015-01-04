using LearningLoop.Core.DomainServices;
using LearningLoop.Infrastructure.Persistence;
using LearningLoop.Web.App_Start;
using Raven.Client;
using Raven.Client.Document;
using ServiceStack.Auth;
using ServiceStack.Authentication.RavenDb;
using SimpleInjector;

namespace LearningLoop.Web
{
    public static class DependencyRegistrar
    {
        public static void RegisterServices(Container container)
        {
            container.Register<IClassroomRepository, ClassroomRepository>();

            //container.Register<IDocumentStore>(() => new DocumentStore { ConnectionStringName = "RavenHQ" }.Initialize());
            container.RegisterPerWebRequest(() => RavenDBBootstrap.DocumentStore.OpenSession());

            container.Register<IAuthRepository>(() =>
                new RavenDbUserAuthRepository<CustomUserAuth, CustomUserAuthDetails>(RavenDBBootstrap.DocumentStore));

            container.Verify();
        }
    }
}
