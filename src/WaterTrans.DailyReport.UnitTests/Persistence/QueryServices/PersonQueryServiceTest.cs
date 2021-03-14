using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaterTrans.DailyReport.Persistence.QueryServices;

namespace WaterTrans.DailyReport.UnitTests.Persistence.QueryServices
{
    [TestClass]
    public class PersonQueryServiceTest
    {
        [TestMethod]
        public void GetAllPerson_正常_例外が発生しないこと()
        {
            var personQueryService = new PersonQueryService(TestEnvironment.DBSettings);
            var result = personQueryService.GetAllPerson();
        }
    }
}
