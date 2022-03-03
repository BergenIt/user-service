using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using AutoMapper;

using DatabaseExtension.Proto;

using Google.Protobuf;

using Microsoft.EntityFrameworkCore;

namespace DatabaseExtension
{
    public static partial class PageExtensions
    {
        private static IMapper s_mapper;
        private static Assembly s_assembly;

        public static void InjectAssembly(Assembly assembly)
        {
            s_assembly = assembly;
        }

        public static void InjectMapper(IMapper mapper)
        {
            s_mapper = mapper;
        }

        /// <summary>
        /// Распаковка прото страницы
        /// </summary>
        /// <param name="any">Запакованная протостраница</param>
        /// <returns></returns>
        public static Page<object> FromProtoPage(this Google.Protobuf.WellKnownTypes.Any any)
        {
            if (any.TryUnpack(out List list))
            {
                Type sourceType = s_assembly.GetType(list.SourceTypeName);

                List<object> protoResult = new(list.Items
                    .Select(l =>
                        typeof(Google.Protobuf.WellKnownTypes.Any)
                            .GetMethod(nameof(Google.Protobuf.WellKnownTypes.Any.Unpack))
                            .MakeGenericMethod(sourceType)
                .Invoke(l, Array.Empty<object>())));

                return new(protoResult, list.CountItems, list.PaginationFilter.FromProtoPagination());
            }
            if (any.TryUnpack(out GroupList groups))
            {
                Type sourceType = s_assembly.GetType(groups.SourceTypeName);

                List<Grouping<object>> distes = new();
                foreach (Group group in groups.GroupItems)
                {
                    List<object> protoSources = new(group.Items
                        .Select(l =>
                            typeof(Google.Protobuf.WellKnownTypes.Any)
                                .GetMethod(nameof(Google.Protobuf.WellKnownTypes.Any.Unpack))
                                .MakeGenericMethod(sourceType)
                                .Invoke(l, Array.Empty<object>()))
                );

                    Grouping<object> distGroup = new()
                    {
                        Key = group.GroupKey,
                        Group = new(protoSources)
                    };

                    distes.Add(distGroup);
                }
                return new(distes, groups.CountItems, groups.PaginationFilter.FromProtoPagination());
            }
            return new(Array.Empty<object>(), 0, null);
        }

        /// <summary>
        /// Распаковка прото страницы
        /// </summary>
        /// <typeparam name="TDist">Целевая сущность при распаковке для маппинга</typeparam>
        /// <param name="any">Запакованная протостраница</param>
        /// <returns></returns>
        public static Page<object> FromProtoPage<TDist>(this Google.Protobuf.WellKnownTypes.Any any) where TDist : class
        {
            if (any.TryUnpack(out List list))
            {
                Type sourceType = s_assembly.GetType(list.SourceTypeName);

                List<object> protoResult = new(list.Items
                    .Select(l =>
                        typeof(Google.Protobuf.WellKnownTypes.Any)
                            .GetMethod(nameof(Google.Protobuf.WellKnownTypes.Any.Unpack))
                            .MakeGenericMethod(sourceType)
                            .Invoke(l, Array.Empty<object>()))
                    );

                List<TDist> distResult = new(s_mapper.Map<IEnumerable<TDist>>(protoResult));
                return new(distResult, list.CountItems, list.PaginationFilter.FromProtoPagination());
            }
            if (any.TryUnpack(out GroupList groups))
            {
                Type sourceType = s_assembly.GetType(groups.SourceTypeName);

                List<Grouping<TDist>> distes = new();
                foreach (Group group in groups.GroupItems)
                {
                    List<object> protoSources = new(group.Items
                        .Select(l =>
                            typeof(Google.Protobuf.WellKnownTypes.Any)
                                .GetMethod(nameof(Google.Protobuf.WellKnownTypes.Any.Unpack))
                                .MakeGenericMethod(sourceType)
                                .Invoke(l, Array.Empty<object>()))
                        );

                    Grouping<TDist> distGroup = new()
                    {
                        Key = group.GroupKey,
                        Group = new(s_mapper.Map<IEnumerable<TDist>>(protoSources))
                    };

                    distes.Add(distGroup);
                }
                return new(distes, groups.CountItems, groups.PaginationFilter.FromProtoPagination());
            }
            return new(Array.Empty<object>(), 0, null);
        }

