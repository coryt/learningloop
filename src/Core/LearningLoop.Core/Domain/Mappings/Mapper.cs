using System.Collections.Generic;
using System.Linq;

namespace LearningLoop.Core.Domain.Mappings
{
    public class Mapper
    {
        public static void CreateMap<T, R>()
        {
            AutoMapper.Mapper.CreateMap<T, R>();
        }

        public static T Map<T, R>(R sourceObj)
        {
            var destinationObj = AutoMapper.Mapper.Map<T>(sourceObj);

            return destinationObj;
        }

        public static IEnumerable<T> Map<T, R>(IEnumerable<R> sourceObjCollection)
        {
            return sourceObjCollection.Select(Map<T, R>);
        }
    }
}
