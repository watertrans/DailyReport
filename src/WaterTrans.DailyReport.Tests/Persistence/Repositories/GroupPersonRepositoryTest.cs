using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WaterTrans.DailyReport.Application.TableEntities;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Persistence.Repositories;

namespace WaterTrans.DailyReport.Tests.Persistence.Repositories
{
    [TestClass]
    public class GroupPersonRepositoryTest
    {
        [TestMethod]
        public void Create_NotThrowsException_StateIsValid()
        {
            var groupPerson = new GroupPersonTableEntity
            {
                GroupId = Guid.NewGuid(),
                PersonId = Guid.NewGuid(),
                PositionType = PositionType.GENERAL_MANAGER.ToString(),
            };
            var groupPersonRepository = new GroupPersonRepository(TestEnvironment.DBSettings);
            groupPersonRepository.Create(groupPerson);
        }

        [TestMethod]
        public void Read_NotThrowsException_StateIsValid()
        {
            var groupPersonKey = new GroupPersonTableEntity
            {
                GroupId = Guid.Parse("00000000-3001-0000-0000-000000000000"),
                PersonId = Guid.Parse("00000000-1001-0000-0000-000000000000"),
            };
            var groupPersonRepository = new GroupPersonRepository(TestEnvironment.DBSettings);
            var groupPerson = groupPersonRepository.Read(groupPersonKey);

            Assert.IsNotNull(groupPerson);
        }

        [TestMethod]
        public void Update_NotThrowsException_StateIsValid()
        {
            var groupPersonKey = new GroupPersonTableEntity
            {
                GroupId = Guid.Parse("00000000-3002-0000-0000-000000000000"),
                PersonId = Guid.Parse("00000000-1001-0000-0000-000000000000"),
            };
            var groupPersonRepository = new GroupPersonRepository(TestEnvironment.DBSettings);
            var groupPerson = groupPersonRepository.Read(groupPersonKey);
            groupPerson.PositionType = PositionType.GENERAL_MANAGER.ToString();
            Assert.IsTrue(groupPersonRepository.Update(groupPerson));
        }

        [TestMethod]
        public void Delete_NotThrowsException_StateIsValid()
        {
            var groupPerson = new GroupPersonTableEntity
            {
                GroupId = Guid.NewGuid(),
                PersonId = Guid.NewGuid(),
                PositionType = PositionType.GENERAL_MANAGER.ToString(),
            };
            var groupPersonRepository = new GroupPersonRepository(TestEnvironment.DBSettings);
            groupPersonRepository.Create(groupPerson);
            Assert.IsTrue(groupPersonRepository.Delete(groupPerson));
        }
    }
}
