using System.Collections.Generic;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// 従業員クエリーサービスインターフェース
    /// </summary>
    public interface IPersonQueryService
    {
        /// <summary>
        /// すべての従業員を取得します。
        /// </summary>
        /// <returns>IList&lt;Person&gt;</returns>
        IList<Person> GetAllPerson();
    }
}
