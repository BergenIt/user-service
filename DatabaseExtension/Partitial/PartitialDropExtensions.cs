using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace DatabaseExtension
{
    public record DropPartitialCommand(DateTime Min, DateTime Max, params object[][] PartitialKeys);

    public static class PartitialDropExtensions
    {
        public static async Task DropPartialTable<TEntity>(this DbContext db, params DropPartitialCommand[] dropRartitialCommands)
        {
            await DropPartialTable<TEntity>(db, dropRartitialCommands.AsEnumerable());
        }

        public static async Task DropPartialTable<TEntity>(this DbContext db, IEnumerable<DropPartitialCommand> dropRartitialCommands)
        {
            List<string> tableNames = new();
            foreach (DropPartitialCommand dropRartitialCommand in dropRartitialCommands)
            {
                tableNames.AddRange(GetFullTableNames<TEntity>(dropRartitialCommand.Min, dropRartitialCommand.Max, dropRartitialCommand.PartitialKeys));
            }

            if (tableNames.Any())
            {
                string sqlDrop = $"DROP TABLE IF EXISTS PUBLIC.\"{string.Join("\" , PUBLIC.\"", tableNames)}\";";
                _ = await db.Database.ExecuteSqlRawAsync(sqlDrop);
            }
        }

        private static List<string> GetFullTableNames<TEntity>(DateTime min, DateTime max, object[][] partitialKeys)
        {
            string entityName = PartitionTableAttribute.GetTableName<TEntity>();

            List<string> tableNames = new();

            DateTime dateTimePart = min;

            while (dateTimePart < max)
            {
                string namePartital = DateTimeToName(dateTimePart);

                tableNames.Add(HashHMACSHA1($"{entityName}_{namePartital}"));
                dateTimePart = dateTimePart.AddHours(1);
            }

            if (tableNames.Any())
            {
                foreach (object[] partitialKey in partitialKeys)
                {
                    List<string> namesPartital = GetNamePartitial(partitialKey);
                    List<string> tempTableNames = new();

                    foreach (string tableName in tableNames)
                    {
                        foreach (string namePart in namesPartital)
                        {
                            string newName = HashHMACSHA1($"{tableName}_{namePart}");
                            tempTableNames.Add(newName);
                        }
                    }
                    tableNames = tempTableNames;
                }
            }

            return tableNames;

            static string DateTimeToName(DateTime dateTime) => $"{$"{dateTime:O}".Split("T").First()}T{dateTime.Hour}";
        }

        private static string HashHMACSHA1(string value)
        {
            byte[] unicodeValue = new byte[value.Length * 2];
            _ = Encoding.ASCII.GetEncoder().GetBytes(value.ToCharArray(), 0, value.Length, unicodeValue, 0, true);

            return Convert.ToBase64String(new HMACSHA1(Encoding.UTF8.GetBytes("EMS")).ComputeHash(unicodeValue));
        }

        private static List<string> GetNamePartitial(object[] values)
        {
            return values.First().GetType().IsEnum
                ? values.Select(value => $"{(int)value}").ToList()
                : values.Select(value => $"{value}").ToList();
        }
    }
}
