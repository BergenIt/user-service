using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

namespace DatabaseExtension
{
    public static class RowSwitchExtensions
    {
        /// <summary>
        /// Сгенерирует запрос для удаления всех данные из таблицы что не были выбраны до этого
        /// </summary>
        /// <typeparam name="TEntity">Сущность для удаления</typeparam>
        /// <param name="selectQuery">Запрос для выборки сгенерированный ранее</param>
        /// <returns></returns>
        public static string DropOther<TEntity>(this SelectIntoQuery<TEntity> selectQuery) where TEntity : class
        {
            return $"{selectQuery.Query}; {DropTable<TEntity>(selectQuery.DbContext)} {SelectFromTemp<TEntity>(selectQuery.DbContext)} {DropTemp<TEntity>(selectQuery.DbContext)}";
        }

        private static string SelectFromTemp<TEntity>(DbContext db) where TEntity : class
        {
            return $"INSERT INTO PUBLIC.\"{GetTableName<TEntity>(db)}\" SELECT { GetNames<TEntity>()} FROM PUBLIC.\"{GetTableName<TEntity>(db)}Temp\" as {typeof(TEntity).Name.ToLower()};";
        }

        /// <summary>
        /// Выборка из таблицы
        /// </summary>
        /// <typeparam name="TEntity">Объект для выборки</typeparam>
        /// <param name="dbContext">Контекст для выборки</param>
        /// <returns></returns>
        public static SelectIntoQuery<TEntity> SelectIntoTemp<TEntity>(this DbContext dbContext) where TEntity : class
        {
            return new($"SELECT {GetNames<TEntity>()} INTO PUBLIC.\"{GetTableName<TEntity>(dbContext)}Temp\" FROM PUBLIC.\"{GetTableName<TEntity>(dbContext)}\" AS {typeof(TEntity).Name.ToLower()}", dbContext);
        }

        private static string GetNames<TEntity>()
        {
            string joinName = $"\", {typeof(TEntity).Name.ToLower()}.\"";
            IEnumerable<System.Reflection.PropertyInfo> enumerableProp = typeof(TEntity).GetProperties().Where(p => p.SetMethod != null && (p.PropertyType.IsValueType || p.PropertyType.Equals(typeof(string))));

            List<string> columnNames = new();
            columnNames.AddRange(enumerableProp.Where(p => p.DeclaringType != typeof(TEntity)).Select(t => t.Name));
            columnNames.AddRange(enumerableProp.Where(p => p.DeclaringType == typeof(TEntity)).Select(t => t.Name));

            return $"{typeof(TEntity).Name.ToLower()}.\"" + string.Join(joinName, columnNames) + "\"";
        }
        /// <summary>
        /// Джоин
        /// </summary>
        /// <typeparam name="TEntity">Базовая сущность</typeparam>
        /// <typeparam name="TProperty">Включаемая сущность</typeparam>
        /// <param name="queryInclude">Ранее сгенерированный запрос</param>
        /// <param name="_">Функция с указанием сущности для выборки</param>
        /// <returns></returns>
        public static IncludeQuery<TEntity, TProperty> Include<TEntity, TProperty>(this SelectIntoQuery<TEntity> queryInclude, Expression<Func<TEntity, TProperty>> _) where TProperty : class
        {
            return new($"{queryInclude.Query} INNER JOIN PUBLIC.\"{GetTableName<TProperty>(queryInclude.DbContext)}\" AS {typeof(TProperty).Name.ToLower()} ON {typeof(TEntity).Name.ToLower()}.\"{typeof(TProperty).Name}Id\" = {typeof(TProperty).Name.ToLower()}.\"Id\" ", queryInclude.DbContext);
        }

        /// <summary>
        /// Джоин ранее включенной сущности
        /// </summary>
        /// <typeparam name="TEntity">Базовая сущность</typeparam>
        /// <typeparam name="TProperty">Включаемая сущность</typeparam>
        /// <param name="queryInclude">Ранее сгенерированный запрос</param>
        /// <param name="_">Функция с указанием сущности для выборки</param>
        /// <returns></returns>
        public static IncludeQuery<TEntity, TIncludable> ThenInclude<TEntity, TProperty, TIncludable>(this IncludeQuery<TEntity, TProperty> queryInclude, Expression<Func<TProperty, TIncludable>> _) where TIncludable : class
        {
            return new($"{queryInclude.Query} INNER JOIN PUBLIC.\"{GetTableName<TIncludable>(queryInclude.DbContext)}\" AS {typeof(TIncludable).Name.ToLower()} ON {typeof(TProperty).Name.ToLower()}.\"{typeof(TIncludable).Name}Id\" = {typeof(TIncludable).Name.ToLower()}.\"Id\" ", queryInclude.DbContext);
        }

        private static string DropTable<TEntity>(DbContext db) where TEntity : class
        {
            return $"TRUNCATE TABLE PUBLIC.\"{GetTableName<TEntity>(db)}\";";
        }

        private static string DropTemp<TEntity>(DbContext db) where TEntity : class
        {
            return $"DROP TABLE PUBLIC.\"{GetTableName<TEntity>(db)}Temp\";";
        }

        private static string GetTableName<TEntity>(DbContext dbContext) where TEntity : class
        {
            return dbContext.GetType().GetProperties().FirstOrDefault(p => p.PropertyType.Equals(typeof(DbSet<TEntity>))).Name;
        }

