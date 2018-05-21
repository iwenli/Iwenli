using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iwenli.Extension;
#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：DateTimeExtensionTests
 *  所属项目：IwenliTests.Extension
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/21 16:09:36
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
    public class DateTimeExtensionTests
    {
        [TestMethod()]
        public void ToUnixTicksTest()
        {
            Assert.AreEqual(1526890288, new DateTime(2018, 5, 21, 16, 11, 28).ToUnixTicks());
        }

        [TestMethod()]
        public void ToJsTicksTest()
        {
            Assert.AreEqual(1526890288000, new DateTime(2018, 5, 21, 16, 11, 28).ToJsTicks());
        }

        [TestMethod()]
        public void ToDatetimeFromJsTicksTest()
        {
            Assert.AreEqual(1526890288000.ToDatetimeFromJsTicks(), new DateTime(2018, 5, 21, 16, 11, 28));
        }

        [TestMethod()]
        public void ToDatetimeFromUnixTicksTest()
        {
            Assert.AreEqual(1526890288L.ToDatetimeFromUnixTicks(), new DateTime(2018, 5, 21, 16, 11, 28));
        }
    }
}