namespace WaterTrans.DailyReport.Domain.Constants
{
    /// <summary>
    /// 配属タイプ
    /// </summary>
    public enum PositionType
    {
        /// <summary>
        /// ゼネラルマネージャー（マネージャーとスタッフを管理できる）
        /// </summary>
        GENERAL_MANAGER,

        /// <summary>
        /// マネージャー（スタッフを管理できる）
        /// </summary>
        MANAGER,

        /// <summary>
        /// スタッフ
        /// </summary>
        STAFF,
    }
}
