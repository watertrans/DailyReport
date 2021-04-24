using System.Data.Common;

namespace WaterTrans.DailyReport.Application.Abstractions
{
    /// <summary>
    /// アプリケーション設定インターフェース
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// アクセストークンの有効期限（秒）を取得します。
        /// </summary>
        int AccessTokenExpiresIn { get; }

        /// <summary>
        /// 認可コードの有効期限（秒）を取得します。
        /// </summary>
        int AuthorizationCodeExpiresIn { get; }
    }
}
