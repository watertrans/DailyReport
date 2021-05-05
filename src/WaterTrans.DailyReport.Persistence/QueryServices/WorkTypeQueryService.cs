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
    /// 業務分類クエリーサービス
    /// </summary>
    public class WorkTypeQueryService : SqlQueryService, IWorkTypeQueryService
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public WorkTypeQueryService(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }

        /// <inheritdoc/>
        public IList<WorkType> Query(string query, SortOrder sort, PagingQuery paging)
        {
            var sqlWhere = new StringBuilder();

            if (!string.IsNullOrEmpty(query))
            {
                sqlWhere.AppendLine(" AND ( ");
                sqlWhere.AppendLine("     WorkTypeCode LIKE @Query OR ");
                sqlWhere.AppendLine("     WorkTypeTree LIKE @Query OR ");
                sqlWhere.AppendLine("     Name LIKE @Query OR ");
                sqlWhere.AppendLine("     WorkTypeId IN ( ");
                sqlWhere.AppendLine("         SELECT TargetId ");
                sqlWhere.AppendLine("         FROM   Tag AS TG1 WITH (NOLOCK) ");
                sqlWhere.AppendLine("         WHERE  TG1.TargetTable = 'WorkType' ");
                sqlWhere.AppendLine("         AND    TG1.Value = @TagQuery ");
                sqlWhere.AppendLine("     ) ");
                sqlWhere.AppendLine(" ) ");
            }

            var sqlSort = new StringBuilder();
            if (sort.Count == 0)
            {
                sqlSort.AppendLine(" ORDER BY {0}.SortNo ASC, {0}.WorkTypeTree ASC ");
            }
            else
            {
                sqlSort.AppendLine(" ORDER BY ");
                foreach (var item in sort)
                {
                    if (item.Field.Equals("WorkTypeCode", StringComparison.OrdinalIgnoreCase))
                    {
                        sqlSort.Append(" {0}.WorkTypeCode " + item.SortType.ToString() + ",");
                    }
                    else if (item.Field.Equals("WorkTypeTree", StringComparison.OrdinalIgnoreCase))
                    {
                        sqlSort.Append(" {0}.WorkTypeTree " + item.SortType.ToString() + ",");
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
            sqlCount.AppendLine(" SELECT COUNT(*) FROM WorkType WHERE  1 = 1 ");
            sqlCount.AppendLine(sqlWhere.ToString());

            paging.TotalCount = (int)Connection.ExecuteScalar(
                sqlCount.ToString(),
                param,
                commandTimeout: DBSettings.CommandTimeout);

            var sql = new StringBuilder();

            sql.AppendLine(" SELECT GS1.* ");
            sql.AppendLine("      , (SELECT TG1.Value ");
            sql.AppendLine("           FROM Tag AS TG1 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG1.TargetId = GS1.WorkTypeId ");
            sql.AppendLine("          ORDER BY TG1.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS WorkTypeTags ");
            sql.AppendLine("   FROM WorkType AS GS1 ");
            sql.AppendLine("  WHERE 1 = 1 ");
            sql.AppendLine(sqlWhere.ToString());
            sql.AppendLine(string.Format(sqlSort.ToString(), "GS1"));
            sql.AppendLine(" OFFSET (@Page - 1) * @PageSize ROWS ");
            sql.AppendLine("  FETCH FIRST @PageSize ROWS ONLY ");

            return Connection.Query<WorkType, string, WorkType>(
                sql.ToString(),
                (workType, workTypeTags) =>
                {
                    workType.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(workTypeTags, "Value"));
                    return workType;
                },
                param,
                splitOn: "WorkTypeTags",
                commandTimeout: DBSettings.CommandTimeout).ToList();
        }

        /// <inheritdoc/>
        public WorkType GetWorkType(Guid workTypeId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT GS1.* ");
            sql.AppendLine("      , (SELECT TG1.Value ");
            sql.AppendLine("           FROM Tag AS TG1 WITH (NOLOCK) ");
            sql.AppendLine("          WHERE TG1.TargetId = GS1.WorkTypeId ");
            sql.AppendLine("          ORDER BY TG1.Value ");
            sql.AppendLine("            FOR JSON PATH ");
            sql.AppendLine("        ) AS WorkTypeTags ");
            sql.AppendLine(" FROM   WorkType     AS GS1 ");
            sql.AppendLine(" WHERE  GS1.WorkTypeId = @WorkTypeId ");

            var param = new
            {
                WorkTypeId = workTypeId,
            };

            return Connection.Query<WorkType, string, WorkType>(
                sql.ToString(),
                (workType, workTypeTags) =>
                {
                    workType.Tags = JsonUtil.Deserialize<List<string>>(JsonUtil.ToRawJsonArray(workTypeTags, "Value"));
                    return workType;
                },
                param,
                splitOn: "WorkTypeTags",
                commandTimeout: DBSettings.CommandTimeout).SingleOrDefault();
        }

        /// <inheritdoc/>
        public bool ExistsWorkTypeCode(string workTypeCode)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT COUNT(*) ");
            sql.AppendLine(" FROM   WorkType ");
            sql.AppendLine(" WHERE  WorkTypeCode = @WorkTypeCode ");

            var param = new
            {
                WorkTypeCode = workTypeCode,
            };

            return Connection.ExecuteScalar<int>(sql.ToString(), param, commandTimeout: DBSettings.CommandTimeout) > 0;
        }

        /// <inheritdoc/>
        public bool ExistsWorkTypeTree(string workTypeTree)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT COUNT(*) ");
            sql.AppendLine(" FROM   WorkType ");
            sql.AppendLine(" WHERE  WorkTypeTree = @WorkTypeTree ");

            var param = new
            {
                WorkTypeTree = workTypeTree,
            };

            return Connection.ExecuteScalar<int>(sql.ToString(), param, commandTimeout: DBSettings.CommandTimeout) > 0;
        }
    }
}
