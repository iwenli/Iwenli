using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Mobile.Platform.Entity
{
    public class ResTemplateMsg:ResMsg
    {
        public ResTemplateMsg()
        {
            TemplateDatas = new List<TemplateData>();
            this.Url = string.Empty;
        }

        public ResTemplateMsg(ReqMsg reqmsg)
        {
            MsgType = ResMsgType.Template;
            CreateTime = DateTime.Now.ToUnixTicks().ToString();
            FuncFlag = "0";
            ToUserName = reqmsg.FromUserName;
            FromUserName = reqmsg.ToUserName;
            Platform = reqmsg.Platform;
            TemplateDatas = new List<TemplateData>();
        }

        /// <summary>
        /// 模板id
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// 点击模板消息跳转的连接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 模板顶部颜色
        /// </summary>
        public string TopColor { get; set; }

        /// <summary>
        /// 填充模板的数据集合
        /// </summary>
        public List<TemplateData> TemplateDatas { get; set; }

        public void AddTemplateData(string value, string attribute, string color)
        {
            TemplateData data = new TemplateData()
                {
                    Attribute =  attribute,
                    Value = value,
                    Color =  color
                };
            TemplateDatas.Add(data);
        }

        public override string GetWeixinResXml()
        {
            throw new NotImplementedException();
        }

        public override string GetWeixinResJson()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 模板数据
    /// </summary>
    public class TemplateData
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 属性值的颜色
        /// </summary>
        public string Color { get; set; }
    }
}
