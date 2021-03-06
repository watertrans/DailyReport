using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WaterTrans.DailyReport.Application.TableEntities;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Persistence.Repositories;

namespace WaterTrans.DailyReport.Tests.Persistence.Repositories
{
    [TestClass]
    public class GroupRepositoryTest
    {
        [TestMethod]
        public void Create_NotThrowsException_StateIsValid()
        {
            var now = DateUtil.Now;
            var group = new GroupTableEntity
            {
                GroupId = Guid.NewGuid(),
                GroupCode = new string('X', 20),
                GroupTree = new string('0', 8),
                Name = new string('X', 256),
                Description = new string('X', 1024),
                SortNo = int.MaxValue,
                Status = GroupStatus.NORMAL.ToString(),
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
            };
            var groupRepository = new GroupRepository(TestEnvironment.DBSettings);
            groupRepository.Create(group);
        }

        [TestMethod]
        public void Read_NotThrowsException_StateIsValid()
        {
            var groupKey = new GroupTableEntity
            {
                GroupId = Guid.Parse("00000000-3001-0000-0000-000000000000"),
            };
            var groupRepository = new GroupRepository(TestEnvironment.DBSettings);
            var group = groupRepository.Read(groupKey);

            Assert.IsNotNull(group);
        }

        [TestMethod]
        public void Update_NotThrowsException_StateIsValid()
        {
            var groupKey = new GroupTableEntity
            {
                GroupId = Guid.Parse("00000000-3001-0000-0000-000000000000"),
            };
            var groupRepository = new GroupRepository(TestEnvironment.DBSettings);
            var group = groupRepository.Read(groupKey);
            group.UpdateTime = DateUtil.Now;
            Assert.IsTrue(groupRepository.Update(group));
        }

        [TestMethod]
        public void Delete_NotThrowsException_StateIsValid()
        {
            var now = DateUtil.Now;
            var group = new GroupTableEntity
            {
                GroupId = Guid.NewGuid(),
                GroupCode = new string('Y', 20),
                GroupTree = new string('1', 8),
                Name = new string('X', 256),
                Description = new string('X', 1024),
                SortNo = int.MaxValue,
                Status = GroupStatus.SUSPENDED.ToString(),
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
            };
            var groupRepository = new GroupRepository(TestEnvironment.DBSettings);
            groupRepository.Create(group);
            Assert.IsTrue(groupRepository.Delete(group));
        }
    }
}
