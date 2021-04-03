using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WaterTrans.DailyReport.Application.Utils;

namespace WaterTrans.DailyReport.UnitTests.Application.Utils
{
    [TestClass]
    public class StringUtilTest
    {
        [TestMethod]
        public void Base64UrlEncode_True_変換結果が一致すること()
        {
            byte[] original = Guid.NewGuid().ToByteArray();
            string encoded = StringUtil.Base64UrlEncode(original);
            byte[] decoded = StringUtil.Base64UrlDecode(encoded);
            Assert.AreEqual(original.ToString(), decoded.ToString());
        }
    }
}

