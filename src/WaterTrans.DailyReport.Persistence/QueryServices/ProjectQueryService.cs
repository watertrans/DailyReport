using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterTrans.DailyReport.Application;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Persistence.QueryServices
{
    /// <summary>
    /// プロジェクトクエリーサービス
    /// </summary>
    public class ProjectQueryService : SqlQueryService, IProjectQueryService
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public ProjectQueryService(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }

        /// <inheritdoc/>
        public IList<Project> Query(string query, SortOrder sort, PagingQuery paging)
        {
            var sqlWhere = new StringBuilder();

            if (!string.IsNullOrEmpty(query))
            {
                sqlWhere.AppendLine(" AND ( ");
                sqlWhere.AppendLine("     ProjectCode LIKE @Query OR ");
                sqlWhere.AppendLine("     Name LIKE @Query OR ");
                sqlWhere.AppendLine("     ProjectId IN ( ");
                sqlWhere.AppendLine("         SELECT TargetId ");
                sqlWhere.AppendLine("         FROM   Tag AS TG1 WITH (NOLOCK) ");
                sqlWhere.AppendLine("         WHERE  TG1.TargetTable = 'Project' ");
                sqlWhere.AppendLine("         AND    TG1.Value = @TagQuery ");
                sqlWhere.AppendLine("     ) ");
                sqlWhere.AppendLine(" ) ");
            }

            var sqlSort = new StringBuilder();
            if (sort.Count == 0)
            {
                sqlSort.AppendLine(" ORDER BY {0}.SortNo ASC, {0}.ProjectCode ASC ");
            }
            else
            {
                sqlSort.AppendLine(" ORDER BY ");
                foreach (var item in sort)
                {
                    if (item.Field.Equals("ProjectCode", StringComparison.OrdinalIgnoreCase))
                    {
                        sqlSort.Append(" {0}.ProjectCode " + item.SortType.ToString() + ",");
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
                Query = DataUtil.EscapeLike(query, LikeMatchType.PrefixSearch),
                TagQuery = query,
                Page = paging.Page,
                PageSize = paging.PageSize,
            };

            var sqlCount = new StringBuilder();
            sqlCount.AppendLine(" SELECT COUNT(*) FROM Project WHERE  1 = 1 ");
            sqlCount.AppendLine(sqlWhere.ToString());

            paging.TotalCount = (int)Connection.ExecuteScalar(
                sqlCount.ToString(),
                param,
                commandTimeout: DBSettings.CommandTimeout);

            var sql = new StringBuilder();

            sql.AppendLine(" SELECT PJ2.* ");
            sql.AppendLine("      , (SELECT TG2.Value ");
            sql.AppendLine("           FROM Tag AS TG2 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG2.TargetId = PJ2.ProjectId ");
            sql.AppendLine("          ORDER BY TG2.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS ProjectTags ");
            sql.AppendLine("      , PS1.* ");
            sql.AppendLine("      , (SELECT TG3.Value ");
            sql.AppendLine("           FROM Tag AS TG3 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG3.TargetId = PS1.PersonId ");
            sql.AppendLine("          ORDER BY TG3.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS PersonTags ");
            sql.AppendLine(" FROM   ( ");
            sql.AppendLine("         SELECT * ");
            sql.AppendLine("           FROM [Project] AS PJ1 ");
            sql.AppendLine("          WHERE 1 = 1 ");
            sql.AppendLine(sqlWhere.ToString());
            sql.AppendLine(string.Format(sqlSort.ToString(), "PJ1"));
            sql.AppendLine("         OFFSET (@Page - 1) * @PageSize ROWS ");
            sql.AppendLine("          FETCH FIRST @PageSize ROWS ONLY ");
            sql.AppendLine("        )           AS PJ2 LEFT OUTER JOIN ");
            sql.AppendLine("        ProjectPerson AS PP1 ON PJ2.ProjectId = PP1.ProjectId LEFT OUTER JOIN ");
            sql.AppendLine("        Person        AS PS1 ON PP1.PersonId  = PS1.PersonId ");
            sql.AppendLine(string.Format(sqlSort.ToString(), "PJ2") + ", PS1.SortNo, PS1.PersonCode ");

            var projectDic = new Dictionary<Guid, Project>();
            return Connection.Query<Project, string, Person, string, Project>(
                sql.ToString(),
                (project, projectTags, person, personTags) =>
                {
                    if (!projectDic.TryGetValue(project.ProjectId, out Project projectEntry))
                    {
                        projectEntry = project;
                        projectEntry.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(projectTags, "Value"));
                        projectDic.Add(projectEntry.ProjectId, projectEntry);
                    }

                    if (person != null && !projectEntry.Persons.Exists(e => e.PersonId == person.PersonId))
                    {
                        person.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(personTags, "Value"));
                        projectEntry.Persons.Add(person);
                    }

                    return projectEntry;
                },
                param,
                splitOn: "ProjectTags,PersonId,PersonTags",
                commandTimeout: DBSettings.CommandTimeout).Distinct().ToList();
        }

        /// <inheritdoc/>
        public IList<Person> QueryPerson(Guid projectId, string query, SortOrder sort, PagingQuery paging)
        {
            var sqlWhere = new StringBuilder();

            if (!string.IsNullOrEmpty(query))
            {
                sqlWhere.AppendLine(" AND ( ");
                sqlWhere.AppendLine("     PS1.PersonCode LIKE @Query OR ");
                sqlWhere.AppendLine("     PS1.Name LIKE @Query OR ");
                sqlWhere.AppendLine("     PS1.Title LIKE @Query OR ");
                sqlWhere.AppendLine("     PS1.PersonId IN ( ");
                sqlWhere.AppendLine("         SELECT TargetId ");
                sqlWhere.AppendLine("         FROM   Tag AS TG1 WITH (NOLOCK) ");
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
                ProjectId = projectId,
                Query = DataUtil.EscapeLike(query, LikeMatchType.PartialMatch),
                TagQuery = query,
                Page = paging.Page,
                PageSize = paging.PageSize,
            };

            var sqlCount = new StringBuilder();
            sqlCount.AppendLine(" SELECT COUNT(*) ");
            sqlCount.AppendLine("   FROM ProjectPerson AS PP1 INNER JOIN ");
            sqlCount.AppendLine("        Person        AS PS1 ON PP1.PersonId = PS1.PersonId ");
            sqlCount.AppendLine("  WHERE PP1.ProjectId = @ProjectId ");
            sqlCount.AppendLine(sqlWhere.ToString());

            paging.TotalCount = (int)Connection.ExecuteScalar(
                sqlCount.ToString(),
                param,
                commandTimeout: DBSettings.CommandTimeout);

            var sql = new StringBuilder();

            sql.AppendLine(" SELECT PS1.* ");
            sql.AppendLine("      , (SELECT TG3.Value ");
            sql.AppendLine("           FROM Tag AS TG3 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG3.TargetId = PS1.PersonId ");
            sql.AppendLine("          ORDER BY TG3.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS PersonTags ");
            sql.AppendLine("   FROM ProjectPerson AS PP1 INNER JOIN ");
            sql.AppendLine("        Person        AS PS1 ON PP1.PersonId = PS1.PersonId ");
            sql.AppendLine("  WHERE PP1.ProjectId = @ProjectId ");
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
        public Project GetProject(Guid projectId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT PJ1.* ");
            sql.AppendLine("      , (SELECT TG1.Value ");
            sql.AppendLine("           FROM Tag AS TG1 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG1.TargetId = PJ1.ProjectId ");
            sql.AppendLine("          ORDER BY TG1.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS ProjectTags ");
            sql.AppendLine("      , PS1.* ");
            sql.AppendLine("      , (SELECT TG2.Value ");
            sql.AppendLine("           FROM Tag AS TG2 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG2.TargetId = PS1.PersonId ");
            sql.AppendLine("          ORDER BY TG2.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS PersonTags ");
            sql.AppendLine(" FROM   Project       AS PJ1 LEFT OUTER JOIN ");
            sql.AppendLine("        ProjectPerson AS PP1 ON PJ1.ProjectId = PP1.ProjectId LEFT OUTER JOIN ");
            sql.AppendLine("        Person        AS PS1 ON PP1.PersonId  = PS1.PersonId ");
            sql.AppendLine(" WHERE  PJ1.ProjectId = @ProjectId ");
            sql.AppendLine(" ORDER BY PS1.SortNo, PS1.PersonCode ");

            var param = new
            {
                ProjectId = projectId,
            };

            var projectDic = new Dictionary<Guid, Project>();
            return Connection.Query<Project, string, Person, string, Project>(
                sql.ToString(),
                (project, projectTags, person, personTags) =>
                {
                    if (!projectDic.TryGetValue(project.ProjectId, out Project projectEntry))
                    {
                        projectEntry = project;
                        projectEntry.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(projectTags, "Value"));
                        projectDic.Add(projectEntry.ProjectId, projectEntry);
                    }

                    if (person != null && !projectEntry.Persons.Exists(e => e.PersonId == person.PersonId))
                    {
                        person.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(personTags, "Value"));
                        projectEntry.Persons.Add(person);
                    }

                    return projectEntry;
                },
                param,
                splitOn: "ProjectTags,PersonId,PersonTags",
                commandTimeout: DBSettings.CommandTimeout).Distinct().SingleOrDefault();
        }

        /// <inheritdoc/>
        public bool ExistsProjectCode(string projectCode)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT COUNT(*) ");
            sql.AppendLine(" FROM   Project ");
            sql.AppendLine(" WHERE  ProjectCode = @ProjectCode ");

            var param = new
            {
                ProjectCode = projectCode,
            };

            return Connection.ExecuteScalar<int>(sql.ToString(), param, commandTimeout: DBSettings.CommandTimeout) > 0;
        }
    }
}
