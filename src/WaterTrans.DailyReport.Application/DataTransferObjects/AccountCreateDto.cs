using System;

namespace WaterTrans.DailyReport.Application.DataTransferObjects
{
    /// <summary>
    /// アカウント作成DTO
    /// </summary>
    public class AccountCreateDto
    {
        /// <summary>
        /// アカウントID
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ログインID
        /// </summary>
        public string LoginId { get; set; }
    }
}
