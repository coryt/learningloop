using LearningLoop.Core.DomainServices;
using LearningLoop.Core.WebServices.Types;
using LearningLoop.Infrastructure.Persistence;
using LearningLoop.Web.App_Start;
using ServiceStack.Auth;
using ServiceStack.Authentication.RavenDb;
using SimpleInjector;

namespace LearningLoop.Web
{
    public static class DependencyRegistrar
    {
        public static Container RegisterServices()
        {
            var container = new Container();
            container.Register<IClassroomRepository, ClassroomRepository>();

            //container.Register<IDocumentStore>(() => new DocumentStore { ConnectionStringName = "RavenHQ" }.Initialize());
            container.RegisterPerWebRequest(() => RavenDBBootstrap.DocumentStore.OpenSession());

            container.Register<IAuthRepository>(() =>
                new RavenDbUserAuthRepository<CustomUserAuth, CustomUserAuthDetails>(RavenDBBootstrap.DocumentStore));

            container.Verify();

            return container;
        }
    }
}
