using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Application.DataTransferObjects;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 従業員クエリーサービスインターフェース
    /// </summary>
    public interface IPersonQueryService
    {
        /// <summary>
        /// 従業員を検索します。
        /// </summary>
        /// <param name="dto"><see cref="PersonQueryDto"/></param>
        /// <returns>従業員の一覧を返します。</returns>
        IList<Person> Query(PersonQueryDto dto);

        /// <summary>
        /// 従業員エンティティを取得します。
        /// </summary>
        /// <param name="personId">プライマリキーを指定します。</param>
        /// <returns>エンティティを返します。存在しない場合はnullを返します。</returns>
        Person GetPerson(Guid personId);

        /// <summary>
        /// 従業員エンティティを取得します。
        /// </summary>
        /// <param name="loginId">ログインIDを指定します。</param>
        /// <returns>エンティティを返します。存在しない場合はnullを返します。</returns>
        Person GetPersonByLoginId(string loginId);

        /// <summary>
        /// 従業員コードが存在するかどうかを取得します。
        /// </summary>
        /// <param name="personCode">従業員コードを指定します。</param>
        /// <returns>存在する場合はtrueを返します。</returns>
        bool ExistsPersonCode(string personCode);

        /// <summary>
        /// ログインIDが存在するかどうかを取得します。
        /// </summary>
        /// <param name="loginId">ログインIDを指定します。</param>
        /// <returns>存在する場合はtrueを返します。</returns>
        bool ExistsLoginId(string loginId);
    }
}
