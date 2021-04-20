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
        public void Create_NotThrowsException_StateIsValid()
        {
            var now = DateUtil.Now;
            var workType = new WorkTypeTableEntity
            {
                WorkTypeId = Guid.NewGuid(),
                WorkTypeCode = new string('X', 20),
                WorkTypeTree = new string('0', 8),
                Name = new string('X', 256),
                Description = new string('X', 1024),
                SortNo = int.MaxValue,
                Status = WorkTypeStatus.NORMAL.ToString(),
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
            };
            var workTypeRepository = new WorkTypeRepository(TestEnvironment.DBSettings);
            workTypeRepository.Create(workType);
        }

        [TestMethod]
        public void Read_NotThrowsException_StateIsValid()
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
        public void Update_NotThrowsException_StateIsValid()
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
        public void Delete_NotThrowsException_StateIsValid()
        {
            var now = DateUtil.Now;
            var workType = new WorkTypeTableEntity
            {
                WorkTypeId = Guid.NewGuid(),
                WorkTypeCode = new string('Y', 20),
                WorkTypeTree = new string('1', 8),
                Name = new string('X', 256),
                Description = new string('X', 1024),
                SortNo = int.MaxValue,
                Status = WorkTypeStatus.SUSPENDED.ToString(),
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
            };
            var workTypeRepository = new WorkTypeRepository(TestEnvironment.DBSettings);
            workTypeRepository.Create(workType);
            Assert.IsTrue(workTypeRepository.Delete(workType));
        }
    }
}
