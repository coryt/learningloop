using System;
using System.Web;
using LearningLoop.Core.DomainServices;
using LearningLoop.DependencyResolution;
using LearningLoop.Infrastructure.Repositories;
using SimpleInjector;

[assembly: PreApplicationStartMethod(typeof(DependencyRegistrar), "RegisterAllDependencies")]
namespace LearningLoop.DependencyResolution
{
    using Core.Domain;
    using Core.Interfaces;

    public static class DependencyRegistrar
    {
        public static Container Container;

        #region Public Methods

        /// <summary>
        /// Registers all dependencies.
        /// </summary>
        public static void RegisterAllDependencies()
        {
            Container = new Container();

            Register<IClassroomRepository, ClassroomRepository>();

            Container.Verify();

            ServiceLocator.SetServiceLocator(() => new SimpleInjectorServiceLocator(Container));
        }

        /// <summary>
        /// Registers the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        public static void Register(Type service, Type implementation)
        {
            Container.Register(service, implementation);
        }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <typeparam name="TService">The service.</typeparam>
        /// <typeparam name="TImplementation">The implementation.</typeparam>
        public static void Register<TService, TImplementation>()
        {
            Register(typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Registers as singleton.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        public static void RegisterSingle(Type service, Type implementation)
        {
            Container.RegisterSingle(service, implementation);
        }

        /// <summary>
        /// Registers as singleton.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        public static void RegisterSingle<TService, TImplementation>()
        {
            RegisterSingle(typeof(TService), typeof(TImplementation));
        }

        #endregion
    }
}