        /// <summary>
        /// Обработка сущностей не являющихся прото файлами
        /// </summary>
        /// <typeparam name="TSource">Исходная сущность</typeparam>
        /// <param name="entityCollection">Запрос</param>
        /// <param name="paginationFilter">Пагинация</param>
        /// <param name="groupData">Группировка</param>
        /// <param name="countItems">Общее количество записей (Необходимо при вызове пагинации до вызова этого метода) </param>
        /// <param name="SearchDatas">Фильтры - работают через контейнс, к регистру не чувствительны</param>
        /// <param name="sortDatas">Сортировка</param>
        /// <param name="paginationIgnore">Не вызывать метод пагинации</param>
        /// <returns></returns>
        public static Page<object> ToPage<TSource>(this IEnumerable<TSource> protoCollect, PaginationFilter paginationFilter = null, GroupData groupData = null, IEnumerable<SearchFilter> SearchDatas = null, IEnumerable<SortFilter> sortDatas = null) where TSource : class
        {
            return protoCollect.ToPage<TSource, TSource>(paginationFilter, groupData, SearchDatas, sortDatas);
        }

        /// <summary>
        /// Обработка сущностей не являющихся прото файлами
        /// </summary>
        /// <typeparam name="TSource">Исходная сущность</typeparam>
        /// <typeparam name="TDist">Сущность для маппинга</typeparam>
        /// <param name="entityCollection">Запрос</param>
        /// <param name="paginationFilter">Пагинация</param>
        /// <param name="groupData">Группировка</param>
        /// <param name="countItems">Общее количество записей (Необходимо при вызове пагинации до вызова этого метода) </param>
        /// <param name="SearchDatas">Фильтры - работают через контейнс, к регистру не чувствительны</param>
        /// <param name="sortDatas">Сортировка</param>
        /// <param name="paginationIgnore">Не вызывать метод пагинации</param>
        /// <returns></returns>
        public static Page<object> ToPage<TSource, TDist>(this IEnumerable<TSource> protoCollect, PaginationFilter paginationFilter = null, GroupData groupData = null, IEnumerable<SearchFilter> SearchDatas = null, IEnumerable<SortFilter> sortDatas = null) where TSource : class where TDist : class
        {
            protoCollect = protoCollect.Search(SearchDatas).Sort(sortDatas);
            int countItem = protoCollect.Count();
            protoCollect = protoCollect.Paginations(paginationFilter);
            if (groupData is not null && !string.IsNullOrEmpty(groupData.GroupName))
            {
                IEnumerable<IGrouping<object, TSource>> groups = protoCollect.GroupBy(groupData);

                List<Grouping<TDist>> distes = new();
                foreach (IGrouping<object, TSource> group in groups)
                {
                    Grouping<TDist> distGroup = new();
                    distGroup.Key = group.Key.ToString();
                    distGroup.Group = new(s_mapper.Map<IEnumerable<TDist>>(group));
                    distes.Add(distGroup);
                }
                return new(distes, countItem, paginationFilter);
            }
            else
            {
                return new(typeof(TSource) == typeof(TDist) ? protoCollect : s_mapper.Map<IEnumerable<TDist>>(protoCollect), countItem, paginationFilter);
            }

        }

        /// <summary>
        /// Обработка коллекции сущностей
        /// </summary>
        /// <typeparam name="T">Тип сущности из базы</typeparam>
        /// <typeparam name="P">Тип прото контракта</typeparam>
        /// <param name="entityCollection">Запрос</param>
        /// <param name="paginationFilter">Пагинация</param>
        /// <param name="groupData">Группировка</param>
        /// <param name="countItems">Общее количество записей (Необходимо при вызове пагинации до вызова этого метода) </param>
        /// <param name="SearchDatas">Фильтры - работают через контейнс, к регистру не чувствительны</param>
        /// <param name="sortDatas">Сортировка</param>
        /// <param name="paginationIgnore">Не вызывать метод пагинации</param>
        /// <returns></returns>
        public static Google.Protobuf.WellKnownTypes.Any ToProtoPage<T, P>(this IEnumerable<T> unProtoCollect, PaginationFilter paginationFilter, GroupData groupData = null, long? countItems = null, IEnumerable<SearchFilter> SearchDatas = null, IEnumerable<SortFilter> sortDatas = null, bool paginationIgnore = false) where T : class where P : class, IMessage<P>
        {
            IEnumerable<P> protoCollect = s_mapper.Map<IEnumerable<P>>(unProtoCollect);
            return protoCollect.ToProtoPage(paginationFilter, groupData, countItems, SearchDatas, sortDatas, paginationIgnore);
        }

