using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iwenli.Extension;
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：StringExtensionTests
 *  所属项目：IwenliTests.Extension
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/21 14:16:24
 *  
 *  功能描述：
 *          1、
 * 
 *  修改标识：  
 *  修改描述：
 *  修改时间：
 *  待 完 善：
 *          1、 
----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Extension.Tests
{
    [TestClass()]
    public class StringExtensionTests
    {
        [TestMethod()]
        public void HMACSHA1Test()
        {
            Assert.AreEqual("jOYfPprr4pemrHOoOQD8jlZwnY0=", "123".HMACSHA1());
        }

        [TestMethod()]
        public void SHA1Test()
        {
            Assert.AreEqual("40BD001563085FC35165329EA1FF5C5ECBDBBEEF", "123".SHA1());
        }

    }
}