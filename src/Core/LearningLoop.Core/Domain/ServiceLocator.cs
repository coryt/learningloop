﻿using System;

namespace LearningLoop.Core.Domain
{
    using Interfaces;

    public static class ServiceLocator
    {
        public static IServiceLocator Current { get; private set; }

        public static void SetServiceLocator(Func<IServiceLocator> create)
        {
            if (create == null)
                throw new ArgumentNullException("create");
            Current = create();
        }
    }
}
