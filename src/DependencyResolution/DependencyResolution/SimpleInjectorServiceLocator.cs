using System;
using SimpleInjector;

namespace LearningLoop.DependencyResolution
{
    using Core.Interfaces;

    class SimpleInjectorServiceLocator : IServiceLocator
    {
        private readonly Container _container;

        public SimpleInjectorServiceLocator(Container container)
        {
            _container = container;
        }
        public T GetInstance<T>() where T : class
        {
            return _container.GetInstance<T>();
        }

        public object GetInstance(Type type)
        {
            return _container.GetInstance(type);
        }
    }
}
