using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Iwenli.Web
{
    /// <summary>
    /// ISkin的默认实现
    /// </summary>
    public class Skin : Config, ISkin
    {
        #region 皮肤基本属性

        string _name = "Defualt";
        string _imagePath = "/Skin/Default/Images/";
        string _cssPath = "/Skin/Default/Css/";
        string _jsPath = "/Skin/Default/Js/";
        string _templatePath = "/Skin/Default/";
        Dictionary<string, string> _templateList = new Dictionary<string, string>();
        /// <summary>
        /// 皮肤名称
        /// </summary>
        public string SkinName
        {
            get
            {

                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// 皮肤图片路径
        /// </summary>
        public string ImagePath
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ImagesPath"]))
                {
                    return ConfigurationManager.AppSettings["ImagesPath"];
                }
                return _imagePath;
            }
            set
            {
                _imagePath = value;
            }
        }

        /// <summary>
        /// 皮肤样式路径
        /// </summary>
        public string CssPath
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["StylePath"]))
                {
                    return ConfigurationManager.AppSettings["StylePath"];
                }
                return _cssPath;
            }
            set
            {
                _cssPath = value;
            }
        }

        /// <summary>
        /// 皮肤脚本路径
        /// </summary>
        public string JsPath
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["JsPath"]))
                {
                    return ConfigurationManager.AppSettings["JsPath"];
                }
                return _jsPath;
            }
            set
            {
                _jsPath = value;
            }
        }

        /// <summary>
        /// 皮肤模板路径
        /// </summary>
        public string TemplatePath
        {
            get
            {
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["TemplatePath"]))
                {
                    return System.Configuration.ConfigurationManager.AppSettings["TemplatePath"];
                }
                return _templatePath;
            }
            set
            {
                _templatePath = value;
            }
        }

        #endregion

        #region 加载配置信息

        public override void Load(System.Xml.XmlElement node)
        {
            base.Load(node);
            foreach (XmlNode item in node)
            {
                if (item is XmlElement)
                {
                    if (item.Name == "Name")
                    {
                        _name = item.InnerText;
                    }
                    else if (item.Name == "ImagePath")
                    {
                        _imagePath = item.InnerText;
                    }
                    else if (item.Name == "CssPath")
                    {
                        _cssPath = item.InnerText;
                    }
                    else if (item.Name == "JsPath")
                    {
                        _jsPath = item.InnerText;
                    }
                    else if (item.Name == "TemplatePath")
                    {
                        _templatePath = item.InnerText;
                    }
                    else if (item.Name == "TemplateList")
                    {
                        foreach (XmlNode nodes in item.ChildNodes)
                        {
                            if (!_templateList.ContainsKey(nodes.Attributes["key"].Value))
                            {
                                _templateList.Add(nodes.Attributes["key"].Value, nodes.Attributes["value"].Value);
                                if (!_templateList.ContainsKey(nodes.Attributes["key"].Value.ToLower()))
                                {
                                    _templateList.Add(nodes.Attributes["key"].Value.ToLower(), nodes.Attributes["value"].Value);//保存一份小写的模板配置数据，用于匹配模板
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 获取模版保存路径

        /// <summary>
        /// 获得模板保存路径
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual string GetTemplateSavePath(string name)
        {
            //string s = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            string _templatePath = name;
            if (!_templateList.TryGetValue(name, out _templatePath))
            {//大写或模板代码不成功
                if (!_templateList.TryGetValue(name.ToLower(), out _templatePath))
                {//小写或模板代码不成功
                    _templatePath = name;
                }
            }
            //配置路径
            //if (_templateConfigIndex[name] != null)
            //{
            //    _templatePath = ((TemplateInfo)_templateConfigIndex[name]).Path;
            //}
            //网络路径
            if (_templatePath.IndexOf("\\\\") == 0)
            {
                if (System.IO.File.Exists(_templatePath))
                {
                    return _templatePath;
                }
            }
            //路径加载
            if (_templatePath.IndexOf(":") < 0)
            {//相对路径
                //获得皮肤路径
                string _skinPath = WebUtility.GetMapPath(TemplatePath + _templatePath, true);
                //获得站点路径
                string _sitePath = WebUtility.GetMapPath(_templatePath, true);

                _templatePath = _sitePath;
                if (System.IO.File.Exists(_skinPath))
                {//优先皮肤路径
                    _templatePath = _skinPath;
                }
                //else
                //{
                //    if (_skinPath.LastIndexOf(".") < _skinPath.LastIndexOf("/"))
                //    {//补全模板后缀名
                //        _templatePath = _skinPath + ".htm";
                //    }
                //}
            }
            //决对路径
            return _templatePath;
        }

        #endregion

        #region 获取模板代码

        /// <summary>
        /// 获取模板代码，如果没有找到，将抛出异常
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public string GetTemplate(string templateName)
        {
            string _templatePath = GetTemplateSavePath(templateName);
            if (!System.IO.File.Exists(_templatePath))
            {
                throw new Exception("所有区域都没找到模板[" + templateName + "][" + HttpContext.Current.Request.Url.ToString() + "]");
                //return "模板错误";
            }
            return System.IO.File.ReadAllText(_templatePath);
        }

        /// <summary>
        /// 尝试获取模板代码
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool TryGetTemplate(string templateName, out string code)
        {
            string _templatePath = GetTemplateSavePath(templateName);
            if (!System.IO.File.Exists(_templatePath))
            {
                code = string.Empty;
                return false;
            }
            else
            {
                code = System.IO.File.ReadAllText(_templatePath);
                return true;
            }
        }

        #endregion

        public void PullOnSkin(ref string htmlStr)
        {
            //throw new NotImplementedException();
        }

        public bool IsThis(HttpContext context)
        {
            return true;
        }


        #region ISkin 成员

        public Dictionary<string, string> TemplateList
        {
            get
            {
                return this._templateList;
            }
            set
            {
                this._templateList = value;
            }
        }

        #endregion
    }
}
