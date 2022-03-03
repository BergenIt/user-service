using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutoMapper;

using DatabaseExtension.Translator;

using Google.Protobuf;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core.NotifyEventTypeGetter;
using UserService.Core_Tests.Moqups;

namespace UserService.Main.Automapper_Tests
{
    [TestClass]
    public class AutoMapperTests
    {
        private readonly ObjectFabric _objectFabric = new();

        private readonly IMapper _mapper = new ServiceCollection()
            .AddSingleton<ITranslator>(new ReverseTranslator())
            .AddSingleton<INotifyEventTypeGetter>(new MoqupsINotifyEventTypeGetter())
            .AddAutoMapper(typeof(Startup))
            .BuildServiceProvider()
            .GetRequiredService<IMapper>();

        //TODO: оно способно работать надо придумать как

        //[TestMethod]
        public void AutomapTest(Type sourceType, Type distType, int deep, bool reverse, bool checkResult = true)
        {
            object source = _objectFabric.CreateInstance(sourceType, deep);

            object result = _mapper.Map(source, sourceType, distType);

            if (checkResult)
            {
                IEnumerable<KeyValuePair<string, string>> sourceValues = GetValues(source);
                IEnumerable<KeyValuePair<string, string>> distValues = GetValues(result);

                Assert.IsTrue(sourceValues.All(v => distValues.Any(d => d.Value == v.Value || (d.Key == v.Key && d.Value == "enum"))));

                if (reverse)
                {
                    AutomapTest(distType, sourceType, deep, false, false);
                }
            }
        }

        private IEnumerable<KeyValuePair<string, string>> GetValues<TModel>(TModel model) where TModel : notnull
        {
            List<KeyValuePair<string, string>> result = new();

            IEnumerable<PropertyInfo> distPropertyInfos = model
                .GetType()
                .GetProperties()
                .Where(p => p.PropertyType.Namespace != typeof(Google.Protobuf.Collections.Lists).Namespace || p.SetMethod is not null)
                .Where(p => p.Name is not (nameof(IMessage.Descriptor)) and not (nameof(YamlDotNet.Core.Parser)));

            foreach (PropertyInfo property in distPropertyInfos)
            {
                object? value = property.GetValue(model);

                if (value is null)
                {
                    continue;
                }

                if (property.PropertyType.IsEnum)
                {
                    result.Add(new(property.PropertyType.Name, "enum"));
                }

                if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                {
                    string? item = value.ToString();

                    if (!string.IsNullOrEmpty(item))
                    {
                        result.Add(new(property.Name, item));
                    }
                }
                else
                {
                    IEnumerable<KeyValuePair<string, string>> values = GetValues(value);

                    result.AddRange(values);
                }
            }

            return result;
        }
    }
}

