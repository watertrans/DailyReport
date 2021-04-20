using System;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// アカウントクエリーサービスインターフェース
    /// </summary>
    public interface IAccountQueryService
    {
        /// <summary>
        /// 最終ログインIDを更新します。
        /// </summary>
        /// <param name="accountId">アカウントIDを指定します。</param>
        void UpdateLastLoginTime(Guid accountId);
    }
}
