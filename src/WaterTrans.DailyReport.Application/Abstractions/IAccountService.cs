using System;
using WaterTrans.DailyReport.Application.DataTransferObjects;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// アカウントサービスインターフェース。
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// アカウント登録
        /// </summary>
        /// <param name="dto"><see cref="AccountCreateDto"/></param>
        /// <returns><see cref="Account"/></returns>
        Account CreateAccountAndPerson(AccountCreateDto dto);

        /// <summary>
        /// アカウント従業員再作成
        /// </summary>
        /// <param name="dto"><see cref="AccountCreateDto"/></param>
        /// <returns><see cref="Account"/></returns>
        Account RecreateAccountAndPerson(AccountCreateDto dto);

        /// <summary>
        /// アカウント取得
        /// </summary>
        /// <param name="accountId"><see cref="Guid"/></param>
        /// <returns><see cref="Account"/></returns>
        Account GetAccount(Guid accountId);

        /// <summary>
        /// アカウント取得
        /// </summary>
        /// <param name="accountId"><see cref="Guid"/></param>
        /// <returns>存在する場合はtrueを返します。</returns>
        bool ExistsAccount(Guid accountId);

        /// <summary>
        /// 最終ログインIDを更新します。
        /// </summary>
        /// <param name="accountId">アカウントIDを指定します。</param>
        void UpdateLastLoginTime(Guid accountId);
    }
}
