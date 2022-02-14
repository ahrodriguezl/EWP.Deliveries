using System;
using System.Linq;

namespace EWP
{
    class SboFramework
    {
        public static T GetAttribute<T>(Type type)
        {
            var tableAttr = type.GetCustomAttributes(true)
                                .Where(x => x.GetType() == typeof(T))
                                .FirstOrDefault();

            return (T)tableAttr;
        }
    }
}
