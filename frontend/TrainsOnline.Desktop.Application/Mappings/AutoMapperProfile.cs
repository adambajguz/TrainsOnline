namespace TrainsOnline.Desktop.Application.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoMapper;
    using global::TrainsOnline.Desktop.Application.Interfaces;

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
    }
}
