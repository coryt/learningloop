using LearningLoop.Core.Domain.Commands;
using LearningLoop.Infrastructure.Persistence;
using MediatR;
using Microsoft.Practices.ServiceLocation;
using Raven.Client.Document;
using ServiceStack.Auth;
using ServiceStack.Authentication.RavenDb;
using SimpleInjector;
using SimpleInjector.Extensions;

namespace LearningLoop.Web
{
    public static class DependencyRegistrar
    {
        /// <summary>
        /// Encapsulating all registration code. For advanced scenarios, check out
        /// https://simpleinjector.readthedocs.org/en/2.6/advanced.html#batch-registration
        /// </summary>
        public static Container RegisterServices()
        {
            var container = new Container();

            container.Register<IMediator, Mediator>();
            container.RegisterManyForOpenGeneric(
                typeof(IRequestHandler<,>),
                typeof(CreateClassroomCommand).Assembly
                );
            container.RegisterManyForOpenGeneric(
                typeof(INotificationHandler<>),
                typeof(CreateClassroomCommand).Assembly
                );
            container.Register(() => new DocumentStore { ConnectionStringName = "RavenHQ" }.Initialize());
            container.RegisterPerWebRequest(() => RavenDBBootstrap.DocumentStore.OpenSession());

            //FYI: both repositories are required by the SS internals. found out the hard way...
            container.Register<IAuthRepository>(() => new RavenDbUserAuthRepository<UserAuth, UserAuthDetails>(RavenDBBootstrap.DocumentStore));
            container.Register<IUserAuthRepository>(() => new RavenDbUserAuthRepository<UserAuth, UserAuthDetails>(RavenDBBootstrap.DocumentStore));

            return container;
        }
    }
}
