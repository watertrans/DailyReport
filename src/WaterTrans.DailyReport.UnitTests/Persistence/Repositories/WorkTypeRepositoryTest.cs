using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WaterTrans.DailyReport.Application.TableEntities;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Persistence.Repositories;

namespace WaterTrans.DailyReport.UnitTests.Persistence.Repositories
{
    [TestClass]
    public class WorkTypeRepositoryTest
    {
        [TestMethod]
        public void Create_正常_例外が発生しないこと()
        {
            var now = DateUtil.Now;
            var workType = new WorkTypeTableEntity
            {
                WorkTypeId = Guid.NewGuid(),
                WorkTypeCode = new string('X', 20),
                WorkTypeTree = new string('0', 8),
                Name = new string('X', 100),
                Description = new string('X', 400),
                SortNo = int.MaxValue,
                Status = WorkTypeStatus.NORMAL.ToString(),
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
                DeleteTime = null,
            };
            var workTypeRepository = new WorkTypeRepository(TestEnvironment.DBSettings);
            workTypeRepository.Create(workType);
        }

        [TestMethod]
        public void Read_正常_例外が発生しないこと()
        {
            var workTypeKey = new WorkTypeTableEntity
            {
                WorkTypeId = Guid.Parse("00000000-4001-0000-0000-000000000000"),
            };
            var workTypeRepository = new WorkTypeRepository(TestEnvironment.DBSettings);
            var workType = workTypeRepository.Read(workTypeKey);

            Assert.IsNotNull(workType);
        }

        [TestMethod]
        public void Update_正常_例外が発生しないこと()
        {
            var workTypeKey = new WorkTypeTableEntity
            {
                WorkTypeId = Guid.Parse("00000000-4001-0000-0000-000000000000"),
            };
            var workTypeRepository = new WorkTypeRepository(TestEnvironment.DBSettings);
            var workType = workTypeRepository.Read(workTypeKey);
            workType.UpdateTime = DateUtil.Now;
            Assert.IsTrue(workTypeRepository.Update(workType));
        }

        [TestMethod]
        public void Delete_正常_例外が発生しないこと()
        {
            var now = DateUtil.Now;
            var workType = new WorkTypeTableEntity
            {
                WorkTypeId = Guid.NewGuid(),
                WorkTypeCode = new string('Y', 20),
                WorkTypeTree = new string('0', 8),
                Name = new string('X', 100),
                Description = new string('X', 400),
                SortNo = int.MaxValue,
                Status = WorkTypeStatus.DELETED.ToString(),
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
                DeleteTime = DateTimeOffset.MaxValue,
            };
            var workTypeRepository = new WorkTypeRepository(TestEnvironment.DBSettings);
            workTypeRepository.Create(workType);
            Assert.IsTrue(workTypeRepository.Delete(workType));
        }
    }
}
