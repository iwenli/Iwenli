using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile
{
    /// <summary>
    /// 用户分组信息
    /// </summary>
    public class UserGroupInfo
    {
        /// <summary>
        /// 分组ID
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string Groupname { get; set; }

        /// <summary>
        /// 分组包含用户数
        /// </summary>
        public int UserCount { get; set; }
    }
}
