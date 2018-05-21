using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Org
{
    /// <summary>
    /// 用户操作日志处理
    /// </summary>
    public static class UserLogHelper
    {
        //调用方法1：this.WriteUserLog("删除了品牌");
        //调用方法2: TxUserLog.WriteUserLog("Txooo.SAAS.Brand","删除了品牌");

        /// <summary>
        /// 记录（实例调用)
        /// </summary>
        /// <param name="o">调用方对象</param>
        /// <param name="info">日志信息</param>
        /// <param name="brandID">品牌id，可不传默认0</param>              
        public static void WriteUserLog(this object o, string userNameOrId, string info, long brandID = 0, string dbName = "UserLog")
        {
            string _projectName = GetProjectName(o);

            WriteUserLog(_projectName, userNameOrId, info, brandID, dbName);
        }

        /// <summary>
        /// 获取调用方对象所在项目名称(当作表名)
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string GetProjectName(object o)
        {
            object[] attributes = o.GetType().Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attributes.Length > 0)
            {
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                return titleAttribute.Title;
            }
            return null;
        }


        /// <summary>
        /// 记录（静态调用）
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="info"></param>
        /// <param name="brandID"></param>
        public static void WriteUserLog(string projectName, string userNameOrId, string info, long brandID = 0, string dbName = "UserLog")
        {
            projectName = projectName ?? "temp";
            using (DataHelper helper = DataHelper.GetDataHelper(dbName))
            {
                try
                {
                    helper.SpFileValue["@Username"] = userNameOrId;
                    helper.SpFileValue["@IP"] = Iwenli.Web.WebUtility.GetIP();
                    helper.SpFileValue["@TableName"] = projectName;
                    helper.SpFileValue["@Info"] = info;
                    helper.SpFileValue["@ID"] = brandID;
                    helper.SpExecute("SP_Service_V1_ZYL_WriteUserLog");
                }
                catch (Exception ex)
                {
                    Iwenli.LogHelper.Logger.Error("记录用户操作日志出错:", ex);
                }
            }
        }
    }
}
