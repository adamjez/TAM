using System;
using System.Reflection;

namespace OnRadio.BL.Helpers
{
    public class ObjectHelper
    {
        public static bool PublicInstancePropertiesEqual<T>(T self, T to) where T : class
        {
            if (self != null && to != null)
            {
                Type type = typeof(T);
                foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                    object toValue = type.GetProperty(pi.Name).GetValue(to, null);

                    if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                    {
                        return false;
                    }
                }
                return true;
            }
            return self == to;
        }
    }
}
