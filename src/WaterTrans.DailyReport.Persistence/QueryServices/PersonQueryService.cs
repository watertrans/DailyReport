using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterTrans.DailyReport.Application;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.DataTransferObjects;
using WaterTrans.DailyReport.Application.Utils;
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
        public IList<Person> Query(PersonQueryDto dto)
        {
            var sqlWhere = new StringBuilder();

            if (!string.IsNullOrEmpty(dto.Query))
            {
                sqlWhere.AppendLine(" AND ( ");
                sqlWhere.AppendLine("     PersonCode LIKE @Query OR ");
                sqlWhere.AppendLine("     LoginId LIKE @Query OR ");
                sqlWhere.AppendLine("     Name LIKE @Query OR ");
                sqlWhere.AppendLine("     Title LIKE @Query OR ");
                sqlWhere.AppendLine("     PersonId IN ( ");
                sqlWhere.AppendLine("         SELECT TargetId ");
                sqlWhere.AppendLine("         FROM   Tag AS TG1 WITH (NOLOCK) ");
                sqlWhere.AppendLine("         WHERE  TG1.TargetTable = 'Person' ");
                sqlWhere.AppendLine("         AND    TG1.Value = @TagQuery ");
                sqlWhere.AppendLine("     ) ");
                sqlWhere.AppendLine(" ) ");
            }

            if (!string.IsNullOrEmpty(dto.Status))
            {
                sqlWhere.AppendLine(" AND Status = @Status ");
            }

            if (!string.IsNullOrEmpty(dto.GroupCode))
            {
                sqlWhere.AppendLine(" AND EXISTS (");
                sqlWhere.AppendLine("     SELECT PersonId ");
                sqlWhere.AppendLine("     FROM   [Group]     AS GR1 WITH (NOLOCK) INNER JOIN ");
                sqlWhere.AppendLine("            GroupPerson AS GP1 WITH (NOLOCK) ON GR1.GroupId = GP1.GroupID ");
                sqlWhere.AppendLine("     WHERE  GR1.GroupCode = @GroupCode ");
                sqlWhere.AppendLine(" ) ");
            }

            if (!string.IsNullOrEmpty(dto.ProjectCode))
            {
                sqlWhere.AppendLine(" AND EXISTS (");
                sqlWhere.AppendLine("     SELECT PersonId ");
                sqlWhere.AppendLine("     FROM   Project       AS PR1 WITH (NOLOCK) INNER JOIN ");
                sqlWhere.AppendLine("            ProjectPerson AS PP1 WITH (NOLOCK) ON PR1.ProjectId = PP1.ProjectID ");
                sqlWhere.AppendLine("     WHERE  PR1.ProjectCode = @ProjectCode ");
                sqlWhere.AppendLine(" ) ");
            }

            var sqlSort = new StringBuilder();
            if (dto.Sort.Count == 0)
            {
                sqlSort.AppendLine(" ORDER BY {0}.SortNo ASC, {0}.PersonCode ASC ");
            }
            else
            {
                sqlSort.AppendLine(" ORDER BY ");
                foreach (var item in dto.Sort)
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
                Status = dto.Status,
                GroupCode = dto.GroupCode,
                ProjectCode = dto.ProjectCode,
                Query = DataUtil.EscapeLike(dto.Query, LikeMatchType.PrefixSearch),
                TagQuery = dto.Query,
                Page = dto.Page,
                PageSize = dto.PageSize,
            };

            var sqlCount = new StringBuilder();
            sqlCount.AppendLine(" SELECT COUNT(*) FROM Person WHERE  1 = 1 ");
            sqlCount.AppendLine(sqlWhere.ToString());

            dto.TotalCount = (int)Connection.ExecuteScalar(
                sqlCount.ToString(),
                param,
                commandTimeout: DBSettings.CommandTimeout);

            var sql = new StringBuilder();

            sql.AppendLine(" SELECT PS1.* ");
            sql.AppendLine("      , (SELECT TG1.Value ");
            sql.AppendLine("           FROM Tag AS TG1 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG1.TargetId = PS1.PersonId ");
            sql.AppendLine("          ORDER BY TG1.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS PersonTags ");
            sql.AppendLine("   FROM Person AS PS1 ");
            sql.AppendLine("  WHERE 1 = 1 ");
            sql.AppendLine(sqlWhere.ToString());
            sql.AppendLine(string.Format(sqlSort.ToString(), "PS1"));
            sql.AppendLine(" OFFSET (@Page - 1) * @PageSize ROWS ");
            sql.AppendLine("  FETCH FIRST @PageSize ROWS ONLY ");

            return Connection.Query<Person, string, Person>(
                sql.ToString(),
                (person, personTags) =>
                {
                    person.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(personTags, "Value"));
                    return person;
                },
                param,
                splitOn: "PersonTags",
                commandTimeout: DBSettings.CommandTimeout).ToList();
        }

        /// <inheritdoc/>
        public Person GetPerson(Guid personId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT PS1.* ");
            sql.AppendLine("      , (SELECT TG1.Value ");
            sql.AppendLine("           FROM Tag AS TG1 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG1.TargetId = PS1.PersonId ");
            sql.AppendLine("          ORDER BY TG1.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS PersonTags ");
            sql.AppendLine(" FROM   Person AS PS1 ");
            sql.AppendLine(" WHERE  PS1.PersonId = @PersonId ");

            var param = new
            {
                PersonId = personId,
            };

            return Connection.Query<Person, string, Person>(
                sql.ToString(),
                (person, personTags) =>
                {
                    person.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(personTags, "Value"));
                    return person;
                },
                param,
                splitOn: "PersonTags",
                commandTimeout: DBSettings.CommandTimeout).SingleOrDefault();
        }

        /// <inheritdoc/>
        public Person GetPersonByLoginId(string loginId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT PS1.* ");
            sql.AppendLine("      , (SELECT TG1.Value ");
            sql.AppendLine("           FROM Tag AS TG1 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG1.TargetId = PS1.PersonId ");
            sql.AppendLine("          ORDER BY TG1.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS PersonTags ");
            sql.AppendLine(" FROM   Person AS PS1 ");
            sql.AppendLine(" WHERE  PS1.LoginId = @LoginId ");

            var param = new
            {
                LoginId = loginId,
            };

            return Connection.Query<Person, string, Person>(
                sql.ToString(),
                (person, personTags) =>
                {
                    person.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(personTags, "Value"));
                    return person;
                },
                param,
                splitOn: "PersonTags",
                commandTimeout: DBSettings.CommandTimeout).SingleOrDefault();
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

        /// <inheritdoc/>
        public bool ExistsLoginId(string loginId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT COUNT(*) ");
            sql.AppendLine(" FROM   Person ");
            sql.AppendLine(" WHERE  LoginId = @LoginId ");

            var param = new
            {
                LoginId = loginId,
            };

            return Connection.ExecuteScalar<int>(sql.ToString(), param, commandTimeout: DBSettings.CommandTimeout) > 0;
        }
    }
}
