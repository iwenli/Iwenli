using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Platform
{
    public class MenuInfo
    {
        public MenuType Type { get; set; }
        public string Name { get; set; }
        public string Vaule { get; set; }

        public List<MenuInfo> SubMenu { get; set; }

        public MenuInfo()
        {
            SubMenu = new List<MenuInfo>();
        }

        public MenuInfo(MenuType type, string name, string vaule)
        {
            SubMenu = new List<MenuInfo>();
            Type = type;
            Name = name;
            Vaule = vaule;
        }

        public void AddSubMenu(MenuInfo menu)
        {
            SubMenu.Add(menu);
        }

        #region 工厂

        /// <summary>
        /// 获取菜单模板配置（缓存，再实现）
        /// </summary>
        /// <param name="replyId"></param>
        /// <returns></returns>
        public static MenuInfo[] GetMenuInfoByIdFromCache(long tempId)
        {
            return GetMenuInfoByIdFromDatabase(tempId);
        }

        /// <summary>
        /// 获取回复模板配置(数据)
        /// </summary>
        /// <param name="replyId"></param>
        /// <returns></returns>
        public static MenuInfo[] GetMenuInfoByIdFromDatabase(long tempId)
        {
            List<MenuInfo> menuList = new List<MenuInfo>();
            DataTable dt;
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
            {
                helper.SpFileValue["@tempid"] = tempId;
                dt = helper.SpGetDataTable("SP_Service_ZDL_GetMenuInfo");
            }
            if (dt.Rows.Count > 0)
            {
                var mainMenus = dt.Select("parent_menu=0");
                MenuInfo menu;
                foreach (var dr in mainMenus)
                {
                    menu = new MenuInfo();
                    menu.Name = dr["menu_name"].ToString();
                    menu.Vaule = (Convert.IsDBNull(dr["menu_content"]) || dr["menu_content"].ToString().Trim() == string.Empty) ?
                        dr["menu_id"].ToString() : dr["menu_content"].ToString();
                    menu.Type = (MenuType)Convert.ToInt32(dr["menu_type"]);
                    var subMenus = dt.Select("parent_menu=" + dr["menu_id"].ToString());
                    MenuInfo sub;
                    foreach (var dc in subMenus)
                    {
                        sub = new MenuInfo();
                        sub.Name = dc["menu_name"].ToString();
                        sub.Vaule = (Convert.IsDBNull(dc["menu_content"]) || dc["menu_content"].ToString().Trim() == string.Empty) ?
                        dc["menu_id"].ToString() : dc["menu_content"].ToString();
                        sub.Type = (MenuType)Convert.ToInt32(dc["menu_type"]);
                        menu.AddSubMenu(sub);
                    }
                    menuList.Add(menu);
                }
            }

            return menuList.ToArray();
        }

        #endregion
    }

    public enum MenuType : int
    {
        /// <summary>
        /// 用户点击click类型按钮后，微信服务器会通过消息接口推送消息类型为event	
        /// 的结构给开发者（参考消息接口指南），并且带上按钮中开发者填写的key值，开发者可以通过自定义的key值与用户进行交互；
        /// </summary>
        Click = 0,
        /// <summary>
        /// 用户点击view类型按钮后，微信客户端将会打开开发者在按钮中填写的url值	（即网页链接），达到打开网页的目的，
        /// 建议与网页授权获取用户基本信息接口结合，获得用户的登入个人信息。
        /// </summary>
        View = 1
    }
}
