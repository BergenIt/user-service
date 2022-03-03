using System;
using System.Linq;
using System.Reflection;

using AutoMapper;

using DatabaseExtension.Translator;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseExtension.Configure
{
    public static class PageConfiguratorBuilder
    {
        public static IApplicationBuilder UsePageConfigurator(this IApplicationBuilder builder, Assembly assembly)
        {
            ITranslator translator = builder.ApplicationServices.GetRequiredService<ITranslator>();

            IMapper mapper = builder.ApplicationServices.GetRequiredService<IMapper>();

            PageExtensions.InjectMapper(mapper);
            PageExtensions.InjectAssembly(assembly);

            System.Collections.Generic.IEnumerable<PageConfigurator> pageConfigurators = assembly.DefinedTypes
                .Where(t => t.BaseType == typeof(PageConfigurator))
                .Select(t => Activator.CreateInstance(t) as PageConfigurator);

            DatabaseExtensionConfig extensionConfig = new(translator);

            foreach (PageConfigurator pageConfigurator in pageConfigurators)
            {
                extensionConfig._routeDictionaryList.AddRange(pageConfigurator._databaseExtensionConfig._routeDictionaryList);
                extensionConfig._routeDictionaryValueList.AddRange(pageConfigurator._databaseExtensionConfig._routeDictionaryValueList);
            }

            SortConverter.InjectConfig(extensionConfig);
            SearchConverter.InjectConfig(extensionConfig);
            GroupConverter.InjectConfig(extensionConfig);

            return builder;
        }
    }
}
