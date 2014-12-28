using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningLoop.Core.Interfaces
{
    public interface IServiceLocator
    {
        T GetInstance<T>() where T : class;
        object GetInstance(Type type);
    }
}
