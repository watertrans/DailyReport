using System.Collections.Generic;

namespace WaterTrans.DailyReport.Web.Api.ResponseObjects
{
    /// <summary>
    /// 組織ノード
    /// </summary>
    public class OrganizationNode
    {
        /// <summary>
        /// 部署ID
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 部署コード
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 部署階層
        /// </summary>
        public string GroupTree { get; set; }

        /// <summary>
        /// 部署名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// マネージャー
        /// </summary>
        public List<OrganizationManager> Managers { get; set; } = new List<OrganizationManager>();

        /// <summary>
        /// 子組織ノード
        /// </summary>
        public List<OrganizationNode> Children { get; set; } = new List<OrganizationNode>();
    }
}
