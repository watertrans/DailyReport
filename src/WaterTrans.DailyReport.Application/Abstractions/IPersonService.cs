using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Application.DataTransferObjects;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 従業員サービスインターフェース。
    /// </summary>
    public interface IPersonService
    {
        /// <summary>
        /// 従業員検索
        /// </summary>
        /// <param name="dto"><see cref="PersonQueryDto"/></param>
        /// <returns><see cref="List&lt;Person&gt;"/></returns>
        IList<Person> QueryPerson(PersonQueryDto dto);

        /// <summary>
        /// 従業員登録
        /// </summary>
        /// <param name="dto"><see cref="PersonCreateDto"/></param>
        /// <returns><see cref="Person"/></returns>
        Person CreatePerson(PersonCreateDto dto);

        /// <summary>
        /// 従業員更新
        /// </summary>
        /// <param name="dto"><see cref="PersonUpdateDto"/></param>
        /// <returns><see cref="Person"/></returns>
        Person UpdatePerson(PersonUpdateDto dto);

        /// <summary>
        /// 従業員削除
        /// </summary>
        /// <param name="personId"><see cref="Guid"/></param>
        void DeletePerson(Guid personId);

        /// <summary>
        /// 従業員取得
        /// </summary>
        /// <param name="personId"><see cref="Guid"/></param>
        /// <returns><see cref="Person"/></returns>
        Person GetPerson(Guid personId);
    }
}
