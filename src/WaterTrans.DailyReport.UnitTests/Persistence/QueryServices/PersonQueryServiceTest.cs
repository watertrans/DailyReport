using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaterTrans.DailyReport.Persistence.QueryServices;

namespace WaterTrans.DailyReport.UnitTests.Persistence.QueryServices
{
    [TestClass]
    public class PersonQueryServiceTest
    {
        [TestMethod]
        public void GetAllPerson_����_��O���������Ȃ�����()
        {
            var personQueryService = new PersonQueryService(TestEnvironment.DBSettings);
            var result = personQueryService.GetAllPerson();
        }
    }
}
