using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Txooo.Mobile.Api
{
    class BaiduMapApiHelper
    {
        static string m_ak = "D6c499f5a9c755b7979da03ea026584e";

        //http://api.map.baidu.com/direction/v1?mode=driving&origin=清华大学&destination=北京大学&origin_region=北京&destination_region=北京&output=json&ak=E4805d16520de693a3fe707cdc962045
        //说明：mode（driving）是导航模式，
        //origin（清华大学）是起点名称，            百度大厦|40.056878,116.30815
        //destination（北京大学）是终点名称，                
        //origin_region（北京）指定起始点城市，
        //destination_region（北京）指定终点城市，
        //output（json）用于指定返回数据的格式；
        //ak是开发者请求数据的标识。

        public static bool GetDirectionPath(BaiduMapApiMode mode, MapLocation origin, MapLocation destination, out string directionInfo, out List<string> directionSteps)
        {
            directionInfo = "";
            directionSteps = new List<string>();
            try
            {
                #region 请求路径数据

                string _url = "http://api.map.baidu.com/direction/v1?"
                    + "mode=" + mode.ToString()
                    + "&origin=" + origin.Label + "|" + origin.Location_X + "," + origin.Location_Y
                    + "&destination=" + destination.Label + "|" + destination.Location_X + "," + destination.Location_Y
                    + "&region=" + origin.Region
                    + "&origin_region=" + origin.Region
                    + "&destination_region=" + destination.Region
                    + "&coord_type=gcj02" //坐标类型，可选参数，默认为bd09ll。允许的值为：bd09ll（百度经纬度坐标）、bd09mc（百度摩卡托坐标）、gcj02（国测局加密坐标）、wgs84（gps设备获取的坐标）。 
                    + "&output=json&ak=" + m_ak;


                WebClient _client = new WebClient();
                string _jsonStr = _client.DownloadString(_url);

                #endregion


                LitJson.JsonData _data = LitJson.JsonMapper.ToObject(_jsonStr);
                string _status = _data["status"].ToString();
                if (_status == "0")
                {
                    #region 精确匹配
                    string _info = _data["info"].ToString();//版权信息
                    string _type = _data["type"].ToString();
                    if (_type == "1")
                    {//1：起终点是模糊的，此时给出的是选择页面


                    }
                    else if (_type == "2")
                    {//2：起终点都是明确的，直跳
                        #region 提取数据

                        LitJson.JsonData _resultData;
                        if (_data.TryGetDataByName("result", out _resultData))
                        {
                            LitJson.JsonData _routesData;
                            if (_resultData.TryGetDataByName("routes", out _routesData))
                            {
                                int _i = 0;
                                if (_routesData.IsArray)
                                {
                                    foreach (LitJson.JsonData item in _routesData)
                                    {
                                        #region 方案数据


                                        switch (item.GetJsonType())
                                        {
                                            case LitJson.JsonType.Object:
                                                {
                                                    LitJson.JsonData _stepsData;
                                                    if (mode == BaiduMapApiMode.transit)
                                                    {
                                                        #region 公交方案

                                                        LitJson.JsonData _schemeData;
                                                        if (item.TryGetDataByName("scheme", out _schemeData))
                                                        {
                                                            foreach (LitJson.JsonData scheme in _schemeData)
                                                            {
                                                                LitJson.JsonData _duration;
                                                                if (scheme.TryGetDataByName("duration", out _duration))
                                                                {
                                                                    directionSteps.Add("【方案" + (_i++) + "】：耗时" + (long.Parse(_duration.ToString()) / 60) + "分钟");
                                                                }

                                                                if (scheme.TryGetDataByName("steps", out _stepsData))
                                                                {
                                                                    foreach (LitJson.JsonData steps in _stepsData)
                                                                    {
                                                                        LitJson.JsonData _instructionsData;
                                                                        if (steps.IsArray)
                                                                        {
                                                                            foreach (LitJson.JsonData stepsitem in steps)
                                                                            {
                                                                                if (stepsitem.IsObject && stepsitem.TryGetDataByName("stepInstruction", out _instructionsData))
                                                                                {
                                                                                    //{[stepInstruction, 乘坐<b><font color="0x000000">地铁9号线(郭公庄方向)</font></b>,经过2站,到达<font color="0x000000">丰台科技园站</font>(D口出)]}
                                                                                    directionInfo += _instructionsData.ToString() + "\r\n";
                                                                                    directionSteps.Add(_instructionsData.ToString());
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region 驾车步行方案

                                                        if (item.TryGetDataByName("steps", out _stepsData))
                                                        {
                                                            foreach (LitJson.JsonData steps in _stepsData)
                                                            {
                                                                LitJson.JsonData _instructionsData;
                                                                if (steps.IsObject && steps.TryGetDataByName("instructions", out _instructionsData))
                                                                {
                                                                    directionInfo += _instructionsData.ToString() + "\r\n";
                                                                    directionSteps.Add(_instructionsData.ToString());
                                                                }
                                                            }
                                                        }

                                                        #endregion
                                                    }
                                                }
                                                break;
                                        }

                                        #endregion
                                    }
                                }                                
                            }
                        }
                        #endregion

                        return true;
                    }
                    #endregion
                }
                else if (_status == "2")
                {//参数错误
                    directionInfo = "参数错误";
                    TxLogHelper.GetLogger("BaiduApi").Info("获取路径信息错误：参数错误");
                    return false;
                }
                else if (_status == "5")
                {//5：权限或配额校验失败
                    directionInfo= "权限或配额校验失败";
                    TxLogHelper.GetLogger("BaiduApi").Info("获取路径信息错误：权限或配额校验失败");
                    return false;
                }

            }
            catch (Exception ex)
            {
                directionInfo = "应用错误：" + ex.Message;
                TxLogHelper.GetLogger("BaiduApi").Error("获取路径信息错误：" + ex.Message, ex);
            }
            return false;
        }
    }


    public struct MapLocation
    {
        /// <summary>
        /// 区域，城市
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 地理位置纬度 
        /// </summary>
        public string Location_X { set; get; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y { set; get; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { set; get; }

    }

    public enum BaiduMapApiMode
    {
        /// <summary>
        /// 驾车
        /// </summary>
        driving,
        /// <summary>
        /// 步行
        /// </summary>
        walking,
        /// <summary>
        /// 公交
        /// </summary>
        transit
    }

}
