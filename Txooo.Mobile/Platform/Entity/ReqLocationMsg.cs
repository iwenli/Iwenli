using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Platform
{
    public class ReqLocationMsg : ReqMsg
    {       
        #region 地理位置内消息

        /// <summary>
        /// 地理位置纬度 
        /// </summary>
        public string Location_X { set; get; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y { set; get; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale { set; get; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { set; get; }

        #endregion

        public ReqLocationMsg(PlatformType type)
        {
            Platform = type;
            MsgType = ReqMsgType.Location;
        }
    }
}