        /// <summary>
        /// Условие для выборки (Связь со следующим Where через оператор OR)
        /// </summary>
        /// <typeparam name="TEntity">Сущность для выборки</typeparam>
        /// <param name="includeQuery">Ранее сгенерированный запрос</param>
        /// <param name="navigationPropertyPath">Булевая функция для выборки</param>
        /// <returns></returns>
        public static WhereQuery<TEntity> WhereOr<TEntity>(this WhereQuery<TEntity> includeQuery, Expression<Func<TEntity, bool>> navigationPropertyPath)
        {
            return new(WhereEnum.OR, $"{includeQuery.Query} {WhereExpressionStatic.WhereExpression(navigationPropertyPath).Replace("WHERE", includeQuery.WhereEnum.ToString())}", includeQuery.DbContext);
        }

        /// <summary>
        /// Условие для выборки (Связь со следующим Where через оператор OR)
        /// </summary>
        /// <typeparam name="TEntity">Сущность для выборки</typeparam>
        /// <param name="includeQuery">Ранее сгенерированный запрос</param>
        /// <param name="navigationPropertyPath">Булевая функция для выборки</param>
        /// <returns></returns>
        public static WhereQuery<TEntity> WhereAnd<TEntity>(this WhereQuery<TEntity> includeQuery, Expression<Func<TEntity, bool>> navigationPropertyPath)
        {
            return new(WhereEnum.AND, $"{includeQuery.Query} {WhereExpressionStatic.WhereExpression(navigationPropertyPath).Replace("WHERE", includeQuery.WhereEnum.ToString())}", includeQuery.DbContext);
        }

        /// <summary>
        /// Условие для выборки (Связь со следующим Where через оператор OR)
        /// </summary>
        /// <typeparam name="TEntity">Сущность для выборки</typeparam>
        /// <param name="includeQuery">Ранее сгенерированный запрос</param>
        /// <param name="navigationPropertyPath">Булевая функция для выборки</param>
        /// <returns></returns>
        public static WhereQuery<TEntity> WhereOr<TEntity>(this SelectIntoQuery<TEntity> includeQuery, Expression<Func<TEntity, bool>> navigationPropertyPath)
        {
            return new(WhereEnum.OR, $"{includeQuery.Query} {WhereExpressionStatic.WhereExpression(navigationPropertyPath)}", includeQuery.DbContext);
        }

        /// <summary>
        /// Условие для выборки (Связь со следующим Where через оператор OR)
        /// </summary>
        /// <typeparam name="TEntity">Сущность для выборки</typeparam>
        /// <param name="includeQuery">Ранее сгенерированный запрос</param>
        /// <param name="navigationPropertyPath">Булевая функция для выборки</param>
        /// <returns></returns>
        public static WhereQuery<TEntity> WhereAnd<TEntity>(this SelectIntoQuery<TEntity> includeQuery, Expression<Func<TEntity, bool>> navigationPropertyPath)
        {
            return new(WhereEnum.AND, $"{includeQuery.Query} {WhereExpressionStatic.WhereExpression(navigationPropertyPath)}", includeQuery.DbContext);
        }

        internal static class WhereExpressionStatic
        {
            public static readonly string[] operators = new string[] { "==", "!=", ">", "<", ">=", "<=" };
            public static string WhereExpression<TEntity>(Expression<Func<TEntity, bool>> navigationPropertyPath)
            {
                Expression left = (navigationPropertyPath.Body as BinaryExpression).Left;
                Expression right = (navigationPropertyPath.Body as BinaryExpression).Right;

                Expression valueExpression = !right.GetType().FullName.Contains("PropertyExpression") ? right : left;
                Expression bodyExpression = left.GetType().FullName.Contains("PropertyExpression") ? left : right;
                if (valueExpression == bodyExpression)
                {
                    bodyExpression = left;
                }

                if (bodyExpression.NodeType == ExpressionType.Convert)
                {
                    bodyExpression = (bodyExpression as UnaryExpression).Operand;
                }

                object value = Expression.Lambda(valueExpression).Compile().DynamicInvoke();

                string @operator = operators.FirstOrDefault(o => navigationPropertyPath.Body.ToString().Contains(o));
                return typeof(WhereExpressionStatic).GetMethod(nameof(Where)).MakeGenericMethod(new Type[] { typeof(TEntity), value.GetType() }).Invoke(null, new object[] { bodyExpression.ToString(), value, @operator }) as string;
            }
            public static string Where<TEntity, TProperty>(string rawBody, TProperty value, string @operator)
            {
                string[] split = rawBody.Split(".").ToArray();
                split[0] = typeof(TEntity).Name.ToLower();
                string body = split[^2].ToLower() + ".\"" + split.Last() + "\"";
                string valueString = typeof(TProperty).Equals(typeof(DateTime)) || typeof(TProperty).Equals(typeof(TimeSpan)) || typeof(TProperty).Equals(typeof(DateTime?)) || typeof(TProperty).Equals(typeof(TimeSpan?)) ?
                    $"'{value:O}'::timestamp" : $"'{value}'";
                return $"WHERE {body} {@operator.Replace("==", "=")} {valueString}";
            }
        }

        public enum WhereEnum { OR, AND }
        public record SelectIntoQuery<TEntity>(string Query, DbContext DbContext);
        public record IncludeQuery<TEntity, TIncludable>(string Query, DbContext DbContext) : SelectIntoQuery<TEntity>(Query, DbContext);
        public record WhereQuery<TEntity>(WhereEnum WhereEnum, string Query, DbContext DbContext) : SelectIntoQuery<TEntity>(Query, DbContext);
    }
}
