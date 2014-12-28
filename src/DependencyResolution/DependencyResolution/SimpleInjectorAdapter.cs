using System;
using ServiceStack.Configuration;
using SimpleInjector;

namespace LearningLoop.DependencyResolution
{
    public class SimpleInjectorAdapter : IContainerAdapter
    {
        private readonly Container container;

        public SimpleInjectorAdapter(Container container)
        {
            this.container = container;
        }

        public T Resolve<T>()
        {
            return (T)this.container.GetInstance(typeof(T));
        }

        public T TryResolve<T>()
        {
            IServiceProvider provider = this.container;
            object service = provider.GetService(typeof(T));
            return service != null ? (T)service : default(T);
        }
    }
}