        /// <summary>
        /// Обработка прото коллекции
        /// </summary>
        /// <typeparam name="T">Тип прото контракта</typeparam>
        /// <param name="entityCollection">Запрос</param>
        /// <param name="paginationFilter">Пагинация</param>
        /// <param name="groupData">Группировка</param>
        /// <param name="countItems">Общее количество записей (Необходимо при вызове пагинации до вызова этого метода) </param>
        /// <param name="SearchDatas">Фильтры - работают через контейнс, к регистру не чувствительны</param>
        /// <param name="sortDatas">Сортировка</param>
        /// <param name="paginationIgnore">Не вызывать метод пагинации</param>
        /// <returns></returns>
        public static Google.Protobuf.WellKnownTypes.Any ToProtoPage<T>(this IEnumerable<T> protoCollect, PaginationFilter paginationFilter, GroupData groupData = null, long? countItems = null, IEnumerable<SearchFilter> SearchDatas = null, IEnumerable<SortFilter> sortDatas = null, bool paginationIgnore = false) where T : class, IMessage<T>
        {
            if (SearchDatas?.Any(f => f.ColumnName is not null && f.Value is not null) == true)
            {
                protoCollect = protoCollect.Search(SearchDatas);
            }

            int countItem = (int)(countItems ?? protoCollect.Count());

            if (sortDatas?.Any(f => f.ColumnName is not null && f.IsDescending is not null) == true)
            {
                protoCollect = protoCollect.Sort(sortDatas);
            }

            if (!paginationIgnore)
            {
                protoCollect = protoCollect.Paginations(paginationFilter);
            }

            return ToProtoPageFromEnumerable(protoCollect, paginationFilter, groupData, countItem);
        }

        /// <summary>
        /// Асинхронная обработка запроса от DbSet<T>
        /// </summary>
        /// <typeparam name="T">Тип сущности из базы</typeparam>
        /// <typeparam name="P">Тип прото контракта</typeparam>
        /// <param name="entityCollection">Запрос</param>
        /// <param name="paginationFilter">Пагинация</param>
        /// <param name="groupData">Группировка</param>
        /// <param name="countItems">Общее количество записей (Необходимо при вызове пагинации до вызова этого метода) </param>
        /// <param name="SearchDatas">Фильтры - работают через контейнс, к регистру не чувствительны</param>
        /// <param name="sortDatas">Сортировка</param>
        /// <param name="paginationIgnore">Не вызывать метод пагинации</param>
        /// <returns></returns>
        public static async Task<Google.Protobuf.WellKnownTypes.Any> ToProtoPageAsync<T, P>(this IQueryable<T> entityCollection, PaginationFilter paginationFilter, GroupData groupData = null, long? countItems = null, IEnumerable<SearchFilter> SearchDatas = null, IEnumerable<SortFilter> sortDatas = null, bool paginationIgnore = false) where T : class where P : class, IMessage<P>
        {
            if (SearchDatas?.Any(f => f.ColumnName is not null && f.Value is not null) == true)
            {
                entityCollection = entityCollection.Search(SearchDatas);
            }

            int countItem = (int)(countItems ?? await entityCollection.CountAsync());

            if (sortDatas?.Any(f => f.ColumnName is not null && f.IsDescending is not null) == true)
            {
                entityCollection = entityCollection.Sort(sortDatas);
            }

            if (!paginationIgnore)
            {
                entityCollection = entityCollection.Paginations(paginationFilter);
            }

            List<T> entityList = await entityCollection.ToListAsync();
            IEnumerable<P> protoCollection = s_mapper.Map<IEnumerable<P>>(entityList);

            return ToProtoPageFromEnumerable(protoCollection, paginationFilter, groupData, countItem);
        }

        private static Google.Protobuf.WellKnownTypes.Any ToProtoPageFromEnumerable<T>(IEnumerable<T> protoCollect, PaginationFilter paginationFilter, GroupData groupData, int countItem) where T : class, IMessage<T>
        {
            if (groupData is not null && !string.IsNullOrEmpty(groupData.GroupName))
            {
                IEnumerable<IGrouping<object, T>> groups = protoCollect.GroupBy(groupData);

                GroupList groupList = new()
                {
                    CountItems = countItem,
                    PaginationFilter = paginationFilter.ToProtoPagination(),
                    SourceTypeName = typeof(T).FullName
                };

                foreach (IGrouping<object, T> group in groups)
                {
                    Group protoGroup = new();
                    protoGroup.GroupKey = group.Key?.ToString();
                    protoGroup.Items.AddRange(group.Select(g => Google.Protobuf.WellKnownTypes.Any.Pack(g)));
                    groupList.GroupItems.Add(protoGroup);
                }
                return Google.Protobuf.WellKnownTypes.Any.Pack(groupList);
            }

            List list = new()
            {
                CountItems = countItem,
                Items = { protoCollect.Select(e => Google.Protobuf.WellKnownTypes.Any.Pack(e)) },
                PaginationFilter = paginationFilter.ToProtoPagination(),
                SourceTypeName = typeof(T).FullName
            };

            return Google.Protobuf.WellKnownTypes.Any.Pack(list);
        }
    }
}
