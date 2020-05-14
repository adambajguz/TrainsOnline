namespace TrainsOnline.Application.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Application.Interfaces.Mapping;
    using AutoMapper;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            Assembly assembly = typeof(ICustomMapping).Assembly;

            LoadCustomMappings(assembly);
        }

        public AutoMapperProfile(Assembly assembly)
        {
            LoadCustomMappings(assembly);
        }

        private void LoadCustomMappings(Assembly rootAssembly)
        {
            IList<ICustomMapping> mappings = GetCustomMappings(rootAssembly);

            foreach (ICustomMapping map in mappings)
                map.CreateMappings(this);
        }

        private static IList<ICustomMapping> GetCustomMappings(Assembly rootAssembly)
        {
            Type[] types = rootAssembly.GetExportedTypes();

            List<ICustomMapping> withCustomMappings = (
                    from type in types
                    from instance in type.GetInterfaces()
                    where
                        typeof(ICustomMapping).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface
                    select Activator.CreateInstance(type) as ICustomMapping).ToList();

            return withCustomMappings;
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            Type[] types = assembly.GetExportedTypes();
            Type interfaceType = typeof(IMapFrom<>);
            string methodName = nameof(IMapFrom<object>.Mapping);
            Type[] argumentTypes = new Type[] { typeof(Profile) };

            foreach (Type type in types)
            {
                List<Type> interfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType)
                    .ToList();

                // Has the type implemented any IMapFrom<T>?
                if (interfaces.Count > 0)
                {
                    // Yes, then let's create an instance
                    object? instance = Activator.CreateInstance(type);

                    // and invoke each implementation of `.Mapping()`
                    foreach (Type i in interfaces)
                    {
                        MethodInfo? methodInfo = i.GetMethod(methodName, argumentTypes);

                        methodInfo?.Invoke(instance, new object[] { this });
                    }
                }
            }
        }
    }
}
