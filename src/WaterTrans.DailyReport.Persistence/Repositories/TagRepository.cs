using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Persistence.Repositories
{
    /// <summary>
    /// タグリポジトリ
    /// </summary>
    public class TagRepository : SqlRepository<TagTableEntity>, ITagRepository
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbSettings"><see cref="IDBSettings"/></param>
        public TagRepository(IDBSettings dbSettings)
            : base(dbSettings)
        {
        }

        /// <inheritdoc/>
        public IList<TagTableEntity> FindByTargetId(Guid targetId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT * FROM Tag ");
            sql.AppendLine(" WHERE TargetId = @TargetId ");
            sql.AppendLine(" ORDER BY Tag ");

            var param = new
            {
                TargetId = targetId,
            };

            return Connection.Query<TagTableEntity>(sql.ToString(), param, commandTimeout: DBSettings.CommandTimeout).ToList();
        }

        /// <inheritdoc/>
        public void DeleteByTargetId(Guid targetId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" DELETE FROM Tag ");
            sql.AppendLine(" WHERE TargetId = @TargetId ");

            var param = new
            {
                TargetId = targetId,
            };

            Connection.Execute(sql.ToString(), param, commandTimeout: DBSettings.CommandTimeout);
        }
    }
}
