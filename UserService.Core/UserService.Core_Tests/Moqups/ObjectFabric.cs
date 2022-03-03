using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UserService.Core.Entity;

namespace UserService.Core_Tests.Moqups
{
    public class ObjectFabric
    {
        public object CreateInstance(Type sourceType, int deep)
        {
            if (sourceType.IsArray || sourceType.Namespace == typeof(IEnumerable<object>).Namespace)
            {
                return CreateCollectionInstance(sourceType, deep);
            }
            else
            {
                object instance = Activator.CreateInstance(sourceType);

                if (!sourceType.IsValueType)
                {
                    int current = 1;
                    SetInstancePropValue(instance, deep, ref current);
                }
                return instance;
            }
        }

        public TInstanse CreateInstance<TInstanse>(int deep) => (TInstanse)CreateInstance(typeof(TInstanse), deep);
        public TInstanse CreateInstance<TInstanse>(Guid id, int deep) where TInstanse : IBaseEntity
        {
            TInstanse instanse = (TInstanse)CreateInstance(typeof(TInstanse), deep);

            instanse.Id = id;

            return instanse;
        }

        private object CreateCollectionInstance(Type sourceType, int deep)
        {
            Type genericCollectionType = sourceType.GenericTypeArguments.First();

            object instanceCollection = typeof(List<>)
                .MakeGenericType(new Type[] { genericCollectionType })
                .GetConstructors()
                .First(c => !c.GetParameters().Any())
                .Invoke(Array.Empty<object>());

            object collectionItem = CreateInstance(genericCollectionType, deep);

            _ = instanceCollection
                .GetType()
                .GetMethods()
                .First(m => m.Name == nameof(List<object>.Add))
                .Invoke(instanceCollection, new object[] { collectionItem });

            return instanceCollection;
        }

        private void SetInstancePropValue(object instance, int deep, ref int current)
        {
            Type instanceType = instance.GetType();

            if (current > deep || instanceType.IsArray || instanceType.Namespace == typeof(IEnumerable<object>).Namespace)
            {
                return;
            }

            current += 1;

            PropertyInfo[] propertyInfos = instanceType.GetProperties().Reverse().ToArray();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Type propertyType = propertyInfo.PropertyType;

                if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                {
                    propertyInfo.SetValue(instance, DateTime.UtcNow);
                }

                object @default = GetDefaultValue(propertyType);
                object value = propertyInfo.GetValue(instance);

                if (value is null || (@default is not null && value.Equals(@default)))
                {
                    object propertyValue = default;

                    bool hasEmptyContructor = propertyType.GetConstructors().Any(c => !c.GetParameters().Any());

                    if (hasEmptyContructor)
                    {
                        propertyValue = Activator.CreateInstance(propertyType);
                        int currentDeep = current;

                        SetInstancePropValue(propertyValue, deep, ref currentDeep);
                    }
                    else if (propertyType.Name.Contains(nameof(Nullable)))
                    {
                        Type distType = propertyType.GenericTypeArguments.First();
                        propertyValue = Activator.CreateInstance(distType);
                    }
                    else if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                    {
                        propertyValue = Guid.NewGuid();
                    }
                    else if (propertyType == typeof(string))
                    {
                        propertyValue = Guid.NewGuid().ToString();
                    }
                    else if (propertyType.IsArray || propertyType.Namespace == typeof(IEnumerable<object>).Namespace)
                    {
                        propertyValue = Activator.CreateInstance(typeof(List<>)
                            .MakeGenericType(new Type[] { propertyType.GenericTypeArguments.First() }));
                    }

                    if (propertyInfo.SetMethod is not null)
                    {
                        propertyInfo.SetValue(instance, propertyValue);
                    }
                }
            }
        }

        private object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            if (type == typeof(string))
            {
                return string.Empty;
            }

            return null;
        }
    }
}
