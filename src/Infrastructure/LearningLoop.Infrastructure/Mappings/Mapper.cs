using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningLoop.Infrastructure.Mappings
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
