using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterTrans.DailyReport.Application;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Persistence.QueryServices
{
    /// <summary>
    /// 従業員クエリーサービス
    /// </summary>
    public class PersonQueryService : SqlQueryService, IPersonQueryService
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public PersonQueryService(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }

        /// <inheritdoc/>
        public IList<Person> Query(string query, SortOrder sort, PagingQuery paging)
        {
            var sqlWhere = new StringBuilder();

            if (!string.IsNullOrEmpty(query))
            {
                sqlWhere.AppendLine(" AND ( ");
                sqlWhere.AppendLine("     PersonCode LIKE @Query OR ");
                sqlWhere.AppendLine("     Name LIKE @Query OR ");
                sqlWhere.AppendLine("     Title LIKE @Query OR ");
                sqlWhere.AppendLine("     PersonId IN ( ");
                sqlWhere.AppendLine("         SELECT TargetId ");
                sqlWhere.AppendLine("         FROM   Tag AS TG1 ");
                sqlWhere.AppendLine("         WHERE  TG1.TargetTable = 'Person' ");
                sqlWhere.AppendLine("         AND    TG1.Value = @TagQuery ");
                sqlWhere.AppendLine("     ) ");
                sqlWhere.AppendLine(" ) ");
            }

            var sqlSort = new StringBuilder();
            if (sort.Count == 0)
            {
                sqlSort.AppendLine(" ORDER BY {0}.SortNo ASC, {0}.PersonCode ASC ");
            }
            else
            {
                sqlSort.AppendLine(" ORDER BY ");
                foreach (var item in sort)
                {
                    if (item.Field.Equals("PersonCode", StringComparison.OrdinalIgnoreCase))
                    {
                        sqlSort.Append(" {0}.PersonCode " + item.SortType.ToString() + ",");
                    }
                    else if (item.Field.Equals("SortNo", StringComparison.OrdinalIgnoreCase))
                    {
                        sqlSort.Append(" {0}.SortNo " + item.SortType.ToString() + ",");
                    }
                    else if (item.Field.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    {
                        sqlSort.Append(" {0}.Name " + item.SortType.ToString() + ",");
                    }
                    else if (item.Field.Equals("CreateTime", StringComparison.OrdinalIgnoreCase))
                    {
                        sqlSort.Append(" {0}.CreateTime " + item.SortType.ToString() + ",");
                    }
                }
                sqlSort.Length -= 1;
                sqlSort.AppendLine();
            }

            var param = new
            {
                Query = DataUtil.EscapeLike(query, LikeMatchType.PartialMatch),
                TagQuery = query,
                Page = paging.Page,
                PageSize = paging.PageSize,
            };

            var sqlCount = new StringBuilder();
            sqlCount.AppendLine(" SELECT COUNT(*) FROM Person WHERE  1 = 1 ");
            sqlCount.AppendLine(sqlWhere.ToString());

            paging.TotalCount = (int)Connection.ExecuteScalar(
                sqlCount.ToString(),
                param,
                commandTimeout: DBSettings.CommandTimeout);

            var sql = new StringBuilder();

            sql.AppendLine(" SELECT * ");
            sql.AppendLine(" FROM   ( ");
            sql.AppendLine("     SELECT * ");
            sql.AppendLine("     FROM   Person AS PS1 ");
            sql.AppendLine("     WHERE  1 = 1 ");
            sql.AppendLine(sqlWhere.ToString());
            sql.AppendLine(string.Format(sqlSort.ToString(), "PS1"));
            sql.AppendLine("     OFFSET (@Page - 1) * @PageSize ROWS ");
            sql.AppendLine("     FETCH FIRST @PageSize ROWS ONLY ");
            sql.AppendLine(" )   AS PS2 LEFT OUTER JOIN ");
            sql.AppendLine(" Tag AS TG2 ON PS2.PersonId = TG2.TargetId ");
            sql.AppendLine(string.Format(sqlSort.ToString(), "PS2") + " , TG2.Value ASC ");

            var personDic = new Dictionary<Guid, Person>();
            return Connection.Query<Person, Tag, Person>(
                sql.ToString(),
                (person, tag) =>
                {
                    if (!personDic.TryGetValue(person.PersonId, out Person personEntry))
                    {
                        personEntry = person;
                        personDic.Add(personEntry.PersonId, personEntry);
                    }

                    if (tag != null)
                    {
                        personEntry.Tags.Add(tag);
                    }

                    return personEntry;
                },
                param,
                splitOn: "TagId",
                commandTimeout: DBSettings.CommandTimeout).Distinct().ToList();
        }

        /// <inheritdoc/>
        public Person GetPerson(Guid personId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT * ");
            sql.AppendLine(" FROM   Person AS PS1 LEFT OUTER JOIN ");
            sql.AppendLine("        Tag    AS TG1 ON PS1.PersonId = TG1.TargetId ");
            sql.AppendLine(" WHERE  PS1.PersonId = @PersonId ");
            sql.AppendLine(" ORDER BY TG1.Value ");

            var param = new
            {
                PersonId = personId,
            };

            var personDic = new Dictionary<Guid, Person>();
            return Connection.Query<Person, Tag, Person>(
                sql.ToString(),
                (person, tag) =>
                {
                    if (!personDic.TryGetValue(person.PersonId, out Person personEntry))
                    {
                        personEntry = person;
                        personDic.Add(personEntry.PersonId, personEntry);
                    }

                    if (tag != null)
                    {
                        personEntry.Tags.Add(tag);
                    }

                    return personEntry;
                },
                param,
                splitOn: "TagId",
                commandTimeout: DBSettings.CommandTimeout).Distinct().SingleOrDefault();
        }

        /// <inheritdoc/>
        public bool ExistsPersonCode(string personCode)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT COUNT(*) ");
            sql.AppendLine(" FROM   Person ");
            sql.AppendLine(" WHERE  PersonCode = @PersonCode ");

            var param = new
            {
                PersonCode = personCode,
            };

            return Connection.ExecuteScalar<int>(sql.ToString(), param, commandTimeout: DBSettings.CommandTimeout) > 0;
        }
    }
}
