using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace Iwenli.Data.Entity
{
    /// <summary>
    /// 用于进行自动数据查询的属性信息
    /// </summary>
    public class DataEntitySelectAttribute : Attribute
    {
        DatabaseInfo m_selectDatabase;
        /// <summary>
        /// 查询数据库
        /// </summary>
        public DatabaseInfo SelectDatabase
        {
            get { return m_selectDatabase; }
            set { m_selectDatabase = value; }
        }

        string m_selectObject;
        /// <summary>
        /// 查询对象
        /// </summary>
        public string SelectObject
        {
            get { return m_selectObject; }
            set { m_selectObject = value; }
        }

        string[] m_selectWhere;
        /// <summary>
        /// 查询条件
        /// </summary>
        public string[] SelectWhere
        {
            get { return m_selectWhere; }
            set { m_selectWhere = value; }
        }

        #region 构造函数

        /// <summary>
        /// 数据实体操作属性
        /// </summary>
        /// <param name="name">数据库名称</param>
        /// <param name="selectObject">查询对象</param>
        public DataEntitySelectAttribute(string name, string selectObject)
        {
            m_selectDatabase = DatabaseConfig.Instance.GetDatabaseInfoByCache(name);
            m_selectObject = selectObject;
        }

        /// <summary>c v
        /// 数据实体操作属性
        /// </summary>
        /// <param name="name">数据库名称</param>
        /// <param name="selectObject">查询对象</param>
        /// <param name="selectParams">查询条件</param>
        public DataEntitySelectAttribute(string name, string selectObject, params string[] selectWhere)
        {
            m_selectDatabase = DatabaseConfig.Instance.GetDatabaseInfoByCache(name);
            m_selectObject = selectObject;
            m_selectWhere = selectWhere;
        }

        #endregion
    }
}
