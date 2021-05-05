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
    /// 部署クエリーサービス
    /// </summary>
    public class GroupQueryService : SqlQueryService, IGroupQueryService
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public GroupQueryService(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }

        /// <inheritdoc/>
        public IList<Group> Query(string query, SortOrder sort, PagingQuery paging)
        {
            var sqlWhere = new StringBuilder();

            if (!string.IsNullOrEmpty(query))
            {
                sqlWhere.AppendLine(" AND ( ");
                sqlWhere.AppendLine("     GroupCode LIKE @Query OR ");
                sqlWhere.AppendLine("     GroupTree LIKE @Query OR ");
                sqlWhere.AppendLine("     Name LIKE @Query OR ");
                sqlWhere.AppendLine("     GroupId IN ( ");
                sqlWhere.AppendLine("         SELECT TargetId ");
                sqlWhere.AppendLine("         FROM   Tag AS TG1 WITH (NOLOCK) ");
                sqlWhere.AppendLine("         WHERE  TG1.TargetTable = 'Group' ");
                sqlWhere.AppendLine("         AND    TG1.Value = @TagQuery ");
                sqlWhere.AppendLine("     ) ");
                sqlWhere.AppendLine(" ) ");
            }

            var sqlSort = new StringBuilder();
            if (sort.Count == 0)
            {
                sqlSort.AppendLine(" ORDER BY {0}.SortNo ASC, {0}.GroupTree ASC ");
            }
            else
            {
                sqlSort.AppendLine(" ORDER BY ");
                foreach (var item in sort)
                {
                    if (item.Field.Equals("GroupCode", StringComparison.OrdinalIgnoreCase))
                    {
                        sqlSort.Append(" {0}.GroupCode " + item.SortType.ToString() + ",");
                    }
                    else if (item.Field.Equals("GroupTree", StringComparison.OrdinalIgnoreCase))
                    {
                        sqlSort.Append(" {0}.GroupTree " + item.SortType.ToString() + ",");
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
            sqlCount.AppendLine(" SELECT COUNT(*) FROM [Group] WHERE  1 = 1 ");
            sqlCount.AppendLine(sqlWhere.ToString());

            paging.TotalCount = (int)Connection.ExecuteScalar(
                sqlCount.ToString(),
                param,
                commandTimeout: DBSettings.CommandTimeout);

            var sql = new StringBuilder();

            sql.AppendLine(" SELECT GS2.* ");
            sql.AppendLine("      , (SELECT TG2.Value ");
            sql.AppendLine("           FROM Tag AS TG2 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG2.TargetId = GS2.GroupId ");
            sql.AppendLine("          ORDER BY TG2.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS GroupTags ");
            sql.AppendLine("      , PS1.* ");
            sql.AppendLine("      , GP1.PositionType ");
            sql.AppendLine("      , (SELECT TG3.Value ");
            sql.AppendLine("           FROM Tag AS TG3 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG3.TargetId = PS1.PersonId ");
            sql.AppendLine("          ORDER BY TG3.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS PersonTags ");
            sql.AppendLine(" FROM   ( ");
            sql.AppendLine("         SELECT * ");
            sql.AppendLine("           FROM [Group] AS GS1 ");
            sql.AppendLine("          WHERE 1 = 1 ");
            sql.AppendLine(sqlWhere.ToString());
            sql.AppendLine(string.Format(sqlSort.ToString(), "GS1"));
            sql.AppendLine("         OFFSET (@Page - 1) * @PageSize ROWS ");
            sql.AppendLine("          FETCH FIRST @PageSize ROWS ONLY ");
            sql.AppendLine("        )           AS GS2 LEFT OUTER JOIN ");
            sql.AppendLine("        GroupPerson AS GP1 ON GS2.GroupId  = GP1.GroupId LEFT OUTER JOIN ");
            sql.AppendLine("        Person      AS PS1 ON GP1.PersonId = PS1.PersonId ");
            sql.AppendLine(string.Format(sqlSort.ToString(), "GS2") + ", PS1.SortNo, PS1.PersonCode ");

            var groupDic = new Dictionary<Guid, Group>();
            return Connection.Query<Group, string, GroupPerson, string, Group>(
                sql.ToString(),
                (group, groupTags, person, personTags) =>
                {
                    if (!groupDic.TryGetValue(group.GroupId, out Group groupEntry))
                    {
                        groupEntry = group;
                        groupEntry.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(groupTags, "Value"));
                        groupDic.Add(groupEntry.GroupId, groupEntry);
                    }

                    if (person != null && !groupEntry.Persons.Exists(e => e.PersonId == person.PersonId))
                    {
                        person.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(personTags, "Value"));
                        groupEntry.Persons.Add(person);
                    }

                    return groupEntry;
                },
                param,
                splitOn: "GroupTags,PersonId,PersonTags",
                commandTimeout: DBSettings.CommandTimeout).Distinct().ToList();
        }

        /// <inheritdoc/>
        public IList<GroupPerson> QueryPerson(Guid groupId, string query, SortOrder sort, PagingQuery paging)
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
                GroupId = groupId,
                Query = DataUtil.EscapeLike(query, LikeMatchType.PartialMatch),
                TagQuery = query,
                Page = paging.Page,
                PageSize = paging.PageSize,
            };

            var sqlCount = new StringBuilder();
            sqlCount.AppendLine(" SELECT COUNT(*) ");
            sqlCount.AppendLine("   FROM GroupPerson AS GP1 INNER JOIN ");
            sqlCount.AppendLine("        Person      AS PS1 ON GP1.PersonId = PS1.PersonId ");
            sqlCount.AppendLine("  WHERE GP1.GroupId = @GroupId ");
            sqlCount.AppendLine(sqlWhere.ToString());

            paging.TotalCount = (int)Connection.ExecuteScalar(
                sqlCount.ToString(),
                param,
                commandTimeout: DBSettings.CommandTimeout);

            var sql = new StringBuilder();

            sql.AppendLine(" SELECT PS1.* ");
            sql.AppendLine("      , GP1.PositionType ");
            sql.AppendLine("      , (SELECT TG3.Value ");
            sql.AppendLine("           FROM Tag AS TG3 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG3.TargetId = PS1.PersonId ");
            sql.AppendLine("          ORDER BY TG3.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS PersonTags ");
            sql.AppendLine("   FROM GroupPerson AS GP1 INNER JOIN ");
            sql.AppendLine("        Person      AS PS1 ON GP1.PersonId = PS1.PersonId ");
            sql.AppendLine("  WHERE GP1.GroupId = @GroupId ");
            sql.AppendLine(sqlWhere.ToString());
            sql.AppendLine(string.Format(sqlSort.ToString(), "PS1"));
            sql.AppendLine(" OFFSET (@Page - 1) * @PageSize ROWS ");
            sql.AppendLine("  FETCH FIRST @PageSize ROWS ONLY ");

            return Connection.Query<GroupPerson, string, GroupPerson>(
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
        public Group GetGroup(Guid groupId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT GS1.* ");
            sql.AppendLine("      , (SELECT TG1.Value ");
            sql.AppendLine("           FROM Tag AS TG1 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG1.TargetId = GS1.GroupId ");
            sql.AppendLine("          ORDER BY TG1.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS GroupTags ");
            sql.AppendLine("      , PS1.* ");
            sql.AppendLine("      , GP1.PositionType ");
            sql.AppendLine("      , (SELECT TG2.Value ");
            sql.AppendLine("           FROM Tag AS TG2 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG2.TargetId = PS1.PersonId ");
            sql.AppendLine("          ORDER BY TG2.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS PersonTags ");
            sql.AppendLine(" FROM   [Group]     AS GS1 LEFT OUTER JOIN ");
            sql.AppendLine("        GroupPerson AS GP1 ON GS1.GroupId  = GP1.GroupId LEFT OUTER JOIN ");
            sql.AppendLine("        Person      AS PS1 ON GP1.PersonId = PS1.PersonId ");
            sql.AppendLine(" WHERE  GS1.GroupId = @GroupId ");
            sql.AppendLine(" ORDER BY PS1.SortNo, PS1.PersonCode ");

            var param = new
            {
                GroupId = groupId,
            };

            var groupDic = new Dictionary<Guid, Group>();
            return Connection.Query<Group, string, GroupPerson, string, Group>(
                sql.ToString(),
                (group, groupTags, person, personTags) =>
                {
                    if (!groupDic.TryGetValue(group.GroupId, out Group groupEntry))
                    {
                        groupEntry = group;
                        groupEntry.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(groupTags, "Value"));
                        groupDic.Add(groupEntry.GroupId, groupEntry);
                    }

                    if (person != null && !groupEntry.Persons.Exists(e => e.PersonId == person.PersonId))
                    {
                        person.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(personTags, "Value"));
                        groupEntry.Persons.Add(person);
                    }

                    return groupEntry;
                },
                param,
                splitOn: "GroupTags,PersonId,PersonTags",
                commandTimeout: DBSettings.CommandTimeout).Distinct().SingleOrDefault();
        }

        /// <inheritdoc/>
        public bool ExistsGroupCode(string groupCode)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT COUNT(*) ");
            sql.AppendLine(" FROM   [Group] ");
            sql.AppendLine(" WHERE  GroupCode = @GroupCode ");

            var param = new
            {
                GroupCode = groupCode,
            };

            return Connection.ExecuteScalar<int>(sql.ToString(), param, commandTimeout: DBSettings.CommandTimeout) > 0;
        }

        /// <inheritdoc/>
        public bool ExistsGroupTree(string groupTree)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT COUNT(*) ");
            sql.AppendLine(" FROM   [Group] ");
            sql.AppendLine(" WHERE  GroupTree = @GroupTree ");

            var param = new
            {
                GroupTree = groupTree,
            };

            return Connection.ExecuteScalar<int>(sql.ToString(), param, commandTimeout: DBSettings.CommandTimeout) > 0;
        }

        /// <inheritdoc/>
        public IList<Group> GetOrganization()
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT GS1.* ");
            sql.AppendLine("      , (SELECT TG1.Value ");
            sql.AppendLine("           FROM Tag AS TG1 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG1.TargetId = GS1.GroupId ");
            sql.AppendLine("          ORDER BY TG1.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS GroupTags ");
            sql.AppendLine("      , PS1.* ");
            sql.AppendLine("      , GP1.PositionType ");
            sql.AppendLine("      , (SELECT TG2.Value ");
            sql.AppendLine("           FROM Tag AS TG2 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG2.TargetId = PS1.PersonId ");
            sql.AppendLine("          ORDER BY TG2.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS PersonTags ");
            sql.AppendLine("   FROM [Group]     AS GS1 LEFT OUTER JOIN ");
            sql.AppendLine("        GroupPerson AS GP1 ON GS1.GroupId  = GP1.GroupId LEFT OUTER JOIN ");
            sql.AppendLine("        Person      AS PS1 ON GP1.PersonId = PS1.PersonId AND PS1.Status = @PersonStatus_NORMAL AND GP1.PositionType <> @PositionType_STAFF ");
            sql.AppendLine("  WHERE GS1.Status = @GroupStatus_NORMAL ");
            sql.AppendLine("  ORDER BY GS1.GroupTree, GP1.PositionType ");

            var param = new
            {
                GroupStatus_NORMAL = GroupStatus.NORMAL.ToString(),
                PersonStatus_NORMAL = PersonStatus.NORMAL.ToString(),
                PositionType_STAFF = PositionType.STAFF.ToString(),
            };

            var groupDic = new Dictionary<Guid, Group>();
            return Connection.Query<Group, string, GroupPerson, string, Group>(
                sql.ToString(),
                (group, groupTags, person, personTags) =>
                {
                    if (!groupDic.TryGetValue(group.GroupId, out Group groupEntry))
                    {
                        groupEntry = group;
                        groupEntry.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(groupTags, "Value"));
                        groupDic.Add(groupEntry.GroupId, groupEntry);
                    }

                    if (person != null && !groupEntry.Persons.Exists(e => e.PersonId == person.PersonId))
                    {
                        person.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(personTags, "Value"));
                        groupEntry.Persons.Add(person);
                    }

                    return groupEntry;
                },
                param,
                splitOn: "GroupTags,PersonId,PersonTags",
                commandTimeout: DBSettings.CommandTimeout).Distinct().ToList();
        }
    }
}
