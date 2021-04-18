using System;
using System.Collections.Generic;
using System.Reflection;

namespace IoCContainer.Container
{
    public class Container
    {
        private Assembly _assembly = null;

        private Dictionary<string, Type> _map = new();

        public void AddType(Type type)
        {
            _map.Add(type.FullName, type);
        }

        public void AddType(Type type, Type baseType)
        {
            _map.Add(baseType.FullName, type);

            // To reference a type by its subtype
            _map.Add(type.FullName, type);
        }

        public void AddAssembly(Assembly assembly)
        {
            if (_assembly != null)
            {
                throw new ApplicationException("There is assembly already registered.");
            }

            _assembly = assembly;

            var types = new List<Type>(_assembly.GetTypes());
            var newMappedTypes = MapTypes(types);

            _map = newMappedTypes;
        }

        public T Get<T>()
        {
            var type = typeof(T);
            var typeRegistered = _map.ContainsKey(type.FullName);

            if (!typeRegistered)
            {
                throw new ApplicationException($"{type.FullName} is not registered.");
            }

            return (T)CreateTypeInstance(type);
        }

        private object CreateTypeInstance(Type type)
        {
            var constructorHasDependencies = type.GetCustomAttribute<ImportConstructorAttribute>() != null;

            if (constructorHasDependencies)
            {
                var firstConstructor = type.GetConstructors()[0];
                var firstConstructorParams = firstConstructor.GetParameters();
                var dependencyTypes = GetConstuctorParamTypes(firstConstructor, _map);

                if (dependencyTypes.Count != firstConstructorParams.Length)
                {
                    throw new ApplicationException("No required dependencies are registered.");
                }

                return InjectDependenciesViaConstructor(type, dependencyTypes);
            }

            return Activator.CreateInstance(_map[type.FullName]);
        }

        private List<Type> GetConstuctorParamTypes(ConstructorInfo constructor, Dictionary<string, Type> map)
        {
            var types = new List<Type>();

            foreach (var param in constructor.GetParameters())
            {
                var key = param.ParameterType.FullName;
                var typeFound = map.ContainsKey(key);

                if (typeFound)
                {
                    types.Add(map[key]);
                }
            }

            return types;
        }

        private object InjectDependenciesViaConstructor(Type targetType, List<Type> dependencies)
        {
            var args = new List<Object>();

            dependencies.ForEach(type => args.Add(Activator.CreateInstance(type)));

            return Activator.CreateInstance(targetType, args.ToArray());
        }

        private Dictionary<string, Type> MapTypes(List<Type> types)
        {
            var map = new Dictionary<string, Type>();
            var models = types.FindAll(type => !type.IsInterface);
            var interfaces = types.FindAll(type => type.IsInterface);

            foreach (Type type in models)
            {
                var modeledInterface = interfaces.Find(i => i.IsAssignableFrom(type));

                if (modeledInterface != null)
                {
                    map.Add(modeledInterface.FullName, type);
                    continue;
                }

                map.Add(type.FullName, type);
            }

            return map;
        }
    }
}
