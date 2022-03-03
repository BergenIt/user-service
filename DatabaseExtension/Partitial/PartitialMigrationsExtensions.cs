using System.Linq;

using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace DatabaseExtension
{
    public static class PartitialMigrationsExtensions
    {
        public record MakeTablePartitionalRecord(string Sql, string TableName, MigrationBuilder MigrationBuilder);
        public enum PartitionalBy { List, Range }
        public static MakeTablePartitionalRecord MakeTablePartitional(this MigrationBuilder migrationBuilder, string tableName, PartitionalBy partitionalBy, params string[] partitionKeyName)
        {
            string pkDropSql = @$"ALTER TABLE PUBLIC.""{tableName}"" DROP CONSTRAINT ""PK_{tableName}"";";
            string pkAddSql = @$"ALTER TABLE PUBLIC.""{tableName}"" ADD CONSTRAINT ""PK_{tableName}"" PRIMARY KEY (""Id"", ""{string.Join(@""", """, partitionKeyName)}"");";

            string createLikeSql = $"CREATE TABLE IF NOT EXISTS \"{tableName}_all\" (LIKE PUBLIC.\"{tableName}\" INCLUDING ALL) PARTITION BY {partitionalBy}(\"{partitionKeyName.First()}\");";

            string renameOldSql = $"ALTER TABLE \"{tableName}\" RENAME TO \"{tableName}_old\"; ";
            string renameNewSql = $"ALTER TABLE \"{tableName}_all\" RENAME TO \"{tableName}\"; ";

            return new(pkDropSql + pkAddSql + createLikeSql + renameOldSql + renameNewSql, tableName, migrationBuilder);
        }
        public static MakeTablePartitionalRecord AddForeiginKey(this MakeTablePartitionalRecord makeTablePartitional, string keyName, string columnIntoPartitional, string parentTableName, string parentColumnName, ReferentialAction onDelete)
        {
            string sqlAdd = $"ALTER TABLE \"{makeTablePartitional.TableName}\" ADD CONSTRAINT \"{keyName}\" FOREIGN KEY(\"{columnIntoPartitional}\") REFERENCES PUBLIC.\"{parentTableName}\" (\"{parentColumnName}\") ON DELETE {onDelete}; ";
            string sqlDrop = $"ALTER TABLE \"{makeTablePartitional.TableName}_old\" DROP CONSTRAINT \"{keyName}\"; ";

            return new(makeTablePartitional.Sql + sqlAdd + sqlDrop, makeTablePartitional.TableName, makeTablePartitional.MigrationBuilder);
        }
        public static OperationBuilder<SqlOperation> EndPartitionalTransaction(this MakeTablePartitionalRecord makeTablePartitional)
        {
            string sqlDrop = $"DROP TABLE PUBLIC.\"{makeTablePartitional.TableName}_old\";";

            string pkRenameSql = @$"ALTER INDEX  ""{makeTablePartitional.TableName}_all_pkey"" RENAME TO ""PK_{makeTablePartitional.TableName}"";";

            string sql = makeTablePartitional.Sql + sqlDrop + pkRenameSql;

            return makeTablePartitional.MigrationBuilder.Sql(sql);
        }

    }
}
