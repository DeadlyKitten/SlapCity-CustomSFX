using System;
using System.Reflection;

namespace CustomSFX.Utilities
{
    internal static class ReflectionUtil
    {
        private const BindingFlags _allBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public static object InvokeMethod(this object obj, string methodName, params object[] methodParams)
        {
            MethodInfo method = obj.GetType().GetMethod(methodName, _allBindingFlags);
            if (method == null)
                throw new InvalidOperationException($"{methodName} is not a member of {obj.GetType().Name}");
            return method.Invoke(obj, methodParams);
        }

        public static object GetProperty(this object obj, string propertyName, Type targetType)
        {
            var prop = targetType.GetProperty(propertyName, _allBindingFlags);
            if (prop == null)
                throw new InvalidOperationException($"{propertyName} is not a member of {targetType.Name}");
            var value = prop.GetValue(obj);
            return value;
        }
        public static T InvokeMethod<T>(this object obj, string methodName, params object[] methodParams) => (T)InvokeMethod(obj, methodName, methodParams);

        public static object GetProperty(this object obj, string propertyName) => obj.GetProperty(propertyName, obj.GetType());

        public static T GetProperty<T>(this object obj, string propertyName) => (T)GetProperty(obj, propertyName);
    }
}
