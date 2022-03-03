using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace DatabaseExtension
{
    public static class PartitialInsertExtensions
    {
        public static async Task InsertRangeIntoPartial<TEntity>(this DbContext db, IEnumerable<TEntity> entitys) where TEntity : class
        {
            foreach (TEntity entity in entitys)
            {
                await db.CreatePartialTable(entity);
            }

            try
            {
                await db.Set<TEntity>().AddRangeAsync(entitys);
                _ = await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                foreach (TEntity entity in entitys)
                {
                    await db.CreatePartialTable(entity);
                }

                await db.Set<TEntity>().AddRangeAsync(entitys);
                _ = await db.SaveChangesAsync();
            }
        }

        public static async Task InsertIntoPartial<TEntity>(this DbContext db, TEntity entity) where TEntity : class
        {
            try
            {
                _ = await db.AddAsync(entity);
                _ = await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                await db.CreatePartialTable(entity);

                _ = await db.AddAsync(entity);
                _ = await db.SaveChangesAsync();
            }
        }

        public static async Task CreatePartialTable<TEntity>(this DbContext db, TEntity entity)
        {
            string nameTable = PartitionTableAttribute.GetTableName<TEntity>();
            List<(PropertyInfo type, object value)> typeValues = GetPropertyParitalValues(entity);

            await CreatePartialTable(db, nameTable, typeValues);
        }

        private static async Task CreatePartialTable(DbContext db, string nameTable, List<(PropertyInfo type, object value)> typeValues)
        {
            string keyName = typeValues.First().type.Name;
            (string value, string sqlPart) = GetFromToValues(typeValues);
            string namePartial = HashHMACSHA1($"{nameTable}_{value}");
            string partitionBy = GetPartitionBy(typeValues);

            string query = $"CREATE TABLE IF NOT EXISTS PUBLIC.\"{namePartial}\" PARTITION OF PUBLIC.\"{nameTable}\" {sqlPart} {partitionBy};";
            string indexQuery = $"CREATE INDEX ON PUBLIC.\"{namePartial}\" (\"{keyName}\");";
            try
            {
                _ = await db.Database.ExecuteSqlRawAsync(query);
                _ = await db.Database.ExecuteSqlRawAsync(indexQuery);
            }
            catch { }

            if (typeValues.Any())
            {
                await CreatePartialTable(db, namePartial, typeValues);
            }
        }

        private static List<(PropertyInfo type, object value)> GetPropertyParitalValues<TEntity>(TEntity entity)
        {
            return typeof(TEntity)
                .GetProperties()
                .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(PartitionAttribute)))
                .Select(p => (p, p.GetValue(entity))).ToList();
        }

        private static (string value, string sqlPart) GetFromToValues(List<(PropertyInfo type, object value)> typeValues)
        {
            (PropertyInfo type, object value) = typeValues.FirstOrDefault();

            string values, valuesForSql;
            if (value is null)
            {
                values = "NULL";
                valuesForSql = "IN (NULL)";
            }
            else if (value.GetType() == typeof(DateTime))
            {
                DateTime dateTime = (DateTime)value;
                int hour = dateTime.Hour;
                values = $"{$"{dateTime:O}".Split("T").First()}T{hour}";
                valuesForSql = dateTime != DateTime.MaxValue ?
                    $"FROM ('{dateTime.Date.AddHours(hour):O}') TO ('{dateTime.Date.AddHours(hour + 1):O}')" :
                    $"FROM ('{dateTime.Date.AddHours(hour):O}') TO ('{DateTime.MaxValue:O}')";
            }
            else if (value.GetType().IsEnum)
            {
                values = $"{(int)value}";
                valuesForSql = $"IN ('{(int)value}')";
            }
            else
            {
                values = $"{value}";
                valuesForSql = $"IN ('{value}')";
            }

            string forValues = $"FOR VALUES {valuesForSql}";
            _ = typeValues.Remove((type, value));

            return (values, forValues);
        }

        private static string GetPartitionBy(List<(PropertyInfo type, object value)> typeValues)
        {
            if (typeValues.Any())
            {
                PropertyInfo type = typeValues.First().type;
                string by = type.PropertyType == typeof(DateTime) || type.PropertyType == typeof(DateTime?) ? "RANGE" : "LIST";
                string forValues = $"PARTITION BY {by} (\"{type.Name}\")";

                return forValues;
            }
            return string.Empty;
        }

        private static string HashHMACSHA1(string value)
        {
            byte[] unicodeValue = new byte[value.Length * 2];
            _ = Encoding.ASCII.GetEncoder().GetBytes(value.ToCharArray(), 0, value.Length, unicodeValue, 0, true);

            return Convert.ToBase64String(new HMACSHA1(Encoding.UTF8.GetBytes("EMS")).ComputeHash(unicodeValue));
        }
    }
}
