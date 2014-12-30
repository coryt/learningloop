using LearningLoop.Core.DomainServices;
using LearningLoop.Infrastructure.Persistence;
using LearningLoop.Infrastructure.Repositories;
using Raven.Client;
using SimpleInjector;

namespace LearningLoop.Web
{
    public static class DependencyRegistrar
    {
        public static void RegisterServices(Container container)
        {
            container.Register<IClassroomRepository, ClassroomRepository>();
            container.RegisterPerWebRequest(() => RavenDBBootstrap.DocumentStore.OpenSession());
            container.Verify();
        }
    }
}
