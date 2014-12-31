using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using LearningLoop.Core.DomainServices;
using LearningLoop.Infrastructure.Persistence;
using LearningLoop.Infrastructure.Repositories;
using LearningLoop.Web.App_Start;
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

            var config = MembershipRebootConfig.Create();

            container.Register<IUserAccountRepository<HierarchicalUserAccount>>(() => new RavenUserAccountRepository("RavenHQ"));
            container.Register<IUserAccountQuery>(() => new RavenUserAccountRepository("RavenHQ"));

            container.Register(() => new MembershipRebootConfiguration<HierarchicalUserAccount>());
            container.Register(() => new UserAccountService<HierarchicalUserAccount>(config, container.GetInstance<IUserAccountRepository<HierarchicalUserAccount>>()));
            //container.Register<AuthenticationService<HierarchicalUserAccount>, SamAuthenticationService<HierarchicalUserAccount>>();

            container.Verify();
        }
    }
}
