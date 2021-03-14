using System.Threading.Tasks;
using WaterTrans.DailyReport.Application.Exceptions;
using WaterTrans.DailyReport.Application.TableEntities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// SQLデータベースリポジトリインターフェース
    /// </summary>
    /// <typeparam name="TTableEntity">テーブルエンティティの型</typeparam>
    public interface ISqlRepository<TTableEntity>
        where TTableEntity : SqlTableEntity
    {
        /// <summary>
        /// テーブルエンティティを登録します。
        /// </summary>
        /// <param name="entity">テーブルエンティティを指定します。</param>
        /// <remarks>すでに登録済みの場合は例外が発生します。</remarks>
        /// <exception cref="DuplicateKeyException">登録済みの場合</exception>
        void Create(TTableEntity entity);

        /// <summary>
        /// テーブルエンティティを登録します。
        /// </summary>
        /// <param name="entity">テーブルエンティティを指定します。</param>
        /// <returns><see cref="Task"/></returns>
        /// <remarks>すでに登録済みの場合は例外が発生します。</remarks>
        /// <exception cref="DuplicateKeyException">登録済みの場合</exception>
        Task CreateAsync(TTableEntity entity);

        /// <summary>
        /// テーブルエンティティを取得します。
        /// </summary>
        /// <param name="entity">プライマリキーをセットしたテーブルエンティティを指定します。</param>
        /// <returns>テーブルエンティティを返します。存在しない場合はnullを返します。</returns>
        TTableEntity Read(TTableEntity entity);

        /// <summary>
        /// テーブルエンティティを取得します。
        /// </summary>
        /// <param name="entity">プライマリキーをセットしたテーブルエンティティを指定します。</param>
        /// <returns>テーブルエンティティを返します。存在しない場合はnullを返します。</returns>
        Task<TTableEntity> ReadAsync(TTableEntity entity);

        /// <summary>
        /// テーブルエンティティを更新します。
        /// </summary>
        /// <param name="entity">テーブルエンティティを指定します。</param>
        /// <returns>更新した場合はTrueを返します。</returns>
        bool Update(TTableEntity entity);

        /// <summary>
        /// テーブルエンティティを更新します。
        /// </summary>
        /// <param name="entity">テーブルエンティティを指定します。</param>
        /// <returns>更新した場合はTrueを返します。</returns>
        Task<bool> UpdateAsync(TTableEntity entity);

        /// <summary>
        /// テーブルエンティティを削除します。
        /// </summary>
        /// <param name="entity">プライマリキーをセットしたテーブルエンティティを指定します。</param>
        /// <returns>削除した場合はTrueを返します。</returns>
        bool Delete(TTableEntity entity);

        /// <summary>
        /// テーブルエンティティを削除します。
        /// </summary>
        /// <param name="entity">プライマリキーをセットしたテーブルエンティティを指定します。</param>
        /// <returns>削除した場合はTrueを返します。</returns>
        Task<bool> DeleteAsync(TTableEntity entity);
    }
}
