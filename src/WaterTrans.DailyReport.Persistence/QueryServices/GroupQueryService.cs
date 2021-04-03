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
                sqlWhere.AppendLine("         FROM   Tag AS TG1 ");
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
                Query = DataUtil.EscapeLike(query, LikeMatchType.PartialMatch),
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

            sql.AppendLine(" SELECT * ");
            sql.AppendLine(" FROM   ( ");
            sql.AppendLine("     SELECT * ");
            sql.AppendLine("     FROM   [Group] AS GS1 ");
            sql.AppendLine("     WHERE  1 = 1 ");
            sql.AppendLine(sqlWhere.ToString());
            sql.AppendLine(string.Format(sqlSort.ToString(), "GS1"));
            sql.AppendLine("     OFFSET (@Page - 1) * @PageSize ROWS ");
            sql.AppendLine("     FETCH FIRST @PageSize ROWS ONLY ");
            sql.AppendLine(" )   AS GS2 LEFT OUTER JOIN ");
            sql.AppendLine(" Tag AS TG2 ON GS2.GroupId = TG2.TargetId ");
            sql.AppendLine(string.Format(sqlSort.ToString(), "GS2") + " , TG2.Value ASC ");

            var groupDic = new Dictionary<Guid, Group>();
            return Connection.Query<Group, Tag, Group>(
                sql.ToString(),
                (group, tag) =>
                {
                    if (!groupDic.TryGetValue(group.GroupId, out Group groupEntry))
                    {
                        groupEntry = group;
                        groupDic.Add(groupEntry.GroupId, groupEntry);
                    }

                    if (tag != null)
                    {
                        groupEntry.Tags.Add(tag);
                    }

                    return groupEntry;
                },
                param,
                splitOn: "TagId",
                commandTimeout: DBSettings.CommandTimeout).Distinct().ToList();
        }

        /// <inheritdoc/>
        public Group GetGroup(Guid groupId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT * ");
            sql.AppendLine(" FROM   [Group] AS GS1 LEFT OUTER JOIN ");
            sql.AppendLine("        Tag    AS TG1 ON GS1.GroupId = TG1.TargetId ");
            sql.AppendLine(" WHERE  GS1.GroupId = @GroupId ");
            sql.AppendLine(" ORDER BY TG1.Value ");

            var param = new
            {
                GroupId = groupId,
            };

            var groupDic = new Dictionary<Guid, Group>();
            return Connection.Query<Group, Tag, Group>(
                sql.ToString(),
                (group, tag) =>
                {
                    if (!groupDic.TryGetValue(group.GroupId, out Group groupEntry))
                    {
                        groupEntry = group;
                        groupDic.Add(groupEntry.GroupId, groupEntry);
                    }

                    if (tag != null)
                    {
                        groupEntry.Tags.Add(tag);
                    }

                    return groupEntry;
                },
                param,
                splitOn: "TagId",
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
    }
}
