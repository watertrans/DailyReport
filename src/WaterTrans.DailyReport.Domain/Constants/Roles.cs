namespace WaterTrans.DailyReport.Domain.Constants
{
    /// <summary>
    /// アプリケーションで扱うロール一覧
    /// </summary>
    public static class Roles
    {
        /// <summary>
        /// Owner
        /// </summary>
        /// <remarks>
        /// すべての操作を実行できます。
        /// </remarks>
        public const string Owner = "Owner";

        /// <summary>
        /// Contributor
        /// </summary>
        /// <remarks>
        /// 管理者の追加・削除、アクセス権限の付与・剥奪の操作を除いてすべての操作を実行できます。
        /// </remarks>
        public const string Contributor = "Contributor";

        /// <summary>
        /// Reader
        /// </summary>
        /// <remarks>
        /// すべての読み取り操作を実行できます。
        /// </remarks>
        public const string Reader = "Reader";

        /// <summary>
        /// UserAdministrator
        /// </summary>
        /// <remarks>
        /// 管理者の読み取り・追加・削除、アクセス権限の読み取り・付与・剥奪の操作を実行できます。
        /// </remarks>
        public const string UserAdministrator = "UserAdministrator";
    }
}